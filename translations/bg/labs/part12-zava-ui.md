![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Част 12: Създаване на Web UI за Zava Creative Writer

> **Цел:** Добавяне на браузър-базиран фронтенд към Zava Creative Writer, за да можете да наблюдавате изпълнението на мултиагентния пейплайн в реално време, с индикатори за статус на агентите и поточно предаване на текста на статията, всичко това сервирано от един локален уеб сървър.

В [Част 7](part7-zava-creative-writer.md) разгледахте Zava Creative Writer като **CLI приложение** (JavaScript, C#) и като **headless API** (Python). В този лабораторен упражнение ще свържете споделен **vanilla HTML/CSS/JavaScript** фронтенд към всеки бекенд, така че потребителите да могат да взаимодействат с пейплайна през браузър, вместо през терминал.

---

## Какво ще научите

| Цел | Описание |
|-----------|-------------|
| Сервиране на статични файлове от бекенд | Монтиране на директория с HTML/CSS/JS до вашия API маршрут |
| Консумиране на поток от NDJSON в браузъра | Използване на Fetch API с `ReadableStream` за четене на JSON, разделен с нов ред |
| Унифициран стрийминг протокол | Осигуряване, че Python, JavaScript и C# бекенди излъчват същия формат на съобщения |
| Прогресивни UI ъпдейти | Обновяване на статус индикаторите на агентите и поточното предаване на текста на статията токен по токен |
| Добавяне на HTTP слой към CLI приложение | Обвиване на съществуващата оркестрационна логика в Express-подобен сървър (JS) или минимален ASP.NET Core API (C#) |

---

## Архитектура

UI е единен набор от статични файлове (`index.html`, `style.css`, `app.js`), споделени от трите бекенда. Всеки бекенд излага едни и същи два маршрута:

![Zava UI архитектура — споделен фронтенд с три бекенда](../../../images/part12-architecture.svg)

| Маршрут | Метод | Цел |
|-------|--------|---------|
| `/` | GET | Сервира статичния UI |
| `/api/article` | POST | Изпълнява мултиагентния пейплайн и предава NDJSON на поток |

Фронтендът изпраща JSON тяло и чете отговора като поток от съобщения на JSON, разделени с нов ред. Всяко съобщение има поле `type`, което UI използва, за да обнови съответния панел:

| Тип на съобщение | Значение |
|-------------|---------|
| `message` | Статусна актуализация (напр. "Започване на задача на изследователя...") |
| `researcher` | Резултатите от изследването са готови |
| `marketing` | Резултатите от продуктовото търсене са готови |
| `writer` | Писателят започна или приключи (съдържа `{ start: true }` или `{ complete: true }`) |
| `partial` | Един токен на поток от писателя (съдържа `{ text: "..." }`) |
| `editor` | Решението на редактора е готово |
| `error` | Нещо се обърка |

![Маршрутизиране на типовете съобщения в браузъра](../../../images/part12-message-types.svg)

![Последователност на стрийминг — Комуникация от браузъра към бекенд](../../../images/part12-streaming-sequence.svg)

---

## Изисквания

- Изпълнена [Част 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Инсталиран Foundry Local CLI и свален модел `phi-3.5-mini`
- Модерен уеб браузър (Chrome, Edge, Firefox, или Safari)

---

## Споделеният UI

Преди да пипате код във бекенда, отделете момент да разгледате фронтенда, който трите езикови варианта ползват. Файловете са разположени в `zava-creative-writer-local/ui/`:

| Файл | Цел |
|------|---------|
| `index.html` | Оформление на страницата: форма за вход, статус индикатори на агентите, зона за статията, разтварящи се панели с подробности |
| `style.css` | Минимално стилизиране със статусни цветови състояния на значките (чакам, работи, готово, грешка) |
| `app.js` | Извикване на Fetch, `ReadableStream` за четене на линии и логика за обновяване на DOM |

> **Съвет:** Отворете `index.html` директно в браузъра, за да видите оформлението. Все още нищо няма да работи, защото няма бекенд, но структурата е видима.

### Как работи Stream Reader

Ключовата функция в `app.js` чете тялото на отговора на порции и разделя по нови редове:

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
    buffer = lines.pop(); // запази непълния краен ред

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

Всяко парснато съобщение се препраща към `handleMessage()`, която обновява съответния DOM елемент според `msg.type`.

---

## Упражнения

### Упражнение 1: Стартиране на Python Бекенд с UI

Python (FastAPI) вариантът вече има потоков API ендпойнт. Единствената промяна е да се монтира папката `ui/` като статични файлове.

**1.1** Отидете до директорията на Python API и инсталирайте зависимостите:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Стартирайте сървъра:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Отворете браузър на `http://localhost:8000`. Трябва да видите UI-то на Zava Creative Writer с три текстови полета и бутон "Generate Article".

**1.4** Натиснете **Generate Article** с подразбиращите се стойности. Наблюдавайте как статус индикаторите на агентите се променят от "Waiting" на "Running" и след това "Done" докато агентите изпълняват задачите си, и вижте как текстът на статията се появява токен по токен в панела за изход.

> **Отстраняване на проблеми:** Ако страницата показва JSON отговор вместо UI, уверете се, че стартирате обновения `main.py`, който монтира статичните файлове. Ендпойнтът `/api/article` продължава да работи на оригиналния си път; монтирането на статични файлове сервира UI-то на всички останали маршрути.

**Как работи:** Обновеният `main.py` добавя един ред в края:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Това сервира всеки файл от `zava-creative-writer-local/ui/` като статичен ресурс, с `index.html` като подразбиращ се документ. `POST` маршрутът `/api/article` е регистриран преди статичното монтиране, така че има приоритет.

---

### Упражнение 2: Добавяне на Уеб Сървър към JavaScript Варианта

JavaScript вариантът е в момента CLI приложение (`main.mjs`). Нов файл, `server.mjs`, обвива същите агентски модули зад HTTP сървър и сервира споделения UI.

**2.1** Отидете до директорията на JavaScript и инсталирайте зависимости:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Стартирайте уеб сървъра:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Ще видите:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Отворете `http://localhost:3000` в браузъра си и натиснете **Generate Article**. Същият UI работи идентично с JavaScript бекенда.

**Проучете кода:** Отворете `server.mjs` и обърнете внимание на ключовите модели:

- **Сервиране на статични файлове** използва вградените в Node.js модули `http`, `fs` и `path` без външен framework.
- **Защита от path-traversal** нормализира заявения път и проверява дали остава в директорията `ui/`.
- **NDJSON стрийминг** използва помощната функция `sendLine()`, която сериализира всеки обект, премахва нови редове вътре и добавя завършващ нов ред.
- **Оркестрация на агентите** използва същите модули `researcher.mjs`, `product.mjs`, `writer.mjs` и `editor.mjs` без промени.

<details>
<summary>Ключов откъс от server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Изследовател
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Писател (на живо)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Упражнение 3: Добавяне на Минимален API към C# Варианта

C# вариантът е в момента конзолно приложение. Нов проект, `csharp-web`, използва ASP.NET Core минимални API-та, които излагат същия пейплайн като уеб услуга.

**3.1** Отидете до C# уеб проекта и възстановете пакети:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Стартирайте уеб сървъра:

```bash
dotnet run
```

```powershell
dotnet run
```

Ще видите:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Отворете `http://localhost:5000` в браузъра и натиснете **Generate Article**.

**Проучете кода:** Отворете `Program.cs` в директорията `csharp-web` и обърнете внимание:

- Файлът на проекта използва `Microsoft.NET.Sdk.Web` вместо `Microsoft.NET.Sdk`, който добавя поддръжка за ASP.NET Core.
- Статичните файлове се сервират чрез `UseDefaultFiles` и `UseStaticFiles`, сочещи към споделената директория `ui/`.
- Ендпойнтът `/api/article` записва NDJSON линии директно в `HttpContext.Response` и изпразва буфера след всяка линия за поточно предаване в реално време.
- Цялата агентска логика (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) е същата като в конзолната версия.

<details>
<summary>Ключов откъс от csharp-web/Program.cs</summary>

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

### Упражнение 4: Разгледайте Индикаторите за Статус на Агентите

Сега, когато имате работещ UI, вижте как фронтендът обновява статусните индикатори.

**4.1** Отворете `zava-creative-writer-local/ui/app.js` в редактора си.

**4.2** Намерете функцията `handleMessage()`. Забележете как картографира типовете съобщения към обновяване на DOM:

| Тип на съобщение | Действие в UI |
|-------------|---------------|
| `message`, съдържащо "researcher" | Задава значката на Изследователя на "Running" |
| `researcher` | Задава значката на Изследователя на "Done" и попълва панела с резултати от изследването |
| `marketing` | Задава значката на Търсенето на продукти на "Done" и попълва панела с намерени продукти |
| `writer` с `data.start` | Задава значката на Писателя на "Running" и изчиства изхода за статията |
| `partial` | Добавя текста на токена в изхода на статията |
| `writer` с `data.complete` | Задава значката на Писателя на "Done" |
| `editor` | Задава значката на Редактора на "Done" и попълва панела с обратна връзка от редактора |

**4.3** Отворете разтварящите се панели "Research Results", "Product Matches" и "Editor Feedback" под статията, за да разгледате суровия JSON, който всеки агент е генерирал.

---

### Упражнение 5: Персонализирайте UI-то (Допълнително)

Опитайте едно или повече от следните подобрения:

**5.1 Добавете брой думи.** След като Писателят приключи, покажете броя на думите на статията под панела на изхода. Можете да го изчислите във `handleMessage`, когато `type === "writer"` и `data.complete` е true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Добавете индикатор за повторение.** Когато Редакторът поиска ревизия, пейплайнът се стартира отново. Покажете банер "Revision 1" или "Revision 2" в панела за статус. Слушайте за съобщение от тип `message`, съдържащо "Revision" и обновете нов DOM елемент.

**5.3 Тъмен режим.** Добавете бутон за превключване и клас `.dark` към `<body>`. Поставете презаписване на фонови, текстови и панелни цветове в `style.css` с селектор `body.dark`.

---

## Резюме

| Какво направихте | Как |
|-------------|-----|
| Сервирахте UI-то от Python бекенда | Монтирахте папката `ui/` с `StaticFiles` в FastAPI |
| Добавихте HTTP сървър към JavaScript варианта | Създадохте `server.mjs` с вградения Node.js модул `http` |
| Добавихте уеб API към C# варианта | Създадохте нов проект `csharp-web` с ASP.NET Core минимални API-та |
| Консумирахте стрийминг NDJSON в браузъра | Използвахте `fetch()` с `ReadableStream` и парсинг на JSON ред по ред |
| Обновявахте UI в реално време | Свързахте типове съобщения с обновявания на DOM (значки, текст, разтварящи се панели) |

---

## Основни Изводи

1. **Споделен статичен фронтенд** може да работи с всеки бекенд, който използва същия стрийминг протокол, което подсилва стойността на OpenAI-компатибилния API модел.
2. **JSON, разделен с нов ред (NDJSON)** е прост стрийминг формат, който работи нативно с браузър API `ReadableStream`.
3. **Python вариантът** имаше нужда от най-малко промени, тъй като вече разполагаше с FastAPI ендпойнт; JavaScript и C# вариантите изискват тънък HTTP слой.
4. Поддържането на UI-то като **vanilla HTML/CSS/JS** избягва инструменти за билд, зависимости от framework и допълнителна сложност за участниците в работилницата.
5. Същите агентски модули (Researcher, Product, Writer, Editor) се използват без промяна; променя се само транспортния слой.

---

## Допълнително Четиво

| Ресурс | Връзка |
|----------|------|
| MDN: Използване на Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Статични файлове | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Статични файлове | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Спецификация NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Продължете към [Част 13: Завършване на Работилницата](part13-workshop-complete.md) за резюме на всичко, което сте изградили през тази работилница.

---
[← Част 11: Извикване на инструмент](part11-tool-calling.md) | [Част 13: Завършване на работилницата →](part13-workshop-complete.md)