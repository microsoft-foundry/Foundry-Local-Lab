# Muutokset — Foundry Local Workshop

Kaikki huomattavat muutokset tähän työpajaan on dokumentoitu alla.

---

## 2026-03-11 — Osa 12 & 13, Web-käyttöliittymä, Whisperin uudelleenkirjoitus, WinML/QNN-korjaus ja validointi

### Lisätty
- **Osa 12: Verkkokäyttöliittymän rakentaminen Zava-kirjoittajalle** — uusi laboratorion opas (`labs/part12-zava-ui.md`) harjoituksineen, jotka käsittelevät suoratoistettua NDJSON:ia, selaimen `ReadableStream`:ia, live-agentin tilamerkkejä ja reaaliaikaista artikkelin tekstin suoratoistoa
- **Osa 13: Työpaja valmis** — uusi yhteenvetolaboratorio (`labs/part13-workshop-complete.md`), jossa kerrataan kaikki 12 osaa, lisäideoita ja resurssilinkkejä
- **Zava UI etupää:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — jaettu vanilla HTML/CSS/JS selainkäyttöliittymä, jota kaikki kolme taustajärjestelmää käyttävät
- **JavaScript HTTP-palvelin:** `zava-creative-writer-local/src/javascript/server.mjs` — uusi Express-tyylinen HTTP-palvelin, joka käärii orkestroijan selaimella käytettäväksi
- **C# ASP.NET Core -taustajärjestelmä:** `zava-creative-writer-local/src/csharp-web/Program.cs` ja `ZavaCreativeWriterWeb.csproj` — uusi minimal API -projekti, joka palvelee käyttöliittymää ja suoratoistaa NDJSON:ia
- **Ääninäytteen generaattori:** `samples/audio/generate_samples.py` — offline TTS-skripti, joka käyttää `pyttsx3`:a tuottamaan Zava-aiheisia WAV-tiedostoja osaan 9
- **Ääninäyte:** `samples/audio/zava-full-project-walkthrough.wav` — uusi pidempi ääninäyte tekstitystestiä varten
- **Validointiskripti:** `validate-npu-workaround.ps1` — automatisoitu PowerShell-skripti NPU/QNN-kierteen varmistamiseen kaikissa C#-näytteissä
- **Mermaid kaaviot SVG-muodossa:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML:n monialustatuki:** Kaikki 3 C# `.csproj` -tiedostoa (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) käyttävät nyt ehdollista TFM:ää ja toisiaan poissulkevia pakettiviittauksia monialustatukea varten. Windowsilla: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (superset, joka sisältää QNN EP -lisäosan). Ei-Windowsilla: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (perus-SDK). Kovakoodattu `win-arm64` RID Zava-projekteissa korvattiin automaattisella tunnistuksella. Transitiivisen riippuvuuden kiertotie sulkee pois natiiviaineistot `Microsoft.ML.OnnxRuntime.Gpu.Linux` -paketista, jossa on rikkinäinen win-arm64-viittaus. Aiempi try/catch NPU-kiertotie poistettiin kaikista 7 C# tiedostosta.

### Muutettu
- **Osa 9 (Whisper):** Merkittävä uudelleenkirjoitus — JavaScript käyttää nyt SDK:n sisäänrakennettua `AudioClient`ia (`model.createAudioClient()`) manuaalisen ONNX Runtime -päättelyn sijaan; päivitetyt arkkitehtuurin kuvaukset, vertailutaulukot ja putkiston kaaviot heijastamaan JS/C# `AudioClient` -lähestymistapaa Python ONNX Runtime -tavasta
- **Osa 11:** Päivitetyt navigointilinkit (osoittaa nyt osaan 12); lisätty renderöidyt SVG-kaaviot työkalukutsujen kulutuksesta ja sekvenssistä
- **Osa 10:** Muutettu navigointi kulkemaan osan 12 kautta työpajan lopettamisen sijaan
- **Python Whisper (`foundry-local-whisper.py`):** Laajennettu lisäääninäytteillä ja parannetulla virheenkäsittelyllä
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Uudelleenkirjoitettu käyttämään `model.createAudioClient()` ja `audioClient.transcribe()` manuaalisen ONNX Runtime -sessioiden sijaan
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Päivitetty palvelemaan staattisia UI-tiedostoja API:n ohella
- **Zava C# konsoli (`zava-creative-writer-local/src/csharp/Program.cs`):** Poistettu NPU-kiertotie (hoidetaan nyt WinML-paketilla)
- **README.md:** Lisätty Osa 12 osio koodinäytteiden taulukoilla ja taustajärjestelmän lisäyksillä; lisätty Osa 13 osio; päivitetty oppimistavoitteita ja projektin rakennetta
- **KNOWN-ISSUES.md:** Poistettu ratkaistu ongelma #7 (C# SDK NPU-mallivariantti — nyt hoidettu WinML-paketilla). Jäljellä olevien ongelmien numerointi muutettu #1–#6. Päivitetty ympäristötiedot .NET SDK 10.0.104:llä
- **AGENTS.md:** Päivitetty projektirakenteen puu uudella `zava-creative-writer-local` -merkinnällä (`ui/`, `csharp-web/`, `server.mjs`); päivitetty C# keskeiset paketit ja ehdolliset TFM-tiedot
- **labs/part2-foundry-local-sdk.md:** Päivitetty `.csproj`-esimerkkiä näyttämään koko monialustamalli ehdollisella TFM:llä, toisiaan poissulkevilla pakettiviittauksilla ja selittävä lappu

### Varmennettu
- Kaikki 3 C# projektia (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) kääntyvät onnistuneesti Windows ARM64:lla
- Chat-näyte (`dotnet run chat`): malli latautuu `phi-3.5-mini-instruct-qnn-npu:1` WinML/QNN:llä — NPU-variantti latautuu suoraan ilman CPU-varmistusta
- Agentti-näyte (`dotnet run agent`): toimii kokonaisuudessaan monikierroksisella keskustelulla, lopetuskoodi 0
- Foundry Local CLI v0.8.117 ja SDK v0.9.0 .NET SDK 9.0.312:lla

---

## 2026-03-11 — Koodikorjaukset, Mallien siivous, Mermaid-kaaviot ja Validointi

### Korjattu
- **Kaikki 21 koodinäytettä (7 Python, 7 JavaScript, 7 C#):** Lisätty `model.unload()` / `unload_model()` / `model.UnloadAsync()` siivous lopetuksessa, jotta ratkaistaan OGA-muistivuoto-varoitukset (tunnettu ongelma #4)
- **csharp/WhisperTranscription.cs:** Korvattu hauras `AppContext.BaseDirectory` suhteellinen polku `FindSamplesDirectory()`-funktiolla, joka käy hakemistoja ylöspäin ja löytää luotettavasti `samples/audio` -kansion (tunnettu ongelma #7)
- **csharp/csharp.csproj:** Korvattu kovakoodattu `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` automaattisella tunnistuksella käyttäen `$(NETCoreSdkRuntimeIdentifier)` -kenttää, jotta `dotnet run` toimii millä tahansa alustalla ilman `-r`-valitsinta ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Muutettu
- **Osa 8:** Muutettu eval-pohjainen iterointisilmukka ASCII-ruutukaaviosta renderöityyn SVG-kuvaan
- **Osa 10:** Muutettu kääntöputken kaavio ASCII-nuolet renderöidyiksi SVG-kuviksi
- **Osa 11:** Muutettu työkalukutsun kulun ja sekvenssin kaaviot renderöidyiksi SVG-kuviksi
- **Osa 10:** Siirretty "Workshop Complete!" -osio Osaan 11 (lopullinen laboratorio); korvattu "Seuraavat askeleet" -linkillä
- **KNOWN-ISSUES.md:** Kaikkien ongelmien täydellinen uudelleentarkistus CLI v0.8.117:llä. Poistettu ratkaistut: OGA-muistivuoto (siivous lisätty), Whisper-polku (FindSamplesDirectory), HTTP 500 kestävä inferenssi (ei toistettavissa, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice-rajoitukset (toimii nyt `"required"` ja erityisillä funktiokohdistuksilla qwen2.5-0.5b:llä). Päivitetty JS Whisper -ongelma — kaikki tiedostot palauttavat nyt tyhjän/binäärisen tuloksen (regressio v0.9.x vs, vakavuus nostettu suureksi). Päivitetty #4 C# RID automaattisella kiertotiellä ja [#497](https://github.com/microsoft/Foundry-Local/issues/497) linkillä. 7 avointa ongelmaa jäljellä.
- **javascript/foundry-local-whisper.mjs:** Korjattu siivousmuuttujan nimeä (`whisperModel` → `model`)

### Varmennettu
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — toimivat onnistuneesti siivouksella
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — toimivat onnistuneesti siivouksella
- C#: `dotnet build` onnistuu 0 varoituksella, 0 virheellä (net9.0-kohde)
- Kaikki 7 Python-tiedostoa läpäisevät `py_compile` syntaksitarkistuksen
- Kaikki 7 JavaScript-tiedostoa läpäisevät `node --check` syntaksivarmistuksen

---

## 2026-03-10 — Osa 11: Työkalukutsu, SDK API:n laajennus ja mallikattavuus

### Lisätty
- **Osa 11: Työkalukutsu paikallisilla malleilla** — uusi laboratorion opas (`labs/part11-tool-calling.md`) 8 harjoituksella, jotka käsittelevät työkalujen skeemoja, monikierroksista kulkua, useita työkalukutsuja, mukautettuja työkaluja, ChatClient-työkalukutsua ja `tool_choice`-ominaisuutta
- **Python-näyte:** `python/foundry-local-tool-calling.py` — työkalukutsu `get_weather`/`get_population` -työkaluilla OpenAI SDK:lla
- **JavaScript-näyte:** `javascript/foundry-local-tool-calling.mjs` — työkalukutsu SDK:n natiivilla `ChatClient`illa (`model.createChatClient()`)
- **C#-näyte:** `csharp/ToolCalling.cs` — työkalukutsu käyttäen `ChatTool.CreateFunctionTool()` OpenAI C# SDK:lla
- **Osa 2, Harjoitus 7:** Natiivi `ChatClient` — `model.createChatClient()` (JS) ja `model.GetChatClientAsync()` (C#) vaihtoehtoina OpenAI SDK:lle
- **Osa 2, Harjoitus 8:** Mallivariantit ja laitteistovalinta — `selectVariant()`, `variants`, NPU-varianttitaulukko (7 mallia)
- **Osa 2, Harjoitus 9:** Mallipäivitykset ja katalogin päivitys — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Osa 2, Harjoitus 10:** Päättelymallit — `phi-4-mini-reasoning` ja `<think>`-tagin jäsennysesimerkit
- **Osa 3, Harjoitus 4:** `createChatClient` vaihtoehtona OpenAI SDK:lle, dokumentaatio suoratoistokutsujen callback-mallista
- **AGENTS.md:** Lisätty Työkalukutsu, ChatClient ja Päättelymallit -koodauskäytänteet

### Muutettu
- **Osa 1:** Mallikatalogia laajennettu — lisätty phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Osa 2:** Laajennettu API-viittaus taulukot — lisätty `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Osa 2:** Harjoitukset 7-9 uudelleen numeroitu 10-13 uusille harjoituksille
- **Osa 3:** Päivitetty keskeisten opittavien taulukko sisältämään natiivin ChatClientin
- **README.md:** Lisätty Osa 11 osio koodinäytetaulukolla; lisätty oppimistavoite #11; päivitetty projektin rakennepuu
- **csharp/Program.cs:** Lisätty `toolcall` CLI-reitittimeen ja päivitetty aputeksti

---

## 2026-03-09 — SDK v0.9.0 Päivitys, brittiläinen englanti ja validointikierros

### Muutettu
- **Kaikki koodinäytteet (Python, JavaScript, C#):** Päivitetty Foundry Local SDK v0.9.0 API:iin — korjattu `await catalog.getModel()` (puuttui `await`), päivitetyt `FoundryLocalManager` initaatiomallit, korjattu päätepisteiden löytyminen
- **Kaikki laboratoriopäivät (Osa 1-10):** Muutettu brittiläiseen englantiin (colour, catalogue, optimised, jne.)
- **Kaikki laboratoriopäivät:** Päivitetty SDK-koodiesimerkit vastaamaan v0.9.0 API-pintaa
- **Kaikki laboratoriopäivät:** Päivitetyt API-viittaus taulukot ja harjoituskoodilohkot
- **JavaScript kriittinen korjaus:** Lisätty puuttuva `await` `catalog.getModel()` kutsuun — palautti `Promise`-olion eikä `Model`ia, aiheuttaen hiljaisia virheitä alavirtaan

### Varmennettu
- Kaikki Python näytteet pyörivät onnistuneesti Foundry Local -palvelua vasten
- Kaikki JavaScript näytteet pyörivät onnistuneesti (Node.js 18+)
- C# projekti kääntyy ja toimii .NET 9.0:lla (eteenpäin yhteensopiva net8.0 SDK-kokoelmasta)
- 29 tiedostoa muokattu ja varmennettu koko työpajassa

---

## Tiedostohakemisto

| Tiedosto | Viimeksi päivitetty | Kuvaus |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Laajennettu mallikatalogi |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Uudet harjoitukset 7-10, laajennetut API-taulukot |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Uusi harjoitus 4 (ChatClient), päivitetyt opittavat asiat |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + brittiläinen englanti |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + brittiläinen englanti |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + brittiläinen englanti |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + brittiläinen englanti |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid-kaavio |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + brittiläinen englanti |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid-kaavio, siirretty Workshop Complete osaan 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Uusi lab, Mermaid-kaaviot, Workshop Complete -osio |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Uusi: apuvälineen kutsun esimerkki |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Uusi: apuvälineen kutsun esimerkki |
| `csharp/ToolCalling.cs` | 2026-03-10 | Uusi: apuvälineen kutsun esimerkki |
| `csharp/Program.cs` | 2026-03-10 | Lisätty `toolcall` CLI-komento |
| `README.md` | 2026-03-10 | Osa 11, projektin rakenne |
| `AGENTS.md` | 2026-03-10 | Apuvälinekuvaukset + ChatClient-käytännöt |
| `KNOWN-ISSUES.md` | 2026-03-11 | Poistettu ratkaistu ongelma #7, jäljellä 6 avointa ongelmaa |
| `csharp/csharp.csproj` | 2026-03-11 | Monialustainen TFM, WinML/pohjaisen SDK:n ehdolliset viittaukset |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Monialustainen TFM, autohavaitse RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Monialustainen TFM, autohavaitse RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Poistettu NPU try/catch -kiertotie |
| `csharp/SingleAgent.cs` | 2026-03-11 | Poistettu NPU try/catch -kiertotie |
| `csharp/MultiAgent.cs` | 2026-03-11 | Poistettu NPU try/catch -kiertotie |
| `csharp/RagPipeline.cs` | 2026-03-11 | Poistettu NPU try/catch -kiertotie |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Poistettu NPU try/catch -kiertotie |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Monialustainen .csproj-esimerkki |
| `AGENTS.md` | 2026-03-11 | Päivitetyt C#-paketit ja TFM-tiedot |
| `CHANGELOG.md` | 2026-03-11 | Tämä tiedosto |