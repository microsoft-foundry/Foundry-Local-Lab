![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# భాగం 12: Zava Creative Writer కోసం వెబ్ UI నిర్మాణం

> **లక్ష్యం:** Zava Creative Writerకి బ్రౌజర్-ఆధారిత ఫ్రంట్ ఎండ్ ని జోడించడం, తద్వారా మీరు మల్టీ-ఏజెంట్ పైప్‌లైన్ ను రియల్ టైమ్ లో వాచ్ చేయగలరు, లైవ్ ఏజెంట్ స్టేటస్ బ్యాడ్జిలు మరియు స్ట్రీమ్డ్ వ్యాసం టెక్ట్స్ సహా, ఇవన్నీ ఒకే స్థానిక వెబ్ సర్వర్ నుండి సేవ చేయబడతాయి.

[భాగం 7](part7-zava-creative-writer.md)లో మీరు Zava Creative Writerను **CLI యాప్** (JavaScript, C#) మరియు **హెడ్లెస్ API** (Python)గా అన్వేషించారు. ఈ ప్రయోగంలో మీరు ప్రతీ బ్యాక్ ఎండ్ కి ఒక భాగస్వామ్య **వనిల్లా HTML/CSS/JavaScript** ఫ్రంట్ ఎండ్ ని అనుసంధానం చేస్తారు, తద్వారా వినియోగదారులు టెర్మినల్ ద్వారమో కాకుండా బ్రౌజర్ ద్వారా పైప్‌లైన్‌తో ఇంటరాక్ట్ చేయగలరు.

---

## మీరు నేర్చుకునేది ఏమిటి

| లక్ష్యం | వివరణ |
|-----------|-------------|
| బ్యాక్ ఎండ్స్ నుండి స్టాటిక్ ఫైళ్ళను సర్వ్ చేయడం | మీ API రూట్ పక్కన HTML/CSS/JS డైరెక్టరీని మౌంట్ చేయడం |
| బ్రౌజర్‌లో NDJSON స్ట్రీమింగ్ కొన్స్యూమ్ చేయడం | `ReadableStream` సహా Fetch API ఉపయోగించి newline-డిలిమిటెడ్ JSON చదవడం |
| ఏకీకృత స్ట్రీమింగ్ ప్రోటోకాల్ | Python, JavaScript, మరియు C# బ్యాక్ ఎండ్స్ ఒకే సందేశ ఫార్మాట్ ఇవ్వడం నిర్ధారించడం |
| ప్రోగ్రెసివ్ UI అప్‌డేట్లు | ఏజెంట్ స్టేటస్ బ్యాడ్జిలు అప్డేట్ చేయడం మరియు వ్యాసంలోని టెక్ట్స్ టోక్‌న్లవారీగా స్ట్రీమ్ చేయడం |
| CLI యాప్‌కు HTTP లేయర్ జోడించడం | ఉన్న ఒర్కెస్ట్రేటర్ లాజిక్‌ను Express-శైలి సర్వర్ (JS) లేదా ASP.NET కోర్ మినిమల్ API (C#)లో ర్యాప్ చేయడం |

---

## శిల్ప నిర్మాణం

UI అనేది మూడు బ్యాక్ ఎండ్స్‌కు కౌశల్యంగా ఉపయోగించే ఒకే స్టాటిక్ ఫైళ్ళ సముదాయం (`index.html`, `style.css`, `app.js`). ప్రతి బ్యాక్ ఎండ్ అదే రెండు రూట్లను ఎక్స్‌పోజ్ చేస్తాయి:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| రూట్ | మెథడ్ | ప్రయోజనం |
|-------|--------|---------|
| `/` | GET | స్టాటిక్ UI ను సర్వ్ చేస్తుంది |
| `/api/article` | POST | మల్టీ-ఏజెంట్ పైప్‌లైన్ ను నడిపించి NDJSON స్ట్రీమ్ చేస్తుంది |

ఫ్రంట్ ఎండ్ JSON బాడీని పంపించి, newline-డిలిమిటెడ్ JSON సందేశాల స్ట్రీమ్ ని చదువుతుంది. ప్రతీ సందేశం `type` ఫీల్డ్ కలిగి ఉంటుంది, ఇది UIకి సరైన ప్యానెల్ అప్‌డేట్ చేయడానికి ఉపయోగపడుతుంది:

| సందేశం రకం | అర్థం |
|-------------|---------|
| `message` | స్టేటస్ అప్‌డేట్ (ఉదా: "Starting researcher agent task...") |
| `researcher` | పరిశోధన ఫలితాలు సిద్ధమయ్యాయి |
| `marketing` | ఉత్పత్తి శోధన ఫలితాలు సిద్ధమయ్యాయి |
| `writer` | రచయిత ప్రారంభించబడింది లేదా పూర్తయింది (`{ start: true }` లేదా `{ complete: true }` ఉన్నవి) |
| `partial` | రచయిత నుంచి ఒక స్ట్రీమ్ అయిన టోకెన్ (`{ text: "..." }`) |
| `editor` | ఎడిటర్ నిర్ణయం సిద్ధమైంది |
| `error` | ఏదో తప్పు జరిగింది |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## మరింత మీకు కావలసినవి

- పూర్తి చేయండి [Part 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI ఇన్స్టాల్ చేసి `phi-3.5-mini` మోడల్ డౌన్లోడ్ చేసుకోండి
- ఆధునిక వెబ్ బ్రౌజర్ (Chrome, Edge, Firefox, లేదా Safari)

---

## భాగస్వామ్య UI

ఏ బ్యాక్ ఎండ్ కోడ్‌ను టచ్ చేయక ముందే, మూడు భాష ట్రాక్స్ ద్వారా ఉపయోగించబడే ఫ్రంట్ ఎండ్‌ను పరిశీలించండి. ఫైర్లు `zava-creative-writer-local/ui/` లో ఉంటాయి:

| ఫైల్ | ప్రయోజనం |
|------|---------|
| `index.html` | పేజీ లేఅవుట్: ఇన్పుట్ ఫారం, ఏజెంట్ స్టేటస్ బ్యాడ్జిలు, వ్యాసం అవుట్‌పుట్ ప్రాంతం, కాల్ప్సిబుల్ వివరాలు ప్యానెల్లు |
| `style.css` | స్టేటస్ బ్యాడ్జ్ రంగుల రాష్ట్రాలతో మినిమల్ స్టైలింగ్ (వేటింగ్, రన్నింగ్, డన్, ఎర్రర్) |
| `app.js` | Fetch కాల్, `ReadableStream` లైన్ రీడర్ మరియు DOM అప్‌డేట్ లాజిక్ |

> **సలహా:** మీ బ్రౌజర్‌లో నేరుగా `index.html` ను ఓపెన్ చేసి లేఅవుట్ ప్రివ్యూ చేయండి. బ్యాక్ ఎండ్ లేకపోవడంతో ఏదీ పని చేయదు, కానీ సవరణను చూడవచ్చు.

### స్ట్రీమ్ రీడర్ ఎలా పనిచేస్తుంది

`app.js` లో కీలక ఫంక్షన్ ప్రతీ బఫర్ భాగాన్ని చదివి, న్యూలైన్ బౌండ్రీలపై విభజిస్తుంది:

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
    buffer = lines.pop(); // అసంపూర్ణ ముగింపు లైన్‌ను ఉంచండి

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
  
ప్రతీ పార్స్ చేసిన సందేశాన్ని `handleMessage()` కి పంపించి, అది `msg.type` ఆధారంగా సంబంధిత DOM ఎలిమెంట్లను అప్‌డేట్ చేస్తుంది.

---

## అభ్యాసాలు

### అభ్యాసం 1: Python బ్యాక్ ఎండ్ UIతో నడపండి

Python (FastAPI) వేరియంట్ ఇప్పటికే స్ట్రీమింగ్ API ఎండ్పాయింట్ కలిగి ఉంది. ఒక్క మార్పు `ui/` ఫోల్డర్‌ను స్టాటిక్ ఫైళ్లుగా మౌంట్ చేయడం.

**1.1** Python API డైరెక్టరీకు వెళ్లి డిపెండెన్సీలు ఇన్స్టాల్ చేయండి:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```
  
```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```
  
**1.2** సర్వర్ ప్రారంభించండి:

```bash
uvicorn main:app --reload --port 8000
```
  
```powershell
uvicorn main:app --reload --port 8000
```
  
**1.3** బ్రౌజర్‌లో `http://localhost:8000` ఓపెన్ చేయండి. మీరు మూడు టెక్ట్స్ ఫీల్డ్లు మరియు "Generate Article" బటన్‌తో Zava Creative Writer UI చూడగలరు.

**1.4** డిఫాల్ట్ విలువలు ఉపయోగించి **Generate Article** పై క్లిక్ చేయండి. ఏజెంట్లు తమ పనిని పూర్తి చేసినట్లుగా "Waiting" నుండి "Running" తరువాత "Done" గా స్టేటస్ బ్యాడ్జిలు మారుతున్నవి చూడండి, మరియు ప్యానెల్లో వ్యాసం టోకెన్ వరుసగా స్ట్రీమ్ అవుతున్నది గమనించండి.

> **పరిష్కార సూచనలు:** UI బదులు JSON రిస్పాన్స్ వస్తే, స్టాటిక్ ఫైల్స్ మౌంట్ చేసిన నవీకరించిన `main.py` నడుపుతున్నారో లేదో నిర్ధారించుకోండి. `/api/article` ఎండ్పాయింట్ మరింతగా అదే మార్గంలో పనిచేస్తుంది; స్టాటిక్ ఫైల్ మౌంట్ UIని ఇతర రూట్లలో సర్వ్ చేస్తుంది.

**ఇది ఎలా పనిచేస్తుంది:** నవీకరించిన `main.py` చివర ఒకే ఒక్క లైన్ జోడిస్తుంది:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```
  
ఇది `zava-creative-writer-local/ui/` నుండి ప్రతి ఫైల్‌ను స్టాటిక్ అసెట్‌గా సర్వ్ చేస్తుంది, `index.html` డిఫాల్ట్ డాక్యుమెంట్. `/api/article` POST రూట్ స్టాటిక్ మౌంట్ కంటే ముందుగా నమోదు చేయబడింది కనుక ప్రాధాన్యం ఉంటుంది.

---

### అభ్యాసం 2: JavaScript వేరియంట్‌కు వెబ్ సర్వర్ జోడించండి

JavaScript వేరియంట్ ప్రస్తుతానికి CLI యాప్ (`main.mjs`). కొత్త ఫైల్ `server.mjs` అదే ఏజెంట్ మాడ్యూల్స్‌ను HTTP సర్వర్ వెనుక ర్యాప్ చేస్తూ, భాగస్వామ్య UIని సర్వ్ చేస్తుంది.

**2.1** JavaScript డైరెక్టరీకి వెళ్లి డిపెండెన్సీలు ఇన్స్టాల్ చేయండి:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```
  
```powershell
cd zava-creative-writer-local\src\javascript
npm install
```
  
**2.2** వెబ్ సర్వర్ ప్రారంభించండి:

```bash
node server.mjs
```
  
```powershell
node server.mjs
```
  
ఈ రిజల్ట్ చూడగలరు:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```
  
**2.3** బ్రౌజర్‌లో `http://localhost:3000` ఓపెన్ చేసి **Generate Article** క్లిక్ చేయండి. అదే UI JavaScript బ్యాక్ ఎండ్‌కు సమానంగా పని చేస్తుంది.

**కోడ్ అధ్యయనం:** `server.mjs` ఓపెన్ చేసి కీలక ప్యాటర్న్లు గమనించండి:

- **స్టాటిక్ ఫైల్ సర్వ్** కోసం Node.js లో బిల్ట్-ఇన్ `http`, `fs`, `path` మాడ్యూల్స్ ఉపయోగించి ఎలాంటి బాహ్య ఫ్రేమ్‌వర్క్ అవసరం లేదు.
- **పাথে-ట్రావర్సల్ రక్షణ** ఆపిల్ చేసిన పాథ్‌ను నార్మలైజ్ చేసి అది `ui/` డైరెక్టరీలోనే ఉందో లేదో చెక్ చేస్తుంది.
- **NDJSON స్ట్రీమింగ్** కోసం `sendLine()` అనే హెల్పర్ ఉపయోగించి ప్రతి ఆబ్జెక్టును సీరియలైజ్ చేసి, అంతర్గత న్యూలైన్లను తొలగించి, చివరలో న్యూలైన్ జోడిస్తుంది.
- **ఏజెంట్ ఆర్కెస్ట్రేషన్** కోసం `researcher.mjs`, `product.mjs`, `writer.mjs`, `editor.mjs` మాడ్యూల్స్ అవ్యవహార మార్పుల లేకుండా పునర్వినియోగం చేస్తుంది.

<details>
<summary>server.mjs నుండి కీలక భాగం</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// పరిశోధకుడు
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// రచయిత (స్ట్రీమింగ్)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### అభ్యాసం 3: C# వేరియంట్‌కు మినిమల్ API చేర్చండి

C# వేరియంట్ ప్రస్తుతానికి కన్‌సోల్ యాప్. కొత్త ప్రాజెక్ట్ `csharp-web` ASP.NET కోర్ మినిమల్ APIs ఉపయోగించి అదే పైప్‌లైన్‌ను వెబ్ సేవగా ఎక్స్‌పోజ్ చేస్తుంది.

**3.1** C# వెబ్ ప్రాజెక్ట్‌కు వెళ్లి ప్యాకేజీలను రిస్టోర్ చేయండి:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```
  
```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```
  
**3.2** వెబ్ సర్వర్ నడపండి:

```bash
dotnet run
```
  
```powershell
dotnet run
```
  
ఈ ఫలితం చూడగలరు:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```
  
**3.3** బ్రౌజర్‌లో `http://localhost:5000` ఓపెన్ చేసి **Generate Article** పై క్లిక్ చేయండి.

**కోడ్ అధ్యయనం:** `csharp-web` డైరెక్టరీలో `Program.cs` ఓపెన్ చేసి గమనించండి:

- ప్రాజెక్ట్ ఫైల్ `Microsoft.NET.Sdk` కాకుండా `Microsoft.NET.Sdk.Web` ను ఉపయోగిస్తుంది, ASP.NET కోర్ సపోర్టు కోసం.
- స్టాటిక్ ఫైళ్లు `UseDefaultFiles` మరియు `UseStaticFiles` ద్వారా భాగస్వామ్య `ui/` డైరెక్టరీకి పాయింట్ చేస్తుంది.
- `/api/article` ఎండ్పాయింట్ NDJSON లైన్లను నేరుగా `HttpContext.Response`కి రాయడం మరియు ప్రతి లైన్ అనంతరం ఫ్లష్ చేయడం ద్వారా రియల్ టైమ్ స్ట్రీమింగ్ అందిస్తుంది.
- అన్ని ఏజెంట్ లాజిక్ (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) కన్‌సోల్ వెర్షన్ లాగా అదే ఉన్నాయి.

<details>
<summary>csharp-web/Program.cs నుండి కీలక భాగం</summary>

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

### అభ్యాసం 4: ఏజెంట్ స్టేటస్ బ్యాడ్జిలను పరిశీలించండి

ప్రస్తుతం పనిచేసే UI ఉన్నందున, ఫ్రంట్ ఎండ్ ఏజెంట్ స్టేటస్ బ్యాడ్జిలను ఎలా అప్‌డేట్ చేస్తుందో చూడండి.

**4.1** `zava-creative-writer-local/ui/app.js` ఫైల్‌ను ఎడిటర్‌లో తెరవండి.

**4.2** `handleMessage()` ఫంక్షన్ కనుగొనండి. సందేశ రకాలను DOM అప్‌డేట్లతో ఎలా మ్యాప్ చేస్తున్నదీ గమనించండి:

| సందేశ రకం | UI చర్య |
|-------------|-----------|
| `message` లో "researcher" ఉంటే | Researcher బ్యాడ్జ్ ను "Running" గా సెట్ చేస్తుంది |
| `researcher` | Researcher బ్యాడ్జ్ ను "Done" గా సెట్ చేసి, Research Results ప్యానెల్లో ఫలితాలు నింపుతుంది |
| `marketing` | Product Search బ్యాడ్జ్ ను "Done" గా సెట్ చేసి, Product Matches ప్యానెల్లో ఫలితాలు నింపుతుంది |
| `writer` లో `data.start` ఉంటే | Writer బ్యాడ్జ్ ను "Running" గా మార్చి వ్యాస అవుట్‌పుట్ క్లియర్ చేస్తుంది |
| `partial` | వ్యాస అవుట్‌పుట్ లో టోకెన్ టెక్ట్స్ ను జతచేస్తుంది |
| `writer` లో `data.complete` ఉంటే | Writer బ్యాడ్జ్ ను "Done" గా సెట్ చేస్తుంది |
| `editor` | Editor బ్యాడ్జ్ ను "Done" గా సెట్ చేసి Editor Feedback ప్యానెల్లో ఫలితాలు నింపిస్తుంది |

**4.3** వ్యాసం దిగువన ఉన్న కాల్ప్సిబుల్ "Research Results", "Product Matches", మరియు "Editor Feedback" ప్యానెల్స్ ను తెరవండి మరియు ప్రతీ ఏజెంట్ ఉత్పత్తి చేసిన ముడి JSON పరిశీలించండి.

---

### అభ్యాసం 5: UIని కస్టమైజ్ చేయండి (విస్తరణ)

ఈ అభివృద్ధులను ఒకటి లేదా అంతకంటే ఎక్కువ ప్రయత్నించండి:

**5.1 పదాల లెక్క జోడించండి.** రచయిత పని పూర్తయ్యాక, అవుట్‌పుట్ ప్యానెల్ కింద వ్యాస పదాల సంఖ్యను ప్రదర్శించండి. `handleMessage` లో `type === "writer"` మరియు `data.complete` నిజమైతే మీరు దీన్ని లెక్కించవచ్చు:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```
  
**5.2 రిత్రై సూచిక జోడించండి.** ఎడిటర్ రివిజన్ కోరినప్పుడు పైప్‌లైన్ మళ్లీ నడుస్తుంది. స్టేటస్ ప్యానెల్లో "Revision 1" లేదా "Revision 2" బ్యానర్ చూపించండి. "Revision" ఉన్న `message` రకం కోసం వినండి మరియు కొత్త DOM ఎలిమెంట్ను అప్‌డేట్ చేయండి.

**5.3 డార్క్ మోడ్.** ఒక టోగుల్ బటన్ మరియు `<body>`లో `.dark` క్లాస్ జోడించండి. `style.css` లో `body.dark` సెలెక్టర్‌తో బ్యాక్‌గ్రౌండ్, టెక్ట్స్, మరియు ప్యానెల్ రంగులను ఒవర్‌రైడ్ చేయండి.

---

## సారాంశం

| మీరు ఏం చేశారో | ఎలా |
|-------------|-----|
| Python బ్యాక్ ఎండ్ నుండి UIకి సేవ్ చేశారు | FastAPIలో `StaticFiles` తో `ui/` ఫోల్డర్ మౌంట్ చేశారు |
| JavaScript వేరియంట్‌కు HTTP సర్వర్ జోడించారు | Node.js బిల్ట్-ఇన్ `http` మాడ్యూల్ ఉపయోగించి `server.mjs` సృష్టించారు |
| C# వేరియంట్‌కు వెబ్ API జోడించారు | ASP.NET కోర్ మినిమల్ APIs తో కొత్త `csharp-web` ప్రాజెక్ట్ సృష్టించారు |
| బ్రౌజర్‌లో NDJSON స్ట్రీమింగ్ వినియోగించారు | `fetch()` తో `ReadableStream` మరియు లైన్-బై-లైన్ JSON పార్సింగ్ ఉపయోగించారు |
| UIని రియల్ టైమ్‌లో అప్‌డేట్ చేశారు | సందేశ రకాలను DOM అప్‌డేట్లకు మ్యాప్ చేశారు (బ్యాడ్జీలు, టెక్ట్స్, కాల్ప్సిబుల్ ప్యానెల్స్) |

---

## ముఖ్యమైన పాఠాలు

1. **భాగస్వామ్య స్టాటిక్ ఫ్రంట్ ఎండ్** ఏ బ్యాక్ ఎండ్ తోనైనా పని చేయవచ్చు, అదే స్ట్రీమింగ్ ప్రోటోకాల్ ఉండాలి, ఇది OpenAI-అనుకూల API ప్యాటర్న్ విలువను బలపరుస్తుంది.
2. **Newline-డిలిమిటెడ్ JSON (NDJSON)** బ్రౌజర్ `ReadableStream` APIతో సహజంగానే పనిచేసే సులభమైన స్ట్రీమింగ్ ఫార్మాట్.
3. **Python వేరియంట్**కి తక్కువ మార్పులు అవసరమైనవి కావున అది ఇప్పటికే FastAPI ఎండ్పాయింట్ కలిగి ఉంది; JavaScript మరియు C# వేరియంట్‌లకు తక్కువ HTTP ర్యాపర్ అవసరం.
4. UIను **వనిల్లా HTML/CSS/JS** గా ఉంచడం వలన బిల్డ్ టూల్స్, ఫ్రేమ్‌వర్క్ ఆధారాల లేమి, మరియు వర్క్షాప్ నేర్చుకునేవారికి అదనపు క్లిష్టతలు తగ్గినవి.
5. Researcher, Product, Writer, Editor అనే ఏజెంట్ మాడ్యూల్స్ మార్పుల లేకుండా పునర్వినియోగం, కేవలం ట్రాన్స్‌పోర్ట్ లేయర్ మాత్రమే మారింది.

---

## మరింత చదవండి

| వనరు | లింక్ |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

అంతర్జాలంలో మీరు ఈ వర్క్షాప్ లో నిర్మించిన అన్ని అంశాల సారాంశం కోసం [Part 13: Workshop Complete](part13-workshop-complete.md) కి కొనసాగండి.

---
[← భాగం 11: సాధనం పిలుపు](part11-tool-calling.md) | [భాగం 13: కార్యశాల పూర్తయింది →](part13-workshop-complete.md)