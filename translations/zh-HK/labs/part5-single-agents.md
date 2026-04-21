![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第五部分：使用代理框架構建 AI 代理

> **目標：** 通過 Foundry Local 使用本地模型，構建具有持續指令和定義角色的第一個 AI 代理。

## 什麼是 AI 代理？

AI 代理將語言模型包裝在<strong>系統指令</strong>中，以定義其行為、個性和限制。與單次聊天完成調用不同，代理提供：

- <strong>角色</strong> - 一致的身份（「你是一位樂於助人的代碼審查員」）
- <strong>記憶</strong> - 跨多輪對話的歷史
- <strong>專精</strong> - 由精心設計的指令驅動的專注行為

![ChatAgent Pattern](../../../translated_images/zh-HK/part5-agent-pattern.36289d1421169525.webp)

---

## 微軟代理框架

<strong>微軟代理框架</strong>（AGF）提供了一個標準化代理抽象，可以跨不同模型後端運作。在本次工作坊中，我們將其與 Foundry Local 搭配使用，因此所有操作均在您的機器上執行，無需雲端。

| 概念 | 說明 |
|---------|-------------|
| `FoundryLocalClient` | Python：處理服務啟動、模型下載/加載，並創建代理 |
| `client.as_agent()` | Python：從 Foundry Local 客戶端創建代理 |
| `AsAIAgent()` | C#：`ChatClient` 的擴充方法 - 創建 `AIAgent` |
| `instructions` | 形塑代理行為的系統提示 |
| `name` | 可讀標籤，在多代理場景中有用 |
| `agent.run(prompt)` / `RunAsync()` | 發送用戶訊息並返回代理回應 |

> **注意：** 代理框架擁有 Python 和 .NET SDK。對於 JavaScript，我們實現了一個輕量的 `ChatAgent` 類，直接使用 OpenAI SDK 模擬相同模式。

---

## 練習

### 練習 1 - 理解代理模式

在編寫代碼之前，先學習代理的主要組件：

1. <strong>模型客戶端</strong> - 連接 Foundry Local 的 OpenAI 相容 API
2. <strong>系統指令</strong> - 「個性」提示
3. <strong>運行迴圈</strong> - 發送用戶輸入，接收輸出

> **思考：** 系統指令與普通用戶訊息有何不同？如果更改系統指令會發生什麼？

---

### 練習 2 - 運行單一代理範例

<details>
<summary><strong>🐍 Python</strong></summary>

**先決條件：**
```bash
cd python
python -m venv venv

# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS：
source venv/bin/activate

pip install -r requirements.txt
```

**執行：**
```bash
python foundry-local-with-agf.py
```

<strong>程式碼解析</strong> (`python/foundry-local-with-agf.py`)：

```python
import asyncio
from agent_framework_foundry_local import FoundryLocalClient

async def main():
    alias = "phi-4-mini"

    # FoundryLocalClient 處理服務啟動、模型下載和加載
    client = FoundryLocalClient(model_id=alias)
    print(f"Client Model ID: {client.model_id}")

    # 使用系統指令建立代理
    agent = client.as_agent(
        name="Joker",
        instructions="You are good at telling jokes.",
    )

    # 非串流：一次獲取完整回應
    result = await agent.run("Tell me a joke about a pirate.")
    print(f"Agent: {result}")

    # 串流：隨結果生成即時獲取
    async for chunk in agent.run("Tell me another joke.", stream=True):
        if chunk.text:
            print(chunk.text, end="", flush=True)

asyncio.run(main())
```

**重點說明：**
- `FoundryLocalClient(model_id=alias)` 一步完成服務啟動、下載與模型加載
- `client.as_agent()` 使用系統指令及名稱創建代理
- `agent.run()` 支援非串流與串流模式
- 透過 `pip install agent-framework-foundry-local --pre` 安裝

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**先決條件：**
```bash
cd javascript
npm install
```

**執行：**
```bash
node foundry-local-with-agent.mjs
```

<strong>程式碼解析</strong> (`javascript/foundry-local-with-agent.mjs`)：

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

class ChatAgent {
  constructor({ client, modelId, instructions, name }) {
    this.client = client;
    this.modelId = modelId;
    this.instructions = instructions;
    this.name = name;
    this.history = [];
  }

  async run(userMessage) {
    const messages = [
      { role: "system", content: this.instructions },
      ...this.history,
      { role: "user", content: userMessage },
    ];
    const response = await this.client.chat.completions.create({
      model: this.modelId,
      messages,
    });
    const assistantMessage = response.choices[0].message.content;

    // 保留對話歷史以便多回合互動
    this.history.push({ role: "user", content: userMessage });
    this.history.push({ role: "assistant", content: assistantMessage });
    return { text: assistantMessage };
  }
}

async function main() {
  FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
  const manager = FoundryLocalManager.instance;
  await manager.startWebService();

  const catalog = manager.catalog;
  const model = await catalog.getModel("phi-3.5-mini");
  if (!model.isCached) {
    console.log("Downloading model: phi-3.5-mini...");
    await model.download();
  }
  await model.load();

  const client = new OpenAI({
    baseURL: manager.urls[0] + "/v1",
    apiKey: "foundry-local",
  });

  const agent = new ChatAgent({
    client,
    modelId: model.id,
    instructions: "You are good at telling jokes.",
    name: "Joker",
  });

  const result = await agent.run("Tell me a joke about a pirate.");
  console.log(result.text);
}

main();
```

**重點說明：**
- JavaScript 自行構建 `ChatAgent` 類，模仿 Python AGF 模式
- `this.history` 存儲對話回合，支援多輪互動
- 明確的 `startWebService()` → 緩存檢查 → `model.download()` → `model.load()` 流程

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**先決條件：**
```bash
cd csharp
dotnet restore
```

**執行：**
```bash
dotnet run agent
```

<strong>程式碼解析</strong> (`csharp/SingleAgent.cs`)：

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Agents.AI;
using OpenAI;
using System.ClientModel;

// 1. Start Foundry Local and load a model
var alias = "phi-3.5-mini";
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);

var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine($"Downloading model: {alias}...");
    await model.DownloadAsync(null, default);
}
await model.LoadAsync(default);

var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls[0] + "/v1")
});

// 2. Create an AIAgent using the Agent Framework extension method
AIAgent joker = client
    .GetChatClient(model.Id)
    .AsAIAgent(
        instructions: "You are good at telling jokes. Keep your jokes short and family-friendly.",
        name: "Joker"
    );

// 3. Run the agent (non-streaming)
var response = await joker.RunAsync("Tell me a joke about a pirate.");
Console.WriteLine($"Joker: {response}");

// 4. Run with streaming
await foreach (var update in joker.RunStreamingAsync("Tell me another joke."))
{
    Console.Write(update);
}
```

**重點說明：**
- `AsAIAgent()` 是來自 `Microsoft.Agents.AI.OpenAI` 的擴充方法，無需自定義 `ChatAgent` 類
- `RunAsync()` 返回完整回應；`RunStreamingAsync()` 逐字串流播送
- 透過 `dotnet add package Microsoft.Agents.AI.OpenAI --version 1.0.0-rc3` 安裝

</details>

---

### 練習 3 - 更改角色

修改代理的 `instructions` 以創建不同角色。嘗試每種角色並觀察輸出如何變化：

| 角色 | 指令 |
|---------|-------------|
| 代碼審查員 | `"你是一位資深代碼審查員。提供針對可讀性、效能和正確性的建設性反饋。"` |
| 旅遊導遊 | `"你是一位友善的旅遊導遊。給出個人化的目的地、活動和當地美食推薦。"` |
| 蘇格拉底式導師 | `"你是一位蘇格拉底式導師。絕不直接回答，而是用深思熟慮的問題引導學生。"` |
| 技術作家 | `"你是一位技術作家。清晰且簡潔地解釋概念。使用範例。避免術語。"` |

**試試看：**
1. 從表中選擇一個角色
2. 替換程式碼中的 `instructions` 字串
3. 調整用戶提示以符合角色（例如要求代碼審查員審查一個函式）
4. 再次執行範例並比較輸出

> **提示：** 代理品質高度依賴指令。具體且結構化良好的指令比模糊指令產生更佳結果。

---

### 練習 4 - 加入多輪對話

擴充範例以支援多輪聊天迴圈，讓你可以與代理進行來回對話。

<details>
<summary><strong>🐍 Python - 多輪迴圈</strong></summary>

```python
import asyncio
from agent_framework_foundry_local import FoundryLocalClient

async def main():
    client = FoundryLocalClient(model_id="phi-4-mini")

    agent = client.as_agent(
        name="Assistant",
        instructions="You are a helpful assistant.",
    )

    print("Chat with the agent (type 'quit' to exit):\n")
    while True:
        user_input = input("You: ")
        if user_input.strip().lower() in ("quit", "exit"):
            break
        result = await agent.run(user_input)
        print(f"Agent: {result}\n")

asyncio.run(main())
```

</details>

<details>
<summary><strong>📦 JavaScript - 多輪迴圈</strong></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";
import * as readline from "node:readline/promises";

// （重用練習2中的ChatAgent類別）

async function main() {
  FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
  const manager = FoundryLocalManager.instance;
  await manager.startWebService();

  const catalog = manager.catalog;
  const model = await catalog.getModel("phi-3.5-mini");
  if (!model.isCached) {
    console.log("Downloading model: phi-3.5-mini...");
    await model.download();
  }
  await model.load();

  const client = new OpenAI({
    baseURL: manager.urls[0] + "/v1",
    apiKey: "foundry-local",
  });

  const agent = new ChatAgent({
    client,
    modelId: model.id,
    instructions: "You are a helpful assistant.",
    name: "Assistant",
  });

  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
  });

  console.log("Chat with the agent (type 'quit' to exit):\n");
  while (true) {
    const userInput = await rl.question("You: ");
    if (["quit", "exit"].includes(userInput.trim().toLowerCase())) break;
    const result = await agent.run(userInput);
    console.log(`Agent: ${result.text}\n`);
  }
  rl.close();
}

main();
```

</details>

<details>
<summary><strong>💜 C# - 多輪迴圈</strong></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Agents.AI;
using OpenAI;
using System.ClientModel;

var alias = "phi-3.5-mini";
var config = new Configuration
{
    AppName = "FoundryLocalSamples",
    Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
};
await FoundryLocalManager.CreateAsync(config, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);

var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine($"Downloading model: {alias}...");
    await model.DownloadAsync(null, default);
}
await model.LoadAsync(default);

var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls[0] + "/v1")
});

AIAgent agent = client
    .GetChatClient(model.Id)
    .AsAIAgent(
        instructions: "You are a helpful assistant.",
        name: "Assistant"
    );

Console.WriteLine("Chat with the agent (type 'quit' to exit):\n");
while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput) ||
        userInput.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
        userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var result = await agent.RunAsync(userInput);
    Console.WriteLine($"Agent: {result}\n");
}
```

</details>

注意代理如何記住之前的回合—提出後續問題即可看到上下文連續。

---

### 練習 5 - 結構化輸出

指示代理總是以特定格式（例如 JSON）回應，並解析結果：

<details>
<summary><strong>🐍 Python - JSON 輸出</strong></summary>

```python
import asyncio
import json
from agent_framework_foundry_local import FoundryLocalClient

async def main():
    client = FoundryLocalClient(model_id="phi-4-mini")

    agent = client.as_agent(
        name="SentimentAnalyzer",
        instructions=(
            "You are a sentiment analysis agent. "
            "For every user message, respond ONLY with valid JSON in this format: "
            '{"sentiment": "positive|negative|neutral", "confidence": 0.0-1.0, "summary": "brief reason"}'
        ),
    )

    result = await agent.run("I absolutely loved the new restaurant downtown!")
    print("Raw:", result)

    try:
        parsed = json.loads(str(result))
        print(f"Sentiment: {parsed['sentiment']} (confidence: {parsed['confidence']})")
    except json.JSONDecodeError:
        print("Agent did not return valid JSON - try refining the instructions.")

asyncio.run(main())
```

</details>

<details>
<summary><strong>💜 C# - JSON 輸出</strong></summary>

```csharp
using System.Text.Json;

AIAgent analyzer = chatClient.AsAIAgent(
    name: "SentimentAnalyzer",
    instructions:
        "You are a sentiment analysis agent. " +
        "For every user message, respond ONLY with valid JSON in this format: " +
        "{\"sentiment\": \"positive|negative|neutral\", \"confidence\": 0.0-1.0, \"summary\": \"brief reason\"}"
);

var response = await analyzer.RunAsync("I absolutely loved the new restaurant downtown!");
Console.WriteLine($"Raw: {response}");

try
{
    var parsed = JsonSerializer.Deserialize<JsonElement>(response.ToString());
    Console.WriteLine($"Sentiment: {parsed.GetProperty("sentiment")} " +
                      $"(confidence: {parsed.GetProperty("confidence")})");
}
catch (JsonException)
{
    Console.WriteLine("Agent did not return valid JSON - try refining the instructions.");
}
```

</details>

> **注意：** 小型本地模型不一定總能產生完美有效的 JSON。可通過指令示例和明確要求格式來提高穩定性。

---

## 主要收穫

| 概念 | 你學到了什麼 |
|---------|-----------------|
| 代理 vs. 原始 LLM 調用 | 代理包裝模型並帶有指令和記憶 |
| 系統指令 | 控制代理行為的最重要槓桿 |
| 多輪對話 | 代理能跨多輪用戶交互攜帶上下文 |
| 結構化輸出 | 指令能強制輸出格式（JSON、markdown 等） |
| 本地執行 | 全部通過 Foundry Local 在本機執行，無需雲端 |

---

## 下一步

在 **[第六部分：多代理工作流程](part6-multi-agent-workflows.md)**，你將把多個代理結合成協調管道，每個代理擁有專門角色。