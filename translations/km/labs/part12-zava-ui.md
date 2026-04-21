![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ផ្នែកទី ១២៖ ការបង្កើត UI វែបសម្រាប់ Zava Creative Writer

> **គោលបំណង៖** បន្ថែមផ្នែកមុខបណ្តាញ ដែលអាចប្រើប្រាស់តាមកម្មង់រក្សារអ្នករុករក (browser-based front end) ទៅកាន់ Zava Creative Writer ដើម្បីអោយអ្នកអាចមើលដំណើរការជាផ្លូវការរបស់ multi-agent pipeline ពេលវេលាពិតជាមួយនឹងបដិសន្ធិភាគអ្នកភ្នាក់ងារប្រតិកម្ម និងអត្ថបទបានចាក់បញ្ចាំងជាស្ទ្រីម ដែលមានដំណើរការពីម៉ាស៊ីនបម្រើវែបមួយក្នុងតំបន់តែមួយ។

ក្នុង [ផ្នែកទី ៧](part7-zava-creative-writer.md) អ្នកបានស្ទង់មើល Zava Creative Writer ជា **កម្មវិធី CLI** (JavaScript, C#) និង **API មួយគ្មាន Interface តំណាង** (Python)។ នៅក្នុងមន្ទីរពិសោធន៍នេះ អ្នកនឹងភ្ជាប់ផ្នែកមុខស្រោម HTML/CSS/JavaScript ទៅកាន់ backend ទាំងអស់ដើម្បីអ្នកប្រើអាចអន្តរកម្មជាមួយ pipeline តាមរយៈកម្មង់រក្សារអ្នករុករក ជំនួសឱ្យតែបង្ហាញតាម Terminal ប៉ុណ្ណោះ។

---

## អ្វីដែលអ្នកនឹងរៀន

| គោលដៅ | សេចក្តីពិពណ៌នា |
|-----------|-------------|
| បម្រើឯកសារស្ថិតស្ថេរពី backend | ភ្ជាប់ថត HTML/CSS/JS ដើម្បីប្រើជាមួយស្ទ្រីម API របស់អ្នក |
| ញ៉ាំស្ទ្រីម NDJSON នៅក្នុងកម្មង់រក្សារអ្នករុករក | ប្រើ Fetch API ជាមួយ `ReadableStream` ដើម្បីអាន JSON ដែលបានបំបែកដោយខ្សែចុងបន្ទាត់ថ្មី |
| សេចក្តីព្រមព្រៀងស្ទ្រីមឯកសារមួយរួម | ធានាថា Python, JavaScript, និង C# backend ផ្ញើសារដូចគ្នា |
| ការបន្ទាន់ UI តាមលំដាប់ | បន្ទាន់ស្ថានភាពបដិសន្ធិភាគអ្នកភ្នាក់ងារនិងស្ទ្រីមអត្ថបទអត្ថបទជា token ដាច់ៗ |
| បន្ថែមជាន់ HTTP សម្រាប់កម្មវិធី CLI | ដាក់កញ្ចប់រូបមន្ត orchestration ជា Express-style server (JS) ឬ ASP.NET Core minimal API (C#) |

---

## វេបសម្ព័ន្ធរចនា

UI ជាឯកសារស្ថិតស្ថេរតែមួយនេះ (`index.html`, `style.css`, `app.js`) ដែលបានចែករំលែកដោយ backend ទាំងបី។ Backend រៀងៗខ្លួនបង្ហាញពីផ្លូវចេញពីរដូចគ្នា៖

![សម្ព័ន្ធ UI Zava — ផ្នែកមុខរួមជាមួយ backend បី](../../../images/part12-architecture.svg)

| ផ្លូវ | វិធីសាស្ត្រ | គោលបំណង |
|-------|--------|---------|
| `/` | GET | បម្រើ UI ស្ថិតស្ថេរ |
| `/api/article` | POST | ដំណើរការប្រព័ន្ធ multi-agent pipeline និងស្ទ្រីម NDJSON |

ផ្នែកមុខផ្ញើខ្លឹមសារជា JSON និងអានការឆ្លើយតបជា ស្ទ្រីមសារជា JSON ដែលបំបែកជាលាយឡំដោយខ្សែចុងបន្ទាត់ថ្មី។ សារនីមួយៗមានវាល `type` ដែល UI ប្រើសម្រាប់បន្ទាន់ផ្ទាំងត្រឹមត្រូវ៖

| ប្រភេទសារ | អត្ថន័យ |
|-------------|---------|
| `message` | ការបន្តស្ថានភាព (ឧ. "កំពុងចាប់ផ្តើមភ្នាក់ងារស្រាវជ្រាវ...") |
| `researcher` | លទ្ធផលស្រាវជ្រាវបានរួចរាល់ |
| `marketing` | លទ្ធផលស្វែងរកផលិតផលបានរួចរាល់ |
| `writer` | អ្នកសរសេរបានចាប់ផ្តើម ឬបញ្ចប់ (មាន `{ start: true }` ឬ `{ complete: true }`) |
| `partial` | តួអក្សរនាមួយដែលបានស្ទ្រីមពីអ្នកសរសេរ (មាន `{ text: "..." }`) |
| `editor` | សេចក្តីសម្រេចអ្នកធ្វើកែសម្រួលបានរួចរាល់ |
| `error` | មានបញ្ហាខ្លះ |

![ការបញ្ជូនប្រភេទសារនៅក្នុងកម្មង់រក្សារអ្នករុករក](../../../images/part12-message-types.svg)

![លំដាប់ស្ទ្រីម — ការទំនាក់ទំនង Browser ទៅ Backend](../../../images/part12-streaming-sequence.svg)

---

## លក្ខខណ្ឌចាំបាច់

- បានបញ្ចប់ [ផ្នែកទី ៧៖ Zava Creative Writer](part7-zava-creative-writer.md)
- ធ្វើការដំឡើង Foundry Local CLI ហើយបានទាញយកម៉ូដែល `phi-3.5-mini`
- កម្មង់រក្សារអ្នករុករកសម័យទំនើប (Chrome, Edge, Firefox, ឬ Safari)

---

## UI រួម

មុនពេលកែប្រែកូដ backend ដូចជាគឺ ផ្តល់ពេលខ្លះសម្រាប់ជ្រាប UI ដែលភ្ជាប់គ្នារវាងភាសាទាំងបី។ ឯកសារពាក់ព័ន្ធភ្ជាប់ក្នុង `zava-creative-writer-local/ui/`៖

| ឯកសារ | គោលបំណង |
|------|---------|
| `index.html` | គម្រោងទំព័រ៖ សំណុំបែបបទបញ្ចូល, ជាបដិសន្ធិភាគស្ថានភាពអ្នកភ្នាក់ងារ, តំបន់ចេញអត្ថបទ, ផ្ទាំងលម្អិតបិទ/បើកបាន |
| `style.css` | ស្ទីលសាមញ្ញ ជាមួយពណ៌បដិសន្ធិភាគស្ថានភាព (កំពុងរង់ចាំ, កំពុងរត់, បានបញ្ចប់, បញ្ហា) |
| `app.js` | ការហៅ Fetch, អ្នកអានបន្ទាត់ `ReadableStream`, និងលក្ខណៈបន្ទាន់ DOM |

> **គន្លឹះ៖** បើក `index.html` ត្រង់ក្នុងកម្មង់រក្សារអ្នករុករក ដើម្បីមើលទិដ្ឋភាពទំព័រ។ មិនមានអ្វីដំណើរការឡើយ ព្រោះគ្មាន backend ត្រូវបញ្ចូល ប៉ុន្តែអ្នកអាចមើលរចនាសម្ព័ន្ធបាន។

### វិធីធ្វើការអាន Stream

មុខងារសម្រាប់អាននៅក្នុង `app.js` អានសម្ភារឆ្លើយតបជាចំណែកៗ ហើយបំបែកដោយដែនកំណត់ខ្សែចុងបន្ទាត់ថ្មី៖

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
    buffer = lines.pop(); // រក្សាទុកបន្ទាត់ចុងក្រោយដែលមិនទាន់បញ្ចប់

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

សារដែលបានផ្ទៀងផ្ទាត់នីមួយៗត្រូវបានផ្ញើទៅ `handleMessage()` ដែលធ្វើការបន្ទាន់ធាតុ DOM តាមប្រភេទ `msg.type`។

---

## ការហាត់ប្រាណ

### ការហាត់ប្រាណទី ១៖ ចាប់ផ្តើម Python Backend ជាមួយ UI

វាជាដំណើរការរួចរាល់របស់ Python (FastAPI) មាន API ស្ទ្រីម។ ការផ្លាស់ប្តូរតែមួយគត់ គឺភ្ជាប់ថត `ui/` ជាឯកសារស្ថិតស្ថេរ។

**១.១** បើកថត Python API ហើយដំឡើងគ្រឿងបន្លាស់៖

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**១.២** ចាប់ផ្តើមម៉ាស៊ីនបម្រើ៖

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**១.៣** បើកកម្មង់រក្សារអ្នករុករកទៅ `http://localhost:8000`។ អ្នកគួរមើលឃើញ UI របស់ Zava Creative Writer មានប្លង់បញ្ចូល ៣ និងប៊ូតុង "Generate Article"។

**១.៤** ចុច **Generate Article** ដើម្បីប្រើតម្លៃលំនាំដើម។ មើលឃើញសញ្ញាបដិសន្ធិភាគអ្នកភ្នាក់ងារផ្លាស់ប្តូរពី "Waiting" ទៅ "Running" ទៅ "Done" ខណៈប្រតិបត្តិការដំណើរការ និងអត្ថបទត្រូវបានបង្ហាញជាស្ទ្រីមក្នុងផ្ទាំងចេញម៉ាសុន token ជាលំដាប់។

> **ការជួសជុលបញ្ហា៖** ប្រសិនបើទំព័របង្ហាញ JSON ជំនួស UI សូមធានាថាអ្នកកំពុងរត់ `main.py` ដែលបានបញ្ចូល static files។ ផ្លូវ `/api/article` នៅតែមមានដើមដូចមុន។ ការភ្ជាប់ static files បម្រើ UI នៅផ្លូវផ្សេងទៀតទាំងអស់។

**របៀបដំណើរការ៖** `main.py` ដែលបានធ្វើបច្ចុប្បន្នភាពបន្ថែមជួរដេកមួយនៅចុងក្រោម៖

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

នេះបម្រើឯកសារទាំងអស់ពី `zava-creative-writer-local/ui/` ជា static asset ដោយ `index.html` ជាឯកសារជំនួយលំនាំ។ ផ្លូវ POST `/api/article` ត្រូវបានចុះបញ្ជីមុន static mount ដូច្នេះវាមានអាទិភាព។

---

### ការហាត់ប្រាណទី ២៖ បន្ថែមម៉ាស៊ីនបម្រើវែបនៅ JavaScript Variant

វាគឺ CLI app សព្វថ្ងៃ (`main.mjs`)។ ឯកសារថ្មី `server.mjs` រួមបញ្ចូលម៉ូឌុលភ្នាក់ងារដូចគ្នានៅក្រោយម៉ាស៊ីនបម្រើ HTTP និងបម្រើ UI រួម។

**២.១** បើកថត JavaScript ហើយដំឡើងគ្រឿងបន្លាស់៖

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**២.២** ចាប់ផ្តើមម៉ាស៊ីនបម្រើវែប៖

```bash
node server.mjs
```

```powershell
node server.mjs
```

អ្នកគួរមើលឃើញ៖

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**២.៣** បើក `http://localhost:3000` នៅកម្មង់រក្សារអ្នករុករក ហើយចុច **Generate Article**។ UI ដូចគ្នាធ្វើការទៅ backend JavaScript ដូចគ្នា។

**សិក្សាកូដ៖** បើក `server.mjs` និងពិនិត្យលំនាំសំខាន់ៈ

- **បម្រើឯកសារតាំងពី static** ប្រើម៉ូឌុល Node.js តែមួយ `http`, `fs`, និង `path` ដោយមិនត្រូវការប្រព័ន្ធក្រៅ។
- **ការការពារការឆ្លងផ្លូវ Path-traversal** និយមន័យផ្លូវស្នើសុំនិងត្រួតពិនិត្យថានៅក្នុងថត `ui/`។
- **ស្ទ្រីម NDJSON** ប្រើជំនួយការ `sendLine()` ដើម្បី serialize ប្រភេទអង្គភាពទាំងអស់ បង្ហាញខ្សែចុងបន្ទាត់ថ្មី និងកាត់បន្ថយខ្សែចុងបន្ទាត់ខាងក្នុង។
- **អង្គការភ្នាក់ងា្រ** ប្រើម៉ូឌុល `researcher.mjs`, `product.mjs`, `writer.mjs`, និង `editor.mjs` ដដែលមិនបានផ្លាស់ប្តូរ។

<details>
<summary>ខ្លឹមសារសំខាន់ពី server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// អ្នកស្រាវជ្រាវ
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// អ្នកសរសេរ (ផ្សាយបន្តផ្ទាល់)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### ការហាត់ប្រាណទី ៣៖ បន្ថែម Minimal API សម្រាប់ C# Variant

C# variant កំពុងជាកម្មវិធី Console។ គំរោងថ្មី `csharp-web` ប្រើ ASP.NET Core minimal APIs ដើម្បីបង្ហាញ pipeline ដូចគ្នាជាសេវាវែប។

**៣.១** បើកគំរោង C# វែប និងធ្វើការជួសជុលគ្រឿងបន្លាស់៖

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**៣.២** រត់ម៉ាស៊ីនបម្រើវែប៖

```bash
dotnet run
```

```powershell
dotnet run
```

អ្នកគួរមើលឃើញ៖

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**៣.៣** បើកកម្មង់រក្សារអ្នករុករកទៅ `http://localhost:5000` ហើយចុច **Generate Article** ។

**សិក្សាកូដ៖** បើក `Program.cs` ក្នុងថត `csharp-web` និងចំណាំៈ

- ឯកសារគំរោងប្រើ `Microsoft.NET.Sdk.Web` ជំនួស `Microsoft.NET.Sdk` ដែលបន្ថែមការ​គាំទ្រ ASP.NET Core។
- ឯកសារស្ថិតស្ថេរបម្រើតាមរយៈ `UseDefaultFiles` និង `UseStaticFiles` ទៅថត `ui/` ដែលចែករំលែក។
- ផ្លូវ `/api/article` សរសេរ NDJSON ទៅ `HttpContext.Response` ដោយបញ្ចេញបន្ទាត់បន្ទាប់ពីរាល់បន្ទាត់សម្រាប់ស្ទ្រីមពេលវេលាពិត។
- របៀបអង្គការភ្នាក់ងារ (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) ដដែលដូចជាកម្មវិធី Console។

<details>
<summary>ខ្លឹមសារ​ចម្បងពី csharp-web/Program.cs</summary>

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

### ការហាត់ប្រាណទី ៤៖ ស្វែងយល់អំពីបដិសន្ធិភាគស្ថានភាពអ្នកភ្នាក់ងារ

ឥឡូវនេះអ្នកមាន UI ធ្វើការបាន សូមមើលថាផ្នែកមុខធ្វើការបន្ទាន់បដិសន្ធិភាគយ៉ាងដូចម្តេច។

**៤.១** បើក `zava-creative-writer-local/ui/app.js` នៅកម្មវិធីកែសម្រួលរបស់អ្នក។

**៤.២** ស្វែងរកមុខងារ `handleMessage()`។ សម្គាល់ថាវាសម្ព័ន្ធប្រភេទសារទៅអំពើ UI ដូចខាងក្រោម៖

| ប្រភេទសារ | សកម្មភាព UI |
|-------------|-----------|
| `message` មាន "researcher" | កំណត់បដិសន្ធិភាគ Researcher ទៅ "Running" |
| `researcher` | កំណត់បដិសន្ធិភាគ Researcher ទៅ "Done" និងបញ្ចូលលទ្ធផលស្រាវជ្រាវ |
| `marketing` | កំណត់បដិសន្ធិភាគ Product Search ទៅ "Done" និងបញ្ចូលលទ្ធផលស្វែងរកគំនូសផលិតផល |
| `writer` មាន `data.start` | កំណត់បដិសន្ធិភាគ Writer ទៅ "Running" និងសម្អាតការចេញអត្ថបទ |
| `partial` | បន្ថែមអត្ថបទ token ទៅកន្លែងកំណត់អត្ថបទ |
| `writer` មាន `data.complete` | កំណត់បដិសន្ធិភាគ Writer ទៅ "Done" |
| `editor` | កំណត់បដិសន្ធិភាគ Editor ទៅ "Done" និងបញ្ចូលមតិយោបល់កែសម្រួល |

**៤.៣** បើកផ្ទាំងលម្អិតដែលអាចបិទ/បើកបាន "Research Results", "Product Matches", និង "Editor Feedback" ខាងក្រោមអត្ថបទ ដើម្បីពិនិត្យ JSON ដើមដែលភ្នាក់ងារនីមួយៗបានផលិត។

---

### ការហាត់ប្រាណទី ៥៖ កែប្រែ UI (ជម្រើស)

សាកល្បងធ្វើការកែប្រែមួយ ឬច្រើនពីការកែលម្អខាងក្រោម៖

**៥.១ បន្ថែមការរាប់ពាក្យ។** បន្ទាប់ពី Writer បញ្ចប់ បង្ហាញចំនួនពាក្យនៅក្រោមផ្ទាំងចេញអត្ថបទ។ អ្នកអាចគណនានៅ `handleMessage` នៅពេល `type === "writer"` និង `data.complete` ជាការពិត៖

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**៥.២ បន្ថែមសញ្ញាការសាកល្បងម្តងទៀត។** នៅពេល Editor ស្នើសុំការកែប្រែលម្អថ្មី pipeline រត់ឡើងវិញ។ បង្ហាញបដិបត្តិភាព "Revision 1" ឬ "Revision 2" នៅផ្ទាំងស្ថានភាព។ ស្តាប់ប្រភេទសារ `message` ដែលមាន "Revision" ហើយធ្វើបន្ទាន់ធាតុ DOM ថ្មី។

**៥.៣ យ៉ាងងងឹត។** បន្ថែមប៊ូតុងបម្លែង និងថ្នាក់ `.dark` ទៅ `<body>`។ ប្ដូរពណ៌ផ្ទៃខាងក្រោម អក្សរ និងផ្ទាំងក្នុង `style.css` ជាមួយអ្នកជ្រើសរើស `body.dark`។

---

## សេចក្ដីសង្ខេប

| អ្វីដែលអ្នកបានធ្វើ | របៀបធ្វើ |
|-------------|-----|
| បម្រើ UI ពី Python backend | ភ្ជាប់ថត `ui/` ជាមួយ `StaticFiles` នៅក្នុង FastAPI |
| បន្ថែមម៉ាស៊ីនបម្រើ HTTP ចំពោះ JavaScript variant | បង្កើត `server.mjs` ប្រើ Node.js ម៉ូឌុល `http` ខ្ទង់ក្នុង |
| បន្ថែម web API ទៅ C# variant | បង្កើតគំរោង `csharp-web` ជាមួយ ASP.NET Core minimal APIs |
| ញ៉ាំស្ទ្រីម NDJSON ក្នុងកម្មង់រក្សារអ្នករុករក | ប្រើ `fetch()` ជាមួយ `ReadableStream` និងវិធីសាស្ត្រអាន JSON តាមបន្ទាត់ |
| បន្ទាន់ UI ពេលវេលាពិត | សម្រង់ប្រភេទសារទៅប្រតិបត្តិ DOM (បដិសន្ធិភាគ, អត្ថបទ, ផ្ទាំងបិទ/បើក) |

---

## ចំណុចគួរចងចាំ

1. **ផ្នែកមុខ static សម្រាប់ការចែករំលែក** អាចដំណើរការជាមួយ backend មួយណាមួយដែលនិយាយភាសាស្ទ្រីមដូចគ្នា ចាស់បង្ហាញប្រសិទ្ធភាពនៃម៉ូដែល API ដែលស្រដៀង OpenAI។
2. **Newline-delimited JSON (NDJSON)** ជារូបមន្តស្ទ្រីមស្រួលដែលប្រើបានជាទៀងទាត់ជាមួយ API `ReadableStream` នៅក្នុងកម្មង់រក្សារអ្នករុករក។
3. **Python variant** ពុំចាំបាច់ផ្លាស់ប្តូរច្រើន ព្រោះវាមាន Endpoints ដែលល្បីហើយ។ JavaScript និង C# ត្រូវការប្រអប់ HTTP តូចៗបន្ថែម។
4. ការរក្សាព្រមាន UI ជា **vanilla HTML/CSS/JS** ជៀសវាងឧបករណ៍បង្រៀន កម្មវិធីបណ្តាញ និងស្មុគស្មាញបន្ថែមសម្រាប់អ្នករៀនកម្មវិធីវគ្គសិក្សា។
5. ម៉ូឌុលភ្នាក់ងារ​ដដែល (Researcher, Product, Writer, Editor) ត្រូវបានប្រើឡើងវិញដោយគ្មានការផ្លាស់ប្តូរ; ផ្នែកផ្លូវដឹកជញ្ជូនតែប៉ុណ្ណោះដែលផ្លាស់ប្ដូរ។

---

## អត្ថបទអានបន្ថែម

| ឯកសារ | តំណភ្ជាប់ |
|----------|------|
| MDN: ការប្រើប្រាស់ Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

បន្តទៅ [ផ្នែកទី ១៣៖ បញ្ចប់វគ្គសិក្សា](part13-workshop-complete.md) សម្រាប់សេចក្ដីសង្ខេបនៃអ្វីដែលអ្នកបានសង់ក្នុងវគ្គសិក្សានេះទាំងមូល។

---
[← ផ្នែក 11: ការហៅឧបករណ៍](part11-tool-calling.md) | [ផ្នែក 13: ការបញ្ចប់សិក្ខាសាលា →](part13-workshop-complete.md)