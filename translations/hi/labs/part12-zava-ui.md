![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 12: ज़ावा क्रिएटिव राइटर के लिए वेब UI बनाना

> **लक्ष्य:** ज़ावा क्रिएटिव राइटर में एक ब्राउज़र-आधारित फ्रंट एंड जोड़ें ताकि आप मल्टी-एजेंट पाइपलाइन को रियल टाइम में देख सकें, जीवित एजेंट स्टेटस बैजेस और स्ट्रीम किए गए लेख पाठ के साथ, सभी एक स्थानीय वेब सर्वर से सेवा प्रदत्त।

[भाग 7](part7-zava-creative-writer.md) में आपने ज़ावा क्रिएटिव राइटर को एक **CLI एप्लिकेशन** (JavaScript, C#) और एक **हेडलैस API** (Python) के रूप में एक्सप्लोर किया था। इस लैब में आप एक साझा **वनिला HTML/CSS/JavaScript** फ्रंट एंड को प्रत्येक बैकएंड से कनेक्ट करेंगे ताकि उपयोगकर्ता टर्मिनल के बजाय ब्राउज़र के माध्यम से पाइपलाइन के साथ इंटरैक्ट कर सकें।

---

## आप क्या सीखेंगे

| उद्देश्य | विवरण |
|-----------|-------------|
| बैकएंड से स्थैतिक फाइलें सर्व करना | अपनी API रूट के साथ HTML/CSS/JS डायरेक्टरी माउंट करना |
| ब्राउज़र में स्ट्रीमिंग NDJSON का उपयोग | `ReadableStream` के साथ Fetch API का उपयोग कर नई लाइन से विभाजित JSON पढ़ना |
| एकीकृत स्ट्रीमिंग प्रोटोकॉल | सुनिश्चित करना कि Python, JavaScript, और C# बैकएंड एक ही संदेश प्रारूप जारी करें |
| प्रोग्रेसिव UI अपडेट्स | एजेंट स्टेटस बैजेस अपडेट करना और लेख पाठ टोकन-दर-टोकन स्ट्रीम करना |
| CLI ऐप में HTTP लेयर जोड़ना | Express-शैली के सर्वर (JS) या ASP.NET Core मिनिमल API (C#) में मौजूदा ऑर्केस्ट्रेटर लॉजिक को रैप करना |

---

## वास्तुकला

UI एक सेट स्थैतिक फाइलों (`index.html`, `style.css`, `app.js`) का है जो तीनों बैकएंड के द्वारा साझा की जाती हैं। प्रत्येक बैकएंड दो समान रूट प्रदर्शित करता है:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| रूट | मेथड | उद्देश्य |
|-------|--------|---------|
| `/` | GET | स्थैतिक UI सर्व करता है |
| `/api/article` | POST | मल्टी-एजेंट पाइपलाइन चलाता है और NDJSON स्ट्रीम करता है |

फ्रंट एंड एक JSON बॉडी भेजता है और प्रतिक्रिया को नई लाइन से विभाजित JSON संदेशों की स्ट्रीम के रूप में पढ़ता है। प्रत्येक संदेश में एक `type` फ़ील्ड होता है जिसे UI सही पैनल अपडेट करने के लिए उपयोग करता है:

| संदेश प्रकार | अर्थ |
|-------------|---------|
| `message` | स्टेटस अपडेट (जैसे "शोधकर्ता एजेंट कार्य शुरू हो रहा है...") |
| `researcher` | शोध परिणाम तैयार हैं |
| `marketing` | उत्पाद खोज परिणाम तैयार हैं |
| `writer` | लेखक शुरू हुआ या समाप्त (जिसमें `{ start: true }` या `{ complete: true }` होता है) |
| `partial` | लेखक से एक स्ट्रीम्ड टोकन (जिसमें `{ text: "..." }` होता है) |
| `editor` | संपादक निर्णय तैयार है |
| `error` | कुछ गलत हुआ |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## पूर्वापेक्षित योग्यताएँ

- [भाग 7: ज़ावा क्रिएटिव राइटर](part7-zava-creative-writer.md) पूरा करें
- Foundry Local CLI स्थापित और `phi-3.5-mini` मॉडल डाउनलोड किया हुआ हो
- एक आधुनिक वेब ब्राउज़र (Chrome, Edge, Firefox, या Safari)

---

## साझा UI

किसी भी बैकएंड को छूने से पहले, उस फ्रंट एंड को एक्सप्लोर करने के लिए एक पल लें जिसे तीनों भाषा ट्रैक उपयोग करेंगे। फाइलें `zava-creative-writer-local/ui/` में रहती हैं:

| फ़ाइल | उद्देश्य |
|------|---------|
| `index.html` | पेज लेआउट: इनपुट फ़ॉर्म, एजेंट स्टेटस बैजेस, लेख आउटपुट क्षेत्र, संक्षिप्त विवरण पैनल |
| `style.css` | न्यूनतम स्टाइलिंग जिसमें स्टेटस-बैज रंग अवस्थाएँ (इंतजार, चल रहा, हुआ, त्रुटि) |
| `app.js` | Fetch कॉल, `ReadableStream` लाइन रीडर, और DOM अपडेट लॉजिक |

> **टिप:** `index.html` को सीधे अपने ब्राउज़र में खोलें ताकि लेआउट का पूर्वावलोकन कर सकें। अभी कुछ काम नहीं करेगा क्योंकि कोई बैकएंड नहीं है, लेकिन आप संरचना देख सकते हैं।

### स्ट्रीम रीडर कैसे काम करता है

`app.js` में प्रमुख फ़ंक्शन प्रतिक्रिया बॉडी को टुकड़ों में पढ़ता है और नई लाइनों पर विभाजित करता है:

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
    buffer = lines.pop(); // अधूरा समाप्त होने वाला लाइन रखें

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

प्रत्येक पार्स किए गए संदेश को `handleMessage()` को भेजा जाता है, जो `msg.type` के आधार पर संबंधित DOM तत्व अपडेट करता है।

---

## अभ्यास

### अभ्यास 1: UI के साथ Python बैकएंड चलाएं

Python (FastAPI) संस्करण के पास पहले से ही एक स्ट्रीमिंग API एंडपॉइंट है। केवल परिवर्तन `ui/` फोल्डर को स्थैतिक फाइलों के रूप में माउंट करना है।

**1.1** Python API डायरेक्टरी पर जाएं और निर्भरताएं इंस्टॉल करें:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** सर्वर शुरू करें:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** अपने ब्राउज़र में `http://localhost:8000` खोलें। आपको ज़ावा क्रिएटिव राइटर UI तीन टेक्स्ट फ़ील्ड्स और "Generate Article" बटन के साथ दिखना चाहिए।

**1.4** डिफ़ॉल्ट मानों के साथ **Generate Article** क्लिक करें। देखें कि कैसे एजेंट स्टेटस बैजेस "Waiting" से "Running" और फिर "Done" में बदलते हैं क्योंकि प्रत्येक एजेंट अपना कार्य पूरा करता है, और लेख पाठ आउटपुट पैनल में टोकन-दर-टोकन स्ट्रीम होता है।

> **समस्या निवारण:** यदि पेज UI की जगह JSON प्रतिक्रिया दिखाता है, तो सुनिश्चित करें कि आप अपडेटेड `main.py` चला रहे हैं जो स्थैतिक फाइलों को माउंट करता है। `/api/article` एंडपॉइंट अपने मूल पथ पर अभी भी काम करता है; स्थैतिक फाइल माउंट बाकी सभी रूटों पर UI सर्व करता है।

**यह कैसे काम करता है:** अपडेटेड `main.py` के नीचे एक ही लाइन जोड़ता है:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

यह हर फाइल को `zava-creative-writer-local/ui/` से एक स्थैतिक संसाधन के रूप में सर्व करता है, जहां `index.html` डिफ़ॉल्ट दस्तावेज़ है। `/api/article` POST रूट स्थैतिक माउंट से पहले रजिस्टर होता है, इसलिए उसे प्राथमिकता मिलती है।

---

### अभ्यास 2: JavaScript संस्करण में वेब सर्वर जोड़ें

JavaScript संस्करण फिलहाल एक CLI एप्लिकेशन (`main.mjs`) है। एक नई फाइल, `server.mjs`, HTTP सर्वर के पीछे उसी एजेंट मॉड्यूल को रैप करती है और साझा UI सर्व करती है।

**2.1** JavaScript डायरेक्टरी पर जाएं और निर्भरताएं इंस्टॉल करें:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** वेब सर्वर शुरू करें:

```bash
node server.mjs
```

```powershell
node server.mjs
```

आपको यह देखना चाहिए:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** अपने ब्राउज़र में `http://localhost:3000` खोलें और **Generate Article** क्लिक करें। वही UI JavaScript बैकएंड पर समान रूप से काम करता है।

**कोड पढ़ें:** `server.mjs` खोलें और प्रमुख पैटर्न देखें:

- **स्थैतिक फाइल सर्विंग** Node.js के अंतर्निहित `http`, `fs`, और `path` मॉड्यूल का उपयोग करता है, बिना किसी बाहरी फ्रेमवर्क के।
- **पथ-यात्रा सुरक्षा** अनुरोधित पथ को सामान्यीकृत करता है और सत्यापित करता है कि वह `ui/` डायरेक्टरी के भीतर ही रहता है।
- **NDJSON स्ट्रीमिंग** एक `sendLine()` हेल्पर का उपयोग करता है जो प्रत्येक ऑब्जेक्ट को सीरियलाइज़ करता है, आंतरिक नई लाइनों को हटाता है, और एक अंतिम नई लाइन जोड़ता है।
- **एजेंट ऑर्केस्ट्रेशन** बिना बदलाव के मौजूदा `researcher.mjs`, `product.mjs`, `writer.mjs`, और `editor.mjs` मॉड्यूल का पुन: उपयोग करता है।

<details>
<summary>server.mjs से प्रमुख अंश</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// शोधकर्ता
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// लेखक (स्ट्रीमिंग)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### अभ्यास 3: C# संस्करण में मिनिमल API जोड़ें

C# संस्करण फिलहाल एक कंसोल एप्लिकेशन है। एक नया प्रोजेक्ट, `csharp-web`, ASP.NET Core मिनिमल APIs का उपयोग करता है ताकि समान पाइपलाइन वेब सेवा के रूप में उपलब्ध हो सके।

**3.1** C# वेब प्रोजेक्ट पर जाएं और पैकेज रिस्टोर करें:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** वेब सर्वर चलाएं:

```bash
dotnet run
```

```powershell
dotnet run
```

आपको यह देखना चाहिए:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** अपने ब्राउज़र में `http://localhost:5000` खोलें और **Generate Article** क्लिक करें।

**कोड पढ़ें:** `csharp-web` डायरेक्टरी में `Program.cs` खोलें और देखें:

- प्रोजेक्ट फाइल `Microsoft.NET.Sdk.Web` का उपयोग करती है बजाय `Microsoft.NET.Sdk` के, जो ASP.NET Core सपोर्ट जोड़ता है।
- स्थैतिक फाइलें `UseDefaultFiles` और `UseStaticFiles` के माध्यम से साझा `ui/` डायरेक्टरी को पॉइंट करते हुए सर्व होती हैं।
- `/api/article` एंडपॉइंट सीधे NDJSON लाइनों को `HttpContext.Response` में लिखता है और प्रत्येक लाइन के बाद फ्लश करता है ताकि रियल टाइम स्ट्रीमिंग हो सके।
- सभी एजेंट लॉजिक (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) कंसोल संस्करण के समान हैं।

<details>
<summary>csharp-web/Program.cs से प्रमुख अंश</summary>

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

### अभ्यास 4: एजेंट स्टेटस बैजेस का अन्वेषण करें

अब जब आपके पास कार्यशील UI है, देखें कि फ्रंट एंड स्टेटस बैजेस कैसे अपडेट करता है।

**4.1** `zava-creative-writer-local/ui/app.js` अपने संपादक में खोलें।

**4.2** `handleMessage()` फ़ंक्शन खोजें। ध्यान दें कि यह संदेश प्रकारों को DOM अपडेट्स से कैसे मैप करता है:

| संदेश प्रकार | UI क्रिया |
|-------------|-----------|
| `"researcher"` शब्द वाला `message` | रिसर्चर बैज को "Running" सेट करता है |
| `researcher` | रिसर्चर बैज को "Done" सेट करता है और Research Results पैनल भरता है |
| `marketing` | Product Search बैज को "Done" सेट करता है और Product Matches पैनल भरता है |
| `data.start` के साथ `writer` | लेखक बैज को "Running" सेट करता है और लेख आउटपुट साफ़ करता है |
| `partial` | लेख आउटपुट में टोकन टेक्स्ट जोड़ता है |
| `data.complete` के साथ `writer` | लेखक बैज को "Done" सेट करता है |
| `editor` | संपादक बैज को "Done" सेट करता है और Editor Feedback पैनल भरता है |

**4.3** लेख के नीचे संक्षिप्त "Research Results", "Product Matches", और "Editor Feedback" पैनलों को खोलें और प्रत्येक एजेंट द्वारा उत्पन्न कच्चा JSON निरीक्षण करें।

---

### अभ्यास 5: UI को अनुकूलित करें (विकास)

इनमें से एक या अधिक सुधारों को आज़माएं:

**5.1 शब्द गणना जोड़ें।** जब लेखक समाप्त हो जाए, तो आउटपुट पैनल के नीचे लेख में शब्दों की संख्या दिखाएं। आप इसे `handleMessage` में तब गणना कर सकते हैं जब `type === "writer"` और `data.complete` सत्य हो:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 पुनः प्रयास संकेतक जोड़ें।** जब संपादक संशोधन का अनुरोध करता है, तो पाइपलाइन पुनः चलती है। स्टेटस पैनल में "Revision 1" या "Revision 2" बैनर दिखाएं। एक `message` प्रकार सुनें जिसमें "Revision" हो और एक नया DOM एलिमेंट अपडेट करें।

**5.3 डार्क मोड।** एक टॉगल बटन और `<body>` को `.dark` क्लास जोड़ें। `style.css` में `body.dark` सेलेक्टर के साथ पृष्ठभूमि, टेक्स्ट, और पैनल रंगों को ओवरराइड करें।

---

## सारांश

| आपने क्या किया | कैसे |
|-------------|-----|
| Python बैकएंड से UI सर्व किया | FastAPI में `StaticFiles` के साथ `ui/` फोल्डर माउंट किया |
| JavaScript संस्करण में HTTP सर्वर जोड़ा | Node.js के बिल्ट-इन `http` मॉड्यूल का उपयोग करते हुए `server.mjs` बनाया |
| C# संस्करण में वेब API जोड़ा | ASP.NET Core मिनिमल APIs के साथ एक नया `csharp-web` प्रोजेक्ट बनाया |
| ब्राउज़र में स्ट्रीमिंग NDJSON का उपयोग किया | `fetch()` के साथ `ReadableStream` और लाइन-दर-लाइन JSON पार्सिंग का उपयोग किया |
| रियल टाइम में UI अपडेट किया | संदेश प्रकारों को DOM अपडेट्स (बैजेस, टेक्स्ट, संक्षिप्त पैनल) से मैप किया |

---

## मुख्य निष्कर्ष

1. एक **साझा स्थैतिक फ्रंट एंड** किसी भी बैकएंड के साथ काम कर सकता है जो समान स्ट्रीमिंग प्रोटोकॉल बोलता है, जो OpenAI-अनुकूल API पैटर्न के मूल्य को मजबूत करता है।
2. **नई लाइन-डिलीमिटेड JSON (NDJSON)** एक सरल स्ट्रीमिंग प्रारूप है जो ब्राउज़र के `ReadableStream` API के साथ स्वाभाविक रूप से काम करता है।
3. **Python संस्करण** को सबसे कम बदलाव की जरूरत थी क्योंकि उसमे पहले से FastAPI एंडपॉइंट था; JavaScript और C# संस्करणों को एक पतली HTTP रैपर की जरूरत थी।
4. UI को **वनिला HTML/CSS/JS** जैसे रखना बिल्ड टूल्स, फ्रेमवर्क निर्भरताओं और वर्कशॉप सीखने वालों के लिए अतिरिक्त जटिलता से बचाता है।
5. समान एजेंट मॉड्यूल (Researcher, Product, Writer, Editor) बिना संशोधन के पुन: उपयोग किए जाते हैं; केवल ट्रांसपोर्ट लेयर बदलती है।

---

## आगे पढ़ें

| संसाधन | लिंक |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON विनिर्देश | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

इस कार्यशाला में आपने जो कुछ भी बनाया है उसका सारांश देखने के लिए [भाग 13: कार्यशाला पूर्ण](part13-workshop-complete.md) पर जाएं।

---
[← भाग 11: टूल कॉलिंग](part11-tool-calling.md) | [भाग 13: कार्यशाला पूर्ण →](part13-workshop-complete.md)