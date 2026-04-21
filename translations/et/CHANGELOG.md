# Muudatuste logi — Foundry Locali töötoa

Kõik selle töötoa olulised muudatused on allpool dokumenteeritud.

---

## 2026-03-11 — Osa 12 & 13, Veebi kasutajaliides, Whisperi ümbersõnastus, WinML/QNN parandus ja valideerimine

### Lisatud
- **Osa 12: Veebi kasutajaliidese loomine Zava loovkirjutaja jaoks** — uus laborijuhend (`labs/part12-zava-ui.md`) harjutustega, mis hõlmavad NDJSON voogedastust, brauseri `ReadableStream`i, reaalajas agendi oleku märke ja artikli reaalajas tekstiedastust
- **Osa 13: Töötoa lõpetamine** — uus kokkuvõttelabor (`labs/part13-workshop-complete.md`) kõigi 12 osa ülevaatega, edasiste ideedega ja ressursilingidega
- **Zava UI esiosa:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — jagatud tavaline HTML/CSS/JS brauseriliides, mida kasutavad kõik kolm tagapoolt
- **JavaScripti HTTP server:** `zava-creative-writer-local/src/javascript/server.mjs` — uus Express-tüüpi HTTP server, mis mähib orkestreerija brauseripõhiseks juurdepääsuks
- **C# ASP.NET Core tagapool:** `zava-creative-writer-local/src/csharp-web/Program.cs` ja `ZavaCreativeWriterWeb.csproj` — uus minimaalne API projekt UI ja NDJSON voogedastuse teenindamiseks
- **Helinäidisgeneraator:** `samples/audio/generate_samples.py` — võrguühenduseta TTS-skript, mis kasutab `pyttsx3` Zava-teemaliste WAV-failide genereerimiseks Osa 9 jaoks
- **Helinäidis:** `samples/audio/zava-full-project-walkthrough.wav` — uus pikem helinäidis transkriptsiooni testimiseks
- **Valideerimisskript:** `validate-npu-workaround.ps1` — automatiseeritud PowerShelli skript NPU/QNN lahenduse valideerimiseks kõigis C# näidetes
- **Mermaid diagrammide SVG-d:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML platvormideülene tugi:** Kõik 3 C# `.csproj` faili (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) kasutavad nüüd tingimuslikku sihtraamistikku (TFM) ja üksteist välistavaid paketiviiteid platvormideüliseks toetuseks. Windowsis: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (ümardpakett, mis sisaldab QNN EP pluginat). Mitte-Windowsis: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (baas SDK). Zava projektides asendati kõvakodeeritud `win-arm64` RID automaatse tuvastusega. Läbiva sõltuvuse lahendus välistab natiivsed ressursid `Microsoft.ML.OnnxRuntime.Gpu.Linux` paketist, millel on katki `win-arm64` viide. Varem kasutusel olnud try/catch NPU lahendust on eemaldatud kõigist 7 C# failist.

### Muudetud
- **Osa 9 (Whisper):** Suur ümbersõnastus — JavaScript kasutab nüüd SDK sisseehitatud `AudioClient`i (`model.createAudioClient()`) käsitsi ONNX Runtime inferentsi asemel; uuendatud arhitektuuri kirjeldused, võrdlustabelid ja torujuhtme diagrammid kajastamaks JS/C# `AudioClient` lähenemist võrreldes Python ONNX Runtime lähenemisega
- **Osa 11:** Uuendatud navigeerimislingid (suunduvad nüüd Osa 12-le); lisatud renderdatud SVG diagrammid tööriista kutsumise voogude ja järjestuse kohta
- **Osa 10:** Uuendatud navigeerimine, et suunata Osa 12 kaudu mitte töötoa lõppu
- **Python Whisper (`foundry-local-whisper.py`):** Laiendatud täiendavate helinäidetega ja parandatud veakäsitlus
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Ümber kirjutatud kasutama `model.createAudioClient()` koos `audioClient.transcribe()` asemel käsitsi ONNX Runtime sessioone
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Uuendatud staatiliste UI failide serveerimiseks koos API-ga
- **Zava C# konsool (`zava-creative-writer-local/src/csharp/Program.cs`):** Eemaldatud NPU lahendus (nüüd haldab WinML pakett)
- **README.md:** Lisatud Osa 12 sektsioon koos koodinäidiste tabelitega ja tagapoolte lisadega; lisatud Osa 13 sektsioon; uuendatud õpieesmärgid ja projekti struktuur
- **KNOWN-ISSUES.md:** Eemaldatud lahendatud probleem #7 (C# SDK NPU mudeli variatsioon — nüüd haldab WinML pakett). Ülejäänud probleemid ümber nummerdatud #1–#6-ks. Uuendatud keskkonna üksikasjad .NET SDK 10.0.104-ga
- **AGENTS.md:** Uuendatud projekti struktuuri puu uute `zava-creative-writer-local` kirjetega (`ui/`, `csharp-web/`, `server.mjs`); uuendatud C# võtme paketid ja tingimuslikud TFM-i detailid
- **labs/part2-foundry-local-sdk.md:** Uuendatud `.csproj` näidet näitamaks täielikku platvormidevahelist mustrit koos tingimusliku TFM-i, üksteist välistavate paketiviidetega ja selgitava märkusega

### Valideeritud
- Kõik 3 C# projekti (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) ehituvad edukalt Windows ARM64 peal
- Vestlusnäide (`dotnet run chat`): mudel laetakse `phi-3.5-mini-instruct-qnn-npu:1` vastavalt WinML/QNN kasutamisele — NPU variatsioon laetakse otse ilma CPU tagavarata
- Agendi näide (`dotnet run agent`): töötab lõpuni mitmekordse vestlusega, väljumiskood 0
- Foundry Local CLI v0.8.117 ja SDK v0.9.0 .NET SDK 9.0.312 peal

---

## 2026-03-11 — Koodiparandused, mudeli puhastus, Mermaid diagrammid ja valideerimine

### Parandatud
- **Kõik 21 koodinäidet (7 Python, 7 JavaScript, 7 C#):** Lisatud `model.unload()` / `unload_model()` / `model.UnloadAsync()` puhastus väljapääsu juures OGA mäluleke hoiatuste lahendamiseks (Tuntud probleem #4)
- **csharp/WhisperTranscription.cs:** Asendatud ebastabiilne `AppContext.BaseDirectory` suhteline tee `FindSamplesDirectory()` funktsiooniga, mis käib kataloogides kõrgemale, et usaldusväärselt leida `samples/audio` (Tuntud probleem #7)
- **csharp/csharp.csproj:** Asendatud kõvakodeeritud `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` automaatse tuvastuse fallbackiga, kasutades `$(NETCoreSdkRuntimeIdentifier)`, nii et `dotnet run` töötab kõikidel platvormidel ilma `-r` liputa ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Muudetud
- **Osa 8:** Muudetud eval-põhine iteratsioonitsükkel ASCII kastidiagrammist renderdatud SVG-pildiks
- **Osa 10:** Muudetud kompileerimistorujuhtme diagramm ASCII nooltest renderdatud SVG-pildiks
- **Osa 11:** Muudetud tööriista kutsumise voog ja järjestuse diagrammid renderdatud SVG-piltideks
- **Osa 10:** Lükatud "Töötoa lõpetamine!" sektsioon Osa 11-le (viimane labor); asendatud "Järgmised sammud" lingiga
- **KNOWN-ISSUES.md:** Täielik kõigi probleemide uuendatud valideerimine CLI v0.8.117 vastu. Eemaldatud lahendatud: OGA mälu lekke probleem (puhastus lisatud), Whisper tee (FindSamplesDirectory), HTTP 500 püsiv inference (mittejälgitav, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice piirangud (nüüd toimib koos `"required"` ja konkreetse funktsiooni sihtimisega qwen2.5-0.5b peal). Uuendatud JS Whisper probleem — nüüd kõik failid tagastavad tühja/binaarse väljundi (tagasiminek v0.9.x-st, raskusaste tõstetud oluliseks). Uuendatud #4 C# RID automaattuvastuse lahendus ja [#497](https://github.com/microsoft/Foundry-Local/issues/497) link. Jätkuvalt avatud 7 probleemi.
- **javascript/foundry-local-whisper.mjs:** Parandatud puhastusmuutuja nimi (`whisperModel` → `model`)

### Valideeritud
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — töötavad edukalt koos puhastusega
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — töötavad edukalt koos puhastusega
- C#: `dotnet build` läbib 0 hoiatust, 0 viga (net9.0 sihtmärk)
- Kõik 7 Pythoni faili läbivad `py_compile` süntaksi kontrolli
- Kõik 7 JavaScripti faili läbivad `node --check` süntaksi valideerimise

---

## 2026-03-10 — Osa 11: Tööriista kutsumine, SDK API laiendus ja mudelite katvus

### Lisatud
- **Osa 11: Tööriistade kutsumine kohalike mudelitega** — uus laborijuhend (`labs/part11-tool-calling.md`) 8 harjutusega, mis käsitlevad tööriista skeeme, mitme-pöörde voogu, mitu tööriista kutset, kohandatud tööriistu, ChatClient tööriista kutset ja `tool_choice`
- **Pythoni näide:** `python/foundry-local-tool-calling.py` — tööriista kutsumine `get_weather`/`get_population` tööriistadega OpenAI SDK kasutades
- **JavaScripti näide:** `javascript/foundry-local-tool-calling.mjs` — tööriista kutsumine SDK natiivse `ChatClient`iga (`model.createChatClient()`)
- **C# näide:** `csharp/ToolCalling.cs` — tööriista kutsumine `ChatTool.CreateFunctionTool()` abil OpenAI C# SDK-ga
- **Osa 2, Harjutus 7:** Natiivne `ChatClient` — `model.createChatClient()` (JS) ja `model.GetChatClientAsync()` (C#) alternatiivina OpenAI SDK-le
- **Osa 2, Harjutus 8:** Mudelivariandid ja riistvara valik — `selectVariant()`, `variants`, NPU variandi tabel (7 mudelit)
- **Osa 2, Harjutus 9:** Mudelite uuendused ja kataloogi värskendus — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Osa 2, Harjutus 10:** Põhjendavad mudelid — `phi-4-mini-reasoning` koos `<think>` sildi tõlgendamise näidetega
- **Osa 3, Harjutus 4:** `createChatClient` alternatiivina OpenAI SDK-le koos voogedastusega tagasikutsumise mustriga dokumentatsiooniga
- **AGENTS.md:** Lisatud Tööriista kutsumise, ChatClienti ja põhjendavate mudelite programmeerimiskonventsioonid

### Muudetud
- **Osa 1:** Laiendatud mudelikataloog — lisatud phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Osa 2:** Laiendatud API viitetabelid — lisatud `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Osa 2:** Harjutuste numbrid 7-9 → 10-13, et mahutada uued harjutused
- **Osa 3:** Uuendatud oluliste võtete tabel natiivse ChatClientiga
- **README.md:** Lisatud Osa 11 sektsioon koodinäidiste tabeliga; lisatud õpieesmärk #11; uuendatud projekti struktuuri puu
- **csharp/Program.cs:** Lisatud `toolcall` juhtum CLI marsruutijasse ja uuendatud abitekst

---

## 2026-03-09 — SDK v0.9.0 uuendus, Briti inglise keel ja valideerimise läbimine

### Muudetud
- **Kõik koodinäited (Python, JavaScript, C#):** Uuendatud Foundry Local SDK v0.9.0 API-le — parandatud `await catalog.getModel()` (puudus `await`), uuendatud `FoundryLocalManager` initsialiseerimismustrid, parandatud lõpp-punkti avastus
- **Kõik laborijuhendid (Osad 1-10):** Konverteeritud Briti inglise keelde (colour, catalogue, optimised jne)
- **Kõik laborijuhendid:** Uuendatud SDK koodinäited vastavalt v0.9.0 APIle
- **Kõik laborijuhendid:** Uuendatud API viitetabelid ja harjutuste koodiblokid
- **JavaScripti kriitiline parandus:** Lisatud puuduv `await` `catalog.getModel()`-ile — tagastas `Promise`, mitte `Model` objekti, põhjustades vaikselt ebaõnnestumisi

### Valideeritud
- Kõik Python näited töötavad edukalt Foundry Local teenuse vastu
- Kõik JavaScripti näited töötavad edukalt (Node.js 18+)
- C# projekt ehitub ja töötab .NET 9.0 peal (edaspidine ühilduvus net8.0 SDK koosseisuga)
- 29 faili muudetud ja valideeritud kogu töötoa ulatuses

---

## Failide indeks

| Fail | Viimati uuendatud | Kirjeldus |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Laiendatud mudelikataloog |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Uued harjutused 7-10, laiendatud API tabelid |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Uus harjutus 4 (ChatClient), uuendatud võtmesõnad |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Briti inglise keel |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Briti inglise keel |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Briti inglise keel |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Briti inglise keel |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagramm |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Briti inglise keel |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagramm, liigutati Workshop Complete ossa 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Uus labor, Mermaid diagrammid, Workshop Complete sektsioon |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Uus: tööriista kutsumise näide |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Uus: tööriista kutsumise näide |
| `csharp/ToolCalling.cs` | 2026-03-10 | Uus: tööriista kutsumise näide |
| `csharp/Program.cs` | 2026-03-10 | Lisatud `toolcall` käsurea käsk |
| `README.md` | 2026-03-10 | Osa 11, projekti struktuur |
| `AGENTS.md` | 2026-03-10 | Tööriista kutsumine + ChatClient konventsioonid |
| `KNOWN-ISSUES.md` | 2026-03-11 | Eemaldatud lahendatud probleem #7, 6 avatud probleemi jäänud |
| `csharp/csharp.csproj` | 2026-03-11 | Platvormideülene TFM, WinML/base SDK tinglikud viited |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Platvormideülene TFM, automaatne RID tuvastamine |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Platvormideülene TFM, automaatne RID tuvastamine |
| `csharp/BasicChat.cs` | 2026-03-11 | Eemaldatud NPU try/catch lahendus |
| `csharp/SingleAgent.cs` | 2026-03-11 | Eemaldatud NPU try/catch lahendus |
| `csharp/MultiAgent.cs` | 2026-03-11 | Eemaldatud NPU try/catch lahendus |
| `csharp/RagPipeline.cs` | 2026-03-11 | Eemaldatud NPU try/catch lahendus |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Eemaldatud NPU try/catch lahendus |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Platvormideülene .csproj näide |
| `AGENTS.md` | 2026-03-11 | Uuendatud C# paketid ja TFM üksikasjad |
| `CHANGELOG.md` | 2026-03-11 | See fail |