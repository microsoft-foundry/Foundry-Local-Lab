# បញ្ចីផ្លាស់ប្តូរ — វគ្គបណ្ដុះបណ្ដាល Foundry Local

ការផ្លាស់ប្តូរពិសេសទាំងអស់ក្នុងវគ្គបណ្ដុះបណ្ដាលនេះ ត្រូវបានកត់ត្រាខាងក្រោម។

---

## 2026-03-11 — ផ្នែក 12 & 13, Web UI, ការសរសេរឡើងវិញ Whisper, ការជួសជុល WinML/QNN, និងការធ្វើតេស្តតម្លាភាព

### ត្រូវបានបន្ថែម
- **ផ្នែក 12: ការបង្កើត Web UI សម្រាប់ Zava Creative Writer** — មគ្គុទេសក៍មេរៀនថ្មី (`labs/part12-zava-ui.md`) មានលំហាត់គ្របដណ្តប់ការផ្ទេរទិន្នន័យ NDJSON ជាបន្ត, `ReadableStream` សម្រាប់កម្មវិធីរុករក, សញ្ញាស្ថិតិភាគីចាក់ផ្ទាល់, និងការផ្ទេរអត្ថបទអត្ថបទពេលវេលា
- **ផ្នែក 13: ការសម្រេចចិត្តវគ្គបណ្ដុះបណ្ដាល** — មេរៀនសង្ខេបថ្មី (`labs/part13-workshop-complete.md`) មានការសង្ស័យឡើងវិញនូវផ្នែកទាំង 12, គំនិតបន្ថែម និងតំណភ្ជាប់ធនធាន
- **ផ្នែកមុខ UI Zava:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — មុខងារ Vanilla HTML/CSS/JS ដែលចែករំលែកដោយ backend ទាំងបី
- **ម៉ាស៊ីនបម្រើ HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — ម៉ាស៊ីនបម្រើ HTTP ស្ទីល Express ថ្មី វេចខ្ចប់អូរកេស្ត្រេតែរួមសម្រាប់ការចូលប្រើដោយកម្មវិធីរុករក
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` និង `ZavaCreativeWriterWeb.csproj` — គម្រោង API តិចតួចថ្មី សម្រាប់បម្រើ UI និងផ្ទេរទិន្នន័យ NDJSON
- **កម្មវិធីបង្កើតសំលេងស្តាប់ពីឧបករណ៍:** `samples/audio/generate_samples.py` — ស្គ្រីប TTS អាផនឡាញ ប្រើ `pyttsx3` បង្កើតឯកសារ WAV សំរាប់ប្រធាន Zava ក្នុងផ្នែក 9
- **សំឡេងឧទាហរណ៍:** `samples/audio/zava-full-project-walkthrough.wav` — ឧទាហរណ៍សំឡេងវែងថ្មីសម្រាប់ការបញ្ចូលសំលេង
- **ស្គ្រីបត្រួតពិនិត្យតម្លាភាព:** `validate-npu-workaround.ps1` — ស្គ្រីប PowerShell ប្រតិបត្តិការបណ្តាញដើម្បីធ្វើតេស្តចំណុចបញ្ហា NPU/QNN ទូទាំងឧទាហរណ៍ C#
- **រូបភាព Mermaid SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **ការគាំទ្រ WinML លំដាប់រូបមន្តចម្រុះ:** ឯកសារ C# `.csproj` ទាំង 3 (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ប្រើ TFM មានលក្ខខណ្ឌ និងកញ្ចប់សម្រាប់គាំទ្រចម្រុះទ្រឹស្តី។ នៅលើរបព័ន្ធ Windows: TFM `net9.0-windows10.0.26100` និងកញ្ចប់ `Microsoft.AI.Foundry.Local.WinML` (ជាសំបុត្រលើ QNN EP plugin). ក្រៅរបព័ន្ធ Windows: TFM `net9.0` និងកញ្ចប់ `Microsoft.AI.Foundry.Local` (SDK មូលដ្ឋាន). រៀបចំពាក្យបញ្ជា `win-arm64` RID ក្នុងគម្រោង Zava ត្រូវបានជំនួសជាមួយការស្វែងរកដោយស្វ័យប្រវត្តិ។ ការត្រួតពិនិត្យជំហុសការពារ dependencies ត្រូវបានដកចេញទ្រង់ទ្រាយ native ពី `Microsoft.ML.OnnxRuntime.Gpu.Linux` ដែលមានការយោង win-arm64 ខូចខាត។ ការជួសជុលត្រួតពិនិត្យ NPU តាម try/catch ត្រូវបានដកចេញពីឯកសារ C# ទាំង 7។

### ត្រូវបានផ្លាស់ប្តូរ
- **ផ្នែក 9 (Whisper):** ការសរសេរឡើងវិញសំខាន់ — JavaScript ឥឡូវប្រើ `AudioClient` built-in របស់ SDK (`model.createAudioClient()`) ជំនួស session ONNX Runtime; ពិពណ៌នាថាស្ថាបត្យកម្ម, តារាងប្រៀបធៀប និងរូបភាព pipeline បានបច្ចប្បន្នភាព សម្រាប់ JS/C# `AudioClient` ប្រឆាំង Python ONNX Runtime
- **ផ្នែក 11:** ធ្វើបច្ចប្បន្នភាពតំណរនាវែនាប្រព័ន្ធ (ឥឡូវបង្ហាញផ្នែក 12); បន្ថែមរូបភាព SVG សម្រាប់លំហូរហៅឧបករណ៍ និងរៀបចំលំដាប់
- **ផ្នែក 10:** ធ្វើបច្ចប្បន្នភាពច្រកផ្លូវដឹកនាំតាមផ្នែក 12 ជំនួសការបញ្ចប់វគ្គបណ្ដុះបណ្ដាល
- **Python Whisper (`foundry-local-whisper.py`):** ពង្រីកជាមួយឧទាហរណ៍សំឡេង បន្ថែមកំណត់ហេតុបញ្ហាល្អប្រសើរ
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** សរសេរឡើងវិញប្រើ `model.createAudioClient()` ជាមួយ `audioClient.transcribe()` ជំនួស session ONNX Runtime ដោយដៃ
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** ធ្វើបច្ចប្បន្នភាពសេវាកម្ម UI Static ជាមួយ API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** ដកចេញការជួសជុល NPU (ឥឡូវគ្រប់គ្រងដោយកញ្ចប់ WinML)
- **README.md:** បន្ថែមផ្នែក 12 ជាមួយតារាងឧទាហរណ៍កូដ និងបន្ថែម backend; បន្ថែមផ្នែក 13; បច្ចប្បន្នភាពគោលបំណងសិក្សា និងរចនាសម្ព័ន្ធគម្រោង
- **KNOWN-ISSUES.md:** ដកចេញបញ្ហា #7 (C# SDK NPU Model Variant – ឥឡូវគ្រប់គ្រងដោយកញ្ចប់ WinML); លេខសម្គាល់បញ្ហាថ្មីជា #1–#6; បច្ចប្បន្នភាពលម្អិត​បរិយាកាសជាមួយ .NET SDK 10.0.104
- **AGENTS.md:** បច្ចប្បន្នភាពរចនាសម្ព័ន្ធគម្រោងជាមួយទិន្នន័យ `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); បច្ចប្បន្នភាពកញ្ចប់ C# សំខាន់ និងព័ត៌មានលម្អិតលក្ខខណ្ឌ TFM
- **labs/part2-foundry-local-sdk.md:** ធ្វើបច្ចប្បន្នភាពឧទាហរណ៍ `.csproj` ដើម្បីបង្ហាញលំនាំទូទាំងយន្តការចម្រុះ ជាមួយ TFM លក្ខខណ្ឌ, កញ្ចប់ផ្សេងគ្នា និងសម្ដីអត្ថាធិប្បាយ

### បានធ្វើតេស្ត
- គម្រោង C# ទាំង 3 (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) បង្កើតបានជោគជ័យលើ Windows ARM64
- ឧទាហរណ៍ការសន្ទនា (`dotnet run chat`): លោតម៉ូដែលជា `phi-3.5-mini-instruct-qnn-npu:1` តាម WinML/QNN — variant NPU លោតបញ្ចូលបន្ទាត់ដោយផ្ទាល់ដោយគ្មាន fallback CPU
- ឧទាហរណ៍ភាគី (`dotnet run agent`): ប្រតិបត្តិទាំងដំណើរការជាមួយការសន្ទនាច្រើនជុំ, កូដចេញ 0
- ឧបករណ៍ Foundry Local CLI v0.8.117 និង SDK v0.9.0 នៅលើ .NET SDK 9.0.312

---

## 2026-03-11 — ការជួសជុលកូដ, ការសំអាតម៉ូដែល, រូបភាព Mermaid និងការធ្វើតេស្តតម្លាភាព

### កែប្រែ
- **ឧទាហរណ៍កូដទាំង 21 (Python 7, JavaScript 7, C# 7):** បន្ថែម `model.unload()` / `unload_model()` / `model.UnloadAsync()` នៅពេលចាកចេញដើម្បីដោះស្រាយការព្រមានរអាក់ចងចាំ OGA (បញ្ហាស្គាល់ #4)
- **csharp/WhisperTranscription.cs:** ដោះស្រាយមន្ទិលសម្រាប់ `AppContext.BaseDirectory` ជំនួសដោយ `FindSamplesDirectory()` ដោយឡើងលើថតដាក់ឯកសារដើម្បីស្វែងរក `samples/audio` បានយកចិត្តទុកដាក់ (បញ្ហាស្គាល់ #7)
- **csharp/csharp.csproj:** ជំនួស `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` ដោយប្រើ fallback ស្វ័យប្រវត្តិក្នុង `$(NETCoreSdkRuntimeIdentifier)` ដូច្នេះ `dotnet run` អនុវត្តនៅលើវេទិកាណាមួយដោយគ្មានប៉ារ៉ាម៉ែត្រពាក្យបញ្ជា `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### ត្រូវបានផ្លាស់ប្តូរ
- **ផ្នែក 8:** បម្លែងរង្វង់សាកល្បងជាបង្ហាញរូបភាព SVG ជំនួសរូបភាព ASCII box
- **ផ្នែក 10:** បម្លែងរូបភាព pipeline ប្រមូលកំរិតពី ASCII arrow ទៅ SVG រូបភាព
- **ផ្នែក 11:** បម្លែងរូបភាពលំហូរហៅឧបករណ៍ និងលំដាប់ជាលេខទៅ SVG រូបភាព
- **ផ្នែក 10:** ផ្នែក "Workshop Complete!" បានផ្លាស់ទីទៅផ្នែក 11 (មេរៀនចុងក្រោយ); ជំនួសដោយតំណ "Next Steps"
- **KNOWN-ISSUES.md:** ត្រួតពិនិត្យទាំងមូលបញ្ហាទាំងអស់ជាមួយ CLI v0.8.117។ ដកចេញបញ្ហាដែលបានដោះស្រាយ៖ កំហុសចងចាំ OGA (បន្ថែមការសំអាត), ផ្លូវ Whisper (FindSamplesDirectory), HTTP 500 sustained inference (មិនអាចធ្វើឱ្យបង្ហាញឡើងវិញបាន, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), ការកំណត់កំណត់ tool_choice (ឥឡូវអាចប្រើបានជាមួយ `"required"` និងគោលដៅមុខងារពិសេសលើ qwen2.5-0.5b)។ កែប្រែបញ្ហា JS Whisper — ឥឡូវឯកសារទាំងអស់ត្រឡប់លទ្ធផលទទេ/ប៊ីណារី (ការរំខានចេញពី v0.9.x, ជំហានគ្នាឡើងទៅធំ). បច្ចប្បន្នភាព #4 RID C# ជាមួយលក្ខខណ្ឌស្វ័យប្រវត្តិ និងតំណ [#497](https://github.com/microsoft/Foundry-Local/issues/497). មានបញ្ហាបើកចំហ 7 ត្រូវដោះស្រាយនៅចំពោះមុខ។
- **javascript/foundry-local-whisper.mjs:** កែឈ្មោះអថេរសំអាត (`whisperModel` → `model`)

### បានធ្វើតេស្ត
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — ប្រតិបត្តិបានជោគជ័យជាមួយការសំអាត
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — ប្រតិបត្តិបានជោគជ័យជាមួយការសំអាត
- C#: `dotnet build` ជោគជ័យដោយគ្មានសំបុត្រព្រមាន 0 និងកំហុស 0 (គោលដៅ net9.0)
- ឯកសារ Python ទាំង 7 បានជោគជ័យក្នុងការត្រួតពិនិត្យ សyntax `py_compile`
- ឯកសារ JavaScript ទាំង 7 បានជោគជ័យក្នុងការត្រួតពិនិត្យ Syntax `node --check`

---

## 2026-03-10 — ផ្នែក 11: ការហៅឧបករណ៍, ការពង្រីក API SDK និងការគ្របដណ្តប់ម៉ូដែល

### ត្រូវបានបន្ថែម
- **ផ្នែក 11: ការហៅឧបករណ៍ជាមួយម៉ូដែលក្នុងស្រុក** — មគ្គុទេសក៍មេរៀនថ្មី (`labs/part11-tool-calling.md`) មាន 8 លំហាត់គ្របដណ្តប់ស្តង់ដារ schema ឧបករណ៍, លំហូរជុំច្រើន, ការហៅឧបករណ៍ច្រើន, ឧបករណ៍ផ្ទាល់ខ្លួន, ការហៅឧបករណ៍ ChatClient និង `tool_choice`
- **ឧទាហរណ៍ Python:** `python/foundry-local-tool-calling.py` — ការហៅឧបករណ៍ `get_weather`/`get_population` ប្រើ OpenAI SDK
- **ឧទាហរណ៍ JavaScript:** `javascript/foundry-local-tool-calling.mjs` — ការហៅឧបករណ៍ប្រើ `ChatClient` ដើម SDK (`model.createChatClient()`)
- **ឧទាហរណ៍ C#:** `csharp/ToolCalling.cs` — ការហៅឧបករណ៍ប្រើ `ChatTool.CreateFunctionTool()` ជាមួយ OpenAI C# SDK
- **ផ្នែក 2, លំហាត់ 7:** Native `ChatClient` — `model.createChatClient()` (JS) និង `model.GetChatClientAsync()` (C#) ជាជម្រើសបន្ថែមរបស់ OpenAI SDK
- **ផ្នែក 2, លំហាត់ 8:** ផលបំណុលម៉ូដែល និងការជ្រើសរើសហត្ថបទ — `selectVariant()`, `variants`, តារាង variant NPU (7 ម៉ូដែល)
- **ផ្នែក 2, លំហាត់ 9:** ការលើកទឹកចិត្តម៉ូដែល និងសារពុលទីតាំងម៉ូដែល — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **ផ្នែក 2, លំហាត់ 10:** ម៉ូដែលហេតុផល — `phi-4-mini-reasoning` ជាមួយឧទាហរណ៍ការវិភាគ `<think>` tag
- **ផ្នែក 3, លំហាត់ 4:** `createChatClient` ជាជម្រើសជំនួស OpenAI SDK, ជាមួយលក្ខណៈ callback streaming ស្តីពីលំហាត់
- **AGENTS.md:** បន្ថែមការហៅឧបករណ៍, ChatClient និងការអភិវឌ្ឍម៉ូដែល Reasoning coding conventions

### ត្រូវបានផ្លាស់ប្តូរ
- **ផ្នែក 1:** ពង្រីកសារពុលម៉ូដែល — បន្ថែម phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **ផ្នែក 2:** ពង្រីកតារាង API — បន្ថែម `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **ផ្នែក 2:** ប្តូរលេខលំហាត់ 7-9 → 10-13 ដើម្បីផ្សារភ្ជាប់លំហាត់ថ្មី
- **ផ្នែក 3:** បច្ចប្បន្នភាពតារាង Key Takeaways រួមបញ្ចូល ChatClient ដើម
- **README.md:** បន្ថែមផ្នែក 11 ជាមួយតារាងឧទាហរណ៍កូដ; បន្ថែមគោលបំណងសិក្សា #11; បច្ចប្បន្នភាពរចនាសម្ព័ន្ធគម្រោង
- **csharp/Program.cs:** បន្ថែមករណី `toolcall` ទៅ CLI router និងបច្ចប្បន្នភាពអត្ថបទជំនួយ

---

## 2026-03-09 — ការអាប់ដេត SDK v0.9.0, ភាសាអង់គ្លេសបរបៀបចាស់ (British English), និងការត្រួតពិនិត្យតម្លាភាព

### ត្រូវបានផ្លាស់ប្តូរ
- **ឧទាហរណ៍កូដទាំងអស់ (Python, JavaScript, C#):** បច្ចប្បន្នភាពទៅ Foundry Local SDK v0.9.0 API — កែប្រែ `await catalog.getModel()` (ខកខាន `await`), បច្ចប្បន្នភាពលំនាំចាប់ផ្តើម `FoundryLocalManager`, កែចំណុចបង្ហាញ endpoint
- **មគ្គុទេសក៍វគ្គទាំងអស់ (ផ្នែក 1-10):** បម្លែងទៅភាសាអង់គ្លេសបរបៀបចាស់ (ពណ៌, បញ្ជី, ការបង្កើនល្អ, ល. )
- **មគ្គុទេសក៍វគ្គទាំងអស់:** បច្ចប្បន្នភាពឧទាហរណ៍កូដ SDK ដើម្បីសម្របតាម v0.9.0 API
- **មគ្គុទេសក៍វគ្គទាំងអស់:** បច្ចប្បន្នភាពតារាង API និងខ្នាតកូដលំហាត់
- **កែតម្រូវកូដ JavaScript ចាំបាច់:** បន្ថែម `await` ខកខានលើ `catalog.getModel()` — ត្រឡប់ដោយ `Promise` មិនមែន `Model` ទិន្នផលធ្វើឲ្យមានកំហុសស្ងាត់នៅក្រោមអ្នកប្រើ

### បានធ្វើតេស្ត
- ឧទាហរណ៍ Python ទាំងអស់ ប្រតិបត្តិបានជោគជ័យជាមួយសេវាកម្ម Foundry Local
- ឧទាហរណ៍ JavaScript ទាំងអស់ ប្រតិបត្តិបានជោគជ័យ (Node.js 18+)
- គម្រោង C# អាចបង្កើត និងរត់បានលើ .NET 9.0 (ជាការចូលរួមពី net8.0 SDK assembly)
- កែប្រែឯកសារ 29 និងបានធ្វើតេស្តក្នុងវគ្គបណ្ដុះបណ្ដាល

---

## បញ្ជីឯកសារ

| ឯកសារ | ថ្ងៃធ្វើបច្ចប្បន្នភាពចុងក្រោយ | សេចក្ដីពិពណ៌នា |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | ពង្រីកសារពុលម៉ូដែល |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | លំហាត់ថ្មី 7-10, ពង្រីកតារាង API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | លំហាត់ថ្មី 4 (ChatClient), បច្ចប្បន្នភាព Key Takeaways |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + ភាសាអង់គ្លេសបរបៀបចាស់ |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + ភាសាអង់គ្លេសបរបៀបចាស់ |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK បោះពុម្ព v0.9.0 + ភាសាអង់គ្លេសប៊្រិទាន |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK បោះពុម្ព v0.9.0 + ភាសាអង់គ្លេសប៊្រិទាន |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | គំនូស Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK បោះពុម្ព v0.9.0 + ភាសាអង់គ្លេសប៊្រិទាន |
| `labs/part10-custom-models.md` | 2026-03-11 | គំនូស Mermaid, បម្លែង Workshop Complete ទៅផ្នែក 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | មន្ទីរពិសោធន៍ថ្មី, គំនូស Mermaid, ផ្នែក Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | ថ្មី: ឧទាហរណ៍ហៅឧបករណ៍ |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | ថ្មី: ឧទាហរណ៍ហៅឧបករណ៍ |
| `csharp/ToolCalling.cs` | 2026-03-10 | ថ្មី: ឧទាហរណ៍ហៅឧបករណ៍ |
| `csharp/Program.cs` | 2026-03-10 | បន្ថែមពាក្យបញ្ជា CLI `toolcall` |
| `README.md` | 2026-03-10 | ផ្នែក 11, រចនាសម្ព័ន្ធគម្រោង |
| `AGENTS.md` | 2026-03-10 | ហៅឧបករណ៍ + ដំណើរការជាមួយ ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | លុបបញ្ហាគ្រប់ Issue #7 រួចរាល់, នៅសល់បញ្ហា 6 ចំហ |
| `csharp/csharp.csproj` | 2026-03-11 | TFM សម្រាប់គ្រប់វេទិកា, ឯកសារយោង WinML/base SDK លក្ខណៈសម្រាប់លក្ខខណ្ឌ |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM សម្រាប់គ្រប់វេទិកា, ការរកអត្តសញ្ញាណ RID ដោយស្វ័យប្រវត្តិ |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM សម្រាប់គ្រប់វេទិកា, ការរកអត្តសញ្ញាណ RID ដោយស្វ័យប្រវត្តិ |
| `csharp/BasicChat.cs` | 2026-03-11 | យកចេញការជួយ NPU try/catch workaround |
| `csharp/SingleAgent.cs` | 2026-03-11 | យកចេញការជួយ NPU try/catch workaround |
| `csharp/MultiAgent.cs` | 2026-03-11 | យកចេញការជួយ NPU try/catch workaround |
| `csharp/RagPipeline.cs` | 2026-03-11 | យកចេញការជួយ NPU try/catch workaround |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | យកចេញការជួយ NPU try/catch workaround |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | ឧទាហរណ៍ .csproj សម្រាប់គ្រប់វេទិកា |
| `AGENTS.md` | 2026-03-11 | កែប្រែវត្ថុ C# និងព័ត៌មានលម្អិត TFM |
| `CHANGELOG.md` | 2026-03-11 | ឯកសារនេះ |