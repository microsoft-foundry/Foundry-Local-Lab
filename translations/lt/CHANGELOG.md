# Pakeitimų žurnalas — Foundry Local dirbtuvės

Visi reikšmingi šių dirbtuvių pakeitimai pateikti žemiau.

---

## 2026-03-11 — 12 ir 13 dalys, žiniatinklio UI, Whisper perrašymas, WinML/QNN pataisa ir tikrinimas

### Pridėta
- **12 dalis: žiniatinklio UI kūrimas Zava kūrybiniam rašytojui** — naujas laboratorijos vadovas (`labs/part12-zava-ui.md`) su pratimais, apimančiais NDJSON transliaciją, naršyklės `ReadableStream`, gyvų agentų būsenos ženklelius ir tikro laiko straipsnio teksto transliaciją
- **13 dalis: dirbtuvių užbaigimas** — naujas apibendrinimo laboratorijos vadovas (`labs/part13-workshop-complete.md`) su visų 12 dalių apžvalga, papildomomis idėjomis ir nuorodomis į šaltinius
- **Zava UI priekinė dalis:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — bendras paprastas HTML/CSS/JS naršyklės sąsajos vaizdas, naudojamas visų trijų backendų
- **JavaScript HTTP serveris:** `zava-creative-writer-local/src/javascript/server.mjs` — naujas Express tipo HTTP serveris, apgaubiantis orkestratorių naršyklės prieigai
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` ir `ZavaCreativeWriterWeb.csproj` — naujas minimalus API projektas, aptarnaujantis UI ir perduodantis NDJSON srautą
- **Garso pavyzdžių generatorius:** `samples/audio/generate_samples.py` — neprisijungimo TTS skriptas, naudojantis `pyttsx3`, generuojantis Zava tematikos WAV failus 9 daliai
- **Garso pavyzdys:** `samples/audio/zava-full-project-walkthrough.wav` — naujas ilgesnis garso pavyzdys transkripcijos testavimui
- **Tikrinimo skriptas:** `validate-npu-workaround.ps1` — automatizuotas PowerShell skriptas NPU/QNN aplinkkelio tikrinimui visuose C# pavyzdžiuose
- **Mermaid schemų SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML daugiaplatformė palaikymas:** Visuose 3 C# `.csproj` failuose (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) dabar naudojamos sąlyginės TFM ir tarpusavyje nesuderinamos paketų nuorodos daugiaplatforminiam palaikymui. Windows sistemoje: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (superset’as, apimantis QNN EP įskiepį). Ne-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (pagrindinis SDK). Zava projektuose įtvirtintas `win-arm64` RID buvo pakeistas automatinio aptikimo funkcija. Vejantis priklausomybės sprendimas išima vietines savybes iš `Microsoft.ML.OnnxRuntime.Gpu.Linux`, kuri turi neteisingą win-arm64 nuorodą. Ankstesnis try/catch NPU aplinkkelis buvo pašalintas iš visų 7 C# failų.

### Pakeista
- **9 dalis (Whisper):** Didelis perrašymas — JavaScript dabar naudoja SDK integruotą `AudioClient` (`model.createAudioClient()`) vietoj rankinio ONNX Runtime apdorojimo; atnaujinti architektūros aprašymai, palyginimo lentelės ir grandinės schemos, atspindinčios JS/C# `AudioClient` metodus priešingai nei Python ONNX Runtime metodų
- **11 dalis:** Atnaujinti navigacijos saitai (dabar nukreipiami į 12 dalį); pridėti atvaizduoti SVG diagramos apie įrankių iškvietimo eigą ir seką
- **10 dalis:** Atnaujinta navigacija nukreipianti į 12 dalį vietoj dirbtuvių pabaigos
- **Python Whisper (`foundry-local-whisper.py`):** Išplėsta su papildomais garso pavyzdžiais ir patobulinta klaidų valdymu
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Perrašyta, naudojant `model.createAudioClient()` ir `audioClient.transcribe()` vietoj rankinių ONNX Runtime sesijų
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Atnaujinta aptarnauti statinius UI failus kartu su API
- **Zava C# konsolė (`zava-creative-writer-local/src/csharp/Program.cs`):** Pašalintas NPU aplinkkelis (dabar tvarkomas WinML paketu)
- **README.md:** Pridėta 12 dalies sekcija su kodo pavyzdžių lentelėmis ir backend priedais; pridėta 13 dalies sekcija; atnaujinti mokymosi tikslai ir projekto struktūra
- **KNOWN-ISSUES.md:** Pašalinta išspręsta problema #7 (C# SDK NPU modelio variantas — dabar tvarkomas WinML paketu). Likusios problemos perskaičiuotos nuo #1 iki #6. Atnaujinta aplinkos informacija su .NET SDK 10.0.104
- **AGENTS.md:** Atnaujinta projekto struktūros medžio su naujais `zava-creative-writer-local` įrašais (`ui/`, `csharp-web/`, `server.mjs`); atnaujintos pagrindinių C# paketų ir sąlyginės TFM detalės
- **labs/part2-foundry-local-sdk.md:** Atnaujintas `.csproj` pavyzdys, rodomas pilnas daugiaplatformis modelis su sąlygine TFM, tarpusavyje nesuderinamomis paketų nuorodomis ir paaiškinimu

### Patvirtinta
- Visi 3 C# projektai (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) sėkmingai kompiliuojami Windows ARM64
- Pokalbių pavyzdys (`dotnet run chat`): modelis užkraunamas kaip `phi-3.5-mini-instruct-qnn-npu:1` per WinML/QNN — NPU variantas užkraunamas tiesiogiai be CPU pakaitalo
- Agentų pavyzdys (`dotnet run agent`): veikia pilnai su daugiataukiu pokalbiu, išeities kodas 0
- Foundry Local CLI v0.8.117 ir SDK v0.9.0 vykdomi su .NET SDK 9.0.312

---

## 2026-03-11 — Kodo pataisos, modelio valymas, Mermaid diagramos ir tikrinimas

### Sutaisyta
- **Visi 21 kodo pavyzdys (7 Python, 7 JavaScript, 7 C#):** Pridėtas `model.unload()` / `unload_model()` / `model.UnloadAsync()` valymas išeinant, kad būtų pašalinti OGA atminties nutekėjimo įspėjimai (Žinoma problema #4)
- **csharp/WhisperTranscription.cs:** Vietoje pažeidžiamo `AppContext.BaseDirectory` santykinio kelio naudota `FindSamplesDirectory()`, kuri patikimai randa `samples/audio` per kopimą katalogų lygiu aukštyn (Žinoma problema #7)
- **csharp/csharp.csproj:** Vietoje užkoduoto `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` pridėtas automatinis aptikimas naudojant `$(NETCoreSdkRuntimeIdentifier)`, tad `dotnet run` veikia bet kurioje platformoje be `-r` jungiklio ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Pakeista
- **8 dalis:** Vertinimo ciklas konvertuotas iš ASCII dėžutės schemos į atvaizduotą SVG vaizdą
- **10 dalis:** Kūrimo grandinės schemos konvertuotos iš ASCII rodyklių į atvaizduotus SVG vaizdus
- **11 dalis:** Įrankių iškvietimo srautų ir sekų diagramos konvertuotos į atvaizduotus SVG vaizdus
- **10 dalis:** "Workshop Complete!" skyrius perkeltas į 11 dalį (paskutinę laboratoriją); pakeistas nuoroda į "Kiti žingsniai"
- **KNOWN-ISSUES.md:** Visi problemos pilnai patikrinti su CLI v0.8.117. Pašalintos išspręstos problemos: OGA atminties nutekėjimas (pridėtas valymas), Whisper kelias (`FindSamplesDirectory`), HTTP 500 trukmės apdorojimas (nepakartojamas, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), įrankio pasirinkimo ribojimai (dabar veikia su `"required"` ir konkrečių funkcijų taikymu qwen2.5-0.5b). Atnaujinta JS Whisper problema — visi failai dabar pateikia tuščią/arba dvejetainį išvestį (regresija nuo v0.9.x, sunkumo lygis padidintas į pagrindinę). Atnaujintas #4 C# RID su automatinio aptikimo sprendimu ir [#497](https://github.com/microsoft/Foundry-Local/issues/497) nuoroda. Likusios atviros 7 problemos.
- **javascript/foundry-local-whisper.mjs:** Pataisytas valymo kintamojo pavadinimas (`whisperModel` → `model`)

### Patvirtinta
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — sėkmingai vykdomi su valymu
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — sėkmingai vykdomi su valymu
- C#: `dotnet build` sėkmingas su 0 įspėjimų ir 0 klaidų (net9.0 tikslas)
- Visi 7 Python failai praeina `py_compile` sintaksės tikrinimą
- Visi 7 JavaScript failai praeina `node --check` sintaksės patikrinimą

---

## 2026-03-10 — 11 dalis: įrankių iškvietimas, SDK API plėtra ir modelių apimtis

### Pridėta
- **11 dalis: įrankių iškvietimas su vietiniais modeliais** — naujas laboratorijos vadovas (`labs/part11-tool-calling.md`) su 8 pratimais apie įrankių schemas, daugiaaukštę seką, kelių įrankių iškvietimus, pasirinktinius įrankius, ChatClient įrankio iškvietimą ir `tool_choice`
- **Python pavyzdys:** `python/foundry-local-tool-calling.py` — įrankių iškvietimas naudojant `get_weather`/`get_population` įrankius per OpenAI SDK
- **JavaScript pavyzdys:** `javascript/foundry-local-tool-calling.mjs` — įrankių iškvietimas naudojant SDK gimtą `ChatClient` (`model.createChatClient()`)
- **C# pavyzdys:** `csharp/ToolCalling.cs` — įrankio iškvietimas naudojant `ChatTool.CreateFunctionTool()` su OpenAI C# SDK
- **2 dalies 7 pratimas:** Natūralus `ChatClient` — `model.createChatClient()` (JS) ir `model.GetChatClientAsync()` (C#) kaip alternatyvos OpenAI SDK
- **2 dalies 8 pratimas:** Modelių variantai ir aparatūros pasirinkimas — `selectVariant()`, `variants`, NPU varianto lentelė (7 modeliai)
- **2 dalies 9 pratimas:** Modelių atnaujinimai ir katalogo atnaujinimas — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **2 dalies 10 pratimas:** Loginiai modeliai — `phi-4-mini-reasoning` su `<think>` žymų analizės pavyzdžiais
- **3 dalies 4 pratimas:** `createChatClient` kaip OpenAI SDK alternatyva, su transliacijos atgalinio ryšio šablonu
- **AGENTS.md:** Pridėti įrankių iškvietimo, ChatClient ir loginio modelio kodavimo konvencijos

### Pakeista
- **1 dalis:** Išplėstas modelių katalogas — pridėti phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **2 dalis:** Išplėstos API lentelės — pridėti `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **2 dalis:** Pernumeruoti pratimai nuo 7-9 į 10-13, dėl naujų pratimų pridėjimo
- **3 dalis:** Atnaujinta pagrindinių mokymosi aspektų lentelė, įtraukiant gimtą ChatClient
- **README.md:** Pridėta 11 dalies dalis su kodo pavyzdžių lentele; pridėtas mokymosi tikslas #11; atnaujintas projekto struktūros medis
- **csharp/Program.cs:** Pridėta `toolcall` byla CLI maršrutizatoriuje ir atnaujinta pagalbos žinutė

---

## 2026-03-09 — SDK v0.9.0 atnaujinimas, britų anglų kalba ir tikrinimo ciklas

### Pakeista
- **Visi kodo pavyzdžiai (Python, JavaScript, C#):** Atnaujinti į Foundry Local SDK v0.9.0 API — pataisytas `await catalog.getModel()` (buvo praleistas `await`), atnaujinti `FoundryLocalManager` inicializacijos modeliai, pataisytas taško atradimas
- **Visi laboratorijos vadovai (1-10 dalys):** Konvertuoti į britų anglų kalbą (colour, catalogue, optimised ir kt.)
- **Visi laboratorijos vadovai:** Atnaujinti SDK kodo pavyzdžiai pagal v0.9.0 API vilką
- **Visi laboratorijos vadovai:** Atnaujintos API nuorodų lentelės ir pratimų kodo blokai
- **JavaScript kritinis pataisymas:** Pridėtas praleistas `await` `catalog.getModel()` — grąžino `Promise`, o ne `Model` objektą, sukeldamas tyliai nepavykusias operacijas vėliau

### Patvirtinta
- Visi Python pavyzdžiai sėkmingai veikia su Foundry Local servisu
- Visi JavaScript pavyzdžiai sėkmingai veikia (Node.js 18+)
- C# projektas kompiliuojasi ir veikia .NET 9.0 (suderinamumas iš net8.0 SDK)
- 29 failai pakeisti ir patvirtinti per visas dirbtuves

---

## Failų indeksas

| Failas | Paskutinė atnaujinimo data | Aprašymas |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Išplėstas modelių katalogas |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nauji pratimai 7-10, išplėstos API lentelės |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Naujas 4 pratimas (ChatClient), atnaujinti mokymosi aspektai |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + britų anglų kalba |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + britų anglų kalba |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Britų anglų kalba |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Britų anglų kalba |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagrama |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Britų anglų kalba |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagrama, „Workshop Complete“ perkelta į 11 dalį |
| `labs/part11-tool-calling.md` | 2026-03-11 | Naujas laboratorinis darbas, Mermaid diagramos, „Workshop Complete“ skyrius |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Naujiena: įrankio kvietimo pavyzdys |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Naujiena: įrankio kvietimo pavyzdys |
| `csharp/ToolCalling.cs` | 2026-03-10 | Naujiena: įrankio kvietimo pavyzdys |
| `csharp/Program.cs` | 2026-03-10 | Pridėta `toolcall` CLI komanda |
| `README.md` | 2026-03-10 | 11 dalis, projekto struktūra |
| `AGENTS.md` | 2026-03-10 | Įrankių kvietimas + ChatClient konvencijos |
| `KNOWN-ISSUES.md` | 2026-03-11 | Pašalinta išspręsta problema Nr. 7, liko 6 atviros problemos |
| `csharp/csharp.csproj` | 2026-03-11 | Tarpplatforminis TFM, WinML/pagrindinės SDK sąlyginės nuorodos |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Tarpplatforminis TFM, automatinis RID aptikimas |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Tarpplatforminis TFM, automatinis RID aptikimas |
| `csharp/BasicChat.cs` | 2026-03-11 | Pašalintas NPU try/catch sprendimas |
| `csharp/SingleAgent.cs` | 2026-03-11 | Pašalintas NPU try/catch sprendimas |
| `csharp/MultiAgent.cs` | 2026-03-11 | Pašalintas NPU try/catch sprendimas |
| `csharp/RagPipeline.cs` | 2026-03-11 | Pašalintas NPU try/catch sprendimas |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Pašalintas NPU try/catch sprendimas |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Tarpplatforminis .csproj pavyzdys |
| `AGENTS.md` | 2026-03-11 | Atnaujintos C# paketų ir TFM detalės |
| `CHANGELOG.md` | 2026-03-11 | Šis failas |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės atsisakymas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors stengiamės užtikrinti tikslumą, atkreipkite dėmesį, kad automatiniai vertimai gali turėti klaidų arba netikslumų. Originalus dokumentas jo gimtąja kalba turėtų būti laikomas autoritetingu šaltiniu. Kritinei informacijai rekomenduojamas profesionalus vertimas žmogaus. Mes neatsakome už bet kokius nesusipratimus ar neteisingą interpretaciją, kilusią naudojant šį vertimą.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->