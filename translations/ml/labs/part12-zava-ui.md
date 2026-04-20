![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ഭാഗം 12: Zava Creative Writer-ന് ഒരു വെബ് UI നിർമ്മിക്കുന്നത്

> **ലക്ഷ്യം:** Zava Creative Writer-ന് ബ്രൗസറിലൂടെയുള്ള ഒരു ഫ്രണ്ട്എൻഡ് ചേർക്കുക, അതിലൂടെ മൾട്ടി-ഏജൻറ് പൈപ്പ്‌ലൈൻ റിയൽ ടൈമിൽ പ്രവർത്തിക്കുന്നത് നിങ്ങൾക്ക് കാണാനാകും, ലൈവ് ഏജൻറ് സ്റ്റാറ്റസ് ബാഡ്ജുകളും സ്റ്റ്രീം ചെയ്ത ലേഖന തെയ്ക്കും എല്ലാം ഒരു ലൊക്കൽ വെബ് സെർവർ വഴി ലഭ്യമാക്കുക.

[Part 7](part7-zava-creative-writer.md)-ൽ നിങ്ങൾ Zava Creative Writer-നെ **CLI അപ്ലിക്കേഷനായി** (JavaScript, C#) ഒപ്പം **ഹെഡ്‌ലെസ് API** (Python) ആയി പരീക്ഷിച്ചു. ഈ ലാബിൽ നിങ്ങൾ ഒരു പങ്കിടുന്ന **വാനില്ല HTML/CSS/JavaScript** ഫ്രണ്ട്എൻഡ് എല്ലാ ബാക്ക്‌എൻഡുകളിലെയും ബന്ധിപ്പിക്കും, അതിലൂടെ ഉപയോക്താക്കൾ ടെർമിനലിന് പകരം ബ്രൗസറിൽ നിന്നാണ് പൈപ്പ്‌ലൈനുമായി ഇടപഴകുന്നതിനുള്ള suvidhya ലഭിക്കുക.

---

## നിങ്ങൾ അറിയാൻ പോകരുന്നത്

| ലക്ഷ്യം | വിവരണം |
|---------|----------|
| ബാക്ക്‌എൻഡിൽ നിന്നുള്ള സ്റ്റാറ്റിക് ഫയലുകൾ സർവ് ചെയ്യുക | നിങ്ങളുടെ API റൂട്ടിനൊപ്പം HTML/CSS/JS ഡയറക്ടറി മൗണ്ട് ചെയ്യുക |
| ബ്രൗസറിൽ സ്റ്റ്രീമിങ് NDJSON ഉപയോഗിക്കുക | `ReadableStream` ഉപയോഗിച്ച് Fetch API-യിൽ ന്യൂലൈൻ ഡിലിമിറ്റഡ് JSON വായിക്കുക |
| ഏകീകൃത സ്റ്റ്രീമിംഗ് പ്രോട്ടോകോൾ | Python, JavaScript, C# ബാക്ക്‌എൻഡുകൾ ഏക തരം സന്ദേശ ഫോർമാറ്റ് പുറപ്പെടുവിക്കുക ഉറപ്പുവരുത്തുക |
| സ്ഥിരം UI അപ്‌ഡേറ്റുകൾ | ഏജൻറ് സ്റ്റാറ്റസ് ബാഡ്ജുകളും ലേഖന ടോക്കൻ പിന്തുടരെയാണ് ടെക്സ്റ്റ് സ്ട്രീം ചെയ്യുക |
| CLI ആപ്പിന് HTTP ലെയർ ചേർക്കുക | നിലവിലുള്ള ഓർക്കസ്ട്രേറ്റർ ലജിക് Express-ശൈലിയിൽ (JS) അല്ലെങ്കിൽ ASP.NET Core മിനിമൽ API (C#) ൽ റാപ്പ് ചെയ്യുക |

---

## ആർക്കിടെക്ചർ

UI ഒരു സിംഗിൾ സ്റ്റാറ്റിക് ഫയൽ സെറ്റ് ആണ് (`index.html`, `style.css`, `app.js`), എല്ലാത്ത്രി ബാക്ക്‌എൻഡുകളും പങ്കുവെക്കുന്നു. ഓരോ ബാക്ക്‌എൻഡും ഒരേ രണ്ട് റൂട്ടുകൾ വെളിപ്പെടുത്തുന്നു:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| റൂട്ടു | രീതിയ്‌ക്കം | ഉദ്ദേശ്യം |
|--------|--------------|-----------|
| `/` | GET | സ്റ്റാറ്റിക് UI സർവ് ചെയ്യുക |
| `/api/article` | POST | മൾട്ടി-ഏജൻ്റ് പൈപ്പ്‌ലൈനിൽ ഓടിച്ച് NDJSON സ്ട്രീം ചെയ്യുക |

ഫ്രണ്ട്എൻഡ് ഒരു JSON ബോഡി അയയ്ക്കും, പ്രതികരണത്തെ ന്യൂലൈൻ ഡിലിമിറ്റഡ് JSON സന്ദേശങ്ങളുടെ സ്ട്രീമായി വായിക്കും. ഓരോ സന്ദേശത്തിലും `type` ഫീൽഡ് ഉണ്ട്, UI ശരിയായ പാനൽ അപ്‌ഡേറ്റ് ചെയ്യാനായി ഉപയോഗിക്കുന്നു:

| സന്ദേശ തരം | അർത്ഥം |
|------------|---------|
| `message` | സ്റ്റാറ്റസ് അപ്‌ഡേറ്റ് (ഉദാ: "Starting researcher agent task...") |
| `researcher` | റിസർച്ച് ഫലങ്ങൾ തയാറായിരിക്കുന്നു |
| `marketing` | പ്രോഡക്ട് സെർച്ച് ഫലങ്ങൾ തയാറായി |
| `writer` | എഴുത്തുകാരൻ ആരംഭിച്ചിട്ടുണ്ട് അല്ലെങ്കിൽ പൂർത്തിയായി (`{ start: true }` അല്ലെങ്കിൽ `{ complete: true }` ഉൾക്കൊള്ളുന്നു) |
| `partial` | എഴുത്തുകാരനിൽ നിന്നുള്ള ഒറ്റ സ്ട്രീം ടോക്കൺ (`{ text: "..." }` ഉൾക്കൊള്ളുന്നു) |
| `editor` | എഡിറ്റർ വിധി തയാറായി |
| `error` | എന്തെങ്കിലും പിഴവ് സംഭവിച്ചു |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## മുൻകൂട്ടി അറിയേണ്ടത്

- [Part 7: Zava Creative Writer](part7-zava-creative-writer.md) പൂർത്തിയാക്കുക
- Foundry Local CLI ഇൻസ്റ്റാൾ ചെയ്‌തിട്ടുണ്ട്, `phi-3.5-mini` മോഡലും ഡൗൺലോഡ് ചെയ്‌തിട്ടുണ്ട്
- ഒരു ആധുനിക വെബ് ബ്രൗസർ (Chrome, Edge, Firefox, Safari)

---

## പങ്കുവെക്കുന്ന UI

ഏതെങ്കിലും ബാക്ക്‌എൻഡ് കോഡിൽ മടക്കമില്ലാതെ മുന്നോട്ട് പോവുന്നതിന് മുമ്പ്, എല്ലാ ഭാഷ ട്രാക്കുകളും ഉപയോഗിക്കുന്ന ഫ്രണ്ട്എൻഡ് ഒരു നിമിഷം പരിശോധിക്കുക. ഫയലുകൾ `zava-creative-writer-local/ui/` ലാണ്:

| ഫയൽ | ഉദ്ദേശ്യം |
|--------|----------|
| `index.html` | പേജ് ലേയൗട്ട്: ഇൻപുട്ട് ഫോം, ഏജൻറ് സ്റ്റാറ്റസ് ബാഡ്ജുകൾ, ലേഖന ഔട്ട്പുട്ട് ഏരിയ, കോളാപ്സിബിൾ ഡീട്ടെയിൽ പാനലുകൾ |
| `style.css` | മിനിമൽ സ്റ്റൈലിംഗ്, സ്റ്റാറ്റസ്-ബാഡ്ജ് നിറങ്ങൾ (വൈറ്റിംഗ്, റണ്ണിംഗ്, ഡൺ, എറർ) |
| `app.js` | Fetch കോളുകൾ, `ReadableStream` ലൈൻ റീഡർ, DOM അപ്‌ഡേറ്റ് ലജിക്ക് |

> **ടിപ്പ്:** ബ്രൗസറിൽ നേരിട്ട് `index.html` തുറക്കുക ലേയൗട്ട് പ്രിവ്യൂ ചെയ്യാൻ. പക്ഷേ ബാക്ക്‌എൻഡ് ഇല്ലാതിരുന്നതുകൊണ്ട് ഇപ്പോൾ ഒന്നും പ്രവർത്തിക്കുകയില്ല, പക്ഷേ ഘടന കാണാനാകും.

### സ്ട്രീം റീഡർ എങ്ങനെ പ്രവർത്തിക്കുന്നു

`app.js`-ലുള്ള പ്രധാന ഫംഗ്ഷൻ Chunk-ചടങ്ങായി ജവാബി ബോഡി വായിക്കുകയും ന്യൂലൈൻ സീമകളിൽ വിഭജിക്കുകയും ചെയ്യും:

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
    buffer = lines.pop(); // അപൂര്‍ണ്ണമായ ട്രെയിലിംഗ് ലൈന്‍ സംരക്ഷിക്കുക

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

പ്രത്യേകമായി തിരിഞ്ഞ സന്ദേശങ്ങൾ `handleMessage()`-ലേക്ക് അയയ്‌ക്കുന്നു, അത് `msg.type` അടിസ്ഥാനമാക്കി അനുയോജ്യമായ DOM എൻറ്റിറ്റി അപ്‌ഡേറ്റ് ചെയ്യും.

---

## വ്യായാമങ്ങൾ

### വ്യായാമം 1: Python ബാക്ക്‌എൻഡ് UI-യോടുകൂടി പ്രവർത്തിപ്പിക്കുക

Python (FastAPI) പതിപ്പ് ഇതിനകം സ്ട്രീമിംഗ് API എന്റ്പോയിന്റ് ഉണ്ട്. മാറ്റം ഒരു സ്വന്തമായാണ് — `ui/` ഫോൾഡർ സ്റ്റാറ്റിക് ഫയലുകൾ ആയി മൗണ്ട് ചെയ്യുക.

**1.1** Python API ഡയറക്ടറിയിലേക്ക് ചെന്നു ഡെപൻഡൻസികൾ ഇൻസ്റ്റാൾ ചെയ്യുക:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** സെർവർ ആരംഭിക്കുക:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** ബ്രൗസറിൽ തുറക്കുക `http://localhost:8000`. Zava Creative Writer UI മൂന്ന് ടെക്സ്റ്റ് ഫീൽഡുകളും “Generate Article” ബട്ടൺ കാണാം.

**1.4** മുന്‍വിധീകരിച്ച മൂല്യങ്ങൾ ഉപയോഗിച്ച് **Generate Article** ക്ലിക്ക്പൊക്കുക. ഏജന്റ് സ്റ്റാറ്റസ് ബാഡ്ജുകൾ "Waiting" മുതൽ "Running" ആയി തുടരുകയും, ഓരോ ഏജന്റിന്റെയും പ്രവർത്തനം പൂർത്തിയായപ്പോൾ "Done" ആകുകയും ചെയ്യും. ലേഖന്റെ ടെക്സ്റ്റ് ടോക്കൺ അനുസരിച്ച് ഔട്ട്പുട്ട് പാനലിലേക്ക് സ്ട്രീം ചെയ്യും.

> **പരിശോധിക്കേണ്ടത്:** പേജ് UI-വിന്റെ പകരം JSON പ്രതികരണം കാണിച്ചാൽ, അപ്‌ഡേറ്റ് ചെയ്ത `main.py` ഇനം സ്റ്റാറ്റിക് ഫയലുകൾ മൗണ്ട് ചെയ്യുന്നതാണെന്ന് ഉറപ്പ് വരുത്തുക. `/api/article` എന്റ്പോയിന്റ് ഇപ്പോഴും അതേ പാതയിലുണ്ട്; സ്റ്റാറ്റിക് ഫയൽ മൗണ്ട് ഏതൊരു മറ്റൊരു റൂട്ടിലും UI സർവ് ചെയ്യും.

**എങ്ങനെ പ്രവർത്തിക്കുന്നു:** പുതുക്കിയ `main.py`-ലൊരു സിംഗിൾ ലൈനാണ് ചേർക്കുന്നത്:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

ഇത് `zava-creative-writer-local/ui/` ഫയൽകൾ സ്റ്റാറ്റിക് ആസ്തികളായി സർവ് ചെയ്യുന്നു, ഡിഫോൾട്ട് ഡോക്യുമെന്റ് ആയി `index.html` ഉപയോഗിക്കുന്നു. `/api/article` POST റൂട്ടു സ്റ്റാറ്റിക് മൗണ്ടിന് മുമ്പ് രജിസ്‌റ്റർ ചെയ്തതിനാൽ പ്രാഥമ്യം ലഭിക്കുന്നു.

---

### വ്യായാമം 2: ജാവസ്ക്രിപ്റ്റ് വേരിയന്റിന് വെബ് സെർവർ ചേർക്കുക

ജാവാസ്ക്രിപ്റ്റ് വേരിയന്റ് ഇപ്പോൾ CLI അപ്ലിക്കേഷനാണ് (`main.mjs`). പുതിയ ഫയൽ `server.mjs` സമാന ഏജന്റ് മോഡ്യൂളുകൾ HTTP സെർവറിന് പിന്നിൽ റാപ്പ് ചെയ്ത് പങ്കുവെക്കുന്ന UI സർവ് ചെയ്യും.

**2.1** ജാവാസ്ക്രിപ്റ്റ് ഡയറക്ടറിയിലേക്ക് പോയി ഡെപൻഡൻസികൾ ഇൻസ്റ്റാൾ ചെയ്യുക:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** വെബ് സെർവർ തുടങ്ങുക:

```bash
node server.mjs
```

```powershell
node server.mjs
```

താഴെ കാണിച്ച വിവരം ലഭിക്കും:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** ബ്രൗസറിൽ `http://localhost:3000` തുറന്ന് **Generate Article** ക്ലിക്ക് ചെയ്യുക. ജാവാസ്ക്രിപ്റ്റ് ബാക്ക്‌എൻഡിനെതിരെ ഒരൊറ്റ UI പ്രവർത്തനം പ്രവർത്തിക്കും.

**കോഡ് പഠിക്കുക:** `server.mjs` തുറക്കുക, പ്രധാന പാറ്റേണുകൾ ശ്രദ്ധിക്കുക:

- **സ്റ്റാറ്റിക് ഫയൽ സർവ്വിംഗ്** Node.js നിർമ്മിത `http`, `fs`, `path` മോഡ്യൂളുകൾ ഉപയോഗിച്ച് പുറത്തുള്ള ഫ്രെയിംവർക്കുകൾ കൂടാതെ
- **പാത്ത്-ട്രാവേഴ്സൽ സംരക്ഷണം** ആവശ്യപ്പെട്ട പാത്ത് നൊര്‍മലൈസ് ചെയ്ത് `ui/` ഡയറക്ടറിയിൽ ഉള്ളതാണ് എന്ന് ഉറപ്പാക്കുന്നു
- **NDJSON സ്ട്രീമിങ്** ഓരോ ഒബ്ജെക്ടും സീരിയലൈസ് ചെയ്ത് അതിലെ പുതിയ വരികൾ നീക്കം ചെയ്ത് ട്രെയിലിംഗ് ന്യൂലൈൻ ചേർക്കുന്ന `sendLine()` സഹായക ഫംഗ്ഷൻ ഉപയോഗിക്കുന്നു
- **ഏജന്റ് ഓർക്കസ്ട്രേഷൻ** നിലവിലുള്ള `researcher.mjs`, `product.mjs`, `writer.mjs`, `editor.mjs` മോഡ്യൂളുകൾ മാറ്റമില്ലാതെ നന്നായി വീണ്ടും ഉപയോഗിക്കുന്നു

<details>
<summary>server.mjs-ൽ നിന്നും പ്രധാന ഭാഗം</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// ഗവേഷകന്‍
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// എഴുത്തുകാരന്‍ (സ്ട്രീമിങ്)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### വ്യായാമം 3: C# വേരിയന്റിന് മിനിമൽ API ചേർക്കുക

C# വേരിയന്റ് ഇപ്പോൾ കൺസോൾ അപ്ലിക്കേഷൻ ആണ്. പുതിയ പ്രോജക്ട് `csharp-web` ASP.NET Core മിനിമൽ APIകൾ ഉപയോഗിച്ച് വെബ് സേവനമായി സമാന പൈപ്പ്‌ലൈനിൽ പുറത്തുവിടുന്നു.

**3.1** C# വെബ് പ്രോജക്ടിലേക്കു പോകുക, പാക്കേജുകൾ റസ്റ്റോർ ചെയ്യുക:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** വെബ് സെർവർ റൺ ചെയ്യുക:

```bash
dotnet run
```

```powershell
dotnet run
```

താഴെ കാണൂ:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** ബ്രൗസറിൽ `http://localhost:5000` തുറന്ന് **Generate Article** ക്ലിക് ചെയ്യുക.

**കോഡ് പഠിക്കുക:** `csharp-web` ഡയറക്ടറിയിലുള്ള `Program.cs` തുറക്കുക, ശ്രദ്ധിക്കുക:

- പ്രോജക്ട് ഫയൽ `Microsoft.NET.Sdk.Web` ഉപയോഗിക്കുന്നു `Microsoft.NET.Sdk` എന്നതിനു പകരം, ASP.NET Core പിന്തുണ നൽകാൻ
- സ്റ്റാറ്റിക് ഫയലുകൾ `UseDefaultFiles`, `UseStaticFiles` വഴിയില്‍ പങ്കുവെക്കുന്ന `ui/` ഡയറക്ടറിയിലേക്ക് പോയിന്റ് ചെയ്തു സർവ് ചെയ്യുന്നു
- `/api/article` എന്റ്പോയിന്റ് NDJSON വരികൾ നേരിട്ട് `HttpContext.Response`-ലേക്ക് എഴുതുന്നു, റിയൽ ടൈം സ്ട്രീമിങ്ങിനായി ഓരോ വരിയും ഫ്ലഷ് ചെയ്യുന്നു
- എല്ലാ ഏജന്റ് ലജിക് (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) കൺസോൾ വേർഷനിലെ പോലെയാണ്

<details>
<summary>csharp-web/Program.cs-ൽ നിന്നും പ്രധാന ഭാഗം</summary>

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

### വ്യായാമം 4: ഏജന്റ് സ്റ്റാറ്റസ് ബാഡ്ജുകൾ പരിശോധിക്കുക

ഇപ്പോൾ പ്രവർത്തനക്ഷമമായ UI ഉണ്ട്, ഫ്രണ്ട്എൻഡ് എങ്ങനെ സ്റ്റാറ്റസ് ബാഡ്ജുകൾ അപ്‌ഡേറ്റ് ചെയ്യുന്നതാവോ നോക്കുക.

**4.1** `zava-creative-writer-local/ui/app.js` നിങ്ങളുടെ എഡിറ്ററിൽ തുറക്കുക.

**4.2** `handleMessage()` ഫംഗ്ഷൻ കണ്ടെത്തുക. മേധാവിത്വം അയക്കുന്ന സന്ദേശ തരം DOM അപ്‌ഡേറ്റുകളിലേക്ക് എങ്ങനെ മാപ്പ് ചെയ്യുന്നു നോക്കുക:

| സന്ദേശ തരം | UI പ്രവർത്തനം |
|------------|---------------|
| "researcher" ഉള്ള `message` | Researcher ബാഡ്ജ് "Running" ആക്കുന്നു |
| `researcher` | Researcher ബാഡ്ജ് "Done" ആക്കി, Research Results പാനൽ പൂരിപ്പിക്കുന്നു |
| `marketing` | Product Search ബാഡ്ജ് "Done" ആക്കി, Product Matches പാനൽ പൂരിപ്പിക്കുന്നു |
| `writer` കൂടെ `data.start` | Writer ബാഡ്ജ് "Running" ആക്കി, ലേഖന ഔട്ട്പുട്ട് ക്ലിയർ ചെയ്യുന്നു |
| `partial` | ടോക്കൺ ടെക്സ്റ്റ് ലേഖന ഔട്ട്പുട്ടിൽ ചേർക്കുന്നു |
| `writer` കൂടെ `data.complete` | Writer ബാഡ്ജ് "Done" ആക്കും |
| `editor` | Editor ബാഡ്ജ് "Done" ആക്കി, Editor Feedback പാനൽ പൂരിപ്പിക്കുന്നു |

**4.3** ലേഖനത്തിന് താഴെയുള്ള കോളാപ്സിബിൾ “Research Results”, “Product Matches”, “Editor Feedback” പാനലുകൾ തുറന്ന് ഓരോ ഏജന്റും പിറവിപ്പിച്ച കയറ്റം പരിശോധിക്കുക.

---

### വ്യായാമം 5: UI അനുയോജനം (Stretch)

ഈ കുറെ മാറ്റങ്ങൾ പരീക്ഷിച്ച് നോക്കുക:

**5.1 ഒരു വാക്കുകൾ എണ്ണൽ ചേർക്കുക.** Writer പൂർത്തിയാക്കിയതിന് ശേഷം, ലേഖന വാക്കുകൾ എണ്ണികാണിക്കുക ഔട്ട്പുട്ട് പാനലിനു താഴെ. ഇത് `handleMessage`-ൽ type === "writer" && data.complete യ ആയി പരിശോധിച്ച് കണക്കാക്കാം:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 റിട്രൈ സൂചകം ചേർക്കുക.** Editor പുനഃപരിശോധന ആവശ്യപ്പെടുമ്പോൾ പൈപ്പ്‌ലൈൻ വീണ്ടും ഓടും. സ്റ്റാറ്റസ് പാനലിൽ "Revision 1" അല്ലെങ്കിൽ "Revision 2" ബാനർ പ്രദർശിപ്പിക്കുക. "Revision" ഉള്ള `message` തരം കേട്ട് പുതിയ DOM ഘടകം അപ്‌ഡേറ്റ് ചെയ്യുക.

**5.3 ഡാർക്ക് മോഡ്.** ടോഗിൾ ബട്ടൺ ചേർക്കുക `<body>`-ൽ `.dark` ക്ലാസും ചേർക്കുക. `style.css`-ൽ `body.dark` സെലക്ടർ ഉപയോഗിച്ച് പശ്ചാത്തലം, ടെക്സ്റ്റ്, പാനൽ നിറങ്ങൾ ഒভার്റൈഡ് ചെയ്യുക.

---

## സംഗ്രഹം

| നിങ്ങൾ ചെയ്തത് | എങ്ങനെ |
|--------------|---------|
| Python ബാക്ക്‌എൻഡിൽ നിന്ന് UI സർവ് ചെയ്‌തു | FastAPI-യിൽ `StaticFiles` ഉപയോഗിച്ച് `ui/` ഫോൾഡർ മൗണ്ട് ചെയ്തു |
| ജാവാസ്‌ക്രിപ്റ്റ് വേരിയന്റിൽ HTTP സെർവർ ചേർത്തു | Node.js ബിൽറ്റ് ഇൻ `http` മോഡ്യൂൾ ഉപയോഗിച്ച് `server.mjs` സൃഷ്‌ടിച്ചു |
| C# വേരിയന്റിൽ വെബ് API ചേർത്തു | ASP.NET Core മിനിമൽ APIകൾ ഉപയോഗിച്ച് പുതിയ `csharp-web` പ്രോജക്ട് സൃഷ്‌ടിച്ചു |
| ബ്രൗസറിൽ സ്ട്രീമിംഗ് NDJSON ഉപയോഗിച്ചു | `fetch()` with `ReadableStream` അധികം ലൈൻ പാഴ്സിങ് ചെയ്തു |
| UI റിയൽ ടൈമിൽ അപ്‌ഡേറ്റ് ചെയ്തു | സന്ദേശ തരം DOM അപ്‌ഡേറ്റുകളിലേക്ക് (ബാഡ്ജുകൾ, ടെക്സ്റ്റ്, കോളാപ്സിബിൾ പാനലുകൾ) മാപ്പ് ചെയ്തു |

---

## പ്രധാന പഠിപ്പുകൾ

1. ഒരു **പങ്കിടുന്ന സ്റ്റാറ്റിക് ഫ്രണ്ട്എൻഡ്** ഏതൊരു ബാക്ക്‌എൻഡുമായും കൂടെ പ്രവര്‍ത്തിക്കാനാകും, ഒപ്പം OpenAI-സമ്മതമാക്കിയ API മാതൃകയുടെ മൂല്യം ശക്തമാക്കുന്നു.
2. **ന്യൂലൈൻ-ഡിലിമിറ്റഡ് JSON (NDJSON)** ബ്രൗസറിലെ `ReadableStream` API-യുമായി നിവർത്തിയുള്ള, എളുപ്പമുള്ള സ്ട്രീമിംഗ് ഫോർമാറ്റാണ്.
3. **Python വേരിയന്റിൽ** രണ്ടുരൂപ മാറ്റം കുറഞ്ഞത്; FastAPI എന്റ്പോയിന്റ് ഇതിനകം ഉണ്ടായിരുന്നു; ജാവാസ്ക്രിപ്റ്റ്, C# വേരിയന്റിന് HTTP റാപ്പർ തThin ആയി ചേർക്കേണ്ടിവന്നു.
4. UI **വാനില്ല HTML/CSS/JS ആയി സൂക്ഷിച്ചത്** ബിൽഡ് ടൂൾസ്, ഫ്രെയിംവർക്ക് ആശ്രിതങ്ങൾ, വർക്ഷോപ്പ് പഠിതാക്കൾക്കുള്ള അധിക സങ്കീർണ്ണത ഒഴിവാക്കി.
5. Researcher, Product, Writer, Editor എന്നിങ്ങനെ ഏജന്റ് മൊഡ്യൂളുകൾ മാറ്റം വരുത്താതെ വീണ്ടും ഉപയോഗിച്ചു; മാറ്റം റാലിവരെയുള്ള പരിവഹനം മാത്രം.

---

## അധിക വായന

| വിഭവം | ലിങ്ക് |
|--------|-------|
| MDN: Readable Streams ഉപയോഗിക്കൽ | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI സ്റ്റാറ്റിക് ഫയലുകൾ | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core സ്റ്റാറ്റിക് ഫയലുകൾ | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON സ്പെസിഫിക്കേഷൻ | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

വർക്ക്ഷോപ്പ് മുഴുവൻ നിർമ്മിച്ചതിന്റെ സംഗ്രഹത്തിനായി [Part 13: Workshop Complete](part13-workshop-complete.md) തുടരുക.

---
[← ഭാഗം 11: ടൂൾ വിളിക്കൽ](part11-tool-calling.md) | [ഭാഗം 13: വർക്ക്‌ഷോപ്പ് സമ്പൂരം →](part13-workshop-complete.md)