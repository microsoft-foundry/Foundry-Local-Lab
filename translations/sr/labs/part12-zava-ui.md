![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deo 12: Izrada Web UI za Zava Creative Writer

> **Cilj:** Dodajte front-end zasnovan na pregledaču za Zava Creative Writer тако да можете pratiti višestruku agenatsku liniju obrade u realnom vremenu, са статус обележјима агената уживо и преносом текста чланка, све послужено са једног локалног веб сервера.

У [Deo 7](part7-zava-creative-writer.md) сте истраживали Zava Creative Writer као **CLI апликацију** (JavaScript, C#) и као **headless API** (Python). У овој лабораторији ћете повезати заједнички **vanilla HTML/CSS/JavaScript** front-end са свим back-end-овима како би корисници могли да интерагују са линијом обраде преко прегледача уместо терминала.

---

## Шта ћете научити

| Циљ | Опис |
|-----------|-------------|
| Послуживање статичких фајлова са back-end-а | Монтирјати HTML/CSS/JS фасциклу поред ваше API руте |
| Потрошња стриминг NDJSON у прегледачу | Користити Fetch API са `ReadableStream` за читање JSON-а раздвојеног новим редом |
| Унифицирани стриминг протокол | Обезбедити да Python, JavaScript и C# back-end-ови емитују исти формат порука |
| Прогресивна ажурирања UI-а | Ажурирати статусне ознаке агената и стримовати текст чланка ознаком по ознаку |
| Додати HTTP слој CLI апликацији | Обмотати постојећу логику оркестратора у Express-style сервер (JS) или ASP.NET Core минимални API (C#) |

---

## Архитектура

UI је један скуп статичких фајлова (`index.html`, `style.css`, `app.js`) који деле сви три бекенда. Свaki бекенд експонира иста два путање:

![Zava UI архитектура — заједнички front end са три back-end-а](../../../images/part12-architecture.svg)

| Рута | Метод | Намена |
|-------|--------|---------|
| `/` | GET | Послужује статички UI |
| `/api/article` | POST | Покреће вишеструку агенатску линију и стримује NDJSON |

Front-end шаље JSON тело и чита одговор као стрим порука раздвојених новим редом. Свaka порука има поље `type` које UI користи за ажурирање одговарајућег панела:

| Тип поруке | Значење |
|-------------|---------|
| `message` | Ажурирање статуса (нпр. "Покреће се задатак истраживачког агента...") |
| `researcher` | Резултати истраживања су спремни |
| `marketing` | Резултати претраге производа су спремни |
| `writer` | Писац је почео или завршио (садржи `{ start: true }` или `{ complete: true }`) |
| `partial` | Једна јединствена пренета ознака од Писца (садржи `{ text: "..." }`) |
| `editor` | Суд редактора је спреман |
| `error` | Нешто је пошло по злу |

![Рутинг типова порука у прегледачу](../../../images/part12-message-types.svg)

![Секвенца стримовања — комуникација између прегледача и бекенда](../../../images/part12-streaming-sequence.svg)

---

## Предуслови

- Завршите [Deo 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Узмите Foundry Local CLI инсталиран и модели `phi-3.5-mini` преузет
- Модеран веб прегледач (Chrome, Edge, Firefox или Safari)

---

## Заједнички UI

Пре него што додирнете иједан бекенд код, узмите тренутак да истражите front-end који ће користити све три језичке варијанте. Фајлови се налазе у `zava-creative-writer-local/ui/`:

| Фајл | Намена |
|------|---------|
| `index.html` | Лаиоут странице: образац за унос, статусне ознаке агената, област за излаз текста чланка, скупљиви панели детаља |
| `style.css` | Минимални стилови са статусним бојама ознака (чекање, ради, готово, грешка) |
| `app.js` | Fetch позив, читач редова `ReadableStream`, и логика ажурирања DOM-а |

> **Савет:** Отворите `index.html` директно у вашем претраживачу да видите распоред. Још ништа неће радити јер нема бекенд-а, али можете видети структуру.

### Како ради Stream Reader

Кључна функција у `app.js` чита тело одговора по деловима и дели на границама новог реда:

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
    buffer = lines.pop(); // задржи непотпуни последњи ред

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

Сваку парсирану поруку прослеђује `handleMessage()`, која ажурира релевантни DOM елемент према `msg.type`.

---

## Вежбе

### Вежба 1: Покрените Python бекенд са UI

Python (FastAPI) варијанта већ има стриминг API крајњу тачку. Једина измена је монтирање фасцикле `ui/` као статички фајлови.

**1.1** Идите у Python API директорјум и инсталирајте зависности:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Покрените сервер:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Отворите прегледач на `http://localhost:8000`. Требало би да видите UI Zava Creative Writer са три текстуална поља и дугметом „Generate Article“.

**1.4** Кликните **Generate Article** користећи подразумеване вредности. Пратите како статусне ознаке агената прелазе са „Waiting“ на „Running“ па на „Done“ док сваки агент завршава свој рад, а текст чланка се учитава током приказа, ознака по ознака.

> **Решавање проблема:** Ако страница приказује JSON одговор уместо UI-а, проверите да ли покрећете ажуриран `main.py` који монтира статичке фајлове. Ендпоинт `/api/article` и даље ради на оригиналној путањи; монтажа статичких фајлова послужује UI на свакој другој рути.

**Како ради:** Ажурирани `main.py` додаје један ред на дну:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Ово послужује сваки фајл из `zava-creative-writer-local/ui/` као статички ресурс, са `index.html` као подразумеваним документом. POST рута `/api/article` је регистрована пре статичког монтiranja, тако да има приоритет.

---

### Вежба 2: Додајте веб сервер JavaScript варијанти

JavaScript варијанта тренутно је CLI апликација (`main.mjs`). Нови фајл, `server.mjs`, омотава исте агенатске модуле иза HTTP сервера и послужује заједнички UI.

**2.1** Идите у JavaScript директоријум и инсталирајте зависности:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Покрените веб сервер:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Треба да видите:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Отворите `http://localhost:3000` у вашем претраживачу и кликните **Generate Article**. Исти UI ради идентично са JavaScript бекендом.

**Проучите код:** Отворите `server.mjs` и обратите пажњу на кључне шаблоне:

- **Послуживање статичких фајлова** користи уграђене Node.js модуле `http`, `fs` и `path` без потребе за спољним оквиром.
- **Заштита од путање преласка** нормализује тражени пут и проверава да ли остаје унутар фасцикле `ui/`.
- **NDJSON стриминг** користи помоћну функцију `sendLine()` која серијализује сваки објекат, уклања унутрашње нове редове и додаје завршни нови ред.
- **Оркестрација агената** поново користи постојеће модуле `researcher.mjs`, `product.mjs`, `writer.mjs` и `editor.mjs` без измена.

<details>
<summary>Кључни исечак из server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Истраживач
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Писац (стримовање)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Вежба 3: Додајте минимални API C# варијанти

C# варијанта је тренутно конзолна апликација. Нови пројекат, `csharp-web`, користи ASP.NET Core минималне API-је да изложи исту линију обраде као веб услугу.

**3.1** Идите у C# web пројекат и обновите пакете:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Покрените веб сервер:

```bash
dotnet run
```

```powershell
dotnet run
```

Треба да видите:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Отворите `http://localhost:5000` у вашем прегледачу и кликните **Generate Article**.

**Проучите код:** Отворите `Program.cs` у директоријуму `csharp-web` и обратите пажњу:

- Пројект фајл користи `Microsoft.NET.Sdk.Web` уместо `Microsoft.NET.Sdk`, што додаје подршку за ASP.NET Core.
- Статички фајлови се послужују преко `UseDefaultFiles` и `UseStaticFiles` усмерених у заједничку фасциклу `ui/`.
- Ендпоинт `/api/article` пише NDJSON линије директно у `HttpContext.Response` и испира након сваке линије ради реал-тайм стриминга.
- Сва логика агената (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) је иста као у конзолној верзији.

<details>
<summary>Кључни исечак из csharp-web/Program.cs</summary>

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

### Вежба 4: Истражите статусне ознаке агената

Сад кад имате радни UI, погледајте како front-end ажурира статусне ознаке.

**4.1** Отворите `zava-creative-writer-local/ui/app.js` у вашем едитору.

**4.2** Пронађите функцију `handleMessage()`. Приметите како мапира типове порука на DOM ажурирања:

| Тип поруке | Акција UI-а |
|-------------|-----------|
| `message` који садржи "researcher" | Поставља ознаку истраживача на "Running" |
| `researcher` | Поставља ознаку истраживача на "Done" и попуњава панел Резултати истраживања |
| `marketing` | Поставља ознаку претраге производа на "Done" и попуњава панел Пронађени производи |
| `writer` са `data.start` | Поставља ознаку писца на "Running" и чисти излазни текст чланка |
| `partial` | Додаје текст ознаке у излазни текст чланка |
| `writer` са `data.complete` | Поставља ознаку писца на "Done" |
| `editor` | Поставља ознаку уредника на "Done" и попуњава панел Коментари уредника |

**4.3** Отворите скупљиве панеле „Research Results“, „Product Matches“ и „Editor Feedback“ испод чланка да бисте прегледали неопходни JSON који је сваки агент произвео.

---

### Вежба 5: Прилагодите UI (додатак)

Пробајте једно или више ових побољшања:

**5.1 Додајте број речи.** Након што писац заврши, приказујте број речи испод излазног панела. Ово можете израчунати у `handleMessage` када је `type === "writer"` и `data.complete` је true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Додајте индикатор поновног покушаја.** Када уредник затражи ревизију, линија обраде се поново покреће. Прикажите транспаренту „Revision 1“ или „Revision 2“ у статус панелу. Слушајте `message` тип који садржи „Revision“ и ажурирајте нови DOM елемент.

**5.3 Тамни режим.** Додајте дугме за пребацивање и `.dark` класу на `<body>`. Преовладајте боје позадине, текста и панела у `style.css` са селектором `body.dark`.

---

## Резиме

| Шта сте урадили | Како |
|-------------|-----|
| Послужили UI са Python бекенда | Монтирали фасциклу `ui/` са `StaticFiles` у FastAPI |
| Додали HTTP сервер JavaScript варијанти | Направили `server.mjs` користећи уграђени Node.js `http` модул |
| Додали веб API C# варијанти | Направили нови пројекат `csharp-web` са ASP.NET Core минималним API-јима |
| Потрошили стриминг NDJSON у прегледачу | Користили `fetch()` са `ReadableStream` и парсирање JSON по линијама |
| Ажурирали UI у реалном времену | Мапирали типове порука на DOM ажурирања (ознаке, текст, скупљиви панели) |

---

## Кључне поуке

1. **Заједнички статички front-end** може радити са било којим бекендом који говори исти стриминг протокол, што појачава вредност OpenAI-компатибилног API шаблона.
2. **JSON раздвојен новим редом (NDJSON)** је једноставан стриминг формат који нативно функционише са `ReadableStream` API прегледача.
3. **Python варијанта** је тражила најмање измена јер је већ имала FastAPI ендпоинт; JavaScript и C# варијанте су тражиле танак HTTP омотач.
4. Чување UI као **vanilla HTML/CSS/JS** избегава алате за градњу, зависности од фрејмворка и додатну сложеност за полазнике радионице.
5. Исти модули агената (Researcher, Product, Writer, Editor) се користе без измена; само слој транспорта се мења.

---

## Додатна литература

| Ресурс | Линк |
|----------|------|
| MDN: Коришћење Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI статички фајлови | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core статички фајлови | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON спецификација | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Наставите на [Deo 13: Workshop Complete](part13-workshop-complete.md) за резиме свега што сте изградили током ове радионице.

---
[← Део 11: Позив алата](part11-tool-calling.md) | [Део 13: Радионица завршена →](part13-workshop-complete.md)