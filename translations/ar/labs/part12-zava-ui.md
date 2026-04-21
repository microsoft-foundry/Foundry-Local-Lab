![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# الجزء 12: بناء واجهة ويب للكاتب الإبداعي زافا

> **الهدف:** إضافة واجهة أمامية قائمة على المتصفح إلى الكاتب الإبداعي زافا بحيث يمكنك مشاهدة تنفيذ خط أنابيب الوكيل المتعدد في الوقت الحقيقي، مع شارات حالة الوكيل مباشرة ونص المقال المتدفق، جميعها تُقدم من خلال خادم ويب محلي واحد.

في [الجزء 7](part7-zava-creative-writer.md) استكشفت الكاتب الإبداعي زافا كتطبيق **واجهة سطر الأوامر** (JavaScript، C#) و **API بدون واجهة** (Python). في هذا المختبر ستربط واجهة أمامية مشتركة **HTML/CSS/JavaScript خام** بكل خلفية بحيث يمكن للمستخدمين التفاعل مع خط الأنابيب عبر المتصفح بدلاً من الطرفية.

---

## ما الذي ستتعلمه

| الهدف | الوصف |
|-----------|-------------|
| تقديم الملفات الثابتة من الخلفية | تركيب مجلد HTML/CSS/JS بجانب مسار API الخاص بك |
| استهلاك NDJSON المتدفق في المتصفح | استخدام Fetch API مع `ReadableStream` لقراءة JSON مفصولة بأسطر جديدة |
| بروتوكول البث الموحد | ضمان أن خلفيات Python وJavaScript وC# تصدر نفس تنسيق الرسالة |
| تحديثات UI تدريجية | تحديث شارات حالة الوكيل وتدفق نص المقال رمز برمز |
| إضافة طبقة HTTP لتطبيق CLI | تغليف منطق المنسق القائم في خادم بأسلوب Express (JS) أو API بسيط ASP.NET Core (C#) |

---

## المعمارية

واجهة المستخدم هي مجموعة واحدة من الملفات الثابتة (`index.html`، `style.css`، `app.js`) مشتركة بين كل الخلفيات الثلاثة. كل خلفية تعرض نفس المسارين:

![معمارية واجهة زافا — واجهة أمامية مشتركة مع ثلاث خلفيات](../../../images/part12-architecture.svg)

| المسار | الطريقة | الغرض |
|-------|--------|---------|
| `/` | GET | تقديم واجهة المستخدم الثابتة |
| `/api/article` | POST | تشغيل خط أنابيب الوكيل المتعدد وبث NDJSON |

الواجهة الأمامية ترسل جسم JSON وتقرأ الاستجابة كتيار من رسائل JSON مفصولة بأسطر جديدة. كل رسالة لها حقل `type` تستخدمه الواجهة لتحديث اللوحة الصحيحة:

| نوع الرسالة | المعنى |
|-------------|---------|
| `message` | تحديث الحالة (مثلاً: "بدء مهمة وكيل الباحث...") |
| `researcher` | نتائج البحث جاهزة |
| `marketing` | نتائج بحث المنتج جاهزة |
| `writer` | الكاتب بدأ أو انتهى (يحتوي على `{ start: true }` أو `{ complete: true }`) |
| `partial` | رمز مفرد متدفق من الكاتب (يحتوي على `{ text: "..." }`) |
| `editor` | حكم المحرر جاهز |
| `error` | حدث خطأ ما |

![توجيه نوع الرسالة في المتصفح](../../../images/part12-message-types.svg)

![تتابع البث — الاتصال من المتصفح إلى الخلفية](../../../images/part12-streaming-sequence.svg)

---

## المتطلبات الأساسية

- إكمال [الجزء 7: الكاتب الإبداعي زافا](part7-zava-creative-writer.md)
- تثبيت Foundry Local CLI وتحميل نموذج `phi-3.5-mini`
- متصفح ويب حديث (Chrome، Edge، Firefox، أو Safari)

---

## واجهة المستخدم المشتركة

قبل لمس أي كود خلفي، خذ لحظة لاستكشاف الواجهة الأمامية التي ستستخدمها جميع المسارات اللغوية الثلاثة. الملفات موجودة في `zava-creative-writer-local/ui/`:

| الملف | الغرض |
|------|---------|
| `index.html` | تخطيط الصفحة: نموذج الإدخال، شارات حالة الوكيل، منطقة إخراج المقال، لوحات تفاصيل قابلة للطي |
| `style.css` | تنسيق بسيط مع حالات ألوان الشارات (انتظار، تشغيل، إتمام، خطأ) |
| `app.js` | مكالمة Fetch، قارئ الخطوط من `ReadableStream`، ومنطق تحديث DOM |

> **نصيحة:** افتح `index.html` مباشرة في متصفحك لمعاينة التخطيط. لا شيء سيعمل بعد لأن لا توجد خلفية، لكن يمكنك رؤية الهيكل.

### كيف يعمل قارئ التدفق

الدالة الأساسية في `app.js` تقرأ جسم الاستجابة قطعة قطعة وتفصلها عند حدود الأسطر الجديدة:

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
    buffer = lines.pop(); // احتفظ بالسطر المتبقي غير المكتمل

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

كل رسالة مُحللة تُرسل إلى `handleMessage()` التي تحدث عنصر DOM المناسب بناءً على `msg.type`.

---

## التمارين

### التمرين 1: تشغيل الخلفية Python مع الواجهة

نسخة Python (FastAPI) تحتوي بالفعل على نقطة نهاية لبث API. التغيير الوحيد هو تركيب مجلد `ui/` كملفات ثابتة.

**1.1** انتقل إلى مجلد Python API وثبّت التبعيات:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** شغّل الخادم:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** افتح متصفحك على `http://localhost:8000`. يجب أن ترى واجهة الكاتب الإبداعي زافا مع ثلاث حقول نصية وزر "إنشاء المقال".

**1.4** انقر على **إنشاء المقال** باستخدام القيم الافتراضية. شاهد شارات حالة الوكيل تتغير من "انتظار" إلى "تشغيل" إلى "منتهٍ" مع انتهاء كل وكيل من عمله، وشاهد نص المقال يتدفق إلى لوحة الإخراج رمز برمز.

> **استكشاف الأخطاء:** إذا عرضت الصفحة استجابة JSON بدلاً من الواجهة، تأكد من تشغيل `main.py` المحدّث الذي يركب الملفات الثابتة. نقطة نهاية `/api/article` لا تزال تعمل في المسار الأصلي؛ التثبيت الثابت يخدم الواجهة على كل المسارات الأخرى.

**كيف تعمل:** `main.py` المحدّث يضيف سطرًا واحدًا في الأسفل:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

هذا يخدم كل ملف من `zava-creative-writer-local/ui/` كأصل ثابت، مع `index.html` كوثيقة افتراضية. مسار POST `/api/article` مسجل قبل التثبيت الثابت، لذا يُعطى الأولوية.

---

### التمرين 2: إضافة خادم ويب إلى نسخة JavaScript

نسخة JavaScript هي حالياً تطبيق CLI (`main.mjs`). ملف جديد، `server.mjs`، يغلف نفس وحدات الوكيل وراء خادم HTTP ويخدم الواجهة المشتركة.

**2.1** انتقل إلى مجلد JavaScript وثبّت التبعيات:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** شغّل خادم الويب:

```bash
node server.mjs
```

```powershell
node server.mjs
```

يجب أن ترى:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** افتح `http://localhost:3000` في متصفحك وانقر **إنشاء المقال**. نفس الواجهة تعمل بشكل متطابق مع خلفية JavaScript.

**ادرس الكود:** افتح `server.mjs` ولاحظ الأنماط الرئيسية:

- **تقديم الملفات الثابتة** يستخدم وحدات Node.js المدمجة `http` و `fs` و `path` بدون إطار عمل خارجي.
- **حماية استعراض المسار** تقوم بتطبيع المسار المطلوب والتحقق من بقائه داخل دليل `ui/`.
- **بث NDJSON** يستخدم دالة مساعدة `sendLine()` تقوم بتسلسلية كل كائن، وتزيل الأسطر الجديدة الداخلية، وتلحق سطرًا جديدًا.
- **تنسيق الوكلاء** يعيد استخدام وحدات `researcher.mjs` و`product.mjs` و`writer.mjs` و`editor.mjs` القائمة دون تغيير.

<details>
<summary>مقتطف رئيسي من server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// باحث
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// كاتب (بث مباشر)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### التمرين 3: إضافة API بسيط إلى نسخة C#

نسخة C# هي حالياً تطبيق كونسول. مشروع جديد، `csharp-web`, يستخدم ASP.NET Core APIs بسيطة لكي يعرض نفس خط الأنابيب كخدمة ويب.

**3.1** انتقل إلى مشروع الويب C# واسترجع الحزم:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** شغّل خادم الويب:

```bash
dotnet run
```

```powershell
dotnet run
```

يجب أن ترى:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** افتح `http://localhost:5000` في متصفحك وانقر **إنشاء المقال**.

**ادرس الكود:** افتح `Program.cs` في مجلد `csharp-web` ولاحظ:

- ملف المشروع يستخدم `Microsoft.NET.Sdk.Web` بدلاً من `Microsoft.NET.Sdk`، مما يضيف دعم ASP.NET Core.
- تخدم الملفات الثابتة عبر `UseDefaultFiles` و`UseStaticFiles` الموجهة إلى دليل `ui/` المشترك.
- نقطة نهاية `/api/article` تكتب أسطر NDJSON مباشرة إلى `HttpContext.Response` وتفرغ بعد كل سطر للبث في الوقت الحقيقي.
- كل منطق الوكلاء (`RunResearcher` و `RunProductSearch` و `RunEditor` و `BuildWriterMessages`) هو نفسه كنسخة الكونسول.

<details>
<summary>مقتطف رئيسي من csharp-web/Program.cs</summary>

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

### التمرين 4: استكشاف شارات حالة الوكلاء

الآن بعد أن لديك واجهة مستخدم عاملة، انظر كيف تقوم الواجهة الأمامية بتحديث شارات الحالة.

**4.1** افتح `zava-creative-writer-local/ui/app.js` في محررك.

**4.2** ابحث عن دالة `handleMessage()`. لاحظ كيف تُعين أنواع الرسائل إلى تحديثات DOM:

| نوع الرسالة | إجراء الواجهة |
|-------------|-----------|
| `message` تحتوي على "researcher" | تعيّن شارة الباحث إلى "تشغيل" |
| `researcher` | تعيّن شارة الباحث إلى "منتهٍ" وتملأ لوحة نتائج البحث |
| `marketing` | تعيّن شارة بحث المنتج إلى "منتهٍ" وتملأ لوحة مطابقات المنتج |
| `writer` مع `data.start` | تعيّن شارة الكاتب إلى "تشغيل" وتمسح إخراج المقال |
| `partial` | ترفق نص الرمز إلى إخراج المقال |
| `writer` مع `data.complete` | تعيّن شارة الكاتب إلى "منتهٍ" |
| `editor` | تعيّن شارة المحرر إلى "منتهٍ" وتملأ لوحة ملاحظات المحرر |

**4.3** افتح لوحات "نتائج البحث"، "مطابقات المنتج"، و"ملاحظات المحرر" القابلة للطي أسفل المقال لفحص JSON الخام الذي أنتجه كل وكيل.

---

### التمرين 5: تخصيص الواجهة (تمديد)

جرّب واحدًا أو أكثر من هذه التحسينات:

**5.1 أضف عداد كلمات.** بعد أن ينهي الكاتب، اعرض عدد كلمات المقال أسفل لوحة الإخراج. يمكنك حساب هذا في `handleMessage` عندما يكون `type === "writer"` و `data.complete` صحيحًا:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 أضف مؤشر إعادة المحاولة.** عندما يطلب المحرر مراجعة، يعاد تشغيل خط الأنابيب. أظهر لافتة "المراجعة 1" أو "المراجعة 2" في لوحة الحالة. استمع إلى نوع رسالة `message` تحتوي على "Revision" وحدّث عنصر DOM جديد.

**5.3 الوضع الداكن.** أضف زر تبديل وفئة `.dark` إلى `<body>`. تجاوز الخلفية، النص، وألوان اللوحة في `style.css` باستخدام محدد `body.dark`.

---

## الملخص

| ما الذي فعلته | كيف |
|-------------|-----|
| قدمت الواجهة من الخلفية Python | ربطت مجلد `ui/` مع `StaticFiles` في FastAPI |
| أضفت خادم HTTP لنسخة JavaScript | أنشأت `server.mjs` باستخدام وحدة Node.js المدمجة `http` |
| أضفت API ويب لنسخة C# | أنشأت مشروع `csharp-web` جديد باستخدام ASP.NET Core APIs بسيطة |
| استهلكت NDJSON المتدفق في المتصفح | استخدمت `fetch()` مع `ReadableStream` وتحليل JSON خطاً بخط |
| حدثت الواجهة في الوقت الحقيقي | عينت أنواع الرسائل إلى تحديثات DOM (شارات، نص، لوحات قابلة للطي) |

---

## النقاط الرئيسية

1. يمكن لـ **واجهة أمامية ثابتة مشتركة** العمل مع أي خلفية تتحدث نفس بروتوكول البث، مما يعزز قيمة نمط API المتوافق مع OpenAI.
2. **JSON مفصول بأسطر جديدة (NDJSON)** هو تنسيق بث بسيط يعمل بصورة أصلية مع API `ReadableStream` في المتصفح.
3. احتاجت **نسخة Python** أقل تغييرات لأنها تحتوي بالفعل على نقطة نهاية FastAPI؛ أما نسخ JavaScript وC# فقد احتاجت إلى غلاف HTTP رقيق.
4. الحفاظ على الواجهة كـ **HTML/CSS/JS خام** يتجنب أدوات البناء، تبعيات الأطر، وتعقيد إضافي لمتعلمي الورشة.
5. يتم إعادة استخدام نفس وحدات الوكيل (الباحث، المنتج، الكاتب، المحرر) دون تعديل؛ فقط تغير طبقة النقل.

---

## للقراءة الإضافية

| المورد | الرابط |
|----------|------|
| MDN: استخدام التدفقات القابلة للقراءة | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI الملفات الثابتة | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core الملفات الثابتة | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| مواصفة NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

تابع إلى [الجزء 13: إتمام الورشة](part13-workshop-complete.md) لملخص كل ما بنيته طوال هذه الورشة.

---
[← الجزء 11: استدعاء الأداة](part11-tool-calling.md) | [الجزء 13: انتهاء ورشة العمل →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**تنويه**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى لتحقيق الدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار المستند الأصلي بلغته الأصلية المصدر المعتمد. للمعلومات الحرجة، يُنصح بالاعتماد على الترجمة المهنية البشرية. نحن غير مسؤولين عن أي سوء فهم أو تفسيرات خاطئة ناشئة عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->