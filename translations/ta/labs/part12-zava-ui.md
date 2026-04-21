![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# பகுதி 12: Zava Creative Writer க்கான வலை UI ஒன்றை உருவாக்குதல்

> **நோக்கம்:** பல முகவர்கள் இயக்கும் குழாய்தாவலை நேரடியாக பின்தொடர உங்கள் உலாவியில் காண்பதற்கான முன்னணி இடைமுகத்தை Zava Creative Writerக்கு சேர்ப்பது, உடனடி முகவர் நிலை படிகைகள் மற்றும் ஒளிபரப்பும் கட்டுரை உரையை ஒரே உள்ளூர் வலை சேவையகத்தில் வழங்குவது.

[பகுதி 7](part7-zava-creative-writer.md)ல் நீங்கள் Zava Creative Writerஐ **CLI பயன்பாடு** (JavaScript, C#) மற்றும் **ஹெட்லெஸ் API** (Python) ஆக ஆராய்ந்தீர்கள். இந்த கலப்பகுதியில், பயனர்கள் ஒரு டெர்மினலுக்கு பதிலாக உலாவியைப் பயன்படுத்தி குழாய்தாவலை தொடர்புகொள்ள, அனைத்து பின்னணி அமர்த்தியுள்ள **வனிலா HTML/CSS/JavaScript** முன்னணி இடைமுகத்தை இணைப்பீர்கள்.

---

## நீங்கள் கற்றுக்கொள்ளப்போகும் விஷயங்கள்

| நோக்கம் | விளக்கம் |
|-----------|-------------|
| பின்னணி இருந்து நிலையான கோப்புகளை வழங்குதல் | உங்கள் API வழிமுறைக்கு இணையாக HTML/CSS/JS கோப்புத்தொகுதியை மவுண்ட் செய்தல் |
| உலாவியில் ஒளிபரப்பும் NDJSONஐ பயன்படுத்துதல் | பதிவேற்றத்துடன் உள்ள Fetch API-யை `ReadableStream` உடன் பயன்படுத்தி வரிசை-உரை JSONஐ படித்து உபயோகித்தல் |
| ஒருங்கிணைந்த ஒளிபரப்பு நெறிமுறை | Python, JavaScript மற்றும் C# பின்னணிகள் ஒரே செய்தி வடிவத்தில் வெளியிடுவதை உறுதி செய்தல் |
| முன்னணி இடைமுக மேம்பாடுகள் | முகவர் நிலை படிகைகளை புதுப்பித்து கட்டுரை உரையை ஒரு குறியீட்டுப்பொறி அடையாளம் மூலம் ஒளிபரப்புதல் |
| CLI செயலிக்கு HTTP அடுக்கு சேர்ப்பது | Express பாணி சேவையகம் (JS) அல்லது ASP.NET Core குறைந்தபட்ச API (C#) மூலம் உள்ளமைவுக் கட்டளைப்படி செயல்பாட்டை ஆவணப்படுத்தல் |

---

## வடிவமைப்பு

UI என்பது அனைத்து மூன்று பின்னணிகளாலும் பகிரப்படுகிறது மற்றும் ஒரே தொகுப்பு நிலையான கோப்புகளைக் கொண்டது (`index.html`, `style.css`, `app.js`). ஒவ்வொரு பின்னணியும் ஒரே இரண்டு வழிமுறைகள் காணப்படுகின்றன:

![Zava UI கட்டமைப்பு — மூன்று பின்னணிகள் உடன் பகிரப்பட்ட முன்னணி](../../../images/part12-architecture.svg)

| வழிமுறை | முறை | நோக்கம் |
|-------|--------|---------|
| `/` | GET | நிலையான UIஐ வழங்கும் |
| `/api/article` | POST | பல-முகவர் குழாய்தாவலை இயக்கி NDJSONஐ ஒளிபரப்பும் |

முன்னணி JSON உடலை அனுப்பி, ஒவ்வொரு பதிலையும் வரிசை-உரை JSON செய்திகளாக ஓதுகிறது. ஒவ்வொரு செய்தியிலும் UI சரியான பலகையை புதுப்பிக்க பயன்படும் `type` புலம் உள்ளது:

| செய்தி வகை | பொருள் |
|-------------|---------|
| `message` | நிலை மேம்பாடு (எ.கா. "ஆராய்ச்சி முகவர் பணியைத் துவங்குகிறது...") |
| `researcher` | ஆராய்ச்சி முடிவுகள் தயார் |
| `marketing` | தயாரிப்பு தேடல் முடிவுகள் தயார் |
| `writer` | எழுத்தாளர் துவங்கியோ முடித்ததோ (உள்ளடக்கம் `{ start: true }` அல்லது `{ complete: true }`) |
| `partial` | எழுத்தாளரிடமிருந்து ஒளிபரப்பப்பட்ட தனி குறியீட்டுப்பொறி (உள்ளடக்கம் `{ text: "..." }`) |
| `editor` | தொகுப்பாளர் முடிவு தயாராக உள்ளது |
| `error` | ஏதேனும் தவறு ஏற்பட்டது |

![உலாவியில் செய்தி வகை வழிசெய்தல்](../../../images/part12-message-types.svg)

![ஒளிபரப்பு பின்னணி தொடர்பு](../../../images/part12-streaming-sequence.svg)

---

## முன் தேவைகள்

- [பகுதி 7: Zava Creative Writer](part7-zava-creative-writer.md) முடித்திருக்க வேண்டும்
- Foundry Local CLI நிறுவப்பட்டு `phi-3.5-mini` மாதிரி பதிவிறக்கப்பட்டுள்ளது
- நவீன வலை உலாவி (Chrome, Edge, Firefox, அல்லது Safari)

---

## பகிரப்பட்ட UI

எந்த பின்னணி கோடை திருத்துவதற்கு முன்பு மூன்று மொழி தடங்களும் பயன்படுத்தும் முன்னணி இடைமுகத்தைக் ஆராய ஒரு நேரம் செலவிடுங்கள். கோப்புகள் `zava-creative-writer-local/ui/` இல் உள்ளன:

| கோப்பு | நோக்கம் |
|------|---------|
| `index.html` | பக்கம் அமைப்பு: உள்ளீட்டுப் படிவம், முகவர் நிலை படிகைகள், கட்டுரை வெளியீடு பகுதி, சுருக்கக்கூடிய விரிவான பலகைகள் |
| `style.css` | நிலை படிகைகளுக்கான குறைந்தபட்ச அலங்காரம் (காத்திருக்கும், ஓடும், முடிந்த, பிழை) |
| `app.js` | Fetch அழைப்பு, `ReadableStream` வரிசை வாசிப்பான், மற்றும் DOM மேம்பாட்டு நெறிமுறை |

> **குறிப்பு:** உங்கள் உலாவியில் நேரடியாக `index.html`ஐ திறந்து அமைப்பை முன்னோட்டமாக பார்க்கலாம். பின்னணி இல்லாததால் இன்னும் செயல்படாது, ஆனால் அமைப்பு தெளிவாகும்.

### ஒளிபரப்பு வாசிப்பான் எப்படி செயல்படுகிறது

`app.js` இல் முக்கியமான செயல்பாடு பதில் உடலை துண்டு துண்டாக வாசித்து, வரிவிரிவுகள் இடையே பிரிக்கின்றது:

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
    buffer = lines.pop(); // முடிவற்ற தொடரும் வரியை வைக்கவும்

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

ஒவ்வொரு கண்காணிக்கப்பட்ட செய்தியும் `handleMessage()`க்கு அனுப்பப்படுகின்றது, இது `msg.type` அடிப்படையில் தொடர்புடைய DOM கூறை புதுப்பிக்கிறது.

---

## பயிற்சிகள்

### பயிற்சி 1: Python பின்னணியை UI உடன் இயக்கவும்

Python (FastAPI) மாறுபாடு ஏற்கனவே ஒளிபரப்பு API முடிவை கொண்டுள்ளது. மாற்றம் ஒரே ஒன்று — `ui/` கோப்புறையை நிலையான கோப்புகளாக மவுண்ட் செய்தல்.

**1.1** Python API அடைவை சென்று சார்ந்த தொகுதிகளைக் இன்ஸ்டால் செய்யவும்:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** சேவையகத்தை துவங்கவும்:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** உலாவியில் `http://localhost:8000` ஐ திறக்கவும். மூன்று உரை புலங்களும் மற்றும் "Generate Article" பொத்தானுடன் Zava Creative Writer UI காட்சியளிக்கும்.

**1.4** இயல்புநிலை மதிப்புகளை பயன்படுத்தி **Generate Article** பொத்தானை கிளிக் செய்யவும். முகவர் நிலை படிகைகள் "காத்திருப்பு" இருந்து "ஓடும்" மற்றும் "முடிந்தது" ஆக மாறும்போது கவனியுங்கள், மற்றும் கட்டுரை உரை ஒவ்வொரு குறியீட்டுப்பொறி அடையாளத்துடனும் வெளியீடு பலகையில் ஓடுகிறது.

> **சிக்கல் தீர்க்கும்:** பக்கத்தில் UI பதிலுக்குப் பதிலாக JSON பதில் காட்டப்பட்டால், நிலையான கோப்புகளை மவுண்ட் செய்கின்ற புதிய `main.py` இனை இயக்குகிறீர்களா என்று உறுதிப்படுத்துக. `/api/article` முடிவு இன்னும் அதன் அசல் பாதையில் வேலை செய்கிறது; நிலையான கோப்பு மவுண்ட் UIஐ மற்ற வழிமுறைகளில் சேமிக்கிறது.

**இது எப்படி செயல்படுகிறது:** புதிய `main.py` க்கு கீழே ஒரே வரி சேர்க்கப்பட்டுள்ளது:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

இது `zava-creative-writer-local/ui/` இல் உள்ள அனைத்து கோப்புகளையும் நிலையான சொத்து போல வழங்குகிறது, `index.html` இயல்புநிலை ஆவணமாகும். `/api/article` POST வழிமுறை நிலையான மவுண்டிங்குக்கு முன்பு பதிவு செய்யப்பட்டுள்ளது, எனவே முன்னுரிமை பெற்றுள்ளது.

---

### பயிற்சி 2: JavaScript மாறுபாட்டிற்கு வலை சேவையகத்தை சேர்க்கவும்

JavaScript மாறுபாடு தற்போது ஒரு CLI பயன்பாடு (`main.mjs`). புதிய கோப்பு `server.mjs` HTTP சேவையகம் பின்புலத்தில் அதே முகவர் தொகுதிகளை மூடியிடும் மற்றும் பகிரப்பட்ட UIஐ வழங்கும்.

**2.1** JavaScript அடைவிற்கு சென்று சார்ந்த தொகுதிகளை இன்ஸ்டால் செய்யவும்:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** வலை சேவையகத்தை துவங்கவும்:

```bash
node server.mjs
```

```powershell
node server.mjs
```

அவற்றைப் பெறுவீர்கள்:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** உலாவியில் `http://localhost:3000` ஐ திறந்து **Generate Article** அழுத்தவும். அதே UI JavaScript பின்னணியோடு வெற்றிகரமாக செயல்படும்.

**கோடுகளை ஆய்வு செய்யவும்:** `server.mjs` திறந்து முக்கிய வடிவிகளை கவனிக்கவும்:

- **நிலையான கோப்புகளை வழங்கல்** Node.js உள்ளூர் `http`, `fs`, மற்றும் `path` தொகுதிகள் மூலம் செயல் படுத்தப்படுகின்றது, ஒரு வெளிப்புற கொள்கை தேவையில்லை.
- **பாதை வழித்தடத் தடுப்பு** கேட்ட பாதையை பொருத்தமாக மாற்றி அது `ui/` கோப்புறையின் உள்ளே இருக்குமா என்பதை சரிபார்க்கின்றது.
- **NDJSON ஒளிபரப்பு** ஒவ்வொரு பொருளையும் தொடரியல் ஆவணமாக மாற்றி, உள்ளிருந்து பதிவேறல் அல்லது அதிர்வெண் நீக்கப்பட்டு வரிசை விரிவான குறுக்கு வரைபடத்தை கடந்த பிறகு `sendLine()` என்ற உதவிக் கருவி பயன்படுத்துகின்றது.
- **முகவர் ஒழுங்கமைப்பு** உள்ளமைவு இல்லாமல் மீண்டும் `researcher.mjs`, `product.mjs`, `writer.mjs`, மற்றும் `editor.mjs` தொகுதிகளை பயன்படுத்துகின்றது.

<details>
<summary>server.mjs இலிருந்து முக்கிய பகுதிகள்</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// ஆராய்ச்சியாளர்
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// எழுத்தாளர் (ஸ்ட்ரீமிங்)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### பயிற்சி 3: C# மாறுபாட்டிற்கு குறைந்தபட்ச API ஐ சேர்க்கவும்

C# மாறுபாடு தற்போது ஒரு கான்சோல் பயன்பாடு. புதிய திட்டம் `csharp-web` ASP.NET Core குறைந்தபட்ச APIகளை பயன்படுத்தி அதே குழாய்தாவலை வலை சேவையாக வெளிப்படுத்துகிறது.

**3.1** C# வலை திட்டத்துக்கு சென்று தொகுதிகள் மீட்டெடுக்கவும்:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** வலை சேவையகத்தை இயக்கவும்:

```bash
dotnet run
```

```powershell
dotnet run
```

அவற்றைப் பெறுவீர்கள்:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** உலாவியில் `http://localhost:5000` ஐ திறந்து **Generate Article** அழுத்தவும்.

**கோடுகளை ஆய்வு செய்யவும்:** `csharp-web` அடைவிலுள்ள `Program.cs` திறந்து கவனிக்கவும்:

- திட்டக் கோப்பு `Microsoft.NET.Sdk` அல்லாமல் `Microsoft.NET.Sdk.Web` ஐ பயன்படுத்துகிறது, இது ASP.NET Core ஆதரவை சேர்க்கிறது.
- நிலையான கோப்புகள் `UseDefaultFiles` மற்றும் `UseStaticFiles` வழியாக பகிரப்பட்ட `ui/` கோப்புறைக்குக் காட்டுகிறது.
- `/api/article` முடிவு நேரடி NDJSON வரிகளை `HttpContext.Response` இல் எழுதுகின்றது மற்றும் ஒவ்வொரு வரியையும் தள்ளுவித்து நேரடி ஒளிபரப்பை செயல்படுத்துகிறது.
- அனைத்து முகவர் லாஜிக்க்களும் (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) கான்சோல் பதிப்புடன் அதேவிதமாக உள்ளது.

<details>
<summary>csharp-web/Program.cs இலிருந்து முக்கிய பகுதிகள்</summary>

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

### பயிற்சி 4: முகவர் நிலை படிகைகளை ஆராயவும்

இப்போது வேலை செய்கிற UI உள்ளது, முன்னணி முகவர் நிலை படிகைகளை எப்படி மேம்படுத்துகிறது என்று பாருங்கள்.

**4.1** உங்கள் தொகுப்பியில் `zava-creative-writer-local/ui/app.js` ஐ திறக்கவும்.

**4.2** `handleMessage()` செயல்பாட்டைப் காணவும். அது செய்தி வகைகளை DOM மேம்பாடுகளுடன் எவ்வாறு இணைக்கிறது என்று கவனிக்கவும்:

| செய்தி வகை | UI நடவடிக்கை |
|-------------|-----------|
| "researcher" கொண்ட `message` | ஆராய்ச்சியாளர் படிகையை "ஓடும்" ஆக அமைக்கிறது |
| `researcher` | ஆராய்ச்சியாளர் படிகையை "முடிந்தது" ஆக செய்து ஆராய்ச்சி முடிவுகளுக்கான பலகையை நிரப்புகிறது |
| `marketing` | தயாரிப்பு தேடு படிகையை "முடிந்தது" ஆக செய்து பொருள் பொருத்தான பலகையை நிரப்புகிறது |
| `writer` மற்றும் `data.start` உள்ளது | எழுத்தாளர்படிகையை "ஓடும்" ஆக செய்து கட்டுரை வெளியீட்டை அழிக்கிறது |
| `partial` | கட்டுரை வெளியீட்டில் குறியீட்டுப் பத்டத்தை இணைக்கிறது |
| `writer` மற்றும் `data.complete` உள்ளது | எழுத்தாளர்படிகையை "முடிந்தது" ஆக அமைக்கிறது |
| `editor` | தொகுப்பாளர் படிகையை "முடிந்தது" ஆக செய்து தொகுப்பாளர் கருத்து பலகையை நிரப்புகிறது |

**4.3** கட்டுரையின் கீழே உள்ள சுருக்கக்கூடிய "ஆராய்ச்சி முடிவுகள்", "தயாரிப்பு பொருத்தங்கள்", மற்றும் "தொகுப்பாளர் கருத்து" பலகைகளைக் திறந்து ஒவ்வொரு முகவரும் உருவாக்கிய மூல JSON ஐ பரிசீலியுங்கள்.

---

### பயிற்சி 5: UI ஐ தனிப்பயனாக்கவும் (விரிவாக்கம்)

பின்வரும் மேம்படுத்தல்களில் ஒன்றோ அல்லது அதற்கு மேற்பட்டவற்றை முயற்சியுங்கள்:

**5.1 ஒரு சொல் எண்ணிக்கையைச் சேர்க்கவும்.** எழுத்தாளர் முடிந்தபின் கட்டுரை சொல் எண்ணிக்கையை வெளியீட்டு பலகையின் கீழ் காண்பிக்கவும். `handleMessage` இல் `type === "writer"` மற்றும் `data.complete` நேர்மையாக இருந்தால் இதை கணக்கிடலாம்:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 மீண்டும் முயற்சிக்கும் குறியீட்டைப் சேர்க்கவும்.** தொகுப்பாளர் திருத்தம் கோரும்போது குழாய்தாவல் மீண்டும் இயங்கும். நிலை பலகையில் "Revision 1" அல்லது "Revision 2" என்ற பட்டையை காண்பியுங்கள். "Revision" கொண்ட `message` வகையை கேட்டு புதிய DOM கூறை புதுப்பிக்கவும்.

**5.3 கருப்பு முறை (Dark mode).** மாற்றி பொத்தான் ஒன்றைச் சேர்த்து `<body>` இற்கு `.dark` வகையை விடுங்கள். பின்னணி, உரை, மற்றும் பலகை நிறத்தை `style.css` இல் `body.dark` தேர்ந்தெடுப்பில் மேம்படுத்தவும்.

---

## சுருக்கம்

| நீங்கள் செய்தது | எப்படி |
|-------------|-----|
| Python பின்னணியில் UIஐ வழங்கியது | FastAPIவில் `StaticFiles` மூலம் `ui/` கோப்புறையை மவுண்ட் செய்தல் |
| JavaScript மாறுபாட்டிற்கு HTTP சேவையகத்தை சேர்த்தது | Node.js உள்ளமைவுத் தொகுதி `http` பயன்படுத்த `server.mjs` உருவாக்கப்பட்டது |
| C# மாறுபாட்டிற்கு வலை API ஐச் சேர்த்தது | ASP.NET Core குறைந்தபட்ச APIகளை கொண்ட புதிய `csharp-web` திட்டம் |
| உலாவியில் ஒளிபரப்பும் NDJSONஐ பயன்படுத்தியது | `fetch()` உடன் `ReadableStream` மற்றும் வரி வாரியாக JSON பகுப்பாய்வு |
| நேரடியாக UI ஐ மேம்படுத்தியது | செய்தி வகைகள் மூலம் DOM மேம்பாடுகள் (படிகைகள், உரை, சுருக்கக்கூடிய பலகைகள்) வரைபடம் செய்தல் |

---

## முக்கிய அம்சங்கள்

1. **பகிரப்பட்ட நிலையான முன்னணி** எந்த பின்னணியுடனும் ஒரே ஒளிபரப்பு நெறிமுறையைக் பேசும் போது செயல்படும், OpenAI-போன்ற API வடிவமைப்பின் மதிப்பை ஊர்துகிறது.
2. **வரிசை-உரை JSON (NDJSON)** உலாவி `ReadableStream` API உடன் இயல்பான ஒளிபரப்பு வடிவமாக சிக்கலற்றது.
3. **Python மாறுபாடு** குறைவு மாற்றத்துடன் இருந்தது, ஏனெனில் அதற்கு ஏற்கனவே FastAPI முடிவு இருந்தது; JavaScript மற்றும் C# மாறுபாடுகள் அதிக HTTP துகள்களைத் தேவைப்பட்டன.
4. UI ஐ **வனிலா HTML/CSS/JS** ஆக வைத்ததன் மூலம் கட்டுமான கருவிகள், շրջանակ சார்ந்த சாரங்கள் மற்றும் கூடுதல் சிக்கல்களைத் தவிர்க்க முடிந்தது.
5. அதே முகவர் தொகுதிகள் (Researcher, Product, Writer, Editor) எந்த மாற்றமும் இல்லாமல் மீண்டும் பயன்படுத்தப்பட்டன; வெறும் கடத்தல் அடுக்கே மாறியது.

---

## மேலதிக வாசிப்பு

| வளம் | இணைப்பு |
|----------|------|
| MDN: Readable Streams பயன்படுத்துதல் | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI நிலையான கோப்புகள் | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core நிலையான கோப்புகள் | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON விவரக்குறிப்பு | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

இந்த பட்டறை முழுமையாக முடிந்த அனைத்து பரிசீலனைகளுக்கான சுருக்கத்தைப் பெற [பகுதி 13: பட்டறை முழுமை](part13-workshop-complete.md)க்கு தொடரவும்.

---
[← பகுதிப் பகுதி 11: கருவி அழைப்பு](part11-tool-calling.md) | [பகுதி 13: பணிமனை முடிந்தது →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**தயவு செய்து கவனிக்கவும்**:  
இந்த ஆவணம் [Co-op Translator](https://github.com/Azure/co-op-translator) எனும் செயற்கை நுண்ணறிவு மொழி மாற்ற சேவையை பயன்படுத்தி மொழிபெயர்க்கப்பட்டுள்ளது. நாம் துல்லியத்திற்காக முயல்வதாலும், தானாக மொழிபெயர்ப்பு செய்யும் போது பிழைகள் அல்லது தவறுதல்கள் இருக்க வாய்ப்புள்ளதாக கவனியுங்கள். அசல் ஆவணம் அதன் சொந்த மொழியில் அதிகாரபூர்வமான மூலமாக கருதப்பட வேண்டும். முக்கியமான தகவல்களுக்கு, தொழில்முறை மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பைப் பயன்படுத்துவதனால் ஏற்படும் புரிதலைச் சரியாக சொல்வதில் அல்லது தவறாக புரிந்துகொள்ளுதலில் எங்களுக்கு எந்தவித பொறுப்பும் இல்லை.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->