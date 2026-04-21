![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Частина 12: Створення веб-інтерфейсу для Zava Creative Writer

> **Мета:** Додати браузерний інтерфейс до Zava Creative Writer, щоб ви могли у реальному часі спостерігати за роботою багатокористувацького конвеєра, з віджетами статусу агентів і потоковим виводом тексту статті, усе це обслуговується з одного локального веб-сервера.

У [Частині 7](part7-zava-creative-writer.md) ви досліджували Zava Creative Writer як **CLI-додаток** (JavaScript, C#) та як **безголовий API** (Python). У цій лабораторній роботі ви підключите спільний фронтенд на **ванільному HTML/CSS/JavaScript** до кожного бекенду, щоб користувачі могли взаємодіяти з конвеєром через браузер замість терміналу.

---

## Чого ви навчитеся

| Мета | Опис |
|-----------|-------------|
| Обслуговування статичних файлів із бекенду | Підключити директорію з HTML/CSS/JS поруч із маршрутом API |
| Обробка потокового NDJSON у браузері | Використовувати Fetch API з `ReadableStream` для читання JSON, розділеного перенесенням рядка |
| Уніфікований протокол потокової передачі | Забезпечити, що Python, JavaScript і C# бекенди використовують однаковий формат повідомлень |
| Прогресивне оновлення інтерфейсу | Оновлювати статусні бейджі агентів і виводити текст статті токен за токеном |
| Додати HTTP-шар до CLI-додатку | Обгорнути існуючу логіку оркестратора в сервер у стилі Express (JS) або мінімальний API ASP.NET Core (C#) |

---

## Архітектура

Інтерфейс — це один набір статичних файлів (`index.html`, `style.css`, `app.js`), який використовується всіма трьома бекендами. Кожен бекенд відкриває ті самі два маршрути:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| Маршрут | Метод | Призначення |
|-------|--------|---------|
| `/` | GET | Обслуговування статичного UI |
| `/api/article` | POST | Запуск багатокористувацького конвеєра і потокова передача NDJSON |

Фронтенд відправляє JSON тіло та читає відповідь як потік повідомлень у форматі JSON, розділених новим рядком. Кожне повідомлення має поле `type`, яке UI використовує для оновлення відповідної панелі:

| Тип повідомлення | Значення |
|-------------|---------|
| `message` | Оновлення статусу (наприклад, "Починається завдання агента дослідника...") |
| `researcher` | Результати дослідження готові |
| `marketing` | Результати пошуку продукту готові |
| `writer` | Автор почав або закінчив роботу (містить `{ start: true }` або `{ complete: true }`) |
| `partial` | Один токен, переданий потоком від Письменника (містить `{ text: "..." }`) |
| `editor` | Вирок редактора готовий |
| `error` | Виникла помилка |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## Передумови

- Завершіть [Частину 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Встановлений Foundry Local CLI та завантажена модель `phi-3.5-mini`
- Сучасний веб-браузер (Chrome, Edge, Firefox або Safari)

---

## Спільний UI

Перед тим як торкатися коду бекенду, приділіть час вивченню фронтенду, який використовують всі три мовні варіанти. Файли знаходяться в `zava-creative-writer-local/ui/`:

| Файл | Призначення |
|------|---------|
| `index.html` | Розмітка сторінки: форма вводу, бейджі статусу агентів, область виводу статті, панелі з деталями, які можна згортати |
| `style.css` | Мінімальне стилювання зі станами кольору бейджів статусу (очікування, виконання, завершено, помилка) |
| `app.js` | Виклик Fetch, лінійний читач `ReadableStream` і логіка оновлення DOM |

> **Порада:** Відкрийте `index.html` безпосередньо у вашому браузері для перегляду макету. Ще нічого не працюватиме через відсутність бекенду, але можна побачити структуру.

### Як працює читач потоку

Ключова функція в `app.js` читає тіло відповіді частинами і ділить на рядки за символами нового рядка:

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
    buffer = lines.pop(); // зберегти неповну кінцеву лінію

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

Кожне розпарсене повідомлення передається у `handleMessage()`, яке оновлює відповідний елемент DOM на основі `msg.type`.

---

## Вправи

### Вправа 1: Запустіть Python бекенд із UI

Варіант на Python (FastAPI) вже має кінцеву точку для потокового API. Єдина зміна – монтовання папки `ui/` як статичних файлів.

**1.1** Перейдіть у каталог Python API і встановіть залежності:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Запустіть сервер:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Відкрийте браузер за адресою `http://localhost:8000`. Ви побачите UI Zava Creative Writer зі трьома текстовими полями та кнопкою "Generate Article".

**1.4** Натисніть **Generate Article** з використанням значень за замовчуванням. Спостерігайте, як бейджі статусу агентів змінюються від "Очікування" до "Виконується" і до "Завершено", коли кожен агент завершить свою роботу, а текст статті потоком з'являється у панелі виводу по одному токену.

> **Вирішення проблем:** Якщо замість UI відкривається JSON відповідь, переконайтеся, що ви запускаєте оновлений `main.py`, який підключає статичні файли. Кінцева точка `/api/article` працює за оригінальним шляхом; статичне монтовання обслуговує UI на всіх інших маршрутах.

**Як це працює:** Оновлений `main.py` додає один рядок у кінці:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Це обслуговує кожен файл із `zava-creative-writer-local/ui/` як статичний ресурс, з `index.html` як документом за замовчуванням. Маршрут POST `/api/article` реєструється до монтовання статичних файлів, тому має пріоритет.

---

### Вправа 2: Додайте веб-сервер до JavaScript варіанту

JavaScript варіант наразі є CLI-додатком (`main.mjs`). Новий файл `server.mjs` обгортає ті ж агентські модулі за HTTP-сервером і обслуговує спільний UI.

**2.1** Перейдіть у директорію JavaScript і встановіть залежності:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Запустіть веб-сервер:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Ви побачите:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Відкрийте `http://localhost:3000` у браузері та натисніть **Generate Article**. Той самий UI працює ідентично з JavaScript бекендом.

**Вивчайте код:** Відкрийте `server.mjs` і підкресліть ключові патерни:

- **Обслуговування статичних файлів** використовує вбудовані Node.js модулі `http`, `fs` і `path` без зовнішніх фреймворків.
- **Захист від обходу шляху** нормалізує запрошений шлях і перевіряє, що він залишається в межах папки `ui/`.
- **Потокова передача NDJSON** використовує допоміжну функцію `sendLine()`, яка серіалізує кожен об’єкт, видаляє внутрішні нові рядки і додає кінцевий новий рядок.
- **Оркестрація агентів** повторно використовує існуючі модулі `researcher.mjs`, `product.mjs`, `writer.mjs` і `editor.mjs` без змін.

<details>
<summary>Ключовий уривок з server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Дослідник
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Письменник (стрімінг)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Вправа 3: Додайте мінімальний API до C# варіанту

C# варіант наразі є консольним додатком. Новий проект `csharp-web` використовує ASP.NET Core мінімальні API для відкриття того самого конвеєра як веб-сервіс.

**3.1** Перейдіть у веб-проект C# і відновіть пакети:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Запустіть веб-сервер:

```bash
dotnet run
```

```powershell
dotnet run
```

Ви побачите:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Відкрийте `http://localhost:5000` у браузері та натисніть **Generate Article**.

**Вивчайте код:** Відкрийте `Program.cs` у директорії `csharp-web` і зверніть увагу:

- Файл проекту використовує `Microsoft.NET.Sdk.Web` замість `Microsoft.NET.Sdk`, що додає підтримку ASP.NET Core.
- Статичні файли обслуговуються через `UseDefaultFiles` та `UseStaticFiles`, вказані на спільну директорію `ui/`.
- Кінцева точка `/api/article` записує NDJSON рядки безпосередньо у `HttpContext.Response` і очищує буфер після кожного рядка для потокової передачі в реальному часі.
- Вся логіка агентів (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) така сама, як і в консолі.

<details>
<summary>Ключовий уривок з csharp-web/Program.cs</summary>

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

### Вправа 4: Дослідження бейджів статусу агентів

Тепер, коли у вас є працюючий UI, подивіться, як фронтенд оновлює бейджі статусу.

**4.1** Відкрийте `zava-creative-writer-local/ui/app.js` у вашому редакторі.

**4.2** Знайдіть функцію `handleMessage()`. Зверніть увагу, як вона відображає типи повідомлень на оновлення DOM:

| Тип повідомлення | Дія інтерфейсу |
|-------------|-----------|
| `message`, що містить "researcher" | Встановлює бейдж Researcher в стан "Виконується" |
| `researcher` | Встановлює бейдж Researcher в стан "Завершено" і наповнює панель результатів дослідження |
| `marketing` | Встановлює бейдж Product Search в стан "Завершено" і наповнює панель відповідностей продукту |
| `writer` з `data.start` | Встановлює бейдж Writer в стан "Виконується" і очищує вивід статті |
| `partial` | Додає текст токена до виводу статті |
| `writer` з `data.complete` | Встановлює бейдж Writer в стан "Завершено" |
| `editor` | Встановлює бейдж Editor в стан "Завершено" і наповнює панель з відгуками редактора |

**4.3** Відкрийте згортальні панелі "Research Results", "Product Matches" і "Editor Feedback" під статтею, щоб переглянути необроблений JSON, який створив кожен агент.

---

### Вправа 5: Налаштування UI (Додаткове)

Спробуйте одну або декілька з наступних доповнень:

**5.1 Додайте підрахунок слів.** Після завершення роботи Writer відобразіть кількість слів статті під панеллю виводу. Ви можете обчислити це у `handleMessage` коли `type === "writer"` і `data.complete` рівне true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Додайте індикатор повторної спроби.** Коли Editor запрошує ревізію, конвеєр перезапускається. Показуйте банер "Revision 1" або "Revision 2" у панелі статусу. Слухайте тип повідомлення `message`, що містить "Revision", і оновлюйте новий елемент DOM.

**5.3 Темна тема.** Додайте кнопку-перемикач і клас `.dark` для `<body>`. Перезапишіть кольори фону, тексту і панелей у `style.css` за селектором `body.dark`.

---

## Підсумок

| Що ви зробили | Як |
|-------------|-----|
| Обслуговували UI з бекенду Python | Підключили папку `ui/` за допомогою `StaticFiles` у FastAPI |
| Додали HTTP-сервер у JavaScript варіант | Створили `server.mjs` із вбудованим Node.js модулем `http` |
| Додали веб-API до C# варіанту | Створили новий проект `csharp-web` з ASP.NET Core мінімальними API |
| Споживали потоковий NDJSON у браузері | Використали `fetch()` з `ReadableStream` і лінійним парсингом JSON |
| Оновлювали UI у реальному часі | Відобразили типи повідомлень як оновлення DOM (бейджі, текст, панелі) |

---

## Основні висновки

1. **Спільний статичний фронтенд** може працювати з будь-яким бекендом, який підтримує однаковий потоковий протокол, що підкреслює цінність схеми API, сумісної з OpenAI.
2. **JSON, розділений новими рядками (NDJSON)** — це простий формат потокової передачі, який нативно підтримує API браузера `ReadableStream`.
3. Варіант на **Python** потребував найменших змін, оскільки вже мав кінцеву точку FastAPI; JavaScript та C# варіанти потребували лише тонкий HTTP-обгортку.
4. Утримання UI як **ванільного HTML/CSS/JS** уникає використання інструментів збірки, залежностей фреймворків і додаткової складності для учасників майстер-класу.
5. Ті самі агентські модулі (Researcher, Product, Writer, Editor) повторно використовуються без змін; змінюється лише транспортний рівень.

---

## Подальше читання

| Ресурс | Посилання |
|----------|------|
| MDN: Використання Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Статичні файли | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Статичні файли | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Специфікація NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Продовжуйте до [Частини 13: Майстер-клас завершено](part13-workshop-complete.md) для підсумку всього, що ви побудували під час цього майстер-класу.

---
[← Частина 11: Виклик інструменту](part11-tool-calling.md) | [Частина 13: Майстерня завершена →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:  
Цей документ було перекладено за допомогою сервісу автоматичного перекладу [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, будь ласка, майте на увазі, що автоматичні переклади можуть містити помилки або неточності. Оригінальний документ його рідною мовою слід вважати авторитетним джерелом. Для критичної інформації рекомендується звертатися до професійного людського перекладача. Ми не несемо відповідальності за будь-які непорозуміння або хибні тлумачення, що виникають через використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->