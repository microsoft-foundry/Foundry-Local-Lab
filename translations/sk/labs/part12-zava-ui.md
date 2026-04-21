![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Časť 12: Vytváranie webového UI pre Zava Creative Writer

> **Cieľ:** Pridať prehliadačové front end rozhranie pre Zava Creative Writer, aby ste mohli sledovať beh viacagentového procesu v reálnom čase, s live odznakmi stavu agentov a prúdovým prenášaním textu článku, všetko podávané z jedného lokálneho webového servera.

V [Časti 7](part7-zava-creative-writer.md) ste preskúmali Zava Creative Writer ako **CLI aplikáciu** (JavaScript, C#) a **headless API** (Python). V tomto cvičení pripojíte zdieľané **vanilkové HTML/CSS/JavaScript** front end rozhranie k jednotlivým backendom, aby používatelia mohli komunikovať s procesom cez prehliadač namiesto terminálu.

---

## Čo sa naučíte

| Cieľ | Popis |
|-----------|-------------|
| Podávanie statických súborov z backendu | Pripojenie HTML/CSS/JS priečinka vedľa API cesty |
| Spotreba streamovaného NDJSON v prehliadači | Použitie Fetch API s `ReadableStream` na čítanie JSON oddeleného novými riadkami |
| Zjednotený streaming protokol | Zabezpečiť, aby Python, JavaScript a C# backendy vydávali rovnaký formát správ |
| Postupné aktualizácie UI | Aktualizovať odznaky stavu agentov a prúdovo pridávať text článku token po tokene |
| Pridanie HTTP vrstvy ku CLI aplikácii | Zabaliť existujúcu orchestráciu do Express-štýlového servera (JS) alebo ASP.NET Core minimálneho API (C#) |

---

## Architektúra

UI je jediná sada statických súborov (`index.html`, `style.css`, `app.js`), ktoré zdieľajú všetky tri backendy. Každý backend exponuje tie isté dve cesty:

![Zava UI architektúra — zdieľaný front end s troma backendmi](../../../images/part12-architecture.svg)

| Cesta | Metóda | Účel |
|-------|--------|---------|
| `/` | GET | Podáva statické UI |
| `/api/article` | POST | Spúšťa viacagentový proces a streamuje NDJSON |

Front end posiela JSON telo a číta odpoveď ako stream správ oddelených novými riadkami. Každá správa má pole `type`, ktoré UI používa na aktualizáciu správneho panelu:

| Typ správy | Význam |
|-------------|---------|
| `message` | Aktualizácia stavu (napr. "Spúšťam úlohu výskumného agenta...") |
| `researcher` | Výsledky výskumu sú pripravené |
| `marketing` | Výsledky vyhľadávania produktu sú pripravené |
| `writer` | Spustenie alebo ukončenie písania (obsahuje `{ start: true }` alebo `{ complete: true }`) |
| `partial` | Jeden token streamovaný z Writeru (obsahuje `{ text: "..." }`) |
| `editor` | Verdikt editora je pripravený |
| `error` | Niečo sa pokazilo |

![Routovanie typov správ v prehliadači](../../../images/part12-message-types.svg)

![Sekvencia streamovania — komunikácia prehliadač → backend](../../../images/part12-streaming-sequence.svg)

---

## Predpoklady

- Dokončiť [Časť 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Mať nainštalovaný Foundry Local CLI a stiahnutý model `phi-3.5-mini`
- Moderný webový prehliadač (Chrome, Edge, Firefox alebo Safari)

---

## Zdieľané UI

Predtým, než sa dotknete backendu, venujte chvíľu preskúmaniu front endu, ktorý všetky tri jazykové vetvy používajú. Súbory sa nachádzajú v `zava-creative-writer-local/ui/`:

| Súbor | Účel |
|------|---------|
| `index.html` | Rozloženie stránky: vstupný formulár, odznaky stavu agentov, výstupná oblasť článku, zbaliteľné detailné panely |
| `style.css` | Minimálny štýl s farebnými stavmi odznakov (čakanie, beh, dokončené, chyba) |
| `app.js` | Fetch volanie, `ReadableStream` čítačka riadkov a logika aktualizácie DOM |

> **Tip:** Otvorte `index.html` priamo v prehliadači a ukážte si rozloženie. Ešte nič nebude fungovať, pretože nie je backend, ale môžete vidieť štruktúru.

### Ako funguje streamovací čítač riadkov

Kľúčová funkcia v `app.js` číta telo odpovede po kúskach a rozdeľuje podľa nových riadkov:

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
    buffer = lines.pop(); // zachovať neúplný záverečný riadok

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

Každá rozparsovaná správa je odoslaná do `handleMessage()`, ktorá aktualizuje relevantný DOM element podľa `msg.type`.

---

## Cvičenia

### Cvičenie 1: Spustite Python backend s UI

Python (FastAPI) varianta už má streaming API endpoint. Jediná zmena je pripojenie priečinka `ui/` ako statických súborov.

**1.1** Prejdite do adresára Python API a nainštalujte závislosti:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Spustite server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Otvorte prehliadač na `http://localhost:8000`. Mali by ste vidieť UI Zava Creative Writer s troma textovými poliami a tlačidlom „Generate Article“.

**1.4** Kliknite na **Generate Article** s predvolenými hodnotami. Sledujte, ako sa odznaky stavu agentov menia z „Waiting“ na „Running“ a potom na „Done“, keď každý agent dokončí svoju úlohu, a ako text článku prúdi token po tokene do výstupného panela.

> **Riešenie problémov:** Ak stránka ukazuje JSON odpoveď namiesto UI, uistite sa, že spúšťate aktualizovaný `main.py`, ktorý pripája statické súbory. Endpoint `/api/article` stále funguje na pôvodnej ceste; statické súbory slúžia UI na všetkých ostatných cestách.

**Ako to funguje:** Aktualizovaný `main.py` pridáva na spodok jednu riadku:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Táto riadka slúži všetky súbory z `zava-creative-writer-local/ui/` ako statický obsah, pričom `index.html` je predvolený dokument. Cesta POST `/api/article` je registrovaná pred statickým mountom, takže má prioritu.

---

### Cvičenie 2: Pridajte webový server ku JavaScript variante

JavaScript varianta je momentálne CLI aplikácia (`main.mjs`). Nový súbor `server.mjs` zabaluje tie isté agentné moduly za HTTP server a slúži zdieľané UI.

**2.1** Prejdite do JavaScript adresára a nainštalujte závislosti:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Spustite webový server:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Mali by ste vidieť:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Otvorte `http://localhost:3000` v prehliadači a kliknite na **Generate Article**. To isté UI funguje rovnako proti JavaScript backendu.

**Študujte kód:** Otvorte `server.mjs` a všimnite si kľúčové vzory:

- **Podávanie statických súborov** používa vstavané Node.js moduly `http`, `fs` a `path` bez potreby externého frameworku.
- **Ochrana proti path traversal** normalizuje požadovanú cestu a overuje, že zostáva v rámci priečinka `ui/`.
- **Streaming NDJSON** používa pomocnú funkciu `sendLine()`, ktorá serializuje každý objekt, odstraňuje vnútorné nové riadky a pridáva záverečný nový riadok.
- **Orchestrace agentov** znovupoužíva existujúce moduly `researcher.mjs`, `product.mjs`, `writer.mjs` a `editor.mjs` bez zmien.

<details>
<summary>Kľúčový úryvok zo server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Výskumník
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Spisovateľ (streamovanie)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Cvičenie 3: Pridajte minimálne API ku C# variante

C# varianta je zatiaľ konzolová aplikácia. Nový projekt `csharp-web` používa ASP.NET Core minimálne API, aby exponoval ten istý pipeline ako webovú službu.

**3.1** Prejdite do C# webového projektu a obnovte balíčky:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Spustite webový server:

```bash
dotnet run
```

```powershell
dotnet run
```

Mali by ste vidieť:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Otvorte `http://localhost:5000` v prehliadači a kliknite na **Generate Article**.

**Študujte kód:** Otvorte `Program.cs` v adresári `csharp-web` a všimnite si:

- Projektový súbor používa `Microsoft.NET.Sdk.Web` namiesto `Microsoft.NET.Sdk`, čo pridáva podporu ASP.NET Core.
- Statické súbory slúžia cez `UseDefaultFiles` a `UseStaticFiles` smerované na zdieľaný priečinok `ui/`.
- Endpoint `/api/article` píše NDJSON riadky priamo do `HttpContext.Response` a po každom riadku flushuje pre prúdovanie v reálnom čase.
- Všetka agentná logika (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) je rovnaká ako v konzolovej verzii.

<details>
<summary>Kľúčový úryvok z csharp-web/Program.cs</summary>

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

### Cvičenie 4: Preskúmajte odznaky stavu agentov

Teraz, keď máte funkčné UI, pozrite sa, ako front end aktualizuje odznaky stavu.

**4.1** Otvorte `zava-creative-writer-local/ui/app.js` vo vašom editore.

**4.2** Nájdite funkciu `handleMessage()`. Všimnite si, ako mapuje typy správ na aktualizácie DOM:

| Typ správy | Akcia v UI |
|-------------|-----------|
| `message` obsahujúce "researcher" | Nastaví odznak Researcher na „Running“ |
| `researcher` | Nastaví odznak Researcher na „Done“ a vyplní panel Výskumné výsledky |
| `marketing` | Nastaví odznak Product Search na „Done“ a vyplní panel Produktové zhody |
| `writer` s `data.start` | Nastaví odznak Writer na „Running“ a vyčistí výstup článku |
| `partial` | Pridá token text do výstupu článku |
| `writer` s `data.complete` | Nastaví odznak Writer na „Done“ |
| `editor` | Nastaví odznak Editor na „Done“ a vyplní panel Spätná väzba editora |

**4.3** Otvorte zbaliteľné panely „Research Results“, „Product Matches“ a „Editor Feedback“ pod článkom a prezrite si surový JSON, ktorý každý agent vyprodukoval.

---

### Cvičenie 5: Prispôsobte UI (voliteľné)

Vyskúšajte jedno alebo viac týchto vylepšení:

**5.1 Pridajte počet slov.** Po dokončení Writerom zobrazte pod výstupným panelom počet slov v článku. Môžete to vypočítať v `handleMessage` keď `type === "writer"` a `data.complete` je true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Pridanie indikátora opakovania.** Keď Editor vyžiada revíziu, pipeline sa spustí znovu. Zobrazte v paneli stavu banner „Revision 1“ alebo „Revision 2“. Počúvajte správu typu `message` obsahujúcu "Revision" a aktualizujte nový DOM element.

**5.3 Tmavý režim.** Pridajte prepínač a `.dark` triedu na `<body>`. Prepíšte pozadie, farbu textu a farby panelov v `style.css` pomocou selektora `body.dark`.

---

## Zhrnutie

| Čo ste urobili | Ako |
|-------------|-----|
| Podali UI z Python backendu | Pripojili priečinok `ui/` s `StaticFiles` vo FastAPI |
| Pridali HTTP server ku JavaScript variante | Vytvorili `server.mjs` s použitím vstavaného Node.js `http` modulu |
| Pridali webové API ku C# variante | Vytvorili nový projekt `csharp-web` s ASP.NET Core minimálnymi API |
| Spotrebovali streamované NDJSON v prehliadači | Použili `fetch()` s `ReadableStream` a line-by-line JSON parsovaním |
| Aktualizovali UI v reálnom čase | Namapovali typy správ na DOM aktualizácie (odznaky, texty, zbaliteľné panely) |

---

## Hlavné poznatky

1. **Zdieľaný statický front end** môže pracovať s akýmkoľvek backendom, ktorý používa rovnaký streaming protokol, čo potvrdzuje hodnotu OpenAI-kompatibilného API vzoru.
2. **NDJSON** (newline-delimited JSON) je jednoduchý streaming formát priamo podporovaný prehliadačovým API `ReadableStream`.
3. **Python varianta** potrebovala najmenej zmien, lebo už mala FastAPI endpoint; JavaScript a C# varianty potrebovali tenkú HTTP vrstvu.
4. Použitie **vanilkového HTML/CSS/JS** vyhýba sa build nástrojom, závislostiam na frameworkoch a zložitejším procesom pre účastníkov workshopov.
5. Rovnaké agentné moduly (Researcher, Product, Writer, Editor) sa používajú bez modifikácií; mení sa len transportná vrstva.

---

## Ďalšie čítanie

| Zdroj | Odkaz |
|----------|------|
| MDN: Použitie Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI statické súbory | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core statické súbory | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON špecifikácia | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Pre pokračovanie choďte na [Časť 13: Workshop dokončený](part13-workshop-complete.md) pre zhrnutie všetkého, čo ste v tomto workshope vybudovali.

---
[← Časť 11: Volanie nástroja](part11-tool-calling.md) | [Časť 13: Workshop dokončený →](part13-workshop-complete.md)