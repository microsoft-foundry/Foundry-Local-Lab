![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 12: Building a Web UI for the Zava Creative Writer

> **Goal:** Add one browser-based front end to the Zava Creative Writer so you fit watch the multi-agent pipeline run for real time, with live agent status badges and streamed article text, all serve from one single local web server.

For [Part 7](part7-zava-creative-writer.md) you don check the Zava Creative Writer as **CLI application** (JavaScript, C#) and **headless API** (Python). For this lab you go connect one shared **vanilla HTML/CSS/JavaScript** front end to each backend so that users fit interact with the pipeline through browser instead of terminal.

---

## Wetin You Go Learn

| Objective | Description |
|-----------|-------------|
| Serve static files from a backend | Mount one HTML/CSS/JS directory beside your API route |
| Consume streaming NDJSON in the browser | Use the Fetch API with `ReadableStream` to read newline-delimited JSON |
| Unified streaming protocol | Make sure Python, JavaScript, and C# backends dey emit the same message format |
| Progressive UI updates | Update agent status badges and stream article text token by token |
| Add an HTTP layer to a CLI app | Wrap the existing orchestrator logic inside Express-style server (JS) or ASP.NET Core minimal API (C#) |

---

## Architecture

The UI na one set of static files (`index.html`, `style.css`, `app.js`) wey all three backends share. Each backend expose the same two routes:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| Route | Method | Purpose |
|-------|--------|---------|
| `/` | GET | Serve the static UI |
| `/api/article` | POST | Run the multi-agent pipeline and stream NDJSON |

The front end go send JSON body and read the response as stream of newline-delimited JSON messages. Each message get one `type` field wey the UI go use update the correct panel:

| Message type | Meaning |
|-------------|---------|
| `message` | Status update (e.g. "Starting researcher agent task...") |
| `researcher` | Research results don ready |
| `marketing` | Product search results don ready |
| `writer` | Writer don start or finish (get `{ start: true }` or `{ complete: true }`) |
| `partial` | One single streamed token from the Writer (get `{ text: "..." }`) |
| `editor` | Editor verdict don ready |
| `error` | Something wrong happen |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## Prerequisites

- Complete [Part 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI wey don install plus the `phi-3.5-mini` model don download
- One modern web browser (Chrome, Edge, Firefox, or Safari)

---

## The Shared UI

Before you begin touch backend code, make you take small time explore the front end wey all three language tracks go use. The files dey inside `zava-creative-writer-local/ui/`:

| File | Purpose |
|------|---------|
| `index.html` | Page layout: input form, agent status badges, article output area, collapsible detail panels |
| `style.css` | Minimal styling with status-badge colour states (waiting, running, done, error) |
| `app.js` | Fetch call, `ReadableStream` line reader, and DOM update logic |

> **Tip:** Open `index.html` direct for your browser to preview the layout. Nothing go work yet because backend no dey, but you fit see the structure.

### How the Stream Reader Works

The main function for `app.js` dey read the response body chunk by chunk and split am for newline boundaries:

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
    buffer = lines.pop(); // make di incomplete trailing line remain

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

Each parsed message go dispatch to `handleMessage()`, wey go update the relevant DOM element based on `msg.type`.

---

## Exercises

### Exercise 1: Run the Python Backend with the UI

The Python (FastAPI) version already get streaming API endpoint. The only change na to mount the `ui/` folder as static files.

**1.1** Goto the Python API directory and install dependencies:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Start the server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Open your browser for `http://localhost:8000`. You suppose see the Zava Creative Writer UI with three text fields and one "Generate Article" button.

**1.4** Click **Generate Article** using the default values. Watch the agent status badges dem change from "Waiting" to "Running" to "Done" as each agent finish e work, plus see the article text stream enter the output panel token by token.

> **Troubleshooting:** If the page show JSON response instead of UI, make sure say you dey run the updated `main.py` wey mount the static files. The `/api/article` endpoint still dey work for e original path; the static file mount dey serve the UI for all other routes.

**How e dey work:** The updated `main.py` add one line for the bottom:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

This one dey serve every file for `zava-creative-writer-local/ui/` as static asset, with `index.html` as the default document. The `/api/article` POST route register before the static mount, so e get priority.

---

### Exercise 2: Add a Web Server to the JavaScript Variant

The JavaScript version currently na CLI application (`main.mjs`). One new file, `server.mjs`, dey wrap the same agent modules behind HTTP server and serve the shared UI.

**2.1** Goto the JavaScript directory and install dependencies:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Start the web server:

```bash
node server.mjs
```

```powershell
node server.mjs
```

You go see:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Open `http://localhost:3000` for your browser and click **Generate Article**. The same UI go work the same way for the JavaScript backend.

**Study the code:** Open `server.mjs` and notice the key patterns:

- **Static file serving** dey use Node.js built-in `http`, `fs`, and `path` modules no external framework needed.
- **Path-traversal protection** dey normalise the requested path and confirm say e no comot for inside `ui/` directory.
- **NDJSON streaming** dey use one `sendLine()` helper wey go serialise each object, remove internal newlines, and add one trailing newline.
- **Agent orchestration** dey reuse the existing `researcher.mjs`, `product.mjs`, `writer.mjs`, and `editor.mjs` modules without change.

<details>
<summary>Key excerpt from server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Risača
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Raita (floe)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Exercise 3: Add a Minimal API to the C# Variant

The C# version currently na console application. One new project, `csharp-web`, dey use ASP.NET Core minimal APIs to expose the same pipeline as web service.

**3.1** Goto the C# web project and restore packages:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Run the web server:

```bash
dotnet run
```

```powershell
dotnet run
```

You go see:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Open `http://localhost:5000` for your browser and click **Generate Article**.

**Study the code:** Open `Program.cs` for inside `csharp-web` directory and note:

- The project file dey use `Microsoft.NET.Sdk.Web` instead of `Microsoft.NET.Sdk`, which add ASP.NET Core support.
- Static files dey serve through `UseDefaultFiles` and `UseStaticFiles` point to the shared `ui/` directory.
- The `/api/article` endpoint dey write NDJSON lines direct to `HttpContext.Response` and dey flush after each line for real-time streaming.
- All agent logic (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) na the same as console version.

<details>
<summary>Key excerpt from csharp-web/Program.cs</summary>

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

### Exercise 4: Explore the Agent Status Badges

Now wey you get one working UI, check how the front end dey update the status badges.

**4.1** Open `zava-creative-writer-local/ui/app.js` for your editor.

**4.2** Find the `handleMessage()` function. Notice how e dey map message types to DOM updates:

| Message type | UI action |
|-------------|-----------|
| `message` wey get "researcher" | Set Researcher badge to "Running" |
| `researcher` | Set Researcher badge to "Done" and fill Research Results panel |
| `marketing` | Set Product Search badge to "Done" and fill Product Matches panel |
| `writer` with `data.start` | Set Writer badge to "Running" and clear the article output |
| `partial` | Add the token text to the article output |
| `writer` with `data.complete` | Set Writer badge to "Done" |
| `editor` | Set Editor badge to "Done" and fill Editor Feedback panel |

**4.3** Open the collapsible "Research Results", "Product Matches", and "Editor Feedback" panels under the article to inspect the raw JSON wey each agent produce.

---

### Exercise 5: Customise the UI (Stretch)

Try one or more of these improvements:

**5.1 Add word count.** After Writer finish, show the article word count under the output panel. You fit calculate this inside `handleMessage` when `type === "writer"` and `data.complete` true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Add retry indicator.** When Editor request revision, the pipeline go run again. Show banner say "Revision 1" or "Revision 2" for the status panel. Listen for `message` type wey get "Revision" and update new DOM element.

**5.3 Dark mode.** Add toggle button and `.dark` class to the `<body>`. Override background, text, and panel colours for `style.css` with one `body.dark` selector.

---

## Summary

| Wetin you do | How |
|-------------|-----|
| Serve the UI from the Python backend | Mount the `ui/` folder with `StaticFiles` for FastAPI |
| Add HTTP server to JavaScript variant | Create `server.mjs` with built-in Node.js `http` module |
| Add web API to C# variant | Create new `csharp-web` project with ASP.NET Core minimal APIs |
| Consume streaming NDJSON for browser | Use `fetch()` with `ReadableStream` and line-by-line JSON parsing |
| Update UI for real time | Map message types to DOM updates (badges, text, collapsible panels) |

---

## Key Takeaways

1. One **shared static front end** fit work with any backend wey dey talk the same streaming protocol, this one dey confirm the value of OpenAI-compatible API pattern.
2. **Newline-delimited JSON (NDJSON)** na simple streaming format wey fit work naturally with browser `ReadableStream` API.
3. The **Python variant** need less change because e already get FastAPI endpoint; the JavaScript and C# variants need small HTTP wrapper.
4. Keep UI as **vanilla HTML/CSS/JS** avoid build tools, framework dependencies, plus other complexity for workshop learners.
5. The same agent modules (Researcher, Product, Writer, Editor) dey reused without change; only the transport layer dey different.

---

## Further Reading

| Resource | Link |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Specification | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Continue to [Part 13: Workshop Complete](part13-workshop-complete.md) for summary of everything you don build for this workshop.

---
[← Part 11: Tool Calling](part11-tool-calling.md) | [Part 13: Workshop Complete →](part13-workshop-complete.md)