<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local -työpaja — Rakenna tekoälysovelluksia laitteella

Käytännön työpaja, jossa ajetaan kielimalleja omalla koneella ja rakennetaan älykkäitä sovelluksia [Foundry Local](https://foundrylocal.ai) ja [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) avulla.

> **Mikä on Foundry Local?** Foundry Local on kevyt suoritusympäristö, jonka avulla voit ladata, hallita ja jakaa kielimalleja täysin omalla laitteistollasi. Se tarjoaa **OpenAI-yhteensopivan API:n**, joten mikä tahansa työkalu tai SDK, joka käyttää OpenAI-yhteyttä, voi yhdistää — pilvitiliä ei tarvita.

---

## Oppimistavoitteet

Tämän työpajan lopussa osaat:

| # | Tavoite |
|---|-----------|
| 1 | Asentaa Foundry Local ja hallita malleja CLI:n avulla |
| 2 | Hallita Foundry Local SDK:n API:ta ohjelmalliseen mallien hallintaan |
| 3 | Yhdistää paikalliseen inferenssipalvelimeen Python-, JavaScript- ja C#-SDK:illa |
| 4 | Rakentaa Retrieval-Augmented Generation (RAG) -ketjun, joka perustaa vastaukset omiin tietoihisi |
| 5 | Luoda tekoälyagentteja, joilla on pysyvät ohjeistukset ja persoonat |
| 6 | Orkestroida monen agentin työnkulkuja palautekierroksilla |
| 7 | Tutkia tuotantotason loppuharjoitussovellus — Zava Creative Writer |
| 8 | Rakentaa arviointikehykset kultaisia datasettejä ja LLM-tuomarointipisteytystä käyttäen |
| 9 | Tekstittää ääntä Whisperillä — puhe tekstiksi laitteella Foundry Local -SDK:n avulla |
| 10 | Kääntää ja ajaa omia tai Hugging Face -malleja ONNX Runtime GenAI:n ja Foundry Localin avulla |
| 11 | Mahdollistaa paikallisten mallien kutsua ulkoisia funktioita työkalukutsumallin avulla |
| 12 | Rakentaa selaimessa toimiva käyttöliittymä Zava Creative Writerille reaaliaikaisella suoratoistolla |

---

## Edellytykset

| Vaatimus | Tiedot |
|-------------|---------|
| **Laitteisto** | Vähintään 8 GB RAM (16 GB suositeltu); AVX2-yhteensopiva suoritin tai tuettu näytönohjain |
| **Käyttöjärjestelmä** | Windows 10/11 (x64/ARM), Windows Server 2025 tai macOS 13+ |
| **Foundry Local CLI** | Asenna komennolla `winget install Microsoft.FoundryLocal` (Windows) tai `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Katso [aloitusopas](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) lisätietoja varten. |
| **Ohjelmointiruntime** | **Python 3.9+** ja/tai **.NET 9.0+** ja/tai **Node.js 18+** |
| **Git** | Tätä arkistoa varten kloonaamiseen |

---

## Aloittaminen

```bash
# 1. Kloonaa repositorio
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Varmista, että Foundry Local on asennettu
foundry model list              # Listaa saatavilla olevat mallit
foundry model run phi-3.5-mini  # Käynnistä vuorovaikutteinen keskustelu

# 3. Valitse kielirata (katso osa 2:n labra täydellistä asennusta varten)
```

| Kieli | Nopea aloitus |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Työpajan osat

### Osa 1: Aloitus Foundry Localin kanssa

**Labraopas:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Mikä on Foundry Local ja miten se toimii
- CLI:n asennus Windowsiin ja macOS:ään
- Mallien tutkiminen — listaus, lataus, ajaminen
- Mallien alias-nimet ja dynaamiset portit

---

### Osa 2: Syvä sukellus Foundry Local SDK:hon

**Labraopas:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Miksi SDK ja CLI eroavat sovelluskehityksessä
- SDK API -viitetiedot Pythonille, JavaScriptille ja C#:lle
- Palvelun hallinta, katalogin selaus, mallin elinkaari (lataus, ottaminen käyttöön, vapauttaminen)
- Nopean aloituksen mallit: Python-konstruktorin käynnistys, JavaScriptin `init()`, C#:n `CreateAsync()`
- `FoundryModelInfo` metadata, aliakset ja laiteresursseihin optimoitu mallin valinta

---

### Osa 3: SDK:t ja API:t

**Labraopas:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Yhteyden muodostaminen Foundry Localiin Pythonilla, JavaScripillä ja C#:lla
- Palvelun ohjelmallinen hallinta Foundry Local SDK:n avulla
- Chat-kompletioiden suoratoisto OpenAI-yhteensopivan API:n kautta
- SDK-metodien viitetiedot kullekin kielelle

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Perus suoratoistettu chat |
| C# | `csharp/BasicChat.cs` | Suoratoistettu chat .NET:llä |
| JavaScript | `javascript/foundry-local.mjs` | Suoratoistettu chat Node.js:llä |

---

### Osa 4: Retrieval-Augmented Generation (RAG)

**Labraopas:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Mikä on RAG ja miksi se on tärkeä
- Muistissa toimivan tietopohjan rakentaminen
- Avainsanojen päällekkäisyyteen perustuva haku ja pisteytys
- Tuetut järjestelmäkehoteet
- Kokonaisen RAG-putken ajaminen laitteella

**Koodiesimerkit:**

| Kieli | Tiedosto |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Osa 5: Tekoälyagenttien rakentaminen

**Labraopas:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Mikä on tekoälyagentti (verrattuna suoraan LLM-kutsuun)
- `ChatAgent`-malli ja Microsoft Agent Framework
- Järjestelmäohjeet, persoonat ja monikierroksiset keskustelut
- Rakenneellinen JSON-vastaus agenteilta

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Yksittäinen agentti Agent Frameworkilla |
| C# | `csharp/SingleAgent.cs` | Yksittäinen agentti (ChatAgent-malli) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Yksittäinen agentti (ChatAgent-malli) |

---

### Osa 6: Monen agentin työnkulut

**Labraopas:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Monen agentin ketjut: Tutkija → Kirjoittaja → Toimittaja
- Peräkkäinen orkestrointi ja palautekierrokset
- Jaettu konfiguraatio ja rakenteelliset siirtotoiminnot
- Oman monen agentin työnkulun suunnittelu

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Kolmen agentin ketju |
| C# | `csharp/MultiAgent.cs` | Kolmen agentin ketju |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Kolmen agentin ketju |

---

### Osa 7: Zava Creative Writer — lopputyösovellus

**Labraopas:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Tuotantotason moniaagenttisovellus neljällä erikoistuneella agentilla
- Peräkkäinen ketju arvioijan ohjaamilla palautekierroksilla
- Suoratoistettu tuloste, tuoteluettelohaku, rakenteelliset JSON-siirrot
- Koko toteutus Pythonilla (FastAPI), JavaScriptillä (Node.js CLI) ja C#:lla (.NET-konsoli)

**Koodiesimerkit:**

| Kieli | Hakemisto | Kuvaus |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI-verkkopalvelu orkestroijalla |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI -sovellus |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsolisovellus |

---

### Osa 8: Arviointiohjattu kehitys

**Labraopas:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Rakentaa järjestelmällinen arviointikehys tekoälyagenteille käyttäen kultaisia datasettejä
- Sääntöpohjaiset tarkistukset (pituus, avainsanojen kattavuus, kielletyt termit) + LLM-tuomarointi
- Vertailu rinnakkain eri kehotevarianttien kanssa yhteenvedolla
- Laajentaa Zava Editor -agenttimallin osiosta 7 offline-testisarjaksi
- Python-, JavaScript- ja C#-reitit

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Arviointikehys |
| C# | `csharp/AgentEvaluation.cs` | Arviointikehys |
| JavaScript | `javascript/foundry-local-eval.mjs` | Arviointikehys |

---

### Osa 9: Puheen tekstitys Whisperillä

**Labraopas:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Puhe tekstiksi Whisperillä paikallisesti
- Yksityisyyttä korostava ääniaineiston käsittely — ääni ei poistu laitteelta
- Python-, JavaScript- ja C#-reitit `client.audio.transcriptions.create()` (Python/JS) ja `AudioClient.TranscribeAudioAsync()` (C#)
- Sisältää Zava-aiheisia esimerkkitiedostoja käytännön harjoitteluun

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper-puheentunnistus |
| C# | `csharp/WhisperTranscription.cs` | Whisper-puheentunnistus |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper-puheentunnistus |

> **Huom:** Tämä labra käyttää **Foundry Local SDK:ta** ohjelmallisesti lataamaan ja lataamaan Whisper-mallin, jonka jälkeen ääni lähetetään paikalliseen OpenAI-yhteensopivaan päätepisteeseen tekstitystä varten. Whisper-malli (`whisper`) on luettelossa Foundry Local -katalogissa ja toimii täysin laitteella — pilvi-API-avaimia tai verkkoyhteyttä ei tarvita.

---

### Osa 10: Oman tai Hugging Face -mallien käyttö

**Labraopas:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face -mallien kokoaminen optimoituun ONNX-muotoon ONNX Runtime GenAI -mallinrakentajalla
- Laiteräätälöity käännös (CPU, NVIDIA GPU, DirectML, WebGPU) ja kvantisointi (int4, fp16, bf16)
- Chat-pohjien konfigurointitiedostojen luonti Foundry Localille
- Käännettyjen mallien lisääminen Foundry Localin välimuistiin
- Käyttö omilla malleilla CLI:n, REST-API:n ja OpenAI SDK:n kautta
- Esimerkkinä Qwen/Qwen3-0.6B:n kokoaminen päästä päähän

---

### Osa 11: Työkalujen kutsuminen paikallisilla malleilla

**Labraopas:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Mahdollista paikallisten mallien kutsua ulkoisia toimintoja (työkalu/toimintakutsut)
- Määrittele työkalukaaviot OpenAI-toimintakutsumallilla
- Käytä monikierroksista keskustelua työkalukutsujen hallintaan
- Kutsu työkalut paikallisesti ja palauta tulokset mallille
- Valitse oikea malli työkalukutsuissa (Qwen 2.5, Phi-4-mini)
- Käytä SDK:n natiivilla `ChatClient`-luokalla työkaluvaatimuksia (JavaScript)

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Sää- ja väestötyökalujen kutsu |
| C# | `csharp/ToolCalling.cs` | Työkalukutsu .NET:llä |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Työkalukutsu ChatClientilla |

---

### Osa 12: Verkkokäyttöliittymän rakentaminen Zava Creative Writerille

**Labraopas:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Lisää selainpohjainen käyttöliittymä Zava Creative Writeriin
- Palvele jaettua käyttöliittymää Pythonilla (FastAPI), JavaScriptillä (Node.js HTTP) ja C#:lla (ASP.NET Core)
- Kuluta suoratoistettua NDJSON:ää selaimessa Fetch API:lla ja ReadableStreamilla
- Livesignaalit agentin tilasta ja reaaliaikainen artikkelitekstin suoratoisto

**Koodi (jaettu UI):**

| Tiedosto | Kuvaus |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sivun asettelu |
| `zava-creative-writer-local/ui/style.css` | Tyylittely |
| `zava-creative-writer-local/ui/app.js` | Virranlukija ja DOM-päivityslogiikka |

**Backend-muutokset:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Päivitetty tarjoamaan staattinen UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Uusi HTTP-palvelin orkestroinnin kääreenä |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Uusi ASP.NET Core minimalistinen API -projekti |

---

### Osa 13: Työpaja valmis
**Laboratorio-opas:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Yhteenveto kaikesta, mitä olet rakentanut kaikissa 12 osassa
- Lisäideoita sovellustesi laajentamiseen
- Linkkejä resursseihin ja dokumentaatioon

---

## Projektin rakenne

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

## Resurssit

| Resurssi | Linkki |
|----------|------|
| Foundry Local -verkkosivusto | [foundrylocal.ai](https://foundrylocal.ai) |
| Malliluettelo | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Aloitusopas | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK -viite | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lisenssi

Tämä työpajamateriaali on tarkoitettu opetuskäyttöön.

---

**Onnea rakentamiseen! 🚀**