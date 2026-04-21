# Changelog — Foundry Local delavnica

Vse pomembne spremembe te delavnice so dokumentirane spodaj.

---

## 2026-03-11 — Del 12 in 13, spletni uporabniški vmesnik, prepis Whisperja, popravki WinML/QNN in validacija

### Dodano
- **Del 12: Ustvarjanje spletnega uporabniškega vmesnika za Zava Creative Writer** — nov vodič delavnice (`labs/part12-zava-ui.md`) z vajami, ki obsegajo pretočni NDJSON, `ReadableStream` v brskalniku, značke statusa agenta v živo in pretočno predvajanje besedila članka v realnem času
- **Del 13: Zaključek delavnice** — nova povzetna vaja (`labs/part13-workshop-complete.md`) z rekapitulacijo vseh 12 delov, nadaljnjimi idejami in povezavami do virov
- **Zava UI sprednji del:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — skupen preprost spletni vmesnik HTML/CSS/JS, ki ga uporabljajo vsi trije strežniški deli
- **JavaScript HTTP strežnik:** `zava-creative-writer-local/src/javascript/server.mjs` — nov Express-stilu HTTP strežnik, ki ovije orkestrator za dostop v brskalniku
- **C# ASP.NET Core strežniški del:** `zava-creative-writer-local/src/csharp-web/Program.cs` in `ZavaCreativeWriterWeb.csproj` — nov minimalni API projekt, ki postreže UI in pretočni NDJSON
- **Generator vzorcev zvoka:** `samples/audio/generate_samples.py` — skripta TTS brez povezave, ki uporablja `pyttsx3` za generiranje WAV datotek z Zava tematiko za del 9
- **Vzorec zvoka:** `samples/audio/zava-full-project-walkthrough.wav` — nov daljši vzorec zvoka za testiranje transkripcije
- **Validacijska skripta:** `validate-npu-workaround.ps1` — avtomatizirana PowerShell skripta za preverjanje zaobitja NPU/QNN v vseh C# vzorcih
- **Mermaid diagrami SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML podporo za več platform:** Vsi 3 C# `.csproj` datoteke (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) sedaj uporabljajo pogojni TFM in medsebojno izključujoče reference paketov za podporo več platformam. Na Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (nadzbor, ki vključuje QNN EP vtičnik). Na ne-Windows platformah: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (osnovni SDK). Trdo kodirani RID `win-arm64` v Zava projektih je bil nadomeščen z avtomatičnim zaznavanjem. Transitivna odvisnost je zaobšla vključitev nativnih sredstev iz `Microsoft.ML.OnnxRuntime.Gpu.Linux` zaradi pokvarjene reference win-arm64. Prejšnje poskusno/ulovitev za NPU je bilo odstranjeno iz vseh 7 C# datotek.

### Spremenjeno
- **Del 9 (Whisper):** Obsežen prepis — JavaScript zdaj uporablja vgrajeni `AudioClient` SDK-ja (`model.createAudioClient()`) namesto ročnega izvajanja ONNX Runtime; posodobljeni opisi arhitekture, primerjalne tabele in diagrami cevovoda za pristop JS/C# `AudioClient` v primerjavi s pristopom Python ONNX Runtime
- **Del 11:** Posodobljene navigacijske povezave (sedaj kažejo na del 12); dodani renderirani SVG diagrami poteka klicev orodij in sekvenca
- **Del 10:** Posodobljena navigacija, da vodi skozi del 12 namesto zaključka delavnice
- **Python Whisper (`foundry-local-whisper.py`):** Razširjen z dodatnimi zvočnimi vzorci in izboljšanim ravnanjem z napakami
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Prepisan za uporabo `model.createAudioClient()` z `audioClient.transcribe()` namesto ročnih ONNX Runtime sej
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Posodobljen za postrežbo statičnih UI datotek skupaj z API-jem
- **Zava C# konzola (`zava-creative-writer-local/src/csharp/Program.cs`):** Odstranjeno zaobitje NPU (zdaj upravlja WinML paket)
- **README.md:** Dodan razdelek za del 12 s tabelami primerov kode in dodatki na backendu; dodan razdelek za del 13; posodobljeni učni cilji in struktura projekta
- **KNOWN-ISSUES.md:** Odstranjena rešena težava #7 (C# SDK NPU Model Variant — zdaj ureja WinML paket). Preštevilčene preostale težave na #1–#6. Posodobljeni podatki o okolju z .NET SDK 10.0.104
- **AGENTS.md:** Posodobljeno drevo strukture projekta z novimi vnosi `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); posodobljene ključne C# pakete in podrobnosti pogojnega TFM
- **labs/part2-foundry-local-sdk.md:** Posodobljen primer `.csproj` za prikaz popolnega večplatformnega vzorca s pogojnim TFM, medsebojno izključujočimi referencami paketov in razlago

### Validirano
- Vsi 3 C# projekti (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) se uspešno sestavijo na Windows ARM64
- Vzorec pogovora (`dotnet run chat`): model se naloži kot `phi-3.5-mini-instruct-qnn-npu:1` preko WinML/QNN — NPU različica se naloži neposredno brez CPU fall-backa
- Vzorec agenta (`dotnet run agent`): uspešno izvede celoten potek z večkrožnim pogovorom, izhodna koda 0
- Foundry Local CLI v0.8.117 in SDK v0.9.0 na .NET SDK 9.0.312

---

## 2026-03-11 — Popravki kode, čiščenje modelov, Mermaid diagrami in validacija

### Popravljeno
- **Vsi 21 vzorcev kode (7 Python, 7 JavaScript, 7 C#):** Dodano čiščenje `model.unload()` / `unload_model()` / `model.UnloadAsync()` pri izhodu za odpravo opozoril o puščanju pomnilnika OGA (znana težava #4)
- **csharp/WhisperTranscription.cs:** Zamenjana krhka relativna pot `AppContext.BaseDirectory` z `FindSamplesDirectory()`, ki zanesljivo išče mapo `samples/audio` v nadrejenih mapah (znana težava #7)
- **csharp/csharp.csproj:** Zamenjan ročno vpisan `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` z avtomatičnim zaznavanjem prek `$(NETCoreSdkRuntimeIdentifier)`, tako da `dotnet run` deluje na katerikoli platformi brez `-r` zastavice ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Spremenjeno
- **Del 8:** Eval driven iteracijska zanka pretvorjena iz ASCII škatlastega diagrama v renderirano SVG sliko
- **Del 10:** Diagram kompilacijskega cevovoda pretvorjen iz ASCII puščic v renderirano SVG sliko
- **Del 11:** Diagrami poteka klicev orodij in sekvenc pretvorjeni v renderirane SVG slike
- **Del 10:** Razdelek "Delavnica zaključena!" premaknjen v del 11 (zadnja vaja); nadomeščen s povezavo "Naslednji koraki"
- **KNOWN-ISSUES.md:** Popolna ponovna validacija vseh težav glede na CLI v0.8.117. Odstranjene rešene: OGA Memory Leak (čiščenje dodano), Whisper pot (FindSamplesDirectory), HTTP 500 trajno napovedovanje (nereproducirano, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), omejitve tool_choice (zdaj deluje s `"required"` in ciljno usmerjenimi funkcijami na qwen2.5-0.5b). Posodobljena težava JS Whisper — zdaj vse datoteke vračajo prazne/bin metode (regresija od v0.9.x, resnost povišana na Osnovno). Posodobljen #4 C# RID z avtomatskim zaznavanjem zaobitja in [#497](https://github.com/microsoft/Foundry-Local/issues/497) povezava. Ostaja 7 odprtih težav.
- **javascript/foundry-local-whisper.mjs:** Popravljen ime spremenljivke za čiščenje (`whisperModel` → `model`)

### Validirano
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — uspešno se izvedejo s čiščenjem
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — uspešno se izvedejo s čiščenjem
- C#: `dotnet build` uspe brez opozoril in napak (ciljna net9.0)
- Vseh 7 Python datotek prestane sintaktično preverjanje `py_compile`
- Vseh 7 JavaScript datotek prestane sintaktično preverjanje `node --check`

---

## 2026-03-10 — Del 11: Klic orodij, razširitev SDK API-ja in pokritost modelov

### Dodano
- **Del 11: Klic orodij z lokalnimi modeli** — nov vodič delavnice (`labs/part11-tool-calling.md`) z 8 vajami, ki obsegajo sheme orodij, večkrožni potek, več klicev orodij, prilagojena orodja, klic orodij z `ChatClient` in `tool_choice`
- **Python vzorec:** `python/foundry-local-tool-calling.py` — klic orodij z orodji `get_weather`/`get_population`, ki uporabljajo OpenAI SDK
- **JavaScript vzorec:** `javascript/foundry-local-tool-calling.mjs` — klic orodij z uporabo izvornega `ChatClient` SDK-ja (`model.createChatClient()`)
- **C# vzorec:** `csharp/ToolCalling.cs` — klic orodij z uporabo `ChatTool.CreateFunctionTool()` in OpenAI C# SDK-ja
- **Del 2, vaja 7:** Izvorni `ChatClient` — `model.createChatClient()` (JS) in `model.GetChatClientAsync()` (C#) kot alternativa OpenAI SDK-ju
- **Del 2, vaja 8:** Različice modelov in izbira strojne opreme — `selectVariant()`, `variants`, tabela različic NPU (7 modelov)
- **Del 2, vaja 9:** Nadgradnje modelov in osvežitev kataloga — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Del 2, vaja 10:** Modeli za razumevanje — `phi-4-mini-reasoning` z primeri analize oznake `<think>`
- **Del 3, vaja 4:** `createChatClient` kot alternativa OpenAI SDK-ju s dokumentacijo vzorca za klic v povratni tok tokom
- **AGENTS.md:** Dodani konvencije za kodiranje za klic orodij, ChatClient in modele za razumevanje

### Spremenjeno
- **Del 1:** Razširjen katalog modelov — dodan phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Del 2:** Razširjene tabele API referenc — dodano `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Del 2:** Ponumerirane vaje 7-9 → 10-13 zaradi dodatnih vaj
- **Del 3:** Posodobljena tabela ključnih spoznanj za vključitev izvornega ChatClient
- **README.md:** Dodan razdelek del 11 s tabelo primerov kode; dodan učni cilj #11; posodobljeno drevo strukture projekta
- **csharp/Program.cs:** Dodan primer `toolcall` v CLI usmerjevalniku in posodobljen opis pomoči

---

## 2026-03-09 — Posodobitev SDK v0.9.0, britanska angleščina in validacijski krog

### Spremenjeno
- **Vsi vzorci kode (Python, JavaScript, C#):** Posodobljeni na Foundry Local SDK v0.9.0 API — popravljen `await catalog.getModel()` (manjkajoči `await`), posodobljeni vzorci inicializacije `FoundryLocalManager`, popravljen odkritje endpointev
- **Vsi vodiči delavnic (1-10):** Pretvorjeni v britansko angleščino (colour, catalogue, optimised itd.)
- **Vsi vodiči delavnic:** Posodobljeni primeri kode SDK za skladnost z v0.9.0 API-jem
- **Vsi vodiči delavnic:** Posodobljene tabele API referenc in code block-i vaj
- **JavaScript kritični popravek:** Dodan manjkajoči `await` v `catalog.getModel()` — vračal je `Promise` namesto `Model` objekta, kar je povzročalo tihe napake

### Validirano
- Vsi Python vzorci uspešno delujejo proti Foundry Local storitvi
- Vsi JavaScript vzorci se uspešno izvajajo (Node.js 18+)
- C# projekt se sestavi in izvaja na .NET 9.0 (naprej združljiv s net8.0 SDK sestavo)
- 29 datotek spremenjenih in validiranih v celotni delavnici

---

## Indeks datotek

| Datoteka | Nazadnje posodobljeno | Opis |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Razširjen katalog modelov |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nove vaje 7-10, razširjene tabele API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nova vaja 4 (ChatClient), posodobljena spoznanja |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + britanska angleščina |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + britanska angleščina |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + britanska angleščina |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + britanska angleščina |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + britanska angleščina |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, delavnica končana premaknjena na del 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nova laboratorijska vaja, Mermaid diagrami, odsek Delavnica končana |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Novo: primer klica orodja |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Novo: primer klica orodja |
| `csharp/ToolCalling.cs` | 2026-03-10 | Novo: primer klica orodja |
| `csharp/Program.cs` | 2026-03-10 | Dodana CLI ukaz `toolcall` |
| `README.md` | 2026-03-10 | Del 11, struktura projekta |
| `AGENTS.md` | 2026-03-10 | Klic orodja + konvencije ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Odstranjena rešena težava #7, ostaja 6 odprtih težav |
| `csharp/csharp.csproj` | 2026-03-11 | Večplatformni TFM, pogojne reference WinML/osnovni SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Večplatformni TFM, samodejno zaznavanje RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Večplatformni TFM, samodejno zaznavanje RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Odstranjena rešitev za poskus/funkcija NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Odstranjena rešitev za poskus/funkcija NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Odstranjena rešitev za poskus/funkcija NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Odstranjena rešitev za poskus/funkcija NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Odstranjena rešitev za poskus/funkcija NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Večplatformni .csproj primer |
| `AGENTS.md` | 2026-03-11 | Posodobljeni C# paketi in podrobnosti TFM |
| `CHANGELOG.md` | 2026-03-11 | Ta datoteka |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Omejitev odgovornosti**:
Ta dokument je bil preveden z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, vas opozarjamo, da avtomatizirani prevodi lahko vsebujejo napake ali netočnosti. Izvirni dokument v njegovem izvorno jeziku velja za avtoritativni vir. Za kritične informacije priporočamo strokovni človeški prevod. Nismo odgovorni za kakršne koli nesporazume ali napačne interpretacije, ki izhajajo iz uporabe tega prevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->