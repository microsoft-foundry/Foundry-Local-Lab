![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Часть 12: Создание веб-интерфейса для Zava Creative Writer

> **Цель:** Добавить браузерный интерфейс для Zava Creative Writer, чтобы вы могли наблюдать работу многоагентного конвейера в реальном времени, с отображением статуса агентов и потоковой передачей текста статьи, всё это подаётся с одного локального веб-сервера.

В [Части 7](part7-zava-creative-writer.md) вы изучали Zava Creative Writer как **CLI приложение** (JavaScript, C#) и как **безголовый API** (Python). В этой лаборатории вы подключите общий **чистый HTML/CSS/JavaScript** фронтенд к каждому бэкенду, чтобы пользователи могли взаимодействовать с конвейером через браузер, а не через терминал.

---

## Чему вы научитесь

| Цель | Описание |
|-----------|-------------|
| Обслуживать статические файлы с бэкенда | Смонтировать каталог HTML/CSS/JS рядом с маршрутом API |
| Обрабатывать поток NDJSON в браузере | Использовать Fetch API с `ReadableStream` для чтения JSON, разделённого переносами строк |
| Унифицированный протокол потоковой передачи | Обеспечить одинаковый формат сообщений в бэкендах на Python, JavaScript и C# |
| Прогрессивные обновления UI | Обновлять статусы агентов и поток текста статьи по токену |
| Добавить HTTP слой к CLI приложению | Обернуть существующую логику оркестратора в сервер в стиле Express (JS) или минимальный API ASP.NET Core (C#) |

---

## Архитектура

UI — это один набор статических файлов (`index.html`, `style.css`, `app.js`), общий для всех трёх бэкендов. Каждый бэкенд предоставляет два одинаковых маршрута:

![Архитектура Zava UI — общий фронтенд с тремя бэкендами](../../../images/part12-architecture.svg)

| Маршрут | Метод | Назначение |
|---------|--------|------------|
| `/` | GET | Обслуживает статический UI |
| `/api/article` | POST | Запускает многоагентный конвейер и передаёт NDJSON в потоковом режиме |

Фронтенд отправляет тело JSON и читает ответ как поток сообщений, разделённых переносами строк. Каждое сообщение содержит поле `type`, которое UI использует для обновления нужной панели:

| Тип сообщения | Значение |
|---------------|----------|
| `message` | Обновление статуса (например, "Запуск задачи агента исследователя...") |
| `researcher` | Готовы результаты исследования |
| `marketing` | Готовы результаты поиска продукта |
| `writer` | Писатель начал или завершил работу (содержит `{ start: true }` или `{ complete: true }`) |
| `partial` | Один токен, переданный из писателя (содержит `{ text: "..." }`) |
| `editor` | Готово решение редактора |
| `error` | Произошла ошибка |

![Маршрутизация типов сообщений в браузере](../../../images/part12-message-types.svg)

![Последовательность потоковой передачи — взаимодействие браузера и бэкенда](../../../images/part12-streaming-sequence.svg)

---

## Требования

- Завершить [Часть 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Установлен Foundry Local CLI и загружена модель `phi-3.5-mini`
- Современный веб-браузер (Chrome, Edge, Firefox или Safari)

---

## Общий UI

Перед тем как трогать бэкенд-логику, уделите время изучению фронтенда, который будет использоваться во всех трёх языковых версиях. Файлы находятся в `zava-creative-writer-local/ui/`:

| Файл | Назначение |
|-------|------------|
| `index.html` | Макет страницы: форма ввода, значки статуса агентов, область вывода статьи, сворачиваемые панели деталей |
| `style.css` | Минимальный стиль с цветами значков состояния (ожидание, выполнение, готово, ошибка) |
| `app.js` | Вызов Fetch, рекурсивный парсер `ReadableStream` и логика обновления DOM |

> **Совет:** Откройте `index.html` напрямую в браузере для предварительного просмотра макета. Пока ничего не работает, так как нет бэкенда, но структура видна.

### Как работает потоковый ридер

Ключевая функция в `app.js` читает тело ответа по частям и разбивает данные по переносам строк:

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
    buffer = lines.pop(); // сохранить незавершённую конечную строку

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

Каждое разобранное сообщение отправляется в `handleMessage()`, который обновляет соответствующий элемент DOM на основе `msg.type`.

---

## Упражнения

### Упражнение 1: Запуск Python-бэкенда с UI

Вариант Python (FastAPI) уже имеет потоковой API-эндпоинт. Единственное изменение — это монтирование папки `ui/` как статических файлов.

**1.1** Перейдите в каталог Python API и установите зависимости:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Запустите сервер:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Откройте браузер по адресу `http://localhost:8000`. Вы должны увидеть UI Zava Creative Writer с тремя текстовыми полями и кнопкой «Generate Article».

**1.4** Нажмите **Generate Article** с настройками по умолчанию. Наблюдайте, как статус значков агентов меняется с «Waiting» на «Running» и «Done» по мере выполнения каждого агента, а текст статьи по токену поступает в панель вывода.

> **Устранение неполадок:** Если страница показывает JSON-ответ вместо UI, убедитесь, что запускаете обновлённый `main.py`, монтирующий статические файлы. Эндпоинт `/api/article` по-прежнему доступен по своему исходному пути; монтирование статики обслуживает UI по всем другим маршрутам.

**Как это работает:** Обновлённый `main.py` содержит одну строку внизу:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Это отдаёт все файлы из `zava-creative-writer-local/ui/` как статические активы с `index.html` по умолчанию. Маршрут POST `/api/article` зарегистрирован до монтирования статических файлов, поэтому имеет приоритет.

---

### Упражнение 2: Добавить веб-сервер к JavaScript-версии

Вариант на JavaScript — это сейчас CLI-приложение (`main.mjs`). Новый файл `server.mjs` оборачивает те же модули агентов в HTTP-сервер и обслуживает общий UI.

**2.1** Перейдите в каталог JavaScript и установите зависимости:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Запустите веб-сервер:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Вы увидите:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Откройте `http://localhost:3000` в браузере и нажмите **Generate Article**. Тот же UI работает идентично с JavaScript-бэкендом.

**Изучите код:** Откройте `server.mjs` и обратите внимание на ключевые шаблоны:

- **Обслуживание статических файлов** использует встроенные модули Node.js `http`, `fs` и `path` без внешних фреймворков.
- **Защита от path-traversal** нормализует запрашиваемый путь и проверяет, что он остаётся внутри каталога `ui/`.
- **Потоковая передача NDJSON** использует помощник `sendLine()`, который сериализует объект, убирает внутренние переносы строк и добавляет завершающий перенос строки.
- **Оркестровка агентов** повторно использует существующие модули `researcher.mjs`, `product.mjs`, `writer.mjs` и `editor.mjs` без изменений.

<details>
<summary>Ключевой фрагмент из server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Исследователь
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Писатель (стриминг)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Упражнение 3: Добавить минимальный API к C#-версии

Вариант на C# — это сейчас консольное приложение. Новый проект `csharp-web` использует минимальные API ASP.NET Core для предоставления того же конвейера как веб-сервиса.

**3.1** Перейдите в проект C# веб и восстановите пакеты:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Запустите веб-сервер:

```bash
dotnet run
```

```powershell
dotnet run
```

Вы увидите:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Откройте `http://localhost:5000` в браузере и нажмите **Generate Article**.

**Изучите код:** Откройте `Program.cs` в каталоге `csharp-web` и обратите внимание:

- Файл проекта использует `Microsoft.NET.Sdk.Web` вместо `Microsoft.NET.Sdk`, что добавляет поддержку ASP.NET Core.
- Статические файлы обслуживаются через `UseDefaultFiles` и `UseStaticFiles`, указывающие на общий каталог `ui/`.
- Эндпоинт `/api/article` записывает строки NDJSON напрямую в `HttpContext.Response` и сбрасывает буфер после каждой строки для потоковой передачи в реальном времени.
- Вся логика агентов (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) такая же, как в консольной версии.

<details>
<summary>Ключевой фрагмент из csharp-web/Program.cs</summary>

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

### Упражнение 4: Изучите значки статуса агентов

Теперь, когда UI работает, посмотрите, как фронтенд обновляет значки статуса.

**4.1** Откройте `zava-creative-writer-local/ui/app.js` в редакторе.

**4.2** Найдите функцию `handleMessage()`. Обратите внимание, как она сопоставляет типы сообщений с обновлениями DOM:

| Тип сообщения | Действие UI |
|---------------|-------------|
| `message`, содержащее "researcher" | Устанавливает значок Researcher в состояние "Running" |
| `researcher` | Устанавливает значок Researcher в "Done" и заполняет панель Research Results |
| `marketing` | Устанавливает значок Product Search в "Done" и заполняет панель Product Matches |
| `writer` с `data.start` | Устанавливает значок Writer в "Running" и очищает вывод статьи |
| `partial` | Добавляет текст токена к выводу статьи |
| `writer` с `data.complete` | Устанавливает значок Writer в "Done" |
| `editor` | Устанавливает значок Editor в "Done" и заполняет панель Editor Feedback |

**4.3** Раскройте сворачиваемые панели "Research Results", "Product Matches" и "Editor Feedback" под статьёй, чтобы просмотреть сырые JSON-данные, которые сгенерировал каждый агент.

---

### Упражнение 5: Настройка UI (опционально)

Попробуйте одно или несколько из этих улучшений:

**5.1 Добавьте счётчик слов.** После завершения работы Writer отображайте количество слов статьи под панелью вывода. Это можно посчитать в `handleMessage`, когда `type === "writer"` и `data.complete` истинно:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Добавьте индикатор повторной попытки.** Когда редактор запрашивает правку, конвейер запускается повторно. Показывайте баннер "Revision 1" или "Revision 2" в панели статуса. Слушайте сообщения типа `message`, содержащие "Revision", и обновляйте новый DOM-элемент.

**5.3 Тёмная тема.** Добавьте переключатель и класс `.dark` для `<body>`. Переопределите цвета фона, текста и панелей в `style.css` через селектор `body.dark`.

---

## Итоги

| Что вы сделали | Как |
|----------------|-----|
| Обслужили UI из Python-бэкенда | Смонтировали папку `ui/` через `StaticFiles` в FastAPI |
| Добавили HTTP-сервер к JavaScript-версии | Создали `server.mjs`, используя встроенный модуль Node.js `http` |
| Добавили веб-API к C#-версии | Создали новый проект `csharp-web` с минимальными API ASP.NET Core |
| Обработали потоковой NDJSON в браузере | Использовали `fetch()` с `ReadableStream` и построчный JSON-парсер |
| Обновляли UI в реальном времени | Сопоставляли типы сообщений с обновлениями DOM (значки, текст, панели) |

---

## Основные выводы

1. **Общий статический фронтенд** может работать с любым бэкендом, который поддерживает один и тот же протокол потоковой передачи, что усиливает ценность паттерна API, совместимого с OpenAI.
2. **NDJSON (Newline-delimited JSON)** — простой потоковый формат, нативно поддерживаемый браузерным API `ReadableStream`.
3. **Вариант на Python** потребовал наименьших изменений, так как уже имел FastAPI-эндпоинт; JavaScript и C# версии нуждались в лёгкой HTTP-обёртке.
4. Сохранение UI в виде **чистого HTML/CSS/JS** избегает инструментов сборки, зависимости от фреймворков и упрощает обучение.
5. Одни и те же модули агентов (Researcher, Product, Writer, Editor) повторно используются без изменений; меняется только транспортный слой.

---

## Дополнительные материалы

| Ресурс | Ссылка |
|---------|---------|
| MDN: Использование Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Статические файлы | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Статические файлы | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Спецификация NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Перейдите к [Части 13: Завершение семинара](part13-workshop-complete.md) для обзора всего, что вы создали в ходе этого семинара.

---
[← Часть 11: Вызов инструмента](part11-tool-calling.md) | [Часть 13: Завершение мастерской →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с использованием сервиса автоматического перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на наши усилия обеспечить точность, обратите внимание, что автоматический перевод может содержать ошибки или неточности. Оригинальный документ на его родном языке следует считать авторитетным источником. Для критически важной информации рекомендуется профессиональный перевод человеком. Мы не несем ответственности за любые недоразумения или неправильные толкования, вызванные использованием данного перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->