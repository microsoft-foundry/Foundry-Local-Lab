![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ഭാഗം 13: വേർക്ക്ഷോപ്പ് പൂര്‍ത്തിയായി

> **അഭിനന്ദനങ്ങൾ!** നിങ്ങൾ മുഴുവൻ Foundry Local വേർക്ക്ഷോപ്പ് പൂർത്തിയാക്കിയിരിക്കുന്നു.

CLI ഇൻസ്റ്റാൾ ചെയ്യുന്നതിൽ നിന്ന് ചാറ്റ് ആപ്ലിക്കേഷനുകൾ നിർമ്മിക്കുന്നതുവരെ, RAG പൈപ്പ്‌ലൈൻ, മൾട്ടി-ഏജന്റ് സിസ്റ്റങ്ങൾ, സ്പീച്ച്-ടു-ടെകസ്റ്റ് ട്രാന്‍സ്ക്രിപ്ഷൻ, നിങ്ങളുടെ സ്വന്തം കസ്റ്റം മോഡലുകൾ കോമ്പൈൽ ചെയ്യൽ, ടൂൾ കോളിംഗ് സജ്ജമാക്കൽ, മൾട്ടി-ഏജന്റ് ആപ്ലിക്കേഷനു വേണ്ടി ലൈവ് വെബ് UI വയറിൽ കണക്ട് ചെയ്യൽ എന്നിവയെല്ലാം നിങ്ങൾ നേടിക്കഴിഞ്ഞു. ഇതെല്ലാം നിങ്ങളുടെ ഉപകരണത്തിൽ പൂർണ്ണമായും പ്രവർത്തിക്കുന്നു.

---

## നിങ്ങൾ നിർമ്മിച്ചതു

| ഭാഗം | നിങ്ങൾ നിർമ്മിച്ചതെന്ത്  |
|------|---------------------|
| 1 | Foundry Local ഇൻസ്റ്റാൾ ചെയ്ത് CLI വഴി മോഡലുകൾ അറിഞ്ഞു |
| 2 | സർവ്വീസ്, കാറ്റലോഗ്, കാഷെ, മോഡൽ മാനേജ്മെന്റിനായി Foundry Local SDK API മാസ്റ്റർ ചെയ്തു |
| 3 | Python, JavaScript, C# എന്നിവയിൽ SDK ഉപയോഗിച്ചും OpenAI-യുമായി കണക്ട് ചെയ്തു |
| 4 | ലോക്കൽ അറിവ് ഉപയോഗിച്ച് RAG പൈപ്പ്‌ലൈൻ നിർമ്മിച്ചു |
| 5 | പേഴ്സോണകൾ ഉള്ള AI ഏജന്റുകളും ഘടിത ഔട്ട്പുട്ട് ഉള്ളവയും സൃഷ്ടിച്ചു |
| 6 | ഫീഡ്ബാക്ക് ലൂപ്പുകൾ സഹിതം മൾട്ടി-ഏജന്റ് പൈപ്പ്‌ലൈൻ ഓർക്കസ്ട്രേറ്റ് ചെയ്തു |
| 7 | പ്രൊഡക്ഷൻ ക്യാപ്സ്ടോൺ ആപ്ലിക്കേഷൻ ആയ Zava Creative Writer പരിശോധിച്ചു |
| 8 | ഏജന്റുകൾക്കായി മൂല്യാഹരണ നയിച്ച ഡെവലപ്പ്മെന്റ് വർക്ക്ഫ്ലോകൾ നിർമ്മിച്ചു |
| 9 | Whisper ഉപയോഗിച്ച് ഓഡിയോ ട്രാൻസ്ക്ക്രൈബ് ചെയ്തു, ഡിവൈസിൽ സ്പീച്ച് ടു ടെകസ്റ്റ് ചെയ്തു |
| 10 | ONNX Runtime GenAI ഉപയോഗിച്ച് കസ്റ്റം Hugging Face മോഡൽ കോമ്പൈൽ ചെയ്ത് പ്രവർത്തിപ്പിച്ചു |
| 11 | ടൂൾ കോളിംഗ് ഉപയോഗിച്ച് ലോക്കൽ മോഡലുകൾക്ക് ബാഹ്യ ഫังก്ഷനുകൾ കോളുചെയ്യാൻ അനുവദിച്ചു |
| 12 | Zava Creative Writer നു വേണ്ടി ബ്രൗസർ അടിസ്ഥാനമാക്കിയ UI റിയൽ ടൈം സ്ട്രീമിങോടെ നിർമ്മിച്ചു |

---

## മുന്നോട്ടുള്ള ആശയങ്ങൾ

- ടൂൾ കോളിംഗ് മൾട്ടി-ഏജന്റുകളുമായ് സംയോജിപ്പിച്ച് സ്വയം പ്രവർത്തിക്കുന്ന വർക്ക്ഫ്ലോകൾ നിർമ്മിക്കുക
- നിങ്ങളുടെ ടൂളുകളിൽ നിന്ന് ഒരു ലോക്കൽ ഡാറ്റാബേസ് ചോദിക്കുക അല്ലെങ്കിൽ ഇന്റേണൽ REST API കളി ചെയ്യുക
- വിവിധ മോഡലുകൾ (`phi-4-mini`, `deepseek-r1-7b`) പരീക്ഷിച്ച് ഗുണനിലവാരവും വേഗവും താരതമ്യം ചെയ്യുക
- Zava Writer വെബ് സെർവറിന് പ്രാമാണീകരണം അല്ലെങ്കിൽ റേറ്റ് ലിമിറ്റിംഗ് ചേർക്കുക
- നിങ്ങൾക്ക് പ്രാധാന്യമുള്ള ഡൊമൈൻ സൃഷ്ടിക്കുന്നതിന് സ്വന്തം മൾട്ടി-ഏജന്റ് ആപ്ലിക്കേഷൻ നിർമ്മിക്കുക
- Foundry Local ന്റെ പകരം Azure AI Foundry ഉപയോഗിച്ച് ക്ലൗഡിലേക്ക് ഡിപ്ലോയ് ചെയ്യുക, കോഡ് ഒരുപോലെ, എൻഡ്‌പോയിന്റ് വ്യത്യസ്തം

---

## വിഭവങ്ങൾ

| വിഭവം | ലിങ്ക് |
|----------|------|
| Foundry Local വെബ്സൈറ്റ് | [foundrylocal.ai](https://foundrylocal.ai) |
| മോഡൽ കാറ്റലോഗ് | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| ആരംഭിക്കാനുള്ള ഗൈഡ് | [Microsoft Learn: Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK റഫറൻസ് | [Microsoft Learn: SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent ഫ്രെയിംവർക്ക് | [Microsoft Learn: Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

നിങ്ങൾ പഠിച്ചവകൾ പരിശോധിക്കാനും കൂടുതൽ വായനാ വിഭവങ്ങൾ പരിശോധിക്കാനും [workshop overview](../README.md) എന്നിടത്തു മടങ്ങിയെത്തുക.

---

[← ഭാഗം 12: Zava Creative Writer നു വേണ്ടി വെബ് UI നിർമ്മിക്കൽ](part12-zava-ui.md) | [വേർക്ക്ഷോപ്പ് ഹോംബാക്ക്](../README.md)