![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# パート11：ローカルモデルでのツール呼び出し

> **目標:** ローカルモデルが外部関数（ツール）を呼び出し、リアルタイムデータの取得、計算の実行、APIとの連携をデバイス上でプライベートに行えるようにする。

## ツール呼び出しとは？

ツール呼び出し（別名 <strong>関数呼び出し</strong>）は、言語モデルがあなたの定義した関数の実行を要求できる機能です。モデルは推測で答えを出すのではなく、ツールが役立つと認識した場合に、あなたのコードが実行するよう構造化されたリクエストを返します。アプリケーションが関数を実行し、結果を返し、モデルはその情報を最終応答に取り込みます。

![Tool-calling flow](../../../images/tool-calling-flow.svg)

このパターンは以下のようなエージェント構築に不可欠です：

- <strong>ライブデータの検索</strong>（天気、株価、データベースクエリ）
- <strong>正確な計算の実行</strong>（数学、単位変換）
- <strong>アクションの実行</strong>（メールの送信、チケット作成、レコード更新）
- <strong>プライベートシステムへのアクセス</strong>（内部API、ファイルシステム）

---

## ツール呼び出しの仕組み

ツール呼び出しの流れは4段階あります：

| ステージ | 内容 |
|-------|-------------|
| **1. ツール定義** | 利用可能な関数をJSON Schemaで記述：名前、説明、パラメーター |
| **2. モデルが判断** | メッセージとツール定義を受け取り、ツールが有効ならテキストではなく `tool_calls` を返す |
| **3. ローカル実行** | コードがツール呼び出しを解析し、関数を実行し結果を取得 |
| **4. 最終回答** | ツールの結果をモデルに返し、最終的な応答を生成 |

> **重要ポイント:** モデル自身はコードを実行しません。あくまでツールの呼び出しを<em>要求</em>します。実行するかどうかはあなたのアプリケーションが決めるため、完全に制御可能です。

---

## ツール呼び出しをサポートするモデル

すべてのモデルがツール呼び出しに対応しているわけではありません。現状のFoundry Localカタログでツール呼び出し対応モデルは以下の通りです：

| モデル | サイズ | ツール呼び出し |
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

> **ヒント:** このラボでは **qwen2.5-0.5b** を使用します — サイズが小さく（822MBダウンロード）、高速で、信頼性の高いツール呼び出しをサポートしています。

---

## 学習目標

このラボの終了時には以下が可能になります：

- ツール呼び出しパターンとは何か、そのAIエージェントにおける重要性を説明する
- OpenAIの関数呼び出しフォーマットを使いツールスキーマを定義する
- マルチターンのツール呼び出し会話フローを処理する
- ツール呼び出しをローカルで実行し、結果をモデルに返す
- ツール呼び出しシナリオに最適なモデルを選ぶ

---

## 前提条件

| 要件 | 詳細 |
|-------------|---------|
| **Foundry Local CLI** | インストール済み、`PATH`に設定されている ([Part 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python、JavaScript、または C# SDK がインストールされている ([Part 2](part2-foundry-local-sdk.md)) |
| <strong>ツール呼び出し対応モデル</strong> | qwen2.5-0.5b（自動的にダウンロードされます） |

---

## 演習

### 演習1 — ツール呼び出しフローの理解

コードを書く前に、このシーケンス図をよく見てください：

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**重要ポイント：**

1. ツールは事前にJSON Schemaオブジェクトとして定義する
2. モデルの応答は通常のコンテンツではなく `tool_calls` を含む
3. 各ツール呼び出しには固有の `id` があり、結果返却時に参照する必要がある
4. モデルは最終回答生成時にすべての過去メッセージとツール結果を見る
5. 1回の応答で複数のツール呼び出しが起こることがある

> **議論:** なぜモデルは関数を直接実行せずツール呼び出しを返すのか？ どんなセキュリティ利点があるか？

---

### 演習2 — ツールスキーマの定義

ツールは標準のOpenAI関数呼び出しフォーマットで定義します。各ツールには：

- **`type`**：常に `"function"`
- **`function.name`**：説明的な関数名（例：`get_weather`）
- **`function.description`**：明確な説明、モデルがいつ呼び出すか判断に使う
- **`function.parameters`**：期待される引数を記述するJSON Schemaオブジェクト

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

> **ツール説明のベストプラクティス：**
> - 具体的に：例えば「特定都市の現在の天気を取得」が「天気を取得」より良い
> - パラメーターを明確に説明：モデルはこれを読み適切な値を埋める
> - 必須・任意パラメーターを区別し、モデルが何を聞くべきか判断しやすくする

---

### 演習3 — ツール呼び出しサンプルの実行

各言語のサンプルは2つのツール（`get_weather` と `get_population`）を定義し、ツール利用を促す質問を送り、ローカルでツールを実行し結果を返して最終回答を得ます。

<details>
<summary><strong>🐍 Python</strong></summary>

**前提条件:**
```bash
cd python
python -m venv venv

# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS / Linux：
source venv/bin/activate

pip install -r requirements.txt
```

**実行:**
```bash
python foundry-local-tool-calling.py
```

**期待出力:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>コード説明</strong> (`python/foundry-local-tool-calling.py`):

```python
# ツールを関数スキーマのリストとして定義する
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

# ツールと共に送信 — モデルはコンテンツの代わりにtool_callsを返す場合がある
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# モデルがツールを呼び出したいかどうかを確認する
if response.choices[0].message.tool_calls:
    # ツールを実行して結果を返す
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**前提条件:**
```bash
cd javascript
npm install
```

**実行:**
```bash
node foundry-local-tool-calling.mjs
```

**期待出力:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>コード説明</strong> (`javascript/foundry-local-tool-calling.mjs`):

この例ではOpenAI SDKではなくネイティブFoundry Local SDKの`ChatClient`を使い、`createChatClient()`の便利さを示しています：

```javascript
// モデルオブジェクトから直接ChatClientを取得する
const chatClient = model.createChatClient();

// ツールを使って送信 — ChatClientがOpenAI互換フォーマットを処理する
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// ツール呼び出しを確認する
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // ツールを実行し、結果を返送する
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**前提条件:**
```bash
cd csharp
dotnet restore
```

**実行:**
```bash
dotnet run toolcall
```

**期待出力:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

<strong>コード説明</strong> (`csharp/ToolCalling.cs`):

C#では`ChatTool.CreateFunctionTool`ヘルパーでツールを定義します：

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

### 演習4 — ツール呼び出し会話の流れ

メッセージ構造を理解することが重要です。各ステージでの `messages` 配列の内容を示します：

**ステージ1 — 初期リクエスト:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**ステージ2 — モデルはtool_callsで応答（通常コンテンツではない）:**
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

**ステージ3 — 助手メッセージとツール結果を追加:**
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

**ステージ4 — モデルがツール結果を使い最終回答を生成。**

> **重要:** ツールメッセージの `tool_call_id` はツール呼び出しの `id` と一致させる必要があります。これによりモデルは結果とリクエストを紐づけます。

---

### 演習5 — 複数ツール呼び出し

モデルは1つの応答内で複数のツール呼び出しを要求する場合があります。ユーザーメッセージを変更して複数の呼び出しを誘発してみてください：

```python
# Pythonでは — ユーザーメッセージを変更してください:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScriptで — ユーザーメッセージを変更する:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

モデルは `get_weather` と `get_population` の2つの `tool_calls` を返すはずです。コードはすでにすべてのツール呼び出しをループ処理します。

> **試してみよう:** ユーザーメッセージを変えて再度実行。モデルは両方のツールを呼び出しますか？

---

### 演習6 — 独自ツールの追加

一つのサンプルに新しいツールを追加してください。例として `get_time` ツール：

1. ツールスキーマを定義：
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

2. 実行ロジックを追加：
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # 実際のアプリでは、タイムゾーンライブラリを使用してください
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... 既存のツール ...
```

3. `tools` 配列に加え、「東京の現在時刻は？」でテスト

> **挑戦:** 温度変換を行う `convert_temperature` ツールの追加に挑戦。例：「100°Fを摂氏に換算して。」

---

### 演習7 — SDKのChatClientによるツール呼び出し（JavaScript）

JavaScriptサンプルはOpenAI SDKではなくFoundry Local SDKの`ChatClient`を使っています。これはOpenAIクライアントを自分で構成しなくて良い便利な機能です：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClientはモデルオブジェクトから直接作成されます
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChatはツールを第二引数として受け取ります
const response = await chatClient.completeChat(messages, tools);
```

PythonでOpenAI SDKを使う例と比較してみてください：

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

どちらの方法も有効です。`ChatClient` は便利で、OpenAI SDKはより幅広いパラメーターを扱えます。

> **試してみよう:** JavaScriptサンプルを`ChatClient`ではなくOpenAI SDK使用に変更。`import OpenAI from "openai"`し、`manager.urls[0]`からエンドポイントを取ってクライアントを構築します。

---

### 演習8 — tool_choice の理解

`tool_choice` パラメーターはモデルがツールを「必須で使うか」または「自由に選ぶか」を制御します：

| 値 | 挙動 |
|-------|-----------|
| `"auto"` | モデルがツール呼び出しを決定（デフォルト） |
| `"none"` | モデルはツールを呼び出さない |
| `"required"` | 少なくとも1つのツールを必ず呼び出す |
| `{"type": "function", "function": {"name": "get_weather"}}` | 指定したツールのみ必ず呼び出す |

Pythonサンプルで各オプションを試してください：

```python
# モデルにget_weatherを呼び出させるよう強制する
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **補足:** すべての`tool_choice`がすべてのモデルでサポートされているわけではありません。`"required"`をサポートしないモデルは無視して`"auto"`と同様に動作する可能性があります。

---

## よくある落とし穴

| 問題 | 解決策 |
|---------|----------|
| モデルがツールを呼び出さない | ツール呼び出し対応モデルを使っているか確認（例：qwen2.5-0.5b）。上記表を参照。 |
| `tool_call_id` が不一致 | ツール呼び出しレスポンスの `id` を常に使い、ハードコードしない |
| `arguments` 内の不正なJSON返却 | 小型モデルは不正なJSONを出すことがある。`JSON.parse()`をtry/catchで包む |
| 存在しないツールを呼び出す | `execute_tool`関数内にデフォルトハンドラを追加 |
| 無限ツール呼び出しループ | 最大ラウンド数（例：5）を設定し無限ループ防止 |

---

## 重要なポイント

1. <strong>ツール呼び出しは</strong> モデルが答えを推測するのではなく、関数実行を要求できる仕組み
2. モデルは<strong>コードを実行しない</strong>。実行決定はアプリケーション側にある
3. ツールは<strong>OpenAI関数呼び出しフォーマットに準拠したJSON Schema</strong>オブジェクトで定義
4. 会話は<strong>マルチターンパターン</strong>を使う：ユーザー → アシスタント（tool_calls）→ ツール（結果）→ アシスタント（最終回答）
5. <strong>ツール呼び出し対応モデル</strong>（Qwen 2.5、Phi-4-miniなど）を使うことが必須
6. SDKの`createChatClient()`はOpenAIクライアント構築不要の便利な方法

---

続いて [パート12：ZavaクリエイティブライターのWeb UI構築](part12-zava-ui.md) でリアルタイムストリーミング対応のマルチエージェントパイプラインにブラウザフロントエンドを追加しましょう。

---

[← パート10：カスタムモデル](part10-custom-models.md) | [パート12：ZavaライターUI →](part12-zava-ui.md)