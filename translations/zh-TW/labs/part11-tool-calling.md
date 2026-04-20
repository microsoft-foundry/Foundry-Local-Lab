![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第11部分：使用本地模型的工具調用

> **目標：** 讓你的本地模型能夠調用外部函數（工具），以便它能檢索即時數據、執行計算或與 API 互動——所有這些都在你的設備上私密運行。

## 什麼是工具調用？

工具調用（也稱為 <strong>函數調用</strong>）讓語言模型請求執行你定義的函數。模型不是猜測答案，而是識別何時需要工具並返回結構化的請求供你的程式執行。你的應用程式執行該函數，將結果發回，模型便將該資訊納入其最終回應中。

![Tool-calling flow](../../../images/tool-calling-flow.svg)

此模式對構建能夠：

- <strong>查詢即時數據</strong>（天氣、股價、資料庫查詢）
- <strong>執行精確計算</strong>（數學、單位轉換）
- <strong>採取操作</strong>（發送郵件、建立工單、更新紀錄）
- <strong>存取私有系統</strong>（內部 API、檔案系統）

的代理人至關重要。

---

## 工具調用如何運作

工具調用流程有四個階段：

| 階段 | 發生什麼事 |
|-------|-------------|
| **1. 定義工具** | 使用 JSON Schema 描述可用函數—名稱、描述和參數 |
| **2. 模型決定** | 模型收到你的訊息及工具定義。如果工具會有幫助，模型返回 `tool_calls` 回應，而非文字答案 |
| **3. 本地執行** | 你的程式解析工具調用，執行函數並收集結果 |
| **4. 最終答案** | 你將工具結果發回模型，模型產生最終答覆 |

> **重點：** 模型從不執行程式碼。它只 <em>請求</em> 呼叫工具。你的應用程式決定是否執行該請求—這讓你保持完全控制權。

---

## 哪些模型支援工具調用？

並非所有模型都支持工具調用。在目前 Foundry Local 目錄中，以下模型具備工具調用功能：

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

> **提示：** 本實驗使用 **qwen2.5-0.5b** — 體積較小（822 MB 下載）、快速且具可靠的工具調用支持。

---

## 學習目標

完成本實驗後，你將能夠：

- 解釋工具調用模式及其對 AI 代理人重要性
- 使用 OpenAI 函數調用格式定義工具結構
- 處理多回合工具調用對話流程
- 在本地執行工具調用並將結果回傳給模型
- 為工具調用場景選擇合適的模型

---

## 先決條件

| 需求 | 詳細 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝並加入你的 `PATH`（[第1部分](part1-getting-started.md)） |
| **Foundry Local SDK** | 已安裝 Python、JavaScript 或 C# SDK（[第2部分](part2-foundry-local-sdk.md)） |
| <strong>支持工具調用的模型</strong> | qwen2.5-0.5b（會自動下載） |

---

## 練習

### 練習1 — 了解工具調用流程

寫代碼之前，先查看此序列圖：

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**主要觀察點：**

1. 你先行定義工具作為 JSON Schema 對象
2. 模型回應包含 `tool_calls`，非一般內容
3. 每次工具調用具有唯一 `id`，返回結果時須引用
4. 模型產生最終答案時會看到之前所有訊息 <em>加上</em> 工具結果
5. 一次回應中可包含多次工具調用

> **討論：** 為什麼模型會返回工具調用而非直接執行函數？這種方式有哪些安全優勢？

---

### 練習2 — 定義工具結構

工具以標準 OpenAI 函數調用格式定義。每個工具需要：

- **`type`**：固定為 `"function"`
- **`function.name`**：具描述性的函數名稱（例如 `get_weather`）
- **`function.description`**：清晰描述—模型靠此決定何時呼叫工具
- **`function.parameters`**：描述預期參數的 JSON Schema 對象

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

> **工具描述最佳實務：**
> - 具體明確：「獲取指定城市的當前天氣」比「獲取天氣」更佳
> - 清楚描述參數：模型會讀取這些描述以填入正確數值
> - 區分必需與可選參數—幫助模型判斷要詢問什麼

---

### 練習3 — 執行工具調用範例

每個語言範例定義兩個工具（`get_weather` 與 `get_population`），發送會觸發工具的問題，本地執行工具並將結果發回以獲得最終答案。

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

**執行：**
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

<strong>程式碼導覽</strong>（`python/foundry-local-tool-calling.py`）：

```python
# 將工具定義為函數結構列表
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

# 與工具一起發送 — 模型可能會返回 tool_calls 而非內容
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# 檢查模型是否想呼叫工具
if response.choices[0].message.tool_calls:
    # 執行工具並將結果發回
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

**執行：**
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

<strong>程式碼導覽</strong>（`javascript/foundry-local-tool-calling.mjs`）：

範例使用 Foundry Local SDK 內建的 `ChatClient`，不需依賴 OpenAI SDK，展示了 `createChatClient()` 方法的便利性：

```javascript
// 從模型物件直接取得 ChatClient
const chatClient = model.createChatClient();

// 使用工具發送 — ChatClient 處理與 OpenAI 相容的格式
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// 檢查工具呼叫
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // 執行工具並將結果回傳
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

**執行：**
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

<strong>程式碼導覽</strong>（`csharp/ToolCalling.cs`）：

C# 利用 `ChatTool.CreateFunctionTool` 輔助方法定義工具：

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

理解訊息結構十分重要。以下是完整流程，顯示每階段的 `messages` 陣列：

**階段1 — 初始請求：**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**階段2 — 模型回應 `tool_calls`（非內容）：**
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

**階段3 — 你加入助理訊息及工具結果：**
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

**階段4 — 模型使用工具結果產生最終回答。**

> **重要：** 工具訊息中的 `tool_call_id` 必須與工具調用的 `id` 相符，這是模型將結果與請求關聯的方式。

---

### 練習5 — 多次工具調用

模型可於一次回應中請求多次工具調用。嘗試改變使用者訊息以觸發多次呼叫：

```python
# 在 Python 中 — 更改用戶訊息：
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// 在 JavaScript 中 — 更改使用者訊息：
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

模型應返回兩個 `tool_calls` — 一個是 `get_weather`，另一個是 `get_population`。你的程式已有迴圈處理多個工具調用。

> **試試看：** 修改使用者訊息再執行範例。模型是否呼叫了兩個工具？

---

### 練習6 — 新增你自己的工具

擴充其中一個範例，加上一個新工具。例如，新增一個 `get_time` 工具：

1. 定義工具結構：
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

2. 加入執行邏輯：
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # 在實際應用中，請使用時區庫
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... 現有工具 ...
```

3. 將工具加入 `tools` 陣列並用「東京現在幾點？」測試

> **挑戰：** 新增一個執行計算的工具，例如 `convert_temperature`，可在攝氏與華氏間轉換。用「將100°F轉換成攝氏」測試。

---

### 練習7 — 使用 SDK 的 ChatClient 進行工具調用（JavaScript）

JavaScript 範例已用 SDK 原生的 `ChatClient`，而非 OpenAI SDK。這是便利功能，免去自己構建 OpenAI 用戶端：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient 是直接從模型物件建立的
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat 接受工具作為第二個參數
const response = await chatClient.completeChat(messages, tools);
```

比較 Python 範例明確使用 OpenAI SDK：

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

兩種模式均有效。`ChatClient` 更方便；OpenAI SDK 可使用完整參數設定。

> **試試看：** 修改 JavaScript 範例改用 OpenAI SDK 替代 `ChatClient`。你需要 `import OpenAI from "openai"` 並使用 `manager.urls[0]` 建構用戶端。

---

### 練習8 — 理解 tool_choice

`tool_choice` 參數控制模型是否 <em>必須</em> 使用工具或可自由選擇：

| 值 | 行為 |
|-------|-----------|
| `"auto"` | 模型決定是否呼叫工具（預設） |
| `"none"` | 模型不呼叫任何工具，即使提供 |
| `"required"` | 模型必須呼叫至少一個工具 |
| `{"type": "function", "function": {"name": "get_weather"}}` | 模型必須呼叫指定工具 |

嘗試在 Python 範例中使用各選項：

```python
# 強制模型呼叫 get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **注意：** 並非所有模型都支持所有 `tool_choice` 選項。如果模型不支援 `"required"`，可能會忽略該設定並以 `"auto"` 行為運作。

---

## 常見問題

| 問題 | 解決方法 |
|---------|----------|
| 模型不呼叫工具 | 確認使用的是支持工具調用的模型（如 qwen2.5-0.5b）。參考上表。 |
| `tool_call_id` 不匹配 | 一定要使用工具調用回應中的 `id`，不要使用硬編碼值 |
| 模型回傳 `arguments` 中的 JSON 格式錯誤 | 較小模型偶爾產生無效 JSON，使用 `JSON.parse()` 時包裹 try/catch |
| 模型呼叫不存在的工具 | 在你的 `execute_tool` 函式中加入預設處理器 |
| 工具調用無限迴圈 | 設定最大回合數（如5回合）以防止無限迴圈 |

---

## 主要收穫

1. <strong>工具調用</strong> 讓模型請求執行函數，而非純猜測答案
2. 模型 <strong>從不執行程式碼</strong>；由你的應用程式決定執行內容
3. 工具以遵循 OpenAI 函數調用格式的 **JSON Schema** 定義
4. 對話採用 <strong>多回合模式</strong>：使用者 → 助理（tool_calls）→ 工具（結果）→ 助理（最終答案）
5. 始終選用 <strong>支持工具調用的模型</strong>（Qwen 2.5、Phi-4-mini）
6. SDK 的 `createChatClient()` 提供方便方式發送工具調用請求，無需手動構建 OpenAI 用戶端

---

繼續閱讀 [第12部分：為 Zava 創意寫作助手構建網頁界面](part12-zava-ui.md)，為多代理管線加入瀏覽器前端並實現即時串流。

---

[← 第10部分：自訂模型](part10-custom-models.md) | [第12部分：Zava Writer UI →](part12-zava-ui.md)