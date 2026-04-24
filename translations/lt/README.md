<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local dirbtuvės - kuriame DI programas įrenginyje

Praktinės dirbtuvės, skirtos vykdyti kalbos modelius savo kompiuteryje ir kurti intelektines programas naudojant [Foundry Local](https://foundrylocal.ai) ir [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Kas yra Foundry Local?** Foundry Local yra lengvas vykdymo laikotarpis, leidžiantis atsisiųsti, valdyti ir aptarnauti kalbos modelius visiškai jūsų aparatinėje įrangoje. Jis teikia **OpenAI suderinamą API**, todėl bet kuris įrankis ar SDK, palaikantis OpenAI, gali prisijungti - nereikia jokios debesijos paskyros.

### 🌐 Daugiakalbė palaikymas

#### Palaikoma per GitHub Action (automatizuota ir visada atnaujinta)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](./README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Pageidaujate kopijuoti vietoje?**
>
> Šis saugyklos archyvas apima daugiau nei 50 kalbų vertimų, kurie ženkliai didina atsisiuntimo dydį. Norėdami kopijuoti be vertimų, naudokite sparse checkout:
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
> Tai suteikia jums viską, ko reikia kursui, su daug greitesniu atsisiuntimu.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Mokymosi tikslai

Po šių dirbtuvių jūs galėsite:

| # | Tikslas |
|---|---------|
| 1 | Įdiegti Foundry Local ir valdyti modelius per CLI |
| 2 | Įvaldyti Foundry Local SDK API programiniam modelių valdymui |
| 3 | Prisijungti prie vietinio inferencijos serverio naudojant Python, JavaScript ir C# SDK |
| 4 | Kurti Retrieval-Augmented Generation (RAG) grandinę, kuri grindžia atsakymus jūsų duomenimis |
| 5 | Kurti DI agentus su nuolatinėmis instrukcijomis ir personomis |
| 6 | Orkestruoti daugiaagentinius darbo srautus su grįžtamojo ryšio ciklais |
| 7 | Išnagrinėti produkcijos galutinę programą - Zava kūrybinio rašytojo aplikaciją |
| 8 | Kurti vertinimo sistemas su aukso duomenų rinkiniais ir LLM kaip teisėjo vertinimu |
| 9 | Transkribuoti garsą su Whisper - kalbos į tekstą įrenginyje naudojant Foundry Local SDK |
| 10 | Kompiliuoti ir vykdyti pasirinktinius arba Hugging Face modelius su ONNX Runtime GenAI ir Foundry Local |
| 11 | Leidžiama vietiniams modeliams kviesti išorines funkcijas naudodami įrankių kvietimo modelį |
| 12 | Kurti naršyklės pagrindu sukurtą UI Zava kūrybiniam rašytojui su realaus laiko srautu |

---

## Reikalavimai

| Reikalavimas | Detalės |
|--------------|---------|
| **Aparatinė įranga** | Bent 8 GB RAM (rekomenduojama 16 GB); AVX2 palaikantis CPU arba palaikoma GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 arba macOS 13+ |
| **Foundry Local CLI** | Įdiekite per `winget install Microsoft.FoundryLocal` (Windows) arba `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Daugiau informacijos rasite [pradžios vadove](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Kalbos vykdymo aplinka** | **Python 3.9+** ir/arba **.NET 9.0+** ir/arba **Node.js 18+** |
| **Git** | Šio saugyklos archyvo klonavimui |

---

## Pradžia

```bash
# 1. Nukopijuokite saugyklą
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Patikrinkite, ar įdiegta Foundry Local
foundry model list              # Rodyti prieinamus modelius
foundry model run phi-3.5-mini  # Paleisti interaktyvų pokalbį

# 3. Pasirinkite savo kalbos takelį (žr. 2 dalies laboratoriją dėl viso nustatymo)
```

| Kalba | Greita pradžia |
|--------|---------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Dirbtuvių dalys

### 1 dalis: Pradžia su Foundry Local

**Laboratorijos vadovas:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Kas yra Foundry Local ir kaip jis veikia
- CLI įdiegimas Windows ir macOS
- Modelių tyrinėjimas - sąrašas, atsisiuntimas, paleidimas
- Modelių slapyvardžių ir dinaminio prievado supratimas

---

### 2 dalis: Foundry Local SDK giluminis apžvalga

**Laboratorijos vadovas:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Kodėl naudoti SDK vietoje CLI programų kūrimui
- Pilnas SDK API sąrašas Python, JavaScript ir C#
- Paslaugų valdymas, katalogo naršymas, modelio gyvavimo ciklas (atsisiuntimas, įkėlimas, iškėlimas)
- Greito pradžios pavyzdžiai: Python konstruktorius bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metaduomenys, slapyvardžiai ir optimalus aparatūros modelio pasirinkimas

---

### 3 dalis: SDK ir API

**Laboratorijos vadovas:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Prisijungimas prie Foundry Local naudojant Python, JavaScript ir C#
- Foundry Local SDK naudojimas paslaugos programiniam valdymui
- Srautas pokalbių užbaigimams per OpenAI suderinamą API
- SDK metodų sąrašas kiekvienai kalbai

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|--------|-------|-----------|
| Python | `python/foundry-local.py` | Pagrindinis srautinio pokalbio pavyzdys |
| C# | `csharp/BasicChat.cs` | Srautinio pokalbio pavyzdys su .NET |
| JavaScript | `javascript/foundry-local.mjs` | Srautinio pokalbio pavyzdys su Node.js |

---

### 4 dalis: Retrieval-Augmented Generation (RAG)

**Laboratorijos vadovas:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Kas yra RAG ir kodėl tai svarbu
- Atmintinėje saugomos žinių bazės kūrimas
- Raktažodžių sutapimų gavimas su įvertinimu
- Sisteminių užuominų kūrimas, grindžiamas kontekstu
- Pilno RAG darbo proceso įrenginyje paleidimas

**Kodo pavyzdžiai:**

| Kalba | Failas |
|--------|--------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 5 dalis: DI agentų kūrimas

**Laboratorijos vadovas:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Kas yra DI agentas (priešingai nei tiesioginis LLM kvietimas)
- `ChatAgent` šablonas ir Microsoft Agent Framework
- Sistemos instrukcijos, personos ir daugiasluoksniai pokalbiai
- Agentų struktūruotas išvestis (JSON)

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|--------|--------|-----------|
| Python | `python/foundry-local-with-agf.py` | Vienas agentas su Agent Framework |
| C# | `csharp/SingleAgent.cs` | Vienas agentas (ChatAgent šablonas) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Vienas agentas (ChatAgent šablonas) |

---

### 6 dalis: Daugiaagentiniai darbo srautai

**Laboratorijos vadovas:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Daugiaagentiniai procesai: tyrėjas → rašytojas → redaktorius
- Sekvencinė orkestracija ir grįžtamojo ryšio ciklai
- Dalinta konfigūracija ir struktūruoti perdavimai
- Sukurkite savo daugiaagentinį darbo srautą

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|--------|--------|-----------|
| Python | `python/foundry-local-multi-agent.py` | Trijų agentų grandinė |
| C# | `csharp/MultiAgent.cs` | Trijų agentų grandinė |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Trijų agentų grandinė |

---

### 7 dalis: Zava kūrybinis rašytojas - baigiamoji programa

**Laboratorijos vadovas:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkcijos stiliaus daugiaagentinė programa su 4 specializuotais agentais
- Sekvencinė grandinė su vertintojo grįžtamojo ryšio ciklais
- Srautinė išvestis, produktų katalogo paieška, struktūruoti JSON perdavimai
- Viso kodo įgyvendinimas Python (FastAPI), JavaScript (Node.js CLI) ir C# (.NET konsolė)

**Kodo pavyzdžiai:**

| Kalba | Katalogas | Aprašymas |
|--------|----------|-----------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI interneto paslauga su orkestratoriumi |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI programa |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsolės programa |

---

### 8 dalis: Vertinimu grindžiamas vystymas

**Laboratorijos vadovas:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Sukurkite sisteminę vertinimo sistemą DI agentams naudojant aukso duomenų rinkinius
- Taisymais pagrįsti patikrinimai (ilgis, raktažodžių aprėptis, draudžiami terminai) + LLM kaip teisėjas balas
- Šalia vienas kito palyginimas sužadinimų variantų su bendrais balų lapais
- Išplėčia 7 dalies Zava Redaktoriaus agento šabloną į neprisijungus testų rinkinį
- Python, JavaScript ir C# takeliai

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|--------|--------|-----------|
| Python | `python/foundry-local-eval.py` | Vertinimo sistema |
| C# | `csharp/AgentEvaluation.cs` | Vertinimo sistema |
| JavaScript | `javascript/foundry-local-eval.mjs` | Vertinimo sistema |

---

### 9 dalis: Balso transkripcija su Whisper

**Laboratorijos vadovas:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Kalbos į tekstą transkripcija naudojant vietoje veikiančią OpenAI Whisper
- Privatumą užtikrinantis garso apdorojimas – garso įrašas niekada neišeina iš jūsų įrenginio
- Python, JavaScript ir C# pavyzdžiai su `client.audio.transcriptions.create()` (Python/JS) ir `AudioClient.TranscribeAudioAsync()` (C#)
- Įtraukti Zava tematikos pavyzdiniai garso failai praktikai

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper balso transkripcija |
| C# | `csharp/WhisperTranscription.cs` | Whisper balso transkripcija |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper balso transkripcija |

> **Pastaba:** Šiame laboratoriniame darbe naudojamas **Foundry Local SDK**, kuris programiškai atsisiunčia ir įkelia Whisper modelį, o tuomet siunčia garsą į vietinį OpenAI suderinamą galinį tašką transkripcijai. Whisper modelis (`whisper`) yra nurodytas Foundry Local kataloge ir veikia visiškai įrenginyje – nereikia jokių debesijos API raktų ar tinklo prieigos.

---

### 10 dalis: Naudojimasis pasirinktiniais arba Hugging Face modeliais

**Laboratorijos gidas:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face modelių kompiliavimas į optimizuotą ONNX formatą naudojant ONNX Runtime GenAI modelio kūrėją
- Aparatūros specifiniai kompiliavimo režimai (CPU, NVIDIA GPU, DirectML, WebGPU) ir kvantizacija (int4, fp16, bf16)
- Pokalbių šablonų konfigūracijos failų kūrimas Foundry Local
- Sukompiliuotų modelių pridėjimas į Foundry Local talpyklą
- Pasirinktinių modelių paleidimas per CLI, REST API ir OpenAI SDK
- Pavyzdys: Qwen/Qwen3-0.6B modelio viso ciklo kompiliavimas

---

### 11 dalis: Įrankių kvietimas su vietiniais modeliais

**Laboratorijos gidas:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Leisti vietiniams modeliams kviesti išorines funkcijas (įrankių/funkcijų kvietimas)
- Apibrėžti įrankių schemas naudojant OpenAI funkcijų kvietimo formatą
- Valdyti daugkartinius įrankių kvietimų pokalbius
- Vykdyti įrankių kvietimus vietoje ir grąžinti rezultatus modeliui
- Parinkti tinkamą modelį įrankių kvietimo scenarijams (Qwen 2.5, Phi-4-mini)
- Naudoti SDK gimtąją `ChatClient` klases įrankių kvietimui (JavaScript)

**Kodo pavyzdžiai:**

| Kalba | Failas | Aprašymas |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Įrankių kvietimas su orų/gyventoju duomenų įrankiais |
| C# | `csharp/ToolCalling.cs` | Įrankių kvietimas su .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Įrankių kvietimas su ChatClient |

---

### 12 dalis: Internetinės sąsajos kūrimas Zava kūrybiniam rašytojui

**Laboratorijos gidas:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Pridėti žiniatinklio naršyklės pagrindu veikiančią sąsają Zava kūrybiniam rašytojui
- Tiekimas bendros sąsajos iš Python (FastAPI), JavaScript (Node.js HTTP) ir C# (ASP.NET Core)
- Naršyklėje vartoti srautinį NDJSON per Fetch API ir ReadableStream
- Gyvo agento būklės ženkleliai ir realaus laiko straipsnio teksto srautas

**Kodas (bendra sąsaja):**

| Failas | Aprašymas |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Puslapio išdėstymas |
| `zava-creative-writer-local/ui/style.css` | Stilius |
| `zava-creative-writer-local/ui/app.js` | Srautinio skaitytuvo ir DOM atnaujinimo logika |

**Serverio papildymai:**

| Kalba | Failas | Aprašymas |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Atnaujinta tarnaujant statinę sąsają |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Naujas HTTP serveris, apgaubiantis orchestratorių |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Naujas minimalus ASP.NET Core API projektas |

---

### 13 dalis: Darbo užbaigimas

**Laboratorijos gidas:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Apžvalga visko, ką sukūrėte per 12 dalių
- Tolimesnės idėjos programėlių plėtimui
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

| Išteklius | Nuoroda |
|----------|------|
| Foundry Local svetainė | [foundrylocal.ai](https://foundrylocal.ai) |
| Modelių katalogas | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Pradžios vadovas | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK referencija | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft agentų karkasas | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licencija

Šie darbo medžiagos skirti švietimo tikslams.

---

**Sėkmingo kūrimo! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, atkreipkite dėmesį, kad automatiniai vertimai gali turėti klaidų ar netikslumų. Originalus dokumentas natūralia kalba turėtų būti laikomas autoritetingu šaltiniu. Svarbios informacijos atveju rekomenduojama naudoti profesionalų žmogaus vertimą. Mes neprisiimame atsakomybės už bet kokius nesusipratimus ar neteisingus aiškinimus, kilusius dėl šio vertimo naudojimo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->