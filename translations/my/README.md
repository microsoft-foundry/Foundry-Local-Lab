<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local စက်ပေါ် AI အက်ပ်များ ဆောက်လုပ်ခြင်း အလုပ်ရုံဆွေးနွေးပွဲ

သင်၏ကိုယ်ပိုင်စက်ပေါ်တွင် ဘာသာစကား မော်ဒယ်များ လည်ပတ်စေပြီး [Foundry Local](https://foundrylocal.ai) နှင့် [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) အသုံးပြုကာ အသိပညာပေး အက်ပ်များ ဆောက်လုပ်ခြင်းအတွက် လက်တွေ့လေ့လာသင်ကြားမှု အလုပ်ရုံဆွေးနွေးပွဲ။

> **Foundry Local ဆိုတာ ဘာလဲ?** Foundry Local သည် သင့်ဟာ့ဒ်ဝဲပေါ်တွင် ဘာသာစကား မော်ဒယ်များကို အပြည့်အဝ ဒေါင်းလုပ်လုပ်နိုင်သည့်၊ စီမံခန့်ခွဲနိုင်သည့်၊ အသုံးပြုနိုင်သည့် အလင်းလွတ် ရက်တိုက် အချိန်ပြေး ဆော့ဖ်ဝဲဖြစ်သည်။ OpenAI-ကို ကိုက်ညီသော API ကို ဖော်ပြသည်၊ ထို့ကြောင့် OpenAI နှင့် ဆက်သွယ်နိုင်သည့်တစ်မျိုးသမျှ ကိရိယာ သို့မဟုတ် SDK များ အချိန်မရွေး ချိတ်ဆက်နိုင်ပါတယ် - ကလောင်းစာရင်း မလိုအပ်ပါ။

---

## သင်ယူရမည့်ရည်မှန်းချက်များ

ဤအလုပ်ရုံဆွေးနွေးပွဲအဆုံးသတ်သည်အထိ သင်မှာ အောက်ပါအချက်များကို ပြုလုပ်နိုင်ပါလိမ့်မည် - 

| # | ရည်မှန်းချက် |
|---|-----------|
| 1 | Foundry Local ကို 설치၍ CLI ဖြင့် မော်ဒယ်များကို စီမံခန့်ခွဲနိုင်ခြင်း |
| 2 | Foundry Local SDK API ကို မော်ဒယ် စီမံခန့်ခွဲမှုအတွက် အပရိုဂရမ်ဖြင့်_master လုပ်နိုင်ခြင်း |
| 3 | Python, JavaScript နှင့် C# SDK များကို အသုံးပြု၍ ဒေသတြင်းအလိုအလျောက် သတင်း များခံ ဆာဗာနှင့် ချိတ်ဆက်နိုင်ခြင်း |
| 4 | သင့်ကိုယ်ပိုင်ဒေတာအပေါ် အခြေခံသော Retrieval-Augmented Generation (RAG) လမ်းကြောင်းတစ်ခု တည်ဆောက်နိုင်ခြင်း |
| 5 | ပုံမှန်ညွှန်ကြားချက်များနှင့် စွယ်စုံပုဂ္ဂိုလ်များ ဖြင့် AI အေးဂျင့်များ ဖန်တီးနိုင်ခြင်း |
| 6 | အမြင့်ကြားအေးဂျင့် လုပ်ငန်းစဉ်များကို တပြိုင်နက် ပြန်လည်တုံ့ပြန်မှု လုပ်ငန်းစဉ်များ နှင့် စီမံခန့်ခွဲနိုင်ခြင်း |
| 7 | ထုတ်လုပ်မှုအလုပ်အပ်ခြင်း အက်ပ်တစ်ခု - Zava Creative Writer ကို လေ့လာနိုင်ခြင်း |
| 8 | ရွှေစစ်စစ်ဒေတာ စုစည်းမှုနှင့် LLM-as-judge အမှတ်ပေးမှုဖြင့် သုံးသပ်ရေးလုပ်ငန်းများ တည်ဆောက်နိုင်ခြင်း |
| 9 | Whisper အသုံးပြု၍ အသံမှ စာသား သို့ လိုက်လျောညီထွေစာ များဖတ်ထုတ်နိုင်ခြင်း - သက်ဆိုင်ရာစက်ပေါ်တွင် |
| 10 | ONNX Runtime GenAI နှင့် Foundry Local ကိုအသုံးပြုကာ အဝိုင်းထုတ် ထုတ်လုပ်ထားသော မော်ဒယ်များ သို့မဟုတ် Hugging Face မော်ဒယ်များကို ချိတ်ဆက် ပြီး run နိုင်ခြင်း |
| 11 | ဒေသတွင်း မော်ဒယ်များကို အပြင်လုပ်ဆောင်ချက်များ ခေါ်ယူနိုင် စေသော tool-calling ပုံစံ အသုံးပြုနိုင်ခြင်း |
| 12 | Zava Creative Writer အတွက် ဘရောင်ဇာအခြေပြု UI တည်ဆောက်နိုင်ခြင်း၊ တိုက်ရိုက်ဒေတာ စီးရီးပေးနိုင်ခြင်း |

---

## လိုအပ်ချက်များ

| လိုအပ်ချက် | အသေးစိတ် |
|-------------|---------|
| **ဟားဒ်ဝဲ** | အနည်းဆုံး 8 GB RAM (16 GB ကို အကြံပြုပြီး); AVX2 ကို ပံ့ပိုးသော CPU သို့မဟုတ် ပံ့ပိုးသော GPU |
| **စက်မူကွဲ** | Windows 10/11 (x64/ARM), Windows Server 2025, သို့မဟုတ် macOS 13+ |
| **Foundry Local CLI** | Windows အတွက် `winget install Microsoft.FoundryLocal` ဖြင့်၊ macOS အတွက် `brew tap microsoft/foundrylocal && brew install foundrylocal` ဖြင့် 설치 လုပ်နိုင်ပါသည်။ အသေးစိတ်ကို [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) တွင် ကြည့်ရှုနိုင်ပါသည်။ |
| **ဘာသာစကား runtime** | **Python 3.9+** နှင့်/သို့မဟုတ် **.NET 9.0+** နှင့်/သို့မဟုတ် **Node.js 18+** |
| **Git** | ဤ repository ကို clone မည့်အတွက် |

---

## စတင်အသုံးပြုခြင်း

```bash
# ၁။ ရိုက်ခုံကို ကလုန်းလုပ်ပါ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# ၂။ Foundry Local ကို ထည့်သွင်းပြီးကြောင်း စစ်ဆေးပါ
foundry model list              # ရရှိနိုင်သော မော်ဒယ်များကို စာရင်းပြုစုပါ
foundry model run phi-3.5-mini  # အပြန်အလှန် ပြောဆိုမည့် စာနယ်ဇင်းကို စတင်ပါ

# ၃။ သင့်ဘာသာစကား လမ်းကြောင်းကို ရွေးချယ်ပါ (ပြည့်စုံဖို့ အပိုင်း ၂ လေ့လာခန်းကို ကြည့်ပါ)
```

| ဘာသာစကား | အမြန်စတင်ခြင်း |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## အလုပ်ရုံဆွေးနွေးပွဲ အစိတ်အပိုင်းများ

### အစိတ်အပိုင်း 1: Foundry Local နှင့် စတင်ခြင်း

**Laboratory guide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local ဆိုတာဘာလဲ၊ ဘယ်လို လည်ပတ်သလဲ
- Windows နှင့် macOS တွင် CLI 설치ခြင်း
- မော်ဒယ်များ အတန်းသတ်, ဒေါင်းလုပ်လုပ်ခြင်း၊ လည်ပတ်ခြင်း များ ရှာဖွေခြင်း
- မော်ဒယ် အယ်လ်အိုင်(aliases) များနှင့် dynamic ports များ တွန်းလှန်၍ သဘောပေါက်ခြင်း

---

### အစိတ်အပိုင်း 2: Foundry Local SDK နက်ရှိုင်းလေ့လာခြင်း

**Laboratory guide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- SDK မှ CLI ထက် အကြောင်း ကြောင်း အသုံးပြုမှု
- Python, JavaScript, C# အတွက် SDK API အပြည့်အစုံ ကိုးကားချက်
- ဝန်ဆောင်မှု စီမံခန့်ခွဲမှု၊ ကတ်တလော့ ဗျူအိုင်၊ မော်ဒယ် ဘဝစက်ဝိတ် (ဒေါင်းလုပ်, တင်သွင်း, ပယ်ဖျက်)
- အမြန်စတင်မှု ပုံစံများ - Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` မီတာဒေတာများ၊ အယ်လ်အိုင်များနှင့် ဟာ့ဒ်ဝဲ အကောင်းဆုံး မော်ဒယ် ရွေးချယ်ခြင်း

---

### အစိတ်အပိုင်း 3: SDK များနှင့် API များ

**Laboratory guide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, C# မှ Foundry Local ဆာဗာ ချိတ်ဆက်ခြင်း
- Foundry Local SDK အား အသုံးပြု၍ စခရင်တင် ဝန်ဆောင်မှု များကို အပရိုဂရမ်ဖြင့် စီမံခန့်ခွဲခြင်း
- OpenAI-compatible API ဖြင့် streaming chat မွေ့အဖွဲ့များ လည်ပတ်ခြင်း
- ဘာသာစကားတစ်ခုချင်းစီအတွက် SDK နည်းဗျုပြုမူ၏ ကိုးကားချက်

**Code ตัวอย่าง:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local.py` | အခြေခံ streaming chat |
| C# | `csharp/BasicChat.cs` | .NET နဲ့ streaming chat |
| JavaScript | `javascript/foundry-local.mjs` | Node.js နဲ့ streaming chat |

---

### အစိတ်အပိုင်း 4: Retrieval-Augmented Generation (RAG)

**Laboratory guide:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG ဆိုတာ ဘာလဲ နှင့် အရေးကြီးချက်
- မေတ္တာသိပ္ပံစနစ် အတွင်းမှတ်ဉာဏ် မှတ်တမ်းတည်ဆောက်ခြင်း
- စကားလုံးထိပ်တိုက် ဆွဲယူမှု နှင့် အမှတ်ပေးစနစ်
- အခြေခံထားသော စနစ် ပရိုမ့်ပ် များ ဖန်တီးခြင်း
- အစစ်အသုံးပြုလမ်းပြ RAG လမ်းကြောင်း လည်ပတ်ခြင်း

**Code ตัวอย่าง:**

| ဘာသာစကား | ဖိုင် |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### အစိတ်အပိုင်း 5: AI အေးဂျင့်များ ဆောက်လုပ်ခြင်း

**Laboratory guide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI အေးဂျင့်ဆိုတာဘာလဲ (သန့်ရှင်း LLM ခေါ်ဆိုမှုနှင့်မတူ)
- `ChatAgent` ပုံစံနှင့် Microsoft Agent Framework
- စနစ်ညွှန်ကြားချက်များ၊ ပုဂ္ဂိုလ်များနှင့် မျိုးစုံပြောဆိုဆက်ဆံမှုများ
- JSON စနစ်သန့်အမြဲ ထွက်လာမှု

**Code ตัวอย่าง:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework ပါသော တစ်ဦးတည်း အေးဂျင့် |
| C# | `csharp/SingleAgent.cs` | တစ်ဦးတည်း အေးဂျင့် (ChatAgent ပုံစံ) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | တစ်ဦးတည်း အေးဂျင့် (ChatAgent ပုံစံ) |

---

### အစိတ်အပိုင်း 6: အမြင့်ကြားအေးဂျင့် လုပ်ငန်းစဉ်များ

**Laboratory guide:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- အမြင့်ကြားအေးဂျင့် လမ်းကြောင်းများ - သုတေသနသူ → စာရေးသူ → တည်းဖြတ်သူ
- အဆက်လိုက် စီမံခန့်ခွဲမှု နှင့် ပြန်လည်တုံ့ပြန်မှု ဆော့လုပ်ငန်းများ
- မျှဝေပြီး ပုံသေသော ပံ့ပိုးမှုများနှင့် လက်တွဲလှုပ်ရှားခြင်းများ
- ကိုယ်ပိုင် multi-agent လုပ်ငန်းစဉ် တီထွင်ခြင်း

**Code ตัวอย่าง:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | သုံးဦးအေးဂျင့် လမ်းကြောင်း |
| C# | `csharp/MultiAgent.cs` | သုံးဦးအေးဂျင့် လမ်းကြောင်း |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | သုံးဦးအေးဂျင့် လမ်းကြောင်း |

---

### အစိတ်အပိုင်း 7: Zava Creative Writer - Capstone Application

**Laboratory guide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- အထွက်အလာ လုပ်ငန်းစတိုင် multi-agent အက်ပ်တစ်ခု၊ အထူးပြု အေးဂျင့် ၄ ဦးပါဝင်သည်
- အဆက်လိုက် pipeline နှင့် ပြန်လည်တုံ့ပြန်မှု ဆော့လုပ်ငန်း မောင်းနှင်မှု
- Streaming output, ကုန်ပစ္စည်း ကတ်တလော့ရှာဖွေမှု, JSON များဖြန့်ဝေမှု
- Python (FastAPI), JavaScript (Node.js CLI), နှင့် C# (.NET console) တွင် ပြည့်စုံတည်ဆောက်မှု

**Code ตัวอย่าง:**

| ဘာသာစကား | ဖိုလ်ဒါ | ဖော်ပြချက် |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI ဝက်ဘ် ဝန်ဆောင်မှု နှင့် စီမံခန့်ခွဲသူ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI အက်ပ် |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console app |

---

### အစိတ်အပိုင်း 8: သုံးသပ်ခြင်း ဦးတည် ဖွံ့ဖြိုးတိုးတက်မှု

**Laboratory guide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 골든 데이터셋များနှင့် AI အေးဂျင့်များ အတွက် စနစ်တကျ သုံးသပ်ခြင်း ဖွံ့ဖြိုးတိုးတက်မှုစနစ် တည်ဆောက်ခြင်း
- စည်းကမ်းတစ်ရပ်ရပ် (အရှည်, စကားလုံးအတွင်းခံ အစရှိသည့်) နှင့် LLM-as-judge အမှတ်ပေးမှု
- ပရိုမ့်အား ပုံမှန် နှိုင်းယှဉ်ခြင်း နှင့် စုပေါင်း အမှတ်ဇယား
- Zava Editor အေးဂျင့် ပုံစံကို အစိတ်အပိုင်း 7 မှ offline စစ်ဆေးမှု အစုအဖွဲ့အဖြစ် တိုးချဲ့ခြင်း
- Python, JavaScript, C# လမ်းကြောင်း

**Code ตัวอย่าง:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | သုံးသပ်ရေး စနစ် |
| C# | `csharp/AgentEvaluation.cs` | သုံးသပ်ရေး စနစ် |
| JavaScript | `javascript/foundry-local-eval.mjs` | သုံးသပ်ရေး စနစ် |

---

### အစိတ်အပိုင်း 9: Whisper ဖြင့် အသံ အသံဖော်ပြခြင်း

**Laboratory guide:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- ဒေသတွင်း OpenAI Whisper ဖြင့် အသံမှ စာသား သို့ ဖော်ပြခြင်း
- ကိုယ်ပိုင် သတင်းအချက်အလက်ကြီးရှု ထိန်းသိမ်းစေသော ကြားနာမှု
- Python, JavaScript, C# မှ `client.audio.transcriptions.create()` (Python/JS) နှင့် `AudioClient.TranscribeAudioAsync()` (C#) အသုံးပြု
- Zava မှ နမူနာ အသံ ဖိုင်များ ပါဝင်သည်

**Code ตัวอย่าง:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper အသံ ဖော်ပြမှု |
| C# | `csharp/WhisperTranscription.cs` | Whisper အသံ ဖော်ပြမှု |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper အသံ ဖော်ပြမှု |

> **မှတ်ချက်:** ဤ လေ့ကျင့်ခန်းတွင် **Foundry Local SDK** ကို အသုံးပြုကာ Whisper မော်ဒယ်ကို အပရိုဂရမ်ဖြင့် ဒေါင်းလုပ်လုပ်၍ တင်သွင်းပြီး ဒေသတွင်း OpenAI-compatible endpoint သို့ အသံ ပို့၍ ဖော်ပြချက်များ ရယူသည်။ Whisper မော်ဒယ် (`whisper`) သည် Foundry Local ကတ်တလော့တွင်စာရင်းဝင်ပြီး စက်ပေါ်တွင်သာ လည်ပတ်သည် - ကလောင်း API key များ သို့မဟုတ် ကွန်ရက် မလိုအပ်ပါ။

---

### အစိတ်အပိုင်း 10: အသုံးပြုသူ သို့မဟုတ် Hugging Face မော်ဒယ်များ အသုံးပြုခြင်း

**Laboratory guide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face မော်ဒယ်များကို ONNX Runtime GenAI မော်ဒယ် တည်ဆောက်စက်ဖြင့် ပြုလုပ်ထားသော ONNX ဖော်မတ်တွင် ကွန်ပိုင်လုပ်ခြင်း
- ဟာ့ဒ်ဝဲအလိုက် ကွန်ပိုင်လုပ်မှု (CPU, NVIDIA GPU, DirectML, WebGPU) နှင့် အရည်အသွေးချိန်ညှိမှု (int4, fp16, bf16)
- Foundry Local အတွက် chat-template ဖိုင် ဖန်တီးခြင်း
- ကွန်ပိုင်လုပ်ထားသော မော်ဒယ်များကို Foundry Local ကတ်တလော့ မှ ထည့်သွင်းခြင်း
- CLI, REST API, OpenAI SDK ဖြင့် လည်ပတ်နိုင်ခြင်း
- ကိုးကားပုံစံ - Qwen/Qwen3-0.6B ကို အစမှ အဆုံး compile လုပ်ခြင်း

---

### အစိတ်အပိုင်း 11: ဒေသတွင်း မော်ဒယ်များနှင့် tool-calling ဖြင့် ပုံစံ

**Laboratory guide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- ဒေသတွင်း မော်ဒယ်များ စက်ပေါ်မှ အပြင်လုပ်ဆောင်ချက်များခေါ်ရန် (tool/function calling) စနစ်ဖွင့်ခြင်း
- OpenAI function-calling ကွက်တိပုံစံဖြင့် tool schemas သတ်မှတ်ခြင်း
- multi-turn tool-calling ဆွေးနွေးမှု လည်ပတ်မှုကို ကိုင်တွယ်ခြင်း
- ဒေသတွင်းမှ tool ခေါ်ယူမှု တည်ဆောက်ပြီး မော်ဒယ်ထံ ပြန်ရလဒ်ပေးခြင်း
- tool-calling အတွက် သင့်တော်သည့် မော်ဒယ်များ ရွေးချယ်ခြင်း (Qwen 2.5, Phi-4-mini)
- SDK ၏ မူရင်း `ChatClient` ကို Tool calling အတွက် အသုံးပြုခြင်း (JavaScript)

**Code ตัวอย่าง:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | မိုးလေဝသ/လူဦးရေ ကိရိယာများဖြင့် Tool calling |
| C# | `csharp/ToolCalling.cs` | .NET ဖြင့် Tool calling |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ဖြင့် Tool calling |

---

### အစိတ်အပိုင်း 12: Zava Creative Writer အတွက် ဝက်ဘ် UI တည်ဆောက်ခြင်း

**Laboratory guide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer အတွက် ဘရောင်ဇာအခြေပြု အရှေ့ဆုံး မျက်နှာပြင်ထည့်သွင်းခြင်း
- Python (FastAPI), JavaScript (Node.js HTTP), နှင့် C# (ASP.NET Core) မှ မျှဝေ UI ကို ဝန်ဆောင်မှုပေးခြင်း
- Fetch API နှင့် ReadableStream ကို အသုံးပြုကာ စီးရီး NDJSON များ ဘရောင်ဇာတွင် သုံးစွဲခြင်း
- တိုက်ရိုက် အေးဂျင့် အခြေအနေ အမှတ်အသားနှင့် လက်ရှိ ဆောင်းပါးစာသား စီးရီး ပေးပို့ခြင်း

**ကုဒ် (မျှဝေ UI):**

| ဖိုင် | ဖော်ပြချက် |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | စာမျက်နှာ layout |
| `zava-creative-writer-local/ui/style.css` | ပုံသဏ္ဍာန်ပေးခြင်း |
| `zava-creative-writer-local/ui/app.js` | စီးရီးဖတ်သူ နှင့် DOM update လုပ်ဆောင်ချက် |

**နောက်ခံပိုင်း ထည့်သွင်းချက်များ:**

| ဘာသာစကား | ဖိုင် | ဖော်ပြချက် |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | static UI ကို ဝန်ဆောင်မှုပေးရန် အပ်ဒိတ်လုပ်ထားသည် |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | စီမံခန့်ခွဲသူ ဦးဆောင် HTTP ဆာဗာ အသစ် |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | ASP.NET Core minimal API project အသစ် |

---

### အစိတ်အပိုင်း 13: အလုပ်ရုံဆွေးနွေးပွဲ ပြီးဆုံးပါပြီ
**Lab guide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- သင့်တည်ဆောက်ခဲ့သောအစိတ်အပိုင်း ၁၂ အားလုံး၏အကျဉ်းချုပ်
- သင့်အပလီကေးရှင်းများကို တိုးချဲ့နိုင်ရန်ထပ်မံအကြံပြုချက်များ
- အရင်းအမြစ်များနှင့်စာရွက်စာတမ်းများနှင့်လင့်ခ်များ

---

## Project Structure

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

## Resources

| Resource | Link |
|----------|------|
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |
| Model catalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Getting started guide | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Reference | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licence

ဒီဆောင်းပါးပစ္စည်းသည် ပညာရေးဆိုင်ရာရည်ရွယ်ချက်များအတွက်ရရှိသည်။

---

**အဆင်ပြေစေဖို့ ဆောက်လုပ်ကြပါစို့! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**မှတ်ချက်**  
ဒီစာရွက်စာတမ်းကို AI ဘာသာပြန်အမှုဆောင် [Co-op Translator](https://github.com/Azure/co-op-translator) ကို အသုံးပြု၍ ဘာသာပြန်ထားပါသည်။ ကျနော်တို့သည် တိကျမှန်ကန်မှု ရှိအောင် ကြိုးစားပေမယ့် အလိုအလျောက် ဘာသာပြန်ချက်များတွင် အမှားများ သို့မဟုတ် တိကျမှု မရှိမှုများ ရှိနိုင်ကြောင်း ကျေးဇူးပြု၍ သိရှိ ပေးပါ။ မူရင်းစာရွက်စာတမ်းသည် မိမိဘာသာစကားဖြင့် ရှိသော အရာ ဖြစ်ပြီး ကန့်သတ်ထားသော အချက်အလက်များအတွက် ဖြစ်ပါက ပရော်ဖက်ရှင်နယ် လူ့ဘာသာပြန်ခြင်းကို အကြံပြုပါသည်။ ဒီဘာသာပြန်ချက်ကို အသုံးပြုခြင်းကြောင့် ဖြစ်ပေါ်လာနိုင်သော နားလည်မှုမမှန်ခြင်းများ သို့မဟုတ် မှားယွင်းသောအဓိပ္ပါယ်ဆောင်မှုများအတွက် ကျနော်တို့ တာဝန်မယူပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->