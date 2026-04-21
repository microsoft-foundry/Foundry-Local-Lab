![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第12部分：為 Zava 創意作家建置網頁 UI

> **目標：** 為 Zava 創意作家新增一個瀏覽器前端，讓您可以即時觀看多代理人流程運行狀態，顯示動態代理人狀態徽章和串流文章文字內容，全部由單一本地網頁伺服器提供服務。

在 [第7部分](part7-zava-creative-writer.md) 中，您已經以 **CLI 應用程式**（JavaScript、C#）及 **無頭 API**（Python）形式探索了 Zava 創意作家。在本實驗中，您將使用共用的 **純 HTML/CSS/JavaScript** 前端連接到各後端，使使用者能通過瀏覽器而非終端機與流程互動。

---

## 您將學習到

| 目標 | 說明 |
|-----------|-------------|
| 從後端提供靜態檔案 | 在 API 路由旁掛載 HTML/CSS/JS 目錄 |
| 在瀏覽器中使用串流 NDJSON | 使用 Fetch API 結合 `ReadableStream` 讀取換行分隔 JSON |
| 統一的串流協定 | 確保 Python、JavaScript 與 C# 後端輸出同一格式的訊息 |
| 漸進式 UI 更新 | 即時更新代理人狀態徽章，以標記逐字串流文章文字 |
| 為 CLI 應用加上 HTTP 層 | 使用 Express 風格伺服器（JS）或 ASP.NET Core 最小 API（C#）包裝現有協調器邏輯 |

---

## 架構

前端是由一組靜態檔案（`index.html`、`style.css`、`app.js`）組成，三個後端共用同一套。每個後端都暴露相同的兩條路由：

![Zava UI 架構 — 共用前端搭配三個後端](../../../images/part12-architecture.svg)

| 路由 | 方法 | 功能 |
|-------|--------|---------|
| `/` | GET | 提供靜態 UI |
| `/api/article` | POST | 執行多代理人流程並串流 NDJSON |

前端送出 JSON 格式的請求主體，並將回應視為換行分隔的 JSON（newline-delimited JSON）訊息串流。每條訊息含有 `type` 欄位，UI 根據此欄位更新對應的面板：

| 訊息類型 | 意義 |
|-------------|---------|
| `message` | 狀態更新（例如「啟動研究員代理人任務中...」） |
| `researcher` | 研究結果已準備好 |
| `marketing` | 產品搜尋結果已準備好 |
| `writer` | 撰稿者開始或完成（包含 `{ start: true }` 或 `{ complete: true }`） |
| `partial` | 來自撰稿者的單一串流文字標記（包含 `{ text: "..." }`） |
| `editor` | 編輯結果已準備好 |
| `error` | 發生錯誤 |

![瀏覽器中訊息類型路由](../../../images/part12-message-types.svg)

![串流序列 — 瀏覽器與後端通訊流程](../../../images/part12-streaming-sequence.svg)

---

## 先備知識

- 完成 [第7部分：Zava 創意作家](part7-zava-creative-writer.md)
- 安裝 Foundry Local CLI 並下載 `phi-3.5-mini` 模型
- 使用現代瀏覽器（Chrome、Edge、Firefox 或 Safari）

---

## 共用 UI

在動手修改任何後端程式碼前，先花點時間瀏覽所有三種語言路徑共用的前端。相關檔案位於 `zava-creative-writer-local/ui/`：

| 檔案 | 功能 |
|------|---------|
| `index.html` | 頁面佈局：輸入表單、代理人狀態徽章、文章輸出區、可摺疊細節面板 |
| `style.css` | 最小化樣式，包含狀態徽章的顏色狀態（等待中、運行中、完成、錯誤） |
| `app.js` | Fetch 呼叫、`ReadableStream` 逐行讀取器與 DOM 更新邏輯 |

> **提示：** 直接在瀏覽器中打開 `index.html` 預覽版面。目前不會有功能，因為還沒有後端，但可以看結構。

### 串流讀取器運作方式

`app.js` 主要函數會逐塊（chunk）讀取回應主體，並在換行符號切割：

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
    buffer = lines.pop(); // 保留不完整的尾隨行

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

每條解析完的訊息會傳給 `handleMessage()`，該函式根據 `msg.type` 更新相對應的 DOM 元素。

---

## 練習題

### 練習題 1：使用 UI 執行 Python 後端

Python（FastAPI）版本已經有串流 API 端點，唯一要變更的是掛載 `ui/` 資料夾作為靜態檔案。

**1.1** 切換到 Python API 目錄並安裝依賴：

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

**1.3** 在瀏覽器打開 `http://localhost:8000`，您應該會看到帶有三個文字欄位和「Generate Article」按鈕的 Zava 創意作家 UI。

**1.4** 點擊 **Generate Article** 使用預設值，觀察代理人狀態徽章從「Waiting」變更為「Running」再到「Done」，文章文字會一標記一標記地串流到輸出面板。

> **故障排除：** 如果頁面顯示 JSON 回應而非 UI，請確認您執行的是掛載靜態檔案的更新版 `main.py`。`/api/article` 端點仍使用原路徑，靜態檔案掛載會提供其他路由的 UI。

**運作原理：** 更新的 `main.py` 在底部新增一行：

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

這會將 `zava-creative-writer-local/ui/` 裡的每個檔案當作靜態資源提供，以 `index.html` 為預設文件。`/api/article` POST 路由在靜態檔案掛載之前註冊，因此優先級較高。

---

### 練習題 2：為 JavaScript 版本新增網頁伺服器

JavaScript 版本目前是 CLI 應用程式（`main.mjs`）。新檔案 `server.mjs` 將相同代理人模組包裝於 HTTP 伺服器後面並提供共用 UI。

**2.1** 切換到 JavaScript 目錄並安裝依賴：

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

您應該會看到：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** 在瀏覽器打開 `http://localhost:3000`，點擊 **Generate Article**，同樣的 UI 可無差異對應 JavaScript 後端。

**研究程式碼：** 打開 `server.mjs`，注意以下重點：

- <strong>靜態檔案伺服</strong> 使用 Node.js 內建的 `http`、`fs` 和 `path` 模組，無需外部框架。
- <strong>路徑遍歷保護</strong> 將請求路徑標準化，並驗證路徑是否限定在 `ui/` 資料夾內。
- **NDJSON 串流** 使用 `sendLine()` 輔助函式，將物件序列化，移除內部換行，並加入尾隨換行符。
- <strong>代理人協調</strong> 循環使用既有的 `researcher.mjs`、`product.mjs`、`writer.mjs` 及 `editor.mjs` 模組，保持不變。

<details>
<summary>server.mjs 中的關鍵摘錄</summary>

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

### 練習題 3：為 C# 版本新增 Minimal API

C# 版本目前是控制台應用程式。新專案 `csharp-web` 使用 ASP.NET Core 最小 API 來將相同流程暴露為網頁服務。

**3.1** 切換到 C# Web 專案並還原套件：

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** 啟動網頁伺服器：

```bash
dotnet run
```

```powershell
dotnet run
```

您應該會看到：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** 在瀏覽器打開 `http://localhost:5000`，點擊 **Generate Article**。

**研究程式碼：** 打開 `csharp-web` 目錄下的 `Program.cs`，注意：

- 專案檔使用 `Microsoft.NET.Sdk.Web` 而非 `Microsoft.NET.Sdk`，以增加 ASP.NET Core 支援。
- 靜態檔案透過 `UseDefaultFiles` 和 `UseStaticFiles` 提供，指向共用的 `ui/` 目錄。
- `/api/article` 端點直接寫入 NDJSON 行到 `HttpContext.Response`，每行寫入後立即 flush 以達到即時串流。
- 所有代理人邏輯（`RunResearcher`、`RunProductSearch`、`RunEditor`、`BuildWriterMessages`）與控制台版本相同。

<details>
<summary>csharp-web/Program.cs 中的關鍵摘錄</summary>

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

### 練習題 4：探索代理人狀態徽章

既然您有了一個運作中的 UI，看看前端是如何更新狀態徽章。

**4.1** 在編輯器中打開 `zava-creative-writer-local/ui/app.js`。

**4.2** 找到 `handleMessage()` 函式。注意它如何將訊息類型映射到 DOM 更新：

| 訊息類型 | UI 動作 |
|-------------|-----------|
| `message` 包含 "researcher" | 將 Researcher 徽章設為「Running」 |
| `researcher` | 將 Researcher 徽章設為「Done」，並填充 Research Results 面板 |
| `marketing` | 將 Product Search 徽章設為「Done」，並填充 Product Matches 面板 |
| `writer` 包含 `data.start` | 將 Writer 徽章設為「Running」，並清空文章輸出區 |
| `partial` | 將文字標記附加到文章輸出區 |
| `writer` 包含 `data.complete` | 將 Writer 徽章設為「Done」 |
| `editor` | 將 Editor 徽章設為「Done」，並填充 Editor Feedback 面板 |

**4.3** 打開文章下方的可摺疊「Research Results」、「Product Matches」與「Editor Feedback」面板，查看每個代理人產生的原始 JSON。

---

### 練習題 5：自訂 UI（進階）

試試以下一項或多項增強功能：

**5.1 加入字數計算。** 撰稿者完成後，在輸出面板下方顯示文章字數。您可以在 `handleMessage` 裡，當 `type === "writer"` 且 `data.complete` 為真時計算字數：

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 加入重試指示。** 當編輯者請求修訂時，流程會重新執行。在狀態面板顯示「Revision 1」或「Revision 2」橫幅。偵聽 `message` 類型且包含「Revision」的訊息，然後更新新 DOM 元素。

**5.3 深色模式。** 加入切換按鈕與 `<body>` 上的 `.dark` 類別。在 `style.css` 中以 `body.dark` 選擇器覆寫背景、文字及面板顏色。

---

## 總結

| 您做了什麼 | 如何達成 |
|-------------|-----|
| 從 Python 後端服務 UI | 以 FastAPI 掛載 `ui/` 資料夾並使用 `StaticFiles` |
| 為 JavaScript 版本新增 HTTP 伺服器 | 使用 Node.js 內建 `http` 模組創建 `server.mjs` |
| 為 C# 版本新增 Web API | 建立新 `csharp-web` 專案並利用 ASP.NET Core 最小 API |
| 在瀏覽器中消費串流 NDJSON | 使用 `fetch()` 搭配 `ReadableStream` 及逐行 JSON 解析 |
| 即時更新 UI | 將訊息類型映射到 DOM 更新（徽章、文字、摺疊面板） |

---

## 重要啟示

1. <strong>共用靜態前端</strong> 可與任何遵守相同串流協定的後端配合，突顯 OpenAI 兼容 API 模式的價值。
2. **換行分隔 JSON (NDJSON)** 是一種簡單的串流格式，與瀏覽器的 `ReadableStream` API 天生契合。
3. **Python 版本** 幾乎不需更改，因其已有 FastAPI 端點；而 JavaScript 和 C# 則需薄薄的 HTTP 包裝層。
4. 保持 UI 為 **純 HTML/CSS/JS** 可避免編譯工具、框架依賴與額外複雜度，利於工作坊學習者。
5. 相同代理人模組（Researcher、Product、Writer、Editor）可重複使用，只有傳輸層改變。

---

## 延伸閱讀

| 資源 | 連結 |
|----------|------|
| MDN：使用可讀串流 | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI 靜態檔案 | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core 靜態檔案 | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON 規格 | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

繼續閱讀 [第13部分：工作坊總結](part13-workshop-complete.md)，回顧您在工作坊中完成的所有內容。

---
[← 第11部分：工具調用](part11-tool-calling.md) | [第13部分：工作坊完成 →](part13-workshop-complete.md)