![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Dio 12: Izgradnja web sučelja za Zava Creative Writer

> **Cilj:** Dodati pregledničko sučelje Zava Creative Writeru kako biste mogli pratiti višestruki agentski pipeline u stvarnom vremenu, s prikazom statusa agenata uživo i streamingom teksta članka, sve posluženo s jednog lokalnog web poslužitelja.

U [Dijelu 7](part7-zava-creative-writer.md) ste istražili Zava Creative Writer kao **CLI aplikaciju** (JavaScript, C#) i kao **headless API** (Python). U ovoj radionici ćete povezati zajedničko **vanilla HTML/CSS/JavaScript** sučelje s backendima tako da korisnici mogu komunicirati s pipelineom putem preglednika umjesto terminala.

---

## Što ćete naučiti

| Cilj | Opis |
|-----------|-------------|
| Poslužiti statične datoteke s backenda | Montirati HTML/CSS/JS direktorij uz vaš API endpoint |
| Konzumirati streaming NDJSON u pregledniku | Koristiti Fetch API s `ReadableStream` za čitanje JSON-a razgraničenog novim redom |
| Jedinstveni streaming protokol | Osigurati da Python, JavaScript i C# backendovi emitiraju isti format poruka |
| Postepena ažuriranja UI-a | Ažurirati statusne bedževe agenata i streamati tekst članka token po token |
| Dodati HTTP sloj CLI aplikaciji | Umotati postojeću logiku orkestratora u Express-stil poslužitelj (JS) ili ASP.NET Core minimalni API (C#) |

---

## Arhitektura

UI je jedan skup statičkih datoteka (`index.html`, `style.css`, `app.js`) koje dijele sva tri backenda. Svaki backend izlaže iste dvije rute:

![Arhitektura Zava UI — zajedničko sučelje s tri backenda](../../../images/part12-architecture.svg)

| Ruta | Metoda | Svrha |
|-------|--------|---------|
| `/` | GET | Poslužuje statično korisničko sučelje |
| `/api/article` | POST | Pokreće višestruki agentski pipeline i streama NDJSON |

Frontend šalje JSON tijelo i čita odgovor kao streaming poruka razgraničenih novim redovima. Svaka poruka ima `type` polje koje UI koristi za ažuriranje odgovarajućeg panela:

| Tip poruke | Značenje |
|-------------|---------|
| `message` | Ažuriranje statusa (npr. "Pokretanje zadatka istraživačkog agenta...") |
| `researcher` | Rezultati istraživanja su spremni |
| `marketing` | Rezultati pretrage proizvoda su spremni |
| `writer` | Pisac je pokrenut ili je završio (sadrži `{ start: true }` ili `{ complete: true }`) |
| `partial` | Jedan streamani token od Pisca (sadrži `{ text: "..." }`) |
| `editor` | Presuda urednika je spremna |
| `error` | Nešto je pošlo po zlu |

![Usmjeravanje tipova poruka u pregledniku](../../../images/part12-message-types.svg)

![Streaming sekvenca — komunikacija preglednik prema backendu](../../../images/part12-streaming-sequence.svg)

---

## Preduvjeti

- Dovršite [Dio 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI instaliran i model `phi-3.5-mini` preuzet
- Moderan web preglednik (Chrome, Edge, Firefox ili Safari)

---

## Zajedničko sučelje

Prije nego što dirate bilo koji kod backend-a, odvojite trenutak da istražite sučelje koje će se koristiti u sva tri programska jezika. Datoteke se nalaze u `zava-creative-writer-local/ui/`:

| Datoteka | Svrha |
|------|---------|
| `index.html` | Raspored stranice: obrazac za unos, statusni bedževi agenata, područje za ispis članka, sklapanje panela s detaljima |
| `style.css` | Minimalno stiliziranje s bojama statusnih bedževa (čekanje, rad, dovršeno, greška) |
| `app.js` | Poziv Fetch, čitač linija `ReadableStream` i logika ažuriranja DOM-a |

> **Savjet:** Otvorite `index.html` direktno u pregledniku da biste vidjeli raspored. Još ništa neće raditi jer nema backenda, ali možete vidjeti strukturu.

### Kako radi čitač streama

Ključna funkcija u `app.js` čita tijelo odgovora komad po komad i dijeli ga po granicama novih redova:

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
    buffer = lines.pop(); // zadrži nepotpunu završnu liniju

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

Svaka parsirana poruka se prosljeđuje `handleMessage()`, koja ažurira relevantni DOM element prema `msg.type`.

---

## Vježbe

### Vježba 1: Pokrenite Python Backend s UI-jem

Python (FastAPI) varijanta već ima streaming API endpoint. Jedina promjena je montiranje `ui/` mape kao statičkih datoteka.

**1.1** Otvorite direktorij Python API-ja i instalirajte ovisnosti:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Pokrenite poslužitelj:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Otvorite preglednik na `http://localhost:8000`. Trebali biste vidjeti Zava Creative Writer UI s tri tekstualna polja i gumbom "Generate Article".

**1.4** Kliknite **Generate Article** koristeći zadane vrijednosti. Promatrajte kako statusni bedževi agenata mijenjaju stanje sa "Waiting" u "Running" i zatim u "Done" kako svaki agent završi svoj rad, te kako tekst članka stiže u izlazni panel token po token.

> **Rješavanje poteškoća:** Ako stranica prikazuje JSON odgovor umjesto UI-ja, provjerite koristite li ažurirani `main.py` koji montira statične datoteke. Endpoint `/api/article` i dalje radi na svom izvornom putu; montiranje statične datoteke služi UI na svim ostalim rutama.

**Kako to radi:** Ažurirani `main.py` dodaje jedan red na dnu:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Ovo poslužuje svaku datoteku iz `zava-creative-writer-local/ui/` kao statičku imovinu, s `index.html` kao zadanim dokumentom. POST ruta `/api/article` registrirana je prije statičkog mounta pa ima prioritet.

---

### Vježba 2: Dodajte web poslužitelj JavaScript varijanti

JavaScript varijanta je trenutno CLI aplikacija (`main.mjs`). Nova datoteka, `server.mjs`, omotava iste agentske module iza HTTP poslužitelja i poslužuje zajedničko UI.

**2.1** Otvorite JavaScript direktorij i instalirajte ovisnosti:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Pokrenite web poslužitelj:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Trebali biste vidjeti:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Otvorite `http://localhost:3000` u pregledniku i kliknite **Generate Article**. Isti UI radi jednako prema JavaScript backendu.

**Proučite kod:** Otvorite `server.mjs` i primijetite ključne obrasce:

- **Posluživanje statičkih datoteka** koristi Node.js ugrađene module `http`, `fs` i `path` bez vanjskih frameworkova.
- **Zaštita od prolaska kroz putanju** normalizira traženi path i provjerava ostaje li unutar `ui/` direktorija.
- **Streamanje NDJSON** koristi pomoćnu funkciju `sendLine()` koja serijalizira svaki objekt, uklanja unutarnje nove retke i dodaje završni novi red.
- **Orkestracija agenata** ponovno koristi postojeće module `researcher.mjs`, `product.mjs`, `writer.mjs` i `editor.mjs` bez izmjena.

<details>
<summary>Ključni isječak iz server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Istraživač
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Pisac (prijenos uživo)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Vježba 3: Dodajte minimalni API C# varijanti

C# varijanta je trenutno konzolna aplikacija. Novi projekt, `csharp-web`, koristi ASP.NET Core minimalne API-je za izlaganje istog pipelinea kao web uslugu.

**3.1** Otvorite C# web projekt i obnovite pakete:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Pokrenite web poslužitelj:

```bash
dotnet run
```

```powershell
dotnet run
```

Trebali biste vidjeti:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Otvorite `http://localhost:5000` u pregledniku i kliknite **Generate Article**.

**Proučite kod:** Otvorite `Program.cs` unutar `csharp-web` direktorija i primijetite:

- Projekt koristi `Microsoft.NET.Sdk.Web` umjesto `Microsoft.NET.Sdk`, što dodaje podršku za ASP.NET Core.
- Statične datoteke se poslužuju preko `UseDefaultFiles` i `UseStaticFiles` usmjerene na zajednički `ui/` direktorij.
- Endpoint `/api/article` zapisuje NDJSON retke izravno u `HttpContext.Response` i flush-a nakon svake linije za streaming u stvarnom vremenu.
- Sva agentska logika (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) ista je kao u konzolnoj verziji.

<details>
<summary>Ključni isječak iz csharp-web/Program.cs</summary>

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

### Vježba 4: Istražite statusne bedževe agenata

Sada kada imate funkcionalno sučelje, proučite kako frontend ažurira statusne bedževe.

**4.1** Otvorite `zava-creative-writer-local/ui/app.js` u svom uređivaču.

**4.2** Pronađite funkciju `handleMessage()`. Primijetite kako mapira vrste poruka na ažuriranja DOM-a:

| Tip poruke | Akcija u UI-u |
|-------------|-----------|
| `message` koji sadrži "researcher" | Postavlja bedž Researcher na "Running" |
| `researcher` | Postavlja bedž Researcher na "Done" i popunjava panel Research Results |
| `marketing` | Postavlja bedž Product Search na "Done" i popunjava panel Product Matches |
| `writer` s `data.start` | Postavlja bedž Writer na "Running" i briše izlazni članak |
| `partial` | Dodaje tekst tokena u izlaz članka |
| `writer` s `data.complete` | Postavlja bedž Writer na "Done" |
| `editor` | Postavlja bedž Editor na "Done" i popunjava panel Editor Feedback |

**4.3** Otvorite sklopive panele "Research Results", "Product Matches" i "Editor Feedback" ispod članka da pregledate sirovi JSON koji je svaki agent proizveo.

---

### Vježba 5: Prilagodite UI (Dodatno)

Isprobajte jedan ili više ovih dodataka:

**5.1 Dodajte broj riječi.** Nakon što Writer završi, prikažite broj riječi članka ispod izlaznog panela. To možete izračunati u `handleMessage` kad `type === "writer"` i `data.complete` je istinito:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Dodajte indikator ponovnog pokušaja.** Kada Editor zatraži reviziju, pipeline se ponovno pokreće. Prikažite banner "Revision 1" ili "Revision 2" u statusnom panelu. Slušajte `message` tip koji sadrži "Revision" i ažurirajte novi DOM element.

**5.3 Tamni način.** Dodajte gumb za prebacivanje i `.dark` klasu na `<body>`. Prekrijte boju pozadine, teksta i panela u `style.css` selektorom `body.dark`.

---

## Sažetak

| Što ste napravili | Kako |
|-------------|-----|
| Poslužili UI iz Python backend-a | Montirali `ui/` mapu s `StaticFiles` u FastAPI-ju |
| Dodali HTTP poslužitelj JavaScript varijanti | Kreirali `server.mjs` koristeći ugrađeni Node.js `http` modul |
| Dodali web API C# varijanti | Kreirali novi `csharp-web` projekt s ASP.NET Core minimalnim API-jima |
| Konzumirali streaming NDJSON u pregledniku | Koristili `fetch()` s `ReadableStream` i parsiranje JSON-a liniju po liniju |
| Ažurirali UI u stvarnom vremenu | Mapirali tipove poruka u DOM ažuriranja (bedževi, tekst, sklapanje panela) |

---

## Ključni zaključci

1. **Zajedničko statično sučelje** može raditi s bilo kojim backendom koji koristi isti streaming protokol, što potvrđuje vrijednost OpenAI-kompatibilnog API obrasca.
2. **Newline-delimited JSON (NDJSON)** je jednostavan streaming format koji radi "iz kutije" s browserovim `ReadableStream` API-jem.
3. **Python varijanta** je trebala najmanje promjena jer je već imala FastAPI endpoint; JavaScript i C# varijante trebale su tanki HTTP omotač.
4. Održavanje UI-ja kao **vanilla HTML/CSS/JS** izbjegava build alate, ovisnosti o frameworku i dodatnu složenost za polaznike radionice.
5. Isti agentski moduli (Researcher, Product, Writer, Editor) koriste se bez izmjena; mijenja se samo transportni sloj.

---

## Dodatna literatura

| Resurs | Link |
|----------|------|
| MDN: Korištenje Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Statične datoteke | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Statične datoteke | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specifikacija | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Nastavite na [Dio 13: Završetak radionice](part13-workshop-complete.md) za sažetak svega što ste izgradili tijekom ove radionice.

---
[← Dio 11: Pozivanje alata](part11-tool-calling.md) | [Dio 13: Završetak radionice →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje**:
Ovaj dokument je preveden pomoću AI usluge prevođenja [Co-op Translator](https://github.com/Azure/co-op-translator). Iako nastojimo postići točnost, imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku trebao bi se smatrati autoritativnim izvorom. Za kritične informacije preporučuje se profesionalni ljudski prijevod. Ne odgovaramo za bilo kakva nesporazuma ili pogrešna tumačenja proizlazeća iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->