![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Phần 12: Xây dựng giao diện Web cho Zava Creative Writer

> **Mục tiêu:** Thêm giao diện front end trên trình duyệt cho Zava Creative Writer để bạn có thể theo dõi quy trình đa tác nhân chạy theo thời gian thực, với các huy hiệu trạng thái tác nhân trực tiếp và văn bản bài viết được stream, tất cả được phục vụ từ một máy chủ web cục bộ duy nhất.

Trong [Phần 7](part7-zava-creative-writer.md) bạn đã khám phá Zava Creative Writer dưới dạng **ứng dụng CLI** (JavaScript, C#) và một **API không giao diện** (Python). Trong bài lab này bạn sẽ kết nối một front end **vanilla HTML/CSS/JavaScript** chung tới mỗi backend sao cho người dùng có thể tương tác với pipeline qua trình duyệt thay vì qua terminal.

---

## Những gì bạn sẽ học

| Mục tiêu | Mô tả |
|-----------|-------------|
| Phục vụ các file tĩnh từ backend | Gắn một thư mục HTML/CSS/JS bên cạnh route API của bạn |
| Tiêu thụ stream NDJSON trên trình duyệt | Sử dụng Fetch API với `ReadableStream` để đọc JSON phân cách bằng dấu xuống dòng |
| Giao thức stream hợp nhất | Đảm bảo các backend Python, JavaScript, và C# phát sinh định dạng tin nhắn giống nhau |
| Cập nhật UI tiến trình | Cập nhật huy hiệu trạng thái tác nhân và stream văn bản bài viết token từng token |
| Thêm lớp HTTP cho ứng dụng CLI | Bao bọc logic điều phối hiện có trong server kiểu Express (JS) hoặc ASP.NET Core minimal API (C#) |

---

## Kiến trúc

Giao diện UI là một bộ file tĩnh (`index.html`, `style.css`, `app.js`) dùng chung cho tất cả ba backend. Mỗi backend cung cấp cùng hai route:

![Kiến trúc Zava UI — front end dùng chung với ba backend](../../../images/part12-architecture.svg)

| Route | Phương thức | Mục đích |
|-------|-------------|----------|
| `/` | GET | Phục vụ UI tĩnh |
| `/api/article` | POST | Chạy pipeline đa tác nhân và stream NDJSON |

Front end gửi một body JSON và đọc phản hồi dưới dạng luồng các tin nhắn JSON phân cách dòng. Mỗi tin nhắn có trường `type` mà UI dùng để cập nhật pane đúng:

| Loại tin nhắn | Ý nghĩa |
|---------------|---------|
| `message` | Cập nhật trạng thái (ví dụ: "Bắt đầu nhiệm vụ tác nhân nghiên cứu...") |
| `researcher` | Kết quả nghiên cứu đã sẵn sàng |
| `marketing` | Kết quả tìm kiếm sản phẩm đã sẵn sàng |
| `writer` | Tác giả bắt đầu hoặc hoàn thành (chứa `{ start: true }` hoặc `{ complete: true }`) |
| `partial` | Một token được stream từ Writer (chứa `{ text: "..." }`) |
| `editor` | Kết luận của biên tập viên đã sẵn sàng |
| `error` | Có lỗi xảy ra |

![Phân luồng các loại tin nhắn trên trình duyệt](../../../images/part12-message-types.svg)

![Chuỗi truyền stream — Giao tiếp giữa trình duyệt và Backend](../../../images/part12-streaming-sequence.svg)

---

## Yêu cầu trước

- Hoàn thành [Phần 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Cài đặt Foundry Local CLI và tải model `phi-3.5-mini`
- Một trình duyệt web hiện đại (Chrome, Edge, Firefox hoặc Safari)

---

## Giao diện UI dùng chung

Trước khi can thiệp code backend, hãy dành chút thời gian khám phá giao diện front end mà cả ba track ngôn ngữ sẽ sử dụng. Các file nằm trong `zava-creative-writer-local/ui/`:

| File | Mục đích |
|------|----------|
| `index.html` | Bố cục trang: form nhập liệu, huy hiệu trạng thái tác nhân, khu vực xuất bài viết, các panel chi tiết có thể thu gọn |
| `style.css` | Định dạng tối giản với các trạng thái màu huy hiệu (đang chờ, đang chạy, hoàn thành, lỗi) |
| `app.js` | Lời gọi fetch, đọc dòng `ReadableStream`, và logic cập nhật DOM |

> **Mẹo:** Mở trực tiếp `index.html` trong trình duyệt để xem trước bố cục. Tất cả sẽ chưa hoạt động vì chưa có backend, nhưng bạn có thể xem cấu trúc giao diện.

### Cách hoạt động của trình đọc Stream

Hàm chính trong `app.js` đọc từng khối dữ liệu của phản hồi và tách theo ranh giới xuống dòng:

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
    buffer = lines.pop(); // giữ lại dòng cuối bị thiếu hoàn chỉnh

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

Mỗi tin nhắn được phân tích cú pháp rồi chuyển đến `handleMessage()`, hàm này cập nhật phần tử DOM tương ứng dựa trên `msg.type`.

---

## Bài tập

### Bài tập 1: Chạy Backend Python với UI

Phiên bản Python (FastAPI) đã có endpoint API stream. Thay đổi duy nhất là gắn thư mục `ui/` thành file tĩnh.

**1.1** Chuyển tới thư mục API Python và cài đặt phụ thuộc:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Khởi động server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Mở trình duyệt vào `http://localhost:8000`. Bạn sẽ thấy UI Zava Creative Writer với ba trường văn bản và nút "Generate Article".

**1.4** Nhấn **Generate Article** với các giá trị mặc định. Quan sát các huy hiệu trạng thái tác nhân chuyển từ "Waiting" sang "Running" rồi "Done" khi mỗi tác nhân hoàn thành nhiệm vụ, và xem luồng văn bản bài viết xuất ra từng token trong panel kết quả.

> **Khắc phục sự cố:** Nếu trang hiển thị phản hồi JSON thay vì UI, hãy chắc chắn bạn đang chạy `main.py` đã cập nhật để gắn các file tĩnh. Endpoint `/api/article` vẫn hoạt động tại đường dẫn gốc; mount file tĩnh phục vụ UI ở mọi route còn lại.

**Cách hoạt động:** `main.py` được cập nhật thêm một dòng ở cuối:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Dòng này phục vụ tất cả file trong `zava-creative-writer-local/ui/` như tài nguyên tĩnh, với `index.html` làm tài liệu mặc định. Route POST `/api/article` được đăng ký trước route static, cho nên có ưu tiên cao hơn.

---

### Bài tập 2: Thêm Web Server cho biến thể JavaScript

Biến thể JavaScript hiện là ứng dụng CLI (`main.mjs`). Một file mới, `server.mjs`, bao bọc các module tác nhân phía sau một server HTTP và phục vụ UI dùng chung.

**2.1** Điều hướng tới thư mục JavaScript và cài phụ thuộc:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Khởi động web server:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Bạn sẽ thấy:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Mở `http://localhost:3000` trên trình duyệt và nhấn **Generate Article**. Giao diện UI cùng một cách với backend JavaScript.

**Nghiên cứu code:** Mở `server.mjs` và lưu ý các mẫu chính:

- **Phục vụ file tĩnh** dùng các module tích hợp `http`, `fs`, `path` của Node.js mà không dùng framework ngoài.
- **Bảo vệ truy xuất đường dẫn** chuẩn hóa đường dẫn yêu cầu và kiểm tra nó nằm trong thư mục `ui/`.
- **Stream NDJSON** dùng hàm trợ giúp `sendLine()` tuần tự hóa mỗi object, loại bỏ newline nội bộ và thêm newline cuối.
- **Điều phối tác nhân** tái sử dụng các module `researcher.mjs`, `product.mjs`, `writer.mjs`, và `editor.mjs` nguyên trạng.

<details>
<summary>Trích đoạn chính từ server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Nhà nghiên cứu
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Người viết (phát trực tiếp)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Bài tập 3: Thêm Minimal API cho biến thể C#

Biến thể C# hiện là ứng dụng console. Dự án mới, `csharp-web`, dùng ASP.NET Core minimal APIs để cung cấp pipeline như dịch vụ web.

**3.1** Điều hướng đến dự án web C# và phục hồi package:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Chạy web server:

```bash
dotnet run
```

```powershell
dotnet run
```

Bạn sẽ thấy:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Mở `http://localhost:5000` trên trình duyệt và nhấn **Generate Article**.

**Nghiên cứu code:** Mở file `Program.cs` trong thư mục `csharp-web` và lưu ý:

- File dự án sử dụng `Microsoft.NET.Sdk.Web` thay vì `Microsoft.NET.Sdk`, bổ sung hỗ trợ ASP.NET Core.
- File tĩnh được phục vụ qua `UseDefaultFiles` và `UseStaticFiles` với thư mục `ui/` dùng chung.
- Endpoint `/api/article` ghi trực tiếp các dòng NDJSON vào `HttpContext.Response` và flush sau mỗi dòng để stream realtime.
- Toàn bộ logic tác nhân (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) giống hệt phiên bản console.

<details>
<summary>Trích đoạn chính từ csharp-web/Program.cs</summary>

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

### Bài tập 4: Khám phá các huy hiệu trạng thái tác nhân

Khi đã có UI hoạt động, xem front end cập nhật các huy hiệu trạng thái ra sao.

**4.1** Mở `zava-creative-writer-local/ui/app.js` trong trình soạn thảo.

**4.2** Tìm hàm `handleMessage()`. Chú ý cách hàm ánh xạ loại tin nhắn đến cập nhật DOM:

| Loại tin nhắn | Hành động UI |
|---------------|-------------|
| `message` chứa "researcher" | Đặt huy hiệu Researcher thành "Running" |
| `researcher` | Đặt huy hiệu Researcher thành "Done" và điền panel Kết quả nghiên cứu |
| `marketing` | Đặt huy hiệu Product Search thành "Done" và điền panel Sản phẩm phù hợp |
| `writer` với `data.start` | Đặt huy hiệu Writer thành "Running" và xóa vùng xuất bài viết |
| `partial` | Thêm token văn bản vào vùng xuất bài viết |
| `writer` với `data.complete` | Đặt huy hiệu Writer thành "Done" |
| `editor` | Đặt huy hiệu Editor thành "Done" và điền panel Phản hồi biên tập |

**4.3** Mở các panel "Research Results", "Product Matches", và "Editor Feedback" có thể thu gọn bên dưới bài viết để xem dữ liệu JSON thô của từng tác nhân.

---

### Bài tập 5: Tùy chỉnh UI (Mở rộng)

Thử một hoặc nhiều cải tiến sau:

**5.1 Thêm đếm số từ.** Sau khi Writer hoàn tất, hiển thị số từ bài viết bên dưới panel kết quả. Bạn có thể tính ở `handleMessage` khi `type === "writer"` và `data.complete` là true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Thêm chỉ báo thử lại.** Khi Editor yêu cầu sửa đổi, pipeline chạy lại. Hiển thị banner "Revision 1" hoặc "Revision 2" trong panel trạng thái. Nghe tin nhắn kiểu `message` chứa "Revision" và cập nhật phần tử DOM mới.

**5.3 Chế độ tối.** Thêm nút chuyển đổi và một class `.dark` vào thẻ `<body>`. Ghi đè màu nền, chữ, và panel trong `style.css` với selector `body.dark`.

---

## Tóm tắt

| Việc bạn đã làm | Cách thực hiện |
|-----------------|---------------|
| Phục vụ UI từ backend Python | Mount thư mục `ui/` bằng `StaticFiles` trong FastAPI |
| Thêm server HTTP cho biến thể JavaScript | Tạo `server.mjs` dùng module Node.js `http` tích hợp |
| Thêm web API cho biến thể C# | Tạo dự án `csharp-web` mới với ASP.NET Core minimal APIs |
| Tiêu thụ stream NDJSON trên trình duyệt | Dùng `fetch()` với `ReadableStream` và phân tích JSON từng dòng |
| Cập nhật UI theo thời gian thực | Ánh xạ loại tin nhắn tới cập nhật DOM (huy hiệu, văn bản, panel thu gọn) |

---

## Những điểm chính

1. Một **front end tĩnh dùng chung** có thể hoạt động với bất kỳ backend nào dùng chung giao thức stream, củng cố giá trị của mô hình API tương thích OpenAI.
2. **NDJSON (JSON phân cách bằng dòng)** là định dạng stream đơn giản và hoạt động trực tiếp với API `ReadableStream` của trình duyệt.
3. Biến thể **Python** thay đổi ít nhất vì đã có endpoint FastAPI; biến thể JavaScript và C# cần lớp bao HTTP nhẹ.
4. Giữ UI ở dạng **vanilla HTML/CSS/JS** tránh công cụ build, phụ thuộc framework và phức tạp không cần thiết cho người học workshop.
5. Cùng một module tác nhân (Researcher, Product, Writer, Editor) được tái sử dụng nguyên thủy; chỉ thay đổi lớp truyền tải.

---

## Tham khảo thêm

| Tài nguyên | Liên kết |
|------------|-----------|
| MDN: Sử dụng Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Tiếp tục xem [Phần 13: Hoàn thành Workshop](part13-workshop-complete.md) để tóm tắt mọi thứ bạn đã xây dựng trong suốt workshop này.

---
[← Phần 11: Gọi Công Cụ](part11-tool-calling.md) | [Phần 13: Hoàn Thành Hội Thảo →](part13-workshop-complete.md)