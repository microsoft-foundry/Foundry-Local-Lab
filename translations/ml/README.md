<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# ഫൗണ്ട്രി ലോക്കൽ വർക്‌ഷോപ്പ് - ഓൺ-ഡിവൈസിൽ AI ആപ്പുകൾ നിർമ്മിക്കുക

നിങ്ങളുടെ സ്വന്തം യന്ത്രത്തിൽ ഭാഷാ മോഡലുകൾ പ്രവർത്തിപ്പിക്കുകയും [Foundry Local](https://foundrylocal.ai) ഉം [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) ഉം ഉപയോഗിച്ച് ബുദ്ധിമുട്ടുള്ള ആപ്പുകൾ നിർമ്മിക്കുകയും ചെയ്യുന്നതിനുള്ള ഒരു പ്രായോഗിക വർക്‌ഷോപ്പ്.

> **Foundry Local എന്താണ്?** Foundry Local നിങ്ങളുടെ ഹാർഡ്‌വെയറിൽ മുഴുവനായി ഡൗൺലോഡ് ചെയ്യാനും, മാനേജ് ചെയ്യാനും, സർവ് ചെയ്യാനും അനുവദിക്കുന്ന ഒരു ലഘു റൺടൈം ആണ്. ഇത് **OpenAI-സമ്മതിച്ച API** പ്രദാനം ചെയ്യുന്നു, അതിനാൽ OpenAI സംസാരിക്കുന്ന ഏതു ടൂൾ അല്ലെങ്കിൽ SDK ആയാലും കണക്റ്റ് ചെയ്യാം - ക്ലൗഡ് അക്കൗണ്ട് ആവശ്യമില്ല.

### 🌐 ബഹുഭാഷാ പിന്തുണ

#### GitHub പ്രവർത്തനം വഴി പിന്തുണയ്ക്കുന്നു (സ്വയം ക്രിയാത്മകവും എല്ലായ്പ്പോഴും പുതുക്കിയതുമായി)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](./README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **ഉളളിൽ നിന്ന് ക്ലോൺ ചെയ്യാൻ ഇഷ്ടപ്പെടുന്നോ?**
>
> ഈ റിപോസിറ്ററിയിൽ 50+ ഭാഷാ വിവർത്തനങ്ങൾ ഉൾക്കൊള്ളുന്നു, ഇത് ഡൗൺലോഡ് വലുപ്പം വളരെ കൂടുന്നു. വിവർത്തനങ്ങൾ ഇല്ലാതെ ക്ലോൺ ചെയ്യാൻ sparse checkout ഉപയോഗിക്കുക:
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
> ഇത് നിങ്ങൾക്ക് കോഴ്സ് പൂർത്തിയാക്കുന്നതിനായി ആവശ്യമായ എല്ലാ ഫയലുകളുംMuch വേഗത്തിൽ ഡൗൺലോഡ് ചെയ്യുന്നു.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## പഠന ലക്ഷ്യങ്ങൾ

ഈ വർക്‌ഷോപ്പ് കഴിഞ്ഞാൽ നിങ്ങൾക്ക് കഴിയുന്നതാണ്:

| # | ലക്ഷ്യം |
|---|-----------|
| 1 | Foundry Localインസ്റ്റാൾ ചെയ്തു CLI ഉപയോഗിച്ച് മോഡലുകൾ മാനേജ് ചെയ്യുക |
| 2 | പ്രോഗ്രാമാറ്റിക് മോഡൽ മാനേജ്മെന്റിനായി Foundry Local SDK API ൽ വിദഗ്ധത നേടുക |
| 3 | Python, JavaScript, C# SDKs ഉപയോഗിച്ച് ലോക്കൽ ഇൻഫറൻസ് സർവറിൽ കണക്റ്റുചെയ്യുക |
| 4 | നിങ്ങളുടെ സ്വന്തമായ ഡാറ്റയിൽ അടിസ്ഥാനമാക്കി ഉത്തരം നൽകുന്ന Retrieval-Augmented Generation (RAG) പൈപ്പ്‌ലൈൻ നിർമ്മിക്കുക |
| 5 | സ്ഥിരതയുള്ള നിർദ്ദേശങ്ങളും വ്യക്തിത്വങ്ങളുമായ AI ഏജന്റുകൾ സൃഷ്ടിക്കുക |
| 6 | ഫീഡ്ബാക്ക് ലൂപ്പുകളുള്ള മൾട്ടി-ഏജന്റ് വർക്‌ഫ്ലോകൾ ഓർക്കസ്ട്രേറ്റ് ചെയ്യുക |
| 7 | പ്രൊഡക്ഷൻ ക്യാപ്‌സ്റ്റോൺ ആപ്പ് - Zava Creative Writer പരിശോധിക്കുക |
| 8 | ഗോൾഡൻ ഡാറ്റാസെറ്റുകളുമായി LLM-അസ്‌-ജഡ്ജ് സ്കോറിംഗ് ഉപയോഗിച്ച് മൂല്യനിർണ്ണയ വ്യവസ്ഥകൾ നിർമ്മിക്കുക |
| 9 | Whisper ഉപയോഗിച്ച് ഓഡിയോ ട്രാൻസ്ക്രൈബ് ചെയ്യുക - ഫൗണ്ട്രി ലോക്കൽ SDK ഉപയോഗിച്ച് ഡിവൈസിൽ സ്പീച്ച്-ടു-ടെക്‌സ്‌റ്റ് |
| 10 | ONNX Runtime GenAI, Foundry Local ഉപയോഗിച്ച് കസ്റ്റം അല്ലെങ്കിൽ Hugging Face മോഡലുകൾ കമ്പൈൽ ചെയ്ത് ഓടിക്കുക |
| 11 | ഉപകരണം-കോൾ ചെയ്ത മാതൃകയെ ഉപയോഗിച്ച് ലോക്കൽ മോഡലുകൾക്ക് ബാഹ്യ ഫംഗ്ഷനുകൾ വിളിക്കാൻ കഴിഞ്ഞു |
| 12 | Zava Creative Writer നു വേണ്ടി റിയൽ-ടൈം സ്റ്റ്രീമിങ്ങുള്ള ബ്രൗസർ-അധിഷ്ഠിത UI നിർമ്മിക്കുക |

---

## മുൻപ് ആവശ്യമുള്ളത്

| ആവശ്യകത | വിശദാംശങ്ങൾ |
|-------------|---------|
| **ഹാർഡ്‌വെയർ** | കുറഞ്ഞത് 8 GB RAM (16 GB നിർദ്ദേശിക്കുന്നു); AVX2-സമർത്ഥ CPU അല്ലെങ്കിൽ പിന്തുണയുള്ള GPU |
| **ഓപ്പറേറ്റിങ് സിസ്റ്റം** | Windows 10/11 (x64/ARM), Windows Server 2025, അല്ലെങ്കിൽ macOS 13+ |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) അല്ലെങ്കിൽ `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) വഴി ഇൻസ്റ്റാൾ ചെയ്യുക. വിശദാംശങ്ങൾക്ക് [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) കാണുക. |
| **ഭാഷ റൺടൈം** | **Python 3.9+**, അല്ലെങ്കിൽ **.NET 9.0+**, അല്ലെങ്കിൽ **Node.js 18+** |
| **Git** | ഈ റിപ്പോസിറ്ററി ക്ലോൺ ചെയ്യുന്നതിനായി |

---

## ആരംഭിക്കുന്നത്

```bash
# 1. റിപോസിറ്ററി ക്ലോൺ ചെയ്യുക
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ഫൗണ്ടറി ലോക്കൽ ഇന്‍സ്റ്റാള്‍ ചെയ്തിട്ടുണ്ടെന്ന് സ്ഥിരീകരിക്കുക
foundry model list              # ലഭ്യമായ മോഡലുകള്‍ പട്ടികപ്പെടുത്തുക
foundry model run phi-3.5-mini  # ഇന്ററാക്ടീവ് ചാറ്റ് ആരംഭിക്കുക

# 3. നിങ്ങളുടെ ഭാഷാ ട്രാക്ക് തിരഞ്ഞെടുക്കുക (പൂർണമാകും ക്രമീകരണത്തിന് ഭാഗം 2 ലാബ് കാണുക)
```

| ഭാഷ | വേഗം ആരംഭിക്കുക |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## വർക്‌ഷോപ്പ് ഭാഗങ്ങൾ

### ഭാഗം 1: Foundry Local ഉപയോഗിച്ച് ആരംഭിക്കൽ

**ലാബ് ഗൈഡ്:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local എന്താണെന്നും ഇത് എങ്ങനെ പ്രവർത്തിക്കുന്നുമെന്നുമുള്ള അവലോകനം
- Windows, macOS-ൽ CLI ഇൻസ്റ്റാൾ ചെയ്യൽ
- മോഡലുകൾ പരിശോധിക്കൽ - ലിസ്റ്റ് ചെയ്യൽ, ഡൗൺലോഡ് ചെയ്യൽ, പ്രവർത്തിപ്പിക്കൽ
- മോഡൽ അലിയാസുകൾ ആൻഡ് ഡൈനാമിക് പോർട്ടുകളുടെ ബോധം

---

### ഭാഗം 2: Foundry Local SDK ആഴത്തിലുള്ള പഠനം

**ലാബ് ഗൈഡ്:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- അപ്ലിക്കേഷൻ ഡെവലപ്പ്മെന്റിനായി CLI നെക്കാൾ SDK ഉപയോഗിക്കുന്നതിനുള്ള കാരണങ്ങൾ
- Python, JavaScript, C# -കായ SDK API ഫുൾ റഫറൻസ്
- സർവീസ് മാനേജ്മെന്റ്, കാറ്റലോഗ് ബ്രൗസിംഗ്, മോഡൽ ലൈഫ്‌സൈക്കിൾ (ഡൗൺലോഡ്, ലോഡ്, അൺലോഡ്)
- വേഗം ആരംഭിക്കൽ പേട്ടേൺസ്: Python കൺസ്ട്രക്ടർ ബൂട്ട്സ്ട്രാപ്പ്, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` മെറ്റാഡാറ്റ, അലിയാസുകൾ, ഹാർഡ്‌വെയർ-ഓപ്റ്റിമൽ മോഡൽ തിരഞ്ഞെടുപ്പ്

---

### ഭാഗം 3: SDKs ആൻഡ് APIs

**ലാബ് ഗൈഡ്:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, C# ഉപയോഗിച്ച് Foundry Local -ലേക്ക് കണക്റ്റുചെയ്യൽ
- Foundry Local SDK ഉപയോഗിച്ച് പ്രോഗ്രാമാറ്റിക് ആയി സർവീസ് മാനേജ് ചെയ്യുക
- OpenAI-സമ്മതിച്ച API വഴി സ്റ്റ്രീമിംഗ് ചാറ്റ് പൂർത്തീകരണങ്ങൾ
- ഓരോ ഭാഷയ്ക്കും SDK മെത്തഡ് റഫറൻസ്

**കോഡ് സാമ്പിൾസ്:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local.py` | അടിസ്ഥാന സ്റ്റ്രീമിംഗ് ചാറ്റ് |
| C# | `csharp/BasicChat.cs` | .NET ഉപയോഗിച്ച് സ്റ്റ്രീമിംഗ് ചാറ്റ് |
| JavaScript | `javascript/foundry-local.mjs` | Node.js ഉപയോഗിച്ച് സ്റ്റ്രീമിംഗ് ചാറ്റ് |

---

### ഭാഗം 4: Retrieval-Augmented Generation (RAG)

**ലാബ് ഗൈഡ്:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG എന്താണെന്നും അതിന്റെ പ്രാധാന്യം
- ഒരു ഇൻ-മെമ്മറി നോളജ് ബേസ് നിർമ്മിക്കൽ
- സ്കോർ ചെയ്യലുള്ള കീ്വേർഡ്-ഓവർലാപ്പ് റിട്രീവൽ
- ഗ്രൗണ്ടഡ് സിസ്റ്റം പ്രൊംപ്റ്റുകൾ രൂപപ്പെടുത്തൽ
- ഡിവൈസിൽ പൂർണ്ണമായ RAG പൈപ്പ്‌ലൈൻ പ്രവർത്തിപ്പിക്കൽ

**കോഡ് സാമ്പിൾസ്:**

| ഭാഷ | ഫയൽ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ഭാഗം 5: AI ഏജന്റുകൾ നിർമ്മിക്കൽ

**ലാബ് ഗൈഡ്:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ഏജന്റ് എന്താണ് (ഒരു ക_RAW_LLM കോളിനോട് താരതമ്യം ചെയ്യുമ്പോൾ)
- `ChatAgent` പേറ്റേൺ, Microsoft Agent Framework
- സിസ്റ്റം നിർദ്ദേശങ്ങൾ, വ്യക്തിത്വങ്ങൾ, മൾട്ടി-ടേൺ സംഭാഷണങ്ങൾ
- ഏജന്റുകളിൽ നിന്നുള്ള ഇൻകൃതമായ ഔട്ട്‌പുട്ട് (JSON)

**കോഡ് സാമ്പിൾസ്:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework ഉള്ള സിംഗിൾ ഏജന്റ് |
| C# | `csharp/SingleAgent.cs` | സിംഗിൾ ഏജന്റ് (ChatAgent പേറ്റേൺ) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | സിംഗിൾ ഏജಂಟ್ (ChatAgent പേറ്റേൺ) |

---

### ഭാഗം 6: മൾട്ടി-ഏജന്റ് പ്രവർത്തന പ്രക്രിയകൾ

**ലാബ് ഗൈഡ്:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- മൾട്ടി-ഏജന്റ് പൈപ്പ്ലൈനുകൾ: ഗവേഷകൻ → എഴുത്തുകാരൻ → എഡിറ്റർ
- നിരന്തര ഓർക്കസ്ട്രേഷൻ, ഫീഡ്ബാക്ക് ലൂപ്പുകൾ
- ഷെയർ ചെയ്‌ത കോൺഫിഗറേഷൻ, ഘടനാപരമായ ഹാൻഡ്-ഓഫുകൾ
- നിങ്ങളുടെ സ്വന്തം മൾട്ടി-ഏജന്റ് വർക്‌ഫ്ലോ സൃഷ്ടി

**കോഡ് സാമ്പിൾസ്:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | മൂന്ന് ഏജന്റ് പൈപ്പ്‌ലൈൻ |
| C# | `csharp/MultiAgent.cs` | മൂന്ന് ഏജന്റ് പൈപ്പ്‌ലൈൻ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | മൂന്ന് ഏജന്റ് പൈപ്പ്‌ലൈൻ |

---

### ഭാഗം 7: Zava Creative Writer - ക്യാപ്‌സ്റ്റോൺ അപ്ലിക്കേഷൻ

**ലാബ് ഗൈഡ്:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 പ്രത്യേക ഏജന്റുകളുള്ള പ്രൊഡക്ഷൻ-സ്‌റ്റൈൽ മൾട്ടി-ഏജന്റ് ആപ്പ്
- ഇന ഒളിക്കാനുള്ള പൈപ്പ്‌ലൈൻ എവാല്യുവേറ്റർ കൂട്ടിയിണക്കലുമായുള്ള ഫീഡ്ബാക്ക് ലൂപ്പുകൾ
- സ്റ്റ്രീമിംഗ് ഔട്ട്‌പുട്ട്, ഉൽപ്പന്ന കാറ്റലോഗ് തിരയൽ, ഘടനാപരമായ JSON ഹാൻഡ്-ഓഫുകൾ
- Python (FastAPI), JavaScript (Node.js CLI), C# (.NET കൺസോൾ) യിൽ പൂർണ രൂപത്തിൽ നടപ്പാക്കൽ

**കോഡ് സാമ്പിൾസ്:**

| ഭാഷ | ഡയറക്ടറി | വിവരണം |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ഓർക്കസ്ട്രേറ്റർ ഒള്ള FastAPI വെബ് സർവീസ് |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI അപ്ലിക്കേഷൻ |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 കൺസോൾ അപ്ലിക്കേഷൻ |

---

### ഭാഗം 8: മൂല്യനിർണ്ണയം വഴി വികസനം

**ലാബ് ഗൈഡ്:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- ഗോൾഡൻ ഡാറ്റാസെറ്റുകൾ ഉപയോഗിച്ച് AI ഏജന്റുകൾക്ക് ഒരു വ്യവസ്ഥാപിത മൂല്യനിർണ്ണയ ഫ്രെയിംവർക്ക് നിർമ്മിക്കുക
- നിയമാധിഷ്ഠിത പരിശോധനകൾ (ദൈർഘ്യം, കീーワード കവറേജ്, വിലക്കപ്പെട്ട വാക്കുകൾ) + LLM-അസ്-ജഡ്ജ് സ്കോറിംഗ്
- പ്രോംപ്റ്റ് വ്യത്യാസങ്ങളുടെ ഒരുമിച്ചു താരതമ്യം, സംഗ്രഹ സ്കോർക്കാർഡുകൾ സഹിതം
- ഭാഗം 7 ൽനിന്നുള്ള Zava Editor ഏജന്റ് പേറ്റേൺ ഒരു ഓഫ്‌ലൈൻ ടെസ്റ്റ് സൂട്ട് ആയി വിപുലീകരിക്കുന്നു
- Python, JavaScript, C# ട്രാക്കുകൾ

**കോഡ് സാമ്പിൾസ്:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | മൂല്യനിർണ്ണയ ഫ്രെയിംവർക്ക് |
| C# | `csharp/AgentEvaluation.cs` | മൂല്യനിർണ്ണയ ഫ്രെയിംവർക്ക് |
| JavaScript | `javascript/foundry-local-eval.mjs` | മൂല്യനിർണ്ണയ ഫ്രെയിംവർക്ക് |

---

### ഭാഗം 9: Whisper ഉപയോഗിച്ച് വോയിസ് ട്രാൻസ്ക്രിപ്ഷൻ

**ലാബ് ഗൈഡ്:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- ഓപ്പൺഎഐ വിസ്പർ ഉപയോഗിച്ച് লোকലിൽ Speech-to-text ട്രാൻ‌സ്‌ക്രിപ്ഷൻ
- പ്രൈവസി-ഫസ്റ്റ് ഓഡിയോ പ്രോസസ്സിംഗ് - ഓഡിയോ നിങ്ങളുടെ ഉപകരണത്തിൽ നിന്നും പുറത്ത് പോകാറില്ല
- Python, JavaScript, C# ട്രാക്കുകൾ `client.audio.transcriptions.create()` (Python/JS) மற்றும் `AudioClient.TranscribeAudioAsync()` (C#) ഉപയോഗിച്ച്
- കൈകളിൽ അഭ്യാസത്തിന് വേണ്ടി Zava-തീം ചെയ്ത സാമ്പിൾ ഓഡിയോ ഫയലുകൾ ഉൾക്കൊള്ളുന്നു

**കോർഡ് സാമ്പിളുകൾ:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | വിസ്പർ വോയ്‌സ് ട്രാൻസ്ക്ക്രിപ്ഷൻ |
| C# | `csharp/WhisperTranscription.cs` | വിസ്പർ വോയ്‌സ് ട്രാൻസ്ക്ക്രിപ്ഷൻ |
| JavaScript | `javascript/foundry-local-whisper.mjs` | വിസ്പർ വോയ്‌സ് ട്രാൻസ്ക്ക്രിപ്ഷൻ |

> **കുറിപ്പ്:** ഈ ലാബ് പ്രോഗ്രാമാറ്റിക് ആയി വിസ്പർ മോഡൽ ഡൗൺലോഡ് ചെയ്ത് ലോഡ് ചെയ്യാൻ **Foundry Local SDK** ഉപയോഗിക്കുന്നു, പിന്നീട് ട്രാൻസ്ക്ക്രിപ്ഷനായി ഓഡിയോ ലോക്കൽ OpenAI-സംഗതമ.Endpoint-ലേക്ക് അയയ്ക്കുന്നു. വിസ്പർ മോഡൽ (`whisper`) Foundry Local കാറ്റലോഗിൽ ലിസ്റ്റുചെയ്തിരിക്കുന്നു, മുഴുവൻ ഉപകരണത്തിൽ ഓടുന്നു - ക്ലൗഡ് API കീകൾ അല്ലെങ്കിൽ നെറ്റ്‌വർക്ക് ആക്സസ് ആവശ്യപ്പെടുന്നില്ല.

---

### ഭാഗം 10: കസ്റ്റം അല്ലെങ്കിൽ Hugging Face മോഡലുകൾ ഉപയോഗിക്കൽ

**ലാബ് ഗൈഡ്:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face മോഡലുകൾ ONNX Runtime GenAI മോഡൽ ബിൽഡർ ഉപയോഗിച്ച് ഓപ്റ്റിമൈസ്ഡ് ONNX ഫോർമാറ്റിലേക്ക് കമ്പൈൽ ചെയ്യൽ
- ഹാർഡ്‌വെയർ-സ്പെസിഫിക് കമ്പൈലേഷൻ (CPU, NVIDIA GPU, DirectML, WebGPU) ഒപ്പം ക്വാണ്ടൈസേഷൻ (int4, fp16, bf16)
- Foundry Local-ക്കുള്ള ചാറ്റ്-ടെംപ്ലേറ്റ് കോൺഫിഗറേഷൻ ഫയലുകൾ സൃഷ്‌ടിക്കൽ
- കമ്പൈൽ ചെയ്ത മോഡലുകൾ Foundry Local കാഷിലേക്ക് ചേർക്കൽ
- CLI, REST API, OpenAI SDK മുഖേന കസ്റ്റം മോഡലുകൾ റൺ ചെയ്യൽ
- റഫറൻസ് ഉദാഹരണം: Qwen/Qwen3-0.6B എന്റു-ടു-എന്റ് കമ്പൈലിംഗ്

---

### ഭാഗം 11: ലോക്കൽ മോഡലുകൾ ഉപയോഗിച്ച് ടൂൾ കോളിംഗ്

**ലാബ് ഗൈഡ്:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- ലോക്കൽ മോഡലുകൾക്ക് ഫംഗ്ഷനുകൾ (ടൂൾ/ഫംഗ്ഷൻ കോളിംഗ്) കോൾ ചെയ്യാൻ സജ്ജമാക്കൽ
- OpenAI ഫംഗ്ഷൻ-കോളിംഗ് ഫോർമാറ്റിൽ ടൂൾ സ്കീമകൾ നിർവചിക്കൽ
- മൾട്ടി-ടേൺ ടൂൾ-കോളിംഗ് സംഭാഷണ പ്രവാഹം കൈകാര്യം ചെയ്യൽ
- ടൂൾ കോളുകൾ ലോക്കലിൽ എക്സിക്യൂട്ട് ചെയ്ത് ഫലങ്ങൾ മോഡലിലേക്ക് തിരിച്ചു നൽകൽ
- ടൂൾ-കോളിംഗ് സിനാരിയോകൾക്കുള്ള ശരിയായ മോഡൽ തിരഞ്ഞെടുക്കൽ (Qwen 2.5, Phi-4-mini)
- SDKയിലെ നാറ്റീവ് `ChatClient` ടൂൾ കോളിംഗിന് (JavaScript)

**കോർഡ് സാമ്പിളുകൾ:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | കാലാവസ്ഥ/ജനസംഖ്യ ടൂളുകളുമായി ടൂൾ കോളിംഗ് |
| C# | `csharp/ToolCalling.cs` | .NET-ൽ ടൂൾ കോളിംഗ് |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ഉപയോഗിച്ച് ടൂൾ കോളിംഗ് |

---

### ഭാഗം 12: Zava Creative Writer-ക്കായുള്ള വെബ് UI നിർമ്മാണം

**ലാബ് ഗൈഡ്:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer-നുള്ള ബ്രൗസർ അടിസ്ഥാനമാക്കിയ ഫ്രണ്ട്‌എൻഡ് ചേർക്കൽ
- Python (FastAPI), JavaScript (Node.js HTTP), C# (ASP.NET Core) എന്നിവയിൽ നിന്നുള്ള പങ്കുവെക്കുന്ന UI സർവ് ചെയ്യൽ
- Fetch API, ReadableStream ഉപയോഗിച്ച് ബ്രൗസറിൽ സ്ട്രീമിംഗ് NDJSON ഉപഭോഗം
- ലൈവ് ഏജന്റ് സ്ഥിതി ബാഡ്ജുകളും റിയൽ-ടൈം ലേഖന ടെക്‌സ്‌റ്റ് സ്ട്രീമിംഗ്

**കോഡ് (പങ്കുവെക്കുന്ന UI):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | പേജ് ലേയൗട്ട് |
| `zava-creative-writer-local/ui/style.css` | സ്റ്റൈലിംഗ് |
| `zava-creative-writer-local/ui/app.js` | സ്ട്രീം റീഡറും DOM അപ്‌ഡേറ്റ് ലജിക്കും |

**ബാക്ക്‌എൻഡ് കൂട്ടിച്ചേർക്കലുകൾ:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | സ്റ്റാറ്റിക് UI സർവ് ചെയ്യാൻ അപ്ഡേറ്റുചെയ്‌തത് |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ഓർക്കസ്ട്രേറ്ററെ റാപ്പ് ചെയ്യുന്ന പുതിയ HTTP സെർവർ |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | പുതിയ ASP.NET കോർ മിനിമൽ API പ്രോജക്ട് |

---

### ഭാഗം 13: വർക്ക്‌ഷോപ്പ് സമ്പൂർത്തി

**ലാബ് ഗൈഡ്:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 12 ഭാഗങ്ങളിലായി നിങ്ങൾ നിർമ്മിച്ചതിന്റെ സംക്ഷേപം
- നിങ്ങളുടെ ആപ്ലിക്കേഷനുകൾ വികസിപ്പിക്കാനുള്ള അധിക ആശയങ്ങൾ
- വിഭവങ്ങൾക്കും ഡോക്യുമെന്റേഷനും വേണ്ടി ലിങ്കുകൾ

---

## പ്രോജക്ട് ഘടന

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

## വിഭവങ്ങൾ

| Resource | Link |
|----------|------|
| Foundry Local വെബ്സൈറ്റ് | [foundrylocal.ai](https://foundrylocal.ai) |
| മോഡൽ കാറ്റലോഗ് | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| തുടങ്ങാനുള്ള ഗൈഡ് | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK റഫറൻസ് | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft ഏജന്റ് ഫ്രെയിംവർക്ക് | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI വിസ്പർ | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## ലൈസൻസ്

ഈ വർക്ക്‌ഷോപ്പ് സാമഗ്രി വിദ്യാഭ്യാസാർത്ഥമാണ് നൽകുന്നത്.

---

**സന്തോഷം നിർമ്മിക്കൂ! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ഡിസ്ക്ലെയിമർ**:  
ഈ രേഖ [Co-op Translator](https://github.com/Azure/co-op-translator) എന്ന AI വിവർത്തന സേവനം ഉപയോഗപ്പെടുത്തി വിവർത്തനം ചെയ്‌തതാണ്. ഞങ്ങൾ കൃത്യതയ്ക്കായി പരിശ്രമിക്കുന്നുവെങ്കിലും, സ്വയംകൃതമായ വിവർത്തനങ്ങളിൽ പിഴവുകൾ അല്ലെങ്കിൽ അസാധുതകൾ ഉണ്ടാകാമെന്ന് ശ്രദ്ധിക്കുക. പ്രഥമ ഭാഷയിലുള്ള യഥാർത്ഥ രേഖ അധികാരമുള്ള ഉറവിടമായി കരുതണം. നിർണ്ണായക വിവരങ്ങൾക്ക്, പ്രൊഫഷണൽ മനുഷ്യ വിവർത്തനം പ്രശസ്തമാണ്. ഈ വിവർത്തനം ഉപയോഗിക്കുന്നതിൽ നിന്നുണ്ടാകുന്ന anumang തെറ്റിദ്ധാരണകൾക്കോ തെറ്റായ വ്യാഖ്യാനങ്ങളിലോ ഞങ്ങൾ ഉത്തരവാദികളല്ല.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->