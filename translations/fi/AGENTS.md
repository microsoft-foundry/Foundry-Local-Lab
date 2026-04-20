# Koodausagentin ohjeet

Tämä tiedosto tarjoaa kontekstin tämän repositorion tekoälykoodausagenteille (GitHub Copilot, Copilot Workspace, Codex jne.).

## Projektin yleiskatsaus

Tämä on **käytännön työpaja** tekoälysovellusten rakentamiseen [Foundry Localin](https://foundrylocal.ai) avulla — kevyt suoritusympäristö, joka lataa, hallinnoi ja tarjoaa kielimalleja kokonaan laitteella OpenAI-yhteensopivan API:n kautta. Työpajassa on vaiheittaiset laboratoriovinkit ja ajettavat koodiesimerkit Pythonilla, JavaScriptillä ja C#:lla.

## Repositorion rakenne

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Kielikohtaiset ja kehyskohtaiset tiedot

### Python
- **Sijainti:** `python/`, `zava-creative-writer-local/src/api/`
- **Riippuvuudet:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Keskeiset paketit:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimiversio:** Python 3.9+
- **Ajo:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Sijainti:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Riippuvuudet:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Keskeiset paketit:** `foundry-local-sdk`, `openai`
- **Moduulijärjestelmä:** ES-moduulit (`.mjs`-tiedostot, `"type": "module"`)
- **Minimiversio:** Node.js 18+
- **Ajo:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Sijainti:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projektitiedostot:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Keskeiset paketit:** `Microsoft.AI.Foundry.Local` (ei-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — laajennettu QNN EP:llä), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Kohde:** .NET 9.0 (ehtoinen TFM: `net9.0-windows10.0.26100` Windowsilla, `net9.0` muualla)
- **Ajo:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Koodauskäytännöt

### Yleistä
- Kaikki koodiesimerkit ovat **itsenäisiä yksittäistiedosto-esimerkkejä** — ei jaettuja apukirjastoja tai abstraktioita.
- Kukin esimerkki suoritetaan itsenäisesti omien riippuvuuksiensa asennuksen jälkeen.
- API-avaimet ovat aina `"foundry-local"` — Foundry Local käyttää tätä paikkamerkkinä.
- Perus-URL:t käyttävät `http://localhost:<port>/v1` — portti on dynaaminen ja löydetään suoritusajossa SDK:lla (`manager.urls[0]` JS:ssä, `manager.endpoint` Pythonissa).
- Foundry Local SDK huolehtii palvelun käynnistyksestä ja päätepisteiden löytämisestä; suositaan SDK:n kuvioita kiinteiden porttien sijaan.

### Python
- Käytä `openai` SDK:ta `OpenAI(base_url=..., api_key="not-required")`.
- Käytä `FoundryLocalManager()` luokkaa `foundry_local`-kirjastosta SDK:n hallitsemaan palvelun elinkaareen.
- Streaming: iteroi `stream`-objektia `for chunk in stream:` -rakenteella.
- Ei tyyppimerkintöjä esimerkkitiedostoissa (pidä esimerkit napakoina työpajan tekijöille).

### JavaScript
- ES-moduulisyntaksi: `import ... from "..."`.
- Käytä `OpenAI`-luokkaa `"openai"`-kirjastosta ja `FoundryLocalManager`ia `"foundry-local-sdk"`-kirjastosta.
- SDK:n alustuskuvio: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Top-level `await` on käytössä laajasti.

### C#
- Nullable käytössä, implisiittiset usingt, .NET 9.
- Käytä `FoundryLocalManager.StartServiceAsync()` SDK:n hallitsemaan elinkaareen.
- Streaming: `CompleteChatStreaming()` ja `foreach (var update in completionUpdates)`.
- Pääasiallinen `csharp/Program.cs` on komentoriviliittymän reititin, joka kutsuu staattisia `RunAsync()`-metodeja.

### Työkalukutsut
- Vain tietyt mallit tukevat työkalukutsuja: **Qwen 2.5** -perhe (`qwen2.5-*`) ja **Phi-4-mini** (`phi-4-mini`).
- Työkalujen skeemat noudattavat OpenAI:n funktiokutsujen JSON-muotoa (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Keskustelu käyttää monivuorokuvioita: käyttäjä → avustaja (tool_calls) → työkalu (tulokset) → avustaja (lopullinen vastaus).
- Työkalutulosten `tool_call_id`-kentän tulee vastata mallin työkalukutsun `id`-kenttää.
- Python käyttää OpenAI SDK:ta suoraan; JavaScript käyttää SDK:n natiivia `ChatClient`-oliota (`model.createChatClient()`); C# käyttää OpenAI SDK:ta yhdessä `ChatTool.CreateFunctionTool()` -metodin kanssa.

### ChatClient (natiivin SDK:n client)
- JavaScript: `model.createChatClient()` palauttaa `ChatClient`-olion, jolla on `completeChat(messages, tools?)` ja `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` palauttaa tavallisen `ChatClient`-olion, jota voi käyttää ilman OpenAI NuGet-paketin tuontia.
- Pythonilla ei ole natiivia ChatClientia — käytä OpenAI SDK:ta ja `manager.endpoint` sekä `manager.api_key`.
- **Tärkeää:** JavaScriptin `completeStreamingChat` käyttää **callback-kuviota**, ei async-iterointia.

### Päättelymallit
- `phi-4-mini-reasoning` ympäröi päättelynsä `<think>...</think>`-tageilla ennen loppuvastausta.
- Parsitaan tagit erottamaan päättely vastauksesta tarvittaessa.

## Laboratoriovinkit

Labratiedostot sijaitsevat `labs/` -hakemistossa Markdown-muotoisina. Niillä on yhtenäinen rakenne:
- Logo otsikkokuvana
- Otsikko ja tavoitekutsu
- Yleiskatsaus, oppimistavoitteet, esitiedot
- Käsiteosuudet kuvioineen
- Numeroidut harjoitukset, joissa on koodilohkot ja odotettu tulos
- Yhteenvetotaulukko, keskeiset opit, jatkolukemista
- Navigointilinkki seuraavaan osaan

Muokattaessa labran sisältöjä:
- Säilytä Markdownin nykyinen muotoilu ja otsikkotasot.
- Koodilohkoissa ilmoitetaan kieli (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Anna sekä bash- että PowerShell-versiot komentorivityökaluille, jossa käyttöjärjestelmällä on merkitystä.
- Käytä kutsutyyliä `> **Note:**`, `> **Tip:**`, ja `> **Troubleshooting:**`.
- Taulukot käyttävät `| Otsikko | Otsikko |` putkimuotoilua.

## Käännös- ja testauskomennot

| Toiminto | Komento |
|--------|---------|
| **Python-esimerkit** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS-esimerkit** | `cd javascript && npm install && node <script>.mjs` |
| **C#-esimerkit** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Kuvioiden generointi** | `npx mmdc -i <input>.mmd -o <output>.svg` (vaatii root:n `npm install`) |

## Ulkoiset riippuvuudet

- **Foundry Local CLI** tulee olla asennettuna kehittäjän koneelle (`winget install Microsoft.FoundryLocal` tai `brew install foundrylocal`).
- **Foundry Local -palvelu** toimii paikallisesti ja tarjoaa OpenAI-yhteensopivan REST API:n dynaamisella portilla.
- Kukaan pilvipalveluja, API-avaimia tai Azure-tilauksia ei tarvita esimerkkien suorittamiseen.
- Osa 10 (mukautetut mallit) vaatii lisäksi `onnxruntime-genai` ja lataa mallipainot Hugging Facesta.

## Tiedostot, joita ei tule sitoa

`.gitignore` sulkee pois (ja sulkee suurimman osan):
- `.venv/` — Python-virtuaaliympäristöt
- `node_modules/` — npm-riippuvuudet
- `models/` — käännetyt ONNX-mallit (suuret binääritiedostot, tuotettu osa 10:ssä)
- `cache_dir/` — Hugging Face -mallilatausten välimuisti
- `.olive-cache/` — Microsoft Oliven työskentelyhakemisto
- `samples/audio/*.wav` — generoituja ääninäytteitä (uudelleen luodaan `python samples/audio/generate_samples.py` -scriptein)
- Tavalliset Pythonin kokoamisjätteet (`__pycache__/`, `*.egg-info/`, `dist/` jne.)

## Lisenssi

MIT — katso `LICENSE`.