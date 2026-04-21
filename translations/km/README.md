<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# ពន្យល់សិក្សា Foundry Local - កសាងកម្មវិធី AI លើឧបករណ៍ដោយផ្ទាល់

ពន្យល់សិក្សាដែលមានដៃចូលរួមសម្រាប់បើកប្រើម៉ូឌែលភាសានៅលើម៉ាស៊ីនរបស់អ្នកផ្ទាល់ និងកសាងកម្មវិធីឆ្លាតវៃជាមួយ [Foundry Local](https://foundrylocal.ai) និង [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)។

> **Foundry Local ជាមួយអ្វី?** Foundry Local គឺជាកម្មវិធីរត់ម៉ូដែលភាសាឆាប់រហ័សដែលអនុញ្ញាតឱ្យអ្នកទាញយក គ្រប់គ្រង និងបម្រើម៉ូឌែលភាសាទាំងឡាយនៅលើឧបករណ៍របស់អ្នក។ វាបង្ហាញអេភីអាយ **ដែលប្រកបដោយភាពសមស្របនឹង OpenAI** ដូច្នេះឧបករណ៍ ឬ SDK ស៊ើបអង្កេតណាមួយដែលអាចនិយាយ OpenAI អាចភ្ជាប់បាន - មិនចាំបាច់មានគណនីបង្ហោះពពក។

### 🌐 គាំទ្រច្រើនភាសា

#### គាំទ្រដោយ GitHub Action (ស្វ័យប្រវត្តិ និងទាន់ពេលវេលានៅសព្វថ្ងៃ)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](./README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **ចូលចិត្តថតចម្លងមកនៅលើកុំព្យូទ័រផ្ទាល់?**
>
> ឃ្លាំងនេះរួមបញ្ចូលការប្រែសម្រួលជាង 50+ ភាសា ដែលបង្កើនទំហំទាញយកយ៉ាងច្រើន។ ដើម្បីចម្លងដោយគ្មានការប្រែសម្រួល ប្រើការចេញជ្រើសជាងមួយ:
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
> វាប្រើបានមុខងារទាំងអស់ដែលអ្នកត្រូវការដើម្បីបញ្ចប់មុខវិជ្ជានេះជាមួយការទាញយកយឺតជាង។
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## វីស័យរៀនសូត្រ

នៅចុងពន្យល់សិក្សានេះ អ្នកនឹងអាច:

| # | បំណង |
|---|-----------|
| 1 | ដំឡើង Foundry Local និងគ្រប់គ្រងម៉ូឌែលជាមួយ CLI |
| 2 | បញ្ញាសិក្សา API SDK របស់ Foundry Local សម្រាប់ការគ្រប់គ្រងម៉ូឌែលដោយកម្មវិធី |
| 3 | ភ្ជាប់ទៅមកម៉ាស៊ីនបម្រើ="./local inference server" ដោយប្រើ SDK ភាសា Python, JavaScript, និង C# |
| 4 | កសាងបណ្តាញបង្កើតតាមការទាញយក (RAG) ដែលផ្អែកលើទិន្នន័យរបស់អ្នកផ្ទាល់ |
| 5 | បង្កើតភេរវកម្ម AI ជាមួយការណែនាំ និងតួអង្គមានអចិន្រ្តៃយ៍ |
| 6 | រៀបចំការប្រតិបត្តិការជាមួយភេរវកម្មជាច្រើនដោយមានវដ្តប្រតិភូ |
| 7 | ស្វែងយល់កម្មវិធីប្រតិបត្តិការ Zava Creative Writer |
| 8 | កសាងស៊ុមវាយតម្លៃដោយប្រើឯកសារមាស និង LLM ជាអ្នកវាយតម្លៃ |
| 9 | បម្លែងសំលេងទៅអត្ថបទជាមួយ Whisper - និយាយទៅអត្ថបទនៅលើឧបករណ៍ដោយប្រើ SDK របស់ Foundry Local |
| 10 | ប្រមូល និងបើកដំណើរការម៉ូឌែលឯកសារផ្ទាល់ ឬ Hugging Face ជាមួយ ONNX Runtime GenAI និង Foundry Local |
| 11 | អនុញ្ញាតម៉ូឌែលក្នុងស្រុកហៅមុខងារផ្សេងទៀតជាមួយលំនាំហៅឧបករណ៍ |
| 12 | កសាង UI ដែលមើលបានតាមរយៈកម្មវិធីរុករកសម្រាប់ Zava Creative Writer ដែលមានការបញ្ចូនទិន្នន័យពេលជាក់ស្តែង |

---

## តម្រូវការ ជាមុន

| តម្រូវការ | ព័ត៌មានលម្អិត |
|-------------|---------|
| **ថេរឹយ | 8 GB RAM នៅតិចបំផុត (ផ្តល់អនុសាសន៍ 16 GB); CPU ដែលគាំទ្រ AVX2 ឬ GPU ដែលគាំទ្រ |
| **ប្រព័ន្ធប្រតិបត្តិការ | Windows 10/11 (x64/ARM), Windows Server 2025, ឬ macOS 13+ |
| **Foundry Local CLI** | ដំឡើងដោយ `winget install Microsoft.FoundryLocal` (Windows) ឬ `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS)។ មើល [មគ្គុទ្ទេសក៍ចាប់ផ្តើម](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) សម្រាប់ព័ត៌មានលម្អិត។ |
| **ម៉ាស៊ីនភាសារត | **Python 3.9+** និង/ឬ **.NET 9.0+** និង/ឬ **Node.js 18+** |
| **Git** | សម្រាប់ចម្លងជាផ្លូវការ |

---

## ចាប់ផ្តើម

```bash
# 1. បង្កើតចម្លងឃ្លាំងកូដ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ពិនិត្យមើលថា Foundry Local ត្រូវបានដំឡើងហើយ
foundry model list              # បង្ហាញបញ្ជីម៉ូដែលដែលមាន
foundry model run phi-3.5-mini  # ចាប់ផ្តើមការជជែកអន្តរការណ៍

# 3. ជ្រើសរើសផ្លូវភាសារបស់អ្នក (មើលហLaboratory ផ្នែក 2 សម្រាប់ការតំឡើងពេញលេញ)
```

| ភាសា | ចាប់ផ្តើមយ៉ាងរហ័ស |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ផ្នែកនៃពន្យល់សិក្សា

### ផ្នែក 1: ចាប់ផ្តើមជាមួយ Foundry Local

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local ជាអ្វី និងរបៀបដំណើរការ
- ដំឡើង CLI លើ Windows និង macOS
- ស្វែងរកម៉ូឌែល - បញ្ជី ទាញយក បើកបរ
- យល់ពីអាលីសម៉ូឌែល និងកំពង់ផែដឺណាមិច

---

### ផ្នែក 2: ការសិក្សាដំណ្រៃជ្រៅ SDK របស់ Foundry Local

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- ហេតុអ្វីបានជាការប្រើ SDK ផ្ទុយពី CLI សម្រាប់ការអភិវឌ្ឍកម្មវិធី
- មូលដ្ឋាន API SDK សម្រាប់ Python, JavaScript, និង C#
- គ្រប់គ្រងសេវា, ការជៀសវាងកាតាឡុក, អាយុកាលម៉ូឌែល (ទាញយក, បើក, បិទ)
- លំនាំចាប់ផ្តើមរហ័ស: បង្កើត Python, ការចាប់ផ្តើម JavaScript `init()`, C# `CreateAsync()`
- កំណត់ត្រា `FoundryModelInfo`, អាលីស និងការជ្រើសម៉ូឌែលសមជាមួយថេរឹយ

---

### ផ្នែក 3: SDK និង API

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- ភ្ជាប់ទៅ Foundry Local ពី Python, JavaScript និង C#
- ប្រើ SDK របស់ Foundry Local ដើម្បីគ្រប់គ្រងសេវា​តាមកម្មវិធី
- ចែកចាយការឆាតជាស្ទ្រីមតាមអេភីអាយដែលសមស្រប OpenAI
- ការយោងវិធីសាស្រ្ត SDK សម្រាប់មួយភាសា

**គំរូកូដ:**

| ភាសា | ឯកសារ | ពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local.py` | ការចាត់ចែងការឆាតតាមស្ទ្រីមមូលដ្ឋាន |
| C# | `csharp/BasicChat.cs` | ការចាត់ចែងការឆាតតាមស្ទ្រីមជាមួយ .NET |
| JavaScript | `javascript/foundry-local.mjs` | ការឆាតតាមស្ទ្រីមជាមួយ Node.js |

---

### ផ្នែក 4: ការបង្កើតតាមការទាញយក (RAG)

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG ជាអ្វី និងហេតុអ្វីវាសំខាន់
- កសាងមូលដ្ឋានចំណេះដឹងក្នុងអង្គចងចាំ
- ការទាញយកដោយស្លាកពាក្យមានការប្រមូលផ្តុំពិន្ទុ
- បង្កើតសម្រង់ប្រព័ន្ធដោយផ្អែកលើគោលការណ៍
- ដំណើរការបណ្ដាញ RAG លើឧបករណ៍ដោយសព្វថ្ងៃ

**គំរូកូដ:**

| ភាសា | ឯកសារ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ផ្នែក 5: កសាងភេរវកម្ម AI

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- ភេរវកម្ម AI ជាអ្វី (ផ្ទៀងផ្ទាត់បំភ្លឺទៅការហៅ LLM ត្រង់)
- លំនាំ `ChatAgent` និង Microsoft Agent Framework
- សេចក្ដីណែនាំប្រព័ន្ធ តួអង្គ និងការសន្ទនាច្រើនជំនាន់
- ផលចេញរៀបចំ (JSON) ពីភេរវកម្ម

**គំរូកូដ:**

| ភាសា | ឯកសារ | ពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | ភេរវកម្មតែម្នាក់ជាមួយ Agent Framework |
| C# | `csharp/SingleAgent.cs` | ភេរវកម្មតែម្នាក់ (លំនាំ ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ភេរវកម្មតែម្នាក់ (លំនាំ ChatAgent) |

---

### ផ្នែក 6: ប្រតិបត្តិការជាមួយភេរវកម្មច្រើន

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- បណ្ដាញការងារជាមួយភេរវកម្មច្រើន៖ អ្នកស្រាវជ្រាវ → អ្នកសរសេរ → អ្នកកែសម្រួល
- ការចាក់បញ្ជាលំដាប់និងវដ្តប្រតិភូ
- កំណត់រចនាសម្ព័ន្ធ និងការផ្ទេរចេញមុខរបររៀបចំ
- រចនាបណ្ដាញរបស់អ្នកដោយផ្ទាល់

**គំរូកូដ:**

| ភាសា | ឯកសារ | ពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | បណ្ដាញភេរវកម្មបី |
| C# | `csharp/MultiAgent.cs` | បណ្ដាញភេរវកម្មបី |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | បណ្ដាញភេរវកម្មបី |

---

### ផ្នែក 7: កម្មវិធី Zava Creative Writer - ប្រតិបត្តិករ Capstone

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- កម្មវិធីជាប្រភេទផលិតកម្មជាមួយភេរវកម្មជាច្រើនឯកទេស 4 នាក់
- បណ្ដាញដំណើរការចុងក្រោយសហរក្សវដ្តប្រតិភូដោយអ្នកវាយតម្លៃ
- ផលចេញចរន្ត ទីផ្សារផលិតផល ស្លាក JSON រៀបចំ
- អនុវត្តពេញលេញជាភាសា Python (FastAPI), JavaScript (Node.js CLI), និង C# (.NET console)

**គំរូកូដ:**

| ភាសា | កថាខណ្ឌ | ពិពណ៌នា |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | សេវាគេហទំព័រ FastAPI ជាមួយមនុស្សរៀបចំ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | កម្មវិធី CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | កម្មវិធី .NET 9 console |

---

### ផ្នែក 8: ការអភិវឌ្ឍភាពដោយការវាយតម្លៃ

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- កសាងស៊ុមវាយតម្លៃដោយរបៀបសមស្របសម្រាប់ភេរវកម្ម AI ដោយប្រើឯកសារមាស
- ពិនិត្យតាមច្បាប់ (ប្រវែង, ការប្រមូលផ្តុំពាក្យ, ពាក្យហាមឃាត់) + សំរុង LLM ជាអ្នកវាយតម្លៃ
- ប្រៀបធៀបជាមួយគ្នារវាងពិធីប្រតិបត្តិស្របជាមួយកាតថ្លៃសរុប
- ក្នុងនោះពង្រីកលំនាំភេរវកម្ម Zava Editor ពីផ្នែក 7 ទៅកាន់ស៊ុមតេស្តផ្ទាល់
- ផ្លូវ Python, JavaScript, និង C#

**គំរូកូដ:**

| ភាសា | ឯកសារ | ពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | ស៊ុមវាយតម្លៃ |
| C# | `csharp/AgentEvaluation.cs` | ស៊ុមវាយតម្លៃ |
| JavaScript | `javascript/foundry-local-eval.mjs` | ស៊ុមវាយតម្លៃ |

---

### ផ្នែក 9: បម្លែងសំលេងជាអត្ថបទជាមួយ Whisper

**មគ្គុទ្ទេសក៍មន្ទីរ:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- ការ​បម្លែង​សន្ទនា​ទៅ​អត្ថបទ​ដោយប្រើ OpenAI Whisper ដែល​រត់​នៅលើឧបករណ៍​ផ្ទាល់
- ការប.processសំឡេងផ្អែកលើភាពឯកជនជាមុន - អូឌីយ៉ូ​មិនធ្លាក់ទៅក្រៅឧបករណ៍​របស់អ្នក
- តាមដាន Python, JavaScript និង C# ជាមួយ `client.audio.transcriptions.create()` (Python/JS) និង `AudioClient.TranscribeAudioAsync()` (C#)
- រួមបញ្ចូលឯកសារ​អូឌីយ៉ូ​ឧទាហរណ៍​ធាតុ Zava សម្រាប់​ផ្តល់​ឱកាសហាត់ការ​ត្រូវការ​ដៃ

**គំរូកូដ៖**

| ភាសា | ឯកសារ | សេចក្តីពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | ការបម្លែងសំឡេង Whisper |
| C# | `csharp/WhisperTranscription.cs` | ការបម្លែងសំឡេង Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | ការបម្លែងសំឡេង Whisper |

> **កំណត់សម្គាល់ ៖** បន្ទាត់នេះប្រើប្រាស់ **Foundry Local SDK** ដើម្បីទាញយក និងបញ្ចូលម៉ូឌែល Whisper ដោយកម្មវិធី អោយយើងផ្ញើសំឡេងទៅកាន់ចំណុច OpenAI ដែលនៅក្នុងឧបករណ៍សម្រាប់បម្លែងសំឡេង។ ម៉ូឌែល Whisper (`whisper`) ត្រូវបានរាយបញ្ជីនៅក្នុងកាថ្លុក Foundry Local ហើយរត់ពេញលេញក្នុងឧបករណ៍ — មិនត្រូវការពាក្យសម្ងាត់ API ពពក ឬការចូលប្រើបណ្ដាញទេ។

---

### ផ្នែក ១០៖ ការប្រើម៉ូឌែលផ្ទាល់ខ្លួន ឬ Hugging Face

**មគ្គុទេសក៍បទបង្រៀនៈ** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ការបង្ហាញម៉ូឌែល Hugging Face ទៅទ្រង់ទ្រាយ ONNX ដែលបានធ្វើឱ្យប្រសើរឡើងដោយកម្មវិធី ONNX Runtime GenAI
- ការបង្កើតស្រេចសម្រាប់ឧបករណ៍ជាក់លាក់ (CPU, NVIDIA GPU, DirectML, WebGPU) និងការបញ្ចុះបញ្ចូលតម្លៃ (int4, fp16, bf16)
- បង្កើតឯកសារកំណត់រចនាសម្ព័ន្ធ chat-template សម្រាប់ Foundry Local
- បន្ថែមម៉ូឌែលដែលបានបង្កប់ទៅកាន់កុសល់ Foundry Local
- រត់ម៉ូឌែលផ្ទាល់ខ្លួនតាម CLI, REST API និង OpenAI SDK
- ឧទាហរណ៍យោង៖ ការបង្ហាញ Qwen/Qwen3-0.6B ពេញលេញពីចាប់ផ្ដើមដល់បញ្ចប់

---

### ផ្នែក ១១៖ ការហៅឧបករណ៍ជាមួយម៉ូឌែលផ្ទាល់ខ្លួន

**មគ្គុទេសក៍បទបង្រៀនៈ** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- អនុញ្ញាតឲ្យម៉ូឌែលផ្ទាល់ខ្លួនហៅមុខងារក្រៅ (ការហៅឧបករណ៍/មុខងារ)
- កំណត់ស្កីម៉ារបស់ឧបករណ៍ដោយប្រើរចនាសម្ព័ន្ធហៅមុខងារ OpenAI
- គ្រប់គ្រងលំហូរកិច្ចសន្ទនាហៅឧបករណ៍ជាច្រើនជួរ
- ប្រតិបត្តិការហៅឧបករណ៍នៅជាប់ក្នុងឧបករណ៍ ហើយត្រឡប់លទ្ធផលទៅម៉ូឌែល
- ជ្រើសរើសម៉ូឌែលសមរម្យសម្រាប់សេណារីយូហៅឧបករណ៍ (Qwen 2.5, Phi-4-mini)
- ប្រើ `ChatClient` នៅក្នុង SDK សម្រាប់ការហៅឧបករណ៍ (JavaScript)

**គំរូកូដ៖**

| ភាសា | ឯកសារ | សេចក្តីពិពណ៌នា |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | ហៅឧបករណ៍ផ្នែកអាកាសធាតុ/ប្រជាជន |
| C# | `csharp/ToolCalling.cs` | ហៅឧបករណ៍ជាមួយ .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ហៅឧបករណ៍ជាមួយ ChatClient |

---

### ផ្នែក ១២៖ សាងសង់ UI វេបសម្រាប់អ្នកសរសេរក្នុងការច្នៃប្រឌិត Zava

**មគ្គុទេសក៍បទបង្រៀនៈ** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- បន្ថែមផ្នែកមុខប្រើប្រាស់នៅលើកម្មវិធីរុករក​សម្រាប់អ្នកសរសេរក្នុងការច្នៃប្រឌិត Zava
- សេវាកម្ម UI រួមបញ្ចូលពី Python (FastAPI), JavaScript (Node.js HTTP), និង C# (ASP.NET Core)
- ប្រើប្រាស់បន្ទាត់ដាក់ប្រកាស NDJSON តាមការប្រមូលនៅក្នុងកម្មវិធីរុករកជាមួយ Fetch API និង ReadableStream
- បាល់បង្ហាញស្ថានភាពភ្នាក់ងាររស់ និងការបញ្ជូនអត្ថបទអត្តបទជាពេលវេលាជាក់លាក់

**កូដ (UI រួម):**

| ឯកសារ | សេចក្តីពិពណ៌នា |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | រចនាបទទំព័រ |
| `zava-creative-writer-local/ui/style.css` | ការតុបតែងរចនាសម្ព័ន្ធ |
| `zava-creative-writer-local/ui/app.js` | អ្នកអានបន្ទាត់ និងលទ្ធផលកំណត់ DOM |

**ការបន្ថែម​ផ្នែក​បញ្ចប់​សម្រាប់​ឆាកសង្គ្រោះមុខ:**

| ភាសា | ឯកសារ | សេចក្តីពិពណ៌នា |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | បានអាប់ដេតសម្រាប់សេរី UI រដ្ឋប្បវត្តិ |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ម៉ាស៊ីនបម្រើ HTTP ថ្មីសម្រាប់បំពាក់កុងត្រូលឡើរ |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | គម្រោង API តិចតួច ASP.NET Core ថ្មី |

---

### ផ្នែក ១៣៖ សិក្ខាសាលាបញ្ចប់

**មគ្គុទេសក៍បទបង្រៀនៈ** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- សង្ខេបអំពីអ្វីៗដែលអ្នកបានកសាងក្នុង ១២ ផ្នែកទាំងអស់
- គំនិតបន្ថែមសម្រាប់ពង្រីកកម្មវិធីរបស់អ្នក
- តំណទៅធនធាន និងឯកសារយោង

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

| ធនធាន | តំណភ្ជាប់ |
|----------|------|
| គេហទំព័រ Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| កាតាឡុកម៉ូឌែល | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| មគ្គុទេសក៍ចាប់ផ្ដើម | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| ឯកសារយោង SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## អាជ្ញាបណ្ណ

សម្ភារៈសិក្ខាសាលានេះផ្តល់ជូនសម្រាប់គោលបំណងការសិក្សា។

---

**សូមសំណាងល្អក្នុងការសាងសង់! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ចំណងជើងនា**៖  
ឯកសារនេះត្រូវបានបកប្រែដោយប្រើសេវាកម្មបកប្រែ AI [Co-op Translator](https://github.com/Azure/co-op-translator)។ ខណៈពេលដែលយើងខិតខំរកលទ្ធភាពត្រឹមត្រូវ សូមយល់ព្រមថាការបកប្រែដោយម៉ាស៊ីនអាចមានកំហុស ឬ ការមិនត្រឹមត្រូវបានបង្កើតឡើង។ ឯកសារដើមជាភាសាដែលមានដើមគួរត្រូវបានយកវាជារៀងរាល់ប្រភពផ្លូវការជាចម្បង។ សម្រាប់ព័ត៌មានមានសារៈសំខាន់ណាស់ យើងសូមផ្តល់អនុសាសន៍ឱ្យប្រើការបកប្រែដោយមនុស្សជំនាញវិជ្ជាជីវៈ។ យើងមិនទទួលខុសត្រូវចំពោះការយល់ច្រឡំ ឬ ការបកប្រែខុសពីរបបណាមួយដែលបណ្តាលមកពីការប្រើប្រាស់ការបកប្រែនេះឡើយ។
<!-- CO-OP TRANSLATOR DISCLAIMER END -->