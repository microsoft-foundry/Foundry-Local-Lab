![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 12 dalis: Svetainės sąsajos kūrimas Zava kūrybiniam rašytojui

> **Tikslas:** Pridėti naršyklėje veikiantį priekinį sluoksnį Zava kūrybiniam rašytojui, kad galėtumėte tiesiogiai stebėti kelių agentų srautą realiu laiku, su gyvais agentų būsenos ženklais ir transliuojamu straipsnio tekstu, visa tai aptarnaujama iš vieno vietinio interneto serverio.

7 dalyje [Part 7](part7-zava-creative-writer.md) tyrinėjote Zava kūrybinį rašytoją kaip **CLI programą** (JavaScript, C#) ir kaip **be galvos API** (Python). Šiame užsiėmime prijungsite bendrą **vanilinio HTML/CSS/JavaScript** priekinį sluoksnį prie kiekvieno galinio sluoksnio, kad vartotojai galėtų bendrauti su srautu naršyklės pagalba, vietoj terminalo.

---

## Ko išmoksite

| Tikslas | Aprašymas |
|-----------|-------------|
| Aptarnauti statinius failus iš galinio sluoksnio | Prijungti HTML/CSS/JS katalogą kartu su API keliais |
| Vartoti transliuojamą NDJSON naršyklėje | Naudoti Fetch API su `ReadableStream`, kad skaityti naujų linių atskirtą JSON |
| Vieningas transliavimo protokolas | Užtikrinti, kad Python, JavaScript ir C# galiniai sluoksniai siunčia tą patį žinutės formatą |
| Progresyvus UI atnaujinimas | Atnaujinti agentų būsenos ženklius ir transliuoti straipsnio tekstą žodis po žodžio |
| Pridėti HTTP sluoksnį CLI programai | Supakuoti esamą koordinavimo logiką į Express tipo serverį (JS) arba ASP.NET Core minimalų API (C#) |

---

## Architektūra

UI yra vienas rinkinys statinių failų (`index.html`, `style.css`, `app.js`), kuris dalijamas visų trijų galinių sluoksnių. Kiekvienas galinis sluoksnis atskleidžia tuos pačius du maršrutus:

![Zava UI architektūra — bendras priekinis sluoksnis su trimis galiniais](../../../images/part12-architecture.svg)

| Maršrutas | Metodas | Paskirtis |
|-------|--------|---------|
| `/` | GET | Aptarnauja statinį UI |
| `/api/article` | POST | Vykdo kelių agentų srautą ir transliuoja NDJSON |

Priekinis sluoksnis siunčia JSON kėbulą ir skaito atsakymą kaip naujų linių atskirtų JSON žinučių srautą. Kiekviena žinutė turi `type` lauką, kurį UI naudoja atnaujinant tinkamą skiltį:

| Žinutės tipas | Reikšmė |
|-------------|---------|
| `message` | Būsenos atnaujinimas (pvz., "Pradedama tyrėjo agento užduotis...") |
| `researcher` | Tyrimo rezultatai paruošti |
| `marketing` | Produkto paieškos rezultatai paruošti |
| `writer` | Rašytojo paleidimas arba užbaigimas (turi `{ start: true }` arba `{ complete: true }`) |
| `partial` | Vienas transliuojamas rašytojo žodis (turi `{ text: "..." }`) |
| `editor` | Redaktoriaus sprendimas paruoštas |
| `error` | Įvyko klaida |

![Žinučių tipų maršrutizavimas naršyklėje](../../../images/part12-message-types.svg)

![Transliavimo seka — Naršyklė į Galinį sluoksnį komunikacija](../../../images/part12-streaming-sequence.svg)

---

## Priešprievoriai

- Užbaikite [7 dalį: Zava kūrybinis rašytojas](part7-zava-creative-writer.md)
- Įdiekite Foundry Local CLI ir atsisiųskite `phi-3.5-mini` modelį
- Naudokite modernią naršyklę (Chrome, Edge, Firefox arba Safari)

---

## Bendras UI

Prieš pradėdami keisti galinio sluoksnio kodą, skirkite laiko apžvelgti priekinį sluoksnį, kurį naudos visos trys kalbų šakos. Failai yra `zava-creative-writer-local/ui/` kataloge:

| Failas | Paskirtis |
|------|---------|
| `index.html` | Puslapio išdėstymas: įvesties forma, agentų būsenos ženkliukai, straipsnio išvesties sritis, išplečiamos detalių sritys |
| `style.css` | Minimalus stilius su būsenos ženkliukų spalvų būsenomis (laukiama, veikia, baigta, klaida) |
| `app.js` | Fetch kvietimas, `ReadableStream` eilutės skaitytuvas ir DOM atnaujinimo logika |

> **Patarimas:** Atidarykite `index.html` tiesiogiai savo naršyklėje, kad peržiūrėtume išdėstymą. Dar niekas neveiks, nes nėra galinio sluoksnio, bet matysite struktūrą.

### Kaip veikia srauto skaitytuvas

Svarbiausia funkcija `app.js` skaito atsakymo turinį gabalais ir dalija pagal naujų linių rėžius:

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
    buffer = lines.pop(); // palikti nebaigtą pabaigos eilutę

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

Kiekviena išanalizuota žinutė siunčiama į `handleMessage()`, kuri atnaujina atitinkamą DOM elementą pagal `msg.type`.

---

## Pratimai

### 1 pratimas: Paleisti Python galinį sluoksnį su UI

Python (FastAPI) variantas jau turi transliuojamą API galinį tašką. Vienintelis pakeitimas – prijungti `ui/` katalogą kaip statinius failus.

**1.1** Eikite į Python API katalogą ir įdiekite priklausomybes:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Paleiskite serverį:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Naršyklėje atidarykite `http://localhost:8000`. Turėtumėte matyti Zava kūrybinio rašytojo UI su trim įvesties laukeliais ir mygtuku "Generate Article".

**1.4** Spustelėkite **Generate Article** naudodami numatytas reikšmes. Stebėkite, kaip agentų būsenos ženkliukai keičiasi iš "Waiting" į "Running", po to į "Done", kai kiekvienas agentas paleidžiamas ir baigiamas, taip pat matykite kaip straipsnio tekstas transliuojamas po žodį į išvesties lauką.

> **Trikčių šalinimas:** Jei puslapyje rodoma JSON atsakymas vietoje UI, įsitikinkite, kad paleidžiate atnaujintą `main.py`, kuris prijungia statinius failus. `/api/article` galinis taškas veikia savo originaliu keliu; statinis failų prijungimas aptarnauja UI visuose kituose maršrutuose.

**Kaip veikia:** Atnaujintas `main.py` prideda vieną eilutę pabaigoje:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Tai aptarnauja visus failus iš `zava-creative-writer-local/ui/` kaip statinius, o numatytasis dokumentas – `index.html`. `/api/article` POST maršrutas yra registruotas prieš statinio prijungimą, todėl turi aukštesnį prioritetą.

---

### 2 pratimas: Pridėti interneto serverį JavaScript variantui

JavaScript variantas šiuo metu yra CLI programa (`main.mjs`). Naujas failas `server.mjs` apgaubia tuos pačius agentų modulius HTTP serveriu ir aptarnauja bendrą UI.

**2.1** Eikite į JavaScript katalogą ir įdiekite priklausomybes:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Paleiskite interneto serverį:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Turėtumėte matyti:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Naršyklėje atidarykite `http://localhost:3000` ir spustelėkite **Generate Article**. Tas pats UI veikia taip pat prieš JavaScript galinį sluoksnį.

**Išnagrinėkite kodą:** Atidarykite `server.mjs` ir atkreipkite dėmesį į pagrindinius modelius:

- **Statinių failų aptarnavimas** naudoja Node.js įmontuotus `http`, `fs` ir `path` modulius be jokių išorinių sistemų.
- **Kelio pereinamo apsauga** normalizuoja prašomą kelią ir patikrina, ar jis lieka `ui/` kataloge.
- **NDJSON transliavimas** naudoja pagalbinę funkciją `sendLine()`, kuri serializuoja kiekvieną objektą, pašalina vidinius naujus linijos simbolius ir prideda pabaigiančią naują eilutę.
- **Agentų koordinavimas** panaudoja esamus `researcher.mjs`, `product.mjs`, `writer.mjs` ir `editor.mjs` modulius be pakeitimų.

<details>
<summary>Pagrindinė ištrauka iš server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Tyrėjas
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Rašytojas (transliavimas)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### 3 pratimas: Pridėti minimalų API C# variantui

C# variantas dabar yra konsolės programa. Naujas projektas `csharp-web` naudoja ASP.NET Core minimalų API, kad tą patį srautą atskleistų kaip interneto paslaugą.

**3.1** Eikite į C# interneto projekto katalogą ir atkurkite paketus:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Paleiskite interneto serverį:

```bash
dotnet run
```

```powershell
dotnet run
```

Turėtumėte matyti:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Naršyklėje atidarykite `http://localhost:5000` ir spustelėkite **Generate Article**.

**Išnagrinėkite kodą:** Atidarykite `Program.cs` `csharp-web` kataloge ir atkreipkite dėmesį:

- Projekto failas naudoja `Microsoft.NET.Sdk.Web` vietoje `Microsoft.NET.Sdk`, tai pridėjo ASP.NET Core palaikymą.
- Statiniai failai aptarnaujami per `UseDefaultFiles` ir `UseStaticFiles`, nukreiptus į bendrą `ui/` katalogą.
- `/api/article` galinis taškas rašo NDJSON eilutes tiesiai į `HttpContext.Response` ir išsipučia po kiekvienos eilutės realiuoju laiku transliavimui.
- Visa agentų logika (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) tokia pati kaip konsolės versijoje.

<details>
<summary>Pagrindinė ištrauka iš csharp-web/Program.cs</summary>

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

### 4 pratimas: Tyrinėkite agentų būsenos ženkliukus

Dabar, kai UI veikia, pažvelkite, kaip priekinis sluoksnis atnaujina būsenos ženkliukus.

**4.1** Atidarykite `zava-creative-writer-local/ui/app.js` savo redaktoriuje.

**4.2** Suraskite funkciją `handleMessage()`. Atkreipkite dėmesį, kaip ji susieja žinučių tipus su DOM atnaujinimais:

| Žinutės tipas | UI veiksmas |
|-------------|-----------|
| `message` su tekstu "researcher" | Nustato tyrėjo ženkliuką į "Running" |
| `researcher` | Nustato tyrėjo ženkliuką į "Done" ir užpildo "Research Results" skiltį |
| `marketing` | Nustato produkto paieškos ženkliuką į "Done" ir užpildo "Product Matches" skiltį |
| `writer` su `data.start` | Nustato rašytojo ženkliuką į "Running" ir valo straipsnio išvestį |
| `partial` | Papildo straipsnio išvestį su žodžio tekstu |
| `writer` su `data.complete` | Nustato rašytojo ženkliuką į "Done" |
| `editor` | Nustato redaktoriaus ženkliuką į "Done" ir užpildo "Editor Feedback" skiltį |

**4.3** Atidarykite išplečiamas „Research Results“, „Product Matches“ ir „Editor Feedback“ skiltis po straipsniu, kad patikrintumėte žaliavinius JSON, kuriuos sugeneravo kiekvienas agentas.

---

### 5 pratimas: Pritaikykite UI (prailginimas)

Išbandykite vieną ar kelis iš šių patobulinimų:

**5.1 Pridėkite žodžių skaičių.** Po to, kai rašytojas baigia darbą, parodykite straipsnio žodžių skaičių po išvesties skiltimi. Galite tai apskaičiuoti `handleMessage` funkcijoje, kai `type === "writer"` ir `data.complete` yra tiesa:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Pridėkite pakartojimo indikatorius.** Kai redaktorius prašo pataisymų, srautas paleidžiamas iš naujo. Rodykite „Revision 1“ arba „Revision 2“ banerį būsenos skiltyje. Nuodėkite `message` tipą, kuriame yra žodis „Revision“, ir atnaujinkite naują DOM elementą.

**5.3 Tamsus režimas.** Pridėkite perjungimo mygtuką ir `.dark` klasę `<body>`. Pakeiskite foną, tekstą ir skilties spalvas `style.css` naudodami `body.dark` selektorių.

---

## Santrauka

| Ką padarėte | Kaip |
|-------------|-----|
| Aptarnavote UI iš Python galinio sluoksnio | Prijungėte `ui/` katalogą su `StaticFiles` FastAPI |
| Pridėjote HTTP serverį JavaScript variantui | Sukūrėte `server.mjs`, naudodami Node.js įmontuotą `http` modulį |
| Pridėjote interneto API C# variantui | Sukūrėte naują `csharp-web` projektą su ASP.NET Core minimaliu API |
| Vartojote transliuojamą NDJSON naršyklėje | Naudojote `fetch()` su `ReadableStream` ir po eilutę JSON analizę |
| Atnaujinote UI realiu laiku | Susiejote žinučių tipus su DOM atnaujinimais (ženkliukai, tekstas, išplečiamos skiltys) |

---

## Pagrindinės išvados

1. **Bendras statinis priekinis sluoksnis** gali veikti su bet kuriuo galiniu sluoksniu, kuris naudoja tą patį transliavimo protokolą, kas pabrėžia OpenAI suderinamo API vertę.
2. **Naujos linijos atskirtas JSON (NDJSON)** yra paprastas transliavimo formatas, natūraliai palaikomas naršyklės `ReadableStream` API.
3. **Python variantui** reikėjo mažiausiai pakeitimų, nes jis jau turėjo FastAPI galinį tašką; JavaScript ir C# variantai turėjo pridėti ploną HTTP apvalkalą.
4. Laikant UI kaip **vanilinį HTML/CSS/JS** išvengiama statybos įrankių, sistemų priklausomybių ir papildomos sudėtingumo mokymosi procese.
5. Tie patys agentų moduliai (Researcher, Product, Writer, Editor) yra naudojami be modifikacijų; keičiasi tik perdavimo sluoksnis.

---

## Tolimesnė literatūra

| Šaltinis | Nuoroda |
|----------|------|
| MDN: Naudojant Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI statiniai failai | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core statiniai failai | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON specifikacija | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Tęskite prie [13 dalies: Užsiėmimas baigtas](part13-workshop-complete.md), kur rasite surašytą viską, ką sukūrėte šio užsiėmimo metu.

---
[← 11 dalis: Įrankio kvietimas](part11-tool-calling.md) | [13 dalis: Dirbtuvės baigtos →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, prašome atkreipti dėmesį, kad automatiniai vertimai gali turėti klaidų arba netikslumų. Originalus dokumentas gimtąja kalba turi būti laikomas autoritetingu šaltiniu. Esant kritinei informacijai rekomenduojamas profesionalus žmogaus vertimas. Mes neprisiimame atsakomybės už bet kokius nesusipratimus ar neteisingus interpretavimus, kilusius naudojantis šiuo vertimu.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->