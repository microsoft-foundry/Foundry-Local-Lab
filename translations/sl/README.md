<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Delavnica Foundry Local - Gradnja AI aplikacij na napravi

Praktična delavnica za zagon jezikovnih modelov na lastnem računalniku in gradnjo inteligentnih aplikacij z [Foundry Local](https://foundrylocal.ai) in [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Kaj je Foundry Local?** Foundry Local je lahkotno izvršilno okolje, ki vam omogoča prenos, upravljanje in streženje jezikovnih modelov povsem na vaši strojni opremi. Ponuja **OpenAI-kompatibilen API**, tako da se lahko vsak pripomoček ali SDK, ki podpira OpenAI, poveže - brez potrebe po oblačnem računu.

### 🌐 Podpora za več jezikov

#### Podprto preko GitHub Action (Samodejno in vedno posodobljeno)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabščina](../ar/README.md) | [Bengalščina](../bn/README.md) | [Bolgarščina](../bg/README.md) | [Burmanski (Myanmar)](../my/README.md) | [Kitajščina (poenostavljena)](../zh-CN/README.md) | [Kitajščina (tradicionalna, Hong Kong)](../zh-HK/README.md) | [Kitajščina (tradicionalna, Macao)](../zh-MO/README.md) | [Kitajščina (tradicionalna, Tajvan)](../zh-TW/README.md) | [Hrvaščina](../hr/README.md) | [Češčina](../cs/README.md) | [Danščina](../da/README.md) | [Nizozemščina](../nl/README.md) | [Estonščina](../et/README.md) | [Finščina](../fi/README.md) | [Francoščina](../fr/README.md) | [Nemščina](../de/README.md) | [Grščina](../el/README.md) | [Hebrejščina](../he/README.md) | [Hindijščina](../hi/README.md) | [Madžarščina](../hu/README.md) | [Indonezijščina](../id/README.md) | [Italijanščina](../it/README.md) | [Japonščina](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korejščina](../ko/README.md) | [Litovščina](../lt/README.md) | [Malezijščina](../ms/README.md) | [Malajalščina](../ml/README.md) | [Maratščina](../mr/README.md) | [Nepalščina](../ne/README.md) | [Nigerijski pidžin](../pcm/README.md) | [Norveščina](../no/README.md) | [Perzijščina (Farsi)](../fa/README.md) | [Poljščina](../pl/README.md) | [Portugalsščina (Brazilija)](../pt-BR/README.md) | [Portugalsščina (Portugalska)](../pt-PT/README.md) | [Pandžabščina (Gurmukhi)](../pa/README.md) | [Romunščina](../ro/README.md) | [Ruščina](../ru/README.md) | [Srbščina (cirilica)](../sr/README.md) | [Slovaščina](../sk/README.md) | [Slovenščina](./README.md) | [Španščina](../es/README.md) | [Svahili](../sw/README.md) | [Švedščina](../sv/README.md) | [Tagalog (Filipinski)](../tl/README.md) | [Tamilščina](../ta/README.md) | [Telugu](../te/README.md) | [Tajščina](../th/README.md) | [Turščina](../tr/README.md) | [Ukrajinščina](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamščina](../vi/README.md)

> **Raje kopirate lokalno?**
>
> Ta repozitorij vključuje več kot 50 prevodov jezikov, kar znatno poveča velikost prenosa. Če želite klonirati brez prevodov, uporabite sparse checkout:
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
> Tako dobite vse, kar potrebujete za zaključek tečaja z veliko hitrejšim prenosom.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Cilji učenja

Do konca te delavnice boste znali:

| # | Cilj |
|---|-----------|
| 1 | Namestiti Foundry Local in upravljati modele s CLI |
| 2 | Obvladati Foundry Local SDK API za programatsko upravljanje modelov |
| 3 | Povezati se z lokalnim strežnikom za inferenco preko SDK v Pythonu, JavaScriptu in C# |
| 4 | Zgraditi Retrieval-Augmented Generation (RAG) pipeline, ki temelji na vaših podatkih |
| 5 | Ustvariti AI agente z vztrajnimi navodili in osebnostmi |
| 6 | Orkestrirati večagentne delovne tokove z zankami povratnih informacij |
| 7 | Raziščite produkcijsko aplikacijo - Zava Creative Writer |
| 8 | Zgraditi evalvacijske okvire z zlatimi nizi podatkov in LLM kot sodnikom |
| 9 | Prepisati zvok z Whisper - pretvorba govora v besedilo na napravi z Foundry Local SDK |
| 10 | Sestaviti in izvajati lastne ali Hugging Face modele z ONNX Runtime GenAI in Foundry Local |
| 11 | Omogočiti lokalnim modelom klicanje zunanjih funkcij s shemo za klic orodij |
| 12 | Zgraditi spletni uporabniški vmesnik za Zava Creative Writer s pretakanjem v realnem času |

---

## Zahteve

| Zahteva | Podrobnosti |
|-------------|---------|
| **Strojna oprema** | Najmanj 8 GB RAM (priporočeno 16 GB); CPU z AVX2 podprtostjo ali podprta GPU |
| **Operacijski sistem** | Windows 10/11 (x64/ARM), Windows Server 2025 ali macOS 13+ |
| **Foundry Local CLI** | Namestite z `winget install Microsoft.FoundryLocal` (Windows) ali `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Podrobnosti si oglejte v [vodniku za začetek](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Jezikovno izvršilno okolje** | **Python 3.9+** in/ali **.NET 9.0+** in/ali **Node.js 18+** |
| **Git** | Za kloniranje tega repozitorija |

---

## Začetek

```bash
# 1. Klonirajte repozitorij
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Preverite, ali je Foundry Local nameščen
foundry model list              # Naštejte razpoložljive modele
foundry model run phi-3.5-mini  # Zaženite interaktivni klepet

# 3. Izberite svoj jezikovni program (glejte Laboratorij del 2 za celotno nastavitev)
```

| Jezik | Hitri začetek |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Deli delavnice

### Del 1: Začetek z Foundry Local

**Vodič za laboratorij:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Kaj je Foundry Local in kako deluje
- Namestitev CLI na Windows in macOS
- Raziščite modele - seznam, prenos, zagon
- Razumevanje aliasov modelov in dinamičnih pristanišč

---

### Del 2: Globoko v Foundry Local SDK

**Vodič za laboratorij:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Zakaj uporabljati SDK namesto CLI za razvoj aplikacij
- Celovit SDK API za Python, JavaScript in C#
- Upravljanje storitev, brskanje po katalogu, življenjski cikel modela (prenosi, nalaganja, razkladanja)
- Vzorec hitrega zagona: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metapodatki, aliasi in izbira optimalnih modelov za strojno opremo

---

### Del 3: SDK-ji in API-ji

**Vodič za laboratorij:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Povezovanje s Foundry Local iz Pythona, JavaScripta in C#
- Uporaba Foundry Local SDK za programatsko upravljanje storitve
- Pretakanje pogovorov prek OpenAI-kompatibilnega API-ja
- Referenca metod SDK za vsak jezik

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Osnovni streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat z .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat z Node.js |

---

### Del 4: Retrieval-Augmented Generation (RAG)

**Vodič za laboratorij:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Kaj je RAG in zakaj je pomembno
- Gradnja baze znanja v pomnilniku
- Iskanje s prekrivanjem ključnih besed in ocenjevanje
- Sestavljanje oporiščnih sistemskih pozivov
- Zagon celotnega RAG pipeline na napravi

**Primeri kode:**

| Jezik | Datoteka |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Del 5: Gradnja AI agentov

**Vodič za laboratorij:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Kaj je AI agent (v primerjavi s čistim klicem LLM)
- Vzorec `ChatAgent` in Microsoft Agent Framework
- Sistemska navodila, osebnosti in večvračilni pogovori
- Strukturiran izhod (JSON) agentov

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Posamezni agent z Agent Framework |
| C# | `csharp/SingleAgent.cs` | Posamezni agent (vzorec ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Posamezni agent (vzorec ChatAgent) |

---

### Del 6: Večagentni delovni tokovi

**Vodič za laboratorij:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Večagentni pipelines: Raziskovalec → Pisec → Urednik
- Zaporedna orkestracija in zanke povratnih informacij
- Skupna konfiguracija in strukturirani prenos nalog
- Oblikovanje lastnega večagentnega delovnega toka

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipelin s tremi agenti |
| C# | `csharp/MultiAgent.cs` | Pipelin s tremi agenti |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipelin s tremi agenti |

---

### Del 7: Zava Creative Writer - Zaključna aplikacija

**Vodič za laboratorij:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkcijska večagentna aplikacija s 4 specializiranimi agenti
- Zaporedni pipeline z evalvacijskimi zankami povratnih informacij
- Pretakan izhod, iskanje po katalogu produktov, strukturiran JSON prenos nalog
- Polna implementacija v Pythonu (FastAPI), JavaScriptu (Node.js CLI) in C# (.NET konzola)

**Primeri kode:**

| Jezik | Mapa | Opis |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI spletna storitev z orkestratorjem |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI aplikacija |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzolna aplikacija |

---

### Del 8: Evalvacijsko vodeni razvoj

**Vodič za laboratorij:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Zgradite sistematični evalvacijski okvir za AI agente z zlatimi nizi podatkov
- Pravilo-osnovane kontrole (dolžina, pokritost ključnih besed, prepovedani izrazi) + LLM kot sodnik
- Primerjava različic pozivov z agregiranimi ocenjevalnimi lističi
- Razširja vzorec Zava Editor agent iz dela 7 v offline testno zbirko
- Veji za Python, JavaScript in C#

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evalvacijski okvir |
| C# | `csharp/AgentEvaluation.cs` | Evalvacijski okvir |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evalvacijski okvir |

---

### Del 9: Prepis govora z Whisper

**Vodič za laboratorij:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Prepis govora v besedilo z uporabo OpenAI Whisper, ki teče lokalno
- Obdelava zvoka z mislijo na zasebnost – zvok nikoli ne zapusti vaše naprave
- Podpora za Python, JavaScript in C# z `client.audio.transcriptions.create()` (Python/JS) in `AudioClient.TranscribeAudioAsync()` (C#)
- Vključuje zvočne posnetke s temo Zava za praktično vadbo

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Prepis govora Whisper |
| C# | `csharp/WhisperTranscription.cs` | Prepis govora Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Prepis govora Whisper |

> **Opomba:** Ta delavnica uporablja **Foundry Local SDK** za programsko prenos in nalaganje modela Whisper, nato pa pošilja zvok na lokalno OpenAI združljivo končno točko za prepis. Model Whisper (`whisper`) je naveden v katalogu Foundry Local in teče v celoti na napravi - niso potrebni API ključi v oblaku ali dostop do omrežja.

---

### Del 10: Uporaba lastnih ali Hugging Face modelov

**Vodnik delavnice:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Prevajanje Hugging Face modelov v optimizirano obliko ONNX z graditeljem modelov ONNX Runtime GenAI
- Strojno-specifično prevajanje (CPU, NVIDIA GPU, DirectML, WebGPU) in kvantizacija (int4, fp16, bf16)
- Ustvarjanje konfiguracijskih datotek s predlogami klepeta za Foundry Local
- Dodajanje prevedenih modelov v predpomnilnik Foundry Local
- Zaganjanje lastnih modelov prek CLI, REST API in OpenAI SDK
- Referenčni primer: celoten postopek prevajanja Qwen/Qwen3-0.6B

---

### Del 11: Klicanje orodij z lokalnimi modeli

**Vodnik delavnice:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Omogočanje lokalnim modelom klicanje zunanjih funkcij (klicanje orodij/funkcij)
- Določanje shem orodij z uporabo formata klicanja funkcij OpenAI
- Upravljanje poteka večkratnega klicanja orodij v pogovoru
- Izvajanje klicev orodij lokalno in vračanje rezultatov modelu
- Izbira pravilnega modela za scenarije klicanja orodij (Qwen 2.5, Phi-4-mini)
- Uporaba nativnega `ChatClient` v SDK za klic orodij (JavaScript)

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Klic orodij z vremenskimi in demografskimi orodji |
| C# | `csharp/ToolCalling.cs` | Klic orodij z .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Klic orodij z ChatClient |

---

### Del 12: Izdelava spletnega UI za Zava Creative Writer

**Vodnik delavnice:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Dodajanje brskalniško temelječega front-enda za Zava Creative Writer
- Ponujanje skupnega UI iz Pythona (FastAPI), JavaScripta (Node.js HTTP) in C# (ASP.NET Core)
- Potrošnja pretočnega NDJSON v brskalniku z Fetch API in ReadableStream
- Prikaz statusnih značk agenta v živo in pretakanje besedila člankov v realnem času

**Koda (skupni UI):**

| Datoteka | Opis |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Postavitev strani |
| `zava-creative-writer-local/ui/style.css` | Stiliziranje |
| `zava-creative-writer-local/ui/app.js` | Bralnik pretoka in logika posodabljanja DOM |

**Dodatki na strežniški strani:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Posodobljeno za strežbo statičnega UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nov HTTP strežnik, ki ovija orkestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nov minimalni API projekt ASP.NET Core |

---

### Del 13: Zaključek delavnice

**Vodnik delavnice:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Povzetek vsega, kar ste zgradili skozi vseh 12 delov
- Nadaljnje ideje za razširitev vaših aplikacij
- Povezave do virov in dokumentacije

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

## Viri

| Vir | Povezava |
|----------|------|
| Foundry Local spletna stran | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalog modelov | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Priročnik za začetek | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referenca za Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licenca

Ta material za delavnico je na voljo za izobraževalne namene.

---

**Veselo ustvarjanje! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Izjava o omejitvi odgovornosti**:
Ta dokument je bil preveden z uporabo storitve za avtomatski prevod AI [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, upoštevajte, da lahko avtomatski prevodi vsebujejo napake ali netočnosti. Izvirni dokument v izvorni jezik je treba velja za avtoritativni vir. Za ključne informacije priporočamo strokovni človeški prevod. Za katera koli nesporazume ali napačne interpretacije, ki izvirajo iz uporabe tega prevoda, ne prevzemamo nobene odgovornosti.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->