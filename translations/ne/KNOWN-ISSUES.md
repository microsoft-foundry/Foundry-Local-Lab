# ज्ञात समस्याहरू — फाउन्ड्री लोकल कार्यशाला

Windows मा चलेको **Snapdragon X Elite (ARM64)** उपकरणमा फाउन्ड्री लोकल SDK v0.9.0, CLI v0.8.117, र .NET SDK 10.0 सँग यो कार्यशाला बनाउँदा र परीक्षण गर्दा पाइएका समस्याहरू।

> **अन्तिम पुष्टि:** २०२६-०३-११

---

## १. Snapdragon X Elite CPU लाई ONNX Runtime द्वारा चिन्ने छैन

**स्थिति:** खुला  
**गम्भीरता:** चेतावनी (अवरुद्ध नगर्ने)  
**घटक:** ONNX Runtime / cpuinfo  
**पुनरुत्पादन:** Snapdragon X Elite हार्डवेयरमा प्रत्येक फाउन्ड्री लोकल सेवा सुरु गर्दा

फाउन्ड्री लोकल सेवा सुरु हुँदा हरेक पटक दुई वटा चेतावनीहरू देखा पर्छन्:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**प्रभाव:** चेतावनीहरू केवल सौन्दर्य सम्बन्धी हुन् — inference सटिक रूपमा काम गर्छ। यद्यपि, यी हरेक पटक देखा पर्दछन् र कार्यशाला सहभागीहरूलाई भ्रमित पार्न सक्छ। Qualcomm Oryon CPU कोरहरूलाई चिन्ने लागि ONNX Runtime को cpuinfo लाइब्रेरी अपडेट गर्न आवश्यक छ।

**अपेक्षित:** Snapdragon X Elite लाई समर्थन गरिएको ARM64 CPU को रूपमा चिन्ने र त्रुटि स्तरका सन्देशहरू नदेखाउने।

---

## २. पहिलो रनमा SingleAgent NullReferenceException

**स्थिति:** खुला (अन्तरालमा)  
**गम्भीरता:** गम्भीर (क्र्यास)  
**घटक:** फाउन्ड्री लोकल C# SDK + Microsoft Agent Framework  
**पुनरुत्पादन:** `dotnet run agent` चलाउँदा — मोडल लोड भए लगत्तै क्र्यास हुन्छ

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**सन्दर्भ:** लाइन ३७ मा `model.IsCachedAsync(default)` कल हुन्छ। क्र्यास पहिलो पटक एजेन्ट चलाउँदा देखिएको हो जब नयाँ `foundry service stop` पछि चलाइएको। पछिल्ला रन भने सफल भए।

**प्रभाव:** अन्तरालमा देखिने समस्या — SDK को सेवा सुरुआत वा क्याटलग क्वेरीमा रेस कन्डिसन संकेत गर्छ। `GetModelAsync()` कल सेवाको पूर्ण तयारी नहुँदा पनि फर्किन सक्छ।

**अपेक्षित:** `GetModelAsync()` ले सेवा तयार नभएसम्म ब्लक गर्नुपर्छ वा सेवा प्रारम्भ हुन नसकेको स्पष्ट त्रुटि जनाउनुपर्छ।

---

## ३. C# SDK लाई स्पष्ट RuntimeIdentifier आवश्यक छ

**स्थिति:** खुला — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) मा ट्र्याक गरिँदैछ  
**गम्भीरता:** दस्तावेजीकरण ग्याप  
**घटक:** `Microsoft.AI.Foundry.Local` NuGet प्याकेज  
**पुनरुत्पादन:** `.csproj` मा `<RuntimeIdentifier>` बिना .NET ८+ प्रोजेक्ट सिर्जना गर्दा

बिल्ड असफल हुन्छ:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**मूल कारण:** यो आवश्यक छ — SDK ले नेटिभ बाइनरी (P/Invoke द्वारा `Microsoft.AI.Foundry.Local.Core` र ONNX Runtime मा) उपलब्ध गराउने हुँदा .NET ले कुन प्लेटफर्म-विशिष्ट लाइब्रेरी प्रयोग गर्ने बुझ्नुपर्छ।

यसलाई MS Learn मा कभर गरिएको छ ([कैसे नेटिभ च्याट कम्प्लीसन प्रयोग गर्ने](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), जहाँ चलाउने निर्देशनहरूमा देखाइएको छ:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
तर प्रयोगकर्ताले हरेक पटक `-r` फ्ल्याग याद गर्नुपर्ने हुनाले सजिलो हुँदैन।

**व्यवस्था:** `.csproj` मा अटो-डिटेक्ट फ्याकलब्याक थप्नुस् जसले `dotnet run` लाई कुनै फ्ल्याग्स बिना काम गर्न सजिलो बनाउँछ:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` एउटा बिल्ट-इन MSBuild प्रोपर्टी हो जुन स्वतः होस्ट मेसिनको RID सेट गर्छ। SDK का आफ्नै टेस्ट प्रोजेक्टहरूले यस विधि अपनाएका छन्। स्पष्ट `-r` फ्ल्याग दिइएमा तिनीहरू अझै मान्य हुन्छन्।

> **सूचना:** कार्यशाला `.csproj` ले यो फ्याकलब्याक समावेश गरेको छ ताकि `dotnet run` को प्रयोग सब प्लेटफर्ममा सहज होस्।

**अपेक्षित:** MS Learn का `.csproj` टेम्प्लेटमा यो अटो-डिटेक्ट पद्धति समावेश होस् जसले प्रयोगकर्ताहरूलाई `-r` फ्ल्याग याद गर्नु नपरोस्।

---

## ४. JavaScript Whisper — अडियो ट्रान्सक्रिप्शन खाली/बाइनरी आउटपुट फर्काउँछ

**स्थिति:** खुला (पछिल्लो रिपोर्टको तुलना मा बिग्रिएको)  
**गम्भीरता:** प्रमुख  
**घटक:** JavaScript Whisper कार्यान्वयन (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**पुनरुत्पादन:** `node foundry-local-whisper.mjs` चलाउँदा — सबै अडियो फाइलहरू खाली वा बाइनरी आउटपुट फर्काउँछन्, पाठ ट्रान्सक्रिप्शन होइन

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
पहिला त केवल पाँचौं फाइल खाली फर्काउँथ्यो; v0.9.x बाट सबै ५ फाइलहरूले एकल बाइट (`\ufffd`) फर्काउँछन् टेस्ट ट्रान्सक्रिप्शनको सट्टा। Python Whisper कार्यान्वयन OpenAI SDK प्रयोग गरेर सोही फाइलहरू सही ट्रान्सक्रिप्शन गर्छ।

**अपेक्षित:** `createAudioClient()` ले Python/C# जस्तै सही पाठ ट्रान्सक्रिप्शन फर्काओस्।

---

## ५. C# SDK ले केवल net8.0 मात्र समावेश गर्छ — आधिकारिक .NET 9 वा .NET 10 लक्ष्य छैन

**स्थिति:** खुला  
**गम्भीरता:** दस्तावेजीकरण ग्याप  
**घटक:** `Microsoft.AI.Foundry.Local` NuGet प्याकेज v0.9.0  
**इंस्टल कमाण्ड:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet प्याकेजले एउटा मात्र लक्ष्य फ्रेमवर्क समावेश गर्छ:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` वा `net10.0` TFM समावेश छैन। यसको विपरित, `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) ले `net8.0`, `net9.0`, `net10.0`, `net472`, र `netstandard2.0` दिन्छ।

### अनुकूलता परीक्षण

| लक्ष्य फ्रेमवर्क | बिल्ड | रन | टिप्पणी |
|-----------------|-------|-----|----------|
| net8.0          | ✅    | ✅  | आधिकारिक समर्थन छ |
| net9.0          | ✅    | ✅  | फारवर्ड कम्प्याट प्रयोग गरेर बिल्ड हुन्छ — कार्यशाला नमूनामा प्रयोग |
| net10.0         | ✅    | ✅  | फर्वार्ड कम्प्याट प्रयोग गरेर .NET 10.0.3 रनटाइममा बिल्ड र रन |

net8.0 असेम्बली नयाँ रनटाइमहरूमा .NET को फारवर्ड कम्पाट मेकानिजम मार्फत लोड हुन्छ, त्यसैले बिल्ड सफल हुन्छ। तर यो SDK टोलीद्वारा अव्याख्यायित र असंयोजित छ।

### किन नमूनाहरूले net9.0 लक्ष्य गर्दछ

1. **.NET 9 सबैभन्दा नयाँ स्थिर संस्करण हो** — धेरै सहभागीहरूसँग यो हुन्छ  
2. **फर्वर्ड कम्प्याट काम गर्छ** — NuGet प्याकेजको net8.0 असेम्बली .NET 9 रनटाइममा सहज चल्छ  
3. **.NET 10 (प्रिभ्यू/आरसी)** कार्यशाला लागि धेरै नयाँ छ जसले सबैलाई काम गर्नुपर्छ।

**अपेक्षित:** भविष्यका SDK रिलिजहरूले `net9.0` र `net10.0` TFMs पनि समावेश गर्न विचार गर्नुपर्छ ताकि `Microsoft.Agents.AI.OpenAI` जस्तै प्याकेजहरूका समानान्तर समर्थन र नयाँ रनटाइमहरूको प्रमाणित समर्थन प्रदान गर्न सकियोस्।

---

## ६. JavaScript ChatClient Streaming ले Callback प्रयोग गर्छ, Async Iterators होइन

**स्थिति:** खुला  
**गम्भीरता:** दस्तावेजीकरण ग्याप  
**घटक:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` ले फर्काउने `ChatClient` मा `completeStreamingChat()` मेथड छ, तर यो **callback ढाँचा** प्रयोग गर्छ, async iterable फर्काउँदैन:

```javascript
// ❌ यसले काम गर्दैन — "stream is not async iterable" फ्याँक्छ
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ सही ढाँचा — callback पास गर्नुहोस्
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**प्रभाव:** OpenAI SDK को async iteration (`for await`) परिचित विकासकर्ताहरूलाई यो त्रुटिपूर्ण लाग्न सक्छ। callback एक मान्य फंक्शन हुनु अनिवार्य छ नभए SDK ले "Callback must be a valid function." त्रुटि दिन्छ।

**अपेक्षित:** SDK सन्दर्भमा callback पद्धति स्पष्ट पार्नुपर्छ। अथवा OpenAI SDK सँग सुसंगतता राख्न async iterable शैली समर्थन गर्नुपर्छ।

---

## वातावरण विवरण

| घटक                         | संस्करण          |
|-----------------------------|------------------|
| OS                          | Windows 11 ARM64 |
| हार्डवेयर                   | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI            | 0.8.117          |
| Foundry Local SDK (C#)       | 0.9.0            |
| Microsoft.Agents.AI.OpenAI   | 1.0.0-rc3        |
| OpenAI C# SDK               | 2.9.0            |
| .NET SDK                    | 9.0.312, 10.0.104|
| foundry-local-sdk (Python)   | 0.5.x            |
| foundry-local-sdk (JS)       | 0.9.x            |
| Node.js                     | 18+              |
| ONNX Runtime                | 1.18+            |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
यो दस्तावेज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरेर अनुवाद गरिएको छ। हामी शुद्धताका लागि प्रयास गर्दैछौं, कृपया जानकार हुनुहोस् कि स्वचालित अनुवादमा त्रुटिहरू वा अशुद्धताहरू हुन सक्छन्। मूल भाषामा रहेको दस्तावेजलाई प्रामाणिक स्रोत मानिनु पर्छ। महत्वपूर्ण जानकारीका लागि, व्यावसायिक मानव अनुवाद सल्लाहयोग्य हुन्छ। यस अनुवादको प्रयोगबाट उत्पन्न हुने कुनै पनि गलत बुझाइ वा व्याख्याका लागि हामी जिम्मेवार छैनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->