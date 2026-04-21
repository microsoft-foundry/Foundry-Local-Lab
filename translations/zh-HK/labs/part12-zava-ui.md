![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第12部分：為Zava創意作家建立網頁UI

> **目標：** 為Zava創意作家新增基於瀏覽器的前端，讓你可以實時觀看多代理管線的運行狀態，擁有即時的代理狀態徽章和串流文章文字，全部由單一本地網頁服務器提供服務。

在[第7部分](part7-zava-creative-writer.md)中，你探索了Zava創意作家作為<strong>CLI應用程序</strong>（JavaScript、C#）和<strong>無頭API</strong>（Python）。在此實驗中，你將連接一個共用的<strong>純HTML/CSS/JavaScript</strong>前端到每個後端，使用戶能透過瀏覽器而非命令行操作管線。

---

## 你將學習到的內容

| 目標 | 說明 |
|-----------|-------------|
| 從後端提供靜態檔案 | 在API路由旁掛載HTML/CSS/JS目錄 |
| 在瀏覽器中消費串流NDJSON | 使用Fetch API與`ReadableStream`讀取換行分隔JSON |
| 統一的串流協議 | 確保Python、JavaScript和C#後端發送相同的訊息格式 |
| 進階的UI更新 | 逐個token更新代理狀態徽章和文章文字串流 |
| 為CLI應用新增HTTP層 | 用Express風格的伺服器（JS）或ASP.NET Core簡易API（C#）包裝現有的協調邏輯 |

---

## 架構

UI是一組由三個後端共享的靜態檔案（`index.html`、`style.css`、`app.js`）。每個後端暴露兩條相同的路由：

![Zava UI 架構 — 共用前端配合三個後端](../../../images/part12-architecture.svg)

| 路由 | 方法 | 用途 |
|-------|--------|---------|
| `/` | GET | 提供靜態UI |
| `/api/article` | POST | 執行多代理管線並串流NDJSON |

前端發送JSON主體並以串流方式讀取以換行分隔的JSON訊息。每條訊息都有一個`type`欄位，UI使用它來更新對應面板：

| 訊息類型 | 意義 |
|-------------|---------|
| `message` | 狀態更新（例如「開始研究代理任務...」） |
| `researcher` | 研究結果已準備好 |
| `marketing` | 產品搜索結果已準備好 |
| `writer` | 作家開始或完成（包含`{ start: true }`或`{ complete: true }`） |
| `partial` | 作家串流的單個token（包含`{ text: "..." }`） |
| `editor` | 編輯決策已準備好 |
| `error` | 發生錯誤 |

![瀏覽器中訊息類型路由](../../../images/part12-message-types.svg)

![串流序列 — 瀏覽器到後端通訊](../../../images/part12-streaming-sequence.svg)

---

## 先決條件

- 完成[第7部分：Zava創意作家](part7-zava-creative-writer.md)
- 已安裝Foundry Local CLI並下載`phi-3.5-mini`模型
- 現代網頁瀏覽器（Chrome、Edge、Firefox或Safari）

---

## 共用前端UI

在觸碰後端程式碼之前，先花點時間探索所有三種語言線程共用的前端。檔案位於`zava-creative-writer-local/ui/`：

| 檔案 | 用途 |
|------|---------|
| `index.html` | 頁面佈局：輸入表單、代理狀態徽章、文章輸出區、可折疊的細節面板 |
| `style.css` | 簡約樣式及狀態徽章顏色狀態（等待中、運行中、完成、錯誤） |
| `app.js` | Fetch調用、`ReadableStream`按行讀取器及DOM更新邏輯 |

> **提示：** 直接在瀏覽器打開`index.html`即可預覽佈局。尚未有後端前一切功能不會啟動，但你能看到結構。

### 串流讀取器運作原理

`app.js`中的關鍵函數逐塊讀取回應主體，並依換行符分割：

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
    buffer = lines.pop(); // 保留未完成的尾行

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

每則解析後的訊息會交由`handleMessage()`處理，該函數根據`msg.type`更新相關的DOM元素。

---

## 練習

### 練習1：搭配UI執行Python後端

Python（FastAPI）版本已有串流API端點，唯一變更是掛載`ui/`資料夾為靜態檔案。

**1.1** 前往Python API目錄並安裝依賴：

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** 啟動服務器：

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** 在瀏覽器開啟`http://localhost:8000`，應看到帶有三個文字欄位及「生成文章」按鈕的Zava創意作家UI。

**1.4** 使用預設值點擊<strong>生成文章</strong>。觀察代理狀態徽章從「等待」變「運行中」，再到「完成」，並看到文章文字逐token串流輸出到面板。

> **故障排除：** 若頁面顯示JSON回應而非UI，請確認已執行更新後掛載靜態檔案的`main.py`。`/api/article`路由仍維持使用原始路徑，靜態檔案掛載則於其他所有路由提供UI。

**運作原理：** 更新後的`main.py`底部新增一行：

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

此行將`zava-creative-writer-local/ui/`下的所有檔案作為靜態資產服務，預設文件為`index.html`。`/api/article` POST路由在靜態掛載之前註冊，因此優先處理。

---

### 練習2：為JavaScript版本新增網頁伺服器

JavaScript版本目前為CLI程式（`main.mjs`）。新增檔案`server.mjs`將相同的代理模組包裝於HTTP伺服器後，並提供共用UI。

**2.1** 前往JavaScript目錄並安裝依賴：

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** 啟動網頁伺服器：

```bash
node server.mjs
```

```powershell
node server.mjs
```

你將看到：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** 在瀏覽器打開`http://localhost:3000`並點擊<strong>生成文章</strong>。相同的UI會無縫與JavaScript後端工作。

**研究程式碼：** 打開`server.mjs`注意以下要點：

- <strong>靜態檔案服務</strong>使用Node.js內建的`http`、`fs`和`path`模組，無需外部框架。
- <strong>路徑穿越防護</strong>會標準化請求路徑，確保路徑仍在`ui/`目錄內。
- <strong>NDJSON串流</strong>透過`sendLine()`輔助函式序列化物件，去除內部換行並附加尾部換行。
- <strong>代理協調</strong>重複使用既有的`researcher.mjs`、`product.mjs`、`writer.mjs`和`editor.mjs`模組，無修改。

<details>
<summary>server.mjs關鍵摘錄</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// 研究員
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// 作家（串流）
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### 練習3：為C#版本新增簡易API

C#版本目前是控制台應用。新專案`csharp-web`使用ASP.NET Core簡易API暴露同一管線作為網頁服務。

**3.1** 前往C#網頁專案並還原套件：

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** 執行網頁伺服器：

```bash
dotnet run
```

```powershell
dotnet run
```

你會看到：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** 在瀏覽器打開`http://localhost:5000`並點擊<strong>生成文章</strong>。

**研究程式碼：** 打開`csharp-web`目錄下的`Program.cs`並注意：

- 專案檔採用`Microsoft.NET.Sdk.Web`而非`Microsoft.NET.Sdk`，添加了ASP.NET Core支援。
- 靜態檔案透過`UseDefaultFiles`和`UseStaticFiles`服務，指向共用的`ui/`目錄。
- `/api/article`路由直接向`HttpContext.Response`寫入NDJSON行並每行呼叫flush實現即時串流。
- 所有代理邏輯（`RunResearcher`、`RunProductSearch`、`RunEditor`、`BuildWriterMessages`）與控制台版本相同。

<details>
<summary>csharp-web/Program.cs的關鍵摘錄</summary>

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

### 練習4：探索代理狀態徽章

成功建立UI後，看看前端如何更新狀態徽章。

**4.1** 使用編輯器打開`zava-creative-writer-local/ui/app.js`。

**4.2** 找到`handleMessage()`函數。注意它如何根據訊息類型映射至DOM更新：

| 訊息類型 | UI動作 |
|-------------|-----------|
| `message`含有"researcher" | 將研究員徽章設為「運行中」 |
| `researcher` | 將研究員徽章設為「完成」，並填充研究結果面板 |
| `marketing` | 將產品搜尋徽章設為「完成」，並填充產品匹配面板 |
| `writer`含`data.start` | 將作家徽章設為「運行中」並清空文章輸出區 |
| `partial` | 附加token文字到文章輸出區 |
| `writer`含`data.complete` | 將作家徽章設為「完成」 |
| `editor` | 將編輯徽章設為「完成」，並填充編輯反饋面板 |

**4.3** 打開文章下方可折疊的「研究結果」、「產品匹配」及「編輯反饋」面板，檢查各代理產出的原始JSON。

---

### 練習5：自訂UI（進階）

嘗試以下一項或多項強化：

**5.1 新增字數計數器。** 作家完成後，在輸出面板下方顯示文章字數。可於`handleMessage`中`type === "writer"`且`data.complete`為真時計算：

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 新增重試指示器。** 當編輯員要求修改，管線會重新運行。在狀態面板顯示「修訂版1」、「修訂版2」等橫幅。監聽包含「Revision」的`message`類型，更新新的DOM元素。

**5.3 深色模式。** 新增切換按鈕及`<body>`的`.dark`類別。於`style.css`中使用`body.dark`選擇器覆寫背景、文字及面板顏色。

---

## 總結

| 你完成了什麼 | 如何做到 |
|-------------|-----|
| 從Python後端提供UI | 在FastAPI中掛載`ui/`資料夾並使用`StaticFiles` |
| 為JavaScript版本新增HTTP伺服器 | 使用內建Node.js的`http`模組創建`server.mjs` |
| 為C#版本新增網頁API | 創建ASP.NET Core簡易API的新專案`csharp-web` |
| 在瀏覽器消費串流NDJSON | 使用`fetch()`配合`ReadableStream`和逐行JSON解析 |
| 即時更新UI | 將訊息類型對應至DOM更新（徽章、文字、可折疊面板） |

---

## 重要心得

1. <strong>共用靜態前端</strong>可搭配任意遵循相同串流協議的後端，凸顯OpenAI相容API模式的價值。
2. **換行分隔JSON (NDJSON)** 是瀏覽器`ReadableStream`的天生搭檔，是簡單又有效的串流格式。
3. <strong>Python版本</strong>變動最少，因已有FastAPI端點；JavaScript及C#版本則需包裝薄層HTTP服務。
4. 保持UI為<strong>純HTML/CSS/JS</strong>避免工具鏈、框架依賴以及工作坊學員負擔。
5. 相同的代理模組（Researcher、Product、Writer、Editor）重複使用且不變，僅傳輸層改變。

---

## 延伸閱讀

| 資源 | 連結 |
|----------|------|
| MDN：使用Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI 靜態檔案 | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core 靜態檔案 | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON 規範 | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

繼續閱讀[第13部分：工作坊結束](part13-workshop-complete.md)，獲取整個工作坊建置內容的總結。

---
[← 第11部分：工具調用](part11-tool-calling.md) | [第13部分：工作坊完成 →](part13-workshop-complete.md)