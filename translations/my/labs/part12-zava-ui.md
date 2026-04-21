![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# အပိုင်း ၁၂: Zava Creative Writer အတွက် Web UI တည်ဆောက်ခြင်း

> **ရည်မှန်းချက်:** Zava Creative Writer အတွက် browser-based front end ကိုထည့်သွင်းပြီး multi-agent pipeline ကို real time မှာကြည့်ရှုနိုင်ရန်၊ live agent status badges နှင့် article စာသားများကို stream လုပ်ပေးပြီး၊ အားလုံးကို single local web server မှာဖြန့်ဝေပေးရန်။

[အပိုင်း ၇](part7-zava-creative-writer.md) တွင် Zava Creative Writer ကို **CLI application** (JavaScript, C#) နှင့် **headless API** (Python) အဖြစ် လေ့လာခဲ့ပါတယ်။ ဒီ lab မှာတော့ shared **vanilla HTML/CSS/JavaScript** front end ကို backend တစ်ခုချင်းစီနှင့် ချိတ်ဆက်ပြီး user တွေဟာ terminal အစား browser မှတဆင့် pipeline နဲ့ အပြန်အလှန်ဆက်သွယ်နိုင်အောင် ပြုလုပ်မှာဖြစ်ပါတယ်။

---

## သင်ဘာတွေသင်ယူမလဲ

| ရည်မှန်းချက် | ဖော်ပြချက် |
|-----------|-------------|
| Backend မှ static ဖိုင်များကို ဝန်ဆောင်ရေးလုပ်ခြင်း | သင်၏ API လမ်းကြောင်းနှင့် အတူ HTML/CSS/JS ဖိုင်တစ်ခုပိုင်ထားခြင်း |
| Browser ထဲတွင် streaming NDJSON အသုံးပြုခြင်း | Fetch API မှ `ReadableStream` ဖြင့် newline-delimited JSON ကို ဖတ်ခြင်း |
| တစ်ညီတစ်နည်းဖြင့် streaming protocol | Python, JavaScript, နှင့် C# backend များသည် တူညီသော message ပုံစံ မက်ဆေ့ချ် ထုတ်ပေးရေးအာမခံခြင်း |
| UI အဆင့်မြှင့်တင်မှုများ | Agent status badges များကို update ပြုလုပ်ပြီး article စာသား ကို token တစ်ခုချင်းစီအလိုက် stream ပြသခြင်း |
| CLI app တစ်ခုတွင် HTTP layer တင်ခြင်း | ရှိပြီးသား orchestrator logic ကို Express-style server (JS) သို့မဟုတ် ASP.NET Core minimal API (C#) နဲ့ ထုပ်ပိုးခြင်း |

---

## ကျယ်ပြန့်ဖွဲ့စည်းချက်

UI သည် သုံး backend အားလုံးနှင့်  ပြန်လည်မျှဝေသော static ဖိုင်များ (`index.html`, `style.css`, `app.js`) လုပ်ငန်းတစ်ခု ဖြစ်ပါတယ်။ Backend တစ်ခုစီမှာ လမ်းကြောင်း နှစ်ခုတူညီစွာ ဖော်ပြထားသည်-

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| လမ်းကြောင်း | နည်းလမ်း | ရည်ရွယ်ချက် |
|-------|--------|---------|
| `/` | GET | Static UI ကို ပေးပို့သည် |
| `/api/article` | POST | Multi-agent pipeline ကို လုပ်ဆောင်ပြီး NDJSON ကို stream ပေးသည် |

Front end က JSON body တစ်ခု ပို့ပြီး နောက်ဆုံး ရရှိသော response ကို newline-delimited JSON messages စီးရီးအဖြစ် ဖတ်ရှုသည်။ မက်ဆေ့ချ် တစ်ခုချင်းစီတွင် `type` အကွက် ပါဝင်ပြီး UI က ပက်နယ်တစ်ခုချင်းကို update ပြုလုပ်ရန် အသုံးပြုသည်-

| မက်ဆေ့ချ်အမျိုးအစား | အဓိပ္ပာယ် |
|-------------|---------|
| `message` | အခြေအနေ အဆင့်ပြောင်းလဲမှု (ဥပမာ- "Starting researcher agent task...") |
| `researcher` | သုတေသနရလဒ်များ ပြင်ဆင်ပြီးဖြစ်သည် |
| `marketing` | ထုတ်ကုန် ရှာဖွေမှု ရလဒ်များ အသင့်ရှိသည် |
| `writer` | Writer စတင်မိ သို့မဟုတ် ပြီးဆုံးပြီး ( `{ start: true }` သို့မဟုတ် `{ complete: true }` ပါဝင်) |
| `partial` | Writer မှ ပေးပို့သော token တစ်ခု ( `{ text: "..." }` ပါ) |
| `editor` | Editor ၏ဆုံးဖြတ်ချက် ပြီးဆုံးကြောင်း |
| `error` | တစ်ခုခုမှားယွင်းမှု ဖြစ်ပွားထားသည် |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## မဖြစ်မနေ ကြိုတင်လိုအပ်ချက်များ

- [အပိုင်း ၇: Zava Creative Writer](part7-zava-creative-writer.md) ကို ပြီးမြောက်ထားရန်
- Foundry Local CLI ကို install ပြီး `phi-3.5-mini` model ကို download ပြီးထားရန်
- ခေတ်မီသော web browser များ (Chrome, Edge, Firefox, Safari) တစ်ခုခု ရှိထားရန်

---

## Shared UI

Backend ကုဒ်ကို ပြင်ဆင်မတိုင်မီ သုံး frontend language track များအသုံးပြုမည့် front end ကို ရှာဖွေကြည့်ရန်။ ဖိုင်များမှာ `zava-creative-writer-local/ui/` တွင်ရှိသည်-

| ဖိုင် | ရည်ရွယ်ချက် |
|------|---------|
| `index.html` | စာမျက်နှာ ဖွဲ့စည်းမှု - input form, agent status badges, article output နေရာ, ဖွင့်/ပိတ် panel များ |
| `style.css` | အနည်းဆုံး အလှဆင်မှု (status-badge ရောအရောင်အခြေအနေများ - waiting, running, done, error) |
| `app.js` | Fetch ခေါ်ဆိုမှု, `ReadableStream` စာကြောင်းဖတ်သူ, DOM update logic |

> **အကြံပြုချက်:** `index.html` ကို browser မှ တိုက်ရိုက် ဖွင့်ပြီး ပုံစံကို ကြည့်ရှုနိုင်သည်။ Backend မရှိသေးသောကြောင့် မလုပ်ဆောင်ပါ။

### Stream Reader အလုပ်လုပ်ပုံ

`app.js` အတွင်းရှိ အဓိက function သည် response body ကို chunk တစ်ခုချင်း ဖတ်ပြီး newline ဖြင့် ခွဲခြားသည်-

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
    buffer = lines.pop(); // မပြည့်စုံသေးသည့် နောက်ဆုံးလိုင်းကို ထိန်းသိမ်းထားပါ

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

ဖတ်ရှုထားသော မက်ဆေ့ချ်တိုင်းကို `handleMessage()` သို့ ပို့ပြီး `msg.type` အလိုက် သက်ဆိုင်ရာ DOM element ကို update လုပ်ပါသည်။

---

## လေ့ကျင့်ခန်းများ

### လေ့ကျင့်ခန်း ၁: Python Backend နှင့် UI ကို Run ခြင်း

Python (FastAPI) variant တွင် streaming API endpoint ရှိပြီး ဖြစ်သည်။ ပြင်ဆင်ရန်လိုတာမှာ `ui/` folder ကို static files အဖြစ် mount လုပ်ခြင်းသာဖြစ်သည်။

**1.1** Python API directory သို့သွားပြီး အထောက်အကူပြု packages များ install ဘား-

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Server ကို စတင်လုပ်ဆောင်-

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Browser ကနေ `http://localhost:8000` ဖြင့် ဝင်ကြည့်ပါ။ Zava Creative Writer UI ရှိပြီး လိုအပ်သော စာသားသုံးခု နှင့် "Generate Article" ခလုတ်ကို တွေ့မြင်ရပါမည်။

**1.4** ကုဒ်များကို default အဖြစ်ထားပြီး **Generate Article** ကိုနှိပ်ပါ။ Agent status badge များသည် "Waiting" မှ "Running" သို့ "Done" ဆီ ပြောင်းလဲလာပြီး article စာသားသည် output panel မှာ token တစ်ခုချင်းစီ streaming ထွက်လာသည်ကို ကြည့်ရှုနိုင်ပါသည်။

> **ပြဿနာဖြေရှင်းခြင်း:** UI အစား JSON response ပဲ ဖော်ပြနေပါက static files မောင့်နေတဲ့ `main.py` ဖိုင်ကို အသစ်လုပ်ဆောင်နေကြောင်းအတည်ပြုပါ။ `/api/article` endpoint က မူရင်းလမ်းကြောင်းမှာ အလုပ်လုပ်ဆဲဖြစ်ပြီး static files mount လုပ်ထားသော လမ်းကြောင်းများမှာ UI ကို ပေးပို့ပါသည်။

**အလုပ်လုပ်ပုံ:** ပြင်ဆင်ထားသော `main.py` အောက်ဆုံးတွင်စာကြောင်းတစ်ကြောင်း ထည့်သွင်းထားသည်-

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

ဒါက `zava-creative-writer-local/ui/` မှာရှိသောဖိုင်တိုင်းကို static asset အနေနှင့် ပေးပို့ရာတွင် `index.html` သည် default document ဖြစ်စေသည်။ `/api/article` POST route သည် static mount မှာ မတိုင်မီ register လုပ်ထားပေးသောကြောင့် အဓိကထားပေးသည်။

---

### လေ့ကျင့်ခန်း ၂: JavaScript Variant တွင် Web Server ထည့်ခြင်း

JavaScript variant သည် CLI application (`main.mjs`) ဖြစ်သည်။ `server.mjs` နောက်ကွယ်တွင် အတူတူ agent modules များကို HTTP server ဖြင့် wrap လုပ်ပြီး shared UI ကိုလည်း serve လုပ်ပေးသည်။

**2.1** JavaScript directory သို့သွားကာ dependencies များ install ပြုလုပ်-

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Web server ကို စတင်လုပ်ဆောင်-

```bash
node server.mjs
```

```powershell
node server.mjs
```

အောက်ပါစာကို မျှော်လင့်ပါ-

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Browser မှ `http://localhost:3000` ဖြင့် ဝင်ကာ **Generate Article** ကို နှိပ်ပါ။ အတူတူဆိုတဲ့ UI သည် JavaScript backend နှင့် တစိတ်တပိုင်းဖြစ်စွာ အလုပ်လုပ်ပါသည်။

**ကုဒ်အကြောင်းကြည့်ပါ:** `server.mjs` ဖိုင်ကိုဖွင့်ပြီး အဓိကပုံစံများကို သတိပြုပါ။

- **Static file serving** အတွက် Node.js မှာ ပါဝင်သော `http`, `fs`, နှင့် `path` modules ကို အသုံးပြုထားပြီး ဘယ် framework မလိုအပ်။
- **Path-traversal ကာကွယ်ခြင်း** အသုံးပြုသူ တောင်းဆိုသည့်လမ်းကြောင်းကို သာမန်ဖြင့် ပြန်တိမ်းပြီး `ui/` ဖိုလ်ဒါနယ်အတွင်းမှာသာ ရှိဖို့ စစ်ဆေးထားသည်။
- **NDJSON streaming** ဆိုသည်မှာ `sendLine()` helper ကိုအသုံးပြုကာ object တစ်ခုချင်း serialization ပြုလုပ်၊ အတွင်း newlines ကို ဖယ်ရှားကာ အဆုံးတွင် newline ရေးထည့်သည်။
- **Agent orchestration** မှာ ရှိပြီးသား `researcher.mjs`, `product.mjs`, `writer.mjs`, နှင့် `editor.mjs` modules ကို ပြင်ဆင်ခြင်းမရှိ အချိန်တိုင်း အသုံးပြုသည်။

<details>
<summary>server.mjs ဖိုင်မှ အဓိက ကောက်နုတ်ချက်</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// သုသေသနအလုပ်ရှင်
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// စာရေးဆရာ (စီးရီးထုတ်လွှင့်သူ)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### လေ့ကျင့်ခန်း ၃: C# Variant တွင် Minimal API တစ်ခု ထည့်သွင်းခြင်း

C# variant သည် ခြုံမှပွဲ console application ဖြစ်သည်။ ထပ်မံတည်ဆောက်ထားသော project အသစ် `csharp-web` は ASP.NET Core minimal APIs ကို အသုံးပြုပြီး web service အဖြစ် pipeline ပေးသည်။

**3.1** C# web project သို့သွားကာ package များ restore ပြုလုပ်-

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Web server ကို စတင်လုပ်ဆောင်-

```bash
dotnet run
```

```powershell
dotnet run
```

အောက်ပါ output ကို ကြည့်ရှုနိုင်ပါသည်-

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Browser မှ `http://localhost:5000` ဖြင့် ဝင်ကာ **Generate Article** ကို နှိပ်ပါ။

**ကုဒ် ဖတ်ရှုမည်။** `csharp-web` directory ထဲ `Program.cs` ကို ဖွင့်လေ့လာပါ-

- project file သည် `Microsoft.NET.Sdk` ၏ နေရာတွင် `Microsoft.NET.Sdk.Web` ကို အသုံးပြုထားပြီး ASP.NET Core support ပါထည့်သည်။
- static files များကို `UseDefaultFiles` နဲ့ `UseStaticFiles` အသုံးပြုကာ shared `ui/` directory နေရာသည် ရည်ညွှန်းထားသည်။
- `/api/article` endpoint က NDJSON line များကို တိုက်ရိုက် `HttpContext.Response` မှာရေးပြီး တစ်ကြောင်းတိုင်းပြီးတိုင်း flush လုပ်ကာ real-time streaming ပေးသည်။
- All agent logic (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) သည် console version နှင့် တူညီသည်။

<details>
<summary>csharp-web/Program.cs မှ အဓိက ကောက်နုတ်ချက်</summary>

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

### လေ့ကျင့်ခန်း ၄: Agent Status Badges ကို လေ့လာကြည့်ရန်

အခု သင် UI ကို အလုပ်လုပ်အောင် ချထားပြီးဖြစ်သည်။ Front end မှ status badges များ မည်သို့ update လုပ်သည်ကို ကြည့်ရှုပါ။

**4.1** `zava-creative-writer-local/ui/app.js` ကို ရှာဖွေဖွင့်ကြည့်ပါ။

**4.2** `handleMessage()` function ကို ရှာဖွေပါ။ message types များကို DOM update များနှင့် မည်သို့ ချိတ်ဆက်ထားသည် ပြပါ-

| Message type | UI အက်ရှင် |
|-------------|-----------|
| `message` မက်ဆေ့ချ်အတွင်း "researcher" ပါသည် | Researcher badge ကို "Running" သတ်မှတ်သည် |
| `researcher` | Researcher badge ကို "Done" သတ်မှတ်ပြီး Research Results panel ကို ပြည့်စုံစွာ ဖြည့်သည် |
| `marketing` | Product Search badge ကို "Done" သတ်မှတ်ပြီး Product Matches panel ထည့်ရေးသည် |
| `writer` နှင့် `data.start` | Writer badge ကို "Running" သတ်မှတ်ပြီး article output ကို ဖယ်ရှားသည် |
| `partial` | Token စာသား ကို article output တွင် ပေါင်းထည့်သည် |
| `writer` နှင့် `data.complete` | Writer badge ကို "Done" သတ်မှတ်သည် |
| `editor` | Editor badge ကို "Done" သတ်မှတ်ပြီး Editor Feedback panel ဖြည့်သည် |

**4.3** အောက်တွင်ရှိသော "Research Results", "Product Matches", နှင့် "Editor Feedback" panel များအား ဖွင့်ကာ တစ်ခုချင်းစီ အယ်ဂျင့်တို့ရဲ့ raw JSON output ကို ကြည့်ရှုပါ။

---

### လေ့ကျင့်ခန်း ၅: UI ကို ကိုယ်ပိုင်ချဲ့ထွင်ခြင်း (အသုံးချန့်)

အောက်ဖော်ပြပါတခု သို့မဟုတ် ပိုမိုလည်ပတ်မှုထည့်ရန် ကြိုးစားပါ-

**5.1 စကားလုံးရေအတွက်ထည့်ရန်။** Writer ပြီးချိန်မှာ article word count ကို output panel အောက်တွင် ပြမည်။ `handleMessage` အတွင်း `type === "writer"` နှင့် `data.complete == true` ဖြစ်ချိန်တွင် ဤအချက်ကိုတွက်ချက်နိုင်သည်-

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 ပြန်လည်ကြိုးစားမှုအချက်ပြသည်။** Editor မှ ပြင်ဆင်မှုတောင်းဆိုလျှင် pipeline ပြန်စတင်ပြုလုပ်သည်။ Status panel တွင် "Revision 1" သို့ "Revision 2" လုပ်ဆောင်မှုကြေညာချက် တစ်ခုကို ပြရန်။ `message` type တွင် "Revision" ပါဝင်လျှင် အသစ် DOM element တစ်ခု update ပြုလုပ်ပါ။

**5.3 အမည်း mode။** toggle ခလုတ်တစ်ခုထည့်ကာ `<body>` တွင် `.dark` class တစ်ခု ထည့်ပါ။ `style.css` ထဲတွင် `body.dark` ဆိုပြီး background, စာသားနှင့် panel အရောင်များ ကို override ပြုလုပ်နိုင်သည်။

---

## အနှစ်ချုပ်

| သင်လုပ်ဆောင်သူမည် | ဘယ်လိုလုပ်ဆောင်ခဲ့သည် |
|-------------|-----|
| Python backend မှ UI ကို serve ပြုလုပ်ခြင်း | FastAPI အတွက် `StaticFiles` ဖြင့် `ui/` folder ကို mount ထားခြင်း |
| JavaScript variant တွင် HTTP server ထည့်သွင်းခြင်း | သဘာဝ Node.js `http` module ကို အသုံးပြုကာ `server.mjs` ဖန်တီးခြင်း |
| C# variant တွင် web API ထည့်ခြင်း | ASP.NET Core minimal APIs ဖြင့် `csharp-web` project အသစ်တည်ဆောက်ခြင်း |
| Browser တွင်း streaming NDJSON ကို အသုံးပြုခြင်း | `fetch()` နှင့် `ReadableStream` ကိုအသုံးပြုကာ ကုဒ်ကို တန်းလိုက် JSON ဖတ်ခြင်း |
| UI ကို real-time အဆင့်မြှင့်တင်ခြင်း | message types များကို DOM update (badges, စာသားများ၊ collapsible panels) နှင့်ချိတ်ဆက်ခြင်း |

---

## အဓိက အယူခံများ

1. **တူညီသော streaming protocol** အသုံးပြု backend အားလုံးနှင့် အတူ shared static front end များ အဆင်ပြေစွာအလုပ်လုပ်နိုင်ပြီး၊ OpenAI-compatible API ပုံစံ၏ တန်ဖိုးကို သက်သေပြသည်။
2. **Newline-delimited JSON (NDJSON)** သည် browser `ReadableStream` API နှင့် သဘာဝအတိုင်း လုပ်ဆောင်နိုင်သည့် ရိုးရှင်းသော streaming format ဖြစ်သည်။
3. **Python variant** သည် FastAPI endpoint ရှိပြီးသားဖြစ်သောကြောင့် ပြင်ဆင်မှု သေးငယ်ဆုံးဖြစ်ခဲ့သည်။ JavaScript နှင့် C# variant များသည် HTTP wrapper သေးငယ်တစ်ခု လိုအပ်သည်။
4. UI ကို **vanilla HTML/CSS/JS** အနေနဲ့ ထားခြင်းသည် build tools, framework ကို မလိုအပ်စေသဖြင့် သင်တန်းတက်သူများအတွက် ယခင်ထက် ရိုးရှင်းလွယ်ကူစေသည်။
5. Agent modules (Researcher, Product, Writer, Editor) တူညီစွာ ပြန်အသုံးပြုထား၍ transport layer မှသာ ပြောင်းလဲထားပါသည်။

---

## ပိုမိုလေ့လာရန်

| အရင်းအမြစ် | လင့်ခ် |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

ဒီလေ့လာမှု ပြီးဆုံးပါက [အပိုင်း ၁၃: လေ့လာမှု အပြီးသတ်](part13-workshop-complete.md) သို့ သွားပြီး လေ့လာမှုအနှစ်ချုပ်ကို ကြည့်ရှုနိုင်ပါသည်။

---
[← အပိုင်း ၁၁: ကိရိယာခေါ်ဆိုခြင်း](part11-tool-calling.md) | [အပိုင်း ၁၃: အလုပ်ရုံပြီးဆုံး →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**အကြောင်းကြားချက်**  
ဤစာရွက်စာတမ်းကို AI ပြန်ဆိုခြင်းဝန်ဆောင်မှု [Co-op Translator](https://github.com/Azure/co-op-translator) အသုံးပြု၍ ဘာသာပြန်ထားခြင်းဖြစ်ပါသည်။ ငါတို့သည် တိကျမှန်ကန်မှုအတွက် ကြိုးစားပေမယ့်၊ စက်ရုပ်ဘာသာပြန်မှုများတွင် အမှားများ သို့မဟုတ် တိကျမှုနည်းပါးမှုများ ရှိနိုင်ကြောင်း သတိပြုပါရန် မေတ္တာရပ်ခံအပ်ပါသည်။ မူလစာရွက်စာတမ်းသည် မူရင်းဘာသာဖြင့် အတည်ပြုအချက်အလက်ဖြစ်သည့်အတွက် အခြေခံစရိုက်အဖြစ် သတ်မှတ်မှုပေးသင့်ပါသည်။ အရေးကြီးသော အချက်အလက်များအတွက် လူ့အကျွမ်းထုတ်ဘာသာပြန်မှုကို အကြံပြုပါသည်။ ဤဘာသာပြန်ချက်ကို အသုံးပြုရာတွင် ဖြစ်ပေါ်နိုင်သော နားလည်မှုမှားခြင်းများ သို့မဟုတ် သဘောထားဖြစ်ပေါ်မှုများအတွက် ကျွန်ုပ်တို့သည် တာဝန်ယူမမှတ်ပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->