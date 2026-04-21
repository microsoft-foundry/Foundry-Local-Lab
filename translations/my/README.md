<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - စက်ပေါ်တွင် AI အက်ပလီကေးရှင်းများ တည်ဆောက်ခြင်း

သင့်ရဲ့ကိုယ်ပိုင်ကွန်ပျူတာပေါ်မှာ ဘာသာစကားမော်ဒယ်တွေကို အသုံးပြုပြီး [Foundry Local](https://foundrylocal.ai) နဲ့ [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) ကို အသုံးပြု၍ တိုက်ရိုက် မဟာသင်တန်း။

> **Foundry Local ဆိုသည်မှာ?** Foundry Local သည် ရိုးရှင်းပေါ့ပါးသော ရန်တဆိုင်ညီကွန်ပျူတာဖြစ်ပြီး သင့်ဟာ့ဒ်ဝဲပေါ် လုံးဝမော်ဒယ်များကို ဒေါင်းလုဒ်၊ စီမံခန့်ခွဲ နှင့် စီမံပေးနိုင်သည်။ ၎င်းသည် **OpenAI သင့်တော်သော API** တစ်ခုကို ထုတ်ဖော်ပေးထားပြီး OpenAI နဲ့ အညီစကားပြောနိုင်သည့် မည်သည့်ကိရိယာ သို့မဟုတ် SDK မှမဆို ချိတ်ဆက်နိုင်သည် - မလိုအပ်သော မိုးလုံလေလုံအကောင့် မလိုပါ။

### 🌐 ဘာသာစကား မျိုးစုံ ထောက်ခံမှု

#### GitHub Action မှတဆင့် ထောက်ခံသည် (အလိုအလျောက် သက်ဆိုင်ရာနောက်ဆုံးပေါ်အနေဖြင့်)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](./README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **ဒေါင်းလုပ်ကိုယ်တိုင် clone လုပ်ချင်ပါသလား?**
>
> ဒီ repository မှာ ဘာသာစကား ၅၀ ကျော် ဘာသာပြန်များပါဝင်ပြီး ဒေါင်းလုပ်ဖိုင်အရွယ်အစားကို အများအပြား တိုးမြှင့်ပေးသည်။ ဘာသာပြန်မပါဘဲ clone လုပ်ရန် sparse checkout ကို အသုံးပြုပါ:
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
> သင့်အတွက် မြန်ဆန်စွာ တတ်မြောက်မှုလုပ်ဆောင်နိုင်ရန် လိုအပ်သည့် အရာအားလုံးကို ဒီလိုရရှိမည်။
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## သင်ယူမှုပန်းတိုင်များ

ဒီ workshop အပြီးတွင် သင်မှာ အောက်ဖော်ပြပါ အတတ်ပညာများ ရယူထားမည်ဖြစ်သည်-

| # | ပန်းတိုင် |
|---|-----------|
| 1 | Foundry Local ကို install ပြုလုပ်၍ CLI ဖြင့် မော်ဒယ်များ ကို စီမံခန့်ခွဲနိုင်ခြင်း |
| 2 | Foundry Local SDK API ကိုကောင်းကောင်းအသုံးပြု၍ မော်ဒယ် စီမံခန့်ခွဲမှု လုပ်ငန်းစဉ်များကို သိရှိနိုင်ခြင်း |
| 3 | Python ၊ JavaScript ၊ C# SDK များ အသုံးပြု၍ local inference server နှင့်ချိတ်ဆက်နိုင်ခြင်း |
| 4 | သင့်ဒေတာအပေါ် အဖြေများ အခြေခံထား ကဲ့သို့သော Retrieval-Augmented Generation (RAG) pipeline တည်ဆောက်ခြင်း |
| 5 | အမြဲတမ်း လမ်းညွှန်ချက်များ နှင့် ပုဂ္ဂိုလ်များ ပါဝင်သော AI Agents များ ဖန်တီးခြင်း |
| 6 | မိတ်ဆက်တမ်း နှင့် ပြန်လည်ထည့်သွင်း Feedback လုပ်ငန်းစဉ်များပါဝင်သည့် multi-agent workflow များ စီမံခန့်ခွဲခြင်း |
| 7 | ထုတ်လုပ်မှုအဆင့် capstone အက်ပလီကေးရှင်းဖြစ်သည့် Zava Creative Writer ကို လေ့လာမည် |
| 8 | ရွေးချယ်သော dataset များ နှင့် LLM-as-judge အခြေခံ scoring ဖြင့် အကဲဖြတ်မှု framework များ တည်ဆောက်ခြင်း |
| 9 | Whisper အသုံးပြု၍ audio ကို စက်ပေါ်မှာတင် စကားသံမှ စာသား ပြောင်းခြင်း |
| 10 | ONNX Runtime GenAI နှင့် Foundry Local ကို အသုံးပြု၍ စိတ်ကြိုက် သို့မဟုတ် Hugging Face မော်ဒယ်များကို လုပ်ဆောင်ခြင်း |
| 11 | ဒေသန်ဆိုင်ရာ မော်ဒယ်များမှ ဘယ်သူ့အကြံပြုချက်များကိုမဆို ပြင်ပ Function များ ခေါ်နိုင်အောင် ဖြေရှင်းခြင်း |
| 12 | Zava Creative Writer အတွက် browser မှတစ်ဆင့် တိုက်ရိုက် streaming ပါဝင်သည့် UI တည်ဆောက်ခြင်း |

---

## လိုအပ်ချက်များ

| လိုအပ်ချက် | အသေးစိတ် |
|-------------|---------|
| **Hardware** | 8 GB RAM အနည်းဆုံး (16 GB အကြံပြု)၊ AVX2 ထောက်ပံ့ CPU သို့မဟုတ် GPU တစ်ခု |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, သို့မဟုတ် macOS 13 နှင့်အထက် |
| **Foundry Local CLI** | Windows တွင် `winget install Microsoft.FoundryLocal` သို့မဟုတ် macOS တွင် `brew tap microsoft/foundrylocal && brew install foundrylocal` ဖြင့် ထည့်သွင်းနိုင်သည်။ အသေးစိတ် အတွက် [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) ဖတ်ပါ။ |
| **ဘာသာစကား runtime** | **Python 3.9+** နှင့်/သို့မဟုတ် **.NET 9.0+** နှင့်/သို့မဟုတ် **Node.js 18+** |
| **Git** | ဒီ repository ကို clone လုပ်ရန် အသုံးပြုသည် |

---

## စတင်လေ့လာခြင်း

```bash
# ၁။ repository ကို မိတ္တူ ဆွဲပါ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# ၂။ Foundry Local သေချာတက်နေကြောင်း စစ်ဆေးပါ
foundry model list              # အသုံးပြုနိုင်သော မော်ဒယ်များစာရင်း
foundry model run phi-3.5-mini  # အပြန်အလှန် စကားပြောဖွင့်ပါ

# ၃။ သင့်ဘာသာစကား လမ်းကြောင်းကို ရွေးပါ (အပြည့်အစုံအတွက် အပိုင်း ၂ လက်ပ်ကို ကြည့်ပါ)
```

| ဘာသာစကား | ဦးစီးစတင်ရန် |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop အပိုင်းများ

### အပိုင်း 1: Foundry Local ဖြင့် စတင်ခြင်း

**လက်တွေ့လမ်းညွှန်:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local ဆိုတာ ဘာလဲ၊ ဘယ်လို အလုပ်လုပ်လဲ
- Windows နှင့် macOS ပေါ်တွင် CLI ထည့်သွင်းခြင်း
- မော်ဒယ်များကို စူးစမ်းလေ့လာခြင်း - စာရင်းပြုစုခြင်း၊ ဒေါင်းလုပ်လုပ်ခြင်း၊ စတင် အသုံးပြုခြင်း
- မော်ဒယ် aliases များနှင့် dynamic ports များ ပြဿနာဖြေရှင်းနည်း

---

### အပိုင်း 2: Foundry Local SDK အသေးစိတ် လေ့လာခြင်း

**လက်တွေ့လမ်းညွှန်:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- SDK ကို CLI ထက် ဘာကြောင့် အသုံးပြုသင့်သနည်း
- Python, JavaScript, C# အတွက် SDK API ပြည့်စုံ ရည်ညွှန်းချက်
- ဝန်ဆောင်မှု စီမံခန့်ခွဲမှု၊ စာရင်းကြည့်ခြင်း၊ မော်ဒယ် အသက်တာစက်ဝန်း (ဒေါင်းလုပ်၊ ဖွင့်၊ ပိတ်)
- မြန်ဆန်စတင်အသုံးပြုမှု နည်းလမ်းများ: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` မက်တာဒေတာ၊ aliases များနှင့် hardware အတွက် အသင့်တော်ဆုံး မော်ဒယ် ရွေးချယ်မှု

---

### အပိုင်း 3: SDKs နှင့် APIs

**လက်တွေ့လမ်းညွှန်:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, C# မှ Foundry Local သို့ ချိတ်ဆက်သုံးခြင်း
- Foundry Local SDK ကို အသုံးပြု၍ ဝန်ဆောင်မှု မန်နေဂျ်လုပ်ခြင်း
- OpenAI သင့်တော်သော API မှတစ်ဆင့် streaming chat completions သုံးခြင်း
- ဘာသာစကားတစ်ခုချင်းစီအတွက် SDK မက်သေါ် အသုံးပြုနည်း

**ကုဒ်နမူနာများ:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local.py` | အခြေခံ streaming chat |
| C# | `csharp/BasicChat.cs` | .NET ဖြင့် streaming chat |
| JavaScript | `javascript/foundry-local.mjs` | Node.js ဖြင့် streaming chat |

---

### အပိုင်း 4: Retrieval-Augmented Generation (RAG)

**လက်တွေ့လမ်းညွှန်:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG ဆိုတာ ဘာလဲ၊ ဘာကြောင့် အရေးကြီးသနည်း
- မျှဝေမှု လေ့လာမှု အတွက် in-memory knowledge base တည်ဆောက်ခြင်း
- Keyword-overlap retrieval နှင့် ရမှတ် ချမှတ်ခြင်း
- တွန်းအားစနစ် ပရောမ့်များ ဖန်တီးခြင်း
- စက်ပေါ်တွင် သင့်တော်သော RAG pipeline လုပ်ဆောင်ခြင်း

**ကုဒ်နမူနာများ:**

| ဘာသာစကား | ဖိုင် |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### အပိုင်း 5: AI Agents ဖန်တီးခြင်း

**လက်တွေ့လမ်းညွှန်:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI agent ဆိုသည်မှာ ဘာလဲ (raw LLM ခေါ်ဆိုမှုနှင့် နှိုင်းယှဉ်၍)
- `ChatAgent` ပုံစံ နှင့် Microsoft Agent Framework
- စနစ် လမ်းညွှန်ချက်များ၊ ပုဂ္ဂိုလ်ရေးများ၊ turn များစဉ်ဆက်ထပ် စကားပြောခြင်း
- Agent များမှ စနစ်တကျ ထုတ်ပေးသော output (JSON)

**ကုဒ်နမူနာများ:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework ဖြင့် single agent |
| C# | `csharp/SingleAgent.cs` | Single agent (ChatAgent ပုံစံ) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Single agent (ChatAgent ပုံစံ) |

---

### အပိုင်း 6: Multi-Agent Workflows

**လက်တွေ့လမ်းညွှန်:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-agent pipeline များ: Researcher → Writer → Editor
- စဉ်ဆက်မပြတ် ညှိနှိုင်းမှု နှင့် feedback လုပ်ငန်းစဉ်များ
- မျှဝေ Configuration နှင့် စနစ်တက်သည့် hand-offs များ
- ဤအတိုင်း သင့်ကိုယ်ပိုင် multi-agent workflow ရေးဆွဲခြင်း

**ကုဒ်နမူနာများ:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | သုံးဦး agent pipeline |
| C# | `csharp/MultiAgent.cs` | သုံးဦး agent pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | သုံးဦး agent pipeline |

---

### အပိုင်း 7: Zava Creative Writer – Capstone Application

**လက်တွေ့လမ်းညွှန်:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- ထုတ်လုပ်မှုအဆင့် အမျိုးမျိုး သုံး agent တွေ ပါဝင်သည့် အက်ပလီကေးရှင်းတစ်ခု
- စဉ်ဆက်မပြတ် pipeline နှင့် evaluator နဲ့ feedback လုပ်ငန်းစဉ်များ
- streaming output, ကုန်ပစ္စည်း စာရင်း ရှာဖွေရေး, စနစ်တကျ JSON hand-offs
- Python (FastAPI), JavaScript (Node.js CLI), C# (.NET console) တွင် အပြည့်အစုံ ထားရှိမှု

**ကုဒ်နမူနာများ:**

| ဘာသာစကား | ဖိုဒါ | ဖော်ပြချက် |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI ဝက်ဘ် ဝန်ဆောင်မှု နှင့် orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI အက်ပလီကေးရှင်း |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console အက်ပလီကေးရှင်း |

---

### အပိုင်း 8: အကဲဖြတ်မှု ဦးတည် အောင်မြင်မှု ဖွံ့ဖြိုးတိုးတက်မှု

**လက်တွေ့လမ်းညွှန်:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- AI agent များအတွက် systematical evaluation framework များ ဖန်တီးခြင်း၊ golden datasets အသုံးပြုခြင်း
- စည်းမျဉ်းအရ စစ်ဆေးမှုများ (အရှည်၊ keyword coverage, တားမြစ်ချက်များ) + LLM-as-judge scoring
- prompt အသစ်များကို side-by-side နှိုင်းယှဉ်ပြီး စုစုပေါင်း စကောများ ရရှိစေခြင်း
- အပိုင်း 7 တွင် ဖော်ပြခဲ့သည့် Zava Editor agent ပုံစံကို offline test suite ထဲသို့ တိုးချဲ့ခြင်း
- Python, JavaScript နှင့် C# လူ့သင်တန်းများ

**ကုဒ်နမူနာများ:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | အကဲဖြတ်မှု framework |
| C# | `csharp/AgentEvaluation.cs` | အကဲဖြတ်မှု framework |
| JavaScript | `javascript/foundry-local-eval.mjs` | အကဲဖြတ်မှု framework |

---

### အပိုင်း 9: Whisper ဖြင့် အသံမှတ်တန်းထုတ်ခြင်း

**လက်တွေ့လမ်းညွှန်:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- OpenAI Whisper ကို ဒေသန္တရမှာ ပြေးနေတဲ့ Speech-to-text transcription
- ကိုယ်ပိုင် device ထဲမှ အယုတ်မဖြစ်စေဘဲ ပုဂ္ဂလိကအဆင့်ရှိသော အသံကို ပြုပြင်ခြင်း
- Python, JavaScript, နှင့် C# မှာ `client.audio.transcriptions.create()` (Python/JS) နှင့် `AudioClient.TranscribeAudioAsync()` (C#) ကို အသုံးပြုခြင်း
- လက်တွေ့သင်ကြားရန် Zava theme များပါဝင်သည့် နမူနာ အသံဖိုင်များပါဝင်သည်

**ကုဒ်နမူနာများ:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper အသံ transcription |
| C# | `csharp/WhisperTranscription.cs` | Whisper အသံ transcription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper အသံ transcription |

> **မှတ်ချက်:** ဤလက်တွေ့အလုပ်ရုံသည် **Foundry Local SDK** ကို အသုံးပြု၍ Whisper မော်ဒယ်ကို အလိုအလျောက် ဒေါင်းလုတ်ဆွဲပြီး လုပ်ဆောင်သည်၊ ထို့နောက် ဒေသန္တရ OpenAI-ကိုက်ညီသော endpoint သို့ အသံကို ပို့ပြီး transcription ပြုလုပ်သည်။ Whisper မော်ဒယ် (`whisper`) သည် Foundry Local catalog တွင် ပါဝင်ပြီး စက်ပစ္စည်းပေါ်တွင် ပုံသေကောင်းစွာ ပြေးဆွဲနိုင်သည် - cloud API key မလိုအပ်ဘဲ နှင့် network ဝင်ရန် မလိုအပ်ပါ။

---

### အပိုင်း ၁၀: စိတ်ကြိုက် သို့မဟုတ် Hugging Face မော်ဒယ်များ အသုံးပြုခြင်း

**လက်တွေ့လမ်းညွှန်:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face မော်ဒယ်များကို ONNX Runtime GenAI မော်ဒယ် တည်ဆောက်သူ ဖြင့် စနစ်တကျ ONNX ပုံစံသို့ လုပ်ငန်းဆောင်ရွက်ခြင်း
- စက်ပစ္စည်းအလိုက် ကွန်ပိုင်လ်ခြင်း (CPU, NVIDIA GPU, DirectML, WebGPU) နှင့် သတ်မှတ်ခြင်း (int4, fp16, bf16)
- Foundry Local အတွက် chat-template ဖိုင်များ ဖန်တီးခြင်း
- ကွန်ပိုင်လ်ပြီး မော်ဒယ်များကို Foundry Local cache ထဲသို့ ထည့်သွင်းခြင်း
- CLI, REST API နှင့် OpenAI SDK မှတစ်ဆင့် စိတ်ကြိုက် မော်ဒယ်များ တိုးတက်အသုံးပြုခြင်း
- ကိုးကား နမူနာ - Qwen/Qwen3-0.6B ကွန်ပိုင်လ်ပြီး အဆုံးထိပြုလုပ်ခြင်း

---

### အပိုင်း ၁၁: ဒေသန္တရ မော်ဒယ်များဖြင့် Tool Calling

**လက်တွေ့လမ်းညွှန်:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- ဒေသန္တရ မော်ဒယ်များကို အပြင်မှ ဖန်ဆင်းထားသော function များအား ခေါ်ယူခွင့် (tool/function calling) ခွင့်ပြုခြင်း
- OpenAI function-calling ပုံစံဖြင့် tool schema များ သတ်မှတ်ရေးဆွဲခြင်း
- အပတ်ပေါင်း tool-calling စကားပြောစဉ် လည်ပတ်ခြင်းကို ကိုင်တွယ်မှု
- ဒေသန္တရတွင် tool call များ ဆောင်ရွက်ပြီး မော်ဒယ်သို့ ရလဒ်များ ပြန်ပေးပို့ခြင်း
- Tool-calling အခြေအနေများအတွက် သင့်တော်သော မော်ဒယ် ရွေးချယ်ခြင်း (Qwen 2.5, Phi-4-mini)
- SDK ရဲ့ ပုံမှန် `ChatClient` ကို tool calling အတွက် အသုံးပြုခြင်း (JavaScript)

**ကုဒ်နမူနာများ:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | ရာသီဥတု/လူဦးရေ ကိရိယာများ ဖြင့် tool calling |
| C# | `csharp/ToolCalling.cs` | .NET ဖြင့် tool calling |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ဖြင့် tool calling |

---

### အပိုင်း ၁၂: Zava Creative Writer အတွက် Web UI တည်ဆောက်ခြင်း

**လက်တွေ့လမ်းညွှန်:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer အတွက် browser-based front end ကို ထည့်သွင်းခြင်း
- Python (FastAPI), JavaScript (Node.js HTTP), နှင့် C# (ASP.NET Core) မှာ ပြန်လည် ဝန်ဆောင်မှုပေးခြင်း
- Browser တွင် Fetch API နှင့် ReadableStream ဖြင့် streaming NDJSON ကို စားသုံးခြင်း
- တိုက်ရိုက် အေးဂျင့် အခြေအနေ စာရင်းနှင့် စာရေးဆောင်းပါး live streaming ဖော်ပြမှု

**ကုဒ် (shared UI):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | စာမျက်နှာ တည်ဆောက်ပုံ |
| `zava-creative-writer-local/ui/style.css` | ပုံစံ သတ်မှတ်ခြင်း |
| `zava-creative-writer-local/ui/app.js` | Stream reader နှင့် DOM update နည်းပညာ |

**နောက်ခံ အပိုင်း ပေါင်းထည့်မှုများ:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Static UI သို့ ဝန်ဆောင်မှုပေးရေး အပ်ဒိတ္ဖြည့်သွင်းခြင်း |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Orchestrator ကို တည်ဆောက်သည့် HTTP Server အသစ် |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | ASP.NET Core minimal API project အသစ် |

---

### အပိုင်း ၁၃: အလုပ်ရုံပြီးစီးခြင်း

**လက်တွေ့လမ်းညွှန်:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- ၁၂ အပိုင်းအတွင်း တည်ဆောက်ခဲ့သည်များ စုစည်းတင်ပြချက်
- သင့်အက်ပလီကေးရှင်းများ တိုးချဲ့ဖွံ့ဖြိုးရန် အတွေးအခေါ်များ
- အချက်အလက်အသုံးအနှုန်းနှင့် စာရွက်စာတမ်း များလင့်ခ်များ

---

## Project ဖွဲ့စည်းပုံ

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

## အရင်းအမြစ်များ

| Resource | Link |
|----------|------|
| Foundry Local ဝဘ်ဆိုက် | [foundrylocal.ai](https://foundrylocal.ai) |
| မော်ဒယ် catalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| စတင်လေ့လာမှု လမ်းညွှန် | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK ကိုးကားစာမျက်နှာ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## အသုံးပြုခွင့်

ဤ workshop ပစ္စည်းများသည် ပညာရေး အသုံးပြုရန် ဗဟုသုတ ရည်ရွယ်၍ ဖြန့်ဝေထားပါသည်။

---

**ပျော်ရွှင်စရာ တည်ဆောက်မှု! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**မှတ်ချက်**  
ဤစာတမ်းကို AI ဘာသာပြန်ဝန်ဆောင်မှု [Co-op Translator](https://github.com/Azure/co-op-translator) အသုံးပြု၍ ဘာသာပြန်ထားပါသည်။ ကျွန်ုပ်တို့သည် တိကျမှန်ကန်မှုအတွက် အားဖြည့်ကြ努力သော်လည်း၊ အလိုအလျောက် ဘာသာပြန်ခြင်းတွင် အမှားများ သို့မဟုတ် မမှန်ကန်သော အချက်အလက်များ ပါရှိနိုင်ကြောင်း သိရှိပါရန် သတိပေးအပ်ပါသည်။ မူရင်းစာတမ်းကို၎င်း၏ မူလဘာသာဖြင့်သာ အတည်ပြုရမည့် အရင်းအမြစ် အဖြစ်ယူဆရန် လိုအပ်ပါသည်။ အရေးကြီးသည့် အချက်အလက်များအတွက် လူ့ဘာသာပြန်ကို အသုံးပြုရန် အကြံပြုပါသည်။ ဤဘာသာပြန်မှုကို အသုံးပြုရာမှ ဖြစ်ပေါ်လာနိုင်သည့် နားမလည်မှုများ သို့မဟုတ် မမှန်ကန်သည့် အယူအဆများအတွက် ကျွန်ုပ်တို့ တာဝန်မယူပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->