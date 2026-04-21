![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Sehemu ya 12: Kujenga UI ya Wavuti kwa Zava Creative Writer

> **Lengo:** Ongeza mbele-mwisho ya kivinjari kwa Zava Creative Writer ili uweze kuangalia mchakato wa wakala wengi ukiendeshwa kwa wakati halisi, ukiwa na alama za hali za wakala zinazoonyesha hali yao na maandishi ya makala yanayotiririka, yote yakihudumiwa kutoka kwa seva moja ya wavuti ya ndani.

Katika [Sehemu ya 7](part7-zava-creative-writer.md) ulichunguza Zava Creative Writer kama **programu ya CLI** (JavaScript, C#) na **API isiyo na kichwa** (Python). Katika maabara hii utaunganisha mbele-mwisho wa pamoja wa **vanilla HTML/CSS/JavaScript** kwa kila nyuma ili watumiaji waweze kuingiliana na mchakato kupitia kivinjari badala ya terminal.

---

## Kitu Utakachojifunza

| Lengo | Maelezo |
|-----------|-------------|
| Hudumia faili za static kutoka kwenye backend | Jenga saraka ya HTML/CSS/JS kando ya njia ya API |
| Tumia NDJSON ya kupeperusha matangazo katika kivinjari | Tumia Fetch API na `ReadableStream` kusoma JSON zilizo na alama ya newline |
| Itifaki moja ya kupeperusha matangazo | Hakikisha Python, JavaScript, na C# backend zinatuma aina moja ya ujumbe |
| Sasisho la UI hatua kwa hatua | Sasisha alama za hali ya wakala na punguza maandishi ya makala token kwa token |
| Ongeza safu ya HTTP kwa app ya CLI | Fungia mantiki ya muendesha aliyopo kwenye seva ya mtindo Express (JS) au API ya ASP.NET Core minimal (C#) |

---

## Usanifu

UI ni seti moja ya faili za static (`index.html`, `style.css`, `app.js`) zinashirikiwa na backends tatu. Kila backend inaonyesha njia mbili sawa:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| Njia | Njia ya Maombi | Kusudi |
|-------|--------|---------|
| `/` | GET | Hudumia UI ya static |
| `/api/article` | POST | Inaendesha mchakato wa wakala wengi na kupeperusha NDJSON |

Mbele-mwisho hutuma mwili wa JSON na husoma majibu kama mfululizo wa ujumbe wa JSON ulio wazi kwa newline. Kila ujumbe una uwanja wa `type` ambao UI hutumia kusasisha paneli sahihi:

| Aina ya Ujumbe | Maana |
|-------------|---------|
| `message` | Sasisho la hali (mfano "Kuanza kazi ya wakala mtafiti...") |
| `researcher` | Matokeo ya utafiti yamekwisha tayari |
| `marketing` | Matokeo ya utafutaji wa bidhaa yamekwisha tayari |
| `writer` | Mwandishi alianza au amemaliza (ina `{ start: true }` au `{ complete: true }`) |
| `partial` | Token moja lililotiririka kutoka kwa Mwandishi (ina `{ text: "..." }`) |
| `editor` | Uamuzi wa mhariri umekamilika |
| `error` | Kitu kimekosea |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## Masharti ya Kuanza

- Kamilisha [Sehemu ya 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI imewekwa na mfano `phi-3.5-mini` umepakuliwa
- Kivinjari cha wavuti cha kisasa (Chrome, Edge, Firefox, au Safari)

---

## UI inayoshirikiwa

Kabla ya kugusa msimbo wowote wa nyuma, chukua muda kuchunguza mbele-mwisho ambao miti yote ya lugha tatu itaitumia. Faili zipo katika `zava-creative-writer-local/ui/`:

| Faili | Kusudi |
|------|---------|
| `index.html` | Mpangilio wa ukurasa: fomu ya kuingiza, alama za hali za wakala, eneo la matokeo ya makala, paneli za maelezo zinazoweza kufichwa |
| `style.css` | Mtindo mdogo wa rangi za alama (kusubiri, kuendesha, kumaliza, kosa) |
| `app.js` | Mwito wa Fetch, msomaji wa mistari ya `ReadableStream`, na mantiki ya kusasisha DOM |

> **Dokezo:** Fungua `index.html` moja kwa moja kwenye kivinjari chako kuona mpangilio. Hakutakuwa na kazi bado kwa sababu hakuna backend, lakini utaona muundo.

### Jinsi Msomaji wa Mtiririko Unavyofanya Kazi

Kazi kuu katika `app.js` husoma mwili wa jibu kipande kipande na kugawanya kwa mipaka ya newline:

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
    buffer = lines.pop(); // hifadhi mstari wa mwisho usio kamili

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

Kila ujumbe uliosomwa hutumwa kwa `handleMessage()`, ambayo husasisha kipengele muhimu cha DOM kulingana na `msg.type`.

---

## Mafunzo

### Mafunzo 1: Endesha Backend ya Python na UI

Tofauti ya Python (FastAPI) tayari ina sehemu ya API ya kupeperusha matangazo. Mabadiliko pekee ni kuweka saraka ya `ui/` kama faili za static.

**1.1** Nenda kwenye saraka ya API ya Python na weka tegemezi:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Anza seva:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Fungua kivinjari chako kwenye `http://localhost:8000`. Unapaswa kuona UI ya Zava Creative Writer yenye sehemu tatu za maandishi na kitufe cha "Generate Article".

**1.4** Bonyeza **Generate Article** ukitumia maadili ya default. Tazama alama za hali za wakala zikibadilika toka "Waiting" kwenda "Running" kisha "Done" wakati kila wakala anamaliza kazi yake, na uone maandishi ya makala yakitiririka token kwa token kwenye paneli ya matokeo.

> **Kurekebisha matatizo:** Ikiwa ukurasa unaonyesha jibu la JSON badala ya UI, hakikisha unatumia toleo lililosasishwa la `main.py` ambalo linahudumia faili za static. Njia ya `/api/article` bado inafanya kazi kwenye njia yake ya awali; kufanyika kwa faili za static kunahudumia UI katika njia nyingine zote.

**Jinsi inavyofanya kazi:** `main.py` iliyosasishwa inaongeza mstari mmoja chini:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Hii huhudumia kila faili kutoka `zava-creative-writer-local/ui/` kama mali ya static, na `index.html` kama hati ya default. Njia ya POST ya `/api/article` imesajiliwa kabla ya kuingizwa kwa faili za static, hivyo inachukua kipaumbele.

---

### Mafunzo 2: Ongeza Seva ya Wavuti kwa Tofauti ya JavaScript

Tofauti ya JavaScript kwa sasa ni programu ya CLI (`main.mjs`). Faili mpya, `server.mjs`, inafunga moduli za wakala sawa nyuma ya seva ya HTTP na huhudumia UI inayoshirikiwa.

**2.1** Nenda kwenye saraka ya JavaScript na weka tegemezi:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Anza seva ya wavuti:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Unapaswa kuona:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Fungua `http://localhost:3000` kwenye kivinjari chako na bonyeza **Generate Article**. UI ile ile inafanya kazi sawasawa dhidi ya backend ya JavaScript.

**Chunguza msimbo:** Fungua `server.mjs` na angalia mifumo kuu:

- **Hudumia faili za static** hutumia moduli built-in za Node.js `http`, `fs`, na `path` bila hitaji la fremu nyingine.
- **Ulinzi wa path-traversal** huweka njia uliyoomba kwenye hali ya kawaida na kuthibitisha kubaki ndani ya saraka ya `ui/`.
- **Kupeperusha NDJSON** hutumia msaidizi `sendLine()` anayesafirisha kila kipengee, kuondoa newline za ndani, na kuongeza newline mwishoni.
- **Uratibu wa wakala** unatilia ndani moduli zilizopo `researcher.mjs`, `product.mjs`, `writer.mjs`, na `editor.mjs` bila mabadiliko.

<details>
<summary>Dondoo kuu kutoka server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Mtafiti
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Mwandishi (kutiririsha)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Mafunzo 3: Ongeza API Ndogo kwa Tofauti ya C#

Tofauti ya C# kwa sasa ni programu ya console. Mradi mpya, `csharp-web`, hutumia API za ASP.NET Core minimal kuonyesha mchakato ule ule kama huduma ya wavuti.

**3.1** Nenda kwenye mradi wa wavuti wa C# na rejesha vifurushi:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Endesha seva ya wavuti:

```bash
dotnet run
```

```powershell
dotnet run
```

Unapaswa kuona:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Fungua `http://localhost:5000` kwenye kivinjari chako na bonyeza **Generate Article**.

**Chunguza msimbo:** Fungua `Program.cs` katika saraka ya `csharp-web` na angalia:

- Faili la mradi linatumia `Microsoft.NET.Sdk.Web` badala ya `Microsoft.NET.Sdk`, ambalo linaongeza msaada wa ASP.NET Core.
- Faili za static huhudumiwa kupitia `UseDefaultFiles` na `UseStaticFiles` zinazolenga saraka ya pamoja `ui/`.
- Njia ya `/api/article` huandika mistari ya NDJSON moja kwa moja kwenye `HttpContext.Response` na kufuta baada ya kila mstari kwa ajili ya kupeperusha wakati halisi.
- Mantiki yote ya wakala (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) ni ile ile kama toleo la console.

<details>
<summary>Dondoo kuu kutoka csharp-web/Program.cs</summary>

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

### Mafunzo 4: Chunguza Alama za Hali za Wakala

Sasa baada ya kuwa na UI inayofanya kazi, angalia jinsi mbele-mwisho husasisha alama za hali.

**4.1** Fungua `zava-creative-writer-local/ui/app.js` mhariri wako.

**4.2** Tafuta kazi `handleMessage()`. Angalia jinsi inavyotumia aina za ujumbe kusanifu sasisho kwenye DOM:

| Aina ya Ujumbe | Hatua ya UI |
|-------------|-----------|
| `message` yenye "researcher" | Inamweka alama ya Mtafiti kuwa "Running" |
| `researcher` | Inamweka alama ya Mtafiti kuwa "Done" na kujaza paneli ya Matokeo ya Utafiti |
| `marketing` | Inamweka alama ya Utafutaji Bidhaa kuwa "Done" na kujaza paneli ya Mechi za Bidhaa |
| `writer` na `data.start` | Inamweka alama ya Mwandishi kuwa "Running" na kuondoa maandishi ya makala |
| `partial` | Inaongeza token ya maandishi kwenye eneo la matokeo ya makala |
| `writer` na `data.complete` | Inamweka alama ya Mwandishi kuwa "Done" |
| `editor` | Inamweka alama ya Mhariri kuwa "Done" na kujaza paneli ya Maoni ya Mhariri |

**4.3** Fungua paneli zinazoweza kufichwa za "Research Results", "Product Matches", na "Editor Feedback" chini ya makala ili kuchunguza JSON ghafi ilizozalisha kila wakala.

---

### Mafunzo 5: Badilisha UI (Jifunze Zaidi)

Jaribu moja au zaidi ya maboresho haya:

**5.1 Ongeza hesabu ya maneno.** Baada ya Mwandishi kumaliza, onyesha hesabu ya maneno ya makala chini ya paneli ya matokeo. Unaweza kuhesabu hili ndani ya `handleMessage` wakati `type === "writer"` na `data.complete` ni kweli:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Ongeza kiashiria cha jaribio jipya.** Wakati Mhariri anatoa ombi la marekebisho, mchakato unaendeshwa tena. Onyesha bendera ya "Revision 1" au "Revision 2" kwenye paneli ya hali. Sikiliza ujumbe wa aina `message` wenye neno "Revision" na sasisha kipengele kipya cha DOM.

**5.3 Hali ya giza.** Ongeza kitufe cha kubadili na darasa `.dark` kwenye `<body>`. Badilisha rangi za asili, maandishi, na paneli ndani ya `style.css` kwa chaguo la `body.dark`.

---

## Muhtasari

| Ulikoje | Jinsi |
|-------------|-----|
| Ulidhumisha UI kutoka kwenye backend ya Python | Ulikwama saraka ya `ui/` ukiitumia `StaticFiles` katika FastAPI |
| Ulipeleka seva ya HTTP kwenye toleo la JavaScript | Uliunda `server.mjs` ukitumia moduli ya Node.js `http` ya ndani |
| Ulipeleka API ya wavuti kwenye toleo la C# | Uliunda mradi mpya `csharp-web` ukiwa na API za ASP.NET Core minimal |
| Ulitumia NDJSON ya kupeperusha matangazo kwenye kivinjari | Umetumia `fetch()` na `ReadableStream` na uchambuzi wa JSON mstari kwa mstari |
| Ulibadilisha UI kwa wakati halisi | Umetumia aina za ujumbe kusasisha DOM (alama, maandishi, paneli zinazozamishwa) |

---

## Vidokezo Vikuu

1. Mbele-mwisho wa **static unaoshirikiwa** unaweza kufanya kazi na backend yoyote inayozungumza itifaki ile ile ya mtiririko, kuthibitisha thamani ya mfano wa API unaolingana na OpenAI.
2. **JSON ulio na alama ya newline (NDJSON)** ni muundo rahisi wa kupeperusha matangazo unaofanya kazi asili na API ya kivinjari `ReadableStream`.
3. Toa tofauti ya **Python** ilihitaji mabadiliko madogo zaidi kwa sababu tayari ilikuwa na njia ya FastAPI; tofauti za JavaScript na C# zilihitaji wambiso mdogo wa HTTP.
4. Kuwa na UI kama **vanilla HTML/CSS/JS** huzuia zana za ujenzi, utegemezi wa fremu, na ugumu zaidi kwa wanafunzi wa warsha.
5. Moduli za wakala kama (Mtafiti, Bidhaa, Mwandishi, Mhariri) hutumika tena bila mabadiliko; mabadiliko yapo kwenye safu ya usafirishaji tu.

---

## Kusoma Zaidi

| Rasilimali | Kiungo |
|----------|------|
| MDN: Matumizi ya Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| Faili za Static za FastAPI | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| Faili za Static za ASP.NET Core | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Maelezo ya NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Endelea kwa [Sehemu ya 13: Warsha Imekamilika](part13-workshop-complete.md) kwa muhtasari wa kila kitu ulichojenga katika warsha hii.

---
[← Sehemu 11: Kupiga Simu Zana](part11-tool-calling.md) | [Sehemu 13: Warsha Imemalizika →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Angalizo**:  
Hati hii imetafsiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Wakati tunajitahidi kwa usahihi, tafadhali fahamu kuwa tafsiri za moja kwa moja zinaweza kuwa na makosa au kasoro. Hati asili katika lugha yake ya asili inapaswa kuchukuliwa kama chanzo cha mamlaka. Kwa taarifa muhimu, tafsiri ya kitaalamu ya binadamu inapendekezwa. Hatubeba jukumu kwa kutoelewana au ufafanuzi mbaya unaotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->