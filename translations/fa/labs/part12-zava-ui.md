![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# بخش ۱۲: ساخت رابط کاربری وب برای Zava نویسنده خلاق

> **هدف:** افزودن یک رابط کاربری مبتنی بر مرورگر به Zava نویسنده خلاق تا بتوانید اجرای خط لوله چندعامله را به صورت زنده مشاهده کنید، با نشانگرهای وضعیت عامل زنده و متن مقاله در حال پخش، همه سرو شده از طریق یک سرور وب محلی واحد.

در [بخش ۷](part7-zava-creative-writer.md) شما Zava نویسنده خلاق را به‌عنوان یک **برنامه خط فرمان (CLI)** (جاوااسکریپت، سی‌شارپ) و یک **API بدون رابط کاربری (headless)** (پایتون) بررسی کردید. در این آزمایشگاه، یک رابط کاربری مشترک **خالص HTML/CSS/جاوااسکریپت** را به هر بک‌اند متصل می‌کنید تا کاربران بتوانند از طریق مرورگر به جای ترمینال با خط لوله تعامل داشته باشند.

---

## آنچه خواهید آموخت

| هدف | توضیح |
|-----------|-------------|
| سرویس‌دهی فایل‌های استاتیک از بک‌اند | نصب دایرکتوری HTML/CSS/JS کنار مسیر API شما |
| مصرف جریان NDJSON در مرورگر | استفاده از Fetch API همراه با `ReadableStream` برای خواندن JSON جداشده با خط جدید |
| پروتکل پخش متحد | اطمینان از اینکه بک‌اندهای پایتون، جاوااسکریپت و سی‌شارپ همان فرمت پیام را صادر می‌کنند |
| به‌روزرسانی‌های پیش‌رونده رابط کاربری | به‌روزرسانی نشانگرهای وضعیت عامل و پخش متن مقاله به صورت توکن به توکن |
| افزودن لایه HTTP به برنامه CLI | قرار دادن منطق گارگردان موجود در یک سرور به سبک Express (جاوااسکریپت) یا API کمینه ASP.NET Core (سی‌شارپ) |

---

## معماری

رابط کاربری یک مجموعه تک از فایل‌های استاتیک (`index.html`، `style.css`، `app.js`) است که توسط هر سه بک‌اند به اشتراک گذاشته شده‌ است. هر بک‌اند همان دو مسیر زیر را ارائه می‌دهد:

![معماری رابط کاربری Zava — رابط کاربری مشترک با سه بک‌اند](../../../images/part12-architecture.svg)

| مسیر | متد | هدف |
|-------|--------|---------|
| `/` | GET | ارائه رابط کاربری استاتیک |
| `/api/article` | POST | اجرای خط لوله چندعامله و پخش NDJSON |

رابط کاربری یک بدنه JSON می‌فرستد و پاسخ را به صورت جریان پیام‌های JSON جداشده با خط جدید می‌خواند. هر پیام دارای فیلد `type` است که رابط کاربری برای به‌روزرسانی پنل مربوطه استفاده می‌کند:

| نوع پیام | معنی |
|-------------|---------|
| `message` | به‌روزرسانی وضعیت (مثلاً "شروع کار عامل پژوهشگر...") |
| `researcher` | نتایج پژوهش آماده است |
| `marketing` | نتایج جستجوی محصول آماده است |
| `writer` | نویسنده شروع یا پایان یافته (حاوی `{ start: true }` یا `{ complete: true }`) |
| `partial` | یک توکن پخش شده از نویسنده (حاوی `{ text: "..." }`) |
| `editor` | نتیجه ویرایشگر آماده است |
| `error` | مشکلی پیش آمده است |

![مسیردهی نوع پیام در مرورگر](../../../images/part12-message-types.svg)

![روند پخش — ارتباط مرورگر با بک‌اند](../../../images/part12-streaming-sequence.svg)

---

## پیش‌نیازها

- تکمیل [بخش ۷: Zava نویسنده خلاق](part7-zava-creative-writer.md)
- نصب Foundry Local CLI و دانلود مدل `phi-3.5-mini`
- یک مرورگر وب مدرن (کروم، اج، فایرفاکس یا سافاری)

---

## رابط کاربری مشترک

قبل از دست زدن به هر کد بک‌اند، کمی زمان بگذارید تا رابط کاربری جلویی را که هر سه زبان استفاده خواهند کرد بررسی کنید. فایل‌ها در `zava-creative-writer-local/ui/` قرار دارند:

| فایل | هدف |
|------|---------|
| `index.html` | طرح صفحه: فرم ورودی، نشانگرهای وضعیت عامل، ناحیه خروجی مقاله، پنل‌های جزئیات جمع‌شونده |
| `style.css` | استایل حداقلی با رنگ‌بندی وضعیت نشانگر (در انتظار، در حال اجرا، انجام شده، خطا) |
| `app.js` | فراخوان Fetch، خواننده خط `ReadableStream` و منطق به‌روزرسانی DOM |

> **نکته:** مستقیماً فایل `index.html` را در مرورگر خود باز کنید تا طرح را پیش‌نمایش کنید. هنوز چیزی کار نمی‌کند چون بک‌اند وجود ندارد، اما ساختار را می‌توانید ببینید.

### نحوه عملکرد خواننده جریان

تابع کلیدی در `app.js` بدنه پاسخ را بخش به بخش می‌خواند و در نقطه‌های خط جدید تقسیم می‌کند:

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
    buffer = lines.pop(); // خط ناقص انتهایی را نگه دارید

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

هر پیام تجزیه‌شده به `handleMessage()` ارسال می‌شود، که عنصر مربوطه در DOM را بر اساس `msg.type` به‌روزرسانی می‌کند.

---

## تمرین‌ها

### تمرین ۱: اجرای بک‌اند پایتون با رابط کاربری

نسخه پایتون (FastAPI) قبلاً یک نقطه پایانی API با قابلیت پخش دارد. تنها تغییر، نصب پوشه `ui/` به عنوان فایل‌های استاتیک است.

**۱.۱** به دایرکتوری API پایتون بروید و وابستگی‌ها را نصب کنید:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**۱.۲** سرور را راه‌اندازی کنید:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**۱.۳** مرورگر خود را باز کرده و به `http://localhost:8000` بروید. باید رابط کاربری Zava نویسنده خلاق با سه فیلد متن و یک دکمه «تولید مقاله» را ببینید.

**۱.۴** روی **تولید مقاله** کلیک کنید با مقادیر پیش‌فرض. تغییر نشانگرهای وضعیت عامل را از «در انتظار» به «در حال اجرا» سپس «انجام شد» مشاهده کنید و ببینید متن مقاله توکن به توکن به پنل خروجی جریان می‌یابد.

> **عیب‌یابی:** اگر صفحه به جای UI پاسخ JSON نشان می‌دهد، مطمئن شوید که `main.py` به‌روزرسانی شده‌اید که فایل‌های استاتیک را سوار می‌کند. نقطه پایانی `/api/article` هنوز در مسیر اصلی خود کار می‌کند؛ نصب فایل‌های استاتیک رابط کاربری را در بقیه مسیرها ارائه می‌دهد.

**نحوه کار:** فایل به‌روزشده `main.py` یک خط در پایین اضافه می‌کند:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

این خط هر فایل در `zava-creative-writer-local/ui/` را به عنوان دارایی استاتیک ارائه می‌دهد، با `index.html` به عنوان سند پیش‌فرض. مسیر POST `/api/article` قبل از نصب استاتیک ثبت شده و در اولویت است.

---

### تمرین ۲: افزودن سرور وب به نسخه جاوااسکریپت

نسخه جاوااسکریپت در حال حاضر یک برنامه CLI (`main.mjs`) است. یک فایل جدید `server.mjs` همان ماژول‌های عامل را پشت سر یک سرور HTTP قرار می‌دهد و رابط کاربری مشترک را سرو می‌کند.

**۲.۱** به دایرکتوری جاوااسکریپت بروید و وابستگی‌ها را نصب کنید:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**۲.۲** سرور وب را راه‌اندازی کنید:

```bash
node server.mjs
```

```powershell
node server.mjs
```

باید ببینید:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**۲.۳** در مرورگر خود `http://localhost:3000` را باز کنید و روی **تولید مقاله** کلیک کنید. همان رابط کاربری به طور کامل با بک‌اند جاوااسکریپت کار می‌کند.

**کد را مطالعه کنید:** `server.mjs` را باز کنید و الگوهای کلیدی را بررسی کنید:

- **سرویس‌دهی فایل‌های استاتیک** از ماژول‌های داخلی Node.js `http`، `fs` و `path` بدون نیاز به چارچوب خارجی استفاده می‌کند.
- **محافظت از عبور مسیر (Path-traversal)** مسیر درخواست شده را نرمال‌سازی می‌کند و تأیید می‌کند که داخل دایرکتوری `ui/` باقی بماند.
- **پخش NDJSON** از تابع کمکی `sendLine()` استفاده می‌کند که هر آبجکت را سریالیزه می‌کند، خط‌های داخلی را حذف می‌کند و یک خط جدید پایانی اضافه می‌کند.
- **مدیریت عوامل** از همان ماژول‌های `researcher.mjs`، `product.mjs`، `writer.mjs` و `editor.mjs` بدون تغییر استفاده می‌کند.

<details>
<summary>گزیده کلیدی از server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// پژوهشگر
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// نویسنده (رایگان)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### تمرین ۳: افزودن API کمینه به نسخه C#

نسخه C# در حال حاضر یک برنامه کنسول است. پروژه جدید `csharp-web` از APIهای کمینه ASP.NET Core برای ارائه همان خط لوله به عنوان یک سرویس وب استفاده می‌کند.

**۳.۱** به پروژه وب C# بروید و بسته‌ها را بازیابی کنید:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**۳.۲** سرور وب را اجرا کنید:

```bash
dotnet run
```

```powershell
dotnet run
```

باید ببینید:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**۳.۳** در مرورگر خود `http://localhost:5000` را باز کنید و روی **تولید مقاله** کلیک کنید.

**کد را مطالعه کنید:** `Program.cs` را در دایرکتوری `csharp-web` باز کنید و توجه کنید:

- فایل پروژه از `Microsoft.NET.Sdk.Web` به جای `Microsoft.NET.Sdk` استفاده می‌کند که پشتیبانی ASP.NET Core را اضافه می‌کند.
- فایل‌های استاتیک با `UseDefaultFiles` و `UseStaticFiles` و اشاره به دایرکتوری مشترک `ui/` ارائه می‌شوند.
- نقطه پایانی `/api/article` خط‌های NDJSON را مستقیماً به `HttpContext.Response` می‌نویسد و پس از هر خط فلش می‌کند برای پخش در زمان واقعی.
- تمام منطق عامل‌ها (`RunResearcher`، `RunProductSearch`، `RunEditor`، `BuildWriterMessages`) مشابه نسخه کنسول است.

<details>
<summary>گزیده کلیدی از csharp-web/Program.cs</summary>

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

### تمرین ۴: بررسی نشانگرهای وضعیت عامل

حالا که یک رابط کاربری کارآمد دارید، ببینید چگونه رابط جلویی نشانگرهای وضعیت را به‌روزرسانی می‌کند.

**۴.۱** فایل `zava-creative-writer-local/ui/app.js` را در ویرایشگر خود باز کنید.

**۴.۲** تابع `handleMessage()` را پیدا کنید. توجه کنید چگونه نوع پیام‌ها را به به‌روزرسانی DOM نگاشت می‌کند:

| نوع پیام | عملیات رابط کاربری |
|-------------|-----------|
| `message` که شامل "researcher" باشد | نشانگر پژوهشگر را به "در حال اجرا" تنظیم می‌کند |
| `researcher` | نشانگر پژوهشگر را به "انجام شده" تغییر می‌دهد و پنل نتایج پژوهش را پر می‌کند |
| `marketing` | نشانگر جستجوی محصول را به "انجام شده" تغییر می‌دهد و پنل تطابق محصولات را پر می‌کند |
| `writer` با `data.start` | نشانگر نویسنده را به "در حال اجرا" تغییر می‌دهد و خروجی مقاله را پاک می‌کند |
| `partial` | متن توکن را به خروجی مقاله اضافه می‌کند |
| `writer` با `data.complete` | نشانگر نویسنده را به "انجام شده" تنظیم می‌کند |
| `editor` | نشانگر ویرایشگر را به "انجام شده" تغییر می‌دهد و پنل بازخورد ویرایشگر را پر می‌کند |

**۴.۳** پنل‌های بازشونده "نتایج پژوهش"، "تطابق محصولات" و "بازخورد ویرایشگر" زیر مقاله را باز کنید تا JSON خامی که هر عامل تولید کرده را بررسی کنید.

---

### تمرین ۵: سفارشی‌سازی رابط کاربری (چالش)

یکی یا چند مورد از بهبودهای زیر را امتحان کنید:

**۵.۱ افزودن شمارش کلمات.** پس از اتمام نویسنده، تعداد کلمات مقاله را زیر پنل خروجی نمایش دهید. می‌توانید این کار را در `handleMessage` وقتی `type === "writer"` و `data.complete` درست است محاسبه کنید:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**۵.۲ افزودن نشانگر تلاش مجدد.** وقتی ویرایشگر درخواست بازنگری می‌دهد، خط لوله دوباره اجرا می‌شود. یک بنر «بازنگری ۱» یا «بازنگری ۲» در پنل وضعیت نمایش دهید. به پیام‌های نوع `message` که شامل کلمه "Revision" هستند گوش کنید و عنصر DOM جدیدی را به‌روزرسانی کنید.

**۵.۳ حالت تاریک.** یک دکمه تغییر حالت و کلاس `.dark` به عنصر `<body>` اضافه کنید. رنگ پس‌زمینه، متن و پنل‌ها را در `style.css` با انتخابگر `body.dark` بازنویسی کنید.

---

## خلاصه

| کاری که انجام دادید | چگونه |
|-------------|-----|
| ارائه رابط کاربری از بک‌اند پایتون | نصب پوشه `ui/` با `StaticFiles` در FastAPI |
| افزودن سرور HTTP به نسخه جاوااسکریپت | ایجاد `server.mjs` با استفاده از ماژول داخلی Node.js `http` |
| افزودن API وب به نسخه C# | ساخت پروژه جدید `csharp-web` با APIهای کمینه ASP.NET Core |
| مصرف جریان NDJSON در مرورگر | استفاده از `fetch()` همراه با `ReadableStream` و تجزیه JSON خط به خط |
| به‌روزرسانی رابط کاربری به صورت زمان واقعی | نگاشت نوع پیام‌ها به به‌روزرسانی‌های DOM (نشانگرها، متن، پنل‌های جمع‌شونده) |

---

## نکات کلیدی

1. یک **رابط کاربری استاتیک مشترک** می‌تواند با هر بک‌اندی که همان پروتکل پخش را صحبت می‌کند کار کند، که ارزش الگوی API سازگار با OpenAI را تقویت می‌کند.
2. **JSON جداشده با خط جدید (NDJSON)** قالبی ساده برای پخش است که به صورت بومی با API `ReadableStream` مرورگر کار می‌کند.
3. **نسخه پایتون** کمترین تغییر را نیاز داشت چون قبلاً یک نقطه پایانی FastAPI داشت؛ نسخه‌های جاوااسکریپت و C# به یک لایه HTTP نازک نیاز داشتند.
4. نگه داشتن رابط کاربری به صورت **خالص HTML/CSS/جاوااسکریپت** از ابزارهای ساخت، وابستگی‌های چارچوب و پیچیدگی‌های اضافی برای یادگیرندگان کارگاه جلوگیری می‌کند.
5. همان ماژول‌های عامل (پژوهشگر، محصول، نویسنده، ویرایشگر) بدون تغییر مجدد استفاده می‌شوند؛ فقط لایه انتقال تغییر می‌کند.

---

## مطالعه بیشتر

| منبع | لینک |
|----------|------|
| MDN: استفاده از Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| فایل‌های استاتیک FastAPI | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| فایل‌های استاتیک ASP.NET Core | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| مشخصات NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

برای خلاصه تمام آنچه در طول این کارگاه ساخته‌اید به [بخش ۱۳: کارگاه تکمیل شد](part13-workshop-complete.md) مراجعه کنید.

---
[← بخش ۱۱: فراخوانی ابزار](part11-tool-calling.md) | [بخش ۱۳: پایان کارگاه →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**سلب مسئولیت**:  
این سند با استفاده از سرویس ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما در تلاش برای دقت هستیم، لطفاً توجه داشته باشید که ترجمه‌های خودکار ممکن است دارای اشتباهات یا عدم دقت‌هایی باشند. سند اصلی به زبان بومی خود باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حیاتی، ترجمه حرفه‌ای انسانی توصیه می‌شود. ما در قبال هرگونه سوءتفاهم یا برداشت نادرستی که ناشی از استفاده از این ترجمه باشد مسئولیتی نداریم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->