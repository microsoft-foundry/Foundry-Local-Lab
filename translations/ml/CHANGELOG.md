# മാറ്റങ്ങൾ — ഫൗണ്ടറി ലോക്കൽ വർക്ക്‌ഷോപ്പ്

ഈ വർക്ക്‌ഷോപ്പിലെ എല്ലാ ശ്രദ്ധേയമായ മാറ്റങ്ങളും താഴെ രേഖപ്പെടുത്തിയിരിക്കുന്നു.

---

## 2026-03-11 — ഫോക്ക്സ് 12 & 13, വെബ് UI, വിസ്പർ റിറൈറ്റ്, WinML/QNN ഫിക്സ്, വാലിഡേഷൻ

### ചേർക്കുന്നു
- **ഫോക്ക്സ് 12: സാവ ക്രിയേറ്റീവ് റൈറ്ററിന് വെബ് UI നിർമ്മിക്കുക** — പുതിയ ലാബ് ഗൈഡ് (`labs/part12-zava-ui.md`) ഇതിനോട് അനുബന്ധിച്ചുള്ള സ്ട്രീമിംഗ് NDJSON, ബ്രൗസർ `ReadableStream`, ലൈവ് ഏജന്റ് സ്റ്റാറ്റസ് ബാഡ്ജുകൾ, റിയൽ ടൈം ആർട്ടിക്കിൾ ടെക്സ്റ്റ് സ്ട്രീമിംഗ് ഉൾപ്പെടെയുള്ള വ്യായാമങ്ങൾ ഉൾക്കൊള്ളുന്നു
- **ഫോക്ക്സ് 13: വർക്ക്‌ഷോപ്പ് പൂർണ്ണം** — എല്ലാ 12 ഭാഗങ്ങളുടെയും സംഗ്രഹ ലാബ് (`labs/part13-workshop-complete.md`) കൂടാതെ അടുത്ത ആശയങ്ങൾ, വിഭവ ബന്ധങ്ങൾ
- **സാവ UI ഫ്രണ്ട് എൻഡ്:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — മൂന്ന് ബാക്ക് എൻറുകളിൽ നിന്നും ഉപയോഗിക്കുന്ന വാനില്ല HTML/CSS/JS ബ്രൗസർ ഇൻറർഫേസ്
- **ജാവാസ്ക്ക്രിപ്‌റ്റ് HTTP സെർവർ:** `zava-creative-writer-local/src/javascript/server.mjs` — ബ്രൗസർ ആക്സസിനായി ഓർക്കസ്ട്രേറ്ററെ പൊതിയുന്ന പുതിയ എക്സ്പ്രസ് രീതിയിലുള്ള HTTP സെർവർ
- **C# ASP.NET കോർ ബാക്ക്‌എൻഡ്:** `zava-creative-writer-local/src/csharp-web/Program.cs` & `ZavaCreativeWriterWeb.csproj` — UIയും സ്ട്രീമിംഗ് NDJSON-ഉം സർവ്വിസ് ചെയ്യുന്ന പുതിയ മിനിമൽ API പ്രോജക്ട്
- **ഓഡിയോ സാമ്പിൾ ജനറേറ്റർ:** `samples/audio/generate_samples.py` — `pyttsx3` ഉപയോഗിച്ച് ഓഫ്ലൈൻ TTS സ്ക്രിപ്റ്റ്, ഫോക്ക്സ് 9-നായി സാവ തീം ചെയ്ത WAV ഫയലുകൾ സൃഷ്ടിക്കുന്നു
- **ഓഡിയോ സാമ്പിൾ:** `samples/audio/zava-full-project-walkthrough.wav` — ട്രാൻസ്ക്രിപ്ഷൻ ടെസ്റ്റിനായി പുതിയ ദീർഘ ഓഡിയോ സാമ്പിൾ
- **വാലിഡേഷൻ സ്ക്രിപ്റ്റ്:** `validate-npu-workaround.ps1` — എല്ലാ C# സാമ്പിളുകളിലും NPU/QNN വർക്അറൗണ്ട് സ്ഥിരീകരിക്കുന്ന ഓട്ടോമേറ്റഡ് പവർ‌ഷെൽ സ്ക്രിപ്റ്റ്
- **മേർമേയ്ഡ് ഡയഗ്രാം SVGകൾ:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML ക്രോസ്-പ്ലാറ്റ്ഫോം പിന്തുണ:** എല്ലാ മൂന്ന് C# `.csproj` ഫയലുകളും (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ഇപ്പോൾ നിബന്ധനയുള്ള TFM ഉപയോഗിക്കുകയും പരസ്പരം അസഹിഷ്ണുതയുള്ള പാക്കേജ് റഫറൻസുകൾക്കായി ക്രോസ്-പ്ലാറ്റ്ഫോം പിന്തുണ നൽകുന്നതായി മാറി. വിൻഡोजിൽ: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (QNN EP പ്ലഗിൻ ഉൾപ്പെടുന്ന സൂപർസെറ്റ്). വിൻഡോസ് അല്ലാത്തവയിൽ: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (ബേസ് SDK). സാവ പ്രോജക്ടുകളിൽ പൊരുത്തപ്പെടുത്തിയ `win-arm64` RID ആവർത്തനശൂന്യമാക്കി. നേറ്റീവ് ആസറ്റുകൾ ഒഴിവാക്കുന്ന ട്രാന്‍സിറ്റിവ് ഡിപ്പെൻഡൻസി വർക്അറൗണ്ട് മൊഡ്യൂൾ `Microsoft.ML.OnnxRuntime.Gpu.Linux`-ൽ നിന്നും വിജയിക്കാതെ പോയ `win-arm64` റഫറൻസ് കാരണം ചേർത്തിരിക്കുന്നു. മുൻപ് ഉണ്ടാക്കിയ try/catch NPU വർ്ക്അറൗണ്ട് എല്ലാ 7 C# ഫയലുകളിലും നിന്ന് നീക്കം ചെയ്യപ്പെട്ടു.

### മാറ്റം വരുത്തിയത്
- **ഫോക്ക്സ് 9 (Whisper):** പ്രധാന റിറൈറ്റ് — ജാവാസ്ക്രിപ്റ്റ് ഇനി SDKയുടെ ഉൾനാട്ട `AudioClient` (`model.createAudioClient()`) ഉപയോഗിച്ച് ONNX റൺടൈം നിർദേശനത്തെ במקום; JS/C# `AudioClient` സമീപനം python ONNX റൺടൈം സമീപതലിനൊപ്പം അളക്കുന്നത് പ്രതിഫലിപ്പിക്കുന്ന ശില്പം വിവരണം, താരതമ്യ പട്ടികകൾ, പൈപ്പ്‌ലൈൻ ഡയഗ്രാമുകൾ പുതുക്കിയിരിക്കുന്നു
- **ഫോക്ക്സ് 11:** നാവിഗേഷന്‍ ലിങ്കുകൾ പുതുക്കിയത് (ഇപ്പോൾ ഫോക്ക്സ് 12-ൽ പോയുന്നു); ടൂൾ കോളിംഗ് ഫ്ലോയും സീക്വൻസും രേഖപ്പെടുത്തുന്ന SVG ഡയഗ്രാമുകൾ ചേർത്തിരിക്കുന്നു
- **ഫോക്ക്സ് 10:** നാവിഗേഷൻ ഇപ്പോൾ വേർക്ക്‌ഷോപ് അവസാനിപ്പിക്കാതെ ഫോക്സ് 12 വഴി മാർഗ്ഗനിർദ്ദേശം ചെയ്യുന്നു
- **Python Whisper (`foundry-local-whisper.py`):** അധിക ഓഡിയോ സാമ്പിളുകൾ ചേർത്തും തെറ്റ് കൈകാര്യം മെച്ചപ്പെടുത്തിയും വിപുലീകരിച്ചത്
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** `model.createAudioClient()` ഉപയോഗിച്ച് `audioClient.transcribe()` പിന്തുടർന്ന് ഓൺ‌എൻ‌എക്സ് റൺടൈം സെഷനുകളുടെ മാൻവൽ മാറ്റവും
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** API യുമായി Static UI ഫയലുകളും സർവ്വ് ചെയ്യുന്നു
- **Zava C# കൺസോൾ (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU വർക്അറൗണ്ട് നീക്കം ചെയ്തു (ഇപ്പോൾ WinML പാക്കേജ് കൈകാര്യം ചെയ്യുന്നു)
- **README.md:** കോഡ് സാമ്പിൾ പട്ടികകളും ബാക്ക്‌എൻഡ് ചേർക്കലുകളും ഉൾപ്പെടുന്ന ഫോക്ക്സ് 12 സെക്ഷൻ ചേർത്തു; ഫോക്ക്സ് 13 സെക്ഷൻ ചേർത്തു; പഠനലക്ഷ്യങ്ങളും പ്രോജക്ട് ഘടനയും അപ്ഡേറ്റുചെയ്തു
- **KNOWN-ISSUES.md:** പരിഹരിച്ച പ്രശ്നം #7 (C# SDK NPU മോഡൽ വേരിയന്റ് — ഇപ്പോൾ WinML പാക്കേജ് കൈകാര്യം ചെയ്യുന്നു) നീക്കം ചെയ്തു. ശേഷിക്കുന്ന പ്രശ്നങ്ങൾക്ക് #1–#6 എന്ന വൻപട്ടിക നൽകി. .NET SDK 10.0.104 ഉപയോഗിച്ച് ഇൻവയേണ്മെന്റ് വിശദാംശങ്ങൾ പുതുക്കി
- **AGENTS.md:** പുതിയ `zava-creative-writer-local` എൻട്രികൾ (`ui/`, `csharp-web/`, `server.mjs`) ഉൾപ്പെടുത്തി പ്രോജക്ട് ത്രി അപ്ഡേറ്റ് ചെയ്തു; C# കീ-പാക്കേജുകളും നിബന്ധനയുള്ള TFM വിശദാംശങ്ങൾ പുതുക്കി
- **labs/part2-foundry-local-sdk.md:** പൂർണ ക്രോസ്-പ്ലാറ്റ്ഫോം മാതൃക കാണിക്കുന്ന `.csproj` ഉദാഹരണവും നിബന്ധനയുള്ള TFM, പരസ്പരം അസഹിഷ്ണുതയുള്ള പാക്കേജ് റഫറൻസുകളും വിശദീകരണ കുറിപ്പും പുതുക്കി

### സ്ഥിരീകരിച്ചു
- മുകളും 3 C# പ്രോജക്ടുകളും (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) വിൻഡോസ് ARM64-ൽ വിജയകരമായി ബിൽഡ് ചെയ്യുന്നു
- ചാറ്റ് സാമ്പിൾ (`dotnet run chat`): മോഡൽ `phi-3.5-mini-instruct-qnn-npu:1` ആയി WinML/QNN വഴി ലോഡ് ചെയ്യുന്നു — NPU വേരിയന്റ് സി.പി.യു പിന്തുടർച്ച കൂടാതെ നേരിട്ട് ലോഡ് ചെയ്യുന്നു
- ഏജന്റ് സാമ്പിൾ (`dotnet run agent`): മൾട്ടി-ടേൺ സംവാദത്തോടെ തുടർച്ചയായി പ്രവർത്തിക്കുന്നു, എക്സിറ്റ് കോഡ് 0
- ഫൗണ്ടറി ലോക്കൽ CLI v0.8.117 കാണിക്കുകയും SDK v0.9.0 .NET SDK 9.0.312-ൽ പ്രവർത്തിക്കുകയും ചെയ്യുന്നു

---

## 2026-03-11 — കോഡ് ഫിക്സുകൾ, മോഡൽ ക്ലീൻ-അപ്പ്, മേർമേയ്ഡ് ഡയഗ്രാമുകൾ, വാലിഡേഷൻ

### പരിഹരിച്ചു
- **എല്ലാ 21 കോഡ് സാമ്പിളുകളും (7 Python, 7 JavaScript, 7 C#):** `model.unload()` / `unload_model()` / `model.UnloadAsync()` എക്സിറ്റ് സമയത്ത് ചേർത്ത് OGA മെമ്മറി ലീക്ക് മുന്നറിയിപ്പുകൾ (ക്നൗൺ ഇഷ്യൂ #4) പരിഹരിച്ചു
- **csharp/WhisperTranscription.cs:** ഭംഗിയില്ലാത്ത `AppContext.BaseDirectory` റിലേറ്റീവ് പാത ഒഴിവാക്കി `FindSamplesDirectory()` ഉപയോഗിച്ച് `samples/audio` ചുരുങ്ങാതെ കണ്ടെത്തുന്നു (ക്നൗൺ ഇഷ്യൂ #7)
- **csharp/csharp.csproj:** ഹാർഡ്‌കോഡ് `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` മറന്നു ഡോട്ട് നെറ്റ് പ്ളാറ്റ്ഫോം ലളിതീകരിക്കാൻ `$(NETCoreSdkRuntimeIdentifier)` ഉപയോഗിച്ച് ഓട്ടോ കണ്ടെത്തൽ ഫാൾബാക്ക് ഉൾപ്പെടുത്തി ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### മാറ്റം വരുത്തിയതു
- **ഫോക്ക്സ് 8:** അസ്കി ബോക്സ് ഡയഗ്രാം `eval`-ഡ്രിവൻ ഇറ്ററേഷൻ ലൂപ്പ് SVG ചിത്രമായി മാറ്റി
- **ഫോക്ക്സ് 10:** അസ്കി അരോകൾ ഉപയോഗടെ കണ്ടംപിലേഷൻ പൈപ്പ്‌ലൈൻ ഡയഗ്രാം SVG-യാക്കി
- **ഫോക്ക്സ് 11:** ടൂൾ കോളിംഗ് ഫ്ലോയും സീക്വൻസും SVG ചിത്രമായി മാറ്റി
- **ഫോക്ക്സ് 10:** "വർക്ക്‌ഷോപ്പ് പൂർണ്ണം!" സെക്ഷൻ ഫോക്ക്സ് 11 (അവസാന ലാബ്) ലേക്ക് മാറ്റി; പകരം "അടുത്ത ചുവടുകൾ" ലിങ്ക് ചേർത്തു
- **KNOWN-ISSUES.md:** CLI v0.8.117-നോട് താരതമ്യപ്പെടുത്തി എല്ലാ ഇഷ്യൂകളും പൂർണ്ണം പുനഃസാധുവാക്കിയിരിക്കുന്നു. പരിഹരിച്ചവ നീക്കം: OGA മെമ്മറി ലീക്ക് (ക്ലീൻഅപ് ചേർത്തു), വിസ്പർ പാത (FindSamplesDirectory), HTTP 500 നീണ്ടുനിൽക്കുന്ന ഇൻഫറൻസ് (പുനരാരംഭിക്കാനാകാത്തത്, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice നിയന്ത്രണങ്ങൾ (ഇപ്പോൾ `"required"` ഉം പ്രത്യേക ഫംഗ്ഷൻ ലക്ഷ്യമിടലും qwen2.5-0.5bയിൽ പ്രവർത്തിക്കുന്നു). ജെഎസ് വിസ്പർ മറ്റ് പ്രശ്‌നം — എല്ലാ ഫയലുകളും ഒഴിവു / ബൈനറി ഔട്ട്‌പുട്ട് തിരികെ നൽകുന്നു (v0.9.x മുതൽ പിന്‍വാങ്ങൽ, ഗുരുതരത്വം പ്രധാനമായി ഉയർത്തി). #4 C# RID ഓട്ടോ കണ്ടെത്തൽ വർക്ക് അറൗണ്ടും [#497](https://github.com/microsoft/Foundry-Local/issues/497) ലിങ്കും പുതുക്കി. 7 തുറന്ന പ്രശ്‌നങ്ങൾ തുടരുന്നു
- **javascript/foundry-local-whisper.mjs:** ക്ലീൻഅപ് വേരിയബിൾ നാമം തിരുത്തി (`whisperModel` → `model`)

### സ്ഥിരീകരിച്ചു
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — ക്ലീൻഅപോടെ വിജയം
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — ക്ലീൻഅപോടെ വിജയം
- C#: `dotnet build` 0 മുന്നറിയിപ്പുകളും 0 പിശകുകളുമായി വിജയകരം (net9.0 ലക്ഷ്യം)
- എല്ലാ 7 Python ഫയലുകളും `py_compile` সিন്ടാക്സ് പരിശോധനയുടെ വഴി കടന്നുവെന്ന്
- എല്ലാ 7 JavaScript ഫയലുകളും `node --check` വാലിഡേഷൻ കടന്നുവെന്ന്

---

## 2026-03-10 — ഫോക്ക്സ് 11: ടൂൾ കോളിംഗ്, SDK API വിപുലീകരണം, മോഡൽ കവറേജ്

### ചേർക്കുന്നു
- **ഫോക്ക്സ് 11: ലോക്കൽ മോഡലുകളുമായി ടൂൾ കോളിംഗ്** — 8 വ്യായാമങ്ങളുള്ള പുതിയ ലാബ് ഗൈഡ് (`labs/part11-tool-calling.md`), ടൂൾ സ്കീമകൾ, മൾട്ടി-ടേൺ ഫ്ലോ, മൾട്ടിപ്പിൾ ടൂൾ കോളുകൾ, കസ്റ്റം ടൂളുകൾ, ChatClient ടൂൾ കോളിംഗ്, `tool_choice` ഉൾപ്പെടുന്നു
- **Python സാമ്പിൾ:** `python/foundry-local-tool-calling.py` — OpenAI SDK ഉപയോഗിച്ച് `get_weather` / `get_population` ടൂളുകൾ ഉപയോഗിച്ച് ടൂൾ കോളിംഗ്
- **ജാവാസ്ക്രിപ്റ്റ് സാമ്പിൾ:** `javascript/foundry-local-tool-calling.mjs` — SDKയുടെ നെയിറ്റീവ് `ChatClient` (`model.createChatClient()`) ഉപയോഗിച്ച് ടൂളുകൾ കോളിംഗ്
- **C# സാമ്പിൾ:** `csharp/ToolCalling.cs` — OpenAI C# SDK ഉപയോഗിച്ച് `ChatTool.CreateFunctionTool()` ഉപയോഗിച്ച് ടൂൾ കോളിംഗ്
- **ഫോക്ക്സ് 2, വ്യായാമം 7:** നൈറ്റീവ് `ChatClient` — `model.createChatClient()` (JS) & `model.GetChatClientAsync()` (C#) OpenAI SDKയ്ക്ക് ബദൽ
- **ഫോക്ക്സ് 2, വ്യായാമം 8:** മോഡൽ വകഭേദങ്ങളും ഹാർഡ്‌വെയർ തിരഞ്ഞെടുപ്പും — `selectVariant()`, `variants`, NPU വേരിയന്റ് പട്ടിക (7 മോഡലുകൾ)
- **ഫോക്ക്സ് 2, വ്യായാമം 9:** മോഡൽ അപ്ഗ്രേഡുകളും കാറ്റലോഗ് പുതുക്കലും — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **ഫോക്ക്സ് 2, വ്യായാമം 10:** റീസണിംഗ് മോഡലുകൾ — `<think>` ടാഗ് പാഴ്സിങ് ഉദാഹരണങ്ങളോടെ `phi-4-mini-reasoning`
- **ഫോക്ക്സ് 3, വ്യായാമം 4:** OpenAI SDK-യ്ക്ക് ബദലായി `createChatClient`, സ്ട്രീമിംഗ് കോൾബാക്ക് പാറ്റേൺ ഡോക്യുമെൻറേഷൻ
- **AGENTS.md:** ടൂൾ കോളിംഗ്, ChatClient, റീസണിംഗ് മോഡലുകളുടെ കോഡിംഗ് ശീലങ്ങൾ ചേർത്തു

### മാറ്റം വരുത്തിയത്
- **ഫോക്ക്സ് 1:** മോഡൽ കാറ്റലോഗ് വിപുലീകരിച്ചു — phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo ഉൾപ്പെടുത്തി
- **ഫോക്ക്സ് 2:** API റഫറൻസ് പട്ടികകൾ വിപുലീകരിച്ചു — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **ഫോക്ക്സ് 2:** വ്യായാമം 7-9 നമ്പറുകൾ 10-13 ആയി മാറ്റി പുതിയ വ്യായാമങ്ങൾ ഉൾപ്പെടുത്താനായി
- **ഫോക്ക്സ് 3:** പ്രധാനപ്പെട്ട ടേബിൾ അപ്ഡേറ്റ് ചെയ്തു - നൈറ്റീവ് ChatClient ഉൾപ്പെടുത്തി
- **README.md:** കോഡ് സാമ്പിൾ പട്ടികയോടുകൂടിയ ഫോക്ക്സ് 11 സെക്ഷൻ ചേർത്തു; പഠനലക്ഷ്യം #11 ചേർത്തു; പ്രോജക്ട് ഘടന ട്രി പുതുക്കി
- **csharp/Program.cs:** CLI റൂട്ടറിന് `toolcall` കേസ് ചേർത്തു; സഹായക്കുറിപ്പ് പുതുക്കി

---

## 2026-03-09 — SDK v0.9.0 അപ്‌ഡേറ്റ്, ബ്രിട്ടീഷ് ഇംഗ്ലീഷ്, വാലിഡേഷൻ

### മാറ്റം വരുത്തിയതു
- **എല്ലാ കോഡ് സാമ്പിളുകൾ (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API-യിലേക്കുള്ള അപ്ഡേറ്റ് — `await catalog.getModel()`-ലെ `await` നഷ്ടമായിരുന്നു ശരിയാക്കി, `FoundryLocalManager` ഇൻ ഷെങ്കരണ മാതൃകകൾ പുതുക്കി, എൻഡ്‌പോയിൻറ് കണ്ടെത്തൽ സുതാര്യമായി
- **എല്ലാ ലാബ് ഗൈഡുകളും (ഫോക്ക്സ് 1-10):** ബ്രിട്ടീഷ് ഇംഗ്ലീഷിലേക്ക് മാറ്റം (colour, catalogue, optimised തുടങ്ങിയവ)
- **എല്ലാ ലാബ് ഗൈഡുകൾ:** SDK കോഡ് ഉദാഹരണങ്ങൾ v0.9.0 API അനുസരിച്ച് പുതുക്കി
- **എല്ലാ ലാബ് ഗൈഡുകൾ:** API റഫറൻസ് പട്ടികകളും വ്യായാമ കോഡ് ബ്ലോകുകളും അപ്ഡേറ്റ് ചെയ്തിരിക്കുന്നു
- **ജാവാസ്ക്രിപ്റ്റ് തീവ്ര ഫിക്സ്:** `catalog.getModel()`-ൽ മറഞ്ഞ `await` ചേർത്തു — മോഡൽ ഒബ്ജക്റ്റ് മല്ലാതെ പ്രോമിസ് തിരികെ നൽകുന്നതു മൂലം നിർവൃതമായ പിഴവുകൾ ഉണ്ടായിരുന്നു

### സ്ഥിരീകരിച്ചു
- എല്ലാ Python സാമ്പിളുകളും വിജയകരമായി Foundry Local സർവീസിനൊപ്പം പ്രവർത്തിക്കുന്നു
- എല്ലാ ജാവാസ്ക്രിപ്റ്റ് സാമ്പിളുകളും വിജയകരമായി (Node.js 18+)
- C# പ്രോജക്ട് .NET 9.0-ൽ വിജയകരമായി ബിൽഡ് ചെയ്ത് പ്രവർത്തിക്കുന്നു (net8.0 SDK അസംബ്ലി മുതൽ ഫോർവേഡ്-കമ്പാറ്റ്)
- വർക്ക്ഷോപ്പിൽ 29 ഫയലുകൾ മാറ്റം വരുത്തി സ്ഥിരീകരിച്ചു

---

## ഫയൽ സൂചിക

| ഫയൽ | അവസാനമായി അപ്ഡേറ്റ് ചെയ്തത് | വിവരണം |
|------|-----------------------|---------|
| `labs/part1-getting-started.md` | 2026-03-10 | മോഡൽ കാറ്റലോഗ് വിപുലീകരിച്ചത് |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | പുതിയ വ്യായാമങ്ങൾ 7-10, API പട്ടികകൾ വിപുലീകരിച്ചു |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | പുതിയ വ്യായാമം 4 (ChatClient), ടേബിൾ അപ്ഡേറ്റ് ചെയ്തു |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + ബ്രിട്ടീഷ് ഇംഗ്ലീഷ് |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + ബ്രിട്ടീഷ് ഇംഗ്ലീഷ് |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid രേഖാചിത്രം |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid രേഖാചിത്രം, Workshop Complete മാറ്റി Part 11-ലേക്ക് |
| `labs/part11-tool-calling.md` | 2026-03-11 | പുതിയ ലാബ്, Mermaid രേഖാചിത്രങ്ങൾ, Workshop Complete വിഭാഗം |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | പുതിയത്: ടൂൾ കോളിംഗ് സാമ്പിൾ |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | പുതിയത്: ടൂൾ കോളിംഗ് സാമ്പിൾ |
| `csharp/ToolCalling.cs` | 2026-03-10 |新的: ടൂൾ കോളിംഗ് സാമ്പിൾ |
| `csharp/Program.cs` | 2026-03-10 | ചേർത്തു `toolcall` CLI കമാൻഡ് |
| `README.md` | 2026-03-10 | പാതി 11, പ്രോജക്ട് ഘടന |
| `AGENTS.md` | 2026-03-10 | ടൂൾ കോളിംഗ് + ChatClient సంప്രദായങ്ങൾ |
| `KNOWN-ISSUES.md` | 2026-03-11 | പരിഹരിച്ച Issue #7 നീക്കം ചെയ്തു, 6 തുറന്ന പ്രശ്നങ്ങൾ തുടരുന്നു |
| `csharp/csharp.csproj` | 2026-03-11 | ക്രോസ്-പ്ലാറ്റ്ഫോം TFM, WinML/base SDK കണ്ടീഷണൽ റഫറൻസുകൾ |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | ക്രോസ്-പ്ലാറ്റ്ഫോം TFM, ഓട്ടോ-ഡിറ്റക്റ്റ് RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | ക്രോസ്-പ്ലാറ്റ്ഫോം TFM, ഓട്ടോ-ഡിറ്റക്റ്റ് RID |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU ശ്രമം/ക്യാച്ച് തൊഴിലുറപ്പ് നീക്കം ചെയ്തു |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU ശ്രമം/ക്യാച്ച് തൊഴിലുറപ്പ് നീക്കം ചെയ്തു |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU ശ്രമം/ക്യാച്ച് തൊഴിലുറപ്പ് നീക്കം ചെയ്തു |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU ശ്രമം/ക്യാച്ച് തൊഴിലുറപ്പ് നീക്കം ചെയ്തു |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU ശ്രമം/ക്യാച്ച് തൊഴിലുറപ്പ് നീക്കം ചെയ്തു |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | ക്രോസ്-പ്ലാറ്റ്ഫോം .csproj ഉദാഹരണം |
| `AGENTS.md` | 2026-03-11 | അപ്‌ഡേറ്റ് ചെയ്ത C# പാക്കേജുകളും TFM വിവരങ്ങളും |
| `CHANGELOG.md` | 2026-03-11 | ഈ ഫയൽ |