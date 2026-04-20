<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# ഫൗണ്ട്രി ലൊക്കൽ വർ ക്ഷോപ് - ഡിവൈസിൽ എ ഐ ആപ്ലിക്കേഷനുകൾ നിർമ്മിക്കുക

നിങ്ങളുടെ സ്വന്തം മെഷീനിൽ ഭാഷാ മാതൃകകൾ 실행ചെയ്യുകയും [Foundry Local](https://foundrylocal.ai) ഉം [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) ഉം ഉപയോഗിച്ച് ബുദ്ധിമുട്ടുള്ള ആപ്ലിക്കേഷനുകൾ നിർമ്മിക്കാനുള്ള ഹാൻഡ്‌സ് ഓൺ വർക്ഷോപ്.

> **Foundry Local എന്താണ?** Foundry Local വളരെ ലഘുവായ ഒരു റൺടൈമാണ്, ഇത് നിങ്ങൾക്ക് ഭാഷാ മാതൃകകൾ പൂർണ്ണമായി നിങ്ങളുടെ ഹാർഡ്‌വെയറിൽ ഡൗൺലോഡ് ചെയ്യാനും, നിയന്ത്രിക്കാനും, സർവ് ചെയ്യാനും അനുവദിക്കുന്നു. ഇത് എല്ലാ OpenAI-ഉം ഉപയോഗിക്കുന്ന ടൂൾമാരും SDK-മാരും ಸಂಪರ್ಕിക്കാൻ കഴിയുന്ന **OpenAI-ഉം പൊരുത്തപ്പെടുന്ന API** തുറക്കുന്നു - ക്ലൗഡ് അക്കൗണ്ട് ആവശ്യമില്ല.

---

## പഠന ലക്ഷ്യങ്ങൾ

ഈ വർക്ഷോപിന്റെ അവസാനത്തോളം നിങ്ങൾക്ക്:

| # | ലക്ഷ്യം |
|---|-----------|
| 1 | Foundry Local ഇൻസ്റ്റാൾ ചെയ്ത് CLI ഉപയോഗിച്ച് മോഡലുകൾ മാനേജ് ചെയ്യാൻ കഴിയും |
| 2 | പ്രോഗ്രാമാറ്റിക് മോഡൽ മാനേജ്മെന്റിനായി Foundry Local SDK API- mastering ഉം ചെയ്യുക |
| 3 | Python, JavaScript, C# SDKകൾ ഉപയോഗിച്ച് പ്രാദേശിക ഇൻഫറൻസ് സർവറുമായി ബന്ധപ്പെടുക |
| 4 | നിങ്ങളുടെ സ്വന്തം ഡേറ്റയിൽ അടിസ്ഥാനമാക്കി ഉത്തരങ്ങൾ നൽകുന്ന Retrieval-Augmented Generation (RAG) പൈപ്പ്ലೈನ್ നിർമ്മിക്കുക |
| 5 | സ്ഥിരമായ നിർദേശങ്ങളും വ്യക്തിത്വങ്ങളുമായ എ ഐ ഏജന്റുകൾ നിർമ്മിക്കുക |
| 6 | പ്രതികരണ ലൂപുകളുള്ള മൾട്ടി-ഏജന്റ് വർക്ക്‌ഫ്ലോകൾ ഏകോപിപ്പിക്കുക |
| 7 | ഒരു പ്രൊഡക്ഷൻ ക്യാപ്‌സ്റ്റോൺ ആപ്പ് പരിശോധിക്കുക - Zava Creative Writer |
| 8 | സ്വർണ്ണ ഡാറ്റാസെറ്റുകളും LLM-as-judge സ്കോറിങ്ങും ഉപയോഗിച്ച് വിലയിരുത്തൽ ഫ്രെയിംവർക്ക് നിർമ്മിക്കുക |
| 9 | Foundry Local SDK ഉപയോഗിച്ചു ഓഡിയോ വാചകം പരിവർത്തനം ചെയ്യുക - ഡിവൈസിൽ സ്പീച്ച്-ടു-ടെക്സ്റ്റ് |
| 10 | ONNX Runtime GenAI ഉം Foundry Local ഉം ഉപയോഗിച്ച് കസ്റ്റം അല്ലെങ്കിൽ Hugging Face മോഡലുകൾ കമ്പൈൽ ചെയ്ത് ഓടിക്കുക |
| 11 | പ്രാദേശിക മോഡലുകളിൽ ടൂൾ-കോളിംഗ് മാതൃക ഉപയോഗിച്ച് പുറം ഫംഗ്ഷനുകൾ വിളിക്കാൻ സാധ്യമാക്കുക |
| 12 | Zava Creative Writer-ന് ബ്രൗസർ അടിസ്ഥാനമായ UI യാഥാർത്ഥ്യകക്ഷേമവും സ്ട്രീമിംഗ് ഉപയോഗിച്ച് നിർമ്മിക്കുക |

---

## മുൻകൂട്ടി ആവശ്യങ്ങൾ

| ആവശ്യം | വിശദാംശങ്ങൾ |
|-------------|---------|
| **ഹാർഡ്‌വെയർ** | കുറഞ്ഞത് 8 GB RAM (16 GB ശിപാർശ ചെയ്യുന്നു); AVX2-സഹായമുള്ള CPU അല്ലെങ്കിൽ പിന്തുണയുള്ള GPU |
| **ഓഎസ്** | Windows 10/11 (x64/ARM), Windows Server 2025, അല്ലെങ്കിൽ macOS 13+ |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) അല്ലെങ്കിൽ `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) വഴി ഇൻസ്റ്റാൾ ചെയ്യുക. വിശദാംശങ്ങൾക്ക് [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) കാണുക. |
| **ഭാഷ റിലേറ്റഡ് റൺടൈം** | **Python 3.9+** അല്ലെങ്കിൽ **.NET 9.0+** അല്ലെങ്കിൽ **Node.js 18+** |
| **Git** | ഈ റിപ്പോസിറ്ററി ക്ലോൺ ചെയ്യാനായി |

---

## ആരംഭിക്കൽ

```bash
# 1. റിപോസിറ്ററി ക്ലോൺ ചെയ്യുക
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ഫൗണ്ടറി ലോക്കൽ ഇൻസ്റ്റാൾ ചെയ്തിട്ടുണ്ടെന്ന് സ്ഥിരീകരിക്കുക
foundry model list              # ലഭ്യമായ മോഡലുകൾ പട്ടിക സംഘട്ടിപ്പിക്കുക
foundry model run phi-3.5-mini  # ഒരു ഇന്ററാക്ടീവ് ചാറ്റ് ആരംഭിക്കുക

# 3. നിങ്ങളുടെ ഭാഷ ട്രാക്ക് തിരഞ്ഞെടുക്കുക (പൂർണ സജ്ജീകരണത്തിന് ഭാഗം 2 ലാബ് കാണുക)
```

| ഭാഷ | വേഗം തുടങ്ങൽ |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## വർക്ഷോപ് ഭാഗങ്ങൾ

### ഭാഗം 1: Foundry Local ഉപയോഗിച്ച് ആരംഭിക്കൽ

**ലാബ് ഗൈഡ്:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local എന്താണെന്നും ഇത് എങ്ങനെ പ്രവർത്തിക്കുന്നതെന്നുമുള്ള ആനുകാലിക ത添കങ്ങൾ
- Windows, macOS-ൽ CLI ഇൻസ്റ്റോൾ ചെയ്യുക
- മോഡലുകൾ പരിശോധിക്കുക - ലിസ്റ്റ് ചെയ്യൽ, ഡൗൺലോഡ്, 실행
- മോഡൽ വ്യത്യാസങ്ങളും ഡൈനാമിക് പോർട്ടുകളും മനസിലാക്കുക

---

### ഭാഗം 2: Foundry Local SDK ആഴത്തിലുള്ള പഠനം

**ലാബ് ഗൈഡ്:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- അപ്ലിക്കേഷൻ വികസനത്തിന് CLI-വിനു പകരം SDK ഉപയോഗിക്കേണ്ടത് എന്തുകൊണ്ട്
- Python, JavaScript, C# പ്രധാന SDK API റഫറൻസ്
- സേവന മാനേജ്മെന്റ്, കാറ്റലോഗ് ബ്രൗസിംഗ്, മോഡൽ ലൈഫ് സൈകിൾ (ഡൗൺലോഡ്, ലോഡ്, അൺലോഡ്)
- പൈതൺ കൺസ്ട്രക്ടർ ബൂട്ട്സ്ട്രാപ്, ജാവാസ്ക്രിപ്റ്റ് `init()`, C# `CreateAsync()` ക്വിക്ക്-സ്റ്റാർട്ട് മാതൃകകൾ
- `FoundryModelInfo` മെടാ‌ഡേറ്റ, വ്യത്യാസങ്ങൾ, ഹാർഡ്‌വെയർ-ഒപ്റ്റിമൽ മോഡൽ തിരഞ്ഞെടുപ്പ്

---

### ഭാഗം 3: SDKകൾക്കും APIകൾക്കും

**ലാബ് ഗൈഡ്:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, C# ഉപയോഗിച്ച് Foundry Local-ലേക്ക് ബന്ധപ്പെടൽ
- Foundry Local SDK ഉപയോഗിച്ച് സർവീസിനെ പ്രോഗ്രാമാറ്റിക്ക് മാനേജുചെയ്യൽ
- OpenAI-സമ്മതപരമായ API വഴി സ്ട്രീമിംഗ് ചാറ്റ് പൂരിപ്പിക്കൽ
- ഓരോ ഭാഷയുടെയും SDK രീതികൾ റഫറൻസ്

**കോഡ് ഉദാഹരണങ്ങൾ:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local.py` | അടിസ്ഥാന സ്ട്രീമിംഗ് ചാറ്റ് |
| C# | `csharp/BasicChat.cs` | .NET കൊണ്ട് സ്ട്രീമിംഗ് ചാറ്റ് |
| JavaScript | `javascript/foundry-local.mjs` | Node.js ഉപയോഗിച്ച് സ്ട്രീമിംഗ് ചാറ്റ് |

---

### ഭാഗം 4: Retrieval-Augmented Generation (RAG)

**ലാബ് ഗൈഡ്:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG എന്താണെന്നും അത് എന്തുകൊണ്ട് പ്രാധാന്യമുള്ളതെന്നും
- ഇൻ-മെമ്മറിയിൽ നോളേജ് ബേസ് നിർമ്മിക്കൽ
- സ്‌കോറിങ്ങോടെയുള്ള കീവർഡ്-ഓവർലാപ് റെട്രീവൽ
- ഗ്രൗണ്ടഡ് സിസ്റ്റം പ്രോംപ്റ്റുകൾ നിർമ്മിക്കൽ
- ഡിവൈസിൽ ഒരു മുഴുവൻ RAG പൈപ്പ്ലൈൻ 실행ചെയ്യൽ

**കോഡ് ഉദാഹരണങ്ങൾ:**

| ഭാഷ | ഫയൽ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ഭാഗം 5: എ ഐ ഏജന്റുകൾ നിർമ്മിക്കൽ

**ലാബ് ഗൈഡ്:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- എ ഐ ഏജന്റ് എന്താണെന്ന് (സാധാരണ LLM വിളിനോട് താരതമ്യം ചെയ്തു)
- `ChatAgent` മാതൃകയും Microsoft Agent Framework ഉം
- സിസ്റ്റം നിർദേശങ്ങൾ, വ്യക്തിത്വങ്ങൾ, മൾട്ടി-ടേൺ സംഭാഷണങ്ങൾ
- ഏജന്റുകളിൽനിന്നുള്ള ഘടനാബദ്ധമായ ഔട്ട്‌പുട്ട് (JSON)

**കോഡ് ഉദാഹരണങ്ങൾ:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | ഏജന്റ് ഫ്രെയിംവർകോടു കൂടിയ ഏക ഏജന്റ് |
| C# | `csharp/SingleAgent.cs` | ഏക ഏജന്റ് (ChatAgent മാതൃക) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ഏക ഏജന്റ് (ChatAgent മാതൃക) |

---

### ഭാഗം 6: മൾട്ടി-ഏജന്റ് വർക്ക്‌ഫ്ലോസുകൾ

**ലാബ് ഗൈഡ്:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- മൾട്ടി-ഏജന്റ് പൈപ്പ്ലൈനുകൾ: റിസർച്ചർ → എഴുത്തുകാരൻ → എഡിറ്റർ
- പരമ്പരാഗത ഏകോപനം, പ്രതികരണ ലൂപുകൾ
- പങ്കുവെച്ച ക്രമീകരണങ്ങളും ഘടനാബദ്ധ ഹാൻഡ്-ഓഫുകളും
- നിങ്ങളുടെ സ്വന്തം മൾട്ടി-ഏജന്റുകൾക്ക് ഡിസൈൻ ചെയ്യുക

**കോഡ് ഉദാഹരണങ്ങൾ:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | മൂന്ന് ഏജന്റുകൾ ഉള്ള പൈപ്പ്ലൈൻ |
| C# | `csharp/MultiAgent.cs` | മൂന്ന് ഏജന്റുകൾ ഉള്ള പൈപ്പ്ലൈൻ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | മൂന്ന് ഏജന്റുകൾ ഉള്ള പൈപ്പ്ലൈൻ |

---

### ഭാഗം 7: Zava Creative Writer - ക്യാപ്‌സ്റ്റോൺ ആപ്ലിക്കേഷൻ

**ലാബ് ഗൈഡ്:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 പ്രത്യേക ഏജന്റുകളുമായി പ്രൊഡക്ഷൻ-സ്റ്റൈൽ മൾട്ടി-ഏജന്റ് ആപ്പ്
- നിരന്തരമായ പൈപ്പ്ലൈൻ, വിലയിരുത്തൽ നിയന്ത്രിത പ്രതികരണ ലൂപുകൾ
- സ്ട്രീമിംഗ് ഔട്ട്‌പുട്ട്, ഉത്പന്ന കാറ്റലോഗ് തിരച്ചിൽ, ഘടനാബദ്ധ JSON ഹാൻഡ്-ഓഫ്
- പൈതൺ (FastAPI), ജാവാസ്ക്രിപ്റ്റ് (Node.js CLI), C# (.NET കൺസോൾ) യിൽ പൂര്‍ണ പരിമിതി

**കോഡ് ഉദാഹരണങ്ങൾ:**

| ഭാഷ | ഡയറക്ടറി | വിവരണം |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ഓർക്കസ്ട്രേറ്റർ ഉള്ള FastAPI വെബ് സർവീസ് |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI ആപ്ലിക്കേഷൻ |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 കൺസോൾ ആപ്ലിക്കേഷൻ |

---

### ഭാഗം 8: വിലയിരുത്തൽ-നിഷേധിച്ച വികസനം

**ലാബ് ഗൈഡ്:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- സ്വർണ്ണ ഡാറ്റാസെറ്റുകൾ ഉപയോഗിച്ച് എ ഐ ഏജന്റുകൾക്കായി സമ്പൂർണ്ണ വിലയിരുത്തൽ ഫ്രെയിംവർക്ക് നിർമ്മിക്കുക
- നിയമാധിഷ്ഠിത പരിശോദനകൾ (നീളം, കീവേഡ്‌ കവറേജ്, നിരോധിത വാക്കുകൾ) + LLM-ജഡ്ജ് സ്കോറിംഗ്
- പ്രോംപ്റ്റ് വ്യത്യാസങ്ങൾ പരസ്പരം താരതമ്യം ചെയ്യുന്ന സമഗ്ര സ്കോർകാർഡുകൾ
- ഭാഗം 7 ലെ Zava Editor ഏജന്റ് മാതൃക ഓഫ്ലൈൻ ടെസ്റ്റ് സ്യൂട്ടിലേക്ക് വികസിപ്പിക്കൽ
- Python, JavaScript, C# ട്രാക്കുകൾ

**കോഡ് ഉദാഹരണങ്ങൾ:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | വിലയിരുത്തൽ ഫ്രെയിംവർക്ക് |
| C# | `csharp/AgentEvaluation.cs` | വിലയിരുത്തൽ ഫ്രെയിംവർക്ക് |
| JavaScript | `javascript/foundry-local-eval.mjs` | വിലയിരുത്തൽ ഫ്രെയിംവർക്ക് |

---

### ഭാഗം 9: Whisper ഉപയോഗിച്ച് വോയ്‌സ് ട്രാൻസ്ക്രിപ്ഷൻ

**ലാബ് ഗൈഡ്:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- പ്രാദേശികമായി ഓടുന്ന OpenAI Whisper ഉപയോഗിച്ച് സ്പീച്ച്-ടു-ടെക്സ്റ്റ് ട്രാൻസ്ക്രിപ്ഷൻ
- പ്രൈവസി ആദ്യം നോക്കി ഓഡിയോ പ്രോസസ്സ് ചെയ്യൽ - നിങ്ങളുടെ ഡിവൈസിൽ നിന്ന് ഓഡിയോ ഒരിക്കലും പുറം പോകില്ല
- Python, JavaScript, C# ട്രാക്കുകൾ `client.audio.transcriptions.create()` (Python/JS) & `AudioClient.TranscribeAudioAsync()` (C#) ഉപയോഗിച്ച്
- ഹാൻഡ്‌സ് ഓൺ പരിശീലനത്തിനായി Zava-തീംഡ് സാമ്പിൾ ഓഡിയോ ഫയലുകളും ഉൾപ്പെടുന്നു

**കോഡ് ഉദാഹരണങ്ങൾ:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper വോയ്‌സ് ട്രാൻസ്ക്രിപ്ഷൻ |
| C# | `csharp/WhisperTranscription.cs` | Whisper വോയ്‌സ് ട്രാൻസ്ക്രിപ്ഷൻ |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper വോയ്‌സ് ട്രാൻസ്ക്രിപ്ഷൻ |

> **ഗమనിക്കുക:** ഈ ലാബ് **Foundry Local SDK** ഉപയോഗിച്ച് പ്രോഗ്രാമാറ്റിക്ക് വിധത്തിൽ Whisper മോഡൽ ഡൗൺലോഡ് ചെയ്ത് ലോഡ് ചെയ്ത്, ഓഡിയോ പ്രാദേശിക OpenAI-ഉം പൊരുത്തപ്പെടുന്ന എന്റ്പോയിന്റ് ലേക്ക് ട്രാൻസ്ക്രിപ്ഷൻ നടത്താൻ അയയ്ക്കുന്നു. Whisper മോഡൽ (`whisper`) Foundry Local കാറ്റലോഗിൽ ലിസ്റ്റുചെയ്‌തിരിക്കുന്നു, ഇത് പൂർണ്ണമായും ഡിവൈസിൽ ഓടുന്നു - ക്ലൗഡ് API കീകൾ അല്ലെങ്കിൽ നെറ്റ്വർക്ക് ആക്‌സസ് ആവശ്യമില്ല.

---

### ഭാഗം 10: കസ്റ്റം അല്ലെങ്കിൽ Hugging Face മോഡലുകൾ ഉപയോഗിക്കൽ

**ലാബ് ഗൈഡ്:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAI മോഡൽ ബിൽഡർ ഉപയോഗിച്ച് Hugging Face മോഡലുകൾ ഓപ്റ്റിമൈസ്ഡ് ONNX формат-ലേക്ക് കമ്പൈൽ ചെയ്യൽ
- ഹാർഡ്‌വെയർ-വിശിഷ്ടം കമ്പൈലേഷൻ (CPU, NVIDIA GPU, DirectML, WebGPU) & ക്വാണ്ടൈസേഷൻ (int4, fp16, bf16)
- Foundry Local-ക്ക് ചാറ്റ്-ടെംപ്ലേറ്റ് കോൺഫിഗറേഷൻ ഫയലുകൾ ഉണ്ടാക്കുക
- കംപൈൽ ചെയ്‌ത മോഡലുകൾ Foundry Local കാഷെയിലേക്കിടുക
- CLI, REST API, OpenAI SDK വഴി കസ്റ്റം മോഡലുകൾ ഓടിക്കുക
- ഉദാഹരണം: Qwen/Qwen3-0.6B തുടക്കമുഴുവനായുള്ള കമ്പൈലേഷൻ റഫറൻസ്

---

### ഭാഗം 11: പ്രാദേശിക മോഡലുകളിൽ ടൂൾ കോളിംഗ്

**ലാബ് ഗൈഡ്:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- പ്രാദേശിക മോഡലുകൾ പുറം ഫംഗ്ഷനുകൾ (ടൂൾ/ഫംഗ്ഷൻ കോളിംഗ്) വിളിക്കാൻ സാധ്യമാക്കുക
- OpenAI ഫംഗ്ഷൻ-കോളിംഗ് ഫോർമാറ്റിൽ ടൂൾ സ്കീമകൾ നിർവ്വചിക്കുക
- മൾട്ടി-ടേൺ ടൂൾ-കോളിംഗ് സംഭാഷണ പ്രവാഹം കൈകാര്യം ചെയ്യുക
- ടൂൾ കോൾ പ്രാദേശികമായി എക്സിക്യൂട്ട് ചെയ്ത് ഫലം മോഡലിന് മടക്കുക
- ടൂൾ-കോളിംഗ് രംഗങ്ങൾക്ക് അനുയോജ്യമായ മോഡലുകൾ (Qwen 2.5, Phi-4-mini) തിരഞ്ഞെടുക്കുക
- SDK-യുടെ പ്രസക്തമായ `ChatClient` ടൂൾ കോളിംഗിനായി ഉപയോഗിക്കുക (JavaScript)

**കോഡ് ഉദാഹരണങ്ങൾ:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | കാലാവസ്ഥാ/ജനസംഖ്യ ടൂളുകളുമായി ടൂൾ കോളിംഗ് |
| C# | `csharp/ToolCalling.cs` | .NET ഉപയോഗിച്ച് ടൂൾ കോളിംഗ് |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ഉപയോഗിച്ച് ടൂൾ കോളിംഗ് |

---

### ഭാഗം 12: Zava Creative Writer-ന് വെബ് UI നിർമ്മിക്കൽ

**ലാബ് ഗൈഡ്:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer-ന് ബ്രൗസർ അടിസ്ഥാനമാക്കിയ ഫ്രണ്ട് എൻഡ് ചേർക്കുക
- Python (FastAPI), JavaScript (Node.js HTTP), C# (ASP.NET Core) വഴി പങ്കുവെച്ച UI സർവ് ചെയ്യുക
- ബ്രൗസറിൽ Fetch API-യും ReadableStream-ഉം ഉപയോഗിച്ച് സ്ട്രീമിംഗ് NDJSON ഉപഭോഗിക്കുക
- ലൈവ് ഏജന്റ് സ്റ്റാറ്റസ് ബേജുകൾ, യഥാർത്ഥ സമയ ലേഖന ഗ്രന്ഥം സ്ട്രീമിംഗ്

**കോഡ് (പങ്കുവെച്ച UI):**

| ഫയൽ | വിവരണം |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | പേജിന്റെ ലേയ്ഔട്ട് |
| `zava-creative-writer-local/ui/style.css` | സ്റ്റൈലിംഗ് |
| `zava-creative-writer-local/ui/app.js` | സ്ട്രീം റീഡർ, DOM അപ്ഡേറ്റ് ലాజിക്ക് |

**ബാക്ക്‌എൻഡ് കൂട്ടിച്ചേർക്കലുകൾ:**

| ഭാഷ | ഫയൽ | വിവരണം |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | സ്റ്റാറ്റിക് UI സർവ്വ് ചെയ്യുന്നവക്ക് അപ്ഡേറ്റ് ചെയ്തു |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ഓർക്കസ്ട്രേറ്റർ വിസ്താരമുള്ള പുതിയ HTTP സർവർ |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | പുതിയ ASP.NET(Core) മിനിമൽ API പ്രോജക്റ്റ് |

---

### ഭാഗം 13: വർക്ഷോപ് പൂർത്തിയായി
**ലാബ് ഗൈഡ്:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- നിങ്ങൾ നിർമ്മിച്ചതിലെ 12 ഭാഗങ്ങളിലുടനീളം എല്ലാം സംഗ്രഹം
- നിങ്ങളുടെ അപ്ലിക്കേഷനുകൾ ഉയർത്താനുള്ള കൂടുതൽ ആശയങ്ങൾ
- റിസോഴ്‌സുകളും ഡോക്യുമെന്റേഷനുംക് ലിങ്കുകൾ

---

## പ്രൊജക്ട് ഘടന

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

## റിസോഴ്‌സുകൾ

| റിസോഴ്‌സ് | ലിങ്ക് |
|----------|------|
| ഫൗണ്ട്രി ലോക്കൽ വെബ്‌സൈറ്റ് | [foundrylocal.ai](https://foundrylocal.ai) |
| മോഡൽ കാറ്റലോഗ് | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| ഫൗണ്ട്രി ലോക്കൽ ഗിറ്റ്ഹബ് | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| ആരംഭിക്കുക ഗൈഡ് | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| ഫൗണ്ട്രി ലോക്കൽ SDK റഫറൻസ് | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft ഏജന്റ് ഫ്രെയിംവർക്ക് | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| ഓപ്പൺഎഐ വിത്സ്പർ | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX റൺടൈം GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## ലൈസൻസ്

ഈ വർക്ക്‌ഷോപ്പ് മെറ്റീരിയൽ വിദ്യാഭ്യാസ ഉദ്ദേശ്യങ്ങളക്കായാണ് നൽകുന്നത്.

---

**നല്ല നിർമ്മാണം! 🚀**