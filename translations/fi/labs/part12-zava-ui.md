![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 12: Verkkokäyttöliittymän rakentaminen Zava Creative Writerille

> **Tavoite:** Lisää selainpohjainen käyttöliittymä Zava Creative Writerille, jotta voit seurata monitoimittajaputken toimintaa reaaliajassa, sisältäen live-agenttien tilamerkit ja virtaavan artikkelin tekstin, kaikki yhdestä paikallisesta verkkopalvelimesta.

Osassa [Osa 7](part7-zava-creative-writer.md) tutustuit Zava Creative Writeriin **CLI-sovelluksena** (JavaScript, C#) ja **päänettömänä rajapintana** (Python). Tässä työpajassa liität jaetun **vanilla HTML/CSS/JavaScript** käyttöliittymän jokaiseen taustajärjestelmään, jotta käyttäjät voivat olla vuorovaikutuksessa putken kanssa selaimen kautta terminaalin sijaan.

---

## Mitä opit

| Tavoite | Kuvaus |
|-----------|-------------|
| Palvella staattisia tiedostoja taustajärjestelmästä | Liittää HTML/CSS/JS hakemisto API-reitin viereen |
| Kuluttaa virtaavaa NDJSON:ia selaimessa | Käyttää Fetch API:ta `ReadableStream`illä rivinvaihdolla eroteltujen JSON-tietojen lukemiseksi |
| Yhtenäinen virtausprotokolla | Varmistaa, että Python-, JavaScript- ja C#-taustajärjestelmät tuottavat saman viestimuodon |
| Jatkuvat käyttöliittymäpäivitykset | Päivittää agenttien tilamerkit ja virtaa artikkelin teksti merkki merkiltä |
| Lisää HTTP-kerros CLI-sovellukseen | Kääri olemassa oleva orkestrointilogiikka Express-tyyppisessä palvelimessa (JS) tai ASP.NET Core minimirajapinnassa (C#) |

---

## Arkkitehtuuri

Käyttöliittymä on yksi staattisten tiedostojen kokoelma (`index.html`, `style.css`, `app.js`), jota kaikki kolme taustajärjestelmää käyttävät. Jokainen taustajärjestelmä tarjoaa samat kaksi reittiä:

![Zava UI arkkitehtuuri — jaettu käyttöliittymä kolmella taustajärjestelmällä](../../../images/part12-architecture.svg)

| Reitti | Metodi | Tarkoitus |
|-------|--------|---------|
| `/` | GET | Palvelee staattisen käyttöliittymän |
| `/api/article` | POST | Suorittaa monitoimittajaputken ja virtaa NDJSON:ia |

Käyttöliittymä lähettää JSON-rungon ja lukee vastauksen rivinvaihdolla eroteltujen JSON-viestien virtana. Jokaisessa viestissä on `type`-kenttä, jota käyttöliittymä käyttää oikean paneelin päivittämiseen:

| Viestin tyyppi | Merkitys |
|-------------|---------|
| `message` | Tilapäivitys (esim. "Käynnistetään tutkijan agentin tehtävä...") |
| `researcher` | Tutkimustulokset ovat valmiit |
| `marketing` | Tuotehaku tulokset ovat valmiit |
| `writer` | Kirjoittaja aloitti tai päätti (sisältää `{ start: true }` tai `{ complete: true }`) |
| `partial` | Yksi virtaava merkki Kirjoittajalta (sisältää `{ text: "..." }`) |
| `editor` | Toimittajan päätös on valmis |
| `error` | Jokin meni pieleen |

![Viestityyppien reititys selaimessa](../../../images/part12-message-types.svg)

![Virtaussekvenssi — selain ja taustajärjestelmä kommunikaatio](../../../images/part12-streaming-sequence.svg)

---

## Vaatimukset

- Valmis [Osa 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI asennettuna ja `phi-3.5-mini` malli ladattuna
- Moderni verkkoselain (Chrome, Edge, Firefox tai Safari)

---

## Jaettu käyttöliittymä

Ennen kuin kosket mihinkään taustajärjestelmän koodiin, tutustu hetki käyttöliittymään, jota kaikki kolme kieliraitaa käyttävät. Tiedostot sijaitsevat kansiossa `zava-creative-writer-local/ui/`:

| Tiedosto | Tarkoitus |
|------|---------|
| `index.html` | Sivun asettelu: syöttölomake, agenttien tilamerkit, artikkelin näyttöalue, laajennettavat yksityiskohtapaneelit |
| `style.css` | Minimityyli tilamerkkien väristateille (odottaa, käynnissä, valmis, virhe) |
| `app.js` | Fetch-kutsu, `ReadableStream` rivinlukija ja DOM-päivityslogiikka |

> **Vinkki:** Avaa `index.html` suoraan selaimessasi esikatsoaksesi asettelun. Mikään ei vielä toimi, koska ei ole taustajärjestelmää, mutta rakenteen näet.

### Kuinka virtalukija toimii

Keskeinen funktio `app.js`:ssa lukee vastauksen runkona palanen kerrallaan ja jakaa rivinvaihdoissa:

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
    buffer = lines.pop(); // säilytä keskeneräinen loppurivi

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

Jokainen purettu viesti ohjataan `handleMessage()` -funktiolle, joka päivittää olennaisen DOM-elementin `msg.type` mukaan.

---

## Harjoitukset

### Harjoitus 1: Suorita Python-taustajärjestelmä käyttöliittymällä

Python (FastAPI) versiossa on jo virtaava API-päätepiste. Ainoa muutos on `ui/` kansion liittäminen staattiseksi tiedostoiksi.

**1.1** Siirry Python API -hakemistoon ja asenna riippuvuudet:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Käynnistä palvelin:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Avaa selain osoitteessa `http://localhost:8000`. Näet Zava Creative Writer -käyttöliittymän, jossa on kolme tekstikenttää ja "Generate Article" -painike.

**1.4** Klikkaa **Generate Article** käyttäen oletusarvoja. Katso agenttien tilamerkkien vaihtuvan "Waitingista" "Runningiksi" ja "Doneksi" kun kukin agentti suorittaa työnsä, ja artikkelin teksti suoratoistaa ulostulopaneeliin token tokenilta.

> **Vianmääritys:** Jos sivulla näkyy JSON-vastaus käyttöliittymän sijaan, varmista, että ajat päivitettyä `main.py`-tiedostoa, joka liittää staattiset tiedostot. `/api/article` päätepiste toimii yhä alkuperäisellä polullaan; staattinen tiedostopolku palvelee käyttöliittymää kaikissa muissa reiteissä.

**Miten se toimii:** Päivitetty `main.py` lisää rivin loppuun:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Tämä palvelee kaikki tiedostot hakemistosta `zava-creative-writer-local/ui/` staattisina tiedostoina oletusdokumenttina `index.html`. `/api/article` POST-reitti on rekisteröity ennen staattista liittämistä, joten se priorisoituu.

---

### Harjoitus 2: Lisää verkkopalvelin JavaScript-versioon

JavaScript-versio on nykyisin CLI-sovellus (`main.mjs`). Uusi tiedosto `server.mjs` käärii samat agenttimoduulit HTTP-palvelimen taakse ja palvelee jaettua käyttöliittymää.

**2.1** Siirry JavaScript-kansioon ja asenna riippuvuudet:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Käynnistä verkkopalvelin:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Näet tuloksen:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Avaa selain osoitteessa `http://localhost:3000` ja napsauta **Generate Article**. Sama käyttöliittymä toimii identtisesti JavaScript-taustajärjestelmän kanssa.

**Tutki koodia:** Avaa `server.mjs` ja huomaa keskeiset mallit:

- **Staattisten tiedostojen palvelu** käyttää Node.js:n sisäänrakennettuja `http`, `fs` ja `path` moduuleja ilman ulkoisia kirjastoja.
- **Polunsuojaus** normalisoi pyydetyn polun ja varmistaa, että se pysyy `ui/` hakemiston sisällä.
- **NDJSON-virtaus** käyttää `sendLine()` apufunktiota, joka sarjoittaa jokaisen objektin, poistaa sisäiset rivinvaihdot ja lisää loppurivinvaihdon.
- **Agenttien orkestrointi** käyttää uudelleen olemassa olevia moduuleja `researcher.mjs`, `product.mjs`, `writer.mjs` ja `editor.mjs` muuttamatta niitä.

<details>
<summary>Keskeinen ote tiedostosta server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Tutkija
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Kirjoittaja (suoratoisto)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Harjoitus 3: Lisää minimirajapinta C#-versioon

C#-versio on nyt konsolisovellus. Uusi projekti `csharp-web` käyttää ASP.NET Core minimirajapintoja tarjoamaan saman putken verkkopalveluna.

**3.1** Siirry C# web-projektiin ja palauta paketit:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Käynnistä verkkopalvelin:

```bash
dotnet run
```

```powershell
dotnet run
```

Näet tuloksen:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Avaa selaimessa osoite `http://localhost:5000` ja napsauta **Generate Article**.

**Tutki koodia:** Avaa `Program.cs` kansiossa `csharp-web` ja huomaa:

- Projektitiedosto käyttää `Microsoft.NET.Sdk.Web`-mallia `Microsoft.NET.Sdk`:n sijaan lisäten ASP.NET Core tuen.
- Staattiset tiedostot palvellaan `UseDefaultFiles` ja `UseStaticFiles` avulla jaettuun `ui/` hakemistoon osoittaen.
- `/api/article` päätepiste kirjoittaa NDJSON-rivit suoraan `HttpContext.Response`:iin ja tyhjentää ne rivin jälkeen reaaliaikaista virtausta varten.
- Kaikki agenttilogiikka (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) on sama kuin konsoliversiossa.

<details>
<summary>Keskeinen ote tiedostosta csharp-web/Program.cs</summary>

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

### Harjoitus 4: Tutki agenttien tilamerkkejä

Nyt kun käyttöliittymä on toiminnassa, katso kuinka käyttöliittymä päivittää tilamerkkejä.

**4.1** Avaa tiedosto `zava-creative-writer-local/ui/app.js` muokkaimessasi.

**4.2** Etsi `handleMessage()`-funktio. Huomaa, miten se liittää viestityypit DOM-päivityksiin:

| Viestin tyyppi | Käyttöliittymätoiminto |
|-------------|-----------|
| `message`, jossa on "researcher" | Asettaa Researcher-merkin tilaksi "Running" |
| `researcher` | Asettaa Researcher-merkin "Done" ja täyttää Tutkimustulokset-paneelin |
| `marketing` | Asettaa Tuotehaun merkin "Done" ja täyttää Tuotteen osumat -paneelin |
| `writer`, jossa `data.start` | Asettaa Writer-merkin "Running" ja tyhjentää artikkeliesityksen |
| `partial` | Lisää token-tekstin artikkeliesitykseen |
| `writer`, jossa `data.complete` | Asettaa Writer-merkin "Done" |
| `editor` | Asettaa Editor-merkin "Done" ja täyttää Toimittajan palaute -paneelin |

**4.3** Avaa laajennettavat paneelit "Research Results", "Product Matches" ja "Editor Feedback" artikkelin alla tarkastellaksesi kunkin agentin tuottaman raakamuotoisen JSON:n.

---

### Harjoitus 5: Muokkaa käyttöliittymää (Lisähaaste)

Kokeile yhtä tai useampaa näistä parannuksista:

**5.1 Lisää sanamäärälaskuri.** Kun Writer on valmis, näytä artikkelin sanamäärä ulostulopaneelin alapuolella. Voit laskea tämän `handleMessage`:ssa kun `type === "writer"` ja `data.complete` on tosi:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Lisää uudelleenyrityssignaali.** Kun Editor pyytää muokkausta, putki suoritetaan uudelleen. Näytä "Revision 1" tai "Revision 2" banneri tilapaneelissa. Kuuntele `message`-tyyppiä, joka sisältää "Revision" ja päivitä uusi DOM-elementti.

**5.3 Tumma tila.** Lisää vaihtopainike ja `.dark`-luokka `<body>`-elementtiin. Ylikirjoita tausta-, teksti- ja paneelivärit `style.css` tiedoston `body.dark`-valitsimella.

---

## Yhteenveto

| Mitä teit | Kuinka |
|-------------|-----|
| Palvelit käyttöliittymän Python-taustajärjestelmästä | Liitit `ui/` kansion `StaticFiles`-palvelimella FastAPI:ssa |
| Lisäsit HTTP-palvelimen JavaScript-varianttiin | Loit `server.mjs` käyttäen Node.js:n sisäänrakennettua `http` modulia |
| Lisäsit verkkorajapinnan C#-versioon | Loit uuden `csharp-web` projektin ASP.NET Core minimirajapinnoilla |
| Kulutit virtaavaa NDJSON:ia selaimessa | Käytit `fetch()` ja `ReadableStream`-rajapintaa rivikohtaiseen JSON-jäsennykseen |
| Päivitit käyttöliittymää reaaliajassa | Yhdistit viestityypit DOM-päivityksiin (merkit, teksti, laajennettavat paneelit) |

---

## Tärkeimmät opit

1. **Jaettu staattinen käyttöliittymä** voi toimia minkä tahansa taustajärjestelmän kanssa, joka käyttää samaa virtausprotokollaa, vahvistaen OpenAI-yhteensopivan API-kuvion hyödyt.
2. **Rivinvaihdolla eroteltu JSON (NDJSON)** on yksinkertainen virtausmuoto, joka toimii natiivisti selaimen `ReadableStream` API:n kanssa.
3. **Python-varianttia** tarvitsi muuttaa vähiten, koska siinä oli jo FastAPI-päätepiste; JavaScript- ja C#-versiot tarvitsivat ohuehkot HTTP-kääröt.
4. UI pidettiin **vanilla HTML/CSS/JS:nä** ilman rakennustyökaluja ja kirjastoja, jotta työpajan oppijat keskittyvät aiheeseen.
5. Sama agenttimoduulit (Researcher, Product, Writer, Editor) käytetään uudelleen muokkaamatta; muuttuu vain kuljetuskerros.

---

## Lisälukemista

| Resurssi | Linkki |
|----------|------|
| MDN: Readable Streamsin käyttö | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI staattiset tiedostot | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core staattiset tiedostot | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Spesifikaatio | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Jatka [Osa 13: Työpajan yhteenveto](part13-workshop-complete.md) yhteenvetoon kaikesta, mitä olet rakentanut tässä työpajassa.

---
[← Osa 11: Työkalun kutsuminen](part11-tool-calling.md) | [Osa 13: Työpaja valmis →](part13-workshop-complete.md)