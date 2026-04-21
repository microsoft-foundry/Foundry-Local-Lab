# ज्ञात समस्याएँ — Foundry लोकल वर्कशॉप

यह वर्कशॉप **Snapdragon X Elite (ARM64)** डिवाइस पर विंडोज़ चलाते हुए, Foundry लोकल SDK v0.9.0, CLI v0.8.117, और .NET SDK 10.0 के साथ बनाते और परीक्षण करते समय आने वाली समस्याएँ।

> **अंतिम सत्यापन:** 2026-03-11

---

## 1. Snapdragon X Elite CPU को ONNX Runtime द्वारा नहीं पहचाना जाना

**स्थिति:** खुला
**गंभीरता:** चेतावनी (गैर-बाधित)
**घटक:** ONNX Runtime / cpuinfo
**पुनः प्राप्ति:** Snapdragon X Elite हार्डवेयर पर हर Foundry Local सेवा शुरू करना

हर बार Foundry Local सेवा शुरू होती है, दो चेतावनियाँ जारी होती हैं:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**प्रभाव:** चेतावनियाँ केवल सौंदर्य संबंधी हैं — अनुमान सही ढंग से काम करता है। हालांकि, ये हर रन पर प्रकट होती हैं और वर्कशॉप प्रतिभागियों को भ्रमित कर सकती हैं। ONNX Runtime cpuinfo लाइब्रेरी को Qualcomm Oryon CPU कोर पहचानने के लिए अपडेट करने की आवश्यकता है।

**अपेक्षित:** Snapdragon X Elite को बिना त्रुटि-स्तर के संदेश निकाले समर्थित ARM64 CPU के रूप में पहचाना जाना चाहिए।

---

## 2. SingleAgent NullReferenceException पहली बार चलाने पर

**स्थिति:** खुला (अवसादपूर्ण)
**गंभीरता:** गंभीर (क्रैश)
**घटक:** Foundry Local C# SDK + Microsoft Agent Framework
**पुनः प्राप्ति:** `dotnet run agent` चलाएं — मॉडल लोड के तुरंत बाद क्रैश हो जाता है

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**प्रसंग:** पंक्ति 37 `model.IsCachedAsync(default)` को कॉल करती है। एजेंट के पहले रन पर क्रैश तब हुआ जब ताजा `foundry service stop` किया गया था। बाद के रन एक ही कोड के साथ सफल रहे।

**प्रभाव:** अस्थायी — SDK की सेवा आरंभ करने या कैटलॉग क्वेरी में दौड़ की स्थिति सुझाता है। `GetModelAsync()` कॉल सेवा पूरी तरह तैयार होने से पहले लौट सकती है।

**अपेक्षित:** `GetModelAsync()` को या तो सेवा के तैयार होने तक ब्लॉक करना चाहिए या एक स्पष्ट त्रुटि संदेश लौटाना चाहिए यदि सेवा शुरू नहीं हुई है।

---

## 3. C# SDK के लिए स्पष्ट RuntimeIdentifier आवश्यक है

**स्थिति:** खुला — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) में ट्रैक किया गया
**गंभीरता:** दस्तावेज़ीकरण अंतर
**घटक:** `Microsoft.AI.Foundry.Local` NuGet पैकेज
**पुनः प्राप्ति:** `.csproj` में `<RuntimeIdentifier>` के बिना .NET 8+ प्रोजेक्ट बनाएं

बिल्ड विफल होता है:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**मूल कारण:** RID आवश्यक है — SDK मूल बाइनरीज (P/Invoke `Microsoft.AI.Foundry.Local.Core` और ONNX Runtime में) भेजता है, इसलिए .NET को यह जानना जरूरी है कि कौन-सी प्लेटफ़ॉर्म-विशिष्ट लाइब्रेरी को हल करना है।

यह MS Learn पर दस्तावेजीकृत है ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), जहाँ रन निर्देश दिखाते हैं:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

हालाँकि, उपयोगकर्ताओं को हर बार `-r` ध्वज याद रखना पड़ता है, जो भूलना आसान है।

**वर्कअराउंड:** अपने `.csproj` में एक स्वचालित डिटेक्ट वापसी जोड़ें ताकि बिना किसी ध्वज के `dotnet run` काम करें:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` एक बिल्ट-इन MSBuild प्रॉपर्टी है जो होस्ट मशीन का RID स्वचालित रूप से हल करती है। SDK के अपने परीक्षण प्रोजेक्ट पहले से ही इस पैटर्न का उपयोग करते हैं। यदि प्रदान किया गया हो तो स्पष्ट `-r` ध्वज अभी भी सम्मानित होते हैं।

> **नोट:** वर्कशॉप `.csproj` में यह वापसी शामिल है ताकि `dotnet run` किसी भी प्लेटफ़ॉर्म पर बॉक्स से बाहर काम करे।

**अपेक्षित:** MS Learn के `.csproj` टेम्पलेट में यह स्वचालित डिटेक्ट पैटर्न शामिल होना चाहिए ताकि उपयोगकर्ताओं को `-r` ध्वज याद रखने की जरूरत न पड़े।

---

## 4. JavaScript Whisper — ऑडियो ट्रांसक्रिप्शन खाली/बाइनरी आउटपुट लौटाता है

**स्थिति:** खुला (पिछली रिपोर्ट के बाद खराब हुआ)
**गंभीरता:** प्रमुख
**घटक:** JavaScript Whisper कार्यान्वयन (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**पुनः प्राप्ति:** `node foundry-local-whisper.mjs` चलाएं — सभी ऑडियो फ़ाइलें खाली या बाइनरी आउटपुट लौटाती हैं न कि टेक्स्ट ट्रांसक्रिप्शन

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

मूल रूप से केवल पाँचवीं ऑडियो फ़ाइल खाली लौटाती थी; v0.9.x के अनुसार, सभी 5 फ़ाइलें ट्रांसक्राइब की गई टेक्स्ट के बजाय एक एकल बाइट (`\ufffd`) लौटाती हैं। Python Whisper कार्यान्वयन OpenAI SDK का उपयोग करते हुए इन्हीं फ़ाइलों का सही ट्रांसक्रिप्शन करता है।

**अपेक्षित:** `createAudioClient()` को Python/C# कार्यान्वयन के समान टेक्स्ट ट्रांसक्रिप्शन लौटाना चाहिए।

---

## 5. C# SDK केवल net8.0 भेजता है — आधिकारिक .NET 9 या .NET 10 लक्ष्य नहीं

**स्थिति:** खुला
**गंभीरता:** दस्तावेज़ीकरण अंतर
**घटक:** `Microsoft.AI.Foundry.Local` NuGet पैकेज v0.9.0
**इंस्टॉल आदेश:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet पैकेज केवल एक टारगेट फ्रेमवर्क भेजता है:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

कोई `net9.0` या `net10.0` TFM शामिल नहीं है। इसके विपरीत, साथी पैकेज `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472`, और `netstandard2.0` भेजता है।

### संगतता परीक्षण

| लक्ष्य फ्रेमवर्क | बिल्ड | रन | नोट्स |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | आधिकारिक रूप से समर्थित |
| net9.0 | ✅ | ✅ | फॉरवर्ड-कंपैट के जरिए बिल्ड — वर्कशॉप नमूनों में उपयोग किया गया |
| net10.0 | ✅ | ✅ | .NET 10.0.3 रनटाइम के साथ फॉरवर्ड-कंपैट द्वारा बिल्ड और रन |

net8.0 असेंबली नए रनटाइम पर .NET के फॉरवर्ड-कंपैट मैकेनिज्म के माध्यम से लोड होती है, इसलिए बिल्ड सफल रहता है। हालांकि, यह SDK टीम द्वारा दस्तावेजीकृत और परीक्षण नहीं की गई है।

### नमूने क्यों net9.0 को लक्षित करते हैं

1. **.NET 9 नवीनतम स्थिर रिलीज़ है** — अधिकांश वर्कशॉप प्रतिभागियों के पास यह स्थापित है
2. **फॉरवर्ड कंपैट काम करता है** — NuGet पैकेज में net8.0 असेंबली .NET 9 रनटाइम पर बिना समस्या चलती है
3. **.NET 10 (पूर्वावलोकन/RC)** नए लोगों के लिए अधिमात्रा है और सभी के लिए काम करने वाले वर्कशॉप में लक्ष्य बनाना व्यवहार्य नहीं

**अपेक्षित:** भविष्य के SDK संस्करणों को `Microsoft.Agents.AI.OpenAI` द्वारा उपयोग किए गए पैटर्न के समान `net9.0` और `net10.0` TFMs जोड़ने पर विचार करना चाहिए और नए रनटाइम के लिए सत्यापित समर्थन प्रदान करना चाहिए।

---

## 6. JavaScript ChatClient स्ट्रीमिंग कॉलबैक का उपयोग करता है, Async Iterators का नहीं

**स्थिति:** खुला
**गंभीरता:** दस्तावेज़ीकरण अंतर
**घटक:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` द्वारा लौटाया गया `ChatClient` एक `completeStreamingChat()` विधि प्रदान करता है, लेकिन यह एक **कॉलबैक पैटर्न** का उपयोग करता है बजाय कि async iterable लौटाने के:

```javascript
// ❌ यह काम नहीं करता — "stream असिंक iterable नहीं है" त्रुटि देता है
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ सही पैटर्न — एक कॉलबैक पास करें
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**प्रभाव:** OpenAI SDK के async iteration पैटर्न (`for await`) से परिचित डेवलपर्स भ्रमित त्रुटियों का सामना करेंगे। कॉलबैक को एक वैध फ़ंक्शन होना चाहिए अन्यथा SDK "Callback must be a valid function." त्रुटि फेंकता है।

**अपेक्षित:** SDK संदर्भ में कॉलबैक पैटर्न का दस्तावेजीकरण करें। वैकल्पिक रूप से, OpenAI SDK के अनुरूपता के लिए async iterable पैटर्न का समर्थन करें।

---

## पर्यावरण विवरण

| घटक | संस्करण |
|-----------|---------|
| OS | Windows 11 ARM64 |
| हार्डवेयर | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |