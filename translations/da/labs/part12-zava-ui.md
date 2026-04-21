![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 12: Bygning af en Web UI til Zava Creative Writer

> **Mål:** Tilføj et browserbaseret front end til Zava Creative Writer, så du kan se multi-agent pipeline køre i realtid, med live agent status badges og streamet artikeltekst, alt serveret fra én lokal webserver.

I [Del 7](part7-zava-creative-writer.md) undersøgte du Zava Creative Writer som en **CLI-applikation** (JavaScript, C#) og en **headless API** (Python). I dette laboratorium vil du forbinde et delt **vanilla HTML/CSS/JavaScript** front end til hver backend, så brugere kan interagere med pipelinen via en browser fremfor en terminal.

---

## Hvad Du Vil Lære

| Mål | Beskrivelse |
|-----------|-------------|
| Server statiske filer fra en backend | Monter et HTML/CSS/JS bibliotek ved siden af din API-rute |
| Forbrug streaming NDJSON i browseren | Brug Fetch API med `ReadableStream` til at læse newline-delimitteret JSON |
| Unified streaming protokol | Sørg for at Python, JavaScript og C# backends udsender samme beskedformat |
| Progressive UI opdateringer | Opdater agent status badges og stream artikeltekst token for token |
| Tilføj et HTTP lag til en CLI app | Pak eksisterende orkestreringslogik ind i en Express-agtig server (JS) eller ASP.NET Core minimal API (C#) |

---

## Arkitektur

UI’et består af et enkelt sæt statiske filer (`index.html`, `style.css`, `app.js`) delt af alle tre backends. Hver backend udstiller de samme to ruter:

![Zava UI arkitektur — delt front end med tre backends](../../../images/part12-architecture.svg)

| Rute | Metode | Formål |
|-------|--------|---------|
| `/` | GET | Server den statiske UI |
| `/api/article` | POST | Kører multi-agent pipelinen og streamer NDJSON |

Frontenden sender en JSON-krop og læser svaret som en stream af newline-delimitterede JSON-beskeder. Hver besked har et `type` felt, som UI’et bruger til at opdatere det relevante panel:

| Beskedtype | Betydning |
|-------------|---------|
| `message` | Statusopdatering (f.eks. "Starter researcher agent opgave...") |
| `researcher` | Forskningsresultater er klar |
| `marketing` | Produkt-søgeresultater er klar |
| `writer` | Writer startede eller færdig (indeholder `{ start: true }` eller `{ complete: true }`) |
| `partial` | Et enkelt streamet token fra Writer (indeholder `{ text: "..." }`) |
| `editor` | Editor afgørelse er klar |
| `error` | Noget gik galt |

![Beskedtype routing i browseren](../../../images/part12-message-types.svg)

![Streaming sekvens — Browser til Backend kommunikation](../../../images/part12-streaming-sequence.svg)

---

## Forudsætninger

- Fuldfør [Del 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI installeret og `phi-3.5-mini` modellen downloadet
- En moderne webbrowser (Chrome, Edge, Firefox, eller Safari)

---

## Det Delte UI

Før du rører ved noget backend-kode, så tag et øjeblik til at udforske frontenden, som alle tre sprogspor vil bruge. Filene ligger i `zava-creative-writer-local/ui/`:

| Fil | Formål |
|------|---------|
| `index.html` | Side-layout: input formular, agent status badges, artikel output område, sammenklappelige detalje-paneler |
| `style.css` | Minimal styling med status-badge farvetilstande (ventende, kørende, færdig, fejl) |
| `app.js` | Fetch kald, `ReadableStream` linjelæser, og DOM opdateringslogik |

> **Tip:** Åbn `index.html` direkte i din browser for at forhåndsvise layoutet. Intet virker endnu, fordi der ikke er nogen backend, men du kan se strukturen.

### Hvordan Stream Readeren Virker

Nøglefunktionen i `app.js` læser svarets krop stykke for stykke og splitter på newline-grænser:

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
    buffer = lines.pop(); // behold den ufuldstændige afsluttende linje

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

Hver fortolket besked bliver sendt til `handleMessage()`, som opdaterer det relevante DOM-element baseret på `msg.type`.

---

## Øvelser

### Øvelse 1: Kør Python Backend med UI

Python-varianten (FastAPI) har allerede en streaming API-endpoint. Den eneste ændring er at montere `ui/` mappen som statiske filer.

**1.1** Naviger til Python API-mappen og installer afhængigheder:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Start serveren:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Åbn din browser på `http://localhost:8000`. Du bør se Zava Creative Writer UI med tre tekstfelter og en "Generate Article" knap.

**1.4** Klik på **Generate Article** med standardværdierne. Se agent status badges ændre sig fra "Waiting" til "Running" til "Done", efterhånden som hver agent fuldfører sit arbejde, og se artikelteksten strømme ind i output-panelet token for token.

> **Fejlfinding:** Hvis siden viser et JSON-svar i stedet for UI’et, så sørg for at du kører den opdaterede `main.py`, som monterer de statiske filer. `/api/article` endpointet fungerer stadig på sin oprindelige sti; den statiske fil-monter serverer UI’et på alle andre ruter.

**Hvordan det virker:** Den opdaterede `main.py` tilføjer en enkelt linje i bunden:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Dette serverer alle filer fra `zava-creative-writer-local/ui/` som statiske assets, med `index.html` som standarddokument. `/api/article` POST-ruten registreres før den statiske mount, så den har prioritet.

---

### Øvelse 2: Tilføj en Webserver til JavaScript Variationen

JavaScript-varianten er p.t. en CLI-applikation (`main.mjs`). En ny fil, `server.mjs`, pakker de samme agentmoduler ind bag en HTTP-server og serverer det delte UI.

**2.1** Naviger til JavaScript-mappen og installer afhængigheder:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Start webserveren:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Du bør se:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Åbn `http://localhost:3000` i din browser og klik på **Generate Article**. Samme UI virker identisk mod JavaScript-backenden.

**Studér koden:** Åbn `server.mjs` og bemærk nøglemønstrene:

- **Statisk filservering** bruger Node.js indbyggede `http`, `fs`, og `path` moduler uden eksternt framework.
- **Path-traversal beskyttelse** normaliserer den anmodede sti og verificerer at den forbliver inden for `ui/` mappen.
- **NDJSON streaming** bruger en `sendLine()` helper, der serialiserer hvert objekt, fjerner interne newlines, og tilføjer en afsluttende newline.
- **Agent orkestrering** genbruger de eksisterende `researcher.mjs`, `product.mjs`, `writer.mjs`, og `editor.mjs` moduler uændret.

<details>
<summary>Nøgleudsnit fra server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Forsker
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Forfatter (streaming)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Øvelse 3: Tilføj en Minimal API til C# Variationen

C#-varianten er p.t. en konsolapplikation. Et nyt projekt, `csharp-web`, bruger ASP.NET Core minimal APIs til at udsætte samme pipeline som en webservice.

**3.1** Naviger til C# webprojektet og gendan pakker:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Kør webserveren:

```bash
dotnet run
```

```powershell
dotnet run
```

Du bør se:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Åbn `http://localhost:5000` i din browser og klik på **Generate Article**.

**Studér koden:** Åbn `Program.cs` i `csharp-web` mappen og bemærk:

- Projektfilen bruger `Microsoft.NET.Sdk.Web` i stedet for `Microsoft.NET.Sdk`, hvilket tilføjer ASP.NET Core support.
- Statisk filer serveres via `UseDefaultFiles` og `UseStaticFiles`, pegende på den delte `ui/` mappe.
- `/api/article` endpoint skriver NDJSON linjer direkte til `HttpContext.Response` og flusher efter hver linje for streaming i realtid.
- Al agentlogik (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) er den samme som i konsolversionen.

<details>
<summary>Nøgleudsnit fra csharp-web/Program.cs</summary>

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

### Øvelse 4: Udforsk Agent Status Badges

Nu hvor du har et fungerende UI, så se på hvordan frontenden opdaterer status badges.

**4.1** Åbn `zava-creative-writer-local/ui/app.js` i din editor.

**4.2** Find funktionen `handleMessage()`. Bemærk hvordan den kortlægger beskedtyper til DOM-opdateringer:

| Beskedtype | UI handling |
|-------------|-----------|
| `message` indeholdende "researcher" | Sætter Researcher badge til "Running" |
| `researcher` | Sætter Researcher badge til "Done" og udfylder Research Results panelet |
| `marketing` | Sætter Product Search badge til "Done" og udfylder Product Matches panelet |
| `writer` med `data.start` | Sætter Writer badge til "Running" og rydder artikeloutput |
| `partial` | Tilføjer token tekst til artikeloutput |
| `writer` med `data.complete` | Sætter Writer badge til "Done" |
| `editor` | Sætter Editor badge til "Done" og udfylder Editor Feedback panelet |

**4.3** Åbn de sammenklappelige paneler "Research Results", "Product Matches" og "Editor Feedback" under artiklen for at inspicere den rå JSON, som hver agent udgav.

---

### Øvelse 5: Tilpas UI (Stretch)

Prøv en eller flere af disse forbedringer:

**5.1 Tilføj en ordtælling.** Når Writer er færdig, vis artikelens ordantal under output-panelet. Du kan beregne dette i `handleMessage`, når `type === "writer"` og `data.complete` er sand:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Tilføj en retry indikator.** Når Editor anmoder om en revision, kører pipelinen igen. Vis et banner "Revision 1" eller "Revision 2" i statuspanelet. Lyt efter en `message` type indeholdende "Revision" og opdater et nyt DOM-element.

**5.3 Mørk tilstand.** Tilføj en toggle-knap og en `.dark` klasse til `<body>`. Overstyr baggrund, tekst og panel farver i `style.css` med en `body.dark` selector.

---

## Resumé

| Hvad du gjorde | Hvordan |
|-------------|-----|
| Serverede UI’et fra Python-backenden | Monterede `ui/` mappen med `StaticFiles` i FastAPI |
| Tilføjede en HTTP server til JavaScript-varianten | Oprettede `server.mjs` med Node.js indbyggede `http` modul |
| Tilføjede en web API til C# varianten | Oprettede nyt `csharp-web` projekt med ASP.NET Core minimal APIs |
| Forbrugte streaming NDJSON i browseren | Brugte `fetch()` med `ReadableStream` og linje-for-linje JSON parsing |
| Opdaterede UI i realtid | Kortlagde beskedtyper til DOM-opdateringer (badges, tekst, sammenklappelige paneler) |

---

## Vigtige Pointer

1. Et **delt statisk front end** kan arbejde med enhver backend, der bruger samme streamingprotokol, hvilket understreger værdien af OpenAI-kompatible API-mønstre.
2. **Newline-delimitteret JSON (NDJSON)** er et ligetil streamingformat, der fungerer native med browserens `ReadableStream` API.
3. **Python varianten** krævede mindst ændringer, da den allerede havde en FastAPI endpoint; JavaScript og C# varianterne fik et tyndt HTTP-wrapper lag.
4. At holde UI’et som **vanilla HTML/CSS/JS** undgår build-værktøjer, framework afhængigheder, og yderligere kompleksitet for workshopdeltagere.
5. De samme agentmoduler (Researcher, Product, Writer, Editor) genbruges uden modifikation; kun transportlaget ændres.

---

## Yderligere Læsning

| Ressource | Link |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Fortsæt til [Del 13: Workshop færdig](part13-workshop-complete.md) for et resumé af alt, hvad du har bygget i denne workshop.

---
[← Del 11: Værktøj Kald](part11-tool-calling.md) | [Del 13: Workshop Færdig →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er blevet oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, skal du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets indfødte sprog bør betragtes som den autoritative kilde. For kritiske oplysninger anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for eventuelle misforståelser eller fejltolkninger, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->