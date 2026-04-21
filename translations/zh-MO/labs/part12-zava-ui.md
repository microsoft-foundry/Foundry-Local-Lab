![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第12部分：為 Zava 創意作家打造網頁使用者介面

> **目標：** 為 Zava 創意作家新增瀏覽器前端，讓您可以即時觀看多代理管線的執行狀態，包含即時代理狀態徽章與串流文章文字，所有功能由一個本地 Web 伺服器提供服務。

在[第7部分](part7-zava-creative-writer.md)中，您探索了 Zava 創意作家的 **CLI 應用程式**（JavaScript、C#）和 **無頭 API**（Python）。在本實驗中，您將為每個後端連接一個共用的 **原生 HTML/CSS/JavaScript** 前端，讓使用者能透過瀏覽器與管線互動，而不是透過終端機。

---

## 您將學到什麼

| 目標 | 說明 |
|-----------|-------------|
| 從後端提供靜態檔案 | 在 API 路徑旁掛載 HTML/CSS/JS 目錄 |
| 在瀏覽器使用串流 NDJSON | 使用 Fetch API 搭配 `ReadableStream` 讀取以換行分隔的 JSON |
| 統一的串流協定 | 確保 Python、JavaScript 和 C# 後端輸出相同格式的訊息 |
| 逐步更新使用者介面 | 更新代理狀態徽章並逐字串流文章文字 |
| 將 HTTP 層加入 CLI 應用 | 用 Express 風格伺服器 (JS) 或 ASP.NET Core minimal API (C#) 包裝現有的協作邏輯 |

---

## 架構

介面是一組靜態檔案（`index.html`、`style.css`、`app.js`）共用於三個後端。每個後端都會暴露相同的兩個路由：

![Zava UI 架構 — 三個後端共用前端](../../../images/part12-architecture.svg)

| 路由 | 方法 | 目的 |
|-------|--------|---------|
| `/` | GET | 提供靜態 UI |
| `/api/article` | POST | 執行多代理管線並串流 NDJSON |

前端會傳送 JSON 主體，並以串流方式讀取以換行分隔的 JSON 訊息。每條訊息都包含 `type` 欄位，介面依此更新對應面板：

| 訊息類型 | 意義 |
|-------------|---------|
| `message` | 狀態更新（例如「啟動研究員代理任務...」） |
| `researcher` | 研究結果已準備好 |
| `marketing` | 產品搜尋結果已準備好 |
| `writer` | 作家啟動或完成（包含 `{ start: true }` 或 `{ complete: true }`） |
| `partial` | 作家串流傳送的單一文字標記（包含 `{ text: "..." }`） |
| `editor` | 編輯判斷已完成 |
| `error` | 發生錯誤 |

![瀏覽器中的訊息類型路由](../../../images/part12-message-types.svg)

![串流序列 — 瀏覽器到後端通訊](../../../images/part12-streaming-sequence.svg)

---

## 先決條件

- 完成[第7部分：Zava 創意作家](part7-zava-creative-writer.md)
- 安裝 Foundry Local CLI 且下載 `phi-3.5-mini` 模型
- 一個現代瀏覽器（Chrome、Edge、Firefox 或 Safari）

---

## 共用使用者介面

在改動任何後端程式之前，先花點時間探索所有三個語言路線共用的前端。檔案位於 `zava-creative-writer-local/ui/`：

| 檔案 | 用途 |
|------|---------|
| `index.html` | 頁面布局：輸入表單、代理狀態徽章、文章輸出區、可摺疊詳細面板 |
| `style.css` | 最小樣式，包含狀態徽章顏色狀態（等待、執行中、完成、錯誤） |
| `app.js` | Fetch 呼叫、`ReadableStream` 行讀取器與 DOM 更新邏輯 |

> **提示：** 直接在瀏覽器中開啟 `index.html` 預覽布局。目前未連接後端，功能無法使用，但可查看結構。

### 流讀取器的運作方式

`app.js` 中的關鍵函式逐塊讀取回應主體，並依換行界定拆分：

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

每個解析後的訊息會傳送到 `handleMessage()`，此函式根據 `msg.type` 更新相應的 DOM 元素。

---

## 練習

### 練習 1：用 UI 運行 Python 後端

Python（FastAPI）版本已有串流 API 路由。唯一改動是在於掛載 `ui/` 資料夾作為靜態檔案。

**1.1** 進入 Python API 目錄並安裝相依：

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** 啟動伺服器：

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** 在瀏覽器開啟 `http://localhost:8000`。您應會看到含三個文字欄位及「Generate Article」按鈕的 Zava 創意作家 UI。

**1.4** 使用預設值點擊 **Generate Article**。觀察代理狀態徽章由「Waiting」轉成「Running」再到「Done」，並隨代理完成任務而更新。文章文字則逐字串流進輸出面板。

> **疑難排解：** 若頁面顯示 JSON 回應而非 UI，請確認您執行的是掛載靜態檔案的更新版 `main.py`。`/api/article` 路由仍在原位置提供服務；靜態檔案掛載讓 UI 在其他路由都能提供。

**運作方式：** 更新後的 `main.py` 在底部加入一行：

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

此行將 `zava-creative-writer-local/ui/` 下的所有檔案作為靜態資源服務，`index.html` 為預設文件。`/api/article` POST 路由在靜態檔案前註冊，因此優先被處理。

---

### 練習 2：為 JavaScript 版本加入 Web 伺服器

JavaScript 版本目前是 CLI 應用程式（`main.mjs`）。新增一個檔案 `server.mjs`，將同樣的代理模組包裝成 HTTP 伺服器，並提供共用 UI。

**2.1** 進入 JavaScript 目錄並安裝相依：

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** 啟動 Web 伺服器：

```bash
node server.mjs
```

```powershell
node server.mjs
```

您應會看到：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** 在瀏覽器打開 `http://localhost:3000`，點擊 **Generate Article**。相同 UI 即可在 JavaScript 後端正常運作。

**研讀程式碼：** 開啟 `server.mjs`，注意以下重點：

- <strong>靜態檔案提供</strong> 使用 Node.js 內建的 `http`、`fs` 與 `path` 模組，無需第三方框架。
- <strong>路徑穿越防護</strong> 正規化請求路徑，確保檔案在 `ui/` 目錄內。
- **NDJSON 串流** 使用 `sendLine()` 幫助函式，將物件序列化、移除內部換行字元並加上尾部換行。
- <strong>代理協調</strong> 重複使用既有的 `researcher.mjs`、`product.mjs`、`writer.mjs` 與 `editor.mjs` 模組，無改動。

<details>
<summary>server.mjs 重要節錄</summary>

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

### 練習 3：為 C# 版本新增 Minimal API

C# 版本目前是主控台應用程式。一個新專案 `csharp-web` 使用 ASP.NET Core minimal API 將同樣的管線暴露為 Web 服務。

**3.1** 進入 C# Web 專案並還原套件：

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** 執行 Web 伺服器：

```bash
dotnet run
```

```powershell
dotnet run
```

您應會看到：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** 在瀏覽器開啟 `http://localhost:5000` ，點擊 **Generate Article**。

**研讀程式碼：** 開啟 `csharp-web` 目錄的 `Program.cs`，注意：

- 專案檔使用 `Microsoft.NET.Sdk.Web`，取代 `Microsoft.NET.Sdk`，支援 ASP.NET Core。
- 靜態檔案透過 `UseDefaultFiles` 和 `UseStaticFiles` 從共用的 `ui/` 目錄提供。
- `/api/article` 路由直接向 `HttpContext.Response` 寫入 NDJSON 行並在每行後立即刷新，以實現即時串流。
- 所有代理邏輯（`RunResearcher`、`RunProductSearch`、`RunEditor`、`BuildWriterMessages`）與主控台版本相同。

<details>
<summary>csharp-web/Program.cs 重要節錄</summary>

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

### 練習 4：探究代理狀態徽章

現在您已有運作中的 UI，看看前端如何更新狀態徽章。

**4.1** 用編輯器開啟 `zava-creative-writer-local/ui/app.js`。

**4.2** 找出 `handleMessage()` 函式。注意它如何將訊息類型映射到 DOM 更新：

| 訊息類型 | UI 動作 |
|-------------|-----------|
| 含 "researcher" 的 `message` | 將 Researcher 徽章設為「Running」 |
| `researcher` | 將 Researcher 徽章設為「Done」並填充研究結果面板 |
| `marketing` | 將 Product Search 徽章設為「Done」並填充產品匹配面板 |
| `writer` 且帶 `data.start` | 將 Writer 徽章設為「Running」並清空文章輸出 |
| `partial` | 將文字標記追加到文章輸出 |
| `writer` 且帶 `data.complete` | 將 Writer 徽章設為「Done」 |
| `editor` | 將 Editor 徽章設為「Done」並填充編輯反饋面板 |

**4.3** 開啟文章下方的可摺疊面板「Research Results」、「Product Matches」及「Editor Feedback」，檢視各代理產出的原始 JSON。

---

### 練習 5：自訂 UI（進階）

嘗試以下一項或多項增強功能：

**5.1 新增字數計數。** Writer 完成後，在輸出面板下顯示文章字數。您可在 `handleMessage` 處理 `type === "writer"` 且 `data.complete` 為 true 時，計算字數：

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 新增重試指示器。** 編輯請求修改時，管線會重新執行。於狀態面板顯示「Revision 1」或「Revision 2」橫幅。監聽含「Revision」的 `message` 類型訊息，更新一個新的 DOM 元素。

**5.3 深色模式。** 新增切換按鈕與 `<body>` 的 `.dark` 類別。在 `style.css` 透過 `body.dark` 選擇器覆蓋背景、文字與面板顏色。

---

## 總結

| 您做了什麼 | 如何做的 |
|-------------|-----|
| 從 Python 後端提供 UI | 在 FastAPI 中用 `StaticFiles` 掛載 `ui/` 目錄 |
| 為 JavaScript 版本新增 HTTP 伺服器 | 用 Node.js 內建 `http` 模組建立 `server.mjs` |
| 為 C# 版本新增 Web API | 建立新 `csharp-web` 專案，使用 ASP.NET Core minimal API |
| 在瀏覽器消費串流 NDJSON | 使用 `fetch()` 搭配 `ReadableStream` 及行式 JSON 解析 |
| 即時更新 UI | 根據訊息類型映射至 DOM 更新（徽章、文字、可摺疊面板） |

---

## 重要重點

1. <strong>共用靜態前端</strong> 能與任何遵循相同串流協定的後端搭配，強化 OpenAI 兼容 API 模式的價值。
2. **以換行分隔的 JSON（NDJSON）** 是簡單的串流格式，原生支援瀏覽器 `ReadableStream` API。
3. **Python 版本** 改動最少，因已有 FastAPI 路由；JavaScript 和 C# 版本則需要一層輕量 HTTP 包裝器。
4. 保持 UI 為 **原生 HTML/CSS/JS**，避免構建工具、框架相依，以及增加學習複雜度。
5. 相同代理模組（Researcher、Product、Writer、Editor）重複使用未改動，僅改變傳輸層。

---

## 延伸閱讀

| 資源 | 連結 |
|----------|------|
| MDN：使用 Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI 靜態檔案 | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core 靜態檔案 | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON 規範 | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

繼續前往[第13部分：工作坊完成](part13-workshop-complete.md) 瀏覽整個工作坊的建置總結。

---
[← 第11部分：工具調用](part11-tool-calling.md) | [第13部分：工作坊完成 →](part13-workshop-complete.md)