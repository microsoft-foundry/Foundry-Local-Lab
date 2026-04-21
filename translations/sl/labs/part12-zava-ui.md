![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 12: Izdelava spletnega uporabniškega vmesnika za Zava Creative Writer

> **Cilj:** Dodati brskalniku dostopen uporabniški vmesnik za Zava Creative Writer, tako da lahko v realnem času spremljate izvajanje večagentske cevovode, z oznakami stanja agentov v živo in pretakanim besedilom člankov, vse skupaj pa se streže iz enega lokalnega spletnega strežnika.

V [Delu 7](part7-zava-creative-writer.md) ste raziskovali Zava Creative Writer kot **CLI aplikacijo** (JavaScript, C#) in kot **headless API** (Python). V tej vaji boste povezali skupen **navaden HTML/CSS/JavaScript** uporabniški vmesnik s vsako back-end rešitvijo, da bodo uporabniki lahko upravljali cevovod preko brskalnika namesto terminala.

---

## Kaj se boste naučili

| Cilj | Opis |
|-----------|-------------|
| Strežba statičnih datotek iz backenda | Pripnete HTML/CSS/JS imenik poleg vaše API poti |
| Uporaba pretakanja NDJSON v brskalniku | Uporabite Fetch API z `ReadableStream` za branje JSON sporočil, ločenih z novo vrstico |
| Enoten protokol pretakanja | Poskrbite, da Python, JavaScript in C# backendi pošiljajo sporočila v istem formatu |
| Postopne posodobitve UI | Posodobite statusne značke agentov in pretakajte besedilo članka znak za znakom |
| Dodajanje HTTP plasti k CLI aplikaciji | Ovijte obstoječo logiko orkestratorja v strežnik tipa Express (JS) ali minimalni API ASP.NET Core (C#) |

---

## Arhitektura

UI je ena sama zbirka statičnih datotek (`index.html`, `style.css`, `app.js`), ki jih delijo vsi trije backendi. Vsak backend razkriva isti dve poti:

![Zava UI arhitektura — skupni uporabniški vmesnik s tremi backend-i](../../../images/part12-architecture.svg)

| Pot | Metoda | Namen |
|-------|--------|---------|
| `/` | GET | Streže statični UI |
| `/api/article` | POST | Izvede večagentski cevovod in pretaka NDJSON |

Uporabniški vmesnik pošlje telo kot JSON in prebere odziv kot tok sporočil JSON, ločenih z novo vrstico. Vsako sporočilo ima polje `type`, ki ga UI uporablja za posodobitev pravilnega panela:

| Tip sporočila | Pomen |
|-------------|---------|
| `message` | Posodobitev stanja (npr. "Začenjam nalogo raziskovalnega agenta...") |
| `researcher` | Raziskovalni rezultati so pripravljeni |
| `marketing` | Rezultati iskanja izdelka so pripravljeni |
| `writer` | Pisatelj je začel ali končal (vsebuje `{ start: true }` ali `{ complete: true }`) |
| `partial` | Posamezni pretakani token iz pisatelja (vsebuje `{ text: "..." }`) |
| `editor` | Mnenje urednika je pripravljeno |
| `error` | Prišlo je do napake |

![Usmerjanje tipov sporočil v brskalniku](../../../images/part12-message-types.svg)

![Zaporedje pretakanja — komunikacija brskalnik na backend](../../../images/part12-streaming-sequence.svg)

---

## Predpogoji

- Zaključite [Del 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Namestite Foundry Local CLI in prenesite model `phi-3.5-mini`
- Sodobni spletni brskalnik (Chrome, Edge, Firefox ali Safari)

---

## Skupni UI

Preden se lotite kakršnekoli kode backend-a, si vzemite trenutek za raziskovanje uporabniškega vmesnika, ki ga bodo vsi trije jezikovni sledovi uporabljali. Datoteke se nahajajo v `zava-creative-writer-local/ui/`:

| Datoteka | Namen |
|------|---------|
| `index.html` | Postavitev strani: obrazec za vhod, statusne značke agentov, izhodno območje članka, zložljivi detajlni paneli |
| `style.css` | Minimalno stiliranje s stanji barv statusnih značk (čakanje, izvajanje, končano, napaka) |
| `app.js` | Klic Fetch, bralnik vrstic `ReadableStream` in logika posodobitve DOM-a |

> **Namig:** Odprite `index.html` neposredno v brskalniku za ogled postavitve. Še nič ne bo delovalo, ker ni backend-a, vendar boste videli strukturo.

### Kako deluje bralnik toka

Ključna funkcija v `app.js` bere telo odziva po delih in razdeli na meje novih vrstic:

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
    buffer = lines.pop(); // ohrani nedokončano zadnjo vrstico

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

Vsako razčlenjeno sporočilo se prenese v `handleMessage()`, ki posodobi ustrezen DOM element glede na `msg.type`.

---

## Vaje

### Vaja 1: Zaženite Python Backend z UI

Python (FastAPI) različica že ima streaming API končno točko. Edina sprememba je pripenjanje mape `ui/` kot statičnih datotek.

**1.1** Pomaknite se v imenik Python API in namestite odvisnosti:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Zaženite strežnik:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Odprite brskalnik na `http://localhost:8000`. Videli boste Zava Creative Writer UI s tremi besedilnimi polji in gumbom "Generate Article".

**1.4** Kliknite **Generate Article** z privzetimi vrednostmi. Opazujte, kako se statusne značke agentov spreminjajo od "Waiting" do "Running" do "Done", ko vsak agent zaključi svoje delo, in kako se besedilo članka prenaša v izhodni panel znak za znakom.

> **Odpravljanje težav:** Če stran prikazuje JSON odgovor namesto UI-ja, preverite, da zaženete posodobljen `main.py`, ki pripenja statične datoteke. `/api/article` POST končna točka še vedno deluje na svoji prvotni poti; montaža statičnih datotek streže UI na vseh ostalih poteh.

**Kako deluje:** Posodobljen `main.py` doda eno samo vrstico na dnu:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

To streže vsako datoteko iz `zava-creative-writer-local/ui/` kot statično vsebino, pri čemer je `index.html` privzeti dokument. POST pot `/api/article` je registrirana pred statično montažo in ima zato prednost.

---

### Vaja 2: Dodajte spletni strežnik JavaScript različici

JavaScript različica je trenutno CLI aplikacija (`main.mjs`). Nova datoteka, `server.mjs`, ovije iste agentne module za HTTP strežnik in streže skupen UI.

**2.1** Pomaknite se v imenik JavaScript in namestite odvisnosti:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Zaženite spletni strežnik:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Videli boste:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Odprite `http://localhost:3000` v brskalniku in kliknite **Generate Article**. Enak UI deluje enako z JavaScript backendom.

**Preučite kodo:** Odprite `server.mjs` in si oglejte ključne vzorce:

- **Strežba statičnih datotek** uporablja vgrajene Node.js module `http`, `fs` in `path` brez zunanjih ogrodij.
- **Zaščita pred prehodom poti** normalizira zahtevano pot in preveri, da ostaja znotraj imenika `ui/`.
- **Pretakanje NDJSON** uporablja pomočnika `sendLine()`, ki serializira vsak objekt, odstrani notranje nove vrstice in doda končno novo vrstico.
- **Orkestracija agentov** ponovno uporablja obstoječe module `researcher.mjs`, `product.mjs`, `writer.mjs` in `editor.mjs` brez sprememb.

<details>
<summary>Ključni izsek iz server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Raziskovalec
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Pisec (pretakanje)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Vaja 3: Dodajte minimalni API C# različici

C# različica je trenutno konzolna aplikacija. Novi projekt, `csharp-web`, uporablja minimalne API-je ASP.NET Core, da razkrije isti cevovod kot spletno storitev.

**3.1** Pomaknite se v C# spletni projekt in obnovite pakete:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Zaženite spletni strežnik:

```bash
dotnet run
```

```powershell
dotnet run
```

Videli boste:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Odprite `http://localhost:5000` v brskalniku in kliknite **Generate Article**.

**Preučite kodo:** Odprite `Program.cs` v imeniku `csharp-web` in upoštevajte:

- Projektna datoteka uporablja `Microsoft.NET.Sdk.Web` namesto `Microsoft.NET.Sdk`, kar doda podporo za ASP.NET Core.
- Statične datoteke se strežejo prek `UseDefaultFiles` in `UseStaticFiles` usmerjenih na skupni imenik `ui/`.
- Končna točka `/api/article` piše NDJSON vrstice neposredno v `HttpContext.Response` in izprazni izhod po vsaki vrstici za pretakanje v realnem času.
- Vsa logika agentov (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) je enaka kot v konzolni različici.

<details>
<summary>Ključni izsek iz csharp-web/Program.cs</summary>

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

### Vaja 4: Raziščite statusne značke agentov

Sedaj, ko imate delujoč UI, poglejte, kako uporabniški vmesnik posodablja statusne značke.

**4.1** Odprite `zava-creative-writer-local/ui/app.js` v urejevalniku.

**4.2** Poiščite funkcijo `handleMessage()`. Opazite, kako preslika tipe sporočil v posodobitve DOM-a:

| Tip sporočila | UI akcija |
|-------------|-----------|
| `message` vsebuje "researcher" | Nastavi značko Researcher na "Running" |
| `researcher` | Nastavi značko Researcher na "Done" in napolni panel Research Results |
| `marketing` | Nastavi značko Product Search na "Done" in napolni panel Product Matches |
| `writer` z `data.start` | Nastavi značko Writer na "Running" in počisti izhod članka |
| `partial` | Doda token besedila v izhod članka |
| `writer` z `data.complete` | Nastavi značko Writer na "Done" |
| `editor` | Nastavi značko Editor na "Done" in napolni panel Editor Feedback |

**4.3** Odprite zložljive panele "Research Results", "Product Matches" in "Editor Feedback" pod člankom, da si ogledate neobdelani JSON, ki ga je vsak agent ustvaril.

---

### Vaja 5: Prilagodite UI (razširitev)

Poskusite eno ali več teh izboljšav:

**5.1 Dodajte število besed.** Po tem, ko pisatelj konča, prikažite število besed članka pod izhodnim panelom. To lahko izračunate v `handleMessage`, ko `type === "writer"` in `data.complete` je resničen:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Dodajte indikator ponovitve.** Ko urednik zahteva revizijo, se cevovod znova zažene. Prikažite oznako "Revision 1" ali "Revision 2" v statusnem panelu. Poslušajte sporočila tipa `message`, ki vsebujejo "Revision" in posodobite nov DOM element.

**5.3 Temni način.** Dodajte gumb za preklop in `.dark` razred na `<body>`. Prepišite barve ozadja, besedila in panelov v `style.css` z selektorjem `body.dark`.

---

## Povzetek

| Kaj ste naredili | Kako |
|-------------|-----|
| Strežba UI iz Python backend-a | Pripeli mapo `ui/` s StaticFiles v FastAPI |
| Dodali HTTP strežnik JavaScript različici | Ustvarili `server.mjs` z vgrajenim Node.js modulom `http` |
| Dodali spletni API k C# različici | Ustvarili nov projekt `csharp-web` z minimalnimi ASP.NET Core API-ji |
| Porabljeno streaming NDJSON v brskalniku | Uporabili `fetch()` z `ReadableStream` in vrstično JSON analizo |
| Posodobili UI v realnem času | Preslikali tipe sporočil v posodobitve DOM-a (značke, besedilo, zložljivi paneli) |

---

## Ključni poudarki

1. **Skupni statični uporabniški vmesnik** lahko deluje z vsakim backendom, ki govori isti streaming protokol, kar poudarja vrednost vzorca API, združljivega z OpenAI.
2. **JSON, ločen z novimi vrsticami (NDJSON)**, je enostaven streaming format, ki nativno deluje z brskalniškim API-jem `ReadableStream`.
3. **Python različica** je zahtevala najmanj sprememb, ker je že imela FastAPI končno točko; JavaScript in C# različici sta potrebovali tanko HTTP ovojnico.
4. Ohranjanje UI-ja kot **navadnega HTML/CSS/JS** se izogne gradbenim orodjem, odvisnostim od ogrodij in dodatni kompleksnosti za udeležence delavnice.
5. Isti agentni moduli (Researcher, Product, Writer, Editor) se ponovno uporabljajo brez sprememb; edina razlika je prevozna plast.

---

## Nadaljnje branje

| Viri | Povezava |
|----------|------|
| MDN: Uporaba Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI statične datoteke | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core statične datoteke | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON specifikacija | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Nadaljujte na [Del 13: Delavnica končana](part13-workshop-complete.md) za povzetek vsega, kar ste izdelali v tej delavnici.

---
[← Del 11: Klic orodja](part11-tool-calling.md) | [Del 13: Delavnica zaključena →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Izjava o omejitvi odgovornosti**:
Ta dokument je bil preveden z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za točnost, vas prosimo, da upoštevate, da lahko avtomatizirani prevodi vsebujejo napake ali netočnosti. Izvirni dokument v njegovem izvirnem jeziku velja za avtoritativni vir. Za ključne informacije priporočamo strokovni človeški prevod. Nismo odgovorni za morebitne nesporazume ali napačne interpretacije, ki izhajajo iz uporabe tega prevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->