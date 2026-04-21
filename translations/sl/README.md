<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Delavnica Foundry Local - Gradnja AI aplikacij na napravi

Praktična delavnica za zagon jezikovnih modelov na lastnem računalniku in gradnjo inteligentnih aplikacij z [Foundry Local](https://foundrylocal.ai) in [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Kaj je Foundry Local?** Foundry Local je lahka izvršilna okolica, ki vam omogoča prenos, upravljanje in gostovanje jezikovnih modelov povsem na vaši strojni opremi. Ponuja **API združljiv z OpenAI**, tako da se lahko vsak pripomoček ali SDK, ki podpira OpenAI, poveže - brez potrebe po računu v oblaku.

---

## Cilji učenja

Na koncu te delavnice boste znali:

| # | Cilj |
|---|-----------|
| 1 | Namestiti Foundry Local in upravljati modele preko CLI |
| 2 | Obvladati Foundry Local SDK API za programsko upravljanje modelov |
| 3 | Povezati se na lokalni strežnik za sklepe z uporabo Python, JavaScript in C# SDK-jev |
| 4 | Zgraditi Retrieval-Augmented Generation (RAG) potek, ki temelji na vaših lastnih podatkih |
| 5 | Ustvariti AI agente s trajnimi navodili in osebnostmi |
| 6 | Orkestrirati večagentne delovne tokove z zankami povratnih informacij |
| 7 | Raziskati produkcijsko zaključeno aplikacijo - Zava Creative Writer |
| 8 | Zgraditi ocenjevalne okvire z zlatimi podatkovnimi zbirkami in LLM-kot-sodnik ocenjevanjem |
| 9 | Presnemati zvok z Whisper - pretvorba govora v besedilo na napravi z Foundry Local SDK |
| 10 | Prevesti in zagnati lastne ali Hugging Face modele z ONNX Runtime GenAI in Foundry Local |
| 11 | Omogočiti lokalnim modelom klicanje zunanjih funkcij z vzorcem tool-calling |
| 12 | Zgraditi brskalniški uporabniški vmesnik za Zava Creative Writer s pretakanjem v realnem času |

---

## Zahteve

| Zahteva | Podrobnosti |
|-------------|---------|
| **Strojna oprema** | Najmanj 8 GB RAM (priporočeno 16 GB); CPU z AVX2 ali podprt GPU |
| **Operacijski sistem** | Windows 10/11 (x64/ARM), Windows Server 2025 ali macOS 13+ |
| **Foundry Local CLI** | Namestite z `winget install Microsoft.FoundryLocal` (Windows) ali `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Ogled podrobnosti v [uvozni vodič](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Jezikovno izvajanje** | **Python 3.9+** in/ali **.NET 9.0+** in/ali **Node.js 18+** |
| **Git** | Za kloniranje tega repozitorija |

---

## Začetek

```bash
# 1. Klonirajte repozitorij
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Preverite, ali je Foundry Local nameščen
foundry model list              # Naštejte razpoložljive modele
foundry model run phi-3.5-mini  # Začnite interaktivni klepet

# 3. Izberite svoj jezikovni program (glejte laboratorij 2. dela za popolno nastavitev)
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
- Raziskovanje modelov - seznam, prenos, zagon
- Razumevanje vzdevkov modelov in dinamičnih vrat

---

### Del 2: Poglobljen pregled SDK Foundry Local

**Vodič za laboratorij:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Zakaj uporabljati SDK namesto CLI za razvoj aplikacij
- Popolna referenca API SDK za Python, JavaScript in C#
- Upravljanje storitev, pregledovanje kataloga, življenjski cikel modelov (prenos, nalaganje, odlaganje)
- Vzorci hitrega začetka: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Metapodatki `FoundryModelInfo`, vzdevki in izbira modela optimalnega za strojno opremo

---

### Del 3: SDK in API-ji

**Vodič za laboratorij:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Povezava na Foundry Local iz Python, JavaScript in C#
- Uporaba Foundry Local SDK za programsko upravljanje storitve
- Pretakanje pogovornih odgovorov prek API-ja, združljivega z OpenAI
- Referenca metod SDK za vsak jezik

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Osnovni pretočni klepet |
| C# | `csharp/BasicChat.cs` | Pretočni klepet z .NET |
| JavaScript | `javascript/foundry-local.mjs` | Pretočni klepet z Node.js |

---

### Del 4: Retrieval-Augmented Generation (RAG)

**Vodič za laboratorij:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Kaj je RAG in zakaj je pomemben
- Gradnja spominskega bazena znanja
- Iskanje po prekrivanju ključnih besed z ocenjevanjem
- Sestavljanje utemeljenih sistemskih pozivov
- Zagon celotnega RAG poteka na napravi

**Primeri kode:**

| Jezik | Datoteka |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Del 5: Gradnja AI agentov

**Vodič za laboratorij:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Kaj je AI agent (v primerjavi z neposrednim klicem LLM)
- Vzorec `ChatAgent` in Microsoft Agent Framework
- Sistemska navodila, osebnosti in večkratni pogovorni krogi
- Strukturiran izhod (JSON) od agentov

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Posamezen agent z Agent Framework |
| C# | `csharp/SingleAgent.cs` | Posamezen agent (vzorec ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Posamezen agent (vzorec ChatAgent) |

---

### Del 6: Večagentni delovni tokovi

**Vodič za laboratorij:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Večagentne poti: Raziskovalec → Pisec → Urednik
- Zaporedna orkestracija in zanke povratnih informacij
- Skupna konfiguracija in strukturirane predaje
- Oblikovanje lastnega večagentnega delovnega toka

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Potek treh agentov |
| C# | `csharp/MultiAgent.cs` | Potek treh agentov |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Potek treh agentov |

---

### Del 7: Zava Creative Writer - zaključna aplikacija

**Vodič za laboratorij:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkcijska večagentna aplikacija s 4 specializiranimi agenti
- Zaporedni potek z zankami povratnih informacij, ki jih vodi ocenjevalec
- Pretakan izhod, iskanje po katalogu izdelkov, strukturirane predaje v JSON
- Polna implementacija v Pythonu (FastAPI), JavaScriptu (Node.js CLI) in C# (.NET konzola)

**Primeri kode:**

| Jezik | Mapa | Opis |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI spletna storitev z orkestratorjem |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI aplikacija |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzolna aplikacija |

---

### Del 8: Razvoj voden z ocenjevanjem

**Vodič za laboratorij:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Zgradite sistematičen ocenjevalni okvir za AI agente z zlatimi podatkovnimi zbirkami
- Pravila za preverjanje (dolžina, pokritost ključnih besed, prepovedani izrazi) + LLM kot sodnik za ocenjevanje
- Primerjava variant pozivov ob strani z zbirkami ocen
- Razširja vzorec agenta Zava Editor iz dela 7 v offline testni komplet
- Poti za Python, JavaScript in C#

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Ocenjevalni okvir |
| C# | `csharp/AgentEvaluation.cs` | Ocenjevalni okvir |
| JavaScript | `javascript/foundry-local-eval.mjs` | Ocenjevalni okvir |

---

### Del 9: Prepis glasu z Whisper

**Vodič za laboratorij:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Pretvorba govora v besedilo z uporabo OpenAI Whisper, ki teče lokalno
- Prednost zasebnosti pri obdelavi zvoka - zvok nikoli ne zapusti vaše naprave
- Poti za Python, JavaScript in C# s `client.audio.transcriptions.create()` (Python/JS) in `AudioClient.TranscribeAudioAsync()` (C#)
- Vključene vzorčne zvočne datoteke s temo Zava za praktično delo

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Prepis glasu Whisper |
| C# | `csharp/WhisperTranscription.cs` | Prepis glasu Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Prepis glasu Whisper |

> **Opomba:** Ta laboratorij uporablja **Foundry Local SDK** za programsko prenos in nalaganje Whisper modela, nato pošilja zvok na lokalno endpoint, združljiv z OpenAI, za prepis. Whisper model (`whisper`) je naveden v katalogu Foundry Local in teče povsem na napravi - brez potreb po API ključih oblaka ali dostopu do omrežja.

---

### Del 10: Uporaba lastnih ali Hugging Face modelov

**Vodič za laboratorij:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Prevajanje Hugging Face modelov v optimizirano obliko ONNX z gradnikom modelov ONNX Runtime GenAI
- Strojno specifično prevajanje (CPU, NVIDIA GPU, DirectML, WebGPU) in kvantizacija (int4, fp16, bf16)
- Ustvarjanje konfiguracijskih datotek s predlogami za klepet za Foundry Local
- Dodajanje prevedenih modelov v predpomnilnik Foundry Local
- Zagon lastnih modelov preko CLI, REST API-ja in OpenAI SDK
- Primer v referenci: kompletno prevajanje Qwen/Qwen3-0.6B

---

### Del 11: Klicanje orodij z lokalnimi modeli

**Vodič za laboratorij:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Omogočite lokalnim modelom klic zunanjih funkcij (klicanje orodij/funkcij)
- Določite sheme orodij z uporabo formata OpenAI za klic funkcij
- Ravnajte z večkrožnim potekom pogovora za klicanje orodij
- Izvedite klice orodij lokalno in vrnite rezultate modelu
- Izberite pravi model za scenarije klicanja orodij (Qwen 2.5, Phi-4-mini)
- Uporaba nativnega `ChatClient` iz SDK za klic orodij (JavaScript)

**Primeri kode:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Klic orodij z vremenom/populacijo |
| C# | `csharp/ToolCalling.cs` | Klic orodij z .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Klic orodij s ChatClient |

---

### Del 12: Gradnja spletnega UI za Zava Creative Writer

**Vodič za laboratorij:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Dodajte brskalniški uporabniški vmesnik za Zava Creative Writer
- Gostite skupni UI iz Pythona (FastAPI), JavaScript (Node.js HTTP) in C# (ASP.NET Core)
- Porabljajte pretočni NDJSON v brskalniku s Fetch API in ReadableStream
- Prikazi statusa agenta v živo in pretakanje besedila članka v realnem času

**Koda (skupni UI):**

| Datoteka | Opis |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Postavitev strani |
| `zava-creative-writer-local/ui/style.css` | Stiliranje |
| `zava-creative-writer-local/ui/app.js` | Bralnik pretoka in logika za posodobitev DOM |

**Dodatki za backend:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Posodobljeno za strežbo statičnega UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nov HTTP strežnik, ki ovija orkestratorja |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nov ASP.NET Core minimalni API projekt |

---

### Del 13: Delavnica končana
**Vodnik delavnice:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Povzetek vsega, kar ste zgradili skozi vseh 12 delov
- Nadaljnje ideje za razširjanje vaših aplikacij
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
| Spletna stran Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalog modelov | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Priročnik za začetek | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referenca Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licenca

Material te delavnice je na voljo za izobraževalne namene.

---

**Uspešno gradnjo! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Omejitev odgovornosti**:  
Ta dokument je bil preveden z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, vas prosimo, da upoštevate, da lahko avtomatizirani prevodi vsebujejo napake ali netočnosti. Izvirni dokument v njegovem izvirnem jeziku velja za avtoritativni vir. Za ključne informacije priporočamo strokovni človeški prevod. Ne odgovarjamo za kakršne koli nesporazume ali napačne razlage, ki izhajajo iz uporabe tega prevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->