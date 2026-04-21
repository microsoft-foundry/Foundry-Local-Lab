# ज्ञात समस्या — Foundry Local कार्यशाळा

**Snapdragon X Elite (ARM64)** डिव्हाइसवर Windows चालवताना, Foundry Local SDK v0.9.0, CLI v0.8.117 आणि .NET SDK 10.0 वापरून ही कार्यशाळा तयार करताना आणि चाचणी करताना आढळलेल्या समस्या.

> **शेवटची पडताळणी:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ONNX Runtime कडून ओळखले जात नाही

**स्थिती:** उघडलेले
**तीव्रता:** चेतावणी (अडथळा नाही)
**घटक:** ONNX Runtime / cpuinfo
**पुन्हा तयार करणे:** Snapdragon X Elite हार्डवेअरवर Foundry Local सेवा प्रत्येक वेळी सुरू केल्यावर

Foundry Local सेवा प्रत्येक वेळी सुरू झाल्यावर दोन चेतावण्या दिसतात:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**परिणाम:** चेतावण्या फक्त दृश्यात्मक आहेत — इंफरन्स योग्य प्रकारे कार्य करते. तथापि, त्या प्रत्येक वेळेस चालू होतात आणि कार्यशाळेतील सहभागी गोंधळले जातात. Qualcomm Oryon CPU कोर ओळखण्यासाठी ONNX Runtime cpuinfo लायब्ररी अद्ययावत करण्याची गरज आहे.

**अपेक्षित:** Snapdragon X Elite समर्थित ARM64 CPU म्हणून ओळखले जावे आणि त्रुटी-स्तरीय संदेश उत्पन्न करू नयेत.

---

## 2. SingleAgent NullReferenceException प्रथम धावणीत

**स्थिती:** उघडलेले (कधीतरी होते)
**तीव्रता:** गंभीर (क्रॅश)
**घटक:** Foundry Local C# SDK + Microsoft Agent Framework
**पुन्हा तयार करणे:** `dotnet run agent` चालवा — मॉडेल लोड झाल्यानंतर लगेच क्रॅश होतो

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**संदर्भ:** ओळ 37 `model.IsCachedAsync(default)` कॉल करते. क्रॅश हा एजंटच्या प्रथम धावणीत झाला, नव्या `foundry service stop` नंतर. पुढील धावण्या यशस्वी होत्या.

**परिणाम:** कधीकधी — SDK च्या सेवा इनिशियलायझेशन किंवा कॅटलॉग क्वेरीमध्ये रेस कंडिशन सूचित करते. `GetModelAsync()` कॉल सेवा पूर्णपणे तयार होण्याच्या आधी परतवू शकतो.

**अपेक्षित:** `GetModelAsync()` ला सेवा तयार होईपर्यंत ब्लॉक करावे किंवा सेवा रेकॉर्ड पूर्ण झालेली नसेल तर स्पष्ट त्रुटी संदेश द्यावा.

---

## 3. C# SDK साठी स्पष्ट RuntimeIdentifier ची गरज आहे

**स्थिती:** उघडलेले — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) मध्ये ट्रॅक केले आहे
**तीव्रता:** दस्तऐवजीकरणाचा फरक
**घटक:** `Microsoft.AI.Foundry.Local` NuGet पॅकेज
**पुन्हा तयार करणे:** `.csproj` मध्ये `<RuntimeIdentifier>` शिवाय .NET 8+ प्रकल्प तयार करा

बिल्ड म्हणून अयशस्वी होतो:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**मूळ कारण:** RID आवश्यकता अपेक्षित आहे — SDK स्थानिक बायनरीज (P/Invoke मध्ये `Microsoft.AI.Foundry.Local.Core` व ONNX Runtime) सह येतो, .NET ला कोणत्या प्लॅटफॉर्म-विशिष्ट लायब्ररीची निवड करायची आहे हे माहित असणे आवश्यक आहे.

हे MS Learn वर दस्तऐवजीकरण केले आहे ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), जिथे चालण्याच्या सूचना दाखवल्या आहेत:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

पण वापरकर्त्यांना प्रत्येक वेळेस `-r` फ्लॅग लक्षात ठेवावा लागतो, जो विसरण्यास सोपा आहे.

**कार्यबंदी:** `.csproj` मध्ये एक स्वयंचलित ओळख फॉलबॅक जोडा जेणेकरून `dotnet run` कोणत्याही फ्लॅगशिवाय चालेल:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` हा MSBuild मधील अंगभूत प्रॉपर्टी आहे जो होस्ट मशीनचा RID स्वयंचलितपणे सोडवतो. SDK च्या स्वतःच्या चाचणी प्रकल्पांनी हा पॅटर्न वापरला आहे. स्पष्ट `-r` फ्लॅग दिल्यास त्याचा आदर केला जातो.

> **टीप:** कार्यशाळेच्या `.csproj` मध्ये हा फॉलबॅक आहे ज्यामुळे `dotnet run` कोणत्याही प्लॅटफॉर्मवर बाहेरून काम करते.

**अपेक्षित:** MS Learn डॉक्युमेंटच्या `.csproj` टेम्प्लेटमध्ये हा स्वयंचलित ओळख पॅटर्न जोडला पाहिजे जेणेकरून वापरकर्त्यांना `-r` फ्लॅग लक्षात ठेवण्याची गरज नसेल.

---

## 4. JavaScript Whisper — ऑडिओ ट्रान्सक्रिप्शन रिकामी/दुर्लक्षित आउटपुट देते

**स्थिती:** उघडलेले (पूर्वीपासून फक्त वाईट झाले आहे)
**तीव्रता:** मुख्य
**घटक:** JavaScript Whisper कार्यान्वयन (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**पुन्हा तयार करणे:** `node foundry-local-whisper.mjs` चालवा — सर्व ऑडिओ फाइल्स रिकामी किंवा द्विमितीय आउटपुट देते, टेक्स्ट ट्रान्सक्रिप्शनऐवजी

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

मुळात फक्त 5 वी ऑडिओ फाइल रिकामी येत होती; v0.9.x पासून सर्व 5 फाइल्स एकच बाइट (`\ufffd`) देते टेक्स्ट ट्रान्सक्रिप्शन ऐवजी. Python Whisper कार्यान्वयन OpenAI SDK वापरून याच फाइल्स योग्यरित्या ट्रान्सक्राइब करते.

**अपेक्षित:** `createAudioClient()` टेक्स्ट ट्रान्सक्रिप्शन परत द्यावी, जी Python/C# कार्यान्वयनांसह सुसंगत असेल.

---

## 5. C# SDK केवळ net8.0 पाठविते — अधिकृत .NET 9 किंवा .NET 10 लक्ष्य नाही

**स्थिती:** उघडलेले
**तीव्रता:** दस्तऐवजीकरणाचा फरक
**घटक:** `Microsoft.AI.Foundry.Local` NuGet पॅकेज v0.9.0
**इन्स्टॉल कमांड:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet पॅकेज फक्त एकच लक्ष्य फ्रेमवर्क पाठवते:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

`net9.0` किंवा `net10.0` TFM समाविष्ट नाही. तुलनेत, जोडीदार पॅकेज `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`, आणि `netstandard2.0` पाठवते.

### सुसंगतता चाचणी

| लक्ष्य फ्रेमवर्क | बिल्ड | धाव | टीप |
|----------------|--------|----|-----|
| net8.0 | ✅ | ✅ | अधिकृतपणे समर्थित |
| net9.0 | ✅ | ✅ | पुढील सुसंगततेने तयार — कार्यशाळेतील उदाहरणांमध्ये वापरलेले |
| net10.0 | ✅ | ✅ | .NET 10.0.3 रनटाइमसह पुढील सुसंगततेने तयार आणि धावते |

net8.0 असेंब्ली नवीन रनटाइमवर .NET च्या पुढील सुसंगतता यंत्रणेने लोड होते, त्यामुळे बिल्ड यशस्वी होते. मात्र, हे SDK टीमकडून दस्तऐवजित आणि चाचणी केलेले नाही.

### का उदाहरणे net9.0 लक्ष्य करतात

1. **.NET 9 हा नवीनतम स्थिर आवृत्ती आहे** — बऱ्याच कार्यशाळेतील सहभाग्यांजवळ तो इंस्टॉल असेल
2. **पुढील सुसंगतता कार्य करते** — NuGet पॅकेजमधील net8.0 असेंब्ली .NET 9 रनटाइमवर न समस्येशिवाय चालते
3. **.NET 10 (पूर्वावलोकन/RC)** खूप नवीन आहे ज्याला सर्वांसाठी कार्य करणाऱ्या कार्यशाळेत लक्ष्य करणे योग्य नाही

**अपेक्षित:** भविष्यातील SDK रिलीझेस `Microsoft.Agents.AI.OpenAI` द्वारे वापरल्या जाणाऱ्या पॅटर्नप्रमाणे `net9.0` आणि `net10.0` TFM समाविष्ट करा ज्यामुळे अधिकृत आणि पडताळलेले समर्थन मिळेल.

---

## 6. JavaScript ChatClient Streaming कॉलबॅक वापरतो, Async Iterator नाही

**स्थिती:** उघडलेले
**तीव्रता:** दस्तऐवजीकरणाचा फरक
**घटक:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` द्वारा परत केलेला `ChatClient` एक `completeStreamingChat()` पद्धत देतो, पण तो async iterable परत न करता **कॉलबॅक पॅटर्न** वापरतो:

```javascript
// ❌ हे कार्य करत नाही — "stream async iterable नाही" असा त्रुटी संदेश निर्माण करते
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ बरोबर नमुना — callback पास करा
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**परिणाम:** OpenAI SDK चा async iteration पॅटर्न (`for await`) ओळखणारे विकसक यावर गोंधळलेल्या त्रुटी अनुभवतील. कॉलबॅक वैध फंक्शन असणे आवश्यक आहे अन्यथा SDK "Callback must be a valid function." चा त्रुटी फेकतो.

**अपेक्षित:** SDK संदर्भात कॉलबॅक पॅटर्नचे दस्तऐवजीकरण करा. किंवा पर्यायी स्वरूपात OpenAI SDK सारखाच async iterable पॅटर्न समर्थन द्या.

---

## पर्यावरण तपशील

| घटक | आवृत्ती |
|-----------|---------|
| OS | Windows 11 ARM64 |
| हार्डवेअर | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
हा दस्तऐवज AI भाषांतर सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) च्या सहाय्याने भाषांतरित केला आहे. आम्ही अचूकतेसाठी प्रयत्नशील असूनही, कृपया लक्षात ठेवा की स्वयंचलित भाषांतरात चुका किंवा अचूकतेमध्ये त्रुटी असू शकतात. मूळ दस्तऐवज त्याच्या स्थानिक भाषेत अधिकृत स्रोत म्हणून विचारात घ्यावा. महत्त्वाच्या माहितीकरिता व्यावसायिक मानवी भाषांतर शिफारस केली जाते. या भाषांतराचा वापर कधीही झालेल्या गैरसमजुती किंवा चुकीच्या अर्थसंग्रहासाठी आम्ही जबाबदार नाही.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->