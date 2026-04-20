![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deel 12: Een web-ui bouwen voor de Zava Creative Writer

> **Doel:** Voeg een browser-gebaseerde front-end toe aan de Zava Creative Writer zodat je de multi-agent pijplijn in real time kunt volgen, met live agent status badges en gestreamde artikeltekst, alles geserveerd vanaf één lokale webserver.

In [Deel 7](../../../labs/part7-zava-creative-writer.md heb je de Zava Creative Writer verkend als een **CLI-toepassing** (JavaScript, C) en als een **headless API** (Python). In deze oefening verbind je een gedeelde **vanilla HTML/CSS/JavaScript** front-end met elke backend zodat gebruikers met de pijplijn via een browser kunnen interacteren in plaats van via een terminal.

---

## Wat je gaat leren

| Doel | Beschrijving |
|-----------|-------------|
| Statische bestanden serveren vanuit een backend | Monteer een HTML/CSS/JS map naast je API route |
| NDJSON streaming in de browser consumeren | Gebruik de Fetch API met `ReadableStream` om newline-gescheiden JSON te lezen |
| Uniform streaming protocol | Zorg dat Python, JavaScript en C# backends hetzelfde berichtformaat uitzenden |
| Progressieve UI-updates | Update agent status badges en stream artikeltekst token voor token |
| Voeg een HTTP-laag toe aan een CLI-app | Wikkel bestaande orkestratie-logica in een Express-achtige server (JS) of ASP.NET Core minimal API (C#) |

---

## Architectuur

De UI is een enkele set statische bestanden (`index.html`, `style.css`, `app.js`) gedeeld door alle drie backends. Elke backend biedt dezelfde twee routes aan:

![Zava UI architectuur — gedeelde front end met drie backends](../../../images/part12-architecture.svg)

| Route | Methode | Doel |
|-------|--------|---------|
| `/` | GET | Dient de statische UI |
| `/api/article` | POST | Draait de multi-agent pijplijn en streamt NDJSON |

De front-end stuurt een JSON-body en leest de response als een stream van newline-gescheiden JSON-berichten. Elk bericht heeft een `type` veld dat de UI gebruikt om het juiste paneel te updaten:

| Berichttype | Betekenis |
|-------------|---------|
| `message` | Statusupdate (bijv. "Starting researcher agent task...") |
| `researcher` | Onderzoeksresultaten zijn klaar |
| `marketing` | Product zoekresultaten zijn klaar |
| `writer` | Schrijver gestart of voltooid (bevat `{ start: true }` of `{ complete: true }`) |
| `partial` | Een enkele gestreamde token van de Writer (bevat `{ text: "..." }`) |
| `editor` | Het oordeel van de editor is klaar |
| `error` | Er is iets misgegaan |

![Berichttype routering in de browser](../../../images/part12-message-types.svg)

![Streamingsequentie — Browser naar Backend communicatie](../../../images/part12-streaming-sequence.svg)

---

## Vereisten

- Voltooi [Deel 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI geïnstalleerd en het `phi-3.5-mini` model gedownload
- Een moderne webbrowser (Chrome, Edge, Firefox, of Safari)

---

## De gedeelde UI

Voordat je aan backend code begint, neem even de tijd om de front-end te verkennen die alle drie de taalsporen gebruiken. De bestanden bevinden zich in `zava-creative-writer-local/ui/`:

| Bestand | Doel |
|------|---------|
| `index.html` | Pagina-indeling: invoerformulier, agent status badges, artikel outputgebied, inklapbare detailpanelen |
| `style.css` | Minimale styling met status-badge kleurtoestanden (wachten, bezig, klaar, fout) |
| `app.js` | Fetch-aanroep, `ReadableStream` lijnlezer en DOM-update logica |

> **Tip:** Open `index.html` direct in je browser om de lay-out te bekijken. Niets werkt nog omdat er geen backend is, maar je kunt de structuur zien.

### Hoe de Stream Reader werkt

De kernfunctie in `app.js` leest het response body chunk voor chunk en splitst op newline grenzen:

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
    buffer = lines.pop(); // behoud de onvolledige laatste regel

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

Elk geparseerd bericht wordt doorgegeven aan `handleMessage()`, die het relevante DOM-element bijwerkt op basis van `msg.type`.

---

## Oefeningen

### Oefening 1: Draai de Python Backend met de UI

De Python (FastAPI) variant heeft al een streaming API endpoint. De enige wijziging is het mounten van de `ui/` map als statische bestanden.

**1.1** Navigeer naar de Python API map en installeer dependencies:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Start de server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Open je browser op `http://localhost:8000`. Je zou de Zava Creative Writer UI moeten zien met drie tekstvelden en een knop "Generate Article".

**1.4** Klik op **Generate Article** met de standaardwaarden. Kijk hoe de agent status badges veranderen van "Waiting" naar "Running" naar "Done" terwijl elke agent zijn werk voltooit, en zie hoe de artikeltekst token voor token in het outputpaneel gestreamd wordt.

> **Problemen oplossen:** Als de pagina een JSON-response toont in plaats van de UI, zorg dan dat je de bijgewerkte `main.py` draait die de statische bestanden monteert. De `/api/article` endpoint werkt nog steeds via het oorspronkelijke pad; de statische montage serveert de UI op alle andere routes.

**Hoe het werkt:** De bijgewerkte `main.py` voegt één regel onderaan toe:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Dit serveert elk bestand uit `zava-creative-writer-local/ui/` als een statische asset, met `index.html` als standaarddocument. De `/api/article` POST-route is geregistreerd vóór de statische montage, dus die heeft prioriteit.

---

### Oefening 2: Voeg een webserver toe aan de JavaScript variant

De JavaScript variant is momenteel een CLI-applicatie (`main.mjs`). Een nieuw bestand `server.mjs` wikkelt dezelfde agent modules in achter een HTTP-server en serveert de gedeelde UI.

**2.1** Navigeer naar de JavaScript map en installeer dependencies:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Start de webserver:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Je zou het volgende moeten zien:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Open `http://localhost:3000` in je browser en klik op **Generate Article**. Dezelfde UI werkt identiek met de JavaScript backend.

**Bestudeer de code:** Open `server.mjs` en let op de belangrijkste patronen:

- **Serveren van statische bestanden** gebruikt de Node.js ingebouwde `http`, `fs`, en `path` modules zonder extern framework.
- **Bescherming tegen path traversal** normaliseert het gevraagde pad en controleert dat het binnen de `ui/` map blijft.
- **NDJSON streaming** gebruikt een `sendLine()` helper die elk object serialiseert, interne nieuwe regels verwijdert, en een afsluitende newline toevoegt.
- **Agent orkestratie** hergebruikt de bestaande `researcher.mjs`, `product.mjs`, `writer.mjs`, en `editor.mjs` modules zonder aanpassingen.

<details>
<summary>Belangrijk fragment uit server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Onderzoeker
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Schrijver (streamen)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Oefening 3: Voeg een Minimal API toe aan de C# variant

De C# variant is momenteel een console-applicatie. Een nieuw project, `csharp-web`, gebruikt ASP.NET Core minimal APIs om dezelfde pijplijn als webservice aan te bieden.

**3.1** Navigeer naar het C# web project en herstel pakketten:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Start de webserver:

```bash
dotnet run
```

```powershell
dotnet run
```

Je zou dit moeten zien:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Open `http://localhost:5000` in je browser en klik op **Generate Article**.

**Bestudeer de code:** Open `Program.cs` in de `csharp-web` map en let op:

- Het projectbestand gebruikt `Microsoft.NET.Sdk.Web` in plaats van `Microsoft.NET.Sdk`, wat ASP.NET Core ondersteuning toevoegt.
- Statische bestanden worden geserveerd via `UseDefaultFiles` en `UseStaticFiles` gericht op de gedeelde `ui/` map.
- De `/api/article` endpoint schrijft NDJSON lijnen direct naar `HttpContext.Response` en flusht na elke regel voor real-time streaming.
- Alle agent logica (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) is gelijk aan de consoleversie.

<details>
<summary>Belangrijk fragment uit csharp-web/Program.cs</summary>

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

### Oefening 4: Verken de Agent Status Badges

Nu je een werkende UI hebt, kijk hoe de front-end de status badges bijwerkt.

**4.1** Open `zava-creative-writer-local/ui/app.js` in je editor.

**4.2** Zoek de functie `handleMessage()`. Let op hoe deze berichttypes aan DOM-updates koppelt:

| Berichttype | UI-actie |
|-------------|-----------|
| `message` met "researcher" | Zet de Researcher badge op "Running" |
| `researcher` | Zet de Researcher badge op "Done" en vult het Research Results paneel |
| `marketing` | Zet de Product Search badge op "Done" en vult het Product Matches paneel |
| `writer` met `data.start` | Zet de Writer badge op "Running" en maakt de artikel output leeg |
| `partial` | Voegt de token tekst toe aan de artikel output |
| `writer` met `data.complete` | Zet de Writer badge op "Done" |
| `editor` | Zet de Editor badge op "Done" en vult het Editor Feedback paneel |

**4.3** Open de inklapbare panelen "Research Results", "Product Matches", en "Editor Feedback" onder het artikel om de ruwe JSON te inspecteren die elke agent produceerde.

---

### Oefening 5: Pas de UI aan (Uitdaging)

Probeer één of meer van deze verbeteringen:

**5.1 Voeg een woordentelling toe.** Na het afsluiten van de Writer, toon het aantal woorden onder het output paneel. Je kunt dit berekenen in `handleMessage` wanneer `type === "writer"` en `data.complete` waar is:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Voeg een retry indicator toe.** Wanneer de Editor een revisie vraagt, wordt de pijplijn opnieuw uitgevoerd. Toon een banner "Revision 1" of "Revision 2" in het statuspaneel. Luister naar een `message` type met "Revision" en update een nieuw DOM-element.

**5.3 Donkere modus.** Voeg een toggle knop toe en een `.dark` klasse aan de `<body>`. Overschrijf achtergrond-, tekst- en paneelkleuren in `style.css` met een `body.dark` selector.

---

## Samenvatting

| Wat je deed | Hoe |
|-------------|-----|
| De UI serveren vanuit de Python backend | Monteer de `ui/` map met `StaticFiles` in FastAPI |
| Een HTTP server toegevoegd aan de JavaScript variant | Gemaakt `server.mjs` met ingebouwde Node.js `http` module |
| Een web API toegevoegd aan de C# variant | Nieuw `csharp-web` project gemaakt met ASP.NET Core minimal APIs |
| NDJSON streaming in de browser geconsumeerd | Gebruik gemaakt van `fetch()` met `ReadableStream` en regel-voor-regel JSON parsing |
| De UI in realtime geüpdatet | Berichttypes gemapped naar DOM-updates (badges, tekst, inklapbare panelen) |

---

## Belangrijkste inzichten

1. Een **gedeelde statische front-end** kan werken met elke backend die hetzelfde streamingprotocol spreekt, wat het belang benadrukt van het OpenAI-compatibele API-patroon.
2. **Newline-delimited JSON (NDJSON)** is een eenvoudig streamingformaat dat native werkt met de browser `ReadableStream` API.
3. De **Python variant** had de minste wijziging nodig omdat deze al een FastAPI endpoint had; de JavaScript en C# varianten hadden een dunne HTTP-wrapper nodig.
4. De UI als **vanilla HTML/CSS/JS** houden vermijdt buildtools, framework-afhankelijkheden en extra complexiteit voor workshopdeelnemers.
5. Dezelfde agent modules (Researcher, Product, Writer, Editor) worden hergebruikt zonder aanpassing; alleen de transportlaag verandert.

---

## Verdieping

| Bron | Link |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON-specificatie | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Ga verder naar [Deel 13: Workshop voltooid](part13-workshop-complete.md) voor een samenvatting van alles wat je in deze workshop hebt opgebouwd.

---
[← Deel 11: Hulpmiddel Aanroepen](part11-tool-calling.md) | [Deel 13: Workshop Voltooid →](part13-workshop-complete.md)