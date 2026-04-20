![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 12: Veebi kasutajaliidese loomine Zava Creative Writer’ile

> **Eesmärk:** Lisada brauseripõhine kasutajaliides Zava Creative Writer’ile, et saaksite jälgida mitme agendi torujuhtme tööd reaalajas, koos elavate agendi staatuse märkide ja voogedastatud artikli tekstiga, kõik serveeritud ühest kohalikust veebiserverist.

Osa 7-s ([Part 7](part7-zava-creative-writer.md)) uurisite Zava Creative Writer’it kui **CLI rakendust** (JavaScript, C#) ja **peata API-d** (Python). Selles laboris ühendate jagatud **vanilla HTML/CSS/JavaScript** kasutajaliidese iga backendiga, nii et kasutajad saavad torujuhtmega suhelda brauseri kaudu mitte terminali kaudu.

---

## Mida õpid

| Eesmärk | Kirjeldus |
|-----------|-------------|
| Staatiliste failide serveerimine backendist | Paiguta HTML/CSS/JS kataloog API marsruudi kõrvale |
| Streaming NDJSON tarbimine brauseris | Kasuta Fetch API-t koos `ReadableStream`-iga, et lugeda reavahemärgiga eraldatud JSON-i |
| Ühtne streaming protokoll | Kindlusta, et Python-, JavaScript- ja C#-backendid väljastavad sama sõnumiformaadi |
| Progressiivsed kasutajaliidese uuendused | Uuenda agendi staatuse märke ja voogedasta artikli teksti sõniti |
| HTTP kihi lisamine CLI rakendusele | Pakenda olemasolev orkestreerimisloogika Expressi tüüpi serverisse (JS) või ASP.NET Core minimaalsete API-dega (C#) |

---

## Arhitektuur

Kasutajaliides koosneb ühest staatiliste failide komplektist (`index.html`, `style.css`, `app.js`), mida jagavad kõik kolm backendit. Iga backend pakub samu kahte marsruuti:

![Zava UI arhitektuur — jagatud kasutajaliides kolme backendiga](../../../images/part12-architecture.svg)

| Marsruut | Meetod | Eesmärk |
|-------|--------|---------|
| `/` | GET | Serveerib staatilist kasutajaliidest |
| `/api/article` | POST | Käivitab mitme agendi torujuhtme ja voogedastab NDJSON-i |

Kasutajaliides saadab JSON keha ja loeb vastust kui reavahemärgiga eraldatud JSON sõnumite voogu. Igal sõnumil on `type` väli, mida UI kasutab õige paneeli uuendamiseks:

| Sõnumitüüp | Tähendus |
|-------------|---------|
| `message` | Staatuse uuendus (nt "Algab uurija agendi ülesanne...") |
| `researcher` | Uurimistulemused on valmis |
| `marketing` | Toote otsingu tulemused on valmis |
| `writer` | Kirjutaja alustas või lõpetas (sisaldab `{ start: true }` või `{ complete: true }`) |
| `partial` | Kirjutajalt voogedastatud üksiku tokeni osa (sisaldab `{ text: "..." }`) |
| `editor` | Toimetaja otsus on valmis |
| `error` | Ilmnes viga |

![Sõnumite tüüpide marsruutimine brauseris](../../../images/part12-message-types.svg)

![Voogedastuse jada — brauseri ja backend’i kommunikatsioon](../../../images/part12-streaming-sequence.svg)

---

## Eeltingimused

- Lõpetatud [Osa 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI installitud ja mudel `phi-3.5-mini` alla laetud
- Moodne veebibrauser (Chrome, Edge, Firefox või Safari)

---

## Jagatud kasutajaliides

Enne backend koodi puudutamist vaata üle kasutajaliides, mida kasutavad kõik kolm programmeerimiskeelt. Failid asuvad kataloogis `zava-creative-writer-local/ui/`:

| Fail | Eesmärk |
|------|---------|
| `index.html` | Lehe paigutus: sisendi vorm, agendi staatuse märgised, artikli väljundala, kokkuvolditavad detailipaneelid |
| `style.css` | Minimaalne stiil koos staatuse märkide värvi seisunditega (ootab, töötab, valmis, viga) |
| `app.js` | Fetch-kõne, `ReadableStream` reaalaja ridade lugemine ja DOM-i uuendamise loogika |

> **Vihje:** Ava `index.html` otse brauseris, et vaadata paigutust. Midagi ei tööta veel, sest backend puudub, kuid saad struktuuri näha.

### Kuidas voolulugeja töötab

Oluline funktsioon `app.js`-is loeb vastuse keha tükikaupa ja lõikab selle reavahede kohalt:

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
    buffer = lines.pop(); // säilita puudulik järgneva rea lõpp

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

Iga töödeldud sõnum saadetakse `handleMessage()`-le, mis uuendab vastavaid DOM elemente lähtudes `msg.type` väärtusest.

---

## Harjutused

### Harjutus 1: Käivita Python backend koos UI-ga

Python (FastAPI) variandil on juba olemas streaming API lõpp-punkt. Ainus muudatus on `ui/` kausta staatiliste failidena sidumine.

**1.1** Liigu Python API kataloogi ja paigalda sõltuvused:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Käivita server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Ava brauseris `http://localhost:8000`. Näed Zava Creative Writer kasutajaliidest, kus on kolm tekstivälja ja nupp "Generate Article".

**1.4** Vajuta **Generate Article** kasutades vaikeseadeid. Jälgi, kuidas agendi staatuse märgid muutuvad "Ootamisest" "Töötavaks" ja lõpuks "Valmis", kui iga agent ülesande lõpetab ning toodud artikli tekst voogedastatakse väljundpaneeli sõniti.

> **Rikkefikseerimine:** Kui lehel kuvatakse JSON vastus UI asemel, veendu, et jooksutad uuendatud `main.py` faili, mis monteerib staatilised failid. `/api/article` lõpp-punkt töötab endiselt algsel teel; staatiliste failide monteerimine serveerib UI-d kõigil teistel marsruutidel.

**Kuidas see töötab:** Uuendatud `main.py` lisab ühe rea lehe lõppu:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

See serveerib kõiki faile kataloogist `zava-creative-writer-local/ui/` staatiliste varadena, vaikimisi dokumendiks on `index.html`. `/api/article` POST marsruut on registreeritud staatiliste failide monteerimisest eespool, nii et sellel on kõrgem prioriteet.

---

### Harjutus 2: Lisa veebiserver JavaScript variandile

JavaScript variant on hetkel CLI rakendus (`main.mjs`). Uus fail `server.mjs` pakub samu agendi mooduleid HTTP serveri taga ja serveerib jagatud kasutajaliidest.

**2.1** Liigu JavaScript kataloogi ja paigalda sõltuvused:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Käivita veebiserver:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Peaksid nägema:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Ava brauseris `http://localhost:3000` ja vajuta **Generate Article**. Sama kasutajaliides töötab takkaotsa JavaScript backendiga.

**Õpi koodi:** Ava `server.mjs` ja pane tähele peamisi mustreid:

- **Staatiliste failide serveerimine** kasutab Node.js sisseehitatud `http`, `fs` ja `path` mooduleid ilma välise raamistiku vajaduseta.
- **Teekondade kaitse** normaliseerib nõutud tee ja kontrollib, et see jääb `ui/` kataloogi sisse.
- **NDJSON voogedastus** kasutab abifunktsiooni `sendLine()`, mis serialiseerib iga objekti, eemaldab sisemised reavahed ja lisab lõpus reavahe.
- **Agendi orkestreerimine** taaskasutab muutmata olemasolevaid `researcher.mjs`, `product.mjs`, `writer.mjs` ja `editor.mjs` mooduleid.

<details>
<summary>Oluline lõik server.mjs-st</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Teadur
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Kirjutaja (voogedastus)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Harjutus 3: Lisa minimaalne API C# variandile

C# variant on praegu konsoolirakendus. Uus projekt `csharp-web` kasutab ASP.NET Core minimaalsete API-dega, et avaldada sama torujuhe veeniteenusena.

**3.1** Liigu C# veebiprojekti kataloogi ja taasta paketid:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Käivita veebiserver:

```bash
dotnet run
```

```powershell
dotnet run
```

Peaksid nägema:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Ava brauseris `http://localhost:5000` ja vajuta **Generate Article**.

**Õpi koodi:** Ava kataloogis `csharp-web` fail `Program.cs` ja pane tähele:

- Projekti fail kasutab `Microsoft.NET.Sdk.Web` asemel `Microsoft.NET.Sdk`, mis lisab ASP.NET Core toe.
- Staatilisi faile serveeritakse `UseDefaultFiles` ja `UseStaticFiles` abil, viidates jagatud `ui/` kataloogile.
- `/api/article` lõpp-punkt kirjutab otse NDJSON ridu `HttpContext.Response`-i ja loputab iga rea järel reaalaja voogedastuseks.
- Kõik agendi loogikad (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) on samad mis konsooliversioonis.

<details>
<summary>Oluline lõik failist csharp-web/Program.cs</summary>

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

### Harjutus 4: Uuri agendi staatuse märke

Kui sul on töötav UI, vaata, kuidas kasutajaliides uuendab staatuse märke.

**4.1** Ava `zava-creative-writer-local/ui/app.js` oma tekstiredaktoris.

**4.2** Leia funktsioon `handleMessage()`. Pane tähele, kuidas see seob sõnumitüübid DOM-i uuendustega:

| Sõnumitüüp | Kasutajaliidese tegevus |
|-------------|-------------------------|
| `message`, mis sisaldab "researcher" | Määrab uurija märgise olekuks "Töötav" |
| `researcher` | Määrab uurija märgise olekuks "Valmis" ja täidab Uurimistulemuste paneeli |
| `marketing` | Määrab toote otsingu märgise olekuks "Valmis" ja täidab Toote vaste paneeli |
| `writer` koos `data.start`-iga | Määrab kirjutaja märgise olekuks "Töötav" ja tühjendab artikli väljundi |
| `partial` | Lisab tokeni teksti artikli väljundisse |
| `writer` koos `data.complete`-ga | Määrab kirjutaja märgise olekuks "Valmis" |
| `editor` | Määrab toimetaja märgise olekuks "Valmis" ja täidab Toimetaja tagasiside paneeli |

**4.3** Ava artikli all olevad kokkuvolditavad paneelid "Research Results", "Product Matches" ja "Editor Feedback", et vaadata iga agendi loodud tooret JSON-it.

---

### Harjutus 5: Kohanda kasutajaliidest (täiendav)

Proovi mõnda järgmistest täiustustest:

**5.1 Lisa sõnade arv.** Pärast kirjutaja lõpetamist näita artikli sõnade arvu väljundpaneeli all. See arvutus käib `handleMessage`-is juhul, kui `type === "writer"` ja `data.complete` on tõene:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Lisa korduse indikaator.** Kui toimetaja nõuab redigeerimist, käivitatakse torujuhe uuesti. Näita staatuse paneelil bännerit "Revision 1" või "Revision 2". Kuula `message` tüüpi sõnumeid, mis sisaldavad "Revision" ja uuenda uue DOM elemendiga.

**5.3 Tume režiim.** Lisa lülitunud nupp ja `.dark` klass `<body>` elemendile. Muuda `style.css`-is tausta, teksti ja paneelide värve `body.dark` valija abil.

---

## Kokkuvõte

| Mida tegid | Kuidas |
|-------------|--------|
| Serveerisid UI Python backendi kaudu | Paigutasid `ui/` kausta FastAPI `StaticFiles`-iga |
| Lisasingi HTTP serveri JavaScript variandile | Lood server.mjs Node.js sisseehitatud http mooduli abil |
| Lisasingi veebipõhise API C# variandile | Lood uue `csharp-web` projekti ASP.NET Core minimaalsete API-dega |
| Tarbisid voogedastatud NDJSON-i brauseris | Kasutasid `fetch()` koos `ReadableStream` ja joon-joonelise JSON parsingu abil |
| Uuendasid UI-d reaalajas | Kaardistasid sõnumitüübid DOM uuendusteks (märgised, tekst, kokkuvolditavad paneelid) |

---

## Peamised järeldused

1. **Jagatud staatiline kasutajaliides** toimib mis tahes backendiga, mis räägib sama streaming-protokolli, tugevdades OpenAI-ga ühilduva API mustri väärtust.
2. **Reavahemärgiga eraldatud JSON (NDJSON)** on lihtne streaming formaat, mis töötab brauseri `ReadableStream` API-ga natiivselt.
3. **Python variant** vajas vähimat muutmist, kuna sellel oli juba FastAPI lõpp-punkt; JavaScript ja C# variandid vajasid õhukest HTTP kesta.
4. Kasutajaliides säilitati **vanilla HTML/CSS/JS** kujul, vältides ehitusvahendeid, raamistiku sõltuvusi ja lisakompleksust töötubade õppijatele.
5. Samad agendi moodulid (Researcher, Product, Writer, Editor) taaskasutati muutmata; muutus oli ainult transpordi kihis.

---

## Täiendav lugemine

| Ressurss | Link |
|----------|------|
| MDN: Readable Stream’i kasutamine | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Staatilised failid | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Staatilised failid | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON spetsifikatsioon | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Jätka [Osa 13: Töötoa lõpetamine](part13-workshop-complete.md), et saada kokkuvõte kõigest, mida selle töötoa jooksul ehitasid.

---
[← Osa 11: Tööriista kutsumine](part11-tool-calling.md) | [Osa 13: Töötuba lõpetatud →](part13-workshop-complete.md)