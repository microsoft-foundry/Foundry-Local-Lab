<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local dirbtuvės – kurkite DI programas vietoje

Praktinės dirbtuvės, skirtos kalbos modeliams vykdyti savo įrenginyje ir intelektualioms programoms kurti naudojant [Foundry Local](https://foundrylocal.ai) ir [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Kas yra Foundry Local?** Foundry Local yra lengvas vykdymo laikas, leidžiantis visiškai valdyti, atsisiųsti ir aptarnauti kalbos modelius savo aparatinėje įrangoje. Jis suteikia **OpenAI suderinamą API**, todėl bet kuris įrankis ar SDK, palaikantis OpenAI, gali prisijungti – nereikia debesies paskyros.

---

## Mokymosi tikslai

Šių dirbtuvių pabaigoje jūs gebėsite:

| # | Tikslas |
|---|---------|
| 1 | Įdiegti Foundry Local ir valdyti modelius per CLI |
| 2 | Išmokti naudotis Foundry Local SDK API programinei modelių valdymui |
| 3 | Prisijungti prie vietinio inferencijos serverio naudojant Python, JavaScript ir C# SDK |
| 4 | Sukurti Retrieval-Augmented Generation (RAG) sistemą, kuri pagrindžia atsakymus jūsų duomenimis |
| 5 | Kurti DI agentus su pastoviomis instrukcijomis ir personomis |
| 6 | Organizuoti daugiaagentinius darbo procesus su atsiliepimų ciklais |
| 7 | Išnagrinėti gamybinį galutinį projektą – Zava Creative Writer |
| 8 | Kurti vertinimo sistemas su auksiniais duomenų rinkiniais ir LLM kaip teisėju įvertinimais |
| 9 | Transkribuoti garsą su Whisper – balso į tekstą vietoje, naudojant Foundry Local SDK |
| 10 | Kompiliuoti ir vykdyti pasirinktinius ar Hugging Face modelius su ONNX Runtime GenAI ir Foundry Local |
| 11 | Leisti vietiniams modeliams kviesti išorines funkcijas naudojant įrankių kvietimo modelį |
| 12 | Kurti naršyklės UI Zava Creative Writer su realaus laiko transliacija |

---

## Reikalavimai

| Reikalavimas | Informacija |
|--------------|-------------|
| **Aparatūra** | Mažiausiai 8 GB RAM (rekomenduojama 16 GB); AVX2 palaikantis CPU arba palaikoma GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 arba macOS 13+ |
| **Foundry Local CLI** | Įdiegimas per `winget install Microsoft.FoundryLocal` (Windows) arba `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Detalesnė informacija [pradžios vadove](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Programavimo aplinka** | **Python 3.9+** ir (arba) **.NET 9.0+** ir (arba) **Node.js 18+** |
| **Git** | Šiam saugyklos klonavimui |

---

## Pradžia

```bash
# 1. Nukopijuokite saugyklą
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Patikrinkite, ar įdiegtas Foundry Local
foundry model list              # Išvardinkite galimus modelius
foundry model run phi-3.5-mini  # Pradėkite interaktyvų pokalbį

# 3. Pasirinkite savo kalbos kelią (žr. 2 dalies laboratoriją visam nustatymui)
```

| Kalba | Greita pradžia |
|-------|----------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Dirbtuvių dalys

### 1 dalis: Pradžia su Foundry Local

**Laboratorinis vadovas:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Kas yra Foundry Local ir kaip jis veikia
- CLI diegimas Windows ir macOS sistemose
- Modelių tyrinėjimas – laukų sąrašas, atsisiuntimas, paleidimas
- Modelių slapyvardžiai ir dinaminiai prievadai

---

### 2 dalis: Foundry Local SDK gilusis tyrimas

**Laboratorinis vadovas:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Kodėl aplikacijos kūrimui naudoti SDK vietoj CLI
- Pilnas SDK API atskaitos pavyzdys Python, JavaScript ir C#
- Paslaugos valdymas, katalogų naršymas, modelių gyvavimo valdymas (atsisiuntimas, užkrovimas, atlaisvinimas)
- Greito starto pavyzdžiai: Python konstruktoriaus pradžia, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metaduomenys, slapyvardžiai ir aparatinės įrangos optimizuotas modelių parinkimas

---

### 3 dalis: SDK ir API

**Laboratorinis vadovas:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Prisijungimas prie Foundry Local iš Python, JavaScript ir C#
- Foundry Local SDK naudojimas paslaugai valdyti programiškai
- Pokalbių srautas per OpenAI suderinamą API
- SDK metodų atskaita kiekvienai programavimo kalbai

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|-------|--------|-----------|
| Python | `python/foundry-local.py` | Paprastas srautinis pokalbis |
| C# | `csharp/BasicChat.cs` | Pokalbio srautas su .NET |
| JavaScript | `javascript/foundry-local.mjs` | Pokalbio srautas su Node.js |

---

### 4 dalis: Retrieval-Augmented Generation (RAG)

**Laboratorinis vadovas:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Kas yra RAG ir kodėl tai svarbu
- Atmintyje sukurti žinių bazę
- Raktinių žodžių sutapimo paieška su vertinimu
- Sisteminių komandų, pagrįstų duomenimis, kūrimas
- Viso RAG srauto vykdymas vietoje

**Kodo pavyzdžiai:**

| Kalba | Failas |
|-------|--------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 5 dalis: DI agentų kūrimas

**Laboratorinis vadovas:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Kas yra DI agentas (vs. tiesioginis LLM kvietimas)
- `ChatAgent` modelis ir Microsoft Agent Framework
- Sistemos instrukcijos, personos ir daugkartinės pokalbio apykaitos
- Agentų struktūruoto (JSON) išvestis

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|-------|--------|-----------|
| Python | `python/foundry-local-with-agf.py` | Vienas agentas su Agent Framework |
| C# | `csharp/SingleAgent.cs` | Vienas agentas (ChatAgent modelis) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Vienas agentas (ChatAgent modelis) |

---

### 6 dalis: Daugiaagentiniai darbo procesai

**Laboratorinis vadovas:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Daugiaagentės grandinės: Tyrėjas → Rašytojas → Redaktorius
- Sekventa organizacija ir atsiliepimų ciklai
- Bendros konfigūracijos ir struktūruotos perdavimų grandinės
- Kurkite savo daugiaagentį darbo procesą

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|-------|--------|-----------|
| Python | `python/foundry-local-multi-agent.py` | Trijų agentų grandinė |
| C# | `csharp/MultiAgent.cs` | Trijų agentų grandinė |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Trijų agentų grandinė |

---

### 7 dalis: Zava Creative Writer – baigiamasis projektas

**Laboratorinis vadovas:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Gamybinio stiliaus daugiaagentė programa su 4 specializuotais agentais
- Sekvencinė grandinė su vertintojo valdomais atsiliepimų ciklais
- Srautinė išvestis, produktų katalogo paieška, struktūrizuoti JSON perdavimai
- Viso sprendimo įgyvendinimas Python (FastAPI), JavaScript (Node.js CLI) ir C# (.NET konsolė)

**Kodo pavyzdžiai:**

| Kalba | Katalogas | Aprašymas |
|-------|-----------|-----------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI žiniatinklio paslauga su valdymo įrankiu |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI programa |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsolės programa |

---

### 8 dalis: Vertinimu grįstas kūrimas

**Laboratorinis vadovas:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Kurkite sistemingą vertinimo sistemą DI agentams, naudojant auksinius duomenų rinkinius
- Taisyklių patikrinimai (ilgiai, raktinių žodžių aprėptis, draudžiami terminai) + LLM kaip teisėju įvertinimai
- Šalutinė visų komandų variantų palyginimo su bendrais balais
- Pratęsia Zava Editoriaus agento modelį iš 7 dalies į neprisijungus testų rinkinį
- Python, JavaScript ir C# sekcijos

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|-------|--------|-----------|
| Python | `python/foundry-local-eval.py` | Vertinimo sistema |
| C# | `csharp/AgentEvaluation.cs` | Vertinimo sistema |
| JavaScript | `javascript/foundry-local-eval.mjs` | Vertinimo sistema |

---

### 9 dalis: Balso transkripcija su Whisper

**Laboratorinis vadovas:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Balsas į tekstą transkripcija naudojant vietoje veikiantį OpenAI Whisper
- Privatumo prioritetas – garso įrašai niekada neišeina iš įrenginio
- Python, JavaScript ir C# sekcijos su `client.audio.transcriptions.create()` (Python/JS) ir `AudioClient.TranscribeAudioAsync()` (C#)
- Įtraukti Zava tematikos garso failų pavyzdžiai praktikai

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|-------|--------|-----------|
| Python | `python/foundry-local-whisper.py` | Whisper balso transkripcija |
| C# | `csharp/WhisperTranscription.cs` | Whisper balso transkripcija |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper balso transkripcija |

> **Pastaba:** Ši laboratorija naudoja **Foundry Local SDK** programiškai atsisiųsti ir užkrauti Whisper modelį, tada siunčia garsą vietiniam OpenAI suderinamam galiniam taškui transkripcijai. Whisper modelis (`whisper`) pateikiamas Foundry Local kataloge ir veikia visiškai vietoje – nereikia debesies API rakto ar tinklo prieigos.

---

### 10 dalis: Pasirinktinių ar Hugging Face modelių naudojimas

**Laboratorinis vadovas:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face modelių kompiliavimas į optimizuotą ONNX formatą naudojant ONNX Runtime GenAI modelių kūrimo įrankį
- Aparatūrai specifinis kompiliavimas (CPU, NVIDIA GPU, DirectML, WebGPU) ir kvantizavimas (int4, fp16, bf16)
- Pokalbių šablonų konfigūracijų kūrimas Foundry Local
- Įtraukiant kompiliuotus modelius į Foundry Local talpyklą
- Pasirinktinių modelių paleidimas per CLI, REST API ir OpenAI SDK
- Pavyzdys: pilnas Qwen/Qwen3-0.6B kompiliacijos procesas

---

### 11 dalis: Įrankių kvietimas su vietiniais modeliais

**Laboratorinis vadovas:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Leisti vietiniams modeliams kviesti išorines funkcijas (įrankių/funkcijų kvietimas)
- Apibrėžti įrankių schemas naudojant OpenAI funkcijų kvietimo formatą
- Valdyti daugkartinius įrankių kvietimo pokalbius
- Vykdyti įrankių kvietimus vietoje ir grąžinti rezultatus modeliui
- Pasirinkti tinkamą modelį įrankių kvietimo scenarijams (Qwen 2.5, Phi-4-mini)
- Naudoti SDK gimtąją `ChatClient` įrankių kvietimui (JavaScript)

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|-------|--------|-----------|
| Python | `python/foundry-local-tool-calling.py` | Įrankių kvietimas su orų/gyventojų įrankiais |
| C# | `csharp/ToolCalling.cs` | Įrankių kvietimas su .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Įrankių kvietimas su ChatClient |

---

### 12 dalis: Web UI kūrimas Zava Creative Writer

**Laboratorinis vadovas:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Pridėti naršyklės pagrindu veikiantį frontendą Zava Creative Writer
- Pateikti bendrą UI per Python (FastAPI), JavaScript (Node.js HTTP) ir C# (ASP.NET Core)
- Naršyklėje vartoti srautinę NDJSON su Fetch API ir ReadableStream
- Tiesioginiai agentų būsenos ženklai ir realaus laiko straipsnio teksto transliacija

**Kodas (bendras UI):**

| Failas | Aprašymas |
|--------|-----------|
| `zava-creative-writer-local/ui/index.html` | Puslapio išdėstymas |
| `zava-creative-writer-local/ui/style.css` | Stilius |
| `zava-creative-writer-local/ui/app.js` | Srauto skaitytuvo ir DOM atnaujinimo logika |

**Backend papildymai:**

| Kalba | Failas | Aprašymas |
|-------|--------|-----------|
| Python | `zava-creative-writer-local/src/api/main.py` | Atnaujinta statinio UI pateikimui |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Naujas HTTP serveris, supakuojantis valdymo įrankį |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Naujas ASP.NET Core minimalios API projektas |

---

### 13 dalis: Dirbtuvės baigtos
**Laboratorijos vadovas:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Santrauka apie visa, ką sukūrėte per 12 dalių
- Tolimesnės idėjos, kaip plėsti savo programas
- Nuorodos į išteklius ir dokumentaciją

---

## Projekto struktūra

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

## Ištekliai

| Ištekliai | Nuoroda |
|----------|------|
| Foundry Local svetainė | [foundrylocal.ai](https://foundrylocal.ai) |
| Modelių katalogas | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Įvadinis gidas | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK nuoroda | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft agentų sistema | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licencija

Ši dirbtuvė skirta mokymosi tikslams.

---

**Sėkmingo kūrimo! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, prašome atkreipti dėmesį, kad automatiniai vertimai gali turėti klaidų ar netikslumų. Originalus dokumentas jo gimtąja kalba turėtų būti laikomas autoritetingu šaltiniu. Kritinei informacijai rekomenduojamas profesionalus žmogaus vertimas. Mes neatsakome už bet kokius nesusipratimus ar neteisingas interpretacijas, kylančias naudojantis šiuo vertimu.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->