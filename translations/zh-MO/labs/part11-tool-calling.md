![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第11部分：使用本地模型進行工具調用

> **目標：** 使你的本地模型能夠調用外部函數（工具），以檢索實時數據、執行計算或與API交互——所有這些都在你的裝置上私密運行。

## 什麼是工具調用？

工具調用（亦稱為<strong>函數調用</strong>）讓語言模型請求執行你定義的函數。模型不再猜測答案，而是在判斷工具有用時返回一個結構化請求，由你的代碼執行該工具。你的應用程式執行函數，回傳結果，模型將該信息融入最終回應中。

![Tool-calling flow](../../../images/tool-calling-flow.svg)

這一模式對構建能夠：

- <strong>查詢實時數據</strong>（天氣、股價、資料庫查詢）
- <strong>執行精確計算</strong>（數學、單位轉換）
- <strong>採取行動</strong>（發送電郵、創建工單、更新記錄）
- <strong>訪問私有系統</strong>（內部API、檔案系統）

的代理至關重要。

---

## 工具調用如何運作

工具調用流程分為四個階段：

| 階段 | 發生了什麼 |
|-------|-------------|
| **1. 定義工具** | 使用 JSON Schema 描述可用函數——名稱、描述及參數 |
| **2. 模型決定** | 模型接收用戶訊息及工具定義。若工具有用，回傳 `tool_calls` 而非文字答案 |
| **3. 本地執行** | 你的代碼解析工具調用，執行函數並收集結果 |
| **4. 最終答案** | 將工具結果返回給模型，模型據此產生最終回應 |

> **關鍵點：** 模型本身不執行程式碼，它只<em>請求</em>調用工具。由你的應用程式決定是否執行此請求——這樣你擁有完全控制權。

---

## 哪些模型支持工具調用？

並非所有模型都支持工具調用。目前 Foundry Local 目錄中，以下模型具備工具調用能力：

| 模型 | 大小 | 工具調用 |
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

> **提示：** 本實驗使用 **qwen2.5-0.5b** — 體積小（822 MB下載），速度快，並且對工具調用支持穩定。

---

## 學習目標

完成本實驗後，你將能夠：

- 解釋工具調用模式及其對AI代理的重要性
- 使用 OpenAI 函數調用格式定義工具架構
- 處理多回合工具調用對話流程
- 本地執行工具調用並將結果返回給模型
- 為工具調用場景選擇合適的模型

---

## 先決條件

| 要求 | 詳細資訊 |
|-------------|---------|
| **Foundry Local CLI** | 安裝並加入你的 `PATH` ([第1部分](part1-getting-started.md)) |
| **Foundry Local SDK** | 已安裝 Python、JavaScript 或 C# SDK ([第2部分](part2-foundry-local-sdk.md)) |
| <strong>具工具調用能力的模型</strong> | qwen2.5-0.5b（將自動下載） |

---

## 練習

### 練習1 — 了解工具調用流程

在寫代碼前，研究此序列圖：

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**關鍵觀察點：**

1. 你提前用 JSON Schema 物件定義工具
2. 模型回應包含 `tool_calls` 代替一般內容
3. 每個工具調用有唯一 `id`，你須在回傳結果時引用
4. 模型在生成最終答案時，看到所有前面訊息<em>及</em>工具結果
5. 單次回應中可有多個工具調用

> **討論：** 為何模型回傳工具調用而非直接執行函數？這帶來哪些安全優勢？

---

### 練習2 — 定義工具架構

工具使用標準 OpenAI 函數調用格式定義。每個工具需包含：

- **`type`**：永遠為 `"function"`
- **`function.name`**：描述性函數名稱（如 `get_weather`）
- **`function.description`**：清晰描述—模型用此判斷何時調用工具
- **`function.parameters`**：描述預期參數的 JSON Schema 物件

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

> **工具描述最佳實踐：**
> - 具體明確："取得指定城市的當前天氣" 比 "取得天氣" 好
> - 清楚描述參數：模型根據此補全合適參數值
> - 標明必填與選填參數——幫助模型判斷需詢問哪些資料

---

### 練習3 — 運行工具調用範例

每種語言範例定義兩個工具（`get_weather` 和 `get_population`），發送觸發工具使用的問題，於本地執行工具，並將結果返回以獲最終答案。

<details>
<summary><strong>🐍 Python</strong></summary>

**先決條件：**
```bash
cd python
python -m venv venv

# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS / Linux：
source venv/bin/activate

pip install -r requirements.txt
```

**運行：**
```bash
python foundry-local-tool-calling.py
```

**預期輸出：**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>程式碼解說</strong>（`python/foundry-local-tool-calling.py`）：

```python
# 將工具定義為函數結構的列表
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

# 使用工具發送 — 模型可能返回 tool_calls 而非內容
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# 檢查模型是否想調用工具
if response.choices[0].message.tool_calls:
    # 執行工具並將結果返回
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**先決條件：**
```bash
cd javascript
npm install
```

**運行：**
```bash
node foundry-local-tool-calling.mjs
```

**預期輸出：**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>程式碼解說</strong>（`javascript/foundry-local-tool-calling.mjs`）：

此範例使用 Foundry Local SDK 的原生 `ChatClient`，而非 OpenAI SDK，展示了 `createChatClient()` 方法的方便性：

```javascript
// 從模型物件直接獲取 ChatClient
const chatClient = model.createChatClient();

// 使用工具發送 — ChatClient 處理與 OpenAI 相容的格式
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// 檢查工具呼叫
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // 執行工具並將結果發送回去
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**先決條件：**
```bash
cd csharp
dotnet restore
```

**運行：**
```bash
dotnet run toolcall
```

**預期輸出：**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>程式碼解說</strong>（`csharp/ToolCalling.cs`）：

C# 使用 `ChatTool.CreateFunctionTool` 輔助工具來定義工具：

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

### 練習4 — 工具調用對話流程

理解訊息結構至關重要。以下展示各階段的完整 `messages` 陣列：

**階段1 — 初始請求：**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**階段2 — 模型以 tool_calls 回應（非內容）：**
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

**階段3 — 你新增助理訊息及工具結果：**
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

**階段4 — 模型使用工具結果產生最終答案。**

> **重要：** 工具訊息中的 `tool_call_id` 必須與工具調用中的 `id` 相符。這是模型將結果與請求關聯的方式。

---

### 練習5 — 多重工具調用

模型可在單次回應中請求多個工具調用。試著更改用戶訊息以觸發多次調用：

```python
# 在 Python — 更改用戶訊息:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// 在 JavaScript — 更改用戶訊息：
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

模型應回傳兩個 `tool_calls`——一個是 `get_weather`，另一個是 `get_population`。你的程式碼已涵蓋這種情況，因為它會迴圈處理所有工具調用。

> **試試看：** 修改用戶訊息再運行範例。模型會同時調用兩個工具嗎？

---

### 練習6 — 添加你自己的工具

擴充其中一個範例，新增工具。例如，添加 `get_time` 工具：

1. 定義工具架構：
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

2. 新增執行邏輯：
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # 喺一個真實應用程式入面，使用時區函式庫
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... 現有嘅工具 ...
```

3. 將工具加入 `tools` 陣列，並測試："東京現在幾點？"

> **挑戰：** 添加一個計算工具，如 `convert_temperature`，可執行攝氏與華氏間轉換。用語句測試："將100°F轉為攝氏是多少？"

---

### 練習7 — 使用 SDK 的 ChatClient 進行工具調用（JavaScript）

JavaScript 範例已用 SDK 原生的 `ChatClient`，而非 OpenAI SDK。這為方便起見，無需自行建立 OpenAI 客戶端：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient 是直接從模型對象創建的
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat 接受工具作為第二個參數
const response = await chatClient.completeChat(messages, tools);
```

與 Python 範例明確使用 OpenAI SDK 相比：

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

兩種方式都有效。`ChatClient` 更方便；OpenAI SDK 提供完整參數選項。

> **試試看：** 將 JavaScript 範例改用 OpenAI SDK 替代 `ChatClient`。你須加入 `import OpenAI from "openai"` 並用 `manager.urls[0]` 建立客戶端。

---

### 練習8 — 理解 tool_choice

`tool_choice` 參數控制模型是否<em>必須</em>使用工具或可自由選擇：

| 值 | 行為 |
|-------|-----------|
| `"auto"` | 模型決定是否調用工具（預設） |
| `"none"` | 模型不會調用任何工具，即使提供了 |
| `"required"` | 模型必須至少調用一個工具 |
| `{"type": "function", "function": {"name": "get_weather"}}` | 模型必須調用指定工具 |

嘗試在 Python 範例中使用各種選項：

```python
# 強制模型調用 get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **注意：** 並非所有模型均支持每種 `tool_choice` 選項。若模型不支持 `"required"`，可能會忽略設定並採用 `"auto"` 行為。

---

## 常見陷阱

| 問題 | 解決方法 |
|---------|----------|
| 模型從不調用工具 | 確保使用具工具調用能力的模型（如 qwen2.5-0.5b）。查看上述表格。 |
| `tool_call_id` 不匹配 | 始終使用工具調用回應中的 `id`，避免硬編碼 |
| 模型在 `arguments` 中返回格式錯誤的 JSON | 較小模型偶爾輸出無效 JSON。在使用 `JSON.parse()` 時實施 try/catch |
| 模型調用不存在的工具 | 在你的 `execute_tool` 函式中加入預設處理器 |
| 工具調用無限循環 | 設定最大回合數（例如5）以防止失控迴圈 |

---

## 重要摘要

1. <strong>工具調用</strong>讓模型請求函數執行，而非猜答案
2. 模型<strong>不執行代碼</strong>；由應用程式決定執行內容
3. 工具使用遵循 OpenAI 函數調用格式的<strong>JSON Schema</strong>物件定義
4. 對話遵循<strong>多回合模式</strong>：用戶 → 助理（tool_calls）→ 工具（結果）→ 助理（最終答案）
5. 請始終使用<strong>支持工具調用的模型</strong>（Qwen 2.5、Phi-4-mini）
6. SDK 的 `createChatClient()` 提供無需建立 OpenAI 客戶端即可方便調用工具的方式

---

繼續閱讀 [第12部分：為 Zava 創意寫手打造 Web UI](part12-zava-ui.md)，為多代理管線增加瀏覽器前端，即時串流展示。

---

[← 第10部分：自訂模型](part10-custom-models.md) | [第12部分：Zava Writer UI →](part12-zava-ui.md)