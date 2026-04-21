# Changelog — Foundry Local Workshop

Sve značajne promjene u ovom radionici dokumentirane su u nastavku.

---

## 2026-03-11 — Dio 12 i 13, Web UI, Whisper prepravljanje, WinML/QNN ispravak i validacija

### Dodano
- **Dio 12: Izrada Web UI-a za Zava Creative Writer** — novi vodič radionice (`labs/part12-zava-ui.md`) s vježbama koje pokrivaju streaming NDJSON, preglednički `ReadableStream`, oznake statusa agenta uživo i streaming teksta članka u stvarnom vremenu
- **Dio 13: Radionica završena** — novi sažeti vodič (`labs/part13-workshop-complete.md`) s pregledom svih 12 dijelova, daljnjim idejama i poveznicama na resurse
- **Zava UI front end:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — zajedničko vanilla HTML/CSS/JS sučelje za preglednik koje koriste sva tri backenda
- **JavaScript HTTP poslužitelj:** `zava-creative-writer-local/src/javascript/server.mjs` — novi Express-stil HTTP poslužitelj koji omotava orkestrator za pristup putem preglednika
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` i `ZavaCreativeWriterWeb.csproj` — novi minimalni API projekt koji služi UI i streaming NDJSON-a
- **Generator audio uzoraka:** `samples/audio/generate_samples.py` — offline TTS skripta koristeći `pyttsx3` za generiranje WAV datoteka s temom Zava za Dio 9
- **Audio uzorak:** `samples/audio/zava-full-project-walkthrough.wav` — novi dulji audio uzorak za testiranje transkripcije
- **Skripta za validaciju:** `validate-npu-workaround.ps1` — automatizirana PowerShell skripta za validaciju NPU/QNN zaobilaznog rješenja u svim C# uzorcima
- **Mermaid dijagrami u SVG-u:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML podrška za više platformi:** Sva 3 C# `.csproj` datoteke (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) sada koriste uvjetni TFM i međusobno isključive reference paketa za podršku na više platformi. Na Windowsu: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (super set koji uključuje QNN EP dodatak). Na ne-Windows sustavima: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (osnovni SDK). Čvrsto kodirani `win-arm64` RID u Zava projektima zamijenjen je automatskim otkrivanjem. Zaobilazno rješenje za tranzitivni dio isključuje nativne resurse iz `Microsoft.ML.OnnxRuntime.Gpu.Linux` koji ima pokvarenu referencu na win-arm64. Prethodno try/catch za NPU je uklonjeno iz svih 7 C# datoteka.

### Promijenjeno
- **Dio 9 (Whisper):** Veliko prepravljanje — JavaScript sada koristi ugrađeni SDK `AudioClient` (`model.createAudioClient()`) umjesto ručnog ONNX Runtime izvođenja; ažurirani opisi arhitekture, usporedne tablice i dijagrami pipelines za JS/C# `AudioClient` pristup u odnosu na Python ONNX Runtime pristup
- **Dio 11:** Ažurirane navigacijske poveznice (sada vodi na Dio 12); dodani prikazani SVG dijagrami za tok pozivanja alata i sekvencu
- **Dio 10:** Ažurirana navigacija koja vodi preko Dio 12 umjesto da završava radionicu
- **Python Whisper (`foundry-local-whisper.py`):** Proširen dodatnim audio uzorcima i poboljšanim rukovanjem pogrešaka
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Prepravljeno za korištenje `model.createAudioClient()` s `audioClient.transcribe()` umjesto ručnih ONNX Runtime sesija
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Ažurirano za posluživanje statičkih UI datoteka uz API
- **Zava C# konzola (`zava-creative-writer-local/src/csharp/Program.cs`):** Uklonjeno NPU zaobilazno rješenje (sada upravlja WinML paket)
- **README.md:** Dodan dio 12 s tablicama primjera koda i dodacima backenda; dodan dio 13; ažurirani ciljevi učenja i struktura projekta
- **KNOWN-ISSUES.md:** Uklonjen riješeni Problem #7 (C# SDK NPU Model Varianta — sada upravlja WinML paket). Prebrojani preostali problemi od #1 do #6. Ažurirani detalji okruženja s .NET SDK 10.0.104
- **AGENTS.md:** Ažurirano stablo strukture projekta s novim unosima `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); ažurirani ključni C# paketi i detalji uvjetnog TFM-a
- **labs/part2-foundry-local-sdk.md:** Ažuriran `.csproj` primjer da prikazuje puni cross-platform obrazac s uvjetnim TFM-om, međusobno isključivim referencama paketa i objašnjenjem

### Validirano
- Sva 3 C# projekta (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) uspješno se grade na Windows ARM64
- Uzorak chat (`dotnet run chat`): model se učitava kao `phi-3.5-mini-instruct-qnn-npu:1` putem WinML/QNN — NPU varijanta se učitava izravno bez CPU pada na leđa
- Uzorak agent (`dotnet run agent`): radi od početka do kraja s višekratnim okretajima razgovora, izlazni kod 0
- Foundry Local CLI v0.8.117 i SDK v0.9.0 na .NET SDK 9.0.312

---

## 2026-03-11 — Ispravci koda, čišćenje modela, Mermaid dijagrami i validacija

### Ispravljeno
- **Svi 21 primjera koda (7 Python, 7 JavaScript, 7 C#):** Dodan `model.unload()` / `unload_model()` / `model.UnloadAsync()` za čišćenje pri izlazu kako bi se riješila upozorenja o curenju memorije OGA (Poznati Problem #4)
- **csharp/WhisperTranscription.cs:** Zamijenjena krhka relativna putanja `AppContext.BaseDirectory` s `FindSamplesDirectory()` koja ide prema gore u direktorije da pouzdano pronađe `samples/audio` (Poznati Problem #7)
- **csharp/csharp.csproj:** Zamijenjen čvrsto kodirani `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` automatskim zaobilaznim rješenjem koristeći `$(NETCoreSdkRuntimeIdentifier)` tako da `dotnet run` radi na bilo kojoj platformi bez `-r` parametra ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Promijenjeno
- **Dio 8:** Pretvoren evaluacijski iteracijski petlja iz ASCII okvira u renderiranu SVG sliku
- **Dio 10:** Pretvoren dijagram pipelines za kompilaciju iz ASCII strelica u renderiranu SVG sliku
- **Dio 11:** Pretvoreni dijagrami toka pozivanja alata i sekvence u renderirane SVG slike
- **Dio 10:** Premješten odjeljak "Radionica završena!" u Dio 11 (završni lab); zamijenjen poveznicom "Sljedeći koraci"
- **KNOWN-ISSUES.md:** Potpuna revalidacija svih problema naspram CLI v0.8.117. Uklonjeni riješeni: OGA curenje memorije (dodan cleanup), Whisper putanja (FindSamplesDirectory), HTTP 500 dugotrajno izvođenje (nije reproducibilno, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), ograničenja `tool_choice` (sada radi s `"required"` i specifičnim ciljanjem funkcija na qwen2.5-0.5b). Ažuriran JS Whisper problem — sada sve datoteke vraćaju prazni/binarni izlaz (regresija od v0.9.x, težina povećana na Major). Ažuriran #4 C# RID s automatskim zaobilaznim rješenjem i linkom [#497](https://github.com/microsoft/Foundry-Local/issues/497). Preostalo je 7 otvorenih problema.
- **javascript/foundry-local-whisper.mjs:** Ispravljen naziv varijable za čišćenje (`whisperModel` → `model`)

### Validirano
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — uspješno pokrenuti s čišćenjem
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — uspješno pokrenuti s čišćenjem
- C#: `dotnet build` uspijeva s 0 upozorenja, 0 pogrešaka (net9.0 cilj)
- Sve 7 Python datoteka prolaze `py_compile` provjeru sintakse
- Sve 7 JavaScript datoteka prolaze `node --check` validaciju sintakse

---

## 2026-03-10 — Dio 11: Pozivanje alata, proširenje SDK API-ja i pokrivenost modela

### Dodano
- **Dio 11: Pozivanje alata s lokalnim modelima** — novi vodič radionice (`labs/part11-tool-calling.md`) s 8 vježbi koje pokrivaju sheme alata, višekratni tok, višestruke pozive alata, prilagođene alate, pozivanje alata ChatClientom i `tool_choice`
- **Python uzorak:** `python/foundry-local-tool-calling.py` — pozivanje alata s `get_weather`/`get_population` alatima koristeći OpenAI SDK
- **JavaScript uzorak:** `javascript/foundry-local-tool-calling.mjs` — pozivanje alata koristeći nativni SDK `ChatClient` (`model.createChatClient()`)
- **C# uzorak:** `csharp/ToolCalling.cs` — pozivanje alata koristeći `ChatTool.CreateFunctionTool()` s OpenAI C# SDK-om
- **Dio 2, vježba 7:** Nativni `ChatClient` — `model.createChatClient()` (JS) i `model.GetChatClientAsync()` (C#) kao alternativa OpenAI SDK-u
- **Dio 2, vježba 8:** Varijante modela i odabir hardvera — `selectVariant()`, `variants`, tablica NPU varijanti (7 modela)
- **Dio 2, vježba 9:** Nadogradnje modela i osvježavanje kataloga — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Dio 2, vježba 10:** Modeli rezoniranja — `phi-4-mini-reasoning` s primjerima parsiranja `<think>` oznaka
- **Dio 3, vježba 4:** `createChatClient` kao alternativa OpenAI SDK-u, s dokumentacijom za uzorak povratnih poziva za streaming
- **AGENTS.md:** Dodane konvencije kodiranja za pozivanje alata, ChatClient i modele rezoniranja

### Promijenjeno
- **Dio 1:** Proširen katalog modela — dodani phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Dio 2:** Proširene tablice API referenci — dodani `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Dio 2:** Prebrojane vježbe 7-9 → 10-13 radi smještaja novih vježbi
- **Dio 3:** Ažurirana tablica ključnih saznanja za uključivanje nativnog ChatClienta
- **README.md:** Dodan dio 11 s tablicom primjera koda; dodan cilj učenja #11; ažurirano stablo strukture projekta
- **csharp/Program.cs:** Dodan slučaj `toolcall` u CLI usmjerivač i ažuriran pomoćni tekst

---

## 2026-03-09 — SDK v0.9.0 ažuriranje, britanski engleski i validacija

### Promijenjeno
- **Svi primjeri koda (Python, JavaScript, C#):** Ažurirani na Foundry Local SDK v0.9.0 API — ispravljeno `await catalog.getModel()` (nedostajao `await`), ažurirani uzorci inicijalizacije `FoundryLocalManager`, ispravljeno otkrivanje krajnje točke
- **Svi vodiči radionice (Dijelovi 1-10):** Pretvoreni u britanski engleski (colour, catalogue, optimised itd.)
- **Svi vodiči radionice:** Ažurirani primjeri koda SDK-a za usklađenost s v0.9.0 API-em
- **Svi vodiči radionice:** Ažurirane API tablice referenci i blokovi koda vježbi
- **JavaScript kritični popravak:** Dodan nedostajući `await` na `catalog.getModel()` — vraćao je `Promise`, ne `Model` objekt, što je uzrokovalo tihe greške u nastavku

### Validirano
- Svi Python primjeri uspješno se izvode protiv Foundry Local servisa
- Svi JavaScript primjeri uspješno se izvode (Node.js 18+)
- C# projekt se gradi i pokreće na .NET 9.0 (naprijed-kompatibilno iz net8.0 SDK skupa)
- 29 datoteka izmijenjeno i validirano kroz radionicu

---

## Indeks datoteka

| Datoteka | Posljednje ažuriranje | Opis |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Proširen katalog modela |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nove vježbe 7-10, proširene API tablice |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nova vježba 4 (ChatClient), ažurirani zaključci |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + britanski engleski |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + britanski engleski |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + britanski engleski |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + britanski engleski |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid dijagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + britanski engleski |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid dijagram, Workshop Complete premješten u Part 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Novi laboratorij, Mermaid dijagrami, odjeljak Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Novo: primjer pozivanja alata |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Novo: primjer pozivanja alata |
| `csharp/ToolCalling.cs` | 2026-03-10 | Novo: primjer pozivanja alata |
| `csharp/Program.cs` | 2026-03-10 | Dodana `toolcall` CLI naredba |
| `README.md` | 2026-03-10 | Part 11, struktura projekta |
| `AGENTS.md` | 2026-03-10 | Pozivanje alata + konvencije ChatClient-a |
| `KNOWN-ISSUES.md` | 2026-03-11 | Uklonjen riješeni Problem #7, ostalo 6 otvorenih problema |
| `csharp/csharp.csproj` | 2026-03-11 | Cross-platform TFM, WinML/base SDK uvjetni refs |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Cross-platform TFM, automatsko otkrivanje RID-a |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Cross-platform TFM, automatsko otkrivanje RID-a |
| `csharp/BasicChat.cs` | 2026-03-11 | Uklonjen NPU try/catch workaround |
| `csharp/SingleAgent.cs` | 2026-03-11 | Uklonjen NPU try/catch workaround |
| `csharp/MultiAgent.cs` | 2026-03-11 | Uklonjen NPU try/catch workaround |
| `csharp/RagPipeline.cs` | 2026-03-11 | Uklonjen NPU try/catch workaround |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Uklonjen NPU try/catch workaround |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Cross-platform .csproj primjer |
| `AGENTS.md` | 2026-03-11 | Ažurirani C# paketi i detalji TFM-a |
| `CHANGELOG.md` | 2026-03-11 | Ova datoteka |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:
Ovaj dokument preveden je koristeći AI uslugu prevođenja [Co-op Translator](https://github.com/Azure/co-op-translator). Iako težimo točnosti, imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku treba smatrati autoritativnim izvorom. Za kritične informacije preporučuje se profesionalni ljudski prijevod. Nismo odgovorni za bilo kakva nesporazuma ili pogrešne interpretacije proizašle iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->