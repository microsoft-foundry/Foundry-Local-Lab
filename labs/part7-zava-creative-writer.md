![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 7: Zava Creative Writer - Capstone Application

> **Goal:** Explore a production-style multi-agent application where four specialised agents collaborate to produce magazine-quality articles for Zava Retail DIY - running entirely on your device with Foundry Local.

This is the **capstone lab** of the workshop. It brings together everything you have learned - SDK integration (Part 3), retrieval from local data (Part 4), agent personas (Part 5), and multi-agent orchestration (Part 6) - into a complete application available in **Python**, **JavaScript**, and **C#**.

---

## What You Will Explore

| Concept | Where in the Zava Writer |
|---------|----------------------------|
| 4-step model loading | Shared config module bootstraps Foundry Local |
| RAG-style retrieval | Product agent searches a local catalog |
| Agent Specialisation | 4 agents with distinct system prompts |
| Streaming output | Writer yields tokens in real-time |
| Structured hand-offs | Researcher → JSON, Editor → JSON decision |
| Feedback loops | Editor can trigger re-execution (max 2 retries) |

---

## Architecture

The Zava Creative Writer uses a **sequential pipeline with evaluator-driven feedback**. All three language implementations follow the same architecture:

![Zava Creative Writer Architecture](../images/part7-zava-architecture.png)

### The Four Agents

| Agent | Input | Output | Purpose |
|-------|-------|--------|---------|
| **Researcher** | Topic + optional feedback | `{"web": [{url, name, description}, ...]}` | Gathers background research via LLM |
| **Product Search** | Product context string | List of matching products | LLM-generated queries + keyword search against local catalog |
| **Writer** | Research + products + assignment + feedback | Streamed article text (split at `---`) | Drafts a magazine-quality article in real time |
| **Editor** | Article + writer's self-feedback | `{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}` | Reviews quality, triggers retry if needed |

### Pipeline Flow

1. **Researcher** receives the topic and produces structured research notes (JSON)
2. **Product Search** queries the local product catalog using LLM-generated search terms
3. **Writer** combines research + products + assignment into a streaming article, appending self-feedback after a `---` separator
4. **Editor** reviews the article and returns a JSON verdict:
   - `"accept"` → pipeline completes
   - `"revise"` → feedback is sent back to the Researcher and Writer (max 2 retries)

---

## Prerequisites

- Complete [Part 6: Multi-Agent Workflows](part6-multi-agent-workflows.md)
- Foundry Local CLI installed and `phi-3.5-mini` model downloaded

---

## Exercises

### Exercise 1 - Run the Zava Creative Writer

Choose your language and run the application:

<details>
<summary><strong>🐍 Python - FastAPI Web Service</strong></summary>

The Python version runs as a **web service** with a REST API, demonstrating how to build a production backend.

**Setup:**
```bash
cd zava-creative-writer-local/src/api
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Run:**
```bash
uvicorn main:app --reload
```

**Test it:**
```bash
curl -X POST http://localhost:8000/api/article \
  -H "Content-Type: application/json" \
  -d '{
    "research": "DIY home improvement trends",
    "products": "power tools and paints",
    "assignment": "Write an article about weekend renovation projects for DIY enthusiasts"
  }'
```

The response streams back as newline-delimited JSON messages showing each agent's progress.

</details>

<details>
<summary><strong>📦 JavaScript - Node.js CLI</strong></summary>

The JavaScript version runs as a **CLI application**, printing agent progress and the article directly to the console.

**Setup:**
```bash
cd zava-creative-writer-local/src/javascript
npm install
```

**Run:**
```bash
node main.mjs
```

You will see:
1. Foundry Local model loading (with progress bar if downloading)
2. Each agent executing in sequence with status messages
3. The article streamed to the console in real time
4. The editor's accept/revise decision

</details>

<details>
<summary><strong>💜 C# - .NET Console App</strong></summary>

The C# version runs as a **.NET console application** with the same pipeline and streaming output.

**Setup:**
```bash
cd zava-creative-writer-local/src/csharp
dotnet restore
```

**Run:**
```bash
dotnet run
```

Same output pattern as the JavaScript version - agent status messages, streamed article, and editor verdict.

</details>

---

### Exercise 2 - Study the Code Structure

Each language implementation has the same logical components. Compare the structures:

**Python** (`src/api/`):
| File | Purpose |
|------|---------|
| `foundry_config.py` | Shared Foundry Local manager, model, and client (4-step init) |
| `orchestrator.py` | Pipeline coordination with feedback loop |
| `main.py` | FastAPI endpoints (`POST /api/article`) |
| `agents/researcher/researcher.py` | LLM-based research with JSON output |
| `agents/product/product.py` | LLM-generated queries + keyword search |
| `agents/writer/writer.py` | Streaming article generation |
| `agents/editor/editor.py` | JSON-based accept/revise decision |

**JavaScript** (`src/javascript/`):
| File | Purpose |
|------|---------|
| `foundryConfig.mjs` | Shared Foundry Local config (4-step init with progress bar) |
| `main.mjs` | Orchestrator + CLI entry point |
| `researcher.mjs` | LLM-based research agent |
| `product.mjs` | LLM query generation + keyword search |
| `writer.mjs` | Streaming article generation (async generator) |
| `editor.mjs` | JSON accept/revise decision |
| `products.mjs` | Product catalog data |

**C#** (`src/csharp/`):
| File | Purpose |
|------|---------|
| `Program.cs` | Complete pipeline: model loading, agents, orchestrator, feedback loop |
| `ZavaCreativeWriter.csproj` | .NET 9 project with Foundry Local + OpenAI packages |

> **Design note:** Python separates each agent into its own file/directory (good for larger teams). JavaScript uses one module per agent (good for medium projects). C# keeps everything in a single file with local functions (good for self-contained examples). In production, choose the pattern that fits your team's conventions.

---

### Exercise 3 - Trace the Shared Configuration

Every agent in the pipeline shares a single Foundry Local model client. Study how this is set up in each language:

<details>
<summary><strong>🐍 Python - foundry_config.py</strong></summary>

```python
from foundry_local import FoundryLocalManager

MODEL_ALIAS = "phi-3.5-mini"

# Step 1: Create manager and start the Foundry Local service
manager = FoundryLocalManager()
manager.start_service()

# Step 2: Check if the model is already downloaded
cached = manager.list_cached_models()
catalog_info = manager.get_model_info(MODEL_ALIAS)
is_cached = any(m.id == catalog_info.id for m in cached) if catalog_info else False

if not is_cached:
    manager.download_model(MODEL_ALIAS, progress_callback=_on_progress)

# Step 3: Load the model into memory
manager.load_model(MODEL_ALIAS)
model_id = manager.get_model_info(MODEL_ALIAS).id

# Shared OpenAI client
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
```

All agents import `from foundry_config import client, model_id`.

</details>

<details>
<summary><strong>📦 JavaScript - foundryConfig.mjs</strong></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import { OpenAI } from "openai";

const manager = new FoundryLocalManager();
await manager.startService();

// Cache check → download → load (same 4-step pattern)
const cachedModels = await manager.listCachedModels();
const catalogInfo = await manager.getModelInfo(MODEL_ALIAS);
const isAlreadyCached = cachedModels.some((m) => m.id === catalogInfo?.id);
if (!isAlreadyCached) {
  await manager.downloadModel(MODEL_ALIAS, undefined, false, progressCallback);
}
const modelInfo = await manager.loadModel(MODEL_ALIAS);

const client = new OpenAI({ baseURL: manager.endpoint, apiKey: manager.apiKey });
export { client, modelId };
```

All agents import `{ client, modelId } from "./foundryConfig.mjs"`.

</details>

<details>
<summary><strong>💜 C# - top of Program.cs</strong></summary>

```csharp
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "ZavaCreativeWriter",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var catalogModel = await catalog.GetModelAsync(alias, default);
var isCached = await catalogModel.IsCachedAsync(default);
if (!isCached)
    await catalogModel.DownloadAsync(null, default);

await catalogModel.LoadAsync(default);
var key = new ApiKeyCredential("foundry-local");
var chatClient = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls[0] + "/v1")
}).GetChatClient(catalogModel.Id);
```

The `chatClient` is then passed to all agent functions in the same file.

</details>

> **Key pattern:** The 4-step model loading (`startService` → cache check → `downloadModel` → `loadModel`) ensures the user sees clear progress and the model is only downloaded once. This is a best practice for any Foundry Local application.

---

### Exercise 4 - Understand the Feedback Loop

The feedback loop is what makes this pipeline "smart" - the Editor can send work back for revision. Trace the logic:

```
Orchestrator:
  1. researcher.research(topic, "No Feedback")    ← first pass
  2. product.findProducts(productContext)
  3. writer.write(research, products, assignment)  ← streams article
  4. Split article at "---" → article + writerFeedback
  5. editor.edit(article, writerFeedback)

  WHILE editor says "revise" AND retryCount < 2:
    6. researcher.research(topic, editor.researchFeedback)  ← refined
    7. writer.write(research, products, editor.editorFeedback)
    8. editor.edit(newArticle, newWriterFeedback)
    9. retryCount++
```

**Questions to consider:**
- Why is the retry limit set to 2? What happens if you increase it?
- Why does the researcher get `researchFeedback` but the writer gets `editorFeedback`?
- What would happen if the editor always says "revise"?

---

### Exercise 5 - Modify an Agent

Try changing one agent's behaviour and observe how it affects the pipeline:

| Modification | What to change |
|-------------|----------------|
| **Stricter editor** | Change the editor's system prompt to always request at least one revision |
| **Longer articles** | Change the writer's prompt from "800-1000 words" to "1500-2000 words" |
| **Different products** | Add or modify products in the product catalog |
| **New research topic** | Change the default `researchContext` to a different subject |
| **JSON-only researcher** | Make the researcher return 10 items instead of 3-5 |

> **Tip:** Since all three languages implement the same architecture, you can make the same modification in whichever language you are most comfortable with.

---

### Exercise 6 - Add a Fifth Agent

Extend the pipeline with a new agent. Some ideas:

| Agent | Where in pipeline | Purpose |
|-------|-------------------|---------|
| **Fact-Checker** | After Writer, before Editor | Verify claims against the research data |
| **SEO Optimiser** | After Editor accepts | Add meta description, keywords, slug |
| **Illustrator** | After Editor accepts | Generate image prompts for the article |
| **Translator** | After Editor accepts | Translate the article to another language |

**Steps:**
1. Write the agent's system prompt
2. Create the agent function (matching the existing pattern in your language)
3. Insert it into the orchestrator at the right point
4. Update the output/logging to show the new agent's contribution

---

## How Foundry Local and the Agent Framework Work Together

This application demonstrates the recommended pattern for building multi-agent systems with Foundry Local:

| Layer | Component | Role |
|-------|-----------|------|
| **Runtime** | Foundry Local | Downloads, manages, and serves the model locally |
| **Client** | OpenAI SDK | Sends chat completions to the local endpoint |
| **Agent** | System prompt + chat call | specialised behaviour through focused instructions |
| **Orchestrator** | Pipeline coordinator | Manages data flow, sequencing, and feedback loops |
| **Framework** | Microsoft Agent Framework | Provides the `ChatAgent` abstraction and patterns |

The key insight: **Foundry Local replaces the cloud backend, not the application architecture.** The same agent patterns, orchestration strategies, and structured hand-offs that work with cloud-hosted models work identically with local models - you just point the client at `manager.endpoint` instead of an Azure endpoint.

---

## Key Takeaways

| Concept | What You Learned |
|---------|-----------------|
| Production architecture | How to structure a multi-agent app with shared config and separate agents |
| 4-step model loading | Best practice for initialising Foundry Local with user-visible progress |
| Agent Specialisation | Each of 4 agents has focused instructions and a specific output format |
| Streaming generation | Writer yields tokens in real time, enabling responsive UIs |
| Feedback loops | Editor-driven retry improves output quality without human intervention |
| Cross-language patterns | Same architecture works in Python, JavaScript, and C# |
| Local = production-ready | Foundry Local serves the same OpenAI-compatible API used in cloud deployments |

---

## Next Step

Continue to [Part 8: Evaluation-Led Development](part8-evaluation-led-development.md) to build a systematic evaluation framework for your agents, using golden datasets, rule-based checks, and LLM-as-judge scoring.
