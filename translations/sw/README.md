<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Warsha ya Foundry Local - Jenga Programu za AI Ndani ya Kifaa

Warsha ya vitendo ya kuendesha mifano ya lugha kwenye mashine yako mwenyewe na kujenga programu za akili zenye akili kwa kutumia [Foundry Local](https://foundrylocal.ai) na [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Foundry Local ni nini?** Foundry Local ni mazingira mepesi yanayokuwezesha kupakua, kusimamia, na kuendesha mifano ya lugha moja kwa moja kwenye vifaa vyako. Inatoa **API inayofanana na OpenAI** ili zana yoyote au SDK inayozungumza OpenAI iweze kuunganishwa - hakuna akaunti ya wingu inayohitajika.

---

## Malengo ya Kujifunza

Mwisho wa warsha hii utaweza:

| # | Lengo |
|---|-----------|
| 1 | Sakinisha Foundry Local na simamia mifano kupitia CLI |
| 2 | Tambua API ya Foundry Local SDK kwa usimamizi wa mifano kwa programu |
| 3 | Unganisha na seva ya ucheshi ya ndani kwa kutumia SDK za Python, JavaScript, na C# |
| 4 | Jenga bomba la Uzalishaji Kwa Msaada wa Urejeshwaji (RAG) unaosimamia majibu kulingana na data yako mwenyewe |
| 5 | Tengeneza mawakala wa AI wenye maelekezo na tabia za kudumu |
| 6 | Ratibu michakato ya mawakala wengi yenye mizunguko ya maoni |
| 7 | Chunguza programu ya uzalishaji wa mwisho - Mwandishi wa Ubunifu wa Zava |
| 8 | Jenga mifumo ya tathmini kwa kutumia seti za dhahabu na alama za LLM kama mwamuzi |
| 9 | Badilisha sauti kuwa maandishi kwa Whisper - hotuba-kuwa-maandishi mojawapo kwa kutumia Foundry Local SDK |
| 10 | Kisanyi naendeshe mifano maalum au ya Hugging Face kwa ONNX Runtime GenAI na Foundry Local |
| 11 | Ruhusu mifano ya ndani kupiga simu kwa kazi za nje kwa kutumia muundo wa simu za zana |
| 12 | Jenga kiolesura cha kivinjari kwa Mwandishi wa Ubunifu wa Zava na uondoaji wa papo kwa papo |

---

## Mahitaji ya Awali

| Mahitaji | Maelezo |
|-------------|---------|
| **Vifaa** | Kidogo kwa RAM 8 GB (Inapendekezwa 16 GB); CPU yenye uwezo wa AVX2 au GPU inayoungwa mkono |
| **Mfumo wa uendeshaji** | Windows 10/11 (x64/ARM), Windows Server 2025, au macOS 13+ |
| **CLI ya Foundry Local** | Sakinisha kwa `winget install Microsoft.FoundryLocal` (Windows) au `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Tazama [mwongozo wa kuanza](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) kwa maelezo. |
| **Mazingira ya lugha** | **Python 3.9+** na/au **.NET 9.0+** na/au **Node.js 18+** |
| **Git** | Kwa kunakili hazina hii |

---

## Kuanza

```bash
# 1. Nakili hifadhidata
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Thibitisha Foundry Local imewekwa
foundry model list              # Orodhesha mifano iliyopo
foundry model run phi-3.5-mini  # Anzisha mazungumzo ya kuingiliana

# 3. Chagua mwelekeo wa lugha yako (ona maabara ya Sehemu ya 2 kwa usanidi kamili)
```

| Lugha | Anza Haraka |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Sehemu za Warsha

### Sehemu 1: Kuanzishwa na Foundry Local

**Mwongozo wa maabara:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local ni nini na jinsi inavyofanya kazi
- Kuweka CLI kwenye Windows na macOS
- Kuchunguza mifano - orodha, kupakua, kuendesha
- Kuelewa majina mbadala ya mifano na bandari zenye mabadiliko

---

### Sehemu 2: Ufundishaji wa Kina wa Foundry Local SDK

**Mwongozo wa maabara:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Kwa nini kutumia SDK badala ya CLI kwa maendeleo ya programu
- Marejeleo kamili ya API ya SDK kwa Python, JavaScript, na C#
- Usimamizi wa huduma, kuvinjari katalogi, mzunguko wa maisha wa mfano (kupakua, kupakia, kuondoa)
- Mifano ya kuanza haraka: uanzishaji wa muundaji wa Python, `init()` ya JavaScript, `CreateAsync()` ya C#
- Metadata ya `FoundryModelInfo`, majina mbadala, na uchaguzi wa mfano bora kwa vifaa

---

### Sehemu 3: SDK na API

**Mwongozo wa maabara:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Kuunganishwa na Foundry Local kutoka Python, JavaScript, na C#
- Kutumia Foundry Local SDK kusimamia huduma kwa njia ya programu
- Uondoaji wa mazungumzo kupitia API inayofanana na OpenAI
- Marejeleo ya njia za SDK kwa kila lugha

**Mifano ya Msimbo:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Mazungumzo ya msingi yanayoendelea |
| C# | `csharp/BasicChat.cs` | Mazungumzo yanayoendelea na .NET |
| JavaScript | `javascript/foundry-local.mjs` | Mazungumzo yanayoendelea na Node.js |

---

### Sehemu 4: Uzalishaji Kwa Msaada wa Urejeshwaji (RAG)

**Mwongozo wa maabara:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG ni nini na kwa nini ni muhimu
- Kujenga hifadhidata ya maarifa ya kumbukumbu
- Urejeshaji wa maneno muhimu na alama
- Kuunda maelekezo ya mfumo yenye msingi wa taarifa
- Kuendesha bomba kamili la RAG ndani ya kifaa

**Mifano ya Msimbo:**

| Lugha | Faili |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Sehemu 5: Kujenga Wakala wa AI

**Mwongozo wa maabara:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Wakili wa AI ni nini (kulinganisha na wito rahisi kwa LLM)
- Mfano wa `ChatAgent` na Microsoft Agent Framework
- Maelekezo ya mfumo, tabia, na mazungumzo ya mzunguko nyingi
- Toleo lililopangwa (JSON) kutoka kwa mawakala

**Mifano ya Msimbo:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Wakili mmoja na Agent Framework |
| C# | `csharp/SingleAgent.cs` | Wakili mmoja (mfano wa ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Wakili mmoja (mfano wa ChatAgent) |

---

### Sehemu 6: Michakato ya Wakala Wengi

**Mwongozo wa maabara:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Mifumo ya mawakala wengi: Mtafiti → Mwandishi → Mhariri
- Ratiba ya mfululizo na mizunguko ya maoni
- Mipangilio ya pamoja na mikatanisho iliyo pangiliwa
- Ubunifu wa mfumo wako wa kazi wa mawakala wengi

**Mifano ya Msimbo:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Bomba la mawakala watatu |
| C# | `csharp/MultiAgent.cs` | Bomba la mawakala watatu |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Bomba la mawakala watatu |

---

### Sehemu 7: Mwandishi wa Ubunifu wa Zava - Programu ya Mwisho

**Mwongozo wa maabara:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Programu yenye mawakala wengi wa kitaalamu wanne
- Bomba la mfululizo lenye mizunguko ya maoni inayosimamiwa na mtaalam wa tathmini
- Toleo la papo kwa papo, utafutaji wa katalogi ya bidhaa, mikatanisho ya JSON iliyopangwa
- Utekelezaji kamili katika Python (FastAPI), JavaScript (CLI ya Node.js), na C# (konsoli ya .NET)

**Mifano ya Msimbo:**

| Lugha | Saraka | Maelezo |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Huduma ya wavuti ya FastAPI na mratibu |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Programu ya CLI ya Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Programu ya konsoli ya .NET 9 |

---

### Sehemu 8: Maendeleo Yanayoongozwa na Tathmini

**Mwongozo wa maabara:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Jenga mfumo wa tathmini wa kitaalamu kwa mawakala wa AI kwa kutumia seti za dhahabu
- Ukaguzi wa sheria (urefu, kifunika maneno, maneno yasiyoruhusiwa) + alama za LLM kama mwamuzi
- Ulinganifu pambano wa aina za kuamsha kwa alama za jumla
- Inapanua mfano wa mhariri wa Zava kutoka Sehemu ya 7 kuwa kundi la majaribio ya nje ya mtandao
- Njia kwa Python, JavaScript, na C#

**Mifano ya Msimbo:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Mfumo wa tathmini |
| C# | `csharp/AgentEvaluation.cs` | Mfumo wa tathmini |
| JavaScript | `javascript/foundry-local-eval.mjs` | Mfumo wa tathmini |

---

### Sehemu 9: Uandikishaji wa Sauti kwa Whisper

**Mwongozo wa maabara:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Uandikishaji wa hotuba-kuwa-maandishi ukitumia OpenAI Whisper unaoendesha kwa ndani
- Uendeshaji wa sauti uliojitahidi kuthamini faragha - sauti haiondoki kwenye kifaa
- Njia za Python, JavaScript, na C# na `client.audio.transcriptions.create()` (Python/JS) na `AudioClient.TranscribeAudioAsync()` (C#)
- Inajumuisha faili za sauti mfano kwa mazoezi ya vitendo kwa mada ya Zava

**Mifano ya Msimbo:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Uandikishaji wa sauti wa Whisper |
| C# | `csharp/WhisperTranscription.cs` | Uandikishaji wa sauti wa Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Uandikishaji wa sauti wa Whisper |

> **Kumbuka:** Maabara hii inatumia **Foundry Local SDK** kupakua na kupakia mfano wa Whisper kwa njia ya programu, halafu kutuma sauti kwenye sehemu ya ndani ya API inayofanana na OpenAI kwa ajili ya uandikishaji. Mfano wa Whisper (`whisper`) umeorodheshwa katika katalogi ya Foundry Local na unafanya kazi kabisa ndani ya kifaa - hakuna funguo za API za wingu au upatikanaji wa mtandao unahitajika.

---

### Sehemu 10: Kutumia Mifano Maalum au ya Hugging Face

**Mwongozo wa maabara:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kusanyiko wa mifano ya Hugging Face hadi muundo bora wa ONNX kwa kutumia kivundikaji cha mifano cha ONNX Runtime GenAI
- Uundaji maalum kwa vifaa (CPU, GPU ya NVIDIA, DirectML, WebGPU) na upimaji (int4, fp16, bf16)
- Kuunda faili za usanidi wa template ya mazungumzo kwa Foundry Local
- Kuongeza mifano iliyosanyikwa kwenye cache ya Foundry Local
- Kuendesha mifano maalum kupitia CLI, REST API, na OpenAI SDK
- Mfano wa rejea: kusanyiko kamili wa Qwen/Qwen3-0.6B kutoka mwanzo hadi mwisho

---

### Sehemu 11: Kupiga Simu za Zana kwa Mifano ya Ndani

**Mwongozo wa maabara:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Ruhusu mifano ya ndani kupiga simu kwa kazi za nje (kupiga simu za zana/funcksheni)
- Eleza taswira za zana kwa kutumia muundo wa simu ya OpenAI
- Dhibiti mzunguko wa mazungumzo ya simu za zana mzunguko nyingi
- Endesha simu za zana ndani na rudisha matokeo kwa mfano
- Chagua mfano unaofaa kwa mazingira ya simu za zana (Qwen 2.5, Phi-4-mini)
- Tumia `ChatClient` ya asili ya SDK kwa simu za zana (JavaScript)

**Mifano ya Msimbo:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Kupiga simu za zana za hali ya hewa/sasa watu |
| C# | `csharp/ToolCalling.cs` | Kupiga simu za zana na .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Kupiga simu za zana na ChatClient |

---

### Sehemu 12: Kujenga UI ya Wavuti kwa Mwandishi wa Ubunifu wa Zava

**Mwongozo wa maabara:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Ongeza sehemu ya mbele ya kivinjari kwa Mwandishi wa Ubunifu wa Zava
- Hutumie UI iliyoshirikiwa kutoka Python (FastAPI), JavaScript (HTTP ya Node.js), na C# (ASP.NET Core)
- Tumia mtiririko wa NDJSON katika kivinjari kwa kutumia API ya Fetch na ReadableStream
- Bango la hali ya wakala wa moja kwa moja na uondoaji wa maandishi ya makala kwa wakati halisi

**Msimbo (UI iliyoshirikiwa):**

| Faili | Maelezo |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Mpangilio wa ukurasa |
| `zava-creative-writer-local/ui/style.css` | Mtindo |
| `zava-creative-writer-local/ui/app.js` | Msomaji wa mtiririko na mantiki ya sasisho la DOM |

**Maboresho ya nyuma:**

| Lugha | Faili | Maelezo |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Imesasishwa kuhudumia UI takatifu |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Seva mpya ya HTTP inayozunguka mratibu |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Mradi mpya wa API ndogo ya ASP.NET Core |

---

### Sehemu 13: Warsha Imekamilika
**Mwongozo wa maabara:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Muhtasari wa yote uliyoyajenga katika sehemu zote 12
- Mawazo zaidi ya kuongeza matumizi yako
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
| Katalogi ya mfano | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Mwongozo wa kuanza | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Marejeleo ya SDK ya Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Mfumo wa Mawakala wa Microsoft | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Leseni

Nyenzo hii ya warsha imetolewa kwa madhumuni ya kielimu.

---

**Jenga kwa furaha! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Kielekezo cha Hukumu**:  
Hati hii imetafsiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Wakati tunajitahidi usahihi, tafadhali fahamu kwamba tafsiri za moja kwa moja zinaweza kuwa na makosa au upungufu. Hati ya awali katika lugha yake asili inapaswa kuzingatiwa kama chanzo cha mamlaka. Kwa taarifa muhimu, inashauriwa kutumia tafsiri ya kitaalamu inayofanywa na watu. Hatubeba dhamana yoyote kwa kuelewa vibaya au tafsiri potofu zinazotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->