![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग १२: Zava Creative Writer को लागि वेब UI निर्माण

> **लक्ष्य:** Zava Creative Writer मा ब्राउजर-आधारित फ्रन्ट एन्ड थप्नुहोस् ताकि तपाईं बहु-एजेन्ट पाइपलाइनलाई प्रत्यक्ष रूपमा हेर्न सक्नुहुनेछ, लाइभ एजेन्ट स्थिति ब्याज र स्ट्रिम गरिएको लेखको पाठ, सबै एउटा स्थानिय वेब सर्भरबाट सेवा गरिन्छ।

[भाग ७](part7-zava-creative-writer.md) मा तपाईंले Zava Creative Writer लाई **CLI एप्लिकेशन** (JavaScript, C#) र **हेडलस API** (Python) को रूपमा अन्वेषण गर्नुभयो। यस ल्याबमा तपाईंले साझा **भानिला HTML/CSS/JavaScript** फ्रन्ट एन्डलाई प्रत्येक ब्याकएन्डसँग जडान गर्नुहुनेछ ताकि प्रयोगकर्ताहरू टर्मिनलको सट्टा ब्राउजर मार्फत पाइपलाइनसँग अन्तरक्रिया गर्न सकून्।

---

## तपाईं के सिक्नुहुनेछ

| उद्देश्य | वर्णन |
|-----------|-------------|
| ब्याकएन्डबाट स्थिर फाइल सेवा गर्ने | तपाईंको API रुटको छेउमै HTML/CSS/JS डिरेक्टरी माउन्ट गर्ने |
| ब्राउजरमा स्ट्रिमिङ NDJSON उपभोग गर्ने | Fetch API लाई `ReadableStream` सँग प्रयोग गरी नयाँ लाइनले छुट्याइएको JSON पढ्ने |
| एकीकृत स्ट्रिमिङ प्रोटोकल | Python, JavaScript, र C# ब्याकएन्डहरूले समान सन्देश ढाँचा उत्सर्जन गर्ने सुनिश्चित गर्ने |
| प्रगतिशील UI अपडेटहरू | एजेन्ट स्थिति ब्याजहरू अपडेट गर्ने र लेखको पाठ टोकनद्वारा टोकन स्ट्रिम गर्ने |
| CLI एपमा HTTP लेयर थप्ने | अवस्थित अॉर्केस्ट्रेटर लॉजिकलाई Express-शैलीको सर्भर (JS) वा ASP.NET Core मिनिमल API (C#) मा र्याप गर्ने |

---

## वास्तुकला

UI एकल सेट स्थिर फाइलहरूको हो (`index.html`, `style.css`, `app.js`) जुन तीनै ब्याकएन्डहरूले साझा गर्छन्। प्रत्येक ब्याकएन्डले ती नै दुई रुटहरू एक्स्पोज गर्छ:

![Zava UI वास्तुकला — तीन ब्याकएन्डहरूसँग साझा फ्रन्ट एन्ड](../../../images/part12-architecture.svg)

| रुट | विधि | उद्देश्य |
|-------|--------|---------|
| `/` | GET | स्थिर UI सेवा गर्ने |
| `/api/article` | POST | बहु-एजेन्ट पाइपलाइन चलाउने र NDJSON स्ट्रिम गर्ने |

फ्रन्ट एन्डले JSON बडी पठाउँछ र जवाफलाई नयाँ लाइनले छुट्याइएको JSON सन्देशको स्ट्रिमको रूपमा पढ्छ। प्रत्येक सन्देशसँग `type` फिल्ड हुन्छ जुन UI ले सही प्यानल अपडेट गर्न प्रयोग गर्छ:

| सन्देश प्रकार | अर्थ |
|-------------|---------|
| `message` | स्थिति अपडेट (जस्तै "शोधकर्ता एजेन्ट कार्य सुरु हुँदैछ...") |
| `researcher` | अनुसन्धान नतिजा तयार छन |
| `marketing` | उत्पादन खोज नतिजा तयार छन |
| `writer` | लेखक सुरु भयो वा समाप्त भयो ({ start: true } वा { complete: true } समावेश) |
| `partial` | लेखकबाट आएको एकल स्ट्रिम गरिएको टोकन ({ text: "..." } समावेश) |
| `editor` | सम्पादकको फैसला तयार छ |
| `error` | केही गडबड भयो |

![ब्राउजरमा सन्देश प्रकार मार्गनिर्देशन](../../../images/part12-message-types.svg)

![स्ट्रिमिङ अनुक्रम — ब्राउजरदेखि ब्याकएन्ड संचार](../../../images/part12-streaming-sequence.svg)

---

## पूर्वापेक्षाहरू

- [भाग ७: Zava Creative Writer](part7-zava-creative-writer.md) पूरा गर्नुभएको हुनुपर्छ
- Foundry Local CLI इन्स्टल गरिएको र `phi-3.5-mini` मोडल डाउनलोड गरिएको छ
- आधुनिक वेब ब्राउजर (Chrome, Edge, Firefox, वा Safari)

---

## साझा UI

कुनै पनि ब्याकएन्ड कोड छुनुअघि, सबै तीन भाषा ट्र्याकहरूले प्रयोग गर्ने फ्रन्ट एन्ड अन्वेषण गर्न केही समय निकाल्नुहोस्। फाइलहरू `zava-creative-writer-local/ui/` मा छन्:

| फाइल | उद्देश्य |
|------|---------|
| `index.html` | पृष्ठ ल्याउट: इनपुट फारम, एजेन्ट स्थिति ब्याजहरू, लेख आउटपुट क्षेत्र, खोल्न सकिने विवरण प्यानलहरू |
| `style.css` | स्थिति-ब्याज रंग अवस्थाहरू सहित न्यूनतम स्टाइलिङ (पर्खिरहेको, चलिरहेको, सम्पन्न, त्रुटि) |
| `app.js` | Fetch कल, `ReadableStream` लाइन रिडर, र DOM अपडेट लजिक |

> **सुझाव:** `index.html` सिधै आफ्नो ब्राउजरमा खोल्नुहोस् लेआउट पूर्वावलोकन गर्न। केही काम गर्ने छैन किनभने ब्याकएन्ड छैन, तर संरचना देख्न सकिन्छ।

### स्ट्रिम रिडर कसरी काम गर्छ

`app.js` मा मुख्य फङ्सनले जवाफ बडीलाई टुक्रा टुक्रा पढ्छ र नयाँ लाइन सीमाहरूमा विभाजन गर्छ:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // अपूर्ण ट्रेलिङ लाइन राख्नुहोस्

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

हरेक पार्स गरिएको सन्देश `handleMessage()` मा पठाइन्छ, जसले `msg.type` अनुसार सम्बन्धित DOM तत्व अपडेट गर्छ।

---

## अभ्यासहरू

### अभ्यास १: Python ब्याकएन्ड UI सँग चलाउनुहोस्

Python (FastAPI) भेरियन्टमा स्ट्रिमिङ API अन्तबिन्दु पहिले नै छ। एक मात्र परिवर्तन `ui/` फोल्डरलाई स्थिर फाइलको रूपमा माउन्ट गर्नु हो।

**१.१** Python API डिरेक्टरीमा जानुहोस् र निर्भरता इन्स्टल गर्नुहोस्:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**१.२** सर्भर सुरु गर्नुहोस्:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**१.३** आफ्नो ब्राउजरमा `http://localhost:8000` खोल्नुहोस्। तपाईंले तीनवटा टेक्स्ट फिल्ड र "Generate Article" बटन सहितको Zava Creative Writer UI देख्नु पर्नेछ।

**१.४** डिफल्ट मानहरू प्रयोग गरेर **Generate Article** क्लिक गर्नुहोस्। एजेन्ट स्थिति ब्याजहरू "Waiting" देखि "Running" र "Done" मा परिवर्तन हुँदै जाँदा हेरिरहनुहोस्, र लेखको पाठ टोकनद्वारा टोकन आउटपुट प्यानलमा स्ट्रिम हुँदै जाँदैछ।

> **समस्या समाधान:** यदि पृष्ठले UI को सट्टा JSON प्रतिक्रिया देखाउँछ भने निश्चित गर्नुहोस् कि अपडेट गरिएको `main.py` चलाइरहनुभएको छ जुन स्थिर फाइलहरू माउन्ट गर्छ। `/api/article` अन्तबिन्दु अझै पनि मूल पथमा काम गर्छ; स्थिर फाइल माउन्टले अन्य सबै रुटहरूमा UI सेवा गर्छ।

**यसले कसरी काम गर्छ:** अपडेट गरिएको `main.py` तल एउटा लाइन थप्छ:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

यसले `zava-creative-writer-local/ui/` मा भएको प्रत्येक फाइललाई स्थिर सम्पत्ति रूपमा सेवा गर्छ, `index.html` लाई पूर्वनिर्धारित कागजातको रूपमा। `/api/article` POST रुट स्थिर माउन्ट अघि रजिस्टर भएकोले यसलाई प्राथमिकता दिइन्छ।

---

### अभ्यास २: JavaScript भेरियन्टमा वेब सर्भर थप्नुहोस्

JavaScript भेरियन्ट हाल CLI एप्लिकेशन (`main.mjs`) हो। एउटा नयाँ फाइल, `server.mjs`, HTTP सर्भर पछाडि एउटै एजेन्ट मोड्युलहरू र्याप गर्छ र साझा UI सेवा गर्छ।

**२.१** JavaScript डिरेक्टरीमा जानुहोस् र निर्भरता इन्स्टल गर्नुहोस्:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**२.२** वेब सर्भर सुरु गर्नुहोस्:

```bash
node server.mjs
```

```powershell
node server.mjs
```

तपाईंले देख्नु पर्नेछ:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**२.३** आफ्नो ब्राउजरमा `http://localhost:3000` खोल्नुहोस् र **Generate Article** क्लिक गर्नुहोस्। एउटै UI JavaScript ब्याकएन्डसँग ठीकसँग काम गर्छ।

**कोड अध्ययन गर्नुहोस्:** `server.mjs` खोल्नुहोस् र मुख्य ढाँचाहरू नोट गर्नुहोस्:

- **स्थिर फाइल सेवा:** Node.js बिल्ट-इन `http`, `fs`, र `path` मोड्युलहरू प्रयोग गर्दै कुनै बाह्य फ्रेमवर्क आवश्यक छैन।
- **पथ-ट्राभर्सल सुरक्षा:** अनुरोध गरिएको पथलाई सामान्य बनाउँछ र यो `ui/` डिरेक्टरी भित्र छ भनी प्रमाणित गर्छ।
- **NDJSON स्ट्रिमिङ:** `sendLine()` हेल्पर प्रयोग गर्छ जुन प्रत्येक वस्तुलाई सिरियलाइज गर्छ, भित्रका नयाँ लाइनहरू हटाउँछ र ट्रेलिङ नयाँ लाइन थप्छ।
- **एजेन्ट अॉर्केस्ट्रेसन:** अवस्थित `researcher.mjs`, `product.mjs`, `writer.mjs`, र `editor.mjs` मोड्युलहरू बिना कुनै परिवर्तन पुन: प्रयोग गर्छ।

<details>
<summary>server.mjs बाट प्रमुख अंश</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// अनुसन्धानकर्ता
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// लेखक (स्ट्रिमिङ)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### अभ्यास ३: C# भेरियन्टमा मिनिमल API थप्नुहोस्

C# भेरियन्ट हाल कन्सोल एप्लिकेशन हो। नयाँ प्रोजेक्ट, `csharp-web`, ASP.NET Core मिनिमल API हरू प्रयोग गरेर एउटै पाइपलाइन वेब सेवा रूपमा एक्स्पोज गर्छ।

**३.१** C# वेब प्रोजेक्टमा जानुहोस् र प्याकेजहरू रिस्टोर गर्नुहोस्:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**३.२** वेब सर्भर चलाउनुहोस्:

```bash
dotnet run
```

```powershell
dotnet run
```

तपाईंले देख्नु पर्नेछ:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**३.३** आफ्नो ब्राउजरमा `http://localhost:5000` खोल्नुहोस् र **Generate Article** क्लिक गर्नुहोस्।

**कोड अध्ययन गर्नुहोस्:** `csharp-web` डिरेक्टरीमा रहेको `Program.cs` खोल्नुहोस् र नोट गर्नुहोस्:

- प्रोजेक्ट फाइलले `Microsoft.NET.Sdk.Web` प्रयोग गर्छ जुन ASP.NET Core सपोर्ट थप्छ, `Microsoft.NET.Sdk` को सट्टा।
- स्थिर फाइलहरू `UseDefaultFiles` र `UseStaticFiles` मार्फत साझा `ui/` डिरेक्टरीमा सेवा गरिन्छ।
- `/api/article` अन्तबिन्दुले NDJSON लाइनहरू `HttpContext.Response` मा सिधै लेख्छ र प्रत्येक लाइन पछि फ्लush गर्छ वास्तविक-समय स्ट्रिमिङका लागि।
- सबै एजेन्ट लॉजिक (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) कन्सोल संस्करणसँग समान छ।

<details>
<summary>csharp-web/Program.cs बाट प्रमुख अंश</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### अभ्यास ४: एजेन्ट स्थिति ब्याजहरू अन्वेषण गर्नुहोस्

अब तपाईंलाई UI काम गरिरहेको छ, हेर्नुहोस् फ्रन्ट एन्डले स्थिति ब्याजहरू कसरी अपडेट गर्छ।

**४.१** `zava-creative-writer-local/ui/app.js` तपाईंको सम्पादकमा खोल्नुहोस्।

**४.२** `handleMessage()` फङ्सन फेला पार्नुहोस्। कसरि यो सन्देश प्रकारलाई DOM अपडेटसँग मिलाउँछ हेर्नुहोस्:

| सन्देश प्रकार | UI कार्य |
|-------------|-----------|
| "researcher" समावेश गरेको `message` | Researcher ब्याजलाई "Running" बनाउँछ |
| `researcher` | Researcher ब्याजलाई "Done" बनाउँछ र Research Results प्यानल भर्छ |
| `marketing` | Product Search ब्याजलाई "Done" बनाउँछ र Product Matches प्यानल भर्छ |
| `writer` सँग `data.start` | Writer ब्याजलाई "Running" बनाउँछ र लेख आउटपुट खाली गर्छ |
| `partial` | टोकन पाठ लेख आउटपुटमा थप्छ |
| `writer` सँग `data.complete` | Writer ब्याजलाई "Done" बनाउँछ |
| `editor` | Editor ब्याजलाई "Done" बनाउँछ र Editor Feedback प्यानल भर्छ |

**४.३** लेखको तल खुल्न सक्ने "Research Results", "Product Matches", र "Editor Feedback" प्यानलहरू खोलेर प्रत्येक एजेन्टले उत्पादन गरेको कच्चा JSON जाँच गर्नुहोस्।

---

### अभ्यास ५: UI अनुकूलन गर्नुहोस् (विस्तार)

यी मध्ये कुनै एक वा एकभन्दा बढी सुधार प्रयास गर्नुहोस्:

**५.१ शब्द गणना थप्नुहोस्।** लेखकले समाप्ति पछि, आउटपुट प्यानल मुनि लेखको शब्द गणना देखाउनुहोस्। `handleMessage` मा `type === "writer"` र `data.complete` सत्य हुँदा यो गणना गर्न सकिन्छ:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**५.२ पुन: प्रयास संकेतक थप्नुहोस्।** सम्पादकले संशोधन माग्दा पाइपलाइन पुन: चल्छ। स्थिति प्यानलमा "Revision 1" वा "Revision 2" ब्यानर देखाउनुहोस्। `message` प्रकार सुन्नुहोस् जुन "Revision" समावेश गर्छ र नयाँ DOM तत्व अपडेट गर्नुहोस्।

**५.३ डार्क मोड।** टगल बटन र `<body>` मा `.dark` क्लास थप्नुहोस्। `style.css` मा `body.dark` चयनकर्ताका साथ पृष्ठभूमि, पाठ, र प्यानल रंगहरू ओभरराइड गर्नुहोस्।

---

## सारांश

| तपाईंले के गर्नुभयो | कसरी |
|-------------|-----|
| Python ब्याकएन्डबाट UI सेवा गर्नुभयो | FastAPI मा `StaticFiles` सँग `ui/` फोल्डर माउन्ट गरेर |
| JavaScript भेरियन्टमा HTTP सर्भर थप्नुभयो | बिल्ट-इन Node.js `http` मोड्युल प्रयोग गरी `server.mjs` बनाएर |
| C# भेरियन्टमा वेब API थप्नुभयो | ASP.NET Core मिनिमल API हरू प्रयोग गर्दै नयाँ `csharp-web` प्रोजेक्ट बनाएर |
| ब्राउजरमा स्ट्रिमिङ NDJSON उपभोग गर्नुभयो | `fetch()` सँग `ReadableStream` र लाइन-द्वारा-लाइन JSON पार्सिङ प्रयोग गरेर |
| UI वास्तविक समयमा अपडेट गर्नुभयो | सन्देश प्रकारहरूलाई DOM अपडेटहरूसँग मिलाएर (ब्याजहरू, पाठ, खोल्न सकिने प्यानलहरू) |

---

## मुख्य सिकाइहरू

1. एउटा **साझा स्थिर फ्रन्ट एन्ड** कुनै पनि ब्याकएन्डसँग काम गर्न सक्छ जुन एउटै स्ट्रिमिङ प्रोटोकल बोल्छ, OpenAI-अनुकूल API ढाँचाको मूल्यलाई बलियो बनाउँदै।
2. **नयाँ लाइनले छुट्याइएको JSON (NDJSON)** एक सरल स्ट्रिमिङ ढाँचा हो जुन ब्राउजर `ReadableStream` API सँग स्वाभाविक रूपमा काम गर्छ।
3. **Python भेरियन्ट** लाई सबैभन्दा कम परिवर्तन आवश्यक थियो किनभने यसमा पहिले नै FastAPI अन्तबिन्दु थियो; JavaScript र C# भेरियन्टलाई पातलो HTTP र्यापर चाहिन्थ्यो।
4. UI लाई **भानिला HTML/CSS/JS** मा राख्नुले बिल्ड टुलहरू, फ्रेमवर्क निर्भरता, र अधिक जटिलता टार्दछ, कार्यशाला सिक्नेहरूका लागि सजिलो।
5. एउटै एजेन्ट मोड्युलहरू (Researcher, Product, Writer, Editor) बिना परिवर्तन पुन: प्रयोग गरिन्छ; मात्र ट्रान्सपोर्ट लेयर परिवर्तन हुन्छ।

---

## थप पढ्न

| स्रोत | लिंक |
|----------|------|
| MDN: Readable Streams प्रयोग गर्दै | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI स्थिर फाइलहरू | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core स्थिर फाइलहरू | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON विनिर्देश | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

यस कार्यशालामा तपाईंले बनाएका सबै कुरा सारांशका लागि [भाग १३: कार्यशाला समाप्त](part13-workshop-complete.md) मा जानुहोस्।

---
[← भाग ११: उपकरण कलिंग](part11-tool-calling.md) | [भाग १३: कार्यशाला पूरा →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
यो दस्तावेज एआई अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरेर अनुवाद गरिएको हो। हामी शुद्धताको प्रयास गर्छौं भने पनि, कृपया जानकार हुनुहोस् कि स्वचालित अनुवादमा त्रुटिहरू वा अशुद्धता हुन सक्छ। मूल दस्तावेज यसको मूल भाषामा मान्य स्रोतको रूपमा मुल्यांकन गर्नुपर्छ। महत्वपूर्ण जानकारीका लागि, व्यावसायिक मानवीय अनुवाद सिफारिस गरिन्छ। यस अनुवादको प्रयोगबाट उत्पन्न कुनै पनि गलतफहमी वा गलत व्याख्याका लागि हामी जिम्मेवार छैनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->