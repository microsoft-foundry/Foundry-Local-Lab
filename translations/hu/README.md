<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - AI Alkalmazások Készítése Eszközön

Egy gyakorlati workshop, amely során saját gépeden futtathatsz nyelvi modelleket, és intelligens alkalmazásokat építhetsz a [Foundry Local](https://foundrylocal.ai) és a [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) segítségével.

> **Mi az a Foundry Local?** A Foundry Local egy könnyű futtatókörnyezet, amely lehetővé teszi nyelvi modellek letöltését, kezelését és kiszolgálását teljes mértékben a saját hardvereden. Egy **OpenAI-kompatibilis API-t** kínál, így bármely eszköz vagy SDK, amely OpenAI-nyelven kommunikál, csatlakozhat – nem szükséges felhőfiók.

### 🌐 Többnyelvű Támogatás

#### GitHub Action segítségével támogatott (Automatizált & Mindig friss)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[arab](../ar/README.md) | [bengáli](../bn/README.md) | [bolgár](../bg/README.md) | [burmai (Myanmar)](../my/README.md) | [kinai (egyszerűsített)](../zh-CN/README.md) | [kinai (hagyományos, Hong Kong)](../zh-HK/README.md) | [kinai (hagyományos, Macau)](../zh-MO/README.md) | [kinai (hagyományos, Taiwan)](../zh-TW/README.md) | [horvát](../hr/README.md) | [cseh](../cs/README.md) | [dán](../da/README.md) | [holland](../nl/README.md) | [észt](../et/README.md) | [finn](../fi/README.md) | [francia](../fr/README.md) | [német](../de/README.md) | [görög](../el/README.md) | [héber](../he/README.md) | [hindi](../hi/README.md) | [magyar](./README.md) | [indonéz](../id/README.md) | [olasz](../it/README.md) | [japán](../ja/README.md) | [kannada](../kn/README.md) | [khmer](../km/README.md) | [koreai](../ko/README.md) | [litván](../lt/README.md) | [maláj](../ms/README.md) | [malayalam](../ml/README.md) | [marathi](../mr/README.md) | [nepáli](../ne/README.md) | [nigériai pidgin](../pcm/README.md) | [norvég](../no/README.md) | [perzsa (fárszi)](../fa/README.md) | [lengyel](../pl/README.md) | [portugál (Brazília)](../pt-BR/README.md) | [portugál (Portugália)](../pt-PT/README.md) | [pandzsábi (Gurmukhi)](../pa/README.md) | [román](../ro/README.md) | [orosz](../ru/README.md) | [szerb (cirill)](../sr/README.md) | [szlovák](../sk/README.md) | [szlovén](../sl/README.md) | [spanyol](../es/README.md) | [szwahili](../sw/README.md) | [svéd](../sv/README.md) | [tagalog (filippínó)](../tl/README.md) | [tamil](../ta/README.md) | [telugu](../te/README.md) | [thai](../th/README.md) | [török](../tr/README.md) | [ukrán](../uk/README.md) | [urdú](../ur/README.md) | [vietnami](../vi/README.md)

> **Inkább helyben klónoznád?**
>
> Ez a tároló 50+ nyelvi fordítást tartalmaz, ami jelentősen növeli a letöltési méretet. Fordítások nélkül klónozáshoz használd a spars checkout-ot:
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
> Így minden szükséges fájlt megkapsz a tanfolyam elvégzéséhez, sokkal gyorsabb letöltéssel.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Tanulási célok

A workshop végére képes leszel:

| # | Célkitűzés |
|---|------------|
| 1 | Telepíteni a Foundry Local-t és kezelésre használni a CLI-t |
| 2 | Mesteri szintű Foundry Local SDK API használat programozott modellkezeléshez |
| 3 | Csatlakozni a helyi inferencia szerverhez Python, JavaScript és C# SDK-kon keresztül |
| 4 | Összeállítani egy Retrieval-Augmented Generation (RAG) csövet, ami saját adatokra alapozza a válaszokat |
| 5 | AI ügynököket létrehozni tartós instrukciókkal és személyiségekkel |
| 6 | Többügynökös munkafolyamatokat koordinálni visszacsatolási ciklusokkal |
| 7 | Megismerni egy termelési végpontú alkalmazást – a Zava Creative Writer-t |
| 8 | Értékelési keretrendszereket építeni aranykészletekkel és LLM-értékelő pontozással |
| 9 | Hangot átírni Whisper-rel – helyi beszédszöveg átalakítás a Foundry Local SDK-val |
| 10 | Egyedi vagy Hugging Face modelleket fordítani és futtatni ONNX Runtime GenAI-vel és Foundry Local-lal |
| 11 | Engedélyezni a helyi modelleknek külső függvények hívását az eszközhívás minta segítségével |
| 12 | Böngészőalapú UI-t építeni a Zava Creative Writerhez valós idejű adatfolyammal |

---

## Előfeltételek

| Követelmény | Részletek |
|-------------|-----------|
| **Hardver** | Minimum 8 GB RAM (ajánlott 16 GB); AVX2-kompatibilis CPU vagy támogatott GPU |
| **Operációs rendszer** | Windows 10/11 (x64/ARM), Windows Server 2025 vagy macOS 13+ |
| **Foundry Local CLI** | Telepíthető `winget install Microsoft.FoundryLocal` (Windows) vagy `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) parancsokkal. Részletek a [kezdő útmutatóban](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Nyelvi futtatókörnyezet** | **Python 3.9+** és/vagy **.NET 9.0+** és/vagy **Node.js 18+** |
| **Git** | A tároló klónozásához |

---

## Kezdés

```bash
# 1. Klónozd a tárat
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Ellenőrizd, hogy a Foundry Local telepítve van-e
foundry model list              # Elérhető modellek listázása
foundry model run phi-3.5-mini  # Interaktív beszélgetés indítása

# 3. Válaszd ki a nyelvi sávodat (a teljes beállításhoz lásd a 2. részt)
```

| Nyelv | Gyors kezdés |
|-------|--------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop részei

### 1. rész: Kezdés a Foundry Local-lal

**Labor útmutató:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Mi az a Foundry Local és hogyan működik
- CLI telepítése Windows és macOS rendszereken
- Modellek felfedezése – listázás, letöltés, futtatás
- Modell aliasok és dinamikus portok megértése

---

### 2. rész: Mélyreható Foundry Local SDK

**Labor útmutató:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Miért használjuk az SDK-t a CLI helyett alkalmazásfejlesztéshez
- Teljes SDK API referencia Python, JavaScript és C# nyelveken
- Szolgáltatáskezelés, katalógus böngészés, modell életciklus (letöltés, betöltés, kirakás)
- Gyorsindítási minták: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metaadatok, aliasok és hardverilletékes modellválasztás

---

### 3. rész: SDK-k és API-k

**Labor útmutató:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Csatlakozás a Foundry Localhoz Python, JavaScript és C# segítségével
- A Foundry Local SDK használata a szolgáltatás programozott kezeléséhez
- Streaming chat befejezések az OpenAI-kompatibilis API-n keresztül
- Módszerreferencia minden nyelvhez

**Kódpéldák:**

| Nyelv | Fájl | Leírás |
|-------|-------|---------|
| Python | `python/foundry-local.py` | Alap streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat .NET-tel |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat Node.js-sel |

---

### 4. rész: Retrieval-Augmented Generation (RAG)

**Labor útmutató:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Mi az a RAG és miért fontos
- Memóriabeli tudásbázis építése
- Kulcsszó-átfedés alapú lekérdezés pontozással
- Alapozott rendszerüzenetek alkotása
- Teljes RAG cső futtatása az eszközön

**Kódpéldák:**

| Nyelv | Fájl |
|-------|-------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 5. rész: AI Ügynökök Készítése

**Labor útmutató:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Mi az az AI ügynök (szemben egy nyers LLM hívással)
- A `ChatAgent` minta és a Microsoft Agent Framework
- Rendszerutasítások, személyiségek és többszörös fordulós beszélgetések
- Strukturált kimenet (JSON) az ügynököktől

**Kódpéldák:**

| Nyelv | Fájl | Leírás |
|-------|-------|---------|
| Python | `python/foundry-local-with-agf.py` | Egyedi ügynök Agent Framework-kel |
| C# | `csharp/SingleAgent.cs` | Egyedi ügynök (ChatAgent minta) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Egyedi ügynök (ChatAgent minta) |

---

### 6. rész: Többügynökös Munkafolyamatok

**Labor útmutató:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Többügynökös csövek: Kutató → Író → Szerkesztő
- Szekvenciális koordináció és visszacsatolási hurkok
- Megosztott konfiguráció és strukturált átadások
- Saját többügynökös munkafolyamat megtervezése

**Kódpéldák:**

| Nyelv | Fájl | Leírás |
|-------|-------|---------|
| Python | `python/foundry-local-multi-agent.py` | Háromügynökös cső |
| C# | `csharp/MultiAgent.cs` | Háromügynökös cső |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Háromügynökös cső |

---

### 7. rész: Zava Creative Writer - Záróalkalmazás

**Labor útmutató:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Termelési stílusú többügynökös alkalmazás 4 specializált ügynökkel
- Szekvenciális cső értékelő vezérelt visszacsatolási hurkokkal
- Streaming kimenet, termékkatalógus keresés, strukturált JSON átadások
- Teljes implementáció Pythonban (FastAPI), JavaScriptben (Node.js CLI) és C#-ban (.NET konzol)

**Kódpéldák:**

| Nyelv | Könyvtár | Leírás |
|-------|----------|---------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webszolgáltatás orchestratorral |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI alkalmazás |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzol alkalmazás |

---

### 8. rész: Értékelés-vezérelt Fejlesztés

**Labor útmutató:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Rendszeres értékelési keretrendszer építése AI ügynökök számára aranykészletekkel
- Szabályalapú ellenőrzések (hossz, kulcsszó lefedettség, tiltott kifejezések) + LLM-értékelő pontozás
- Azonnali összehasonlítás promptváltozatokról összesített pontokkal
- Kiterjeszti a 7. rész Zava Editor ügynök mintáját offline tesztcsomaggá
- Python, JavaScript és C# nyelvű útvonalak

**Kódpéldák:**

| Nyelv | Fájl | Leírás |
|-------|-------|---------|
| Python | `python/foundry-local-eval.py` | Értékelési keretrendszer |
| C# | `csharp/AgentEvaluation.cs` | Értékelési keretrendszer |
| JavaScript | `javascript/foundry-local-eval.mjs` | Értékelési keretrendszer |

---

### 9. rész: Hangátírás Whisper-rel

**Labor útmutató:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Beszéd szöveggé alakítása OpenAI Whisper lokális futtatásával
- Adatvédelmet előtérbe helyező hangfeldolgozás – a hanganyag soha nem hagyja el az eszközödet
- Python, JavaScript és C# példák a `client.audio.transcriptions.create()` (Python/JS) és `AudioClient.TranscribeAudioAsync()` (C#) használatával
- Zava témájú minta hangfájlok a gyakorláshoz

**Kódminták:**

| Nyelv | Fájl | Leírás |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper hangfelismerés |
| C# | `csharp/WhisperTranscription.cs` | Whisper hangfelismerés |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper hangfelismerés |

> **Megjegyzés:** Ez a labor a **Foundry Local SDK**-t használja, amely programozottan tölti le és tölti be a Whisper modellt, majd a hanganyagot a helyi, OpenAI-kompatibilis végpontra küldi át szövegfelismerés céljából. A Whisper modell (`whisper`) megtalálható a Foundry Local katalógusban, és teljes egészében az eszközön fut – nincs szükség felhő API kulcsokra vagy hálózati kapcsolatra.

---

### 10. rész: Egyéni vagy Hugging Face modellek használata

**Labor útmutató:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face modellek optimalizált ONNX formátumba fordítása az ONNX Runtime GenAI modellkészítő eszközével
- Hardverspecifikus fordítás (CPU, NVIDIA GPU, DirectML, WebGPU) és kvantálás (int4, fp16, bf16)
- Chat-sablon konfigurációs fájlok létrehozása a Foundry Local számára
- Fordított modellek hozzáadása a Foundry Local gyorsítótárához
- Egyéni modellek futtatása CLI-n, REST API-n és az OpenAI SDK-n keresztül
- Referencia példa: Qwen/Qwen3-0.6B teljes körű fordítása

---

### 11. rész: Eszközhívás helyi modellekkel

**Labor útmutató:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Helyi modellek engedélyezése külső funkciók (eszköz/funkcióhívás) meghívására
- Eszközsémák definiálása az OpenAI funkcióhívás formátumban
- Többfordulós eszközhívó párbeszéd kezelése
- Eszközhívások helyi végrehajtása és eredmények visszaküldése a modellnek
- Megfelelő modell kiválasztása eszközhívási forgatókönyvekhez (Qwen 2.5, Phi-4-mini)
- SDK natív `ChatClient` használata eszközhíváshoz (JavaScript)

**Kódminták:**

| Nyelv | Fájl | Leírás |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Eszközhívás időjárás/népesség eszközökkel |
| C# | `csharp/ToolCalling.cs` | Eszközhívás .NET-tel |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Eszközhívás ChatClienttel |

---

### 12. rész: Webes felület építése a Zava Kreatív Íróhoz

**Labor útmutató:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Böngészőalapú felület hozzáadása a Zava Kreatív Íróhoz
- Megosztott UI kiszolgálása Pythonból (FastAPI), JavaScriptből (Node.js HTTP) és C#-ból (ASP.NET Core)
- Streaming NDJSON feldolgozása böngészőben Fetch API és ReadableStream segítségével
- Élő ügynök státuszjelzők és valós idejű cikk szöveg streamelés

**Kód (megosztott UI):**

| Fájl | Leírás |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Oldal elrendezés |
| `zava-creative-writer-local/ui/style.css` | Stíluslap |
| `zava-creative-writer-local/ui/app.js` | Stream olvasó és DOM frissítő logika |

**Backend bővítések:**

| Nyelv | Fájl | Leírás |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Frissítve a statikus UI kiszolgálására |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Új HTTP szerver az orchestrator köré építve |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Új ASP.NET Core minimális API projekt |

---

### 13. rész: Műhelymunka befejezése

**Labor útmutató:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Összefoglaló mindenről, amit a 12 rész során építettél
- További ötletek alkalmazásaid bővítéséhez
- Linkek forrásokra és dokumentációra

---

## Projekt struktúra

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

## Források

| Forrás | Link |
|----------|------|
| Foundry Local weboldal | [foundrylocal.ai](https://foundrylocal.ai) |
| Modell katalógus | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Kezdő útmutató | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referencia | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licenc

Ez a műhelyanyag oktatási célokat szolgál.

---

**Jó munkát kívánunk! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Jogi nyilatkozat**:  
Ez a dokumentum az [Co-op Translator](https://github.com/Azure/co-op-translator) AI fordítószolgáltatás segítségével készült. Bár a pontosságra törekszünk, kérjük, vegye figyelembe, hogy az automatikus fordítások hibákat vagy pontatlanságokat tartalmazhatnak. Az eredeti dokumentum az anyanyelvén tekintendő hiteles forrásnak. Kritikus információk esetén professzionális, emberi fordítás ajánlott. Nem vállalunk felelősséget a fordítás használatából eredő félreértésekért vagy téves értelmezésekért.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->