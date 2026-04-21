<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Műhely - AI Alkalmazások Készítése Eszközön

Egy gyakorlati műhely nyelvi modellek futtatásához a saját gépeden, és intelligens alkalmazások építéséhez a [Foundry Local](https://foundrylocal.ai) és a [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) segítségével.

> **Mi az a Foundry Local?** A Foundry Local egy könnyű futtatókörnyezet, amely lehetővé teszi, hogy nyelvi modelleket tölts le, kezelj és szolgálj ki teljes egészében a saját hardvereden. Egy **OpenAI-kompatibilis API-t** tesz elérhetővé, így bármely olyan eszköz vagy SDK, amely beszél OpenAI nyelven, tud kapcsolódni – felhőfiók nem szükséges.

---

## Tanulási célok

A műhely végére képes leszel:

| # | Célkitűzés |
|---|------------|
| 1 | Telepíteni a Foundry Local-t és modelleket kezelni a CLI-val |
| 2 | Mesteri szintű Foundry Local SDK API használat programozott modellezéshez |
| 3 | Kapcsolódni a helyi inferencia szerverhez a Python, JavaScript és C# SDK-kkal |
| 4 | Építeni egy Kikeresésen Alapuló Generálási (RAG) folyamatot, amely a válaszokat a saját adataidra alapozza |
| 5 | Létrehozni AI ügynököket kitartó utasításokkal és személyiségekkel |
| 6 | Többügynökös munkafolyamatokat irányítani visszacsatolásokkal |
| 7 | Felfedezni egy éles végponti alkalmazást - a Zava Kreatív Írót |
| 8 | Értékelő keretrendszert építeni arany adatállományokkal és LLM-bíró általi pontozással |
| 9 | Hangot átírni Whisper-rel - beszédről szövegre eszközön belül a Foundry Local SDK-val |
| 10 | Fordítani és futtatni egyedi vagy Hugging Face modelleket ONNX Runtime GenAI és Foundry Local segítségével |
| 11 | Lehetővé tenni, hogy a helyi modellek külső funkciókat hívjanak a tool-calling mintával |
| 12 | Böngésző-alapú felhasználói felületet építeni a Zava Kreatív Íróhoz valós idejű streameléssel |

---

## Előfeltételek

| Követelmény | Részletek |
|-------------|-----------|
| **Hardver** | Minimum 8 GB RAM (ajánlott 16 GB); AVX2-t támogató CPU vagy támogatott GPU |
| **Operációs rendszer** | Windows 10/11 (x64/ARM), Windows Server 2025 vagy macOS 13+ |
| **Foundry Local CLI** | Telepítés a `winget install Microsoft.FoundryLocal` (Windows) vagy `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) parancsokkal. Részletekért lásd a [bevezető útmutatót](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Nyelvi futtatókörnyezet** | **Python 3.9+** és/vagy **.NET 9.0+** és/vagy **Node.js 18+** |
| **Git** | A repozitórium klónozásához |

---

## Kezdés

```bash
# 1. Klónozd a tárhelyet
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Ellenőrizd, hogy a Foundry Local telepítve van-e
foundry model list              # Listázd a rendelkezésre álló modelleket
foundry model run phi-3.5-mini  # Indíts interaktív csevegést

# 3. Válaszd ki a nyelvi pályádat (lásd a 2. rész laborját a teljes beállításhoz)
```

| Nyelv | Gyors indulás |
|--------|--------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Műhely részei

### 1. rész: Kezdés a Foundry Local-lel

**Laborvezető:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Mi az a Foundry Local és hogyan működik
- CLI telepítése Windows és macOS rendszeren
- Modellek felfedezése - listázás, letöltés, futtatás
- Modell aliasok és dinamikus portok megértése

---

### 2. rész: Foundry Local SDK mélyreható

**Laborvezető:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Miért érdemes az SDK-t használni a CLI helyett alkalmazásfejlesztéshez
- Teljes SDK API referenciák Pythonhoz, JavaScript-hez és C#-hoz
- Szolgáltatás-kezelés, katalógus böngészés, modell életciklus (letöltés, betöltés, eltávolítás)
- Gyorsindítási minták: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metaadatok, aliasok és hardver-optimalizált modellválasztás

---

### 3. rész: SDK-k és API-k

**Laborvezető:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Kapcsolódás a Foundry Local-hez Pythonból, JavaScriptből és C#-ból
- A Foundry Local SDK használata a szolgáltatás programozott kezelésére
- Streaming chat kiegészítések az OpenAI-kompatibilis API-n keresztül
- SDK metódus referenciák minden nyelvhez

**Kód példák:**

| Nyelv | Fájl | Leírás |
|--------|------|--------|
| Python | `python/foundry-local.py` | Alap streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat .NET-tel |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat Node.js-szel |

---

### 4. rész: Kikeresésen Alapuló Generálás (RAG)

**Laborvezető:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Mi az a RAG és miért fontos
- Memóriabeli tudásbázis építése
- Kulcsszó-átfedéses kikeresés pontozással
- Alapozott rendszer-promtok összeállítása
- Teljes RAG folyamat futtatása eszközön

**Kód példák:**

| Nyelv | Fájl |
|--------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 5. rész: AI Ügynökök építése

**Laborvezető:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Mi az AI ügynök (összehasonlítva a sima LLM hívással)
- A `ChatAgent` minta és a Microsoft Agent Framework
- Rendszerutasítások, személyiségek és többszörös körös beszélgetések
- Strukturált kimenet (JSON) az ügynököktől

**Kód példák:**

| Nyelv | Fájl | Leírás |
|--------|------|--------|
| Python | `python/foundry-local-with-agf.py` | Egyetlen ügynök az Agent Framework-kel |
| C# | `csharp/SingleAgent.cs` | Egyetlen ügynök (ChatAgent minta) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Egyetlen ügynök (ChatAgent minta) |

---

### 6. rész: Többügynökös munkafolyamatok

**Laborvezető:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Többügynökös folyamatok: Kutató → Író → Szerkesztő
- Szekvenciális koordináció és visszacsatolások
- Megosztott konfiguráció és strukturált átadások
- Saját többügynökös munkafolyamat tervezése

**Kód példák:**

| Nyelv | Fájl | Leírás |
|--------|------|--------|
| Python | `python/foundry-local-multi-agent.py` | Három-ügynökös folyamat |
| C# | `csharp/MultiAgent.cs` | Három-ügynökös folyamat |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Három-ügynökös folyamat |

---

### 7. rész: Zava Kreatív Író - Befejező alkalmazás

**Laborvezető:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Egy termelési stílusú többügynökös alkalmazás 4 specializált ügynökkel
- Szekvenciális folyamat vevői visszacsatolással
- Streaming kimenet, termékkatalógus keresés, strukturált JSON átadások
- Teljes megvalósítás Python-ban (FastAPI), JavaScript-ben (Node.js CLI) és C#-ban (.NET konzol)

**Kód példák:**

| Nyelv | Könyvtár | Leírás |
|--------|-----------|--------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webszolgáltatás az irányítóval |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI alkalmazás |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzol alkalmazás |

---

### 8. rész: Értékelés-vezérelt fejlesztés

**Laborvezető:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Rendszeres értékelési keretrendszer építése AI ügynökök számára arany adatállományokkal
- Szabályalapú ellenőrzések (hossz, kulcsszótartalom, tiltott kifejezések) + LLM-bíró pontozás
- Páros összehasonlítás a prompt variánsokról összesített pontozó táblázatokkal
- Kiterjeszti a Part 7-beli Zava Editor ügynököt offline tesztcsomagként
- Python, JavaScript és C# ágak

**Kód példák:**

| Nyelv | Fájl | Leírás |
|--------|------|--------|
| Python | `python/foundry-local-eval.py` | Értékelő keretrendszer |
| C# | `csharp/AgentEvaluation.cs` | Értékelő keretrendszer |
| JavaScript | `javascript/foundry-local-eval.mjs` | Értékelő keretrendszer |

---

### 9. rész: Hangátírás Whisper-rel

**Laborvezető:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Beszéd-szöveg átírás lokálisan futó OpenAI Whisper-rel
- Adatvédelmi szempontból elsődleges hangfeldolgozás – a hanganyag soha nem hagyja el az eszközt
- Python, JavaScript és C# ágak `client.audio.transcriptions.create()` (Python/JS) és `AudioClient.TranscribeAudioAsync()` (C#)
- Zava témájú mintahangfájlokat is tartalmaz gyakorláshoz

**Kód példák:**

| Nyelv | Fájl | Leírás |
|--------|------|--------|
| Python | `python/foundry-local-whisper.py` | Whisper hangátírás |
| C# | `csharp/WhisperTranscription.cs` | Whisper hangátírás |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper hangátírás |

> **Megjegyzés:** Ez a labor a **Foundry Local SDK-t** használja a Whisper modell programozott letöltéséhez és betöltéséhez, majd az audio lokális OpenAI-kompatibilis végpontra küldése átírás céljából. A Whisper modell (`whisper`) megtalálható a Foundry Local katalógusban és teljesen eszközön fut – felhő API kulcs vagy hálózati hozzáférés nincs szükség.

---

### 10. rész: Egyedi vagy Hugging Face modellek használata

**Laborvezető:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face modellek lefordítása optimalizált ONNX formátumba az ONNX Runtime GenAI modellezővel
- Hardver-specifikus fordítás (CPU, NVIDIA GPU, DirectML, WebGPU) és kvantálás (int4, fp16, bf16)
- Chat-template konfigurációs fájlok létrehozása Foundry Local számára
- Fordított modellek hozzáadása a Foundry Local cache-hez
- Egyedi modellek futtatása CLI-vel, REST API-val és OpenAI SDK-val
- Referenciaminta: a Qwen/Qwen3-0.6B végpont előkészítése end-to-end

---

### 11. rész: Eszközhívás helyi modellekkel

**Laborvezető:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Lehetővé tenni, hogy a helyi modellek külső funkciókat hívjanak (eszköz/funkció hívás)
- Eszközsémák definiálása az OpenAI funkcióhívási formátumban
- Többszörös körös eszközhívás beszélgetés kezelése
- Eszközhívások helyi végrehajtása és eredmények visszaadása a modellnek
- Megfelelő modell kiválasztása eszközhívásos esetekhez (Qwen 2.5, Phi-4-mini)
- Az SDK natív `ChatClient` használata eszközhíváshoz (JavaScript)

**Kód példák:**

| Nyelv | Fájl | Leírás |
|--------|------|--------|
| Python | `python/foundry-local-tool-calling.py` | Eszközhívás időjárás/népesség eszközökkel |
| C# | `csharp/ToolCalling.cs` | Eszközhívás .NET-tel |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Eszközhívás ChatClient-tel |

---

### 12. rész: Böngésző-alapú felület a Zava Kreatív Íróhoz

**Laborvezető:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Böngésző-alapú frontend hozzáadása a Zava Kreatív Íróhoz
- Megosztott UI kiszolgálása Pythonból (FastAPI), JavaScript-ből (Node.js HTTP) és C#-ból (ASP.NET Core)
- Streaming NDJSON fogyasztása böngészőben Fetch API és ReadableStream segítségével
- Élő ügynök állapotjelzők és valós idejű cikk szöveg streamelés

**Kód (megosztott UI):**

| Fájl | Leírás |
|-------|---------|
| `zava-creative-writer-local/ui/index.html` | Oldal elrendezés |
| `zava-creative-writer-local/ui/style.css` | Stílusok |
| `zava-creative-writer-local/ui/app.js` | Stream olvasó és DOM frissítő logika |

**Backend kiegészítések:**

| Nyelv | Fájl | Leírás |
|--------|------|--------|
| Python | `zava-creative-writer-local/src/api/main.py` | Frissítve a statikus UI kiszolgálására |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Új HTTP szerver, amely beburkolja az orchestrator-t |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Új ASP.NET Core minimális API projekt |

---

### 13. rész: Műhely befejezve
**Labor útmutató:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Összefoglaló mindarról, amit az összes 12 rész során felépítettél
- További ötletek az alkalmazásaid bővítéséhez
- Linkek erőforrásokhoz és dokumentációhoz

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

## Erőforrások

| Erőforrás | Link |
|----------|------|
| Foundry Local weboldal | [foundrylocal.ai](https://foundrylocal.ai) |
| Modell katalógus | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Kezdő útmutató | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referencia | [Microsoft Learn - SDK Referencia](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licenc

Ez a workshop anyag oktatási célokra készült.

---

**Sikeres építkezést! 🚀**