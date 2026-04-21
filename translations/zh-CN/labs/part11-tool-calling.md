![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第11部分：使用本地模型调用工具

> **目标：** 使您的本地模型能够调用外部函数（工具），以便检索实时数据、执行计算或与API交互——所有操作均在您的设备上私密运行。

## 什么是工具调用？

工具调用（也称为<strong>函数调用</strong>）允许语言模型请求执行您定义的函数。模型不再只是猜测答案，而是识别到工具能提供帮助时，返回一个结构化请求，由您的代码执行该函数。您的应用程序运行该函数，发送结果回来，模型将该信息融入最终回答中。

![工具调用流程](../../../images/tool-calling-flow.svg)

此模式对于构建能够：

- <strong>查询实时数据</strong>（天气、股票价格、数据库查询）
- <strong>执行精确计算</strong>（数学、单位转换）
- <strong>执行操作</strong>（发送邮件、创建工单、更新记录）
- <strong>访问私有系统</strong>（内部API、文件系统）

的智能代理非常关键。

---

## 工具调用如何工作

工具调用流程包含四个阶段：

| 阶段 | 发生的事情 |
|-------|-------------|
| **1. 定义工具** | 使用JSON Schema描述可用函数——名称、描述和参数 |
| **2. 模型决定** | 模型接收您的消息和工具定义。如果工具有用，返回`tool_calls`响应而非文本回答 |
| **3. 本地执行** | 您的代码解析工具调用，运行函数并收集结果 |
| **4. 最终答案** | 您将工具结果发送回模型，模型生成最终回答 |

> **关键点：** 模型<strong>永远不会执行代码</strong>，它只会<em>请求</em>调用工具。您的应用程序决定是否执行请求——这让您完全掌控。

---

## 哪些模型支持工具调用？

并非所有模型都支持工具调用。在当前Foundry Local目录中，下列模型具有工具调用能力：

| 模型 | 大小 | 工具调用 |
|-------|------|:---:|
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

> **提示：** 本实验使用 **qwen2.5-0.5b** ——体积小（822 MB 下载）、速度快且工具调用支持可靠。

---

## 学习目标

在本实验结束时，您将能够：

- 解释工具调用模式及其对AI代理的重要性
- 使用OpenAI函数调用格式定义工具模式
- 处理多轮工具调用的对话流程
- 本地执行工具调用并将结果返回给模型
- 为工具调用场景选择合适的模型

---

## 先决条件

| 要求 | 详情 |
|-------------|---------|
| **Foundry Local CLI** | 已安装并配置在您的`PATH`中（[第1部分](part1-getting-started.md)） |
| **Foundry Local SDK** | 已安装Python、JavaScript或C# SDK（[第2部分](part2-foundry-local-sdk.md)） |
| <strong>支持工具调用的模型</strong> | qwen2.5-0.5b（将自动下载） |

---

## 练习

### 练习1 — 理解工具调用流程

在编写代码前，先学习此序列图：

![工具调用序列图](../../../images/tool-calling-sequence.svg)

**关键观察点：**

1. 您事先定义工具作为JSON Schema对象
2. 模型响应包含`tool_calls`而非普通内容
3. 每次工具调用有唯一的`id`，您返回结果时必须引用它
4. 模型生成最终答案时，能看到所有之前的消息<strong>加上</strong>工具结果
5. 单次响应中可能包含多个工具调用

> **讨论：** 为什么模型返回工具调用，而不是直接执行函数？这为安全性带来了哪些优势？

---

### 练习2 — 定义工具模式

工具使用标准的OpenAI函数调用格式定义。每个工具需要：

- **`type`**：始终为`"function"`
- **`function.name`**：描述性函数名（例如`get_weather`）
- **`function.description`**：清晰描述——模型根据此决定何时调用工具
- **`function.parameters`**：描述预期参数的JSON Schema对象

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

> **工具描述最佳实践：**
> - 具体明确：“获取指定城市当前天气”优于“获取天气”
> - 清楚描述参数：模型依据此填写正确值
> - 明确必需或可选参数——帮助模型决定询问内容

---

### 练习3 — 运行工具调用示例

每个语言示例定义了两个工具（`get_weather`和`get_population`），发送问题触发工具调用，本地执行工具并将结果返回，得到最终回答。

<details>
<summary><strong>🐍 Python</strong></summary>

**先决条件：**
```bash
cd python
python -m venv venv

# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS / Linux：
source venv/bin/activate

pip install -r requirements.txt
```

**运行：**
```bash
python foundry-local-tool-calling.py
```

**期望输出：**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>代码讲解</strong>（`python/foundry-local-tool-calling.py`）：

```python
# 将工具定义为函数架构的列表
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

# 使用工具发送 — 模型可能会返回 tool_calls 而不是内容
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# 检查模型是否想要调用工具
if response.choices[0].message.tool_calls:
    # 执行工具并将结果发送回去
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**先决条件：**
```bash
cd javascript
npm install
```

**运行：**
```bash
node foundry-local-tool-calling.mjs
```

**期望输出：**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>代码讲解</strong>（`javascript/foundry-local-tool-calling.mjs`）：

此示例使用原生Foundry Local SDK的`ChatClient`，而非OpenAI SDK，演示了`createChatClient()`方法的便捷性：

```javascript
// 直接从模型对象获取 ChatClient
const chatClient = model.createChatClient();

// 使用工具发送 — ChatClient 处理与 OpenAI 兼容的格式
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// 检查工具调用
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // 执行工具并发送结果回去
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**先决条件：**
```bash
cd csharp
dotnet restore
```

**运行：**
```bash
dotnet run toolcall
```

**期望输出：**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>代码讲解</strong>（`csharp/ToolCalling.cs`）：

C#使用`ChatTool.CreateFunctionTool`帮助定义工具：

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

### 练习4 — 工具调用对话流程

了解消息结构至关重要。以下展示了每个阶段的`messages`数组：

**阶段1 — 初始请求：**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**阶段2 — 模型返回`tool_calls`（非内容）：**
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

**阶段3 — 您添加助手消息和工具结果：**
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

**阶段4 — 模型用工具结果生成最终答案。**

> **重要提示：** 工具消息中的`tool_call_id`必须与工具调用的`id`匹配。这是模型将结果与请求关联的方式。

---

### 练习5 — 多工具调用

模型可在一次响应中请求多个工具调用。尝试更改用户消息以触发多次调用：

```python
# 在 Python 中 — 更改用户消息：
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// 在 JavaScript 中 — 更改用户消息：
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

模型应返回两个`tool_calls`：一个`get_weather`，一个`get_population`。您的代码已支持此场景，因为它会遍历所有工具调用。

> **试试吧：** 修改用户消息并重新运行示例。模型是否调用了两个工具？

---

### 练习6 — 添加您自己的工具

在示例中扩展一个新工具。例如，添加一个`get_time`工具：

1. 定义工具模式：
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

2. 添加执行逻辑：
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # 在真实应用中，使用时区库
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... 现有工具 ...
```

3. 将工具添加到`tools`数组并测试：“东京现在几点？”

> **挑战：** 添加一个执行计算的工具，例如`convert_temperature`，用于摄氏度与华氏度转换。测试句子：“把100°F转换成摄氏度。”

---

### 练习7 — 使用SDK的ChatClient调用工具（JavaScript）

JavaScript示例已使用SDK原生的`ChatClient`，而非OpenAI SDK。这是个方便功能，免去您自己构建OpenAI客户端的麻烦：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient 是直接从模型对象创建的
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat 接受工具作为第二个参数
const response = await chatClient.completeChat(messages, tools);
```

对比Python示例，它显式使用OpenAI SDK：

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

两种模式都有效。`ChatClient`更便捷；OpenAI SDK让您使用完整参数。

> **试试吧：** 修改JavaScript示例，使用OpenAI SDK替代`ChatClient`。您需要`import OpenAI from "openai"`，并用`manager.urls[0]`中端点构建客户端。

---

### 练习8 — 理解tool_choice

`tool_choice`参数控制模型是否<strong>必须</strong>使用工具，或可以自由选择：

| 值 | 行为 |
|-------|-----------|
| `"auto"` | 模型决定是否调用工具（默认） |
| `"none"` | 模型不会调用任何工具，即使有工具 |
| `"required"` | 模型必须至少调用一个工具 |
| `{"type": "function", "function": {"name": "get_weather"}}` | 模型必须调用指定工具 |

在Python示例中尝试各选项：

```python
# 强制模型调用 get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **注意：** 不是每个模型都支持所有`tool_choice`选项。如果模型不支持`"required"`，可能忽略此设置并表现为`"auto"`。

---

## 常见错误

| 问题 | 解决方案 |
|---------|----------|
| 模型从不调用工具 | 确认使用支持工具调用的模型（例如qwen2.5-0.5b）。参考上表。 |
| `tool_call_id`不匹配 | 始终使用工具调用响应中的`id`，不要硬编码。 |
| 模型在`arguments`中返回格式错误的JSON | 小型模型偶尔会生成无效JSON。使用`try/catch`包裹`JSON.parse()`。 |
| 模型调用不存在的工具 | 在`execute_tool`函数添加默认处理器。 |
| 无限工具调用循环 | 设置最大调用轮数（例如5轮），防止无限循环。 |

---

## 关键总结

1. <strong>工具调用</strong>让模型请求执行函数，而非猜答案
2. 模型<strong>从不执行代码</strong>，由您的应用决定运行内容
3. 工具定义为遵循OpenAI函数调用格式的<strong>JSON Schema</strong>对象
4. 对话采用<strong>多轮模式</strong>：用户、助手（tool_calls）、工具（结果）、助手（最终回答）
5. 总是使用<strong>支持工具调用的模型</strong>（Qwen 2.5，Phi-4-mini）
6. SDK的`createChatClient()`提供便捷方式，无需手动构建OpenAI客户端即可发起工具调用请求

---

继续查看[第12部分：为Zava创意写作器构建Web UI](part12-zava-ui.md)，为多代理管道添加基于浏览器的前端及实时流式传输。

---

[← 第10部分：自定义模型](part10-custom-models.md) | [第12部分：Zava Writer UI →](part12-zava-ui.md)