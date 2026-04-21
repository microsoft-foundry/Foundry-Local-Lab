<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local -työpaja – Rakenna tekoälysovelluksia laitteellasi

Käytännön työpaja kielimallien ajamiseen omassa koneessasi ja älykkäiden sovellusten rakentamiseen [Foundry Localin](https://foundrylocal.ai) ja [Microsoft Agent Frameworkin](https://learn.microsoft.com/en-us/agent-framework/) avulla.

> **Mikä on Foundry Local?** Foundry Local on kevyt käyttöaikaympäristö, jonka avulla voit ladata, hallita ja tarjota kielimalleja kokonaan omalla laitteellasi. Se tarjoaa **OpenAI-yhteensopivan API:n**, joten mikä tahansa OpenAI:ta käyttävä työkalu tai SDK voi muodostaa yhteyden – pilvitiliä ei tarvita.

### 🌐 Monikielinen tuki

#### Tuki GitHub Actionin kautta (automaattinen ja aina ajan tasalla)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](./README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Haluatko mieluummin kloonata paikallisesti?**
>
> Tämä arkisto sisältää yli 50 kielen käännökset, mikä lisää merkittävästi lataustiedoston kokoa. Jos haluat kloonata ilman käännöksiä, käytä sparse checkout -ominaisuutta:
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
> Tämä antaa kaiken tarvittavan kurssin suorittamiseen huomattavasti nopeammalla latauksella.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Oppimistavoitteet

Tämän työpajan lopussa osaat:

| # | Tavoite |
|---|-----------|
| 1 | Asentaa Foundry Local ja hallita malleja komentoriviltä |
| 2 | Hallita Foundry Local SDK:n API:a ohjelmalliseen mallien hallintaan |
| 3 | Yhdistää paikalliseen päättelypalvelimeen Python-, JavaScript- ja C#-SDK:illa |
| 4 | Rakentaa RAG-putkiston (Retrieval-Augmented Generation), joka perustaa vastaukset omiin tietoihisi |
| 5 | Luoda tekoälyagentteja, joilla on pysyvät ohjeet ja persoonat |
| 6 | Orkestroida monien agenttien työnkulkuja palautesilmukoineen |
| 7 | Tutustua tuotantotason päätössovellukseen – Zava Creative Writeriin |
| 8 | Rakentaa arviointikehyksiä kultaiset aineistot ja LLM-tuomarointi-menetelmät hyödyntäen |
| 9 | Tekstittää ääntä Whisperillä – puheesta tekstiksi paikallisesti Foundry Local SDK:n avulla |
| 10 | Koota ja ajaa omia tai Hugging Face -malleja ONNX Runtime GenAI:n ja Foundry Localin avulla |
| 11 | Mahdollistaa paikallisille malleille ulkoisten funktioiden kutsuminen työkalu-kutsumismallilla |
| 12 | Rakentaa selaimessa toimiva käyttöliittymä Zava Creative Writerille reaaliaikaisella suoratoistolla |

---

## Edellytykset

| Vaatimus | Yksityiskohdat |
|-------------|---------|
| **Laitteisto** | Vähintään 8 GB RAM (suositus 16 GB); AVX2-yhteensopiva CPU tai tuettu GPU |
| **Käyttöjärjestelmä** | Windows 10/11 (x64/ARM), Windows Server 2025 tai macOS 13+ |
| **Foundry Local CLI** | Asenna `winget install Microsoft.FoundryLocal` (Windows) tai `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Katso [aloitusopas](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) lisätietoja varten. |
| **Kieliajoalustat** | **Python 3.9+** ja/tai **.NET 9.0+** ja/tai **Node.js 18+** |
| **Git** | Tätä arkistoa varten kloonaamiseen |

---

## Aloitus

```bash
# 1. Kloonaa repositorio
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Varmista, että Foundry Local on asennettu
foundry model list              # Listaa saatavilla olevat mallit
foundry model run phi-3.5-mini  # Aloita interaktiivinen keskustelu

# 3. Valitse kielipolkusi (katso Osa 2 labraa täydelliseen asennukseen)
```

| Kieli | Nopeasti käyntiin |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Työpajan osat

### Osa 1: Aloittaminen Foundry Localin kanssa

**Harjoitusopas:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Mikä on Foundry Local ja miten se toimii
- CLI:n asentaminen Windowsiin ja macOS:ään
- Mallien tutkiminen – listaus, lataus, ajaminen
- Mallien alias-nimien ja dynaamisten porttien ymmärtäminen

---

### Osa 2: Foundry Local SDK:n syväluotaus

**Harjoitusopas:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Miksi SDK:n käyttö on parempi sovelluskehityksessä kuin CLI
- Täydellinen SDK-API-referenssi Pythonille, JavaScriptille ja C#:lle
- Palvelun hallinta, katalogin selaus, mallien elinkaaren hallinta (lataus, kuormaus, purku)
- Nopean aloituksen mallit: Python-konstruktorin alku, JavaScriptin `init()`, C#:n `CreateAsync()`
- `FoundryModelInfo` metatiedot, alias-nimet ja laitteistolle optimaaliset mallivalinnat

---

### Osa 3: SDK:t ja API:t

**Harjoitusopas:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Yhdistäminen Foundry Localiin Pythonilla, JavaScriptillä ja C#:lla
- Foundry Local SDK:n käyttö palvelun ohjelmalliseen hallintaan
- Chat-kompletiot suoratoistona OpenAI-yhteensopivan API:n kautta
- SDK-menetelmäreferenssi jokaiselle kielelle

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Perus chat-suoratoisto |
| C# | `csharp/BasicChat.cs` | Chat-suoratoisto .NET:llä |
| JavaScript | `javascript/foundry-local.mjs` | Chat-suoratoisto Node.js:llä |

---

### Osa 4: Retrieval-Augmented Generation (RAG)

**Harjoitusopas:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Mikä on RAG ja miksi se on tärkeä
- Muistissa toimivan tietokannan rakentaminen
- Avainsana-yhdenmukaisuutta hyödyntävä haku pisteytyksellä
- Maadoitettujen järjestelmäohjeiden koostaminen
- Kokonaisen RAG-putkiston ajaminen laitteella

**Koodiesimerkit:**

| Kieli | Tiedosto |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Osa 5: Tekoälyagenttien rakentaminen

**Harjoitusopas:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Mikä on tekoälyagentti (verrattuna pelkkään LLM-kutsuun)
- `ChatAgent`-malli ja Microsoft Agent Framework
- Järjestelmäohjeet, persoonat, monivuoropuhelut
- Rakenteinen tuloste (JSON) agenteilta

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Yksittäinen agentti Agent Frameworkilla |
| C# | `csharp/SingleAgent.cs` | Yksittäinen agentti (ChatAgent-malli) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Yksittäinen agentti (ChatAgent-malli) |

---

### Osa 6: Monen agentin työnkulut

**Harjoitusopas:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Monen agentin putkistot: Tutkija → Kirjoittaja → Toimittaja
- Sarjallinen orkestrointi ja palautesilmukat
- Jaettu konfiguraatio ja rakenteelliset siirrot
- Oman monen agentin työnkulun suunnittelu

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Kolmen agentin putkisto |
| C# | `csharp/MultiAgent.cs` | Kolmen agentin putkisto |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Kolmen agentin putkisto |

---

### Osa 7: Zava Creative Writer – päättötyösovellus

**Harjoitusopas:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Tuotantotason monen agentin sovellus neljällä erikoistuneella agentilla
- Sarjallinen putkisto arvioijavetoinen palautesilmukoilla
- Suoratoistettu tulostus, tuoteluettelohaku, rakenteelliset JSON-siirrot
- Täysi toteutus Pythonilla (FastAPI), JavaScriptillä (Node.js CLI) ja C#:lla (.NET-konsoli)

**Koodiesimerkit:**

| Kieli | Hakemisto | Kuvaus |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI-verkkopalvelu orkestroijalla |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI -sovellus |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 -konsolisovellus |

---

### Osa 8: Arviointivetoine kehitys

**Harjoitusopas:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Rakentaa systemaattisen arviointikehyksen tekoälyagenteille kultaisilla aineistoilla
- Sääntöpohjaiset tarkistukset (pituus, avainsanan kattavuus, kielletyt termit) + LLM-tuomaristipisteytys
- Sivuttainen vertailu eri kehotetyyppien välillä kokonaisarvosanauksin
- Laajentaa Zava Editor -agenttimallia osaan 7 offline-testipaketiksi
- Python-, JavaScript- ja C#-polut

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Arviointikehys |
| C# | `csharp/AgentEvaluation.cs` | Arviointikehys |
| JavaScript | `javascript/foundry-local-eval.mjs` | Arviointikehys |

---

### Osa 9: Äänitallennuksen tekstitys Whisperillä

**Harjoitusopas:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Puheesta tekstiksi -tekstitys OpenAI Whisperillä, joka toimii paikallisesti
- Yksityisyys ensin -äänentoisto – ääni ei koskaan poistu laitteeltasi
- Python-, JavaScript- ja C#-polut `client.audio.transcriptions.create()` (Python/JS) ja `AudioClient.TranscribeAudioAsync()` (C#) avulla
- Sisältää Zava-teemaisia esimerkkitiedostoja käytännön harjoittelua varten

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper-puheentunnistus |
| C# | `csharp/WhisperTranscription.cs` | Whisper-puheentunnistus |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper-puheentunnistus |

> **Huom:** Tämä työpaja käyttää **Foundry Local SDK:ta** ohjelmallisesti lataamaan ja lataamaan Whisper-mallin, ja sitten lähettää äänen paikalliseen OpenAI-yhteensopivaan päätepisteeseen tekstitystä varten. Whisper-malli (`whisper`) on listattu Foundry Localin luettelossa ja toimii kokonaan laitteella – pilvi-API-avaimia tai verkkoyhteyttä ei tarvita.

---

### Osa 10: Mukautettujen tai Hugging Face -mallien käyttäminen

**Työpajan ohje:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face -mallien kääntäminen optimoituun ONNX-muotoon ONNX Runtime GenAI -mallinrakentajan avulla
- Laiterajoitteinen kääntäminen (CPU, NVIDIA GPU, DirectML, WebGPU) ja kvantisointi (int4, fp16, bf16)
- Keskustelupohjien määritystiedostojen luominen Foundry Localille
- Käännettyjen mallien lisääminen Foundry Local -välimuistiin
- Mukautettujen mallien suorittaminen CLI:n, REST API:n ja OpenAI SDK:n kautta
- Esimerkkivertailu: Qwen/Qwen3-0.6B koko ketjun kääntäminen

---

### Osa 11: Työkalukutsut paikallisilla malleilla

**Työpajan ohje:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Mahdollista paikallisten mallien kutsua ulkoisia toimintoja (työkalukutsu/toimintokutsu)
- Määrittele työkalujen skeemat OpenAI:n toimintokutsumuodon avulla
- Hallitse monikierroksista työkalukeskustelun kulkua
- Suorita työkalukutsut paikallisesti ja palauta tulokset mallille
- Valitse oikea malli työkalukutsutilanteisiin (Qwen 2.5, Phi-4-mini)
- Käytä SDK:n natiivia `ChatClientia` työkalukutsuihin (JavaScript)

**Koodiesimerkit:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Työkalukutsut sää-/väestötyökaluilla |
| C# | `csharp/ToolCalling.cs` | Työkalukutsut .NETillä |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Työkalukutsut ChatClientilla |

---

### Osa 12: Verkkokäyttöliittymän rakentaminen Zava Creative Writerille

**Työpajan ohje:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Lisää selainpohjainen käyttöliittymä Zava Creative Writerille
- Tarjoa jaettu käyttöliittymä Pythonista (FastAPI), JavaScriptistä (Node.js HTTP) ja C#:sta (ASP.NET Core)
- Kuluta selaimessa suoratoistettua NDJSONia Fetch API:n ja ReadableStreamin avulla
- Elävät agenttitilamerkit ja reaaliaikainen artikkelitekstin suoratoisto

**Koodi (jaettu käyttöliittymä):**

| Tiedosto | Kuvaus |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sivun asettelu |
| `zava-creative-writer-local/ui/style.css` | Tyylittely |
| `zava-creative-writer-local/ui/app.js` | Suoratoistolukija ja DOM-päivityslogiikka |

**Backend-laajennukset:**

| Kieli | Tiedosto | Kuvaus |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Päivitetty tarjoamaan staattinen käyttöliittymä |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Uusi HTTP-palvelin, joka käärii orkestroijan |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Uusi ASP.NET Core minimal API -projekti |

---

### Osa 13: Työpaja valmis

**Työpajan ohje:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

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
| Foundry Local -sivusto | [foundrylocal.ai](https://foundrylocal.ai) |
| Malliluettelo | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Aloitusopas | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK viite | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lisenssi

Tämä työpajamateriaali on tarkoitettu opetuskäyttöön.

---

**Hauskaa rakentamista! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Vastuuvapauslauseke**:  
Tämä asiakirja on käännetty käyttämällä tekoälypohjaista käännöspalvelua [Co-op Translator](https://github.com/Azure/co-op-translator). Pyrimme tarkkuuteen, mutta huomioithan, että automaattisissa käännöksissä saattaa esiintyä virheitä tai epätarkkuuksia. Alkuperäinen asiakirja omalla kielellään on erotettava auktoriteettina. Kritiikissä tiedoissa suositellaan ammattilaisten tekemää ihmiskäännöstä. Emme ole vastuussa tästä käännöksestä aiheutuvista väärinymmärryksistä tai tulkinnoista.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->