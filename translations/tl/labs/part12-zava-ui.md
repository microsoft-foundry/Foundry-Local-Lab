![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagi 12: Paggawa ng Web UI para sa Zava Creative Writer

> **Layunin:** Magdagdag ng browser-based na front end sa Zava Creative Writer upang mapanood mo ang multi-agent pipeline na tumatakbo nang real time, na may live na mga badge ng katayuan ng ahente at na-stream na teksto ng artikulo, lahat ay pinagsilbihan mula sa isang lokal na web server.

Sa [Bahagi 7](part7-zava-creative-writer.md) na-explore mo ang Zava Creative Writer bilang isang **CLI application** (JavaScript, C#) at isang **headless API** (Python). Sa lab na ito, ikokonekta mo ang isang shared na **vanilla HTML/CSS/JavaScript** na front end sa bawat backend para makipag-interact ang mga user sa pipeline gamit ang browser sa halip na terminal.

---

## Ano ang Matututunan Mo

| Layunin | Deskripsyon |
|---------|-------------|
| Mag-serve ng static files mula sa backend | I-mount ang HTML/CSS/JS directory kasabay ng iyong API route |
| Gumamit ng streaming NDJSON sa browser | Gamitin ang Fetch API na may `ReadableStream` para basahin ang newline-delimited JSON |
| Pagsamahin ang streaming protocol | Siguraduhing ang Python, JavaScript, at C# backends ay naglalabas ng parehas na format ng mensahe |
| Progressive UI updates | I-update ang mga badge ng status ng agent at i-stream ang teksto ng artikulo token-by-token |
| Magdagdag ng HTTP layer sa CLI app | Balutin ang umiiral na orchestrator logic sa isang Express-style server (JS) o ASP.NET Core minimal API (C#) |

---

## Arkitektura

Ang UI ay isang set ng static files (`index.html`, `style.css`, `app.js`) na ginagamit ng lahat ng tatlong backend. Bawat backend ay may dalawang route:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| Ruta | Metodo | Layunin |
|-------|--------|---------|
| `/` | GET | Nagse-serve ng static UI |
| `/api/article` | POST | Pinapatakbo ang multi-agent pipeline at nag-stream ng NDJSON |

Ang front end ay nagpapadala ng JSON na body at binabasa ang sagot bilang stream ng newline-delimited JSON na mga mensahe. Bawat mensahe ay may field na `type` na ginagamit ng UI para i-update ang tamang panel:

| Uri ng Mensahe | Kahulugan |
|----------------|-----------|
| `message` | Update sa status (hal. "Nagsisimula ang researcher agent task...") |
| `researcher` | Handa na ang resulta ng research |
| `marketing` | Handa na ang resulta ng product search |
| `writer` | Nagsimula o natapos ang writer (may `{ start: true }` o `{ complete: true }`) |
| `partial` | Isang streamed na token mula sa Writer (may `{ text: "..." }`) |
| `editor` | Handa na ang hatol ng editor |
| `error` | May nangyaring mali |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## Mga Kailangan

- Tapusin ang [Bahagi 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Na-install ang Foundry Local CLI at na-download ang modelong `phi-3.5-mini`
- Isang modernong web browser (Chrome, Edge, Firefox, o Safari)

---

## Ang Shared UI

Bago hawakan ang anumang backend code, maglaan ng sandali para i-explore ang front end na gagamitin ng tatlong language tracks. Ang mga files ay nasa `zava-creative-writer-local/ui/`:

| File | Layunin |
|------|---------|
| `index.html` | Layout ng pahina: input form, mga badge ng katayuan ng ahente, lugar para sa output ng artikulo, mga collapsible detail panels |
| `style.css` | Minimal na styling gamit ang kulay ng status badge (waiting, running, done, error) |
| `app.js` | Fetch call, `ReadableStream` line reader, at lohika sa pag-update ng DOM |

> **Tip:** Buksan ang `index.html` nang direkta sa iyong browser para makita ang layout nang preview. Hindi pa ito gagana dahil wala pang backend, pero makikita mo ang istruktura.

### Paano Gumagana ang Stream Reader

Ang pangunahing function sa `app.js` ay binabasa ang response body chunk-by-chunk at naghahati sa bawat newline:

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
    buffer = lines.pop(); // panatilihin ang hindi kumpletong huling linya

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

Ang bawat parsed na mensahe ay ipinapasa sa `handleMessage()`, na ina-update ang kaukulang elemento sa DOM batay sa `msg.type`.

---

## Mga Ehersisyo

### Ehersisyo 1: Patakbuhin ang Python Backend gamit ang UI

Ang Python (FastAPI) variant ay may streaming API endpoint na. Ang tanging pagbabago ay ang pag-mount ng `ui/` folder bilang static files.

**1.1** Pumunta sa Python API directory at i-install ang mga dependencies:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Patakbuhin ang server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Buksan ang browser sa `http://localhost:8000`. Makikita mo dapat ang Zava Creative Writer UI na may tatlong text fields at isang "Generate Article" na button.

**1.4** I-click ang **Generate Article** gamit ang default na mga halaga. Panoorin ang mga badge ng status ng agent na magbago mula "Waiting" papuntang "Running" hanggang "Done" habang natatapos ang bawat ahente, at makita ang teksto ng artikulo na na-stream sa output panel token by token.

> **Pag-aayos ng Problema:** Kung nagpapakita ang page ng JSON response sa halip na UI, siguraduhing pinapatakbo mo ang updated na `main.py` na nag-mount ng static files. Gumagana pa rin ang `/api/article` endpoint sa orihinal nitong path; ang static file mount ay nagsisilbi sa UI sa lahat ng ibang ruta.

**Paano ito gumagana:** Ang updated na `main.py` ay nagdadagdag ng isang linya sa ibaba:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Ito ay nagseserbisyo sa bawat file mula sa `zava-creative-writer-local/ui/` bilang static asset, na may `index.html` bilang default na dokumento. Ang `/api/article` POST route ay narehistro bago pa ang static mount kaya ito ay mas prayoridad.

---

### Ehersisyo 2: Magdagdag ng Web Server sa JavaScript Variant

Ang JavaScript variant ay kasalukuyang CLI application (`main.mjs`). Isang bagong file, `server.mjs`, ang binalot ang mga parehong agent modules sa likod ng HTTP server at nagseserbisyo ng shared UI.

**2.1** Pumunta sa JavaScript directory at i-install ang mga dependencies:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Patakbuhin ang web server:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Makikita mo dapat:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Buksan ang `http://localhost:3000` sa iyong browser at i-click ang **Generate Article**. Gumagana ang parehong UI nang parehas sa JavaScript backend.

**Pag-aralan ang code:** Buksan ang `server.mjs` at pansinin ang mga pangunahing pattern:

- **Static file serving** ay gumagamit ng built-in na Node.js na `http`, `fs`, at `path` modules nang walang karagdagang external framework.
- **Path-traversal protection** ay nagno-normalize ng hiniling na path at sinisigurong nananatili ito sa loob ng `ui/` directory.
- **NDJSON streaming** ay gumagamit ng `sendLine()` helper na nagsasaayos ng bawat object, tinatanggal ang internal na newlines, at nagdadagdag ng trailing newline.
- **Agent orchestration** ay nire-reuse ang umiiral na `researcher.mjs`, `product.mjs`, `writer.mjs`, at `editor.mjs` modules nang walang pagbabago.

<details>
<summary>Key excerpt from server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Mananaliksik
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Manunulat (nagsasahimpapawid)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Ehersisyo 3: Magdagdag ng Minimal API sa C# Variant

Ang C# variant ay kasalukuyang console application. Isang bagong proyekto, `csharp-web`, ang gumagamit ng ASP.NET Core minimal APIs para i-expose ang parehong pipeline bilang web service.

**3.1** Pumunta sa C# web project at i-restore ang mga package:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Patakbuhin ang web server:

```bash
dotnet run
```

```powershell
dotnet run
```

Makikita mo dapat:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Buksan ang `http://localhost:5000` sa iyong browser at i-click ang **Generate Article**.

**Pag-aralan ang code:** Buksan ang `Program.cs` sa `csharp-web` directory at pansinin:

- Ginagamit ng project file ang `Microsoft.NET.Sdk.Web` sa halip na `Microsoft.NET.Sdk`, na nagdadagdag ng ASP.NET Core support.
- Ang mga static files ay sine-serve gamit ang `UseDefaultFiles` at `UseStaticFiles` na nakaturo sa shared na `ui/` directory.
- Ang `/api/article` endpoint ay sumusulat ng mga NDJSON line nang diretso sa `HttpContext.Response` at nag-flush pagkatapos ng bawat linya para sa real-time streaming.
- Ang lahat ng agent logic (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) ay kapareho ng console version.

<details>
<summary>Key excerpt from csharp-web/Program.cs</summary>

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

### Ehersisyo 4: I-explore ang Agent Status Badges

Ngayon na may gumaganang UI ka na, tingnan kung paano ina-update ng front end ang mga status badges.

**4.1** Buksan ang `zava-creative-writer-local/ui/app.js` sa iyong editor.

**4.2** Hanapin ang function na `handleMessage()`. Mapapansin mo kung paano nito nire-redirect ang mga uri ng mensahe sa mga update sa DOM:

| Uri ng Mensahe | Aksyon sa UI |
|----------------|--------------|
| `message` na naglalaman ng "researcher" | Itinatakda ang Researcher badge na "Running" |
| `researcher` | Itinatakda ang Researcher badge na "Done" at pinupuno ang Research Results panel |
| `marketing` | Itinatakda ang Product Search badge na "Done" at pinupuno ang Product Matches panel |
| `writer` na may `data.start` | Itinatakda ang Writer badge na "Running" at nililinis ang output ng artikulo |
| `partial` | Dinadagdag ang token text sa output ng artikulo |
| `writer` na may `data.complete` | Itinatakda ang Writer badge na "Done" |
| `editor` | Itinatakda ang Editor badge na "Done" at pinupuno ang Editor Feedback panel |

**4.3** Buksan ang mga collapsible na panel na "Research Results", "Product Matches", at "Editor Feedback" sa ibaba ng artikulo para matingnan ang raw JSON na ginawa ng bawat ahente.

---

### Ehersisyo 5: I-customize ang UI (Stretch)

Subukan ang isa o higit pang mga sumusunod na pagbuti:

**5.1 Magdagdag ng word count.** Pagkatapos matapos ang Writer, ipakita ang bilang ng salita ng artikulo sa ilalim ng output panel. Maaari mo itong i-compute sa `handleMessage` kapag `type === "writer"` at `data.complete` ay totoo:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Magdagdag ng retry indicator.** Kapag humiling ang Editor ng rebisyon, muling tumatakbo ang pipeline. Ipakita ang banner na "Revision 1" o "Revision 2" sa status panel. Makinig para sa uri ng `message` na naglalaman ng "Revision" at i-update ang bagong DOM element.

**5.3 Dark mode.** Magdagdag ng toggle button at isang `.dark` class sa `<body>`. I-override ang background, text, at kulay ng panel sa `style.css` gamit ang `body.dark` selector.

---

## Buod

| Ano ang ginawa mo | Paano |
|-------------------|-------|
| Nag-serve ng UI mula sa Python backend | Naka-mount ang `ui/` folder gamit ang `StaticFiles` sa FastAPI |
| Nagdagdag ng HTTP server sa JavaScript variant | Gumawa ng `server.mjs` gamit ang built-in na Node.js `http` module |
| Nagdagdag ng web API sa C# variant | Gumawa ng bagong `csharp-web` project gamit ang ASP.NET Core minimal APIs |
| Gumamit ng streaming NDJSON sa browser | Ginamit ang `fetch()` na may `ReadableStream` at line-by-line JSON parsing |
| Naka-update ang UI nang real time | Kino-connect ang uri ng mensahe sa mga update sa DOM (badges, teksto, collapsible panels) |

---

## Pangunahing Mga Aral

1. Ang **shared static front end** ay maaaring gumana sa anumang backend na gumagamit ng parehong streaming protocol, pinapalakas ang halaga ng OpenAI-compatible API na pattern.
2. Ang **Newline-delimited JSON (NDJSON)** ay isang simpleng streaming format na katutubong gumagana sa browser `ReadableStream` API.
3. Ang **Python variant** ang may pinakamaliit na pagbabago dahil may FastAPI na itong endpoint; ang mga JavaScript at C# variants ay kailangan ng manipis na HTTP wrapper.
4. Ang pagkopya ng UI bilang **vanilla HTML/CSS/JS** ay nakakaiwas sa build tools, dependencies ng framework, at dagdag na komplikasyon para sa mga learner ng workshop.
5. Ang parehong mga agent modules (Researcher, Product, Writer, Editor) ay nire-reuse nang walang pagbabago; ang transport layer lang ang nagbabago.

---

## Karagdagang Basahin

| Resource | Link |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Magpatuloy sa [Bahagi 13: Workshop Complete](part13-workshop-complete.md) para sa isang buod ng lahat ng iyong nagawa sa workshop na ito.

---
[← Bahagi 11: Pagtawag ng Kasangkapan](part11-tool-calling.md) | [Bahagi 13: Kumpleto ang Workshop →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Paunawa**:  
Ang dokumentong ito ay isinalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagamat nagsusumikap kami para sa katumpakan, pakitandaan na ang mga awtomatikong pagsasalin ay maaaring maglaman ng mga pagkakamali o hindi pagkakatugma. Ang orihinal na dokumento sa kanyang orihinal na wika ang dapat ituring na pinagmumulan ng katotohanan. Para sa mahahalagang impormasyon, inirerekomenda ang propesyonal na human translation. Hindi kami mananagot sa anumang hindi pagkakaintindihan o maling interpretasyon na nagmula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->