![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 11: Tool Calling with Local Models

> **Goal:** Make your local model fit to call outside functions (tools) so e fit find real-time data, do calculations, or interact with APIs — all dey run privately for your device.

## Wetin Be Tool Calling?

Tool calling (wey dem dey call **function calling** too) na im dey allow language model request say make e run functions wey you define. No be to dey guess answer, the model dey sabi when tool go help and e go return structured request for your code make e execute. Your app go run the function, send result back, and model go use that info take finalize answer.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Dis kind pattern na important for to build agents wey fit:

- **Find live data** (weather, stock prices, database queries)
- **Do correct calculations** (maths, unit conversions)
- **Make actions** (send emails, create tickets, update records)
- **Enter private systems** (internal APIs, file systems)

---

## How Tool Calling Dey Work

The tool-calling flow get four steps:

| Stage | Wetin Dey Happen |
|-------|-----------------|
| **1. Define tools** | You go explain available functions dey use JSON Schema — name, description, and parameters |
| **2. Model decides** | The model go receive your message plus the tool definitions. If tool fit help, e go return `tool_calls` response instead of text answer |
| **3. Execute locally** | Your code go read the tool call, run the function, and gather the result |
| **4. Final answer** | You send the tool result back to the model, wey go give final answer |

> **Important point:** The model no dey run code. E just dey *request* make tool call happen. Your app na im dey decide if e go do am — dis one dey give you full control.

---

## Which Models Fit Tool Calling?

No be all model dey support tool calling. For current Foundry Local catalogue, these models get tool-calling power:

| Model | Size | Tool Calling |
|-------|------|:------------:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **Tip:** For dis lab we go use **qwen2.5-0.5b** — e small (822 MB to download), sharp sharp, and e get better tool-calling support.

---

## Wetin You Go Learn

After you finish dis lab, you go fit:

- Explain tool-calling pattern and why e matter for AI agents
- Define tool schemas dey use OpenAI function-calling format
- Handle multi-turn tool-calling conversation flow
- Run tool calls locally and send results back to the model
- Choose correct model for tool-calling cases

---

## Wetin You Need Before You Start

| Requirement | Details |
|-------------|---------|
| **Foundry Local CLI** | Installed and dey your `PATH` ([Part 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript, or C# SDK installed ([Part 2](part2-foundry-local-sdk.md)) |
| **One tool-calling model** | qwen2.5-0.5b (e go download automatically) |

---

## Exercises

### Exercise 1 — Understand Tool-Calling Flow

Before you start write code, check this sequence diagram:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Main things to note:**

1. You define tools before as JSON Schema objects
2. The model response get `tool_calls` instead of normal content
3. Each tool call get unique `id` wey you go use reference when you return results
4. The model go see all previous messages *plus* the tool results when e dey generate final answer
5. E fit call many tools for one response

> **Discussion:** Why model dey return tool calls instead of to run functions one time? Wetin be security advantages wey e get?

---

### Exercise 2 — Define Tool Schemas

Tools dey defined by using the usual OpenAI function-calling format. Each tool need:

- **`type`**: Always `"function"`
- **`function.name`**: One descriptive function name (like `get_weather`)
- **`function.description`**: Clear description — model go use am decide when to call tool
- **`function.parameters`**: JSON Schema object wey describe expected arguments

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **Best practices for tool descriptions:**
> - Make e specific: "Get di current weather for given city" better pass "Get weather"
> - Describe parameters clear clear: model dey read dis one to know correct values to enter
> - Mark wetin mandatory and wetin optional — dis one dey help model decide wetin to ask

---

### Exercise 3 — Run Tool-Calling Examples

Each language example get two tools (`get_weather` and `get_population`), e send question wey trigger tool use, run tool locally, and send result back for final answer.

<details>
<summary><strong>🐍 Python</strong></summary>

**Wetin you need:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Run am:**
```bash
python foundry-local-tool-calling.py
```

**Wetin you go see:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code explanation** (`python/foundry-local-tool-calling.py`):

```python
# Define tools as a list of function schemas
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# Send wit tools — di model fit return tool_calls instead of content
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Check if di model want call tool
if response.choices[0].message.tool_calls:
    # Run di tool and send di result back
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Wetin you need:**
```bash
cd javascript
npm install
```

**Run am:**
```bash
node foundry-local-tool-calling.mjs
```

**Wetin you go see:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code explanation** (`javascript/foundry-local-tool-calling.mjs`):

Dis example dey use native Foundry Local SDK `ChatClient` instead of OpenAI SDK, dey show how `createChatClient()` method dey easy:

```javascript
// Comot ChatClient direct from di model object
const chatClient = model.createChatClient();

// Send wit tools — ChatClient dey handle di OpenAI-compatible format
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Check for tool calls
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Run tools and send back di results
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Wetin you need:**
```bash
cd csharp
dotnet restore
```

**Run am:**
```bash
dotnet run toolcall
```

**Wetin you go see:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Code explanation** (`csharp/ToolCalling.cs`):

C# dey use `ChatTool.CreateFunctionTool` helper to define tools:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### Exercise 4 — Tool-Calling Conversation Flow

To sabi message structure na important. Here be full flow, wey show `messages` array for each step:

**Stage 1 — Initial request:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Stage 2 — Model answer with tool_calls (no content):**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**Stage 3 — You add assistant message PLUS tool result:**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**Stage 4 — Model produce final answer from tool result.**

> **Important:** The `tool_call_id` for tool message must match `id` from tool call. Dis na how model connect results with requests.

---

### Exercise 5 — Many Tool Calls

Model fit ask for many tool calls for one response. Try change user message to trigger many calls:

```python
# For Python — change di user message:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// For JavaScript — change di user message:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Model go return two `tool_calls` — one for `get_weather` and one for `get_population`. Your code don support dis one because e dey loop for all tool calls.

> **Try am:** Change user message and run the sample again. Model go call both tools?

---

### Exercise 6 — Add Your Own Tool

Make sample get new tool. For example, add `get_time` tool:

1. Define tool schema:
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. Add code wey go run am:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # For real app, make you use timezone library
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... tools wey dey already ...
```

3. Add tool to `tools` array and test with: "What time is it in Tokyo?"

> **Challenge:** Add tool wey do calculation like `convert_temperature` wey fit change Celsius to Fahrenheit or the other way. Test am with: "Convert 100°F to Celsius."

---

### Exercise 7 — Tool Calling with SDK's ChatClient (JavaScript)

The JavaScript example already dey use SDK native `ChatClient` instead OpenAI SDK. Na easier way wey no need you construct OpenAI client yourself:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient dem create am straight from di model object
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat dey accept tools as di second parameter
const response = await chatClient.completeChat(messages, tools);
```

Compare am with Python way wey use OpenAI SDK directly:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Both ways correct. `ChatClient` better for ease; OpenAI SDK dey give full access to all OpenAI parameters.

> **Try am:** Change JavaScript sample to use OpenAI SDK, no `ChatClient`. You go need `import OpenAI from "openai"` and build client from `manager.urls[0]`.

---

### Exercise 8 — Understand `tool_choice`

The `tool_choice` parameter dey control whether model *must* use tool or fit choose freely:

| Value | How e Dey Work |
|-------|----------------|
| `"auto"` | Model go decide if e call tool (na default) |
| `"none"` | Model no go call any tools, even if you provide am |
| `"required"` | Model must call at least one tool |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model must call that particular tool |

Try all options for Python sample:

```python
# Make di model call get_weather for sure
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Note:** No be all model go support every `tool_choice` option. If model no support `"required"`, e fit just ignore am and behave like `"auto"`.

---

## Common Wahala

| Problem | How to Solve |
|---------|--------------|
| Model no dey call tools | Make sure you dey use tool-calling model (example qwen2.5-0.5b). See table above. |
| `tool_call_id` no match | Always use `id` from tool call response, no hardcode am |
| Model dey return broken JSON for `arguments` | Small models fit produce bad JSON sometimes. Use `JSON.parse()` inside try/catch |
| Model call tool wey no exist | Put default handler for your `execute_tool` function |
| Tool calling dey run forever | Set max number of rounds (like 5) to stop endless loop |

---

## Main Points

1. **Tool calling** dey allow models request function runs instead of to guess answer
2. Model **no dey run code**; your app dey decide wetin to run
3. Tools defined as **JSON Schema** objects following OpenAI function-calling format
4. Conversation get **multi-turn pattern**: user, then assistant (tool_calls), then tool (results), then assistant (final answer)
5. Always use **model wey support tool calling** (Qwen 2.5, Phi-4-mini)
6. SDK's `createChatClient()` na easier way to make tool-calling requests without building OpenAI client

---

Continue go [Part 12: Building a Web UI for the Zava Creative Writer](part12-zava-ui.md) to add browser-based frontend to the multi-agent pipeline with real-time streaming.

---

[← Part 10: Custom Models](part10-custom-models.md) | [Part 12: Zava Writer UI →](part12-zava-ui.md)