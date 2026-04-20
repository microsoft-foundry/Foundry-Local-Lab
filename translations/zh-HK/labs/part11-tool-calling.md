![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第11部分：使用本地模型的工具調用

> **目標：** 使您的本地模型能夠調用外部函數（工具），以便它可以檢索實時數據、執行計算或與API互動 — 全部在您的設備上私密運行。

## 什麼是工具調用？

工具調用（也稱為<strong>函數調用</strong>）允許語言模型請求執行您定義的函數。模型不再猜測答案，而是識別何時需要工具並返回結構化的請求，供您的代碼執行。您的應用程序執行該函數，將結果發送回模型，模型將該信息納入最終回應中。

![Tool-calling flow](../../../images/tool-calling-flow.svg)

此模式對構建智能代理至關重要，能讓它們：

- <strong>查詢實時數據</strong>（天氣、股價、數據庫查詢）
- <strong>執行精確計算</strong>（數學運算、單位轉換）
- <strong>採取行動</strong>（發送郵件、建立工單、更新紀錄）
- <strong>訪問私有系統</strong>（內部API、檔案系統）

---

## 工具調用如何運作

工具調用流程有四個階段：

| 階段 | 發生內容 |
|-------|-------------|
| **1. 定義工具** | 您用 JSON Schema 描述可用函數，包括名稱、描述和參數 |
| **2. 模型決定** | 模型接收您的訊息和工具定義。如果工具有用，模型返回 `tool_calls` 回應，而非文本答案 |
| **3. 本地執行** | 您的代碼解析工具調用，執行函數並收集結果 |
| **4. 最終答案** | 您將工具結果發送回模型，模型產生最終回應 |

> **重點：** 模型從不執行代碼，它只會<em>請求</em>調用工具。您的應用決定是否響應該請求 — 確保您始終擁有完全控制權。

---

## 哪些模型支持工具調用？

不是所有模型都支持工具調用。在當前 Foundry Local 目錄中，以下模型具備工具調用功能：

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

> **提示：** 本實驗使用 **qwen2.5-0.5b** — 體積輕（822 MB下載）、速度快，且具備穩定的工具調用支持。

---

## 學習目標

完成此實驗後您將能：

- 解釋工具調用模式及其對AI代理的重要性
- 用 OpenAI 函數調用格式定義工具架構
- 處理多回合的工具調用對話流程
- 本地執行工具調用，並將結果返回給模型
- 為工具調用場景選擇合適的模型

---

## 前置需求

| 需求 | 詳細資訊 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝並加入您的 `PATH`（參考[第1部分](part1-getting-started.md)） |
| **Foundry Local SDK** | 已安裝 Python、JavaScript 或 C# SDK（參考[第2部分](part2-foundry-local-sdk.md)） |
| <strong>支持工具調用的模型</strong> | qwen2.5-0.5b（將自動下載） |

---

## 練習

### 練習 1 — 理解工具調用流程

在撰寫代碼前，請學習此序列圖：

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**主要觀察點：**

1. 您事先將工具定義為 JSON Schema 物件
2. 模型回應包含 `tool_calls` 取代一般內容
3. 每個工具調用有唯一 `id`，回傳結果時必須用到
4. 模型生成最終答案時，會看到所有先前訊息 <em>以及</em> 工具結果
5. 一次回應中可以發生多個工具調用

> **討論：** 為何模型返回工具調用而非直接執行函數？這有何安全優勢？

---

### 練習 2 — 定義工具架構

工具採用標準 OpenAI 函數調用格式定義。每個工具需包含：

- **`type`**：固定為 `"function"`
- **`function.name`**：描述性函數名稱（例如 `get_weather`）
- **`function.description`**：清晰描述 — 模型依此判斷何時調用工具
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
> - 具體明確：「獲取指定城市的當前天氣」比「獲取天氣」更好
> - 清晰描述參數：模型依參數描述填寫正確值
> - 標註必填與選填參數 — 幫助模型判斷必要資訊

---

### 練習 3 — 執行工具調用範例

每個範例定義兩個工具（`get_weather` 與 `get_population`），發送會觸發工具的問題，本地執行工具，然後回傳結果取得最終答案。

<details>
<summary><strong>🐍 Python</strong></summary>

**前置需求：**
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

<strong>代碼解析</strong> (`python/foundry-local-tool-calling.py`)：

```python
# 將工具定義為一組函數結構
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

# 使用工具發送 — 模型可能返回工具調用而非內容
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# 檢查模型是否想調用工具
if response.choices[0].message.tool_calls:
    # 執行工具並將結果發送回來
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**前置需求：**
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

<strong>代碼解析</strong> (`javascript/foundry-local-tool-calling.mjs`)：

本範例使用 Foundry Local SDK 原生的 `ChatClient`，而非 OpenAI SDK，展示了 `createChatClient()` 方法的便利：

```javascript
// 直接從模型物件獲取 ChatClient
const chatClient = model.createChatClient();

// 使用工具發送 — ChatClient 處理兼容 OpenAI 的格式
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// 檢查工具調用
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // 執行工具並將結果發送回去
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**前置需求：**
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

<strong>代碼解析</strong> (`csharp/ToolCalling.cs`)：

C# 使用 `ChatTool.CreateFunctionTool` 助理函數定義工具：

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

### 練習 4 — 工具調用對話流程

理解訊息結構至關重要。以下為完整流程，展示每個階段的 `messages` 陣列：

**階段 1 — 初始請求：**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**階段 2 — 模型回應工具調用（非內容）：**
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

**階段 3 — 您添加助理訊息 AND 工具結果：**
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

**階段 4 — 模型使用工具結果產生最終答案。**

> **重要：** 工具訊息中的 `tool_call_id` 必須與工具調用的 `id` 匹配。模型依此關聯結果與請求。

---

### 練習 5 — 多重工具調用

模型可以在一次回應中請求多個工具。嘗試更改使用者訊息以觸發多重調用：

```python
# 喺 Python 入面 — 更改用戶訊息：
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// 在 JavaScript — 更改使用者訊息：
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

模型應返回兩個 `tool_calls` — 一個為 `get_weather`，另一個為 `get_population`。您的代碼已處理此情況，因為它會迴圈遍歷所有工具調用。

> **試試看：** 修改使用者訊息並重新執行範例。模型是否同時調用了兩個工具？

---

### 練習 6 — 增加您自己的工具

在任一範例中新增工具。例如添加一個 `get_time` 工具：

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
        # 喺一個真正嘅應用程式入面，使用時區庫
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... 現有嘅工具 ...
```

3. 將工具加入 `tools` 陣列並用「東京現在是幾點？」測試

> **挑戰：** 新增執行計算的工具，如 `convert_temperature`，執行攝氏與華氏轉換。用「100°F 轉換成攝氏溫度」測試。

---

### 練習 7 — 使用 SDK 的 ChatClient 進行工具調用（JavaScript）

JavaScript 範例已使用 SDK 原生的 `ChatClient`，非 OpenAI SDK。此為便捷功能，免去自行構建 OpenAI 客戶端：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient 是直接從模型對象創建的
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat 接受工具作為第二個參數
const response = await chatClient.completeChat(messages, tools);
```

與明確使用 OpenAI SDK 的 Python 方法相比：

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

兩種模式皆可。`ChatClient` 更便利；OpenAI SDK 則提供完整 OpenAI 參數。

> **試試看：** 修改 JavaScript 範例改用 OpenAI SDK 代替 `ChatClient`。您需 `import OpenAI from "openai"`，並用 `manager.urls[0]` 的端點構建客戶端。

---

### 練習 8 — 理解 tool_choice

`tool_choice` 參數控制模型是否<em>必須</em>使用工具或可自由選擇：

| 值 | 行為 |
|-------|-----------|
| `"auto"` | 模型自己決定是否調用工具（預設） |
| `"none"` | 模型不會調用任何工具，即使有提供 |
| `"required"` | 模型必須至少調用一個工具 |
| `{"type": "function", "function": {"name": "get_weather"}}` | 模型必須調用指定工具 |

在 Python 範例中嘗試每個選項：

```python
# 強制模型調用 get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **注意：** 並非所有 `tool_choice` 選項皆被每個模型支持。若模型不支持 `"required"`，它可能會忽略此設定，表現如 `"auto"`。

---

## 常見問題

| 問題 | 解決方案 |
|---------|----------|
| 模型從不調用工具 | 確認您使用的是支持工具調用的模型（例如 qwen2.5-0.5b）。請參見上表。 |
| `tool_call_id` 不匹配 | 始終使用工具調用回應中的 `id`，不要使用硬編碼值 |
| 模型返回錯誤格式的 `arguments` JSON | 較小模型偶爾產生無效 JSON。將 `JSON.parse()` 用 try/catch 包裹 |
| 模型調用不存在工具 | 在您的 `execute_tool` 函數中加入預設處理器 |
| 工具調用無限循環 | 設定最大回合數（如 5）防止無限迴圈 |

---

## 主要總結

1. <strong>工具調用</strong> 讓模型請求執行函數，而非猜答案
2. 模型<strong>永不執行代碼</strong>；決定執行權在您應用端
3. 工具以遵循 OpenAI 函數調用格式的 **JSON Schema** 物件定義
4. 對話採用 <strong>多回合模式</strong>：使用者 → 助理（tool_calls）→ 工具（結果）→ 助理（最終答案）
5. 請務必使用<strong>支持工具調用的模型</strong>（Qwen 2.5、Phi-4-mini）
6. SDK 的 `createChatClient()` 提供方便的工具調用方式，無需自行構建 OpenAI 客戶端

---

繼續閱讀 [第12部分：為 Zava 創意作家構建網頁介面](part12-zava-ui.md)，為多代理流程增添瀏覽器即時串流前端。

---

[← 第10部分：自訂模型](part10-custom-models.md) | [第12部分：Zava 撰寫介面 →](part12-zava-ui.md)