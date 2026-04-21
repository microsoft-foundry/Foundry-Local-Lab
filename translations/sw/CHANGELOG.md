# Mabadiliko — Warsha ya Foundry Local

Mabadiliko yote muhimu kwa warsha hii yameandikwa hapa chini.

---

## 2026-03-11 — Sehemu 12 & 13, Kiolesura cha Mtandao, Uandikaji Upya wa Whisper, Marekebisho ya WinML/QNN, na Uhakiki

### Imekuwa
- **Sehemu 12: Kujenga Kiolesura cha Mtandao kwa Mwandishi wa Ubunifu wa Zava** — mwongozo mpya wa maabara (`labs/part12-zava-ui.md`) ikiwa na mazoezi yanayohusisha mtiririko wa NDJSON, kivinjari `ReadableStream`, bages za hali ya wakala wa moja kwa moja, na mtiririko wa maandishi wa makala kwa wakati halisi
- **Sehemu 13: Warsha Imekamilika** — maabara mpya ya muhtasari (`labs/part13-workshop-complete.md`) ikiwa na muhtasari wa sehemu zote 12, mawazo zaidi, na viungo vya rasilimali
- **Mbele ya Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — kiolesura cha kivinjari cha vanilla HTML/CSS/JS kinachoshirikiwa na backend zote tatu
- **Seva ya HTTP ya JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — seva mpya ya mtindo wa Express inayozunguka mratibu kwa ufikiaji wa kupitia kivinjari
- **Backend ya C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` na `ZavaCreativeWriterWeb.csproj` — mradi mpya wa API wa msingi unaotumika kuhudumia UI na mtiririko wa NDJSON
- **Kizalishaji cha sampuli za sauti:** `samples/audio/generate_samples.py` — script ya TTS isiyo mtandao ikitumia `pyttsx3` kuzalisha faili za WAV za mandhari ya Zava kwa Sehemu ya 9
- **Sampuli ya sauti:** `samples/audio/zava-full-project-walkthrough.wav` — sampuli mpya ya sauti ndefu kwa majaribio ya uandikishaji
- **Script ya uthibitisho:** `validate-npu-workaround.ps1` — script ya PowerShell ya moja kwa moja kuthibitisha suluhisho la NPU/QNN kwa sampuli zote za C#
- **Mchoro wa Mermaid SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Msaada wa WinML wa kuvuka majukwaa:** Faili zote 3 za C# `.csproj` (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) sasa zinatumia TFM ya masharti na rejeleo za kifurushi zinazoachana kwa pamoja kwa msaada wa kuvuka majukwaa. Kwenye Windows: TFM ya `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (jumla inayojumuisha kiendelezi cha QNN EP). Kwenye si-Windows: TFM ya `net9.0` + `Microsoft.AI.Foundry.Local` (SDK msingi). RID ya `win-arm64` iliyowekwa vigumu katika miradi ya Zava ilibadilishwa na kugunduliwa moja kwa moja. Suluhisho la utegemezi wa mzunguko linalozuia mali za asili kutoka `Microsoft.ML.OnnxRuntime.Gpu.Linux` ambayo ina rejeleo lililovunjika la win-arm64. Jaribio la zamani la suluhisho la NPU limeondolewa kutoka kwa faili zote 7 za C#.

### Imebadilika
- **Sehemu 9 (Whisper):** Uandikaji upya mkubwa — JavaScript sasa inatumia `AudioClient` iliyojengwa ya SDK (`model.createAudioClient()`) badala ya kuendesha tuli ONNX Runtime; maelezo ya usanifu yaliyosasishwa, meza za kulinganisha, na michoro ya mchakato kuonyesha njia ya `AudioClient` ya JS/C# dhidi ya njia ya Python ONNX Runtime
- **Sehemu 11:** Viungo vya urambazaji vilivyosasishwa (sasa vinaelekeza sehemu ya 12); michoro ya SVG ya mchakato wa kuitwa kwa zana na mfuatano imejumuishwa
- **Sehemu 10:** Urambazaji umesasishwa kuonyesha sehemu ya 12 badala ya kumaliza warsha
- **Python Whisper (`foundry-local-whisper.py`):** Imepanuliwa na sampuli za sauti zaidi na kuboresha utunzaji wa makosa
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Imeandikwa upya kutumia `model.createAudioClient()` na `audioClient.transcribe()` badala ya vikao vya mwongozo vya ONNX Runtime
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Imesasishwa kuhudumia faili za UI huru pamoja na API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** Suluhisho la NPU limeondolewa (sasa linaendeshwa na kifurushi cha WinML)
- **README.md:** Imeongezwa sehemu ya 12 yenye meza za sampuli za msimbo na nyongeza za backend; imeongezwa sehemu ya 13; malengo ya kujifunza na muundo wa mradi vimesasishwa
- **KNOWN-ISSUES.md:** Tatizo liliotatuliwa #7 (SDK ya C# NPU Model Variant — sasa linaendeshwa na kifurushi cha WinML) limeondolewa. Masuala yaliyobaki yamenambariwa upya kutoka #1–#6. Maelezo ya mazingira yameboreshwa na .NET SDK 10.0.104
- **AGENTS.md:** Muundo wa mti wa mradi umesasishwa na takwimu mpya `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); vifurushi vikuu vya C# na maelezo ya TFM ya masharti vimesasishwa
- **labs/part2-foundry-local-sdk.md:** Mfano wa `.csproj` umesasishwa kuonyesha njia kamili ya kuvuka majukwaa na TFM ya masharti, rejeleo za kifurushi zenye kubana na maelezo ya ufafanuzi

### Imethibitishwa
- Miradi yote 3 ya C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) imetengenezwa kwa mafanikio kwenye Windows ARM64
- Sampuli ya Chat (`dotnet run chat`): modeli inapakia kama `phi-3.5-mini-instruct-qnn-npu:1` kupitia WinML/QNN — toleo la NPU linapakia moja kwa moja bila kupungua kwenye CPU
- Sampuli ya Agent (`dotnet run agent`): inaendesha kutoka mwanzo hadi mwisho na mazungumzo ya mizunguko mingi, nambari ya kutoka 0
- CLI ya Foundry Local v0.8.117 na SDK v0.9.0 juu ya .NET SDK 9.0.312

---

## 2026-03-11 — Marekebisho ya Msimbo, Usafishaji wa Modeli, Michoro ya Mermaid, na Uhakiki

### Imerekebishwa
- **Sampuli zote 21 za msimbo (7 Python, 7 JavaScript, 7 C#):** Imeongezwa `model.unload()` / `unload_model()` / `model.UnloadAsync()` usafishaji wakati wa kutoka ili kutatua onyo la mmomonyoko wa kumbukumbu ya OGA (Tatizo lililojulikana #4)
- **csharp/WhisperTranscription.cs:** Njia dhaifu ya `AppContext.BaseDirectory` ilibadilishwa na `FindSamplesDirectory()` inayotembea juu kwenye folda ili kupata `samples/audio` kwa uhakika (Tatizo lililojulikana #7)
- **csharp/csharp.csproj:** `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` iliyowekwa vigumu ilibadilishwa na kugundua kiotomatiki kwa kutumia `$(NETCoreSdkRuntimeIdentifier)` ili `dotnet run` ifanye kazi kwenye jukwaa lolote bila bendera ya `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Imebadilika
- **Sehemu 8:** Mzunguko wa tathmini ulioendeshwa na eval ulibadilishwa kutoka mchoro wa boksi la ASCII kuwa picha ya SVG iliyochorwa
- **Sehemu 10:** Mchoro wa mchakato wa mkusanyiko ulibadilishwa kutoka mishale ya ASCII kuwa picha ya SVG iliyochorwa
- **Sehemu 11:** Michoro ya mchakato wa kuitwa kwa zana na mfuatano ilibadilishwa kuwa picha za SVG zilizochorwa
- **Sehemu 10:** Sehemu ya "Warsha Imekamilika!" ilihamishwa kwenda Sehemu ya 11 (maabara ya mwisho); ilibadilishwa na kiungo cha "Hatua Zifuatazo"
- **KNOWN-ISSUES.md:** Uhakiki kamili wa masuala yote dhidi ya CLI v0.8.117. Masuala yaliyotatuliwa yameondolewa: mmomonyoko wa kumbukumbu ya OGA (usafishaji umeongezwa), njia ya Whisper (FindSamplesDirectory), inference ya HTTP 500 iliyodumu (haitarudi, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), mipaka ya tool_choice (sasa inafanya kazi na `"required"` na lengo la kazi maalum kwenye qwen2.5-0.5b). Tatizo la JS Whisper limeboreshwa — sasa faili zote zinarudisha matokeo tupu/za binary (kutoka kwa toleo v0.9.x, ukali umeongezwa kuwa Kuu). RID ya #4 C# imesasishwa na suluhisho la kugundua kiotomatiki na kiungo cha [#497](https://github.com/microsoft/Foundry-Local/issues/497). Masuala 7 bado yamefunguliwa.
- **javascript/foundry-local-whisper.mjs:** Jina la kigezo cha usafishaji limerekebishwa (`whisperModel` → `model`)

### Imethibitishwa
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — zinaendesha kwa mafanikio na usafishaji
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — zinaendesha kwa mafanikio na usafishaji
- C#: `dotnet build` inafanikiwa bila onyo au makosa (mlinzi net9.0)
- Faili zote 7 za Python hupita ukaguzi wa msimbo wa `py_compile`
- Faili zote 7 za JavaScript hupita uthibitisho wa msimbo wa `node --check`

---

## 2026-03-10 — Sehemu 11: Kuitwa kwa Zana, Upanuzi wa API ya SDK, na Mipako ya Modeli

### Imekuwa
- **Sehemu 11: Kuitwa kwa Zana kwa Modeli za Mitaa** — mwongozo mpya wa maabara (`labs/part11-tool-calling.md`) wenye mazoezi 8 yanayojumuisha skimu za zana, mchakato wa mizunguko mingi, kuitwa kwa zana nyingi, zana maalum, kuitwa kwa zana za ChatClient, na `tool_choice`
- **Sampuli ya Python:** `python/foundry-local-tool-calling.py` — kuitwa kwa zana kwa kutumia zana za `get_weather`/`get_population` kwa SDK ya OpenAI
- **Sampuli ya JavaScript:** `javascript/foundry-local-tool-calling.mjs` — kuitwa kwa zana kwa kutumia `ChatClient` ya asili ya SDK (`model.createChatClient()`)
- **Sampuli ya C#:** `csharp/ToolCalling.cs` — kuitwa kwa zana kwa kutumia `ChatTool.CreateFunctionTool()` na SDK ya OpenAI C#
- **Sehemu 2, Zoazoezi 7:** `ChatClient` ya asili — `model.createChatClient()` (JS) na `model.GetChatClientAsync()` (C#) kama mbadala wa SDK ya OpenAI
- **Sehemu 2, Zoazoezi 8:** Tofauti za modeli na uchaguzi wa vifaa — `selectVariant()`, `variants`, jedwali la toleo la NPU (modeli 7)
- **Sehemu 2, Zoazoezi 9:** Maboresho ya modeli na masasisho ya katalogi — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Sehemu 2, Zoazoezi 10:** Modeli za hoja — `phi-4-mini-reasoning` na mifano ya uchambuzi wa lebo `<think>`
- **Sehemu 3, Zoazoezi 4:** `createChatClient` kama mbadala wa SDK ya OpenAI, pamoja na nyaraka za mtindo wa callback wa mtiririko
- **AGENTS.md:** Imeongezwa miongozo ya usanifu wa mitindo ya Kuitwa kwa Zana, ChatClient, na Modeli za Hoja

### Imebadilika
- **Sehemu 1:** Katalogi ya modeli imeongezwa — kutumwa phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Sehemu 2:** Jedwali la rejeleo za API limeongezwa — kuongezwa `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Sehemu 2:** Nambari za mazoezi 7-9 zilibadilishwa kuwa 10-13 ili kuendana na mazoezi mapya
- **Sehemu 3:** Jedwali la Mambo Muhimu limeboreshwa kuonyesha ChatClient ya asili
- **README.md:** Imeongezwa sehemu ya 11 na jedwali la sampuli za msimbo; lengo la kujifunza #11 limeongezwa; mti wa muundo wa mradi umesasishwa
- **csharp/Program.cs:** Kama ya `toolcall` imeongezwa kwa mpangilio wa CLI na maandishi ya msaada yamesasishwa

---

## 2026-03-09 — Sasisho la SDK v0.9.0, Kiingereza cha Uingereza, na Zarani la Uhakiki

### Imebadilika
- **Sampuli zote za msimbo (Python, JavaScript, C#):** Zimesasishwa kwa API ya Foundry Local SDK v0.9.0 — marekebisho ya `await catalog.getModel()` (ilikuwa bila `await`), mifumo ya kuanzisha `FoundryLocalManager` imesasishwa, kugundua vituo kulirekebishwa
- **Mwongozo wote wa maabara (Sehemu 1-10):** Yamebadilishwa kuwa Kiingereza cha Uingereza (rangi, katalogi, boreshwa, n.k.)
- **Mwongozo wote wa maabara:** Marejeo ya mifano ya msimbo ya SDK yamesasishwa kuendana na API ya v0.9.0
- **Mwongozo wote wa maabara:** Jedwali za rejeleo za API na sehemu za msimbo wa mazoezi zimesasishwa
- **Rekebisho muhimu la JavaScript:** Imeongezwa `await` iliyoikosekana kwenye `catalog.getModel()` — ilirudisha `Promise` badala ya kitu cha `Model`, kinasababisha kushindikana kwa kimya kando ya njia

### Imethibitishwa
- Sampuli zote za Python zinaendesha kwa mafanikio dhidi ya huduma ya Foundry Local
- Sampuli zote za JavaScript zinaendesha kwa mafanikio (Node.js 18+)
- Mradi wa C# unajengwa na kuendesha kwenye .NET 9.0 (mambo yanayotangulia kutoka net8.0 SDK)
- Faili 29 zimesasishwa na kuthibitishwa kwenye warsha

---

## Orodha ya Faili

| Faili | Imebadilishwa Mwisho | Maelezo |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Katalogi ya modeli imetuzwa |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Mazoezi mapya 7-10, meza za API zimepanuliwa |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Zoezi jipya 4 (ChatClient), yafuatayo yamesasishwa |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Kiingereza cha Uingereza |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Kiingereza cha Uingereza |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Kiingereza cha Uingereza |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Kiingereza cha Uingereza |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mchoro wa Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Kiingereza cha Uingereza |
| `labs/part10-custom-models.md` | 2026-03-11 | Mchoro wa Mermaid, Kura ya Warsha Imekamilika imehamishwa kwa Sehemu ya 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Maabara mpya, michoro ya Mermaid, sehemu ya Warsha Imekamilika |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Mpya: sampuli ya kuita chombo |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Mpya: sampuli ya kuita chombo |
| `csharp/ToolCalling.cs` | 2026-03-10 | Mpya: sampuli ya kuita chombo |
| `csharp/Program.cs` | 2026-03-10 | Iliongezwa amri ya CLI `toolcall` |
| `README.md` | 2026-03-10 | Sehemu ya 11, muundo wa mradi |
| `AGENTS.md` | 2026-03-10 | Kuita chombo + kanuni za ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Tatizo lililoratibiwa #7 limetolewa, matatizo 6 bado yamebaki |
| `csharp/csharp.csproj` | 2026-03-11 | TFM ya kuvuka majukwaa, marejeleo ya WinML/base SDK kwa masharti |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM ya kuvuka majukwaa, utambuzi wa kiatomati wa RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM ya kuvuka majukwaa, utambuzi wa kiatomati wa RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Uondolewa wa suluhisho la jaribio/kushika NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Uondolewa wa suluhisho la jaribio/kushika NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Uondolewa wa suluhisho la jaribio/kushika NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Uondolewa wa suluhisho la jaribio/kushika NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Uondolewa wa suluhisho la jaribio/kushika NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Mfano wa .csproj ya kuvuka majukwaa |
| `AGENTS.md` | 2026-03-11 | Maelezo ya sasisho ya vifurushi vya C# na TFM |
| `CHANGELOG.md` | 2026-03-11 | Hili faili |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Kandido**:  
Hati hii imetafsiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Ingawa tunajitahidi kuhakikisha usahihi, tafadhali fahamu kuwa tafsiri za kiotomatiki zinaweza kuwa na makosa au upungufu wa usahihi. Hati ya asili katika lugha yake ya asili inapaswa kuzingatiwa kama chanzo cha mamlaka. Kwa habari nyeti, tafsiri ya mtaalamu wa kibinadamu inashauriwa. Hatutojibika kwa kutoelewana au tafsiri potofu zinazotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->