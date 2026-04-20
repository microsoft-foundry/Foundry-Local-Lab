![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# حصہ 12: زاوہ کریئیٹو رائٹر کے لیے ویب UI تیار کرنا

> **مقصد:** زاوہ کریئیٹو رائٹر کے لیے ایک براؤزر پر مبنی فرنٹ اینڈ شامل کرنا تاکہ آپ ملٹی-ایجنٹ پائپ لائن کو حقیقی وقت میں دیکھ سکیں، ایجنٹ کے اسٹیٹس کے بیجز لائیو اور آرٹیکل کا متن اسٹریم ہو رہا ہو، اور یہ سب ایک مقامی ویب سرور سے پیش کیا جائے۔

[حصہ 7](part7-zava-creative-writer.md) میں آپ نے زاوہ کریئیٹو رائٹر کو **CLI ایپلیکیشن** (جاوا اسکرپٹ، سی#) اور ایک **ہیڈلیس API** (پائتھون) کے طور پر دریافت کیا تھا۔ اس لیب میں آپ ایک مشترکہ **ونٹیلا HTML/CSS/جاوا اسکرپٹ** فرنٹ اینڈ کو ہر بیک اینڈ کے ساتھ منسلک کریں گے تاکہ صارفین پائپ لائن کے ساتھ براؤزر کے ذریعے تعامل کر سکیں نہ کہ ٹرمینل سے۔

---

## آپ کیا سیکھیں گے

| مقصد | وضاحت |
|-----------|-------------|
| بیک اینڈ سے جامد فائلیں فراہم کرنا | اپنی API روٹ کے ساتھ HTML/CSS/JS ڈائریکٹری ماؤنٹ کریں |
| براؤزر میں اسٹریمنگ NDJSON استعمال کرنا | `ReadableStream` کے ساتھ Fetch API استعمال کریں تاکہ نیو لائن-ڈیلیمیٹڈ JSON پڑھا جا سکے |
| متحدہ اسٹریمنگ پروٹوکول | یقینی بنائیں کہ پائتھون، جاوا اسکرپٹ، اور سی# بیک اینڈز ایک ہی میسج فارمیٹ جاری کریں |
| تدریجی UI اپ ڈیٹس | ایجنٹ اسٹیٹس بیجز اپ ڈیٹ کریں اور آرٹیکل کے متن کو ٹوکن بہ ٹوکن اسٹریم کریں |
| CLI ایپ میں HTTP لئیر شامل کرنا | موجودہ آرکسٹریٹر لاجک کو Express-سٹائل سرور (JS) یا ASP.NET Core minimal API (C#) میں لپیٹیں |

---

## فن تعمیر

UI ایک سیٹ جامد فائلوں کا ہے (`index.html`, `style.css`, `app.js`) جو تینوں بیک اینڈز کے لیے مشترک ہے۔ ہر بیک اینڈ ایک ہی دو روٹس ظاہر کرتا ہے:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| روٹ | طریقہ | مقصد |
|-------|--------|---------|
| `/` | GET | جامد UI فراہم کرتا ہے |
| `/api/article` | POST | ملٹی-ایجنٹ پائپ لائن چلائیں اور NDJSON اسٹریم کریں |

فرنٹ اینڈ JSON باڈی بھیجتا ہے اور جواب کو نیو لائن-ڈیلیمیٹڈ JSON پیغامات کے اسٹریم کے طور پر پڑھتا ہے۔ ہر پیغام میں ایک `type` فیلڈ ہوتا ہے جو UI درست پینل کو اپ ڈیٹ کرنے کے لیے استعمال کرتا ہے:

| میسج کی قسم | مطلب |
|-------------|---------|
| `message` | اسٹیٹس اپ ڈیٹ (مثلاً "ریسرچر ایجنٹ ٹاسک شروع ہو رہا ہے...") |
| `researcher` | تحقیقی نتائج تیار ہیں |
| `marketing` | پروڈکٹ سرچ کے نتائج تیار ہیں |
| `writer` | رائٹر شروع یا مکمل ہوا (محتوی `{ start: true }` یا `{ complete: true }`) |
| `partial` | رائٹر سے ایک سنگل اسٹریم شدہ ٹوکن (محتوی `{ text: "..." }`) |
| `editor` | ایڈیٹر کا فیصلہ تیار ہے |
| `error` | کچھ غلط ہو گیا |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## لازمی شرائط

- [حصہ 7: زاوہ کریئیٹو رائٹر](part7-zava-creative-writer.md) مکمل کریں
- Foundry Local CLI انسٹال کریں اور `phi-3.5-mini` ماڈل ڈاؤن لوڈ کریں
- ایک جدید ویب براؤزر (کروم، ایج، فائرفوکس، یا سفاری)

---

## مشترکہ UI

کسی بھی بیک اینڈ کو چھوئے بغیر، فرنٹ اینڈ کو دریافت کرنے کے لیے ایک لمحہ لیں جو تینوں زبان کے راستے استعمال کریں گے۔ فائلیں `zava-creative-writer-local/ui/` میں موجود ہیں:

| فائل | مقصد |
|------|---------|
| `index.html` | صفحہ ترتیب: ان پٹ فارم، ایجنٹ اسٹیٹس بیجز، آرٹیکل آؤٹ پٹ ایریا، کولیپس ہونے والے تفصیلی پینلز |
| `style.css` | حد سے کم اسٹائلنگ کے ساتھ اسٹیٹس بیج کلر اسٹیٹس (انتظار، چل رہا، مکمل، خرابی) |
| `app.js` | Fetch کال، `ReadableStream` لائن ریڈر، اور DOM اپ ڈیٹ لاجک |

> **مشورہ:** `index.html` کو براہ راست اپنے براؤزر میں کھولیں تاکہ ترتیب کا جائزہ لیں۔ ابھی کچھ کام نہیں کرے گا کیونکہ بیک اینڈ نہیں ہے، لیکن اسٹرکچر دیکھ سکتے ہیں۔

### اسٹریم ریڈر کس طرح کام کرتا ہے

`app.js` میں کلیدی فنکشن جواب کے جسم کو ٹکڑوں میں پڑھتا ہے اور نیو لائن باؤنڈری پر تقسیم کرتا ہے:

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
    buffer = lines.pop(); // نامکمل آخری لائن کو برقرار رکھیں

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

ہر پارس کیا گیا پیغام `handleMessage()` کو بھیجا جاتا ہے، جو `msg.type` کے مطابق متعلقہ DOM عنصر کو اپ ڈیٹ کرتا ہے۔

---

## مشقیں

### مشق 1: Python بیک اینڈ کو UI کے ساتھ چلائیں

Python (FastAPI) ورژن میں پہلے سے اسٹریمنگ API اینڈپوائنٹ موجود ہے۔ صرف تبدیلی `ui/` فولڈر کو جامد فائلوں کے طور پر ماؤنٹ کرنا ہے۔

**1.1** Python API ڈائریکٹری میں جائیں اور dependencies انسٹال کریں:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** سرور شروع کریں:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** براؤزر میں `http://localhost:8000` کھولیں۔ آپ کو زاوہ کریئیٹو رائٹر UI تین ٹیکسٹ فیلڈز اور "Generate Article" بٹن کے ساتھ نظر آئے گی۔

**1.4** ڈیفالٹ ویلیوز کے ساتھ **Generate Article** پر کلک کریں۔ ایجنٹ اسٹیٹس بیجز "Waiting" سے "Running" اور پھر "Done" میں تبدیل ہوتے ہوئے دیکھیں جب ہر ایجنٹ اپنا کام مکمل کرتا ہے، اور آرٹیکل کا متن ٹوکن بہ ٹوکن آؤٹ پٹ پینل میں اسٹریم ہوتا ہے۔

> **مسائل کی صورت میں:** اگر صفحہ UI کی جگہ JSON جواب دکھاتا ہے تو یقینی بنائیں کہ آپ تازہ ترین `main.py` چلا رہے ہیں جو جامد فائلز ماؤنٹ کرتا ہے۔ `/api/article` اینڈپوائنٹ اپنے اصلی راستے پر کام کرتا رہے گا؛ جامد فائل ماؤنٹ ہر دوسرے روٹ پر UI فراہم کرتا ہے۔

**یہ کیسے کام کرتا ہے:** تازہ ترین `main.py` نیچے ایک لائن شامل کرتا ہے:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

یہ `zava-creative-writer-local/ui/` سے ہر فائل کو جامد اثاثے کے طور پر فراہم کرتا ہے، جس میں `index.html` ڈیفالٹ دستاویز ہے۔ `/api/article` POST روٹ جامد ماؤنٹ سے پہلے رجسٹر ہوتا ہے اس لیے اسے ترجیح دی جاتی ہے۔

---

### مشق 2: جاوا اسکرپٹ ورژن میں ویب سرور شامل کریں

جاوا اسکرپٹ ورژن فی الحال CLI ایپلیکیشن (`main.mjs`) ہے۔ ایک نئی فائل، `server.mjs`، وہی ایجنٹ ماڈیول HTTP سرور کے پیچھے لپیٹتی ہے اور مشترکہ UI فراہم کرتی ہے۔

**2.1** جاوا اسکرپٹ ڈائریکٹری میں جائیں اور dependencies انسٹال کریں:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** ویب سرور شروع کریں:

```bash
node server.mjs
```

```powershell
node server.mjs
```

آپ کو دکھائی دے گا:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** براؤزر میں `http://localhost:3000` کھولیں اور **Generate Article** پر کلک کریں۔ یہی UI جاوا اسکرپٹ بیک اینڈ کے خلاف بالکل ویسا ہی کام کرتا ہے۔

**کوڈ کا مطالعہ کریں:** `server.mjs` کھولیں اور کلیدی انداز دیکھیں:

- **جامد فائل خدمت** Node.js کے بلٹ-ان `http`, `fs`, اور `path` ماڈیولز کے ساتھ کی جاتی ہے بغیر کسی بیرونی فریم ورک کے۔
- **راستے کی گردش سے تحفظ** درخواست کیے گئے راستے کو نارملائز کرتا ہے اور تصدیق کرتا ہے کہ وہ `ui/` ڈائریکٹری کے اندر ہی رہے۔
- **NDJSON اسٹریمنگ** ایک `sendLine()` ہیلپر استعمال کرتا ہے جو ہر آبجیکٹ کو سیریلائز کرتا ہے، اندرونی نیو لائنز ہٹاتا ہے، اور ایک trailing newline لگاتا ہے۔
- **ایجنٹ آرکسٹریشن** موجودہ `researcher.mjs`, `product.mjs`, `writer.mjs`, اور `editor.mjs` ماڈیولز کو بغیر تبدیلی کے دوبارہ استعمال کرتا ہے۔

<details>
<summary>server.mjs سے کلیدی اقتباس</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// محقق
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// مصنف (براہ راست نشریات)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### مشق 3: سی# ورژن میں Minimal API شامل کریں

سی# ورژن فی الحال کنسول ایپلیکیشن ہے۔ ایک نیا پروجیکٹ، `csharp-web`, ASP.NET Core minimal APIs استعمال کرتا ہے تاکہ وہی پائپ لائن ویب سروس کے طور پر پیش کی جا سکے۔

**3.1** سی# ویب پروجیکٹ میں جائیں اور پیکجز بحال کریں:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** ویب سرور چلائیں:

```bash
dotnet run
```

```powershell
dotnet run
```

آپ کو دکھائی دے گا:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** براؤزر میں `http://localhost:5000` کھولیں اور **Generate Article** پر کلک کریں۔

**کوڈ کا مطالعہ کریں:** `csharp-web` ڈائریکٹری میں `Program.cs` کھولیں اور نوٹ کریں:

- پروجیکٹ فائل `Microsoft.NET.Sdk.Web` استعمال کرتی ہے جو ASP.NET Core کی حمایت شامل کرتی ہے، `Microsoft.NET.Sdk` کی جگہ۔
- جامد فائلیں `UseDefaultFiles` اور `UseStaticFiles` کے ذریعہ مشترکہ `ui/` ڈائریکٹری کی طرف فراہم کی جاتی ہیں۔
- `/api/article` اینڈپوائنٹ NDJSON لائنز براہ راست `HttpContext.Response` کو لکھتا ہے اور ہر لائن کے بعد فوراً فلش کرتا ہے تاکہ ریئل ٹائم اسٹریمنگ ہو۔
- تمام ایجنٹ لاجک (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) کنسول ورژن جیسا ہی ہے۔

<details>
<summary>csharp-web/Program.cs سے کلیدی اقتباس</summary>

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

### مشق 4: ایجنٹ اسٹیٹس بیجز کا جائزہ لیں

اب جب کہ آپ کے پاس کام کرنے والا UI ہے، دیکھیں کہ فرنٹ اینڈ اسٹیٹس بیجز کو کیسے اپ ڈیٹ کرتا ہے۔

**4.1** `zava-creative-writer-local/ui/app.js` اپنے ایڈیٹر میں کھولیں۔

**4.2** `handleMessage()` فنکشن تلاش کریں۔ دیکھیں کہ یہ کس طرح میسج کی اقسام کو DOM اپ ڈیٹس کے ساتھ میپ کرتا ہے:

| میسج کی قسم | UI عمل |
|-------------|-----------|
| `message` جس میں "researcher" شامل ہو | ریسرچر بیج کو "Running" پر سیٹ کرتا ہے |
| `researcher` | ریسرچر بیج کو "Done" پر سیٹ کرتا ہے اور ریسرچ رزلٹس پینل بھر دیتا ہے |
| `marketing` | پروڈکٹ سرچ بیج کو "Done" پر سیٹ کرتا ہے اور پروڈکٹ میچز پینل بھر دیتا ہے |
| `writer` جس میں `data.start` ہو | رائٹر بیج کو "Running" پر سیٹ کرتا ہے اور آرٹیکل آؤٹ پٹ صاف کرتا ہے |
| `partial` | ٹوکن کے متن کو آرٹیکل آؤٹ پٹ میں شامل کرتا ہے |
| `writer` جس میں `data.complete` ہو | رائٹر بیج کو "Done" پر سیٹ کرتا ہے |
| `editor` | ایڈیٹر بیج کو "Done" پر سیٹ کرتا ہے اور ایڈیٹر فیڈبیک پینل بھر دیتا ہے |

**4.3** آرٹیکل کے نیچے واقع کولیپس ہونے والے "Research Results", "Product Matches", اور "Editor Feedback" پینلز کھولیں تاکہ ہر ایجنٹ کی پیدا کردہ خام JSON دیکھ سکیں۔

---

### مشق 5: UI کو حسبِ منشا بنائیں (اختیاری)

ان میں سے ایک یا زیادہ اضافہ آزما کر دیکھیں:

**5.1 ایک لفظی شمار شامل کریں۔** جب رائٹر مکمل ہو جائے، تو آرٹیکل ورڈ کاؤنٹ آؤٹ پٹ پینل کے نیچے دکھائیں۔ آپ `handleMessage` میں اس کو کمپیوٹ کر سکتے ہیں جب `type === "writer"` اور `data.complete` true ہو:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 ری ٹرائی انڈی کیٹر شامل کریں۔** جب ایڈیٹر ترمیم کی درخواست کرے، پائپ لائن دوبارہ چلتی ہے۔ اسٹیٹس پینل میں "Revision 1" یا "Revision 2" بینر دکھائیں۔ `message` قسم جو "Revision" پر مشتمل ہو سنیں اور ایک نیا DOM عنصر اپ ڈیٹ کریں۔

**5.3 ڈارک موڈ۔** ایک ٹوگل بٹن اور `<body>` کو `.dark` کلاس شامل کریں۔ `style.css` میں `body.dark` سیلیکٹر کے ساتھ بیک گراؤنڈ، متن، اور پینل کے رنگ اوور رائیڈ کریں۔

---

## خلاصہ

| آپ نے کیا کیا | کیسے |
|-------------|-----|
| Python بیک اینڈ سے UI فراہم کیا | FastAPI میں `StaticFiles` کے ساتھ `ui/` فولڈر ماؤنٹ کیا |
| جاوا اسکرپٹ ورژن میں HTTP سرور شامل کیا | Node.js کے بلٹ ان `http` ماڈیول کے ساتھ `server.mjs` بنایا |
| سی# ورژن میں ویب API شامل کیا | ASP.NET Core minimal APIs کے ساتھ نیا `csharp-web` پروجیکٹ بنایا |
| براؤزر میں اسٹریمنگ NDJSON استعمال کی | `fetch()` کے ساتھ `ReadableStream` اور لائن بہ لائن JSON پارسنگ کی |
| UI کو حقیقی وقت میں اپ ڈیٹ کیا | میسج اقسام کو DOM اپ ڈیٹس (بیجز، متن، کولیپس پینلز) کے ساتھ میپ کیا |

---

## بنیادی نکات

1. ایک **مشترکہ جامد فرنٹ اینڈ** کسی بھی بیک اینڈ کے ساتھ کام کر سکتا ہے جو ایک ہی اسٹریمنگ پروٹوکول بولتا ہو، جس سے OpenAI-مطابقت API پیٹرن کی اہمیت مضبوط ہوتی ہے۔
2. **نیو لائن-ڈیلیمیٹڈ JSON (NDJSON)** ایک آسان اسٹریم فارمیٹ ہے جو براؤزر کے `ReadableStream` API کے ساتھ فطری طور پر کام کرتا ہے۔
3. **Python ورژن** کو سب سے کم تبدیلی کی ضرورت تھی کیونکہ اس کے پاس پہلے ہی FastAPI اینڈپوائنٹ تھا؛ جاوا اسکرپٹ اور C# ورژنز کو ہلکا HTTP ریپر چاہیے تھا۔
4. UI کو **ونٹیلا HTML/CSS/JS** رکھنے سے بلڈ ٹولز، فریم ورک انحصارات، اور ورکشاپ سیکھنے والوں کے لیے اضافی پیچیدگی سے بچا جاتا ہے۔
5. وہی ایجنٹ ماڈیولز (ریسرچر، پروڈکٹ، رائٹر، ایڈیٹر) بغیر کسی ترمیم کے دوبارہ استعمال ہوتے ہیں؛ صرف ٹرانسپورٹ لئیر بدلی ہے۔

---

## مزید مطالعہ

| ذریعہ | لنک |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

ورکشاپ میں آپ کے بنائے ہوئے تمام کام کا خلاصہ حاصل کرنے کے لیے [حصہ 13: ورکشاپ مکمل](part13-workshop-complete.md) پر جائیں۔

---
[← حصہ 11: ٹول کالنگ](part11-tool-calling.md) | [حصہ 13: ورکشاپ مکمل →](part13-workshop-complete.md)