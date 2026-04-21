<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# វគ្គបណ្តុះបណ្តាល Foundry Local - សាងសង់កម្មវិធី AI លើឧបករណ៍ផ្ទាល់ខ្លួន

វគ្គបណ្តុះបណ្តាល​ផ្ទាល់សម្រាប់រត់ម៉ូដែលភាសាលើ​គ្រឿងម៉ាស៊ីន​របស់អ្នក និង​សាងសង់កម្មវិធីមានប្រាជ្ញាជាមួយ [Foundry Local](https://foundrylocal.ai) និង [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)។

> **Foundry Local គឺជាអ្វី?** Foundry Local ជាវមមានទម្រាស់ស្រាលមួយដែលអនុញ្ញាតឱ្យអ្នកទាញយក គ្រប់គ្រង និងបម្រើម៉ូដែលភាសាទាំងស្រុងលើឧបករណ៍របស់អ្នក។ វាបញ្ចេញ **API ដែលស្រដៀងនឹង OpenAI** ដើម្បីឲ្យឧបករណ៍ ឬ SDK ដែលគាំទ្រនឹង OpenAI អាចភ្ជាប់បាន - អត់បាច់មានគណនីមេឃណាស់។

---

## គោលបំណងសិក្សា

នៅចុងបញ្ចប់វគ្គបណ្តុះបណ្តាលនេះ អ្នកនឹងអាចធ្វើរឿងដូចខាងក្រោម៖

| # | គោលបំណង |
|---|-----------|
| 1 | តំឡើង Foundry Local និងគ្រប់គ្រងម៉ូដែលជាមួយ CLI |
| 2 | អភិវឌ្ឍន៍ជំនាញ API SDK Foundry Local សម្រាប់គ្រប់គ្រងម៉ូដែលប្រើកម្មវិធី |
| 3 | ភ្ជាប់ទៅម៉ាស៊ីនប្លុកសញ្ញាផ្ទាល់ខ្លួនប្រើ SDK Python, JavaScript និង C# |
| 4 | សាងសង់បណ្តាញបង្កើតបញ្ញាព្រមទាំងចាប់ផ្តើមចម្លើយពីទិន្នន័យផ្ទាល់ខ្លួន (RAG) |
| 5 | បង្កើតភ្នាក់ងារ AI ដែលមានការណែនាំ និងបុគ្គលិកលក្ខណៈបន្ត |
| 6 | រៀបចំដំណើរការជាមួយភ្នាក់ងារច្រើននិងចំណុចត្រលប់មតិយោបល់ |
| 7 | ស្ទាត់ជំនាញកម្មវិធីផ្នែកផលិតផលសំរាប់ការអភិវឌ្ឍន៍ - Zava Creative Writer |
| 8 | បង្កើតស៊ុមវាយតម្លៃជាមួយឯកសាររូបមន្ត និងការវាយតម្លៃ LLM ជាអ្នកវាយតម្លៃ |
| 9 | បម្លែងសំឡេងជាអត្ថបទដោយ Whisper - ព្រមទាំងប្រើ SDK Foundry Local លើឧបករណ៍ |
| 10 | រួមបញ្ចូល និងរត់ម៉ូដែលផ្ទាល់ខ្លួន ឬ Hugging Face ជាមួយ ONNX Runtime GenAI និង Foundry Local |
| 11 | អនុញ្ញាតម៉ូដែលផ្ទាល់ខ្លួនហៅមុខងារចេញក្រៅដោយលំនាំហៅឧបករណ៍ |
| 12 | បង្កើត UI នៅលើមេឃើញសំរាប់ Zava Creative Writer ជាមួយការផ្ទុកចរន្តពេលវេលពិត |

---

## លក្ខខណ្ឌមុនវគ្គបណ្តុះបណ្តាល

| លក្ខខណ្ឌ | ព័ត៌មានលម្អិត |
|-------------|---------|
| **\"Hardware\"** | នៅតិច RAM 8 GB (ណែនាំ 16 GB); CPU មានសមត្ថភាព AVX2 ឬ GPU ដែលគាំទ្រ |
| **ប្រព័ន្ធ​ប្រតិបត្តិការ** | Windows 10/11 (x64/ARM), Windows Server 2025, หรือ macOS 13+ |
| **Foundry Local CLI** | តំឡើងតាម `winget install Microsoft.FoundryLocal` (Windows) ឬ `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS)។ មើល [មគ្គុទេសក៍ចាប់ផ្តើម](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) សម្រាប់ពត៌មានលម្អិត។ |
| **ស្នូលភាសា** | **Python 3.9+** និង/ឬ **.NET 9.0+** និង/ឬ **Node.js 18+** |
| **Git** | សម្រាប់ចម្លងឃ្លាំងរបស់នេះ |

---

## ចាប់ផ្តើម

```bash
# 1. ចម្លងឃ្លាំងទិន្នន័យ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ផ្ទៀងផ្ទាត់ថា Foundry Local ត្រូវបានដំឡើង
foundry model list              # បញ្ជីម៉ូដែលដែលមាន
foundry model run phi-3.5-mini  # ចាប់ផ្តើមជជែកបែបអន្តរាគមន៍

# 3. ជ្រើសរើសភាសារបស់អ្នក (មើលមោងថ្នាក់ 2 សម្រាប់ការតំឡើងពេញលេញ)
```

| ភាសា | ចាប់ផ្តើមរហ័ស |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ផ្នែកនៃវគ្គបណ្តុះបណ្តាល

### ផ្នែក 1: ចាប់ផ្តើមជាមួយ Foundry Local

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local ជាអ្វី និងវិធីដំណើរការ
- តំឡើង CLI លើ Windows និង macOS
- ស្វែងរកម៉ូដែល - បញ្ជី ទាញយក ដំណើរការ
- យល់ដឹងអំពីឈ្មោះសម្ងាត់ម៉ូដែល និងច្រកផ្លូវកំពុងដំណើរការ

---

### ផ្នែក 2: ជ្រៅជាមួយ Foundry Local SDK

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- មូលហេតុដែលប្រើ SDK មិនប្រើ CLI សម្រាប់អភិវឌ្ឍកម្មវិធី
- ឯកសារពេញលេញសម្រាប់ Python, JavaScript, និង C#
- គ្រប់គ្រងសេវាកម្ម, ស្វែងរកសៀវភៅម៉ូដែល, វិវឌ្ឍន៍ម៉ូដែល (ទាញយក ផ្ទុក ផ្តាច់)
- តំរូវការចាប់ផ្តើមរហ័ស: បង្កើត Python, JavaScript `init()`, C# `CreateAsync()`
- ព័ត៌មាន `FoundryModelInfo`, ឈ្មោះសម្ងាត់ និងជ្រើសម៉ូដែលល្អបំផុតសម្រាប់រឹង

---

### ផ្នែក 3: SDK និង API

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- ភ្ជាប់ទៅ Foundry Local ពី Python, JavaScript និង C#
- ប្រើ SDK សម្រាប់គ្រប់គ្រងសេវាកម្មដោយកម្មវិធី
- ផ្តល់បទពិសោធន៍ការផ្សាយជាស្ត្រីម Chat តាម API ស្រដៀង OpenAI
- ឯកសាររបៀបប្រើ SDK សម្រាប់ភាសាតែមួយៗ

**ឧទាហរណ៍កូដ៖**

| ភាសា | ឯកសារ | សេចក្ដីពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat ផ្សាយជាស្ត្រីមមូលដ្ឋាន |
| C# | `csharp/BasicChat.cs` | Chat ផ្សាយជាស្ត្រីមជាមួយ .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat ផ្សាយជាស្ត្រីមជាមួយ Node.js |

---

### ផ្នែក 4: ការបង្កើតបញ្ញាព្រមទាំងចាប់ផ្តើម (RAG)

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG គឺជាអ្វី និងមូលហេតុដែលវាសំខាន់
- សាងសង់មូលដ្ឋានចំណេះដឹងក្នុងចងចាំដាច់
- ស្វែងរកតាមពាក្យគន្លឹះ និងការវាយពិន្ទុ
- បង្កើតសេចក្ដីណែនាំក្នុងប្រព័ន្ធត្រឹមត្រូវ
- ដំណើរការបណ្តាញ RAG ពេញលេញលើឧបករណ៍ផ្ទាល់ខ្លួន

**ឧទាហរណ៍កូដ៖**

| ភាសា | ឯកសារ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ផ្នែក 5: សាងសង់ភ្នាក់ងារ AI

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- ភ្នាក់ងារ AI ជាអ្វី (បិទជាមួយការហៅ LLM ផ្ទាល់)
- លំនាំ `ChatAgent` និង Microsoft Agent Framework
- សេចក្ដីណែនាំប្រព័ន្ធ, បុគ្គលិកលក្ខណៈ និងការពិភាក្សាច្រើនជាន់
- លទ្ធផលរចនាសម្ព័ន្ធ (JSON) ពីភ្នាក់ងារ

**ឧទាហរណ៍កូដ៖**

| ភាសា | ឯកសារ | សេចក្ដីពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | ភ្នាក់ងារតែមួយជាមួយ Agent Framework |
| C# | `csharp/SingleAgent.cs` | ភ្នាក់ងារតែមួយ (លំនាំ ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ភ្នាក់ងារតែមួយ (លំនាំ ChatAgent) |

---

### ផ្នែក 6: ដំណើរការជាមួយភ្នាក់ងារច្រើន

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- បណ្តាញភ្នាក់ងារច្រើន: ស្រាវជ្រាវ → សរសេរ → ធ្វើសម្ភាសន៍
- ការរៀបចំចែករំលែក និងចំណុចត្រលប់មតិ
- ការចែករំលែកការកំណត់ និងសម្របសម្រួលដំណើរការ
- រចនាដំណើរការជាមួយភ្នាក់ងារច្រើនផ្ទាល់ខ្លួន

**ឧទាហរណ៍កូដ៖**

| ភាសា | ឯកសារ | សេចក្ដីពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | បណ្តាញភ្នាក់ងារបី |
| C# | `csharp/MultiAgent.cs` | បណ្តាញភ្នាក់ងារបី |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | បណ្តាញភ្នាក់ងារបី |

---

### ផ្នែក 7: Zava Creative Writer - កម្មវិធីចុងក្រោយ

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- កម្មវិធីផលិតផលភាពច្រើនជាមួយ ៤ ភ្នាក់ងារបច្ចេកទេស
- បណ្តាញរៀបចំដោយអ្នកវាយតម្លៃជាមួយចំណុចត្រលប់មតិ
- ផ្សាយលទ្ធផលស្ទ្រីម, ស្វែងរកក្នុងкаталогផលិតផល, ដៃគូ JSON រចនាសម្ព័ន្ធ
- អនុវត្តពេញលេញក្នុង Python (FastAPI), JavaScript (Node.js CLI), និង C# (.NET console)

**ឧទាហរណ៍កូដ៖**

| ភាសា | ថត | សេចក្ដីពិពណ៌នា |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | សេវាកម្មបណ្តាញ FastAPI ជាមួយកែសម្រួល |
| JavaScript | `zava-creative-writer-local/src/javascript/` | កម្មវិធី Node.js CLI |
| C# | `zava-creative-writer-local/src/csharp/` | កម្មវិធី .NET 9 console |

---

### ផ្នែក 8: ការអភិវឌ្ឍន៍ដឹកនាំដោយការវាយតម្លៃ

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- បង្កើតស៊ុមវាយតម្លៃប្រព័ន្ធសម្រាប់ភ្នាក់ងារ AI ដោយប្រើឯកសារទិន្នន័យមាស
- ការត្រួតពិនិត្យមូលដ្ឋានច្បាប់ (ប្រវែង, ពាក្យគន្លឹះ, ពាក្យហាម) និង LLM ជាអ្នកវាយតម្លៃ
- ប្រៀបធៀបទាំងពីរផ្នែកសម្រាប់បម្លែងបំរើបន្ថែមជាមួយកាតពិន្ទុបូកសរុប
- ផ្សំលំនាំភ្នាក់ងារ Zava Editor ពីផ្នែក 7 ជាស៊ុមសាកល្បងក្រៅបណ្ដាញ
- តាមដាន Python, JavaScript និង C#

**ឧទាហរណ៍កូដ៖**

| ភាសា | ឯកសារ | សេចក្ដីពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | ស៊ុមវាយតម្លៃ |
| C# | `csharp/AgentEvaluation.cs` | ស៊ុមវាយតម្លៃ |
| JavaScript | `javascript/foundry-local-eval.mjs` | ស៊ុមវាយតម្លៃ |

---

### ផ្នែក 9: បម្លែងសំឡេងជាអត្ថបទជាមួយ Whisper

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- បម្លែងសំឡេងជាអត្ថបទដោយប្រើ OpenAI Whisper រត់នៅលើឧបករណ៍ផ្ទាល់ខ្លួន
- ការបំពានឯកជនភាពជាប្រភេទដំបូង - សំឡេងមិនចេញពីឧបករណ៍អ្នក
- តាមដាន Python, JavaScript និង C# ដោយប្រើ `client.audio.transcriptions.create()` (Python/JS) និង `AudioClient.TranscribeAudioAsync()` (C#)
- មានឯកសារ​សំឡេងគំរូរបស់ Zava សម្រាប់ហាត់ប្រាណដៃគូ

**ឧទាហរណ៍កូដ៖**

| ភាសា | ឯកសារ | សេចក្ដីពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | បម្លែងសំឡេង Whisper |
| C# | `csharp/WhisperTranscription.cs` | បម្លែងសំឡេង Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | បម្លែងសំឡេង Whisper |

> **ចំណាំ៖** ល្បែងនេះប្រើ **Foundry Local SDK** ដើម្បីទាញយក និងផ្ទុកម៉ូដែល Whisper ដោយកម្មវិធី បន្ទាប់ផ្ញើសំឡេងទៅចំណុចបញ្ចប់ដែលស្រដៀងនឹង OpenAI នៅក្នុងផ្ទៃក្នុងសម្រាប់បម្លែងអត្ថបទ។ ម៉ូដែល Whisper (`whisper`) ត្រូវបានរាយនាមនៅក្នុងបញ្ជី Foundry Local និងរត់លើឧបករណ៍ទាំងស្រុង - មិនបាច់កូនសោ API មេឃ ឬចូលប្រព័ន្ធបណ្តាញទេ។

---

### ផ្នែក 10: ប្រើម៉ូដែលផ្ទាល់ខ្លួន ឬ Hugging Face

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- បំលែងម៉ូដែល Hugging Face ទៅទ្រង់ទ្រាយ ONNX ដែលឆ្លាតវៃប្រើ ONNX Runtime GenAI Model Builder
- ការបំលែងមានលក្ខណៈពិសេសទៅHardware (CPU, NVIDIA GPU, DirectML, WebGPU) និងកំណត់តុល្យភាព (int4, fp16, bf16)
- បង្កើតឯកសារកំណត់រចនាសម្ព័ន្ធសម្រាប់ Foundry Local
- បន្ថែមម៉ូដែលបំលែងទៅផ្ទុក Foundry Local
- រត់ម៉ូដែលផ្ទាល់ខ្លួនតាម CLI, REST API និង OpenAI SDK
- ឧទាហរណ៍យោង: ការបំលែង Qwen/Qwen3-0.6B ពីដំណាក់កាលដំបូងទៅចុងក្រោយ

---

### ផ្នែក 11: ហៅឧបករណ៍ជាមួយម៉ូដែលផ្ទាល់ខ្លួន

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- អនុញ្ញាតម៉ូដែលផ្ទាល់ខ្លួនហៅមុខងារចេញក្រៅ (ហៅឧបករណ៍/មុខងារ)
- កំណត់ស្គីម៉ាឧបករណ៍ដោយប្រើទ្រង់ទ្រាយហៅមុខងារ OpenAI
- ដោះស្រាយការបើកដំណើរការជាច្រើនជាន់នៃការហៅឧបករណ៍
- អនុវត្តហៅឧបករណ៍ក្នុងផ្ទៃ និងម៉ូដែលទទួលលទ្ធផលវិញ
- ជ្រើសម៉ូដែលត្រឹមត្រូវសម្រាប់ការហៅឧបករណ៍ (Qwen 2.5, Phi-4-mini)
- ប្រើ `ChatClient` ដើម SDK សម្រាប់ហៅឧបករណ៍ (JavaScript)

**ឧទាហរណ៍កូដ៖**

| ភាសា | ឯកសារ | សេចក្ដីពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | ហៅឧបករណ៍ទាយទល់អាកាសធាតុ/ប្រជាជន |
| C# | `csharp/ToolCalling.cs` | ហៅឧបករណ៍ជាមួយ .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ហៅឧបករណ៍ជាមួយ ChatClient |

---

### ផ្នែក 12: សាងសង់ UI វេបសាយសម្រាប់ Zava Creative Writer

**មគ្គុទេសក៍ពិសោធន៍:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- បន្ថែមផ្នែកមុខប្រើនៅលើកម្មវិធី Zava Creative Writer
- បម្រើ UI រួមពី Python (FastAPI), JavaScript (Node.js HTTP), និង C# (ASP.NET Core)
- ប្រើ NDJSON ផ្សាយក្នុងពេលវេលាពិតក្នុងកម្មវិធីរុករកដោយ Fetch API និង ReadableStream
- បថាភ្នាក់ងារនៅសកម្មភាព និងការផ្សាយអត្ថបទដោយពេលវេលាពិត

**កូដ (UI រួម)៖**

| ឯកសារ | សេចក្ដីពិពណ៌នា |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | រៀបចំបទភាគទម្រង់ទំព័រ |
| `zava-creative-writer-local/ui/style.css` | ស្ទីលកម្មវិធី |
| `zava-creative-writer-local/ui/app.js` | អានស្ទ្រីម និងបច្ចុប្បន្នភាព DOM |

**បន្ថែមនៅផ្នែកខាងក្រោយ៖**

| ភាសា | ឯកសារ | សេចក្ដីពិពណ៌នា |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | អាប់ដេតសម្រាប់បម្រើ UI ប្រភេទស្ថិតស្ថាន |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ម៉ាស៊ីនបម្រើ HTTP ថ្មី កញ្ចប់ជាមួយកែសម្រួល |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | គម្រោង ASP.NET Core Minimal API ថ្មី |

---

### ផ្នែក 13: បញ្ចប់វគ្គបណ្តុះបណ្តាល
**មគ្គុទេសក៍ល្បែង:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- សង្ខេបនៃអ្វីដែលអ្នកបានកសាងជាទូទៅតាមផ្នែកទាំង១២
- គំនិតបន្ថែមសម្រាប់ពង្រីកកម្មវិធីរបស់អ្នក
- តំណរភ្ជាប់ទៅកាន់ធនធាននិងឯកសារ

---

## រចនាសម្ព័ន្ធគម្រោង

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

## ធនធាន

| ធនធាន | តំណ |
|----------|------|
| គេហទំព័រ Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| ប្រតិបតិ្តការម៉ូឌែល | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| GitHub របស់ Foundry Local | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| មគ្គុទេសក៍ការចាប់ផ្តើម | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| ឯកសារ SDK របស់ Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| ស៊ុមភ្នាក់ងាររ Microsoft | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## អាជ្ញាបណ្ណ

សម្ភារៈសិក្សាសម្រាប់បង្រៀននៃសិក្ខាសាលានេះត្រូវបានផ្ដល់ជូន។

---

**សូមសំណាងល្អក្នុងការកសាង! 🚀**