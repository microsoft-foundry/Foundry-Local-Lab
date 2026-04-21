![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 12: झावा क्रिएटिव रायटरसाठी वेब यूआय तयार करणे

> **उद्दिष्ट:** झावा क्रिएटिव रायटरसाठी ब्राउझर-आधारित फ्रंट एंड जोडा जेणेकरून आपण मल्टी-एजंट पाईपलाइन रिअल टाइममध्ये पाहू शकता, लाईव्ह एजंट स्टेटस बॅजेस आणि स्ट्रीमिंग आर्टिकल टेक्स्टसह, हे सगळे एका स्थानिक वेब सर्व्हरवरून सर्व्ह केले जाईल.

[भाग 7](part7-zava-creative-writer.md) मध्ये आपण झावा क्रिएटिव रायटरचा शोध घेतला होता म्हणून तो एक **CLI अनुप्रयोग** (JavaScript, C#) आणि एक **हेडलस API** (Python) होता. या प्रयोगशाळेत आपण एक सामायिक **वेनिला HTML/CSS/JavaScript** फ्रंट एंड प्रत्येक बॅकएंडशी कनेक्ट कराल जेणेकरून वापरकर्ते टर्मिनलऐवजी ब्राउझरद्वारे पाईपलाइनशी संवाद साधू शकतील.

---

## आपण काय शिकाल

| उद्दिष्ट | वर्णन |
|-----------|-------------|
| बॅकएंडमधून स्थिर फाइल्स सेवा देणे | आपल्या API मार्गासोबत HTML/CSS/JS निर्देशिका माउंट करा |
| ब्राउझरमध्ये स्ट्रीमिंग NDJSON वापरणे | `ReadableStream` सह Fetch API वापरून न्यूलाइन-डिलीमिटेड JSON वाचा |
| एकसंध स्ट्रीमिंग प्रोटोकॉल | Python, JavaScript, आणि C# बॅकएंड्स एकसारखे संदेश स्वरूप प्रसारित करतात याची खात्री करा |
| प्रगत UI अपडेट्स | एजंट स्टेटस बॅजेस अपडेट करा आणि आर्टिकल टेक्स्ट टोकननिहाय स्ट्रीम करा |
| CLI अॅपसाठी HTTP लेयर जोडा | विद्यमान ऑर्केस्ट्रेटर लॉजिकला Express-शैलीतील सर्व्हर (JS) किंवा ASP.NET Core मिनिमल API (C#) मध्ये रॅप करा |

---

## आर्किटेक्चर

UI ही एका सेटमध्ये असलेल्या स्थिर फाइल्स (`index.html`, `style.css`, `app.js`) ची आहे जी तीनही बॅकएंडद्वारे सामायिक केली जाते. प्रत्येक बॅकएंड एकसारखे दोन मार्ग उघडतो:

![Zava UI आर्किटेक्चर — तीन बॅकएंडसह सामायिक फ्रंट एंड](../../../images/part12-architecture.svg)

| मार्ग | पद्धत | उद्दिष्ट |
|-------|--------|---------|
| `/` | GET | स्थिर UI सेवा करा |
| `/api/article` | POST | मल्टी-एजंट पाईपलाइन चालवा आणि NDJSON स्ट्रीम करा |

फ्रंट एंड JSON बॉडी पाठवतो आणि प्रतिसाद न्यूलाइन-डिलीमिटेड JSON संदेशांच्या स्ट्रीम स्वरूपात वाचतो. प्रत्येक संदेशात `type` क्षेत्र असते ज्याचा UI योग्य पॅनेल अपडेट करण्यासाठी वापर करतो:

| संदेश प्रकार | अर्थ |
|-------------|---------|
| `message` | स्टेटस अपडेट (उदा. "रिसर्चर एजंट टास्क सुरू करत आहे...") |
| `researcher` | संशोधन निकाल तयार आहेत |
| `marketing` | उत्पादन शोध निकाल तयार आहेत |
| `writer` | रायटरने सुरुवात केली किंवा पूर्ण केली आहे (`{ start: true }` किंवा `{ complete: true }` समाविष्ट आहे) |
| `partial` | रायटरकडून एकाच वेळी आलेला टोकन ( `{ text: "..." }` समाविष्ट आहे) |
| `editor` | संपादक निर्णय तयार आहे |
| `error` | काहीतरी चूक झाली आहे |

![ब्राउझरमधील संदेश प्रकार रूटिंग](../../../images/part12-message-types.svg)

![स्ट्रीमिंग सिक्वेन्स — ब्राउझर ते बॅकएंड कम्युनिकेशन](../../../images/part12-streaming-sequence.svg)

---

## पूर्वअट

- पूर्ण [भाग 7: झावा क्रिएटिव रायटर](part7-zava-creative-writer.md)
- Foundry Local CLI इंस्टॉल आणि `phi-3.5-mini` मॉडेल डाउनलोड केलेले
- एक आधुनिक वेब ब्राउझर (Chrome, Edge, Firefox, किंवा Safari)

---

## सामायिक UI

कोणत्याही बॅकएंड कोडला आवर्जून न स्पर्श करता, इतिहास द्या की सर्व तीन भाषा ट्रॅक्स वापरणार्‍या फ्रंट एंडचा अनुभव घ्या. फाइल्स `zava-creative-writer-local/ui/` या फोल्डरमध्ये असतात:

| फाइल | उद्दिष्ट |
|------|---------|
| `index.html` | पेज लेआउट: इनपुट फॉर्म, एजंट स्टेटस बॅजेस, आर्टिकल आउटपुट क्षेत्र, कॉलेप्सिबल तपशील पॅनल |
| `style.css` | स्टेटस-बॅज रंग स्थितींसह (प्रतीक्षा, चालू, पूर्ण, चूक) किमान स्टाइलिंग |
| `app.js` | Fetch कॉल, `ReadableStream` लाइन वाचक, आणि DOM अपडेट लॉजिक |

> **टीप:** `index.html` थेट आपल्या ब्राउझरमध्ये उघडा आणि लेआउटचा प्रिव्ह्यू पाहा. अजून काहीही कार्यान्वित होणार नाही कारण बॅकएंड नाही, पण रचना पाहू शकता.

### स्ट्रीम रीडर कसा काम करतो

`app.js` मधील मुख्य फंक्शन प्रतिसाद बॉडी एका तुकड्याप्रमाणे वाचते आणि नवीन ओळीवर विभागते:

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
    buffer = lines.pop(); // अपूर्ण शेवटची ओळ ठेवा

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

प्रत्येक पार्स केलेल्या संदेशाला `handleMessage()` कडे पाठवले जाते, जे msg.type नुसार संबंधित DOM घटक अपडेट करते.

---

## प्रयोग

### प्रयोग 1: UI सह Python बॅकएंड चालवा

Python (FastAPI) पर्यायात आधीच एक स्ट्रीमिंग API एंडपॉइंट आहे. फक्त `ui/` फोल्डर स्थिर फाइल्स प्रमाणे माउंट करायचा बदल आहे.

**1.1** Python API निर्देशिकेत जा आणि अवलंबित्वे इंस्टॉल करा:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** सर्व्हर सुरू करा:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** आपला ब्राउझर `http://localhost:8000` येथे उघडा. झावा क्रिएटिव रायटर UI तीन मजकूर फील्डसह आणि "Generate Article" बटणासह दिसेल.

**1.4** पूर्वनिर्धारित मूल्यांसह **Generate Article** क्लिक करा. आपण "Waiting" वरून "Running" आणि "Done" पर्यंत एजंट स्टेटस बॅजेस कसे बदलत आहेत ते पहा, तसेच आर्टिकल टेक्स्ट आउटपुट पॅनेलमध्ये टोकननिहाय स्ट्रीम होत आहे.

> **समस्या निराकरण:** जर पृष्ठ UI ऐवजी JSON प्रतिसाद दाखवत असेल तर खात्री करा की आपण अपडेटेड `main.py` चालवत आहात जे स्थिर फाइल्स माउंट करते. `/api/article` एंडपॉइंट त्याच्या मूळ मार्गावर अजूनही कार्यरत आहे; स्थिर फाइल माउंट प्रत्येक इतर मार्गावर UI सेवा देतो.

**काम कसे करते:** अपडेटेड `main.py` च्या खालील ओळ जोडते:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

हे `zava-creative-writer-local/ui/` मधील प्रत्येक फाइल स्थिर फायलींसाठी सर्व्ह करते, ज्यामध्ये `index.html` डिफॉल्ट दस्तऐवज आहे. `/api/article` POST मार्ग स्थिर माउंटच्या आधी नोंदणीकृत आहे, त्यामुळे त्याला प्राधान्य आहे.

---

### प्रयोग 2: JavaScript पर्यायासाठी वेब सर्व्हर जोडा

JavaScript पर्याय सध्या CLI अॅपलीकेशन (`main.mjs`) आहे. एक नवीन फाइल `server.mjs` त्याच एजंट मॉड्यूल्सना HTTP सर्व्हरमागे रॅप करते आणि सामायिक UI सेवा देते.

**2.1** JavaScript निर्देशिकेत जा आणि अवलंबित्वे इंस्टॉल करा:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** वेब सर्व्हर सुरू करा:

```bash
node server.mjs
```

```powershell
node server.mjs
```

आपल्याला खालील दिसेल:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** ब्राउझरमध्ये `http://localhost:3000` उघडा आणि **Generate Article** क्लिक करा. आताचील UI जावास्क्रिप्ट बॅकएंडवर समान प्रकारे कार्य करतो.

**कोड अभ्यासा:** `server.mjs` उघडा आणि मुख्य नमुने लक्षात ठेवा:

- **स्थिर फाइल सेवा:** Node.js मध्ये अंतर्भूत `http`, `fs`, आणि `path` मॉड्यूल्स वापरून कोणत्याही बाह्य फ्रेमवर्कशिवाय.
- **पथ-प्रवेश संरक्षण:** विनंती केलेले पथ सामान्यीकृत करते आणि `ui/` निर्देशिकेत टिकते का ते तपासते.
- **NDJSON स्ट्रीमिंग:** `sendLine()` हेल्परसह प्रत्येक ऑब्जेक्ट सीरियलाइज करतो, अंतर्गत नवीन ओळी काढून टाकतो, आणि नंतर ट्रेलिंग नवीन ओळ जोडतो.
- **एजंट ऑर्केस्ट्रेशन:** विद्यमान `researcher.mjs`, `product.mjs`, `writer.mjs`, आणि `editor.mjs` मॉड्यूल्स बदल न करता पुन्हा वापरतो.

<details>
<summary>server.mjs मधून मुख्य उतारा</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// संशोधक
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// लेखक (प्रवाहित)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### प्रयोग 3: C# पर्यायासाठी मिनिमल API जोडा

C# पर्याय सध्या एक कन्सोल अॅप्लिकेशन आहे. एक नवीन प्रोजेक्ट `csharp-web` ASP.NET Core मिनिमल APIs वापरून वेब सेवा म्हणून समान पाईपलाइन उघडतो.

**3.1** C# वेब प्रोजेक्टमध्ये जा आणि पॅकेजेस पुनर्स्थित करा:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** वेब सर्व्हर चालवा:

```bash
dotnet run
```

```powershell
dotnet run
```

आपल्याला हे दिसेल:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** ब्राउझरमध्ये `http://localhost:5000` उघडा आणि **Generate Article** क्लिक करा.

**कोड अभ्यासा:** `csharp-web` निर्देशिकेतील `Program.cs` उघडा आणि लक्षात ठेवा:

- प्रोजेक्ट फाइल `Microsoft.NET.Sdk` ऐवजी `Microsoft.NET.Sdk.Web` वापरते ज्यामुळे ASP.NET Core समर्थन मिळते.
- स्थिर फाइल्स `UseDefaultFiles` आणि `UseStaticFiles` वापरून सामायिक `ui/` निर्देशिकेकडे निर्देशित केल्या जातात.
- `/api/article` एंडपॉइंट NDJSON ओळी थेट `HttpContext.Response` कडे लिहितो आणि प्रत्येक ओळीनंतर फ्लशल करतो, म्हणून रिअल-टाइम स्ट्रीमिंग शक्य होते.
- सर्व एजंट लॉजिक (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) कन्सोल आवृत्तीप्रमाणे समान आहे.

<details>
<summary>csharp-web/Program.cs मधून मुख्य उतारा</summary>

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

### प्रयोग 4: एजंट स्टेटस बॅजेस तपासा

आता आपल्याकडे कार्यरत UI आहे, पहा की फ्रंट एंड एजंट स्टेटस बॅजेस कसे अपडेट करते.

**4.1** `zava-creative-writer-local/ui/app.js` आपल्या संपादकात उघडा.

**4.2** `handleMessage()` फंक्शन शोधा. लक्षात घ्या की ते संदेश प्रकार DOM अपडेट्सशी कसे जुळवते:

| संदेश प्रकार | UI क्रिया |
|-------------|-----------|
| `message` ज्यात "researcher" आहे | रिसर्चर बॅज "Running" मध्ये सेट करते |
| `researcher` | रिसर्चर बॅज "Done" करते आणि रिसर्च निकाल पॅनेल भरते |
| `marketing` | प्रॉडक्ट सर्च बॅज "Done" करते आणि प्रॉडक्ट मॅचेस पॅनेल भरते |
| `writer` ज्यात `data.start` आहे | रायटर बॅज "Running" करतो आणि आर्टिकल आउटपुट रिकामी करतो |
| `partial` | आर्टिकल आउटपुटमध्ये टोकन टेक्स्ट जोडतो |
| `writer` ज्यात `data.complete` आहे | रायटर बॅज "Done" करतो |
| `editor` | संपादक बॅज "Done" करतो आणि संपादक अभिप्राय पॅनेल भरतो |

**4.3** आर्टिकलच्या खालील "Research Results", "Product Matches", आणि "Editor Feedback" कॉलेप्सिबल पॅनेल उघडा आणि प्रत्येक एजंटने तयार केलेला रॉ JSON तपासा.

---

### प्रयोग 5: UI सानुकूलित करा (विस्तार)

खालील सुधारणा एक किंवा अधिक प्रयत्न करा:

**5.1 शब्द मोजणी जोडा.** रायटर संपल्यानंतर, आउटपुट पॅनेलखालील लेखमगदूर शब्द मोजणी दाखवा. हे `handleMessage` मध्ये `type === "writer"` आणि `data.complete` खरे असताना गणू शकता:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 पुनःप्रयत्न निर्देशक जोडा.** संपादकाने बदल मागितल्यावर, पाईपलाइन पुन्हा चालू होते. स्टेटस पॅनेलमध्ये "Revision 1" किंवा "Revision 2" बॅनर दाखवा. "Revision" हा मजकूर असलेला `message` प्रकार ऐका आणि नवीन DOM घटक अपडेट करा.

**5.3 डार्क मोड.** एका टॉगल बटण आणि `<body>` ला `.dark` वर्ग जोडा. `style.css` मध्ये `body.dark` निवडकर्ता वापरून पार्श्वभूमी, मजकूर, आणि पॅनेल रंग ओव्हरराइड करा.

---

## सारांश

| आपण काय केले | कसे |
|-------------|-----|
| UI Python बॅकएंडमधून सर्व्ह केली | FastAPI मध्ये `StaticFiles` वापरून `ui/` फोल्डर माउंट केला |
| JavaScript पर्यायाला HTTP सर्व्हर जोडला | Node.js अंतर्भूत `http` मॉड्यूल वापरून `server.mjs` तयार केला |
| C# पर्यायाला वेब API जोडले | ASP.NET Core मिनिमल API सह नवीन `csharp-web` प्रोजेक्ट तयार केला |
| ब्राउझरमध्ये स्ट्रीमिंग NDJSON वापरले | `fetch()` सह `ReadableStream` आणि ओळी-प्रति-ओळ JSON पार्सिंग वापरली |
| UI वास्तविक वेळेत अपडेट केली | संदेश प्रकारांसाठी DOM अपडेट्स नकाशित केले (बॅजेस, टेक्स्ट, कॉलेप्सिबल पॅनेल) |

---

## मुख्य शिका

1. एक **सामायिक स्थिर फ्रंट एंड** कोणत्याही बॅकएंडबरोबर काम करू शकतो जो समान स्ट्रीमिंग प्रोटोकॉल वापरतो, जे OpenAI-युक्त API पॅटर्नचे मूल्यवान सिद्ध करते.
2. **न्यूलाइन-डिलीमिटेड JSON (NDJSON)** हा एक सोपा स्ट्रीमिंग फॉर्मॅट आहे जो ब्राउझरच्या `ReadableStream` API सोबत नैसर्गिकरित्या कार्य करतो.
3. **Python पर्यायाला** सर्वात कमी बदल लागले कारण त्याकडे आधीच FastAPI एंडपॉइंट होता; JavaScript आणि C# पर्यायांना HTTP रॅपरची गरज होती.
4. UI ला **वेनिला HTML/CSS/JS** स्वरूपात ठेवल्याने बिल्ड टूल्स, फ्रेमवर्क अवलंबित्व, आणि अधिक गुंतागुंतीपासून कार्यशाळेतील शिकणाऱ्यांना बचाव मिळतो.
5. समान एजंट मॉड्यूल्स (Researcher, Product, Writer, Editor) कोणत्याही बदलांशिवाय पुन्हा वापरले जातात; फक्त ट्रान्सपोर्ट लेयर बदलतो.

---

## पुढील वाचन

| संदर्भ | दुवा |
|----------|------|
| MDN: Readable Streams वापरणे | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI स्थिर फाइल्स | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core स्थिर फाइल्स | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON तपशीलवार | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

संपूर्ण कार्यशाळा आपल्याला काय तयार केले त्याचा सारांश पाहण्यासाठी पुढे जा [भाग 13: कार्यशाळा पूर्ण](part13-workshop-complete.md).

---
[← भाग ११: साधन कॉलिंग](part11-tool-calling.md) | [भाग १३: कार्यशाळा पूर्ण →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
हा दस्तऐवज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) चा वापर करून अनुवादित केला आहे. आम्ही अचूकतेसाठी प्रयत्न करतो, परंतु कृपया लक्षात ठेवा की स्वयंचलित अनुवादांमध्ये चुका किंवा गैरसमज होऊ शकतात. मूळ दस्तऐवज त्याच्या स्थानिक भाषेमध्ये अधिकृत स्त्रोत मानला जावा. अत्यंत महत्त्वाच्या माहितीसाठी व्यावसायिक मानवी अनुवाद शिफारसीय आहे. या अनुवादाच्या वापरामुळे उद्भवणाऱ्या कोणत्याही गैरसमजुती किंवा अप्रत्यक्ष अर्थासाठी आम्ही जबाबदार नाही.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->