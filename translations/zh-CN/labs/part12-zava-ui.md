![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第12部分：为Zava创意写作器构建Web UI

> **目标：** 为Zava创意写作器添加基于浏览器的前端，以便您可以实时观看多代理管道的运行，实时显示代理状态徽章和流式输出的文章文本，所有内容均由单个本地Web服务器提供。

在[第7部分](part7-zava-creative-writer.md)中，您以<strong>CLI应用程序</strong>（JavaScript，C#）和<strong>无头API</strong>（Python）的形式探索了Zava创意写作器。在本实验中，您将为每个后端连接一个共享的<strong>原生HTML/CSS/JavaScript</strong>前端，使用户能够通过浏览器而非终端与管道交互。

---

## 您将学到什么

| 目标 | 描述 |
|-----------|-------------|
| 从后端提供静态文件 | 在API路由旁挂载HTML/CSS/JS目录 |
| 在浏览器中使用流式NDJSON | 使用Fetch API和`ReadableStream`读取以换行符分隔的JSON |
| 统一的流协议 | 确保Python、JavaScript和C#后端发出相同消息格式 |
| 渐进式UI更新 | 逐个令牌地更新代理状态徽章和文章文本流 |
| 为CLI应用添加HTTP层 | 将现有协调器逻辑包裹在Express风格服务器（JS）或ASP.NET Core最小API（C#）中 |

---

## 架构

UI是共享的静态文件集（`index.html`，`style.css`，`app.js`），由三种后端共用。每个后端暴露相同的两个路由：

![Zava UI架构——共享前端和三个后端](../../../images/part12-architecture.svg)

| 路由 | 方法 | 目的 |
|-------|--------|---------|
| `/` | GET | 提供静态UI |
| `/api/article` | POST | 运行多代理管道并流式传输NDJSON |

前端发送JSON请求体，并将响应读取为换行分隔的JSON消息流。每条消息均包含一个`type`字段，UI据此更新相应面板：

| 消息类型 | 含义 |
|-------------|---------|
| `message` | 状态更新（例如“开始研究员代理任务...”） |
| `researcher` | 研究结果已准备好 |
| `marketing` | 产品搜索结果已准备好 |
| `writer` | 写作器开始或完成（包含`{ start: true }`或`{ complete: true }`） |
| `partial` | 写作器流式传输的单个令牌（包含`{ text: "..." }`） |
| `editor` | 编辑器判决已准备好 |
| `error` | 发生错误 |

![浏览器中的消息类型路由](../../../images/part12-message-types.svg)

![流式序列——浏览器到后端通信](../../../images/part12-streaming-sequence.svg)

---

## 先决条件

- 完成[第7部分：Zava创意写作器](part7-zava-creative-writer.md)
- 安装Foundry Local CLI并下载`phi-3.5-mini`模型
- 使用现代网页浏览器（Chrome，Edge，Firefox或Safari）

---

## 共享UI

在修改任何后端代码之前，先花点时间探索三种语言版本都将使用的前端。文件位于`zava-creative-writer-local/ui/`：

| 文件 | 目的 |
|------|---------|
| `index.html` | 页面布局：输入表单、代理状态徽章、文章输出区域、可折叠详细面板 |
| `style.css` | 带有状态徽章颜色状态（等待、运行、完成、错误）的最小样式 |
| `app.js` | Fetch调用、`ReadableStream`行读取器及DOM更新逻辑 |

> **提示：** 直接在浏览器中打开`index.html`预览布局。尚无后端支持，功能无法正常，但可查看结构。

### 流读取器如何工作

`app.js`中的关键函数按块读取响应体，并在换行符处拆分：

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
    buffer = lines.pop(); // 保留不完整的尾随行

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


每条解析的消息都会发送到`handleMessage()`，后者根据`msg.type`更新相关DOM元素。

---

## 练习

### 练习1：使用UI运行Python后端

Python（FastAPI）版本已经有流式API端点。唯一变化是将`ui/`文件夹挂载为静态文件。

**1.1** 进入Python API目录并安装依赖：

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** 启动服务器：

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** 在浏览器中打开`http://localhost:8000`。您应看到带有三个文本字段和“生成文章”按钮的Zava创意写作器UI。

**1.4** 保持默认值，点击<strong>生成文章</strong>。观察代理状态徽章从“等待”变为“运行”，再变为“完成”，同时文章文本逐令牌流式传入输出区域。

> **故障排除：** 若页面显示JSON响应而非UI，请确认您运行的是已挂载静态文件的更新版`main.py`。`/api/article`端点路径未变，静态文件挂载为除该路径外的所有路由提供UI。

**工作原理：** 更新后的`main.py`底部添加了如此一行：

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```


这会将`zava-creative-writer-local/ui/`中的所有文件作为静态资源提供，并将`index.html`设为默认文档。`/api/article` POST路由注册在静态挂载之前，优先级更高。

---

### 练习2：为JavaScript版本添加Web服务器

JavaScript版本当前是CLI应用（`main.mjs`）。新增文件`server.mjs`，将同样的代理模块封装在HTTP服务器后，并提供共享UI。

**2.1** 进入JavaScript目录并安装依赖：

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** 启动Web服务器：

```bash
node server.mjs
```

```powershell
node server.mjs
```

您应看到：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** 在浏览器中打开`http://localhost:3000`，点击<strong>生成文章</strong>。UI在JavaScript后端表现完全相同。

**学习代码：** 打开`server.mjs`，注意关键模式：

- <strong>静态文件服务</strong>使用Node.js内置的`http`、`fs`和`path`模块，无需外部框架。
- <strong>目录遍历保护</strong>对请求路径进行规范化，并验证其仍位于`ui/`目录内。
- <strong>NDJSON流式传输</strong>使用`sendLine()`助手，对每个对象序列化，删除内部换行符，并追加尾部换行。
- <strong>代理协调</strong>重用现有的`researcher.mjs`，`product.mjs`，`writer.mjs`和`editor.mjs`模块，未做修改。

<details>
<summary>server.mjs关键摘录</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// 研究员
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// 作家（流媒体）
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### 练习3：为C#版本添加最小API

C#版本当前是控制台应用。新增项目`csharp-web`，使用ASP.NET Core最小API将同一管道作为Web服务暴露。

**3.1** 进入C# Web项目并还原包：

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** 运行Web服务器：

```bash
dotnet run
```

```powershell
dotnet run
```

您应看到：

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** 在浏览器中打开`http://localhost:5000`，点击<strong>生成文章</strong>。

**学习代码：** 打开`csharp-web`目录下的`Program.cs`，留意：

- 项目文件使用`Microsoft.NET.Sdk.Web`而非`Microsoft.NET.Sdk`，以支持ASP.NET Core。
- 静态文件通过`UseDefaultFiles`和`UseStaticFiles`指向共享`ui/`目录。
- `/api/article`端点直接向`HttpContext.Response`写入NDJSON行，每行写入后即时刷新以实现实时流。
- 所有代理逻辑（`RunResearcher`，`RunProductSearch`，`RunEditor`，`BuildWriterMessages`）与控制台版本相同。

<details>
<summary>csharp-web/Program.cs关键摘录</summary>

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

### 练习4：查看代理状态徽章

既然UI可用，看看前端如何更新状态徽章。

**4.1** 在编辑器中打开`zava-creative-writer-local/ui/app.js`。

**4.2** 找到`handleMessage()`函数。注意它如何映射消息类型到DOM更新：

| 消息类型 | UI动作 |
|-------------|-----------|
| 含“researcher”的`message` | 将研究员徽章设置为“运行中” |
| `researcher` | 将研究员徽章设置为“完成”，填充研究结果面板 |
| `marketing` | 将产品搜索徽章设置为“完成”，填充产品匹配面板 |
| 带`data.start`的`writer` | 将写作器徽章设置为“运行中”，清空文章输出 |
| `partial` | 将文本令牌追加到文章输出 |
| 带`data.complete`的`writer` | 将写作器徽章设置为“完成” |
| `editor` | 将编辑器徽章设置为“完成”，填充编辑器反馈面板 |

**4.3** 打开文章下方的“研究结果”、“产品匹配”和“编辑器反馈”可折叠面板，查看每个代理产生的原始JSON。

---

### 练习5：自定义UI（拓展）

尝试以下一个或多个增强：

**5.1 添加字数统计。** 写作器完成后，在输出面板下方显示文章字数。您可以在`handleMessage`中，当`type === "writer"`且`data.complete`为真时计算：

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```


**5.2 添加重试指示。** 当编辑器请求修订时，管道会重新运行。在状态面板显示“修订1”或“修订2”横幅。监听包含“Revision”的`message`类型，更新新DOM元素。

**5.3 深色模式。** 添加切换按钮和对`<body>`的`.dark`类。在`style.css`中使用`body.dark`选择器覆盖背景色、文字色和面板颜色。

---

## 总结

| 您做了什么 | 如何实现 |
|-------------|-----|
| 从Python后端提供UI | 使用FastAPI中的`StaticFiles`挂载`ui/`文件夹 |
| 为JavaScript版本添加HTTP服务器 | 使用Node.js内置`http`模块创建`server.mjs` |
| 为C#版本添加Web API | 创建ASP.NET Core最小API的新`csharp-web`项目 |
| 在浏览器中消费流式NDJSON | 使用`fetch()`配合`ReadableStream`，逐行解析JSON |
| 实时更新UI | 根据消息类型映射DOM更新（徽章、文字、可折叠面板） |

---

## 关键要点

1. 一个<strong>共享的静态前端</strong>可以配合任何遵循相同流协议的后端使用，凸显OpenAI兼容API模式的价值。
2. <strong>换行分隔的JSON（NDJSON）</strong>是一种简单的流格式，能原生支持浏览器`ReadableStream` API。
3. <strong>Python版本</strong>最少改动，因为已有FastAPI端点；JavaScript和C#版本需要一个轻量级HTTP包装层。
4. 保持UI作为<strong>原生HTML/CSS/JS</strong>避免构建工具、框架依赖及给研讨会学习者带来的额外复杂度。
5. 同一套代理模块（研究员、产品、写作器、编辑器）无修改复用，仅传输层发生变化。

---

## 进一步阅读

| 资源 | 链接 |
|----------|------|
| MDN：使用Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI静态文件 | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core静态文件 | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON规范 | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

继续前往[第13部分：研讨会总结](part13-workshop-complete.md)，回顾您在本研讨会中构建的所有内容。

---
[← 第11部分：工具调用](part11-tool-calling.md) | [第13部分：工作坊完成 →](part13-workshop-complete.md)