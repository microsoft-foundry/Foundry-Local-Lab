<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local töötuba - AI rakenduste loomine seadmes

Praktiline töötuba keelemudelite käivitamiseks oma masinas ja intelligentsete rakenduste ehitamiseks [Foundry Local](https://foundrylocal.ai) ja [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) abil.

> **Mis on Foundry Local?** Foundry Local on kergekaaluline käitusaeg, mis võimaldab sul keelemudeleid täielikult oma riistvaral alla laadida, hallata ja teenindada. See avab **OpenAI-ühilduva API**, nii et iga tööriist või SDK, mis toetab OpenAI-d, saab ühendada - pilvekonto pole vajalik.

### 🌐 Mitmekeelne tugi

#### Toetatud GitHub Actioni kaudu (automaatne ja alati ajakohane)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Araabia](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgaaria](../bg/README.md) | [Birma (Myanmar)](../my/README.md) | [Hiina (lihtsustatud)](../zh-CN/README.md) | [Hiina (traditsiooniline, Hongkong)](../zh-HK/README.md) | [Hiina (traditsiooniline, Macau)](../zh-MO/README.md) | [Hiina (traditsiooniline, Taiwan)](../zh-TW/README.md) | [Horvaadi](../hr/README.md) | [Tšehhi](../cs/README.md) | [Taani](../da/README.md) | [Hollandi](../nl/README.md) | [Eesti](./README.md) | [Soome](../fi/README.md) | [Prantsuse](../fr/README.md) | [Saksa](../de/README.md) | [Kreeka](../el/README.md) | [Heebrea](../he/README.md) | [Hindi](../hi/README.md) | [Ungari](../hu/README.md) | [Indoneesia](../id/README.md) | [Itaalia](../it/README.md) | [Jaapani](../ja/README.md) | [Kannada](../kn/README.md) | [Khmeri](../km/README.md) | [Korea](../ko/README.md) | [Leedu](../lt/README.md) | [Malai](../ms/README.md) | [Malajalami](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigeeria pidgin](../pcm/README.md) | [Norra](../no/README.md) | [Pärsia (Farsi)](../fa/README.md) | [Poola](../pl/README.md) | [Portugali (Brasiilia)](../pt-BR/README.md) | [Portugali (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Rumeenia](../ro/README.md) | [Vene](../ru/README.md) | [Serbia (kirilitsa)](../sr/README.md) | [Slovaki](../sk/README.md) | [Sloveeni](../sl/README.md) | [Hispaania](../es/README.md) | [Suaheli](../sw/README.md) | [Rootsi](../sv/README.md) | [Tagalog (Filipiinid)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Tai](../th/README.md) | [Türgi](../tr/README.md) | [Ukraina](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnami](../vi/README.md)

> **Eelistad kloonimist lokaalselt?**
>
> See hoidla sisaldab rohkem kui 50 keele tõlget, mis suurendab oluliselt allalaadimise mahtu. Kloonimiseks ilma tõlgeteta kasuta sparsikotšeket:
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
> See annab sulle kõik vajaliku kursuse läbimiseks palju kiirema allalaadimisega.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Õpieesmärgid

Selle töötuba lõpus oskad järgmisi asju:

| # | Eesmärk |
|---|---------|
| 1 | Paigalda Foundry Local ja halda mudeleid CLI kaudu |
| 2 | Valda Foundry Local SDK API programmeeritud mudelite haldamiseks |
| 3 | Ühendu kohaliku inference serveriga Python, JavaScript ja C# SDK-de abil |
| 4 | Ehita Retrieval-Augmented Generation (RAG) torujuhe, mis tugineb sinu enda andmetel vastuste leidmisel |
| 5 | Loo AI agendid püsivate juhiste ja isikupäradega |
| 6 | Orkestreeri mitme agendi töövooge tagasiside ahelatega |
| 7 | Uuri tootmise tipptaset - Zava Creative Writeri rakendust |
| 8 | Ehita hindamisraamistikud kullastandardsete andmekogude ja LLM-žürii skoorimisega |
| 9 | Teosta heli transkriptsiooni Whisperiga - kõnetekstiks teisendamine seadmes Foundry Local SDK abil |
| 10 | Kompileeri ja käivita kohandatud või Hugging Face mudeleid ONNX Runtime GenAI ja Foundry Local abil |
| 11 | Võimalda kohalikel mudelitel väliseid funktsioone kutsuda tööriistakutsestandardiga |
| 12 | Ehita brauseripõhine kasutajaliides Zava Creative Writerile reaalajas voogedastusega |

---

## Eeldused

| Nõue | Üksikasjad |
|-------|------------|
| **Riistvara** | Vähemalt 8 GB RAM (soovitatavalt 16 GB); AVX2-toega protsessor või toetatud GPU |
| **Operatsioonisüsteem** | Windows 10/11 (x64/ARM), Windows Server 2025 või macOS 13+ |
| **Foundry Local CLI** | Paigalda käsuga `winget install Microsoft.FoundryLocal` (Windows) või `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Vaata [alustamise juhendit](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| **Keeleruntime** | **Python 3.9+** ja/või **.NET 9.0+** ja/või **Node.js 18+** |
| **Git** | Selle hoidla kloonimiseks |

---

## Alustamine

```bash
# 1. Kopeeri hoidla
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Kontrolli, kas Foundry Local on installitud
foundry model list              # Saadaval olevate mudelite nimekiri
foundry model run phi-3.5-mini  # Alusta interaktiivset vestlust

# 3. Vali oma keele rada (täieliku seadistuse jaoks vaata Osa 2 labor)
```

| Keel | Kiiralgus |
|-------|------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Töötuba osad

### Osa 1: Tutvustus Foundry Localiga

**Labide juhend:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Mis on Foundry Local ja kuidas see töötab
- CLI paigaldamine Windowsis ja macOS-is
- Mudelite uurimine - nimekirjastamine, allalaadimine, käivitamine
- Mudeli aliaste ja dünaamiliste portide mõistmine

---

### Osa 2: Foundry Local SDK süvitsi

**Labide juhend:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Miks kasutada SDK-d CLI asemel rakenduste arendamiseks
- Täielik SDK API viide Pythonile, JavaScriptile ja C#-le
- Teenuse haldamine, kataloogi sirvimine, mudeli elutsükkel (allalaadimine, laadimine, vabastamine)
- Kiirelt käivitamise mustrid: Python konstruktor, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metaandmed, aliasid ja riistvaralist optimeerimist toetavad mudeli valikud

---

### Osa 3: SDK-d ja API-d

**Labide juhend:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Ühendamine Foundry Locali juurde Pythonist, JavaScriptist ja C#-st
- Foundry Local SDK kasutamine teenuse programmeeritud haldamiseks
- Reaalajas vestluste voogedastus OpenAI-ühilduva API kaudu
- SDK meetodite viited igale keelele

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|-------|-------|-----------|
| Python | `python/foundry-local.py` | Põhiline voogedastusega vestlus |
| C# | `csharp/BasicChat.cs` | Voogedastusega vestlus .NET-iga |
| JavaScript | `javascript/foundry-local.mjs` | Voogedastusega vestlus Node.js-iga |

---

### Osa 4: Retrieval-Augmented Generation (RAG)

**Labide juhend:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Mis on RAG ja miks see oluline on
- Mälus teadmistebaasi ehitamine
- Märksõnade kattuvusel põhinev otsing koos skoorimisega
- Maandatud süsteemi promptide loomine
- Täieliku RAG torujuhe seadmes käivitamine

**Koodinäited:**

| Keel | Fail |
|-------|-------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Osa 5: AI agentide loomine

**Labide juhend:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Mis on AI agent (vastandina puhtale LLM-kutsule)
- `ChatAgent` muster ja Microsoft Agent Framework
- Süsteemi juhised, isiksused ja mitme vooru vestlused
- Agentide struktureeritud väljund (JSON)

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|-------|-------|-----------|
| Python | `python/foundry-local-with-agf.py` | Üks agent Agent Frameworkiga |
| C# | `csharp/SingleAgent.cs` | Üks agent (ChatAgent muster) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Üks agent (ChatAgent muster) |

---

### Osa 6: Mitme agendi töövood

**Labide juhend:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Mitme agendi torujuht: Teadur → Kirjutaja → Toimetaja
- Järjestikune orkestreerimine ja tagasiside ahelad
- Ühised konfiguratsioonid ja struktureeritud üleandmised
- Oma mitme agendi töövoo kujundamine

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|-------|-------|-----------|
| Python | `python/foundry-local-multi-agent.py` | Kolme agendi torujuht |
| C# | `csharp/MultiAgent.cs` | Kolme agendi torujuht |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Kolme agendi torujuht |

---

### Osa 7: Zava Creative Writer - tipprakendus

**Labide juhend:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Tootmistasemel mitme agendi rakendus nelja spetsialiseerunud agendiga
- Järjestikune torujuht hindajapõhiste tagasiside ahelatega
- Voogedastuse väljund, tootekataloogi otsing, struktureeritud JSON üleandmised
- Täielik rakendus Pythonis (FastAPI), JavaScriptis (Node.js CLI) ja C#-s (.NET konsool)

**Koodinäited:**

| Keel | Kaust | Kirjeldus |
|-------|---------|-----------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI veebiteenus orkestreerijaga |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI rakendus |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsoolirakendus |

---

### Osa 8: Hinnangul põhinev arendus

**Labide juhend:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Ehita süsteemne hindamisraamistik AI agentide jaoks kullastandardsete andmekogude abil
- Reeglipõhised kontrollid (pikkus, märksõnade katvus, keelatud terminid) + LLM-žürii skoorimine
- Kõrvuti võrdlus prompti variatsioonidega koos koguskooridega
- Laiendab Zava Toimetaja agendi mustrit osas 7 offline testikomplektiks
- Vastavad rajad Pythonile, JavaScriptile ja C#-le

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|-------|-------|-----------|
| Python | `python/foundry-local-eval.py` | Hindamisraamistik |
| C# | `csharp/AgentEvaluation.cs` | Hindamisraamistik |
| JavaScript | `javascript/foundry-local-eval.mjs` | Hindamisraamistik |

---

### Osa 9: Hääle transkriptsioon Whisperiga

**Labide juhend:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Kõnetekstiks muutmine OpenAI Whisperiga, mis töötab lokaalselt
- Privaatsusele keskenduv heli töötlemine – heli ei lahku kunagi teie seadmest
- Python, JavaScript ja C# näited `client.audio.transcriptions.create()` (Python/JS) ja `AudioClient.TranscribeAudioAsync()` (C#) kasutamiseks
- Sisaldab Zava-teemalisi näidishelifaile praktiliseks harjutamiseks

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisperi hääle tekstiks muutmine |
| C# | `csharp/WhisperTranscription.cs` | Whisperi hääle tekstiks muutmine |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisperi hääle tekstiks muutmine |

> **Märkus:** See labor kasutab **Foundry Local SDK-d**, et programmiliselt alla laadida ja laadida Whisperi mudel ning seejärel saata heli lokaalsele OpenAI-ga ühilduvale otsapunktile teksti väljastamiseks. Whisperi mudel (`whisper`) on loetletud Foundry Local kataloogis ja töötab täielikult seadmes – pole vaja pilve API võtmeid ega võrguühendust.

---

### Osa 10: Kohandatud või Hugging Face mudelite kasutamine

**Laborijuhend:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face mudelite kompileerimine optimeeritud ONNX vormingusse ONNX Runtime GenAI mudeliloome tööriistaga
- Riistvaraspetsiifiline kompileerimine (CPU, NVIDIA GPU, DirectML, WebGPU) ja kvantimine (int4, fp16, bf16)
- Chat-malli konfiguratsioonifailide loomine Foundry Local jaoks
- Kompileeritud mudelite lisamine Foundry Local vahemällu
- Kohandatud mudelite käivitamine CLI, REST API ja OpenAI SDK kaudu
- Näidis: Qwen/Qwen3-0.6B mudeli lõplik kompileerimine

---

### Osa 11: Tööriistakõnede tegemine kohalike mudelitega

**Laborijuhend:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Lubage kohalikel mudelitel välisseadmeid kutsuda (tööriistade/funktsioonide kõne)
- Määratlege tööriistade skeemid OpenAI funktsioonikõne vormingus
- Töötage välja mitme vooru tööriistakõnede vestluse voog
- Käivitage tööriistakõned lokaalselt ja tagastage tulemused mudelile
- Valige tööriistakõnede stsenaariumide jaoks sobiv mudel (Qwen 2.5, Phi-4-mini)
- Kasutage SDK sisseehitatud `ChatClient`-i tööriistakõnede tegemiseks (JavaScript)

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tööriistakõned ilma/rahvastiku tööriistadega |
| C# | `csharp/ToolCalling.cs` | Tööriistakõned .NET-ga |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tööriistakõned ChatClient-iga |

---

### Osa 12: Veebiliidese ehitamine Zava Loovkirjutajale

**Laborijuhend:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Lisage Zava Loovkirjutajale veebipõhine kasutajaliides
- Serveeri jagatud kasutajaliidest Pythoni (FastAPI), JavaScripti (Node.js HTTP) ja C# (ASP.NET Core) kaudu
- Tarbi sirvijas striimivat NDJSON-i Fetch API ja ReadableStream abil
- Otseagentide oleku märgised ja reaalajas artikli teksti striimimine

**Kood (jagatud kasutajaliides):**

| Fail | Kirjeldus |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Lehe paigutus |
| `zava-creative-writer-local/ui/style.css` | Stiilid |
| `zava-creative-writer-local/ui/app.js` | Striimilugeja ja DOM-i uuendamise loogika |

**Tagapõhja täiendused:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Uuendatud staatilise kasutajaliidese serveerimiseks |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Uus HTTP-server orkestreerija ümber |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Uus ASP.NET Core minimaalse API projekt |

---

### Osa 13: Töötuba lõpetatud

**Laborijuhend:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Kokkuvõte kõigest, mida olete kõikides 12 osas ehitanud
- Edasised ideed teie rakenduste laiendamiseks
- Lingid ressurssidele ja dokumentatsioonile

---

## Projekti struktuur

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

## Ressursid

| Ressurss | Link |
|----------|------|
| Foundry Local veebileht | [foundrylocal.ai](https://foundrylocal.ai) |
| Mudelikataloog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Algusjuhend | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK viide | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent raamistik | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Litsents

See tööstuba materjal on mõeldud hariduslikel eesmärkidel.

---

**Head ehitamist! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Vastutühing**:
See dokument on tõlgitud AI tõlketeenuse [Co-op Translator](https://github.com/Azure/co-op-translator) abil. Kuigi püüame täpsust, palun arvestage, et automaatsed tõlked võivad sisaldada vigu või ebatäpsusi. Algne dokument selle emakeeles tuleks pidada autoriteetseks allikaks. Olulise teabe puhul soovitatakse kasutada professionaalset inimtõlget. Me ei vastuta selle tõlke kasutamisest tingitud arusaamatuste või valesti mõistmiste eest.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->