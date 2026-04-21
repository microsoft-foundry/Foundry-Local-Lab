![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 12: Bygga ett webbgränssnitt för Zava Creative Writer

> **Mål:** Lägg till ett webbläsarbaserat gränssnitt till Zava Creative Writer så att du kan följa multi-agent-pipelinen i realtid, med live-statusmärken för agenterna och strömmande artikeltxt, allt serverat från en enda lokal webbserver.

I [Del 7](part7-zava-creative-writer.md) utforskade du Zava Creative Writer som en **CLI-applikation** (JavaScript, C#) och en **headless API** (Python). I detta labb kopplar du ett gemensamt **vanilla HTML/CSS/JavaScript**-gränssnitt till varje backend så att användare kan interagera med pipelinen via en webbläsare istället för en terminal.

---

## Vad du kommer att lära dig

| Mål | Beskrivning |
|-----------|-------------|
| Servera statiska filer från en backend | Montera en HTML/CSS/JS-katalog vid sidan av din API-rutt |
| Konsumera strömmande NDJSON i webbläsaren | Använd Fetch API med `ReadableStream` för att läsa nylinjeavgränsad JSON |
| Enhetligt strömprotokoll | Säkerställ att Python-, JavaScript- och C#-backend levererar samma meddelandeformat |
| Progressiv UI-uppdatering | Uppdatera agentstatusmärken och strömma artikelttext token för token |
| Lägg till ett HTTP-lager till en CLI-app | Wrappa befintlig orkestreringslogik i en Express-liknande server (JS) eller ASP.NET Core minimal API (C#) |

---

## Arkitektur

UI:t är en uppsättning statiska filer (`index.html`, `style.css`, `app.js`) som delas av alla tre backend. Varje backend exponerar samma två rutter:

![Zava UI-arkitektur — gemensamt frontend med tre backend](../../../images/part12-architecture.svg)

| Rutt | Metod | Syfte |
|-------|--------|---------|
| `/` | GET | Serverar det statiska UI:t |
| `/api/article` | POST | Kör multi-agent-pipelinen och strömmar NDJSON |

Frontend skickar en JSON-body och läser svaret som en ström av nylinjeavgränsade JSON-meddelanden. Varje meddelande har ett `type`-fält som UI använder för att uppdatera rätt panel:

| Meddelandetyp | Betydelse |
|-------------|---------|
| `message` | Statusuppdatering (t.ex. "Startar forskaragentuppgift...") |
| `researcher` | Forskningsresultat är klara |
| `marketing` | Produktsökningsresultat är klara |
| `writer` | Skribenten startad eller klar (innehåller `{ start: true }` eller `{ complete: true }`) |
| `partial` | Enstaka token från Writer (innehåller `{ text: "..." }`) |
| `editor` | Redaktörens beslut är klart |
| `error` | Något gick fel |

![Meddelandetyp-routning i webbläsaren](../../../images/part12-message-types.svg)

![Strömningssekvens — Kommunikation mellan webbläsaren och backend](../../../images/part12-streaming-sequence.svg)

---

## Förutsättningar

- Slutför [Del 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI installerat och modellen `phi-3.5-mini` nedladdad
- En modern webbläsare (Chrome, Edge, Firefox eller Safari)

---

## Det Gemensamma UI:t

Innan du rör backend-koden, ta en stund att utforska frontend som alla tre språkspår kommer att använda. Filerna finns i `zava-creative-writer-local/ui/`:

| Fil | Syfte |
|------|---------|
| `index.html` | Sidlayout: inmatningsformulär, agentstatusmärken, artikelutgång, kollapsbara detaljpaneler |
| `style.css` | Minimal styling med statusmärkets färgstate (väntar, kör, klar, fel) |
| `app.js` | Fetch-anrop, `ReadableStream` radläsare och DOM-uppdateringslogik |

> **Tips:** Öppna `index.html` direkt i din webbläsare för att förhandsgranska layouten. Ingenting fungerar än eftersom det inte finns någon backend, men du kan se strukturen.

### Hur strömläsaren fungerar

Nyckelfunktionen i `app.js` läser svarskroppen bit för bit och delar vid nylinjer:

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
    buffer = lines.pop(); // behåll den ofullständiga avslutande raden

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

Varje parsat meddelande skickas vidare till `handleMessage()`, som uppdaterar relevant DOM-element baserat på `msg.type`.

---

## Övningar

### Övning 1: Kör Python-backend med UI

Python-varianten (FastAPI) har redan en strömmande API-endpoint. Den enda ändringen är att montera `ui/`-mappen som statiska filer.

**1.1** Navigera till Python API-katalogen och installera beroenden:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Starta servern:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Öppna din webbläsare på `http://localhost:8000`. Du bör se Zava Creative Writer UI med tre textfält och en knapp "Generate Article".

**1.4** Klicka på **Generate Article** med standardvärdena. Följ agentstatusmärkena som går från "Waiting" till "Running" till "Done" när varje agent avslutar, och se artikelttexten strömma in i utmatningspanelen token för token.

> **Felsökning:** Om sidan visar ett JSON-svar istället för UI, säkerställ att du kör den uppdaterade `main.py` som monterar de statiska filerna. `/api/article` endpoint fungerar fortfarande på sin ursprungliga väg; den statiska filmonteringen serverar UI:t på alla andra rutter.

**Hur det fungerar:** Den uppdaterade `main.py` lägger till en rad längst ner:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Detta serverar varje fil från `zava-creative-writer-local/ui/` som en statisk resurs, med `index.html` som standarddokument. `/api/article` POST-rutten registreras före den statiska monteringen och har därför hög prioritet.

---

### Övning 2: Lägg till en webbserver till JavaScript-varianten

JavaScript-varianten är för närvarande en CLI-applikation (`main.mjs`). En ny fil, `server.mjs`, paketerar samma agentmoduler bakom en HTTP-server och serverar det gemensamma UI:t.

**2.1** Navigera till JavaScript-katalogen och installera beroenden:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Starta webbservern:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Du bör se:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Öppna `http://localhost:3000` i din webbläsare och klicka på **Generate Article**. Samma UI fungerar identiskt mot JavaScript-backenden.

**Studera koden:** Öppna `server.mjs` och notera nyckelmönster:

- **Statisk filservering** använder Node.js inbyggda `http`, `fs` och `path` moduler utan externa ramverk.
- **Skydd mot path-traversering** normaliserar den begärda sökvägen och kontrollerar att den stannar inom `ui/`-katalogen.
- **NDJSON-strömning** använder en hjälpfunktion `sendLine()` som serialiserar varje objekt, tar bort interna nylinjer och lägger till en avslutande ny rad.
- **Agentorkestrering** återanvänder de befintliga modulerna `researcher.mjs`, `product.mjs`, `writer.mjs` och `editor.mjs` utan ändringar.

<details>
<summary>Nyckelutdrag från server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Forskare
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Författare (strömmande)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Övning 3: Lägg till ett minimalistiskt API till C#-varianten

C#-varianten är för närvarande en konsolapplikation. Ett nytt projekt, `csharp-web`, använder ASP.NET Core minimal API:er för att exponera samma pipeline som en webbservice.

**3.1** Navigera till C# webprojektet och återställ paket:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Kör webbservern:

```bash
dotnet run
```

```powershell
dotnet run
```

Du bör se:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Öppna `http://localhost:5000` i din webbläsare och klicka på **Generate Article**.

**Studera koden:** Öppna `Program.cs` i `csharp-web`-katalogen och notera:

- Projektfilen använder `Microsoft.NET.Sdk.Web` istället för `Microsoft.NET.Sdk`, vilket lägger till stöd för ASP.NET Core.
- Statiska filer serveras via `UseDefaultFiles` och `UseStaticFiles` pekade på den gemensamma `ui/`-katalogen.
- `/api/article` endpoint skriver NDJSON-rader direkt till `HttpContext.Response` och tömmer efter varje rad för realtidsströmning.
- All agentlogik (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) är densamma som konsolversionen.

<details>
<summary>Nyckelutdrag från csharp-web/Program.cs</summary>

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

### Övning 4: Utforska agentstatusmärkena

Nu när du har ett fungerande UI, titta på hur frontend uppdaterar statusmärkena.

**4.1** Öppna `zava-creative-writer-local/ui/app.js` i din editor.

**4.2** Hitta funktionen `handleMessage()`. Notera hur den mappar meddelandetyper till DOM-uppdateringar:

| Meddelandetyp | UI-åtgärd |
|-------------|-----------|
| `message` som innehåller "researcher" | Sätter forskarmärket till "Running" |
| `researcher` | Sätter forskarmärket till "Done" och fyller forskningsresultatpanelen |
| `marketing` | Sätter produktsökningsmärket till "Done" och fyller produkttrefferpanelen |
| `writer` med `data.start` | Sätter skribentmärket till "Running" och rensar artikelutgången |
| `partial` | Lägger till token-text till artikelutgången |
| `writer` med `data.complete` | Sätter skribentmärket till "Done" |
| `editor` | Sätter redaktörsmärket till "Done" och fyller redaktörskommentarspanelen |

**4.3** Öppna de kollapsbara panelerna "Research Results", "Product Matches" och "Editor Feedback" under artikeln för att inspektera den råa JSON var och en av agenterna producerade.

---

### Övning 5: Anpassa UI:t (Stretch)

Prova en eller flera av dessa förbättringar:

**5.1 Lägg till en ord-räknare.** När skribenten är klar, visa artikelns ordantal under utmatningspanelen. Du kan beräkna detta i `handleMessage` när `type === "writer"` och `data.complete` är sant:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Lägg till en indikator för omskrivning.** När redaktören begär en revidering körs pipelinen om. Visa en "Revision 1" eller "Revision 2" banner i statuspanelen. Lyssna efter ett `message`-typ med "Revision" och uppdatera ett nytt DOM-element.

**5.3 Mörkt läge.** Lägg till en växlingsknapp och en `.dark`-klass till `<body>`. Åsidosätt bakgrunds-, text- och panelfärger i `style.css` med en `body.dark`-selektor.

---

## Sammanfattning

| Vad du gjorde | Hur |
|-------------|-----|
| Serverade UI:t från Python-backenden | Monterade `ui/`-mappen med `StaticFiles` i FastAPI |
| Lade till HTTP-server till JavaScript-varianten | Skapade `server.mjs` med inbyggda Node.js `http`-modulen |
| Lade till web-API till C#-varianten | Skapade nytt `csharp-web` projekt med ASP.NET Core minimal APIs |
| Konsumerade strömmande NDJSON i webbläsaren | Använde `fetch()` med `ReadableStream` och rad-för-rad JSON-parsing |
| Uppdaterade UI:t i realtid | Mappade meddelandetyper till DOM-uppdateringar (märken, text, kollapsbara paneler) |

---

## Viktiga lärdomar

1. Ett **gemensamt statiskt frontend** kan fungera med vilken backend som helst som talar samma strömprotokoll, vilket stärker värdet av OpenAI-kompatibla API-mönster.
2. **Nylinjeavgränsad JSON (NDJSON)** är ett enkelt strömformat som fungerar nativt med webbläsarens `ReadableStream` API.
3. **Python-varianten** behövde minst ändring eftersom den redan hade en FastAPI-endpoint; JavaScript- och C#-varianterna behövde ett tunt HTTP-wrapper.
4. Att behålla UI:t som **vanilla HTML/CSS/JS** undviker byggverktyg, ramverksberoenden och ytterligare komplexitet för workshopdeltagarna.
5. Samma agentmoduler (Researcher, Product, Writer, Editor) återanvänds utan modifiering; endast transportlagret ändras.

---

## Vidare läsning

| Resurs | Länk |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Fortsätt till [Del 13: Workshop klar](part13-workshop-complete.md) för en sammanfattning av allt du byggt under denna workshop.

---
[← Del 11: Verktygsanrop](part11-tool-calling.md) | [Del 13: Verkstaden klar →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, bör du vara medveten om att automatiska översättningar kan innehålla fel eller brister. Det ursprungliga dokumentet på dess modersmål bör anses vara den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår vid användning av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->