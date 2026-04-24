<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Warsha ya Foundry Local - Jenga Programu za AI Kwenye Kifaa

Warsha ya vitendo ya kuendesha mifano ya lugha kwenye mashine yako mwenyewe na kujenga programu zenye akili kwa kutumia [Foundry Local](https://foundrylocal.ai) na [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Foundry Local ni nini?** Foundry Local ni mazingira nyepesi ya kuendesha yanayokuruhusu kupakua, kusimamia, na kuhudumia mifano ya lugha kwa kabisa kwenye vifaa vyako. Inatoa **API inayolingana na OpenAI** hivyo chombo chochote au SDK kinachozungumza OpenAI kinaweza kuunganishwa - hakihitaji akaunti ya wingu.

### 🌐 Msaada wa Lugha Nyingi

#### Inatangiwa kupitia Kitendo cha GitHub (Kiotomatiki & Daima Kinasasishwa)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](./README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Unapendelea Kwenye Nakala Mitaa?**
>
> Rejesta hii inajumuisha tafsiri za lugha zaidi ya 50 ambayo huongeza sana ukubwa wa kupakua. Ili kunakili bila tafsiri, tumia sparse checkout:
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
> Hii inakupa kila kitu unachohitaji kukamilisha kozi kwa upakuaji wa kasi zaidi.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Malengo ya Kujifunza

Mwisho wa warsha hii utaweza:

| # | Lengo |
|---|-----------|
| 1 | Sakinisha Foundry Local na usimamishe mifano kwa CLI |
| 2 | Shuuru API ya SDK ya Foundry Local kwa usimamizi wa mifano kwa mpango |
| 3 | Unganisha kwenye seva la inference la ndani kwa kutumia SDK za Python, JavaScript, na C# |
| 4 | Jenga mnyororo wa Matengenezo ya Ukuaji wa Uzalishaji (RAG) unaosimamia majibu kwa data yako mwenyewe |
| 5 | Tengeneza mawakala wa AI wenye maelekezo ya kudumu na utu wa matumizi |
| 6 | Ratibu mtiririko wa kazi wa mawakala wengi na mizunguko ya maoni |
| 7 | Chunguza programu ya uzalishaji wa daraja la juu - Mwandishi wa Ubunifu wa Zava |
| 8 | Jenga mfumo wa tathmini kwa kutumia seti za dhahabu na alama za LLM kama hakimu |
| 9 | Andika sauti kwa maandishi kwa Whisper - hotuba-kwa-maandishi kwenye kifaa kwa kutumia SDK ya Foundry Local |
| 10 | Kusanya na endesha mifano maalum au ya Hugging Face kwa ONNX Runtime GenAI na Foundry Local |
| 11 | Wezesha mifano ya ndani kupiga simu za nje kwa mtindo wa kuitwa kwa zana |
| 12 | Jenga muonekano wa kivinjari kwa Mwandishi wa Ubunifu wa Zava kwa mtiririko wa moja kwa moja |

---

## Masharti

| Sharti | Maelezo |
|-------------|---------|
| **Vifaa** | Angalau 8 GB RAM (16 GB inashauriwa); CPU yenye uwezo wa AVX2 au GPU inayoungwa mkono |
| **Mfumo wa Uendeshaji** | Windows 10/11 (x64/ARM), Windows Server 2025, au macOS 13+ |
| **CLI ya Foundry Local** | Sakinisha kupitia `winget install Microsoft.FoundryLocal` (Windows) au `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Angalia [mwongozo wa kuanza](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) kwa maelezo. |
| **Runtime ya Lugha** | **Python 3.9+** na/au **.NET 9.0+** na/au **Node.js 18+** |
| **Git** | Kwa kunakili rejesta hii |

---

## Jinsi ya Kuanzisha

```bash
# 1. Nakili hifadhidata
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Thibitisha Foundry Local imewekwa
foundry model list              # Orodhesha mifano iliyopo
foundry model run phi-3.5-mini  # Anza mazungumzo ya kuingiliana

# 3. Chagua njia ya lugha yako (angalia kiwanda cha Sehemu ya 2 kwa usanidi kamili)
```

| Lugha | Anza Haraka |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Sehemu za Warsha

### Sehemu ya 1: Kuanzisha na Foundry Local

**Mwongozo wa maabara:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local ni nini na jinsi inavyofanya kazi
- Kusakinisha CLI kwenye Windows na macOS
- Kuchunguza mifano - orodha, kupakua, kuendesha
- Kuelewa majina ya kifupi ya mifano na bandari zinazobadilika

---

### Sehemu ya 2: Ukingo wa SDK wa Foundry Local

**Mwongozo wa maabara:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Kwa nini kutumia SDK badala ya CLI kwa maendeleo ya programu
- Marejeleo kamili ya API ya SDK kwa Python, JavaScript, na C#
- Usimamizi wa huduma, kuvinjari orodha, mizunguko ya maisha ya mfano (pakua, chomeka, toa)
- Mifumo ya kuanza haraka: kujenga Python, JavaScript `init()`, C# `CreateAsync()`
- Metadata ya `FoundryModelInfo`, majina ya kifupi, na mchaguzi wa modeli bora kwa vifaa

---

### Sehemu ya 3: SDK na API

**Mwongozo wa maabara:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Kuunganisha na Foundry Local kutoka Python, JavaScript, na C#
- Kutumia SDK ya Foundry Local kusimamia huduma kwa mpango
- Mkondo wa mazungumzo ya moja kwa moja kwa API inayolingana na OpenAI
- Marejeleo ya mbinu za SDK kwa kila lugha

**Mifano ya Kanuni:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Mazungumzo ya mto wa msingi |
| C# | `csharp/BasicChat.cs` | Mazungumzo ya mto na .NET |
| JavaScript | `javascript/foundry-local.mjs` | Mazungumzo ya mto na Node.js |

---

### Sehemu ya 4: Utoaji wa Maarifa (RAG)

**Mwongozo wa maabara:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG ni nini na kwa nini ni muhimu
- Kujenga msingi wa maarifa ndani ya kumbukumbu
- Urejeshaji wa maneno muhimu kwa alama
- Kuunda maelekezo ya mfumo yaliyo na msingi
- Kuendesha mnyororo kamili wa RAG kwenye kifaa

**Mifano ya Kanuni:**

| Lugha | Faili |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Sehemu ya 5: Kujenga Wakala wa AI

**Mwongozo wa maabara:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Wakafu wa AI ni nini (tofa mtu moja kwa moja wa LLM)
- Mfumo wa ChatAgent na Microsoft Agent Framework
- Maelekezo ya mfumo, utu wa mawakala, na mazungumzo ya mizunguko mingi
- Matokeo yaliyo na muundo (JSON) kutoka kwa mawakala

**Mifano ya Kanuni:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Wakafu mmoja na Agent Framework |
| C# | `csharp/SingleAgent.cs` | Wakafu mmoja (mfumo wa ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Wakafu mmoja (mfumo wa ChatAgent) |

---

### Sehemu ya 6: Mtiririko wa Kazi wa Wakala Wengi

**Mwongozo wa maabara:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Mnyororo wa mawakala wengi: Mtafiti → Mwandishi → Mhariri
- Uratibu wa mfululizo na mizunguko ya maoni
- Usaidizi wa usanidi ulio sambazwa na mawasilisho yaliyo na muundo
- Kubuni mtiririko wako wa mawakala wengi

**Mifano ya Kanuni:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Mnyororo wa mawakala watatu |
| C# | `csharp/MultiAgent.cs` | Mnyororo wa mawakala watatu |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Mnyororo wa mawakala watatu |

---

### Sehemu ya 7: Mwandishi wa Ubunifu wa Zava - Programu ya Capstone

**Mwongozo wa maabara:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Programu ya mtindo wa uzalishaji yenye mawakala 4 maalum
- Mnyororo wa mfululizo na mizunguko ya maoni inayosimamiwa na mtathmini
- Matokeo ya mtiririko, utafutaji wa orodha ya bidhaa, mawasilisho yaliyo na muundo wa JSON
- Utekelezaji kamili kwa Python (FastAPI), JavaScript (Node.js CLI), na C# (konsoli ya .NET)

**Mifano ya Kanuni:**

| Lugha | Saraka | Maelezo |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Huduma ya wavuti ya FastAPI na mratibu |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Programu ya CLI ya Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Programu ya konsoli ya .NET 9 |

---

### Sehemu ya 8: Maendeleo ya Kutegemea Tathmini

**Mwongozo wa maabara:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Jenga mfumo wa tathmini wa mfumo kwa mawakala wa AI kwa kutumia seti za dhahabu
- Ukaguzi unaotegemea sheria (urefu, usambazaji wa maneno muhimu, maneno yasiyoruhusiwa) + alama za LLM kama hakimu
- Ulinganisho sambamba wa aina za maelekezo kwa karatasi za alama jumla
- Inanukuu mfumo wa mfuatiliaji Zava Editor kutoka Sehemu ya 7 hadi jaribio la nje la mtandao
- Njia za Python, JavaScript, na C#

**Mifano ya Kanuni:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Mfumo wa tathmini |
| C# | `csharp/AgentEvaluation.cs` | Mfumo wa tathmini |
| JavaScript | `javascript/foundry-local-eval.mjs` | Mfumo wa tathmini |

---

### Sehemu ya 9: Kutoa Matini kwa Sauti kwa Whisper

**Mwongozo wa maabara:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Ubadilishaji wa hotuba-kwa-maandishi ukitumia OpenAI Whisper unaoendesha kwa ndani
- Usindikaji wa sauti unaolinda faragha - sauti haijiungi na kifaa chako
- Nyimbo za Python, JavaScript, na C# zikiwa na `client.audio.transcriptions.create()` (Python/JS) na `AudioClient.TranscribeAudioAsync()` (C#)
- Inajumuisha faili za sauti za mfano zenye mandhari ya Zava kwa mazoezi ya vitendo

**Mifano ya msimbo:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Ubadilishaji wa sauti kwa kutumia Whisper |
| C# | `csharp/WhisperTranscription.cs` | Ubadilishaji wa sauti kwa kutumia Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Ubadilishaji wa sauti kwa kutumia Whisper |

> **Kumbuka:** maabara hii inatumia **Foundry Local SDK** kupakua na kupakia mfano wa Whisper kupitia programu, kisha kutuma sauti kwa kituo cha OpenAI kilicho ndani kwa ajili ya ubadilishaji. Mfano wa Whisper (`whisper`) umeorodheshwa kwenye katalo ya Foundry Local na unaendesha kabisa kwenye kifaa chako - hakuna funguo za API za wingu wala upatikanaji wa mtandao unaohitajika.

---

### Sehemu ya 10: Kutumia Mifano Maalum au ya Hugging Face

**Mwongozo wa maabara:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kuunda mifano ya Hugging Face kwa muundo ulioboreshwa wa ONNX kwa kutumia mtengenezaji wa modeli ya ONNX Runtime GenAI
- Uundaji maalum kwa vifaa (CPU, NVIDIA GPU, DirectML, WebGPU) na kupunguza ukubwa (int4, fp16, bf16)
- Kuunda faili za usanidi wa templeti ya mazungumzo kwa Foundry Local
- Kuongeza mifano iliyoundwa kwenye hifadhidata ya Foundry Local
- Kuendesha mifano maalum kupitia CLI, REST API, na OpenAI SDK
- Mfano wa marejeleo: kuunda modeli ya Qwen/Qwen3-0.6B hatua kwa hatua

---

### Sehemu ya 11: Kupiga Simu za Zana Kwa Mifano ya Ndani

**Mwongozo wa maabara:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Kuruhusu mifano ya ndani kupiga simu kwa kazi za nje (kupiga simu kwa zana/kazi)
- Eleza mbinu za zana kwa kutumia muundo wa kupiga simu wa kazi wa OpenAI
- Simamia mzungumzo wa multi-turn wa kupiga simu kwa zana
- Tekeleza simu za zana ndani na rudisha matokeo kwa mfano
- Chagua mfano sahihi kwa hali za kupiga simu kwa zana (Qwen 2.5, Phi-4-mini)
- Tumia `ChatClient` ya asili ya SDK kwa kupiga simu za zana (JavaScript)

**Mifano ya msimbo:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Kupiga simu kwa zana za hali ya hewa/idadi ya watu |
| C# | `csharp/ToolCalling.cs` | Kupiga simu kwa zana na .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Kupiga simu kwa zana na ChatClient |

---

### Sehemu ya 12: Kujenga UI ya Wavuti kwa Mwandishi wa Ubunifu wa Zava

**Mwongozo wa maabara:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Ongeza sehemu ya mbele ya kivinjari kwa Mwandishi wa Ubunifu wa Zava
- Tumikia UI iliyoshirikiwa kutoka Python (FastAPI), JavaScript (Node.js HTTP), na C# (ASP.NET Core)
- Tumia usambazaji wa NDJSON kwenye kivinjari kwa kutumia Fetch API na ReadableStream
- Bango la hali ya wakala hai na usambazaji wa maandishi wa makala kwa wakati halisi

**Msimbo (UI iliyoshirikiwa):**

| Faili | Maelezo |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Mpangilio wa ukurasa |
| `zava-creative-writer-local/ui/style.css` | Mtindo |
| `zava-creative-writer-local/ui/app.js` | Msomaji wa mto na mantiki ya sasisho la DOM |

**Kuongeza upande wa nyuma:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Imesasishwa kuhudumia UI ya statiki |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Seva mpya ya HTTP inayozunguka mratibu |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Mradi mpya wa API ndogo ya ASP.NET Core |

---

### Sehemu ya 13: Warsha Imeisha

**Mwongozo wa maabara:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Muhtasari wa kila kitu uliojenga katika sehemu zote 12
- Mibono zaidi ya kuongeza programu zako
- Viungo vya rasilimali na nyaraka

---

## Muundo wa Mradi

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

## Rasilimali

| Rasilimali | Kiungo |
|----------|------|
| Tovuti ya Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalogi ya modeli | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Mwongozo wa kuanza | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Marejeleo ya Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Mfumo wa Wakala wa Microsoft | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Leseni

Nyenzo za warsha hii zinatolewa kwa madhumuni ya elimu.

---

**Jenga kwa furaha! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Tangazo la Kukana**:
Hati hii imetafsiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Wakati tunajitahidi kwa usahihi, tafadhali fahamu kuwa tafsiri za moja kwa moja zinaweza kuwa na makosa au upotoshaji. Hati ya asili katika lugha yake ya asili inapaswa kuchukuliwa kama chanzo cha mamlaka. Kwa taarifa muhimu, tafsiri ya kitaalamu inayotolewa na binadamu inapendekezwa. Hatuna dhamana kwa maelewano potofu au tafsiri zisizo sahihi zinazotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->