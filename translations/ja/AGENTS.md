# コーディングエージェントの指示

このファイルは、このリポジトリで作業するAIコーディングエージェント（GitHub Copilot、Copilot Workspace、Codexなど）向けのコンテキストを提供します。

## プロジェクト概要

これは、[Foundry Local](https://foundrylocal.ai) を使ってAIアプリケーションを構築するための <strong>ハンズオンワークショップ</strong> です。Foundry Localは、完全にデバイス上で言語モデルをダウンロード、管理、提供する軽量ランタイムであり、OpenAI互換のAPIを提供します。ワークショップには、段階的なラボガイドとPython、JavaScript、C#での実行可能なコードサンプルが含まれています。

## リポジトリ構成

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## 言語 & フレームワークの詳細

### Python
- **場所:** `python/`, `zava-creative-writer-local/src/api/`
- **依存関係:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **主なパッケージ:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **最小バージョン:** Python 3.9+
- **実行:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **場所:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **依存関係:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **主なパッケージ:** `foundry-local-sdk`, `openai`
- **モジュールシステム:** ESモジュール（`.mjs`ファイル、`"type": "module"`）
- **最小バージョン:** Node.js 18+
- **実行:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **場所:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **プロジェクトファイル:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **主なパッケージ:** `Microsoft.AI.Foundry.Local`（非Windows）、`Microsoft.AI.Foundry.Local.WinML`（Windows — QNN EPを含むスーパーセット）、`OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **ターゲット:** .NET 9.0（条件付きTFM: Windows上は `net9.0-windows10.0.26100`、その他は `net9.0`）
- **実行:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## コーディング規約

### 全般
- すべてのコードサンプルは <strong>単一ファイルの自己完結型例</strong> であり、共有のユーティリティライブラリや抽象化はありません。
- 各サンプルは依存関係をインストールした後に独立して実行可能です。
- APIキーは常に `"foundry-local"` に設定されます — Foundry Localはこれをプレースホルダーとして使います。
- ベースURLは `http://localhost:<port>/v1` を使い、ポートはSDK経由でランタイムに動的に検出されます（JSでは `manager.urls[0]`、Pythonでは `manager.endpoint`）。
- Foundry Local SDKがサービスの起動とエンドポイントの検出を管理するため、ポートのハードコーディングよりSDKパターンを優先してください。

### Python
- `openai` SDKを `OpenAI(base_url=..., api_key="not-required")` で使用します。
- サービスライフサイクルは `foundry_local` の `FoundryLocalManager()` を使って管理します。
- ストリーミングは `for chunk in stream:` で `stream` オブジェクトをイテレートします。
- サンプルファイルには型注釈はありません（ワークショップ学習者向けに簡潔にするため）。

### JavaScript
- ESモジュール構文：`import ... from "..."`。
- `"openai"` から `OpenAI`、`"foundry-local-sdk"` から `FoundryLocalManager` を使用します。
- SDK初期化パターン：`FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`。
- ストリーミングは `for await (const chunk of stream)`。
- トップレベルの `await` を全体で使用しています。

### C#
- Nullable有効化、暗黙のusing、.NET 9を使用。
- SDK管理のライフサイクルは `FoundryLocalManager.StartServiceAsync()` を使います。
- ストリーミングは `CompleteChatStreaming()` を `foreach (var update in completionUpdates)` で処理します。
- メインの `csharp/Program.cs` はCLIルーターであり、スタティックな `RunAsync()` メソッドに振り分けます。

### ツール呼び出し
- ツール呼び出しに対応するモデルは限定的です：**Qwen 2.5** ファミリー（`qwen2.5-*`）と<strong>Phi-4-mini</strong>（`phi-4-mini`）のみ。
- ツールのスキーマはOpenAIの関数呼び出しJSONフォーマットを踏襲しています（`type: "function"`、`function.name`、`function.description`、`function.parameters`）。
- 会話はマルチターンパターンを使います：ユーザー → アシスタント（tool_calls）→ ツール（結果）→ アシスタント（最終回答）。
- ツール結果メッセージの `tool_call_id` はモデルのツール呼び出しの `id` と一致しなければなりません。
- PythonはOpenAI SDKを直接使用、JavaScriptはSDKネイティブの `ChatClient`（`model.createChatClient()`）を使用、C#はOpenAI SDKの `ChatTool.CreateFunctionTool()` を使います。

### ChatClient（ネイティブSDKクライアント）
- JavaScript：`model.createChatClient()` は `completeChat(messages, tools?)` と `completeStreamingChat(messages, callback)` を持つ `ChatClient` を返します。
- C#：`model.GetChatClientAsync()` はOpenAI NuGetパッケージをインポート不要で使える標準的な `ChatClient` を返します。
- PythonにはネイティブChatClientはなく、`manager.endpoint` と `manager.api_key` を用いてOpenAI SDKを使用します。
- **重要：** JavaScriptの `completeStreamingChat` は非同期イテレーションではなく、<strong>コールバックパターン</strong>を使用します。

### 推論モデル
- `phi-4-mini-reasoning` は最終回答の前に `<think>...</think>` タグで思考をラップします。
- 必要に応じてタグを解析して推論と回答を分離してください。

## ラボガイド

ラボファイルは `labs/` にMarkdownで格納され、一貫した構造に従っています：
- ロゴヘッダー画像
- タイトルとゴールの呼び出し
- 概要、学習目標、前提条件
- 図解を含む概念説明セクション
- コードブロックと期待される出力を伴う番号付き演習
- まとめ表、重要ポイント、さらなる読書案内
- 次のパートへのナビゲーションリンク

ラボ内容を編集する際は：
- 既存のMarkdown書式スタイルとセクション階層を維持してください。
- コードブロックは言語指定（`python`、`javascript`、`csharp`、`bash`、`powershell`）を明示してください。
- OSによる違いがある場合はbashとPowerShellの両方の例を提供してください。
- `> **Note:**`、`> **Tip:**`、`> **Troubleshooting:**` の呼び出しスタイルを使用してください。
- 表は `| ヘッダー | ヘッダー |` のパイプ形式を使用します。

## ビルドとテストコマンド

| アクション | コマンド |
|------------|----------|
| **Pythonサンプル** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JSサンプル** | `cd javascript && npm install && node <script>.mjs` |
| **C#サンプル** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS（web）** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C#（web）** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| <strong>図の生成</strong> | `npx mmdc -i <input>.mmd -o <output>.svg` （rootでの `npm install` が必要） |

## 外部依存関係

- **Foundry Local CLI** は開発者のマシンにインストールされている必要があります（`winget install Microsoft.FoundryLocal` または `brew install foundrylocal`）。
- **Foundry Localサービス** はローカルで実行され、OpenAI互換のREST APIを動的なポートで公開します。
- サンプルを実行するのにクラウドサービス、APIキー、Azureサブスクリプションは不要です。
- パート10（カスタムモデル）だけは追加で `onnxruntime-genai` が必要で、Hugging Faceからモデル重みをダウンロードします。

## コミットすべきでないファイル

`.gitignore` は以下を除外しています（ほとんどの場合すでに設定済み）：
- `.venv/` — Python仮想環境
- `node_modules/` — npm依存関係
- `models/` — コンパイル済みONNXモデル出力（大容量バイナリファイル、パート10で生成）
- `cache_dir/` — Hugging Faceモデルダウンロードキャッシュ
- `.olive-cache/` — Microsoft Olive作業ディレクトリ
- `samples/audio/*.wav` — 生成されたオーディオサンプル（`python samples/audio/generate_samples.py` で再生成可能）
- 標準的なPythonビルド成果物（`__pycache__/`, `*.egg-info/`, `dist/` など）

## ライセンス

MIT — `LICENSE` を参照してください。