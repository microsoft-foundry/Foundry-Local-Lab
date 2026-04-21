![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 12: Lage et nettgrensesnitt for Zava Creative Writer

> **Mål:** Legg til en nettleserbasert frontend til Zava Creative Writer slik at du kan se multi-agent-pipelinen kjøre i sanntid, med levende agentstatusmerker og streamet artikkeltekst, alt servert fra en enkelt lokal nettserver.

I [Del 7](part7-zava-creative-writer.md) utforsket du Zava Creative Writer som en **CLI-applikasjon** (JavaScript, C#) og en **headless API** (Python). I denne labben skal du koble en delt **vanilla HTML/CSS/JavaScript** frontend til hver backend slik at brukere kan samhandle med pipelinen via en nettleser i stedet for en terminal.

---

## Hva du vil lære

| Mål | Beskrivelse |
|-----------|-------------|
| Servere statiske filer fra en backend | Monter en HTML/CSS/JS-mappe ved siden av API-ruten din |
| Konsumere streaming NDJSON i nettleseren | Bruke Fetch API med `ReadableStream` for å lese newline-delimitert JSON |
| Enhetlig streamingprotokoll | Sørg for at Python, JavaScript og C#-backends sender samme meldingsformat |
| Progressiv oppdatering av UI | Oppdatere agentstatusmerker og strømme artikkeltekst token for token |
| Legg til et HTTP-lag til en CLI-app | Pakk inn eksisterende orkestreringslogikk i en Express-stil server (JS) eller ASP.NET Core minimal API (C#) |

---

## Arkitektur

UI er et sett med statiske filer (`index.html`, `style.css`, `app.js`) delt av alle tre backends. Hver backend eksponerer de samme to rutene:

![Zava UI arkitektur — delt frontend med tre backends](../../../images/part12-architecture.svg)

| Rute | Metode | Formål |
|-------|--------|---------|
| `/` | GET | Serverer den statiske UI |
| `/api/article` | POST | Kjører multi-agent-pipelinen og strømmer NDJSON |

Frontenden sender en JSON-kropp og leser responsen som en strøm av newline-delimitert JSON-meldinger. Hver melding har et `type`-felt som UI bruker for å oppdatere korrekt panel:

| Meldings-type | Betydning |
|-------------|---------|
| `message` | Statusoppdatering (f.eks. "Starter oppgave for researcher agent...") |
| `researcher` | Forskningsresultater er klare |
| `marketing` | Produktsøkresultater er klare |
| `writer` | Writer startet eller ferdig (inneholder `{ start: true }` eller `{ complete: true }`) |
| `partial` | En enkelt streamet token fra Writer (inneholder `{ text: "..." }`) |
| `editor` | Editorens dom er klar |
| `error` | Noe gikk galt |

![Meldings-type ruting i nettleseren](../../../images/part12-message-types.svg)

![Streamingsekvens — kommunikasjon fra nettleser til backend](../../../images/part12-streaming-sequence.svg)

---

## Forutsetninger

- Fullfør [Del 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI installert og `phi-3.5-mini` modellen lastet ned
- En moderne nettleser (Chrome, Edge, Firefox eller Safari)

---

## Den delte UI

Før du rører noe backend-kode, ta et øyeblikk til å utforske frontenden som alle tre språksporene vil bruke. Filene ligger i `zava-creative-writer-local/ui/`:

| Fil | Formål |
|------|---------|
| `index.html` | Sidens oppsett: inngangsskjema, agentstatusmerker, område for artikkelutdata, kollapsbare detaljpaneler |
| `style.css` | Minimal styling med statusmerke-fargevalg (venter, kjører, ferdig, feil) |
| `app.js` | Fetch-kall, `ReadableStream` linjeleser, og DOM-oppdateringslogikk |

> **Tips:** Åpne `index.html` direkte i nettleseren for å forhåndsvise oppsettet. Ingenting vil fungere ennå fordi det ikke er noen backend, men du kan se strukturen.

### Hvordan Stream Reader fungerer

Den sentrale funksjonen i `app.js` leser responskroppen bit for bit og splitter på newline-grenser:

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
    buffer = lines.pop(); // behold den ufullstendige avsluttende linjen

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

Hver analysert melding blir sendt videre til `handleMessage()`, som oppdaterer det relevante DOM-elementet basert på `msg.type`.

---

## Øvelser

### Øvelse 1: Kjør Python-backenden med UI

Python-varianten (FastAPI) har allerede et streaming-API-endepunkt. Den eneste endringen er å montere `ui/`-mappen som statiske filer.

**1.1** Naviger til Python API-mappen og installer avhengigheter:

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

**1.3** Åpne nettleseren på `http://localhost:8000`. Du skal se Zava Creative Writer UI med tre tekstfelt og en knapp "Generate Article".

**1.4** Klikk på **Generate Article** med standardverdiene. Følg hvordan agentstatusmerkene endres fra "Waiting" til "Running" til "Done" etter hvert som hver agent fullfører oppgaven, og se artikkelteksten strømme inn i utdata-panelet token for token.

> **Feilsøking:** Hvis siden viser en JSON-respons i stedet for UI, sørg for at du kjører den oppdaterte `main.py` som monterer de statiske filene. `/api/article` endepunktet fungerer fortsatt på sin originale sti; den statiske filmonteringen server UI på alle andre ruter.

**Slik fungerer det:** Den oppdaterte `main.py` legger til en enkelt linje nederst:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Dette serverer alle filer fra `zava-creative-writer-local/ui/` som statiske ressurser, med `index.html` som standarddokument. `/api/article` POST-ruten er registrert før den statiske monteringen, så den har prioritet.

---

### Øvelse 2: Legg til en webserver til JavaScript-varianten

JavaScript-varianten er for øyeblikket en CLI-applikasjon (`main.mjs`). En ny fil, `server.mjs`, pakker de samme agentmodulene bak en HTTP-server og serverer den delte UI-en.

**2.1** Naviger til JavaScript-mappen og installer avhengigheter:

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

Du skal se:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Åpne `http://localhost:3000` i nettleseren og klikk på **Generate Article**. Den samme UI fungerer likt mot JavaScript-backenden.

**Studer koden:** Åpne `server.mjs` og legg merke til nøkkelmønstrene:

- **Servering av statiske filer** bruker Node.js innebygde `http`, `fs`, og `path` moduler uten eksternt rammeverk.
- **Stiangrepsbeskyttelse** normaliserer den forespurte stien og verifiserer at den holder seg innenfor `ui/`-mappen.
- **NDJSON streaming** bruker en hjelper `sendLine()` som serialiserer hvert objekt, fjerner interne newline-tegn, og legger til et avsluttende newline.
- **Agentorkestrering** gjenbruker de eksisterende modulene `researcher.mjs`, `product.mjs`, `writer.mjs`, og `editor.mjs` uten endringer.

<details>
<summary>Nøkkelutdrag fra server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Forsker
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Skribent (strømming)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Øvelse 3: Legg til en minimal API til C#-varianten

C#-varianten er for øyeblikket en konsollapplikasjon. Et nytt prosjekt, `csharp-web`, bruker ASP.NET Core minimal API for å eksponere samme pipeline som en webtjeneste.

**3.1** Naviger til C# web-prosjektet og gjenopprett pakker:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Kjør webserveren:

```bash
dotnet run
```

```powershell
dotnet run
```

Du skal se:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Åpne `http://localhost:5000` i nettleseren og klikk på **Generate Article**.

**Studer koden:** Åpne `Program.cs` i `csharp-web` katalogen og legg merke til:

- Prosjektfilen bruker `Microsoft.NET.Sdk.Web` i stedet for `Microsoft.NET.Sdk`, som legger til ASP.NET Core støtte.
- Statisk filer serveres via `UseDefaultFiles` og `UseStaticFiles` som peker til den delte `ui/`-mappen.
- `/api/article` endepunktet skriver NDJSON-linjer direkte til `HttpContext.Response` og tømmer bufferet etter hver linje for sanntidsstreaming.
- All agentlogikk (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) er den samme som konsollversjonen.

<details>
<summary>Nøkkelutdrag fra csharp-web/Program.cs</summary>

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

### Øvelse 4: Utforsk agentstatusmerker

Nå som du har en fungerende UI, se på hvordan frontenden oppdaterer statusmerkene.

**4.1** Åpne `zava-creative-writer-local/ui/app.js` i redigeringsprogrammet ditt.

**4.2** Finn funksjonen `handleMessage()`. Legg merke til hvordan den kartlegger meldinger til DOM-oppdateringer:

| Meldings-type | UI-aksjon |
|-------------|-----------|
| `message` med "researcher" | Setter Researcher-merket til "Running" |
| `researcher` | Setter Researcher-merket til "Done" og fyller panelet Forskningsresultater |
| `marketing` | Setter Product Search-merket til "Done" og fyller panelet Produktmatcher |
| `writer` med `data.start` | Setter Writer-merket til "Running" og tømmer artikkelutdata |
| `partial` | Legger til token-teksten i artikkelutdata |
| `writer` med `data.complete` | Setter Writer-merket til "Done" |
| `editor` | Setter Editor-merket til "Done" og fyller panelet Editorfeedback |

**4.3** Åpne de kollapsbare panelene "Research Results", "Product Matches", og "Editor Feedback" under artikkelen for å inspisere den rå JSON hver agent produserte.

---

### Øvelse 5: Tilpass UI-en (ekstra)

Prøv ett eller flere av disse forbedringene:

**5.1 Legg til ordtelling.** Etter at Writer er ferdig, vis artikkelens ordtelling under utdata-panelet. Du kan beregne dette i `handleMessage` når `type === "writer"` og `data.complete` er sant:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Legg til en retry-indikator.** Når Editoren ber om revisjon, kjører pipelinen på nytt. Vis et banner "Revision 1" eller "Revision 2" i status-panelet. Lykk etter en meldingstype `message` som inneholder "Revision" og oppdater et nytt DOM-element.

**5.3 Mørk modus.** Legg til en bytteknapp og en `.dark` klasse på `<body>`. Overstyr bakgrunn, tekst og panelfarger i `style.css` med en `body.dark`-velger.

---

## Oppsummering

| Hva du gjorde | Hvordan |
|-------------|-----|
| Serverte UI-en fra Python-backenden | Monterte `ui/`-mappen med `StaticFiles` i FastAPI |
| La til en HTTP-server til JavaScript-varianten | Laget `server.mjs` ved å bruke innebygd Node.js `http`-modul |
| Laget en web-API til C#-varianten | Laget et nytt `csharp-web` prosjekt med ASP.NET Core minimal API |
| Konsumerte streaming NDJSON i nettleseren | Brukte `fetch()` med `ReadableStream` og linje-for-linje JSON-parsing |
| Oppdaterte UI i sanntid | Kartla meldings-typer til DOM-oppdateringer (merker, tekst, kollapsbare paneler) |

---

## Viktige læringspunkter

1. En **delt statisk frontend** kan fungere med hvilken som helst backend som snakker samme streamingprotokoll, noe som forsterker verdien av OpenAI-kompatibelt API-mønster.
2. **Newline-delimitert JSON (NDJSON)** er et enkelt streamingformat som fungerer nativt med nettleserens `ReadableStream` API.
3. **Python-varianten** trengte minst endring fordi den allerede hadde et FastAPI-endepunkt; JavaScript og C#-variantene trengte et tynt HTTP-innpakningslag.
4. Å holde UI som **vanilla HTML/CSS/JS** unngår byggeverktøy, rammeverksavhengigheter og ekstra kompleksitet for workshopdeltakere.
5. De samme agentmodulene (Researcher, Product, Writer, Editor) gjenbrukes uten modifikasjon; bare transportlaget endres.

---

## Videre lesning

| Ressurs | Lenke |
|----------|------|
| MDN: Bruke Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI statiske filer | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core statiske filer | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON-spesifikasjon | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Fortsett til [Del 13: Workshop Fullført](part13-workshop-complete.md) for en oppsummering av alt du har bygd i løpet av denne workshoppen.

---
[← Del 11: Verktøyanrop](part11-tool-calling.md) | [Del 13: Verksted fullført →](part13-workshop-complete.md)