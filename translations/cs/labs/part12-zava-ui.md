![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Část 12: Vytváření webového UI pro Zava Creative Writer

> **Cíl:** Přidat do Zava Creative Writer prohlížečem ovládané front-end rozhraní, abyste mohli sledovat běh multi-agentního pipeline v reálném čase, s živými odznaky stavu agentů a streamovaným textem článku, vše servírované z jednoho místního webového serveru.

V [Části 7](part7-zava-creative-writer.md) jste prozkoumali Zava Creative Writer jako **CLI aplikaci** (JavaScript, C#) a **headless API** (Python). V tomto labu připojíte sdílený **vanilkový HTML/CSS/JavaScript** front-end k jednotlivým backendům, aby uživatelé mohli interagovat s pipeline přes prohlížeč místo terminálu.

---

## Co se naučíte

| Cíl | Popis |
|-----------|-------------|
| Servírovat statické soubory z backendu | Připojit HTML/CSS/JS adresář vedle vaší API routy |
| Spotřebovávat streaming NDJSON v prohlížeči | Použít Fetch API s `ReadableStream` pro čtení JSON odděleného novými řádky |
| Jednotný streamingový protokol | Zajistit, aby Python, JavaScript i C# backendy vydávaly stejný formát zpráv |
| Postupné aktualizace UI | Aktualizovat odznaky stavu agentů a streamovat text článku token po tokenu |
| Přidat HTTP vrstvu ke CLI aplikaci | Zabalit existující orchestrátorskou logiku do serveru ve stylu Express (JS) nebo ASP.NET Core minimal API (C#) |

---

## Architektura

UI je jediná sada statických souborů (`index.html`, `style.css`, `app.js`) sdílená všemi třemi backendy. Každý backend zpřístupňuje stejné dvě routy:

![Zava UI architektura — sdílený front end se třemi backendy](../../../images/part12-architecture.svg)

| Trasa | Metoda | Účel |
|-------|--------|---------|
| `/` | GET | Slouží statické UI |
| `/api/article` | POST | Spouští multi-agentní pipeline a streamuje NDJSON |

Front end odesílá JSON tělo a čte odpověď jako stream JSON zpráv oddělených novými řádky. Každá zpráva má pole `type`, které UI používá k aktualizaci správného panelu:

| Typ zprávy | Význam |
|-------------|---------|
| `message` | Aktualizace stavu (např. "Spouštím úlohu agenta badatele...") |
| `researcher` | Výsledky výzkumu jsou připravené |
| `marketing` | Výsledky hledání produktu jsou připravené |
| `writer` | Spuštění nebo dokončení psaní (obsahuje `{ start: true }` nebo `{ complete: true }`) |
| `partial` | Jeden token ze streamu od Writer (obsahuje `{ text: "..." }`) |
| `editor` | Rozhodnutí editora je připravené |
| `error` | Něco se pokazilo |

![Smerovani typu zpráv v prohlížeči](../../../images/part12-message-types.svg)

![Sekvence streamování — komunikace prohlížeče s backendem](../../../images/part12-streaming-sequence.svg)

---

## Požadavky

- Dokončit [Část 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Nainstalovaný Foundry Local CLI a stažený model `phi-3.5-mini`
- Moderní webový prohlížeč (Chrome, Edge, Firefox nebo Safari)

---

## Sdílené UI

Než se pustíte do kódu backendu, věnujte chvíli prozkoumání front endu, který bude používat všechna tři jazyková řešení. Soubory jsou umístěné v `zava-creative-writer-local/ui/`:

| Soubor | Účel |
|------|---------|
| `index.html` | Rozložení stránky: vstupní formulář, odznaky stavu agentů, výstupní oblast článku, skládací panely s detaily |
| `style.css` | Minimální stylování s barevnými stavovými odznaky (čeká, běží, hotovo, chyba) |
| `app.js` | Volání Fetch, line reader přes `ReadableStream`, a logika aktualizace DOM |

> **Tip:** Otevřete `index.html` přímo v prohlížeči pro náhled rozložení. Nic nebude fungovat bez backendu, ale uvidíte strukturu.

### Jak funguje stream reader

Klíčová funkce v `app.js` čte odpověď po kusech a dělí ji na řádky podle nových řádků:

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
    buffer = lines.pop(); // ponechat neúplný koncový řádek

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

Každá zpracovaná zpráva je předána do `handleMessage()`, která aktualizuje příslušný DOM prvek podle hodnoty `msg.type`.

---

## Cvičení

### Cvičení 1: Spuštění Python backendu s UI

Python (FastAPI) varianta už má streamingové API endpoint. Jedinou změnou je připojení složky `ui/` jako statických souborů.

**1.1** Přejděte do adresáře Python API a nainstalujte závislosti:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Spusťte server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Otevřete v prohlížeči `http://localhost:8000`. Měli byste vidět UI Zava Creative Writer s třemi textovými poli a tlačítkem "Generate Article".

**1.4** Klikněte na **Generate Article** s výchozími hodnotami. Sledujte, jak se odznaky stavu agentů mění z "Waiting" na "Running" až "Done", jakmile každý agent dokončí svou práci, a text článku se token po tokenu streamuje do výstupního panelu.

> **Řešení potíží:** Pokud stránka místo UI zobrazí JSON odpověď, ujistěte se, že běžíte aktualizovaný `main.py`, který připojuje statické soubory. Endpunkt `/api/article` stále funguje na původní cestě; statické soubory zajišťují UI na všech ostatních trasách.

**Jak to funguje:** Aktualizovaný `main.py` přidává na konec jeden řádek:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Tímto se obsluhují všechny soubory ze složky `zava-creative-writer-local/ui/` jako statické assety s `index.html` jako výchozím dokumentem. POST trasa `/api/article` je registrována před mountem statiky, takže má prioritu.

---

### Cvičení 2: Přidání webového serveru k JavaScript variantě

JavaScript varianta je aktuálně CLI aplikace (`main.mjs`). Nový soubor `server.mjs` obaluje stávající agentní moduly za HTTP server a slouží sdílené UI.

**2.1** Přejděte do adresáře JavaScript a nainstalujte závislosti:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Spusťte webový server:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Měli byste vidět:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Otevřete `http://localhost:3000` v prohlížeči a klikněte na **Generate Article**. Stejné UI funguje stejně i s JavaScript backendem.

**Prostudujte kód:** Otevřete `server.mjs` a všimněte si klíčových postupů:

- **Servování statických souborů** používá vestavěné moduly Node.js `http`, `fs` a `path` bez externích frameworků.
- **Ochrana proti průchodu cestou (path traversal)** normalizuje požadovanou cestu a ověřuje, že zůstává ve složce `ui/`.
- **Streaming NDJSON** používá pomocnou funkci `sendLine()`, která serializuje každý objekt, odstraní vnitřní nové řádky a přidá na konec nový řádek.
- **Orchestrace agentů** znovu používá existující moduly `researcher.mjs`, `product.mjs`, `writer.mjs` a `editor.mjs` beze změn.

<details>
<summary>Vybraný úryvek ze server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Výzkumník
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Spisovatel (streamování)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Cvičení 3: Přidání Minimal API k C# variantě

C# varianta je aktuálně konzolová aplikace. Nový projekt `csharp-web` používá ASP.NET Core minimal APIs, aby zpřístupnil stejný pipeline jako webovou službu.

**3.1** Přejděte do C# webového projektu a obnovte balíčky:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Spusťte webový server:

```bash
dotnet run
```

```powershell
dotnet run
```

Měli byste vidět:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Otevřete v prohlížeči `http://localhost:5000` a klikněte na **Generate Article**.

**Prostudujte kód:** Otevřete `Program.cs` v adresáři `csharp-web` a všimněte si:

- Projektový soubor používá `Microsoft.NET.Sdk.Web` místo `Microsoft.NET.Sdk`, což přidává podporu ASP.NET Core.
- Statické soubory jsou servírovány přes `UseDefaultFiles` a `UseStaticFiles` ukazující na sdílenou složku `ui/`.
- Endpunkt `/api/article` zapisuje NDJSON řádky přímo do `HttpContext.Response` a po každém řádku provádí flush pro streamování v reálném čase.
- Veškerá logika agentů (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) je stejná jako v konzolové verzi.

<details>
<summary>Vybraný úryvek z csharp-web/Program.cs</summary>

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

### Cvičení 4: Prozkoumejte odznaky stavu agentů

Nyní, když máte funkční UI, podívejte se, jak front end aktualizuje stavové odznaky.

**4.1** Otevřete `zava-creative-writer-local/ui/app.js` ve vašem editoru.

**4.2** Najděte funkci `handleMessage()`. Všimněte si, jak mapuje typy zpráv na aktualizace DOM:

| Typ zprávy | Akce v UI |
|-------------|-----------|
| `message` obsahující "researcher" | Nastaví odznak badatele na "Running" |
| `researcher` | Nastaví odznak badatele na "Done" a naplní panel Výsledky výzkumu |
| `marketing` | Nastaví odznak hledání produktu na "Done" a naplní panel Produktové shody |
| `writer` s `data.start` | Nastaví odznak pisatele na "Running" a vyčistí výstup článku |
| `partial` | Přidá tokenový text do výstupu článku |
| `writer` s `data.complete` | Nastaví odznak pisatele na "Done" |
| `editor` | Nastaví odznak editora na "Done" a naplní panel Editorova zpětná vazba |

**4.3** Otevřete skládací panely "Výsledky výzkumu", "Produktové shody" a "Editorova zpětná vazba" pod článkem a podívejte se na surový JSON, který každý agent vytvořil.

---

### Cvičení 5: Přizpůsobení UI (Rozšíření)

Vyzkoušejte jednu nebo více z těchto vylepšení:

**5.1 Přidat počítadlo slov.** Po dokončení pisatele zobrazte počet slov článku pod panelem výstupu. Můžete to spočítat v `handleMessage` když `type === "writer"` a `data.complete` je pravda:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Přidat indikátor opakování.** Když editor požaduje revizi, pipeline se znovu spustí. Zobrazte banner "Revision 1" nebo "Revision 2" v panelu stavu. Naslouchejte zprávám typu `message` obsahujícím "Revision" a aktualizujte nový DOM prvek.

**5.3 Tmavý režim.** Přidejte přepínač a třídu `.dark` na `<body>`. Přepište barvy pozadí, textu a panelů v `style.css` pomocí selektoru `body.dark`.

---

## Souhrn

| Co jste udělali | Jak |
|-------------|-----|
| Servírování UI z Python backendu | Připojením složky `ui/` pomocí `StaticFiles` ve FastAPI |
| Přidání HTTP serveru k JavaScript variantě | Vytvořením `server.mjs` pomocí vestavěného Node.js modulu `http` |
| Přidání webového API k C# variantě | Vytvořením nového projektu `csharp-web` s ASP.NET Core minimal APIs |
| Spotřebování streamingového NDJSON v prohlížeči | Použitím `fetch()` s `ReadableStream` a parsováním JSON po řádcích |
| Aktualizace UI v reálném čase | Mapováním typů zpráv na aktualizace DOM (odznaky, text, skládací panely) |

---

## Klíčové poznatky

1. **Sdílený statický front-end** funguje s jakýmkoliv backendem, který používá stejný streamingový protokol, potvrzující hodnotu OpenAI-kompatibilního API vzoru.
2. **JSON oddělený novými řádky (NDJSON)** je jednoduchý streamingový formát, který nativně funguje s browserovým `ReadableStream` API.
3. **Python varianta** potřebovala nejméně úprav, protože už měla endpoint ve FastAPI; JavaScript a C# varianty potřebovaly tenkou HTTP vrstvu.
4. Zachování UI jako **vanilkový HTML/CSS/JS** eliminuje potřebu build nástrojů, frameworkových závislostí a složitosti pro účastníky workshopu.
5. Stejné agentní moduly (Researcher, Product, Writer, Editor) jsou znovu použity beze změn; mění se pouze transportní vrstva.

---

## Další čtení

| Zdroj | Odkaz |
|----------|------|
| MDN: Použití Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Statické soubory | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Statické soubory | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Specifikace NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Pokračujte do [Části 13: Workshop dokončen](part13-workshop-complete.md) pro shrnutí všeho, co jste během workshopu vybudovali.

---
[← Část 11: Volání nástroje](part11-tool-calling.md) | [Část 13: Workshop dokončen →](part13-workshop-complete.md)