![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ส่วนที่ 12: การสร้างเว็บ UI สำหรับ Zava Creative Writer

> **เป้าหมาย:** เพิ่มส่วนหน้าเว็บบนเบราว์เซอร์ให้กับ Zava Creative Writer เพื่อให้คุณสามารถดูการทำงานของ multi-agent pipeline แบบเรียลไทม์ พร้อมกับแสดงสถานะตัวแทนและข้อความบทความที่สตรีมออกมา โดยทั้งหมดให้บริการจากเว็บเซิร์ฟเวอร์ท้องถิ่นเพียงตัวเดียว

ใน [ส่วนที่ 7](part7-zava-creative-writer.md) คุณได้สำรวจ Zava Creative Writer ในรูปแบบ **CLI application** (JavaScript, C#) และ **headless API** (Python) ในห้องปฏิบัติการนี้ คุณจะเชื่อมต่อส่วนหน้าแบบ **vanilla HTML/CSS/JavaScript** ร่วมกับแต่ละ backend เพื่อให้ผู้ใช้สามารถโต้ตอบกับ pipeline ผ่านเบราว์เซอร์แทนการใช้เทอร์มินัล

---

## สิ่งที่คุณจะได้เรียนรู้

| วัตถุประสงค์ | คำอธิบาย |
|-----------|-------------|
| ให้บริการไฟล์สแตติกจาก backend | ติดตั้งโฟลเดอร์ HTML/CSS/JS ควบคู่กับเส้นทาง API ของคุณ |
| ใช้การสตรีม NDJSON ในเบราว์เซอร์ | ใช้ Fetch API กับ `ReadableStream` เพื่ออ่าน JSON ที่คั่นด้วยบรรทัดใหม่ |
| โปรโตคอลสตรีมมิ่งแบบครบวงจร | รับประกันว่า backend Python, JavaScript, และ C# ส่งข้อความในรูปแบบเดียวกัน |
| อัปเดต UI อย่างก้าวหน้า | อัปเดตสถานะตัวแทนและสตรีมข้อความบทความทีละโทเคน |
| เพิ่มเลเยอร์ HTTP ให้กับแอป CLI | ห่อหุ้มตรรกะ orchestrator ที่มีอยู่ในเซิร์ฟเวอร์สไตล์ Express (JS) หรือ ASP.NET Core minimal API (C#) |

---

## สถาปัตยกรรม

UI คือชุดไฟล์สแตติกเดียวกัน (`index.html`, `style.css`, `app.js`) ใช้ร่วมกันโดยทั้งสาม backend แต่ละ backend จะเปิดเผยเส้นทางสองเส้นทางเหมือนกัน:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| เส้นทาง | วิธีการ | จุดประสงค์ |
|-------|--------|---------|
| `/` | GET | ให้บริการ UI สแตติก |
| `/api/article` | POST | รัน multi-agent pipeline และสตรีม NDJSON |

ส่วนหน้ายื่นร่างข้อความ JSON และอ่านคำตอบเป็นสตรีมของข้อความ JSON คั่นด้วยบรรทัดใหม่ แต่ละข้อความมีฟิลด์ `type` ที่ UI ใช้เพื่ออัปเดตแผงควบคุมที่ถูกต้อง:

| ประเภทข้อความ | ความหมาย |
|-------------|---------|
| `message` | การอัปเดตสถานะ (เช่น "เริ่มงานตัวแทนผู้วิจัย...") |
| `researcher` | ผลลัพธ์การวิจัยพร้อมใช้งาน |
| `marketing` | ผลการค้นหาผลิตภัณฑ์พร้อมใช้งาน |
| `writer` | นักเขียนเริ่มหรือเสร็จสิ้น (ประกอบด้วย `{ start: true }` หรือ `{ complete: true }`) |
| `partial` | โทเคนเดียวที่สตรีมจากนักเขียน (ประกอบด้วย `{ text: "..." }`) |
| `editor` | ผลสรุปของบรรณาธิการพร้อมใช้งาน |
| `error` | มีบางอย่างผิดพลาด |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## ข้อกำหนดเบื้องต้น

- ทำ [ส่วนที่ 7: Zava Creative Writer](part7-zava-creative-writer.md) เสร็จแล้ว
- ติดตั้ง Foundry Local CLI และดาวน์โหลดโมเดล `phi-3.5-mini`
- ใช้เว็บเบราว์เซอร์สมัยใหม่ (Chrome, Edge, Firefox, หรือ Safari)

---

## UI ร่วมกัน

ก่อนแก้ไขโค้ด backend ใดๆ ให้ใช้เวลาสำรวจส่วนหน้าที่ใช้ร่วมกันของทั้งสามภาษา ไฟล์เหล่านี้อยู่ในโฟลเดอร์ `zava-creative-writer-local/ui/`:

| ไฟล์ | จุดประสงค์ |
|------|---------|
| `index.html` | เค้าโครงหน้า: ฟอร์มป้อนข้อมูล, แสดงสถานะตัวแทน, พื้นที่แสดงบทความ, แผงรายละเอียดที่ย่อ/ขยายได้ |
| `style.css` | การจัดแต่งสไตล์ขั้นต่ำพร้อมสถานะสีของบัตรสถานะ (รอ, กำลังทำงาน, เสร็จ, ข้อผิดพลาด) |
| `app.js` | การเรียก fetch, ตัวอ่านแถว `ReadableStream` และตรรกะอัปเดต DOM |

> **คำแนะนำ:** เปิด `index.html` โดยตรงในเบราว์เซอร์เพื่อดูตัวอย่างเค้าโครง ยังไม่มีอะไรทำงานเพราะยังไม่มี backend แต่คุณจะเห็นโครงสร้าง

### วิธีทำงานของตัวอ่านสตรีม

ฟังก์ชันสำคัญใน `app.js` อ่านตัวตอบสนองทีละชิ้นแล้วแยกตามขอบเขตบรรทัดใหม่:

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
    buffer = lines.pop(); // เก็บบรรทัดที่ตามมาซึ่งไม่สมบูรณ์ไว้

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

ข้อความที่ถูกแยกแล้วแต่ละข้อความจะถูกส่งไปที่ `handleMessage()` ซึ่งจะอัปเดตองค์ประกอบ DOM ที่เกี่ยวข้องตาม `msg.type`

---

## แบบฝึกหัด

### แบบฝึกหัด 1: รัน Backend Python พร้อม UI

ตัวเลือก Python (FastAPI) มี endpoint สตรีมมิ่ง API แล้ว การเปลี่ยนแปลงต้องทำเพียงการติดตั้งโฟลเดอร์ `ui/` เป็นไฟล์สแตติกเท่านั้น

**1.1** ไปที่ไดเรกทอรี Python API และติดตั้ง dependencies:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** เริ่มเซิร์ฟเวอร์:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** เปิดเบราว์เซอร์ที่ `http://localhost:8000` คุณจะเห็น UI ของ Zava Creative Writer ที่มีสามช่องข้อความและปุ่ม "Generate Article"

**1.4** คลิก **Generate Article** โดยใช้ค่าเริ่มต้น ดูบัตรสถานะตัวแทนเปลี่ยนจาก "Waiting" เป็น "Running" เป็น "Done" ตามที่ตัวแทนแต่ละตัวทำงานเสร็จ และดูข้อความบทความสตรีมเข้าแผงผลลัพธ์ทีละโทเคน

> **แก้ไขปัญหา:** หากหน้าแสดงผลเป็น JSON แทน UI ให้แน่ใจว่าคุณใช้ `main.py` ที่อัปเดตแล้วซึ่งติดตั้งไฟล์สแตติก endpoint `/api/article` ยังใช้งานได้ตามเส้นทางเดิม ตัวติดตั้งไฟล์สแตติกให้บริการ UI บนเส้นทางอื่นทั้งหมด

**วิธีการทำงาน:** `main.py` ที่อัปเดตเพิ่มบรรทัดเดียวที่ด้านล่าง:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

นี้ให้บริการไฟล์ทั้งหมดจาก `zava-creative-writer-local/ui/` เป็นไฟล์สแตติก โดย `index.html` เป็นเอกสารเริ่มต้น endpoint POST `/api/article` ลงทะเบียนก่อนการติดตั้งสแตติก จึงมีความสำคัญสูงกว่า

---

### แบบฝึกหัด 2: เพิ่มเว็บเซิร์ฟเวอร์ให้ตัวเลือก JavaScript

ตัวเลือก JavaScript ปัจจุบันเป็นแอป CLI (`main.mjs`) ไฟล์ใหม่ `server.mjs` ห่อหุ้มโมดูลตัวแทนเดิมหลังเซิร์ฟเวอร์ HTTP และให้บริการ UI ร่วมกัน

**2.1** ไปที่ไดเรกทอรี JavaScript และติดตั้ง dependencies:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** เริ่มเว็บเซิร์ฟเวอร์:

```bash
node server.mjs
```

```powershell
node server.mjs
```

คุณจะเห็น:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** เปิด `http://localhost:3000` ในเบราว์เซอร์แล้วคลิก **Generate Article** UI เหมือนกันนี้ทำงานอย่างเดียวกับ backend JavaScript

**ศึกษารหัส:** เปิดไฟล์ `server.mjs` และสังเกตแพตเทิร์นสำคัญ:

- การให้บริการไฟล์สแตติกใช้โมดูลในตัวของ Node.js `http`, `fs`, และ `path` โดยไม่ต้องใช้เฟรมเวิร์กภายนอก
- การป้องกัน path-traversal ปกติไวยากรณ์เส้นทางที่ร้องขอและยืนยันว่าอยู่ในไดเรกทอรี `ui/`
- การสตรีม NDJSON ใช้ฟังก์ชันช่วย `sendLine()` ที่ทำการซีเรียลไลส์แต่ละอ็อบเจ็กต์ ลบบรรทัดใหม่ภายใน แล้วต่อด้วยบรรทัดใหม่สุดท้าย
- การออร์เคสตราเตอร์ตัวแทนยังใช้โมดูล `researcher.mjs`, `product.mjs`, `writer.mjs`, และ `editor.mjs` ที่ไม่เปลี่ยนแปลง

<details>
<summary>ตัวอย่างสำคัญจาก server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// นักวิจัย
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// นักเขียน (สตรีมมิง)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### แบบฝึกหัด 3: เพิ่ม Minimal API ในตัวเลือก C#

ตัวเลือก C# ปัจจุบันเป็นแอปคอนโซล โปรเจคใหม่ `csharp-web` ใช้ ASP.NET Core minimal APIs เพื่อเปิดเผย pipeline เดียวกันเป็นเว็บเซอร์วิส

**3.1** ไปที่โปรเจค C# เว็บและ Restore Packages:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** รันเว็บเซิร์ฟเวอร์:

```bash
dotnet run
```

```powershell
dotnet run
```

คุณจะเห็น:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** เปิด `http://localhost:5000` ในเบราว์เซอร์และคลิก **Generate Article**

**ศึกษารหัส:** เปิดไฟล์ `Program.cs` ในไดเรกทอรี `csharp-web` และสังเกต:

- ไฟล์โปรเจคใช้ `Microsoft.NET.Sdk.Web` แทน `Microsoft.NET.Sdk` ซึ่งเพิ่มการสนับสนุน ASP.NET Core
- ไฟล์สแตติกให้บริการผ่าน `UseDefaultFiles` และ `UseStaticFiles` โดยชี้ไปที่โฟลเดอร์ `ui/` ร่วมกัน
- Endpoint `/api/article` เขียนไลน์ NDJSON ลงใน `HttpContext.Response` โดยตรงและ flush หลังแต่ละบรรทัดเพื่อสตรีมแบบเรียลไทม์
- ตรรกะตัวแทนทั้งหมด (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) เหมือนกับเวอร์ชันคอนโซล

<details>
<summary>ตัวอย่างสำคัญจาก csharp-web/Program.cs</summary>

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

### แบบฝึกหัด 4: สำรวจบัตรสถานะตัวแทน

เมื่อคุณมี UI ที่ใช้งานได้แล้ว ให้ดูว่าหน้าส่วนหน้าจะอัปเดตบัตรสถานะอย่างไร

**4.1** เปิด `zava-creative-writer-local/ui/app.js` ในตัวแก้ไข

**4.2** หา `handleMessage()` ฟังก์ชันสังเกตว่ามันแมปประเภทข้อความกับการอัปเดต DOM อย่างไร:

| ประเภทข้อความ | การทำงานใน UI |
|-------------|-----------|
| `message` ที่มีคำว่า "researcher" | ตั้ง badge Researcher เป็น "Running" |
| `researcher` | ตั้ง badge Researcher เป็น "Done" และเติมข้อมูลในแผง Research Results |
| `marketing` | ตั้ง badge Product Search เป็น "Done" และเติมข้อมูลในแผง Product Matches |
| `writer` มี `data.start` | ตั้ง badge Writer เป็น "Running" และล้างข้อความบทความในแผงผลลัพธ์ |
| `partial` | ต่อข้อความโทเคนเข้ากับแผงบทความ |
| `writer` มี `data.complete` | ตั้ง badge Writer เป็น "Done" |
| `editor` | ตั้ง badge Editor เป็น "Done" และเติมข้อมูลในแผง Editor Feedback |

**4.3** เปิดแผงย่อ/ขยาย "Research Results", "Product Matches", และ "Editor Feedback" ด้านล่างบทความเพื่อตรวจสอบ JSON ต้นฉบับที่แต่ละตัวแทนสร้างขึ้น

---

### แบบฝึกหัด 5: ปรับแต่ง UI (ตัวเลือกเสริม)

ลองทำหนึ่งหรือหลายรายการต่อไปนี้:

**5.1 เพิ่มจำนวนคำ** หลังจาก Writer เสร็จ ให้แสดงจำนวนคำของบทความใต้แผงแสดงผล คุณสามารถคำนวณนี้ใน `handleMessage` เมื่อ `type === "writer"` และ `data.complete` เป็นจริง:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 เพิ่มตัวบ่งชี้ retry** เมื่อ Editor ร้องขอการแก้ไข pipeline จะรันใหม่ แสดงแบนเนอร์ "Revision 1" หรือ "Revision 2" ในแผงสถานะ ฟังข้อความประเภท `message` ที่มีคำว่า "Revision" และอัปเดตองค์ประกอบ DOM ใหม่

**5.3 โหมดมืด** เพิ่มปุ่มสลับและคลาส `.dark` ให้กับ `<body>` เขียนทับสีพื้นหลัง, ข้อความ, และสีแผงใน `style.css` ด้วยตัวเลือก `body.dark`

---

## สรุป

| สิ่งที่คุณทำ | วิธีทำ |
|-------------|-----|
| ให้บริการ UI จาก backend Python | ติดตั้งโฟลเดอร์ `ui/` ด้วย `StaticFiles` ใน FastAPI |
| เพิ่มเว็บเซิร์ฟเวอร์ให้ตัวเลือก JavaScript | สร้าง `server.mjs` ใช้โมดูล `http` ในตัวของ Node.js |
| เพิ่มเว็บ API ให้ตัวเลือก C# | สร้างโปรเจค `csharp-web` ใหม่ด้วย ASP.NET Core minimal APIs |
| ใช้ streaming NDJSON ในเบราว์เซอร์ | ใช้ `fetch()` กับ `ReadableStream` และแยก JSON ทีละบรรทัด |
| อัปเดต UI แบบเรียลไทม์ | แมปประเภทข้อความกับการอัปเดต DOM (บัตรสถานะ, ข้อความ, แผงรายละเอียด) |

---

## ข้อสรุปสำคัญ

1. **ส่วนหน้าแบบ static ร่วมกัน** สามารถทำงานกับ backend ใดก็ได้ที่พูดโปรโตคอลสตรีมมิ่งเดียวกัน ยืนยันคุณค่าของแพตเทิร์น API ที่เข้ากันได้กับ OpenAI
2. **NDJSON (newline-delimited JSON)** เป็นรูปแบบสตรีมมิ่งที่เข้าใจง่ายและทำงานกับ API เบราว์เซอร์ `ReadableStream` ได้โดยตรง
3. ตัวเลือก **Python** ต้องการการเปลี่ยนแปลงน้อยที่สุดเพราะมี endpoint FastAPI แล้ว ส่วน JavaScript และ C# ต้องการห่อ HTTP ขนาดเล็ก
4. รักษา UI ให้เป็น **vanilla HTML/CSS/JS** เพื่อหลีกเลี่ยงเครื่องมือบิวด์, ขึ้นอยู่กับเฟรมเวิร์ก หรือความซับซ้อนเพิ่มเติมสำหรับผู้เรียนในเวิร์กช็อป
5. โมดูลตัวแทน (Researcher, Product, Writer, Editor) ใช้ซ้ำโดยไม่เปลี่ยนแปลง แค่เปลี่ยนชั้นการขนส่งเท่านั้น

---

## แหล่งข้อมูลเพิ่มเติม

| แหล่งข้อมูล | ลิงก์ |
|----------|------|
| MDN: การใช้ Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| ข้อกำหนด NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

ดำเนินการต่อไปยัง [ส่วนที่ 13: การสรุปเวิร์กช็อป](part13-workshop-complete.md) เพื่อสรุปสิ่งที่คุณสร้างในเวิร์กช็อปนี้ทั้งหมด

---
[← ส่วนที่ 11: การเรียกใช้เครื่องมือ](part11-tool-calling.md) | [ส่วนที่ 13: เสร็จสิ้นเวิร์กช็อป →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**คำปฏิเสธความรับผิดชอบ**:  
เอกสารนี้ได้รับการแปลโดยใช้บริการแปลภาษาด้วย AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้ว่าเราจะพยายามความถูกต้องอย่างเต็มที่ โปรดทราบว่าการแปลโดยอัตโนมัติอาจมีข้อผิดพลาดหรือความคลาดเคลื่อน เอกสารต้นฉบับในภาษาต้นฉบับควรถูกพิจารณาเป็นแหล่งข้อมูลที่เชื่อถือได้ สำหรับข้อมูลที่สำคัญ แนะนำให้ใช้การแปลโดยมนุษย์มืออาชีพ เราไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความที่ผิดพลาดใด ๆ ที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->