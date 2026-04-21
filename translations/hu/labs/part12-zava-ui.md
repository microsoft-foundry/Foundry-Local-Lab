![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 12. rész: Webes UI készítése a Zava Kreatív Íróhoz

> **Cél:** Hozz létre egy böngésző-alapú frontendet a Zava Kreatív Íróhoz, hogy valós időben nyomon kövesd a többügynökös folyamat futását, élő ügynök állapotjelzőkkel és folyamatosan érkező cikk szöveggel, mindezt egyetlen helyi web szerverről kiszolgálva.

A [7. részben](part7-zava-creative-writer.md) megismerted a Zava Kreatív Írót mint **parancssori alkalmazást** (JavaScript, C#) és mint **headless API-t** (Python). Ebben a laborban egy megosztott **vanilla HTML/CSS/JavaScript** frontendet kapcsolsz az egyes backendekhez, hogy a felhasználók böngészőn keresztül, terminál használata nélkül léphessenek interakcióba a folyamattal.

---

## Amit megtanulsz

| Célkitűzés | Leírás |
|-----------|-------------|
| Statikus fájlok kiszolgálása backendről | HTML/CSS/JS könyvtár csatolása az API útvonal mellé |
| Streaming NDJSON feldolgozása böngészőben | A Fetch API és `ReadableStream` használata, hogy soronként olvasd a newline-delimitált JSON-t |
| Egységes streaming protokoll | Biztosítani, hogy a Python, JavaScript és C# backendek ugyanazt az üzenetformátumot adják ki |
| Progresszív UI frissítések | Ügynök állapotjelzők frissítése és cikk szöveg tokenenkénti streamelése |
| HTTP réteg hozzáadása CLI alkalmazáshoz | Meglévő vezérlő logika becsomagolása Express-szerű szerverbe (JS) vagy ASP.NET Core minimális API-ba (C#) |

---

## Architektúra

A UI egyetlen statikus fájlkészlet (`index.html`, `style.css`, `app.js`), amit mindhárom backend használ. Mindegyik backend ugyanazt a két útvonalat szolgálja ki:

![Zava UI architektúra – megosztott front end három backenddel](../../../images/part12-architecture.svg)

| Útvonal | Metódus | Cél |
|---------|---------|-----|
| `/` | GET | A statikus UI kiszolgálása |
| `/api/article` | POST | Többügynökös folyamat futtatása és NDJSON streamelése |

A front end JSON törzset küld és a választ newline-delimitált JSON üzenetek streamjeként olvassa. Minden üzenet rendelkezik egy `type` mezővel, amit a UI használ a megfelelő panel frissítéséhez:

| Üzenettípus | Jelentés |
|-------------|----------|
| `message` | Állapotfrissítés (pl. "Kutató ügynök feladat indítása...") |
| `researcher` | A kutatási eredmények készen állnak |
| `marketing` | A termék keresési eredmények készen állnak |
| `writer` | Író indult vagy befejezett (tartalmaz `{ start: true }` vagy `{ complete: true }`) |
| `partial` | Tegnapi író által küldött egyetlen token (tartalmaz `{ text: "..." }`) |
| `editor` | Szerkesztői vélemény készen áll |
| `error` | Valami hiba történt |

![Üzenettípus alapú útválasztás a böngészőben](../../../images/part12-message-types.svg)

![Streaming leképezés — Böngésző és Backend közti kommunikáció](../../../images/part12-streaming-sequence.svg)

---

## Előfeltételek

- A [7. rész: Zava Kreatív Író](part7-zava-creative-writer.md) befejezése
- Foundry Local CLI telepítve és a `phi-3.5-mini` modell letöltve
- Egy modern webböngésző (Chrome, Edge, Firefox vagy Safari)

---

## A megosztott UI

Mielőtt bármilyen backend kódhoz nyúlnál, nézd át a frontendet, amit mindhárom nyelvi változat használni fog. A fájlok a `zava-creative-writer-local/ui/` könyvtárban találhatók:

| Fájl | Cél |
|-------|-----|
| `index.html` | Oldal felépítés: bemeneti űrlap, ügynök állapotjelzők, cikk megjelenítő terület, összecsukható részletek panel |
| `style.css` | Minimális stílus állapotjelző színállapotokkal (várakozás, futás, kész, hiba) |
| `app.js` | Fetch hívás, `ReadableStream` sorolvasó és DOM frissítő logika |

> **Tipp:** Nyisd meg az `index.html`-t közvetlenül a böngésződben a kinézet megtekintéséhez. Még nem fog működni, mert nincs backend, de láthatod a struktúrát.

### Hogyan működik az Stream olvasó

Az `app.js` kulcsfunkciója a válasz törzsét darabonként olvassa, és sorelválasztó mentén osztja szét:

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
    buffer = lines.pop(); // tartsa meg a hiányos lezáró sort

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

Minden feldolgozott üzenetet a `handleMessage()` függvényhez továbbít, ami a `msg.type` alapján frissíti a megfelelő DOM elemet.

---

## Gyakorlatok

### Gyakorlat 1: A Python backend futtatása a UI-val

A Python (FastAPI) változat már rendelkezik streaming API végponttal. Az egyetlen változás, hogy statikus fájlként csatoljuk az `ui/` mappát.

**1.1** Navigálj a Python API könyvtárba és telepítsd a függőségeket:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Indítsd el a szervert:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Nyisd meg a böngésződben a `http://localhost:8000` címet. Látnod kell a Zava Kreatív Író UI-t három szövegmezejével és egy "Generate Article" gombbal.

**1.4** Kattints a **Generate Article** gombra az alapértelmezett értékekkel. Figyeld, ahogy az ügynök állapotjelzők „Várakozás”-ból „Futás”-ba, majd „Kész”-be váltanak, miközben a cikk szöveg tokenenként folyamatosan megjelenik a kimeneti panelen.

> **Hibaelhárítás:** Ha az oldal JSON választ jelenít meg UI helyett, győződj meg, hogy a frissített `main.py`-t futtatod, amely csatolja a statikus fájlokat. Az `/api/article` végpont továbbra is az eredeti útvonalon működik; a statikus fájlok csatolása minden más útvonalon kiszolgálja az UI-t.

**Hogyan működik:** A frissített `main.py` a végén egyetlen sort ad hozzá:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Ez az összes fájlt a `zava-creative-writer-local/ui/` helyről statikus tartalomként szolgálja ki, az alapértelmezett dokumentum az `index.html`. Az `/api/article` POST útvonal a statikus csatolás előtt van regisztrálva, ezért prioritás élvez.

---

### Gyakorlat 2: Web szerver hozzáadása a JavaScript változathoz

A JavaScript változat jelenleg CLI alkalmazás (`main.mjs`). Egy új fájl, `server.mjs`, HTTP szerver mögé csomagolja ugyanazokat az ügynök modulokat és kiszolgálja a megosztott UI-t.

**2.1** Navigálj a JavaScript könyvtárba és telepítsd a függőségeket:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Indítsd el a web szervert:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Ezt kell látnod:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Nyisd meg a `http://localhost:3000` címet a böngésződben és kattints a **Generate Article** gombra. Ugyanaz a UI működik ugyanígy a JavaScript backenddel is.

**Tanulmányozd a kódot:** Nyisd meg a `server.mjs`-t, és jegyezd meg a kulcsmintákat:

- **Statikus fájlok kiszolgálása** Node.js beépített `http`, `fs` és `path` modulokkal, külső keretrendszer nélkül.
- **Útvonal-védelmi mechanizmus** normalizálja a kért útvonalat és ellenőrzi, hogy az a `ui/` könyvtárban marad.
- **NDJSON streaming** egy `sendLine()` segédfüggvényt használ, ami objektumokat sorosan JSON-be konvertál, belső sorelválasztókat eltávolít, és egy új sort tesz a végére.
- **Ügynök vezérlés** újrahasznosítja az eredeti `researcher.mjs`, `product.mjs`, `writer.mjs` és `editor.mjs` modulokat változtatás nélkül.

<details>
<summary>Kulcsrészlet a server.mjs-ből</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Kutató
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Író (live közvetítés)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Gyakorlat 3: Minimális API hozzáadása a C# változathoz

A C# változat jelenleg konzolos alkalmazás. Egy új projekt, `csharp-web`, ASP.NET Core minimal API-k segítségével teszi elérhetővé ugyanazt a folyamatot webszolgáltatásként.

**3.1** Navigálj a C# web projekthez és állítsd vissza a csomagokat:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Indítsd el a web szervert:

```bash
dotnet run
```

```powershell
dotnet run
```

Ezt kell látnod:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Nyisd meg `http://localhost:5000` címet a böngésződben és kattints a **Generate Article** gombra.

**Tanulmányozd a kódot:** Nyisd meg a `Program.cs` fájlt a `csharp-web` könyvtárban és nézd meg:

- A projekt fájl `Microsoft.NET.Sdk.Web`-et használ `Microsoft.NET.Sdk` helyett, ami ASP.NET Core támogatást ad.
- Statikus fájlokat a `UseDefaultFiles` és `UseStaticFiles` szolgálja ki, a megosztott `ui/` könyvtár felé mutatva.
- Az `/api/article` végpont NDJSON sorokat ír közvetlenül az `HttpContext.Response`-re és minden sor után flush-ol, hogy valós idejű legyen a streaming.
- Minden ügynök logika (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) ugyanaz, mint a konzolos változatban.

<details>
<summary>Kulcsrészlet a csharp-web/Program.cs-ből</summary>

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

### Gyakorlat 4: Fedezd fel az ügynök állapotjelzőket

Most, hogy működik a UI, nézd meg, hogyan frissíti a front end az állapotjelzőket.

**4.1** Nyisd meg `zava-creative-writer-local/ui/app.js`-t a szerkesztődben.

**4.2** Keresd meg a `handleMessage()` függvényt. Figyeld meg, hogyan társítja az üzenettípusokat DOM frissítésekhez:

| Üzenettípus | UI művelet |
|-------------|------------|
| `message` amely tartalmazza a "researcher" szót | A Researcher jelzőt "Running"-re állítja |
| `researcher` | A Researcher jelzőt "Done"-ra állítja és kitölti a Kutatási eredmények panelt |
| `marketing` | A Product Search jelzőt "Done"-ra állítja és kitölti a Termék találatok panelt |
| `writer` `data.start`-tal | A Writer jelzőt "Running"-re állítja és üríti a cikk kimeneti területet |
| `partial` | Hozzáfűzi a token szöveget a cikk kimenethez |
| `writer` `data.complete`-tel | A Writer jelzőt "Done"-ra állítja |
| `editor` | Az Editor jelzőt "Done"-ra állítja és kitölti a Szerkesztői visszajelzés panelt |

**4.3** Nyisd ki az összecsukható "Research Results", "Product Matches" és "Editor Feedback" paneleket a cikk alatt, hogy megtekintsd az ügynökök által generált nyers JSON-t.

---

### Gyakorlat 5: Testreszabás (Kihívás)

Próbálj ki egy vagy több fejlesztést:

**5.1 Szó számláló hozzáadása.** Miután az Író befejezte, jelenítsd meg a cikk szó számát a kimeneti panel alatt. Ezt a `handleMessage`-ben számolhatod ki, amikor a `type === "writer"` és `data.complete` igaz:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Újrapróbálkozás jelző.** Amikor a Szerkesztő javítást kér, a folyamat újraindul. Mutass egy "Revision 1" vagy "Revision 2" címkét az állapotpanelen. Hallgass egy `message` típust, amely tartalmazza a "Revision" szót, és frissíts egy új DOM elemet.

**5.3 Sötét mód.** Adj hozzá egy kapcsoló gombot és egy `.dark` osztályt a `<body>`-hoz. A `style.css`-ben írd felül a háttér, szöveg és panel színeket a `body.dark` szelektorral.

---

## Összefoglaló

| Mit csináltál | Hogyan |
|---------------|--------|
| UI kiszolgálása Python backendről | Az `ui/` könyvtár csatolása `StaticFiles`-el FastAPI alatt |
| HTTP szerver hozzáadása JavaScript változathoz | `server.mjs` létrehozása Node.js beépített `http` moduljával |
| Web API hozzáadása C# változathoz | Új `csharp-web` projekt ASP.NET Core minimal API-kkal |
| Streaming NDJSON fogyasztása böngészőben | `fetch()` és `ReadableStream` soronkénti JSON feldolgozás használata |
| UI valós idejű frissítése | Üzenettípusok leképezése DOM frissítésekre (jelzők, szöveg, összecsukható panelek) |

---

## Fő tanulságok

1. Egy **megosztott statikus front end** bármilyen backenddel működhet, amely azonos streaming protokollt használ, megerősítve az OpenAI-kompatibilis API minta értékét.
2. A **newline-delimitált JSON (NDJSON)** egyszerű streaming formátum, amely natívan működik a böngésző `ReadableStream` API-jával.
3. A **Python változat** igényelte a legkevesebb változtatást, mert már volt FastAPI végpontja; a JavaScript és C# változatoknak egy vékony HTTP csomagoló kellett.
4. A UI **vanilla HTML/CSS/JS** marad, így elkerüli az építőeszközöket, a keretrendszer-függőségeket és nem növeli a bonyolultságot a workshop résztvevőinek.
5. Ugyanazokat az ügynök modulokat (Researcher, Product, Writer, Editor) változatlanul újrahasznosítjuk; csak a szállítási réteg változik.

---

## További olvasmányok

| Forrás | Link |
|--------|------|
| MDN: Readable Stream-ek használata | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI statikus fájlok | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core statikus fájlok | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON specifikáció | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Folytasd a [13. résszel: Workshop befejezve](part13-workshop-complete.md), ahol összefoglaljuk az egész workshop során felépített megoldásokat.

---
[← 11. rész: Eszközhívás](part11-tool-calling.md) | [13. rész: Műhely teljes →](part13-workshop-complete.md)