# അറിയപ്പെട്ട പ്രശ്നങ്ങൾ — Foundry ലൊക്കൽ വർക്ഷോപ്പ്

Windows ഓടുന്ന **Snapdragon X Elite (ARM64)** ഉപകരണത്തിൽ Foundry Local SDK v0.9.0, CLI v0.8.117, .NET SDK 10.0 ഉപയോഗിച്ച് ഈ വർക്ഷോപ്പ് ബിൽഡ് ചെയ്ത് പരീക്ഷിക്കുമ്പോൾ ഉണ്ടായ പ്രശ്നങ്ങൾ.

> **സമീപകാല പരിശോധന:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ONNX Runtime സംഘടിപ്പിക്കാത്ത പ്രശ്നം

**സ്ഥിതി:** തുറന്നവ സ്ഥിതി
**ഗൗരവം:** മുന്നറിയിപ്പ് (തടസ്സമില്ലാത്തത്)
**ഘടകം:** ONNX Runtime / cpuinfo
**പുനരാവർത്തനം:** Snapdragon X Elite ഹാർഡ്‌വെയറിൽ ഓരോ Foundry Local സർവീസ് ആരംഭിക്കുന്നപ്പോഴും

Foundry Local സർവീസ് ആരംഭിക്കുമ്പോൾ ഓരോ തവണയും രണ്ട് മുന്നറിയിപ്പുകൾ ഉണ്ടാകുന്നു:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**പ്രഭാവം:** മുന്നറിയിപ്പുകൾ കാഴ്ച സൗന്ദര്യ സംബന്ധമായതും — inference ശരിയായി പ്രവർത്തിക്കുന്നു. എങ്കിലും, ബാഹ്യമായി പ്രവർത്തനത്തിൽ തെറ്റിദ്ധാരണ ഉണ്ടാക്കാവുന്നതാണ്. Qualcomm Oryon CPU കോറുകൾ തിരിച്ചറിയാൻ ONNX Runtime cpuinfo ലൈബ്രറിയെ അപ്ഡേറ്റ് ചെയ്യേണ്ടതുണ്ട്.

**പ്രതീക്ഷ:** Snapdragon X Elite പിന്തുണക്കുന്ന ARM64 CPU ആയി തിരിച്ചറിയപ്പെടണം, പിഴവ് തലത്തിലുള്ള സന്ദേശങ്ങൾ ഇല്ലാതെ.

---

## 2. SingleAgent ആദ്യ റൺ സമയ ശൂന്യരഫറൻസ് എക്സെപ്ഷൻ

**സ്ഥിതി:** തുറന്ന (അക്കസ്മികമായി)
**ഗൗരവം:** ആപത്ത് (ക്രാഷ്)
**ഘടകം:** Foundry Local C# SDK + Microsoft Agent Framework
**പുനരാവർത്തനം:** `dotnet run agent` റൺ ചെയ്യുക — മോഡൽ ലോഡ് കഴിഞ്ഞ് ഉടനെ ക്രാഷ്

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**പ്രസംഗം:** ലൈൻ 37-ൽ `model.IsCachedAsync(default)` വിളിക്കുന്നു. പുതിയ `foundry service stop` കഴിഞ്ഞ് ഏജന്റ് ആദ്യ റണിൽ മാത്രമാണ് ക്രാഷ് സംഭവിച്ചത്. പിന്നീട് ഒറ്റത്തവണ കോഡ് ഉപയോഗിച്ച് നടന്ന റണുകൾ വിജയിച്ചു.

**പരിണാമം:** ഇടക്കാലങ്ങളിൽ SDK സർവീസ് ആരംഭിക്കൽ അല്ലെങ്കിൽ കാറ്റലോഗ് ക്വറിയിൽ ഒരു റേസ് കൺഡിഷൻ ഉണ്ടാകാം എന്നാണ് സൂചന. `GetModelAsync()` സർവീസ് പൂർണ്ണമായി സജ്ജമാകുന്നതിനു മുൻപ് തന്നെ മടങ്ങി വയ്ക്കാം.

**പ്രതീക്ഷ:** `GetModelAsync()` സർവീസ് സജ്ജമാകുന്നത് വരെ തടഞ്ഞിരിക്കണം അല്ലെങ്കിൽ സർവീസ് ആരംഭിക്കൽ പൂർത്തിയാവാത്തപ്പോൾ വ്യക്തമാക്കിയ പിഴവ് സന്ദേശം നൽകണം.

---

## 3. C# SDK-യ്ക്ക് വ്യക്തമായ RuntimeIdentifier ആവശ്യമാണ്

**സ്ഥിതി:** തുറന്ന — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) ൽ ട്രാക്ക് ചെയ്യുന്നു
**ഗൗരവം:** ഡോക്യുമെന്റേഷൻ കുറവ്
**ഘടകം:** `Microsoft.AI.Foundry.Local` NuGet പാക്കേജ്
**പുനരാവർത്തനം:** `.csproj` ഫയലിൽ `<RuntimeIdentifier>` കൂടാതെ .NET 8+ പ്രോജക്ട് നിർമ്മിക്കുക

ബിൽഡ് പരാജയപ്പെടുന്നു:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**കാരണകാര്യം:** RID ആവശ്യകത പ്രതീക്ഷിക്കപ്പെടുന്നു — SDK നativa ബൈനറികൾ അടങ്ങിയതിനാൽ (P/Invoke `Microsoft.AI.Foundry.Local.Core`-ലും ONNX Runtime-ൽ), .NET പ്രത്യേകം പ്ലാറ്റ്ഫോം-സ്പെസിഫിക് ലൈബ്രറി കണ്ടെത്താൻ RID അറിയേണ്ടത് അനിവാര്യമാണ്.

MS Learn-ൽ ഇതിന്റെ ഡോക്യുമെന്റേഷൻ ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)) കാണിക്കുകയുമുണ്ട്:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

എങ്കിലും, ഓരോ തവണയും `-r` ഫ്ലാഗ് ഓർക്കേണ്ടതിനെന്താനും ഉപയോക്താക്കളെ എളുപ്പത്തിൽ മറക്കാനിടയുണ്ട്.

**പരിഹാരം:** `.csproj`-യിൽ ഓട്ടോ-ഡിറ്റക്റ്റ് ഫാൾബാക്ക് ചേർത്ത് `dotnet run` ഫോൾഗുകൾ കൂടാതെ പ്രവർത്തിപ്പിക്കുക:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` ഒരു യഥാർത്ഥ MSBuild പ്രോപ്പർട്ടി ആണ്, അതു ഹോസ്റ്റ് മെഷീൻ RID സ്വയം കണ്ടെത്തും. SDKയുടെ ടെസ്റ്റ് പ്രോജക്ടുകൾ ഇതിനകം ഈ രീതിയിലുള്ളവയാണ്. വ്യക്തമായ `-r` ഫ്ലാഗുകൾ നൽകിയാൽ അവക്ക് മാന്യമാണ്.

> **ടippിനോട്ട്:** വർക്ഷോപ്പ് `.csproj` ഇതു ഉള്‍പ്പെടുത്തുന്നു, അതിനാൽ ഒന്നും നൽകാതെ `dotnet run` പ്രവർത്തിക്കും.

**പ്രതീക്ഷ:** MS Learn ഡോക്യുമെന്റിലെ `.csproj` ടെംപ്ലേറ്റിൽ ഇത് ഉൾപ്പെടുത്തണം, ഉപയോക്താക്കൾക്ക് `-r` ഫ്ലാഗ് ഓർക്കേണ്ട ആവശ്യമില്ലാതാക്കാൻ.

---

## 4. JavaScript Whisper — ഓഡിയോ ട്രാൻസ്ക്രിപ്ഷൻ ശൂന്യ/ബൈനറി ഔട്ട്പുട്ട് നൽകുന്നു

**സ്ഥിതി:** തുറന്ന (പുനരാരംഭം — ആദിത്യ റിപ്പോർട്ടുമുതൽ മോശം)
**ഗൗരവം:** പ്രധാന
**ഘടകം:** JavaScript Whisper നടത്തിപ്പ് (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**പുനരാവർത്തനം:** `node foundry-local-whisper.mjs` റൺ ചെയ്യുക — എല്ലാ ഓഡിയോ ഫയലുകളും ശൂന്യമായോ ബൈനറി ഔട്ട്പुटായോ മടക്കുന്നു, വാചക ട്രാൻസ്ക്രിപ്ഷൻ ഇല്ലാതെ

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

ആദ്യമായി 5-ആം ഓഡിയോ ഫയലിൽ മാത്രമേ ശൂന്യം ലഭിച്ചിരിക്കുകയുള്ളൂ; v0.9.x മുതൽ എല്ലാ 5 ഫയലുകളും ട്രാൻസ്ക്രൈബു ചെയ്ത വാക്കുകളുടെ പകരം ഒരു ബൈറ്റ് (`\ufffd`) മടങ്ങി നൽകുന്നു. Python Whisper OpenAI SDK ഉപയോഗിച്ച് ശരിയായി ട്രാൻസ്ക്രൈബ് ചെയ്യുന്നു.

**പ്രതീക്ഷ:** `createAudioClient()` Python/C# നടപ്പാക്കലുമായി യോജിച്ച് വാചക ട്രാൻസ്ക്രിപ്ഷൻ മടക്കണം.

---

## 5. C# SDK net8.0 മാത്രം അടക്കം ചെയ്യുന്നു — ഔദ്യോഗിക .NET 9 അല്ലെങ്കിൽ .NET 10 ലക്ഷ്യം ഇല്ല

**സ്ഥിതി:** തുറന്ന
**ഗൗരവം:** ഡോക്യുമെന്റേഷൻ കുറവ്
**ഘടകം:** `Microsoft.AI.Foundry.Local` NuGet പാക്കേജ് v0.9.0
**സ്ഥാപന കമാൻഡ്:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet പാക്കേജ് ഒരു ഏക ടാർഗറ്റ് ഫ്രെയിംവർക്കും മാത്രമായി ഉൾക്കൊള്ളിക്കുന്നു:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

`net9.0` അല്ലെങ്കിൽ `net10.0` TFM ഇല്ല. എതിരായി, കൂട്ടുകാരൻ പാക്കേജ് `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`, `netstandard2.0` ഉൾക്കൊള്ളിക്കുന്നു.

### അനുയോജ്യതാ പരിശോധന

| ലക്ഷ്യ ഫ്രെയിംവർക്ക് | ബിൽഡ് | റൺ | കുറിപ്പുകൾ |
|-----------------|--------|------|-----------|
| net8.0 | ✅ | ✅ | ഔദ്യോഗികമായി പിന്തുണയ്ക്കുന്നു |
| net9.0 | ✅ | ✅ | ഫോർവേഡ്-കമ്പാറ്റ് ഉപയോഗിച്ച് ബിൽഡ് — വർക്ഷോപ്പ് ഉദാഹരണങ്ങളിൽ പ്രസിദ്ധമാണ് |
| net10.0 | ✅ | ✅ | .NET 10.0.3 റൺടൈമിൽ ഫോർവേഡ്-കമ്പാറ്റിങ്ങിൽ വർക്കും |

net8.0 അസംബ്ലി പുതിയ റൺടൈമുകളിൽ .NET-ന്റെ ഫോർവേഡ്-കമ്പാറ്റിബിലിറ്റി സംവിധാനം വഴി ലോഡ് ചെയ്യപ്പെടുന്നു, അതിനാൽ ബിൽഡ് വിജയിക്കുന്നു. എന്നാൽ ഇത് SDK ടീമിന്റെ അനാച്ഛാദനവും പരീക്ഷണവും ഇല്ലാത്തതാണു.

### ഉദാഹരണങ്ങൾ net9.0 ലക്ഷ്യം ചെയ്തിരിക്കുന്നത് എന്തുകൊണ്ട്

1. **.NET 9 ഏറ്റവും പുതിയ സ്തിരമായ റിലീസാണ്** — അധികം വർക്ഷോപ്പ് പങ്കെടുത്തവർ ഇതിനകം ഇൻസ്റ്റാൾ ചെയ്തിരിക്കും
2. **ഫോർവേഡ്-കമ്പാറ്റിബിലിറ്റി പ്രവർത്തിക്കുന്നു** — NuGet പാക്കേജിലെ net8.0 അസംബ്ലി .NET 9 റൺടൈമിൽ തടസ്സമില്ലാതെ പ്രവർത്തിക്കുന്നു
3. **.NET 10 (പ്രിവ്യൂ/RC)** അത്ര പുതിയതാണ് — എല്ലാ ഉപയോക്താക്കൾക്കും ഉള്ള വർക്ഷോപ്പിൽ ലക്ഷ്യമിടാൻ അനുയോജ്യമല്ല

**പ്രതീക്ഷ:** ഭാവി SDK റിലീസുകളിൽ `net9.0`യും `net10.0`-ഉം `net8.0`-ക്കൊപ്പം ഉൾപ്പെടുത്താൻ പരിഗണിക്കണം, `Microsoft.Agents.AI.OpenAI` മാതൃകയെ അനുസരിച്ച് പുതിയ റൺടൈമുകൾക്കുള്ള സ്ഥിരീകരിക്കപ്പെട്ട പിന്തുണ നൽകാൻ.

---

## 6. JavaScript ChatClient Streaming കോൾബാക്കുകൾ മാത്രമാണ് ഉപയോഗിക്കുന്നത്, Async Iterators അല്ല

**സ്ഥിതി:** തുറന്ന
**ഗൗരവം:** ഡോക്യുമെന്റേഷൻ കുറവ്
**ഘടകം:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` നൽകുന്ന `ChatClient`-ൽ `completeStreamingChat()` രീതിയുണ്ട്, എന്നാൽ ഇത് async iterable മടക്ക് നടത്താതെ **കോൾബാക്ക് പാറ്റേൺ** ഉപയോഗിക്കുന്നു:

```javascript
// ❌ ഇത് പ്രവർത്തിക്കുന്നില്ല — "stream is not async iterable" എന്ന തെറ്റു വരുന്നു
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ ശരിയായ മാതൃക — ഒരു കോൾബാക്ക് പാസ്സ് ചെയ്യുക
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**പ്രഭാവം:** OpenAI SDKയുടെ async iteration പാറ്റേൺ (`for await`) അറിയുന്ന ഡെവലപ്പർമാർക്ക് തെറ്റിദ്ധാരണയുള്ള പിശകുകൾ വരാം. കോൾബാക്ക് സാധുവായ ഫംഗ്ഷൻ ആയിരിക്കണം അല്ലെങ്കിൽ SDK "Callback must be a valid function." എന്ന പിശക് ഉണ്ട്.

**പ്രതീക്ഷ:** SDK റഫറൻസിൽ കോൾബാക്ക് പാറ്റേൺ ദൃശ്യവൽക്കരിക്കുക. അല്ലെങ്കിൽ, OpenAI SDKയുമായി ഏകീകൃതമാക്കാൻ async iterable പാറ്റേൺ പിന്തുണക്കുക.

---

## പരിസ്ഥിതി വിവരങ്ങൾ

| ഘടകം | പതിപ്പ് |
|--------|---------|
| OS | Windows 11 ARM64 |
| ഹാർഡ്‌വെയർ | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |