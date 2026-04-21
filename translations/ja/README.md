<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local ワークショップ - デバイス上でAIアプリを構築

言語モデルを自身のマシンで実行し、[Foundry Local](https://foundrylocal.ai) と [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) を使ってインテリジェントなアプリケーションを構築する実践型ワークショップです。

> **Foundry Localとは？** Foundry Localは、言語モデルを完全にあなたのハードウェア内でダウンロード、管理、提供できる軽量ランタイムです。<strong>OpenAI互換API</strong>を公開し、OpenAIと通信できる任意のツールやSDKから接続可能であり、クラウドアカウントは不要です。

---

## 学習目標

このワークショップ終了時には以下ができるようになります：

| # | 目標 |
|---|-----------|
| 1 | Foundry Localをインストールし、CLIでモデルを管理する |
| 2 | プログラム的にモデル管理を行うFoundry Local SDK APIを習得する |
| 3 | Python、JavaScript、およびC#のSDKを使ってローカル推論サーバーに接続する |
| 4 | 自分のデータに基づいた回答を行うリトリーバル強化生成（RAG）パイプラインを構築する |
| 5 | 永続的な指示とペルソナを持つAIエージェントを作成する |
| 6 | フィードバックループを含むマルチエージェントワークフローをオーケストレーションする |
| 7 | 本番レベルのキャップストーンアプリ - Zava Creative Writer を探求する |
| 8 | ゴールデンデータセットとLLM-ジャッジスコアリングを用いた評価フレームワークを構築する |
| 9 | Whisperを使った音声の文字起こし - Foundry Local SDKでデバイス上の音声認識を実行する |
| 10 | ONNX Runtime GenAIとFoundry LocalでカスタムまたはHugging Faceモデルをコンパイル・実行する |
| 11 | ツール呼び出しパターンを用いてローカルモデルから外部関数を呼び出せるようにする |
| 12 | リアルタイムストリーミング対応のZava Creative Writer用ブラウザベースUIを構築する |

---

## 必要条件

| 必要条件 | 詳細 |
|-------------|---------|
| <strong>ハードウェア</strong> | 最低8GB RAM（推奨16GB）；AVX2対応CPUまたはサポートGPU |
| **OS** | Windows 10/11 (x64/ARM)、Windows Server 2025、またはmacOS 13以降 |
| **Foundry Local CLI** | Windowsは `winget install Microsoft.FoundryLocal`、macOSは `brew tap microsoft/foundrylocal && brew install foundrylocal` でインストール。詳細は[入門ガイド](https://learn.microsoft.com/en-us/azure/foundry-local/get-started)参照。 |
| <strong>言語ランタイム</strong> | <strong>Python 3.9以降</strong>および/または<strong>.NET 9.0以降</strong>および/または<strong>Node.js 18以降</strong> |
| **Git** | このリポジトリのクローン用 |

---

## はじめに

```bash
# 1. リポジトリをクローンする
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local がインストールされていることを確認する
foundry model list              # 利用可能なモデルを一覧表示する
foundry model run phi-3.5-mini  # インタラクティブなチャットを開始する

# 3. 言語トラックを選択する（完全設定についてはパート2のラボを参照）
```

| 言語 | クイックスタート |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ワークショップ構成

### パート1：Foundry Localのはじめかた

**ラボガイド：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Localとは何か、その仕組み
- WindowsとmacOSへのCLIのインストール
- モデルの探索 - リスト表示、ダウンロード、実行
- モデルエイリアスと動的ポートの理解

---

### パート2：Foundry Local SDKの詳細解説

**ラボガイド：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- アプリケーション開発にCLIではなくSDKを使う理由
- Python、JavaScript、C#向けの完全なSDK APIリファレンス
- サービス管理、カタログブラウズ、モデルライフサイクル（ダウンロード、ロード、アンロード）
- クイックスタートパターン：Pythonのコンストラクタブートストラップ、JavaScriptの `init()`、C#の `CreateAsync()`
- `FoundryModelInfo`メタデータ、エイリアス、ハードウェア最適モデルの選択

---

### パート3：SDKとAPI

**ラボガイド：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python、JavaScript、C#からFoundry Localに接続
- Foundry Local SDKを使ったプログラム的なサービス管理
- OpenAI互換API経由のストリーミングチャット補完
- 各言語のSDKメソッドリファレンス

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local.py` | 基本的なストリーミングチャット |
| C# | `csharp/BasicChat.cs` | .NETでのストリーミングチャット |
| JavaScript | `javascript/foundry-local.mjs` | Node.jsでのストリーミングチャット |

---

### パート4：リトリーバル強化生成（RAG）

**ラボガイド：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAGとは何か、なぜ重要か
- インメモリ知識ベースの構築
- スコアリングによるキーワード重複検索
- 根拠を持つシステムプロンプトの作成
- デバイス上での完全なRAGパイプラインの実行

**コードサンプル：**

| 言語 | ファイル |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### パート5：AIエージェントの構築

**ラボガイド：** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AIエージェントとは何か（生のLLMコールと比較して）
- `ChatAgent`パターンとMicrosoft Agent Framework
- システム指示、ペルソナ、マルチターン会話
- エージェントからの構造化された出力（JSON）

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Frameworkを使った単一エージェント |
| C# | `csharp/SingleAgent.cs` | 単一エージェント（ChatAgentパターン） |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 単一エージェント（ChatAgentパターン） |

---

### パート6：マルチエージェントワークフロー

**ラボガイド：** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- マルチエージェントパイプライン：リサーチャー → ライター → エディター
- 順次オーケストレーションとフィードバックループ
- 共通設定と構造化された引き継ぎ
- オリジナルのマルチエージェントワークフロー設計

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | 3エージェントパイプライン |
| C# | `csharp/MultiAgent.cs` | 3エージェントパイプライン |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 3エージェントパイプライン |

---

### パート7：Zava Creative Writer - キャップストーンアプリケーション

**ラボガイド：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4つの専門エージェントを持つ本格的なマルチエージェントアプリ
- 評価者駆動のフィードバックループによる順次パイプライン
- ストリーミング出力、製品カタログ検索、構造化JSONハンドオフ
- Python (FastAPI)、JavaScript (Node.js CLI)、C# (.NETコンソール)での完全実装

**コードサンプル：**

| 言語 | ディレクトリ | 説明 |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | オーケストレーター付きFastAPIウェブサービス |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLIアプリケーション |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9コンソールアプリ |

---

### パート8：評価主導開発

**ラボガイド：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- ゴールデンデータセットを使ったAIエージェントの体系的評価フレームワークの構築
- ルールベースチェック（長さ、キーワードカバレッジ、禁止用語）＋LLMでのジャッジスコアリング
- プロンプトバリアントの並列比較と総合スコアカード
- パート7のZava Editorエージェントパターンを拡張したオフラインテストスイート
- Python、JavaScript、C#対応

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | 評価フレームワーク |
| C# | `csharp/AgentEvaluation.cs` | 評価フレームワーク |
| JavaScript | `javascript/foundry-local-eval.mjs` | 評価フレームワーク |

---

### パート9：Whisperによる音声文字起こし

**ラボガイド：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- ローカルで動作するOpenAI Whisperを使った音声認識（Speech-to-Text）
- プライバシー重視の音声処理 - 音声は絶対にデバイス外に送信されません
- `client.audio.transcriptions.create()` (Python/JS) および `AudioClient.TranscribeAudioAsync()` (C#) を使ったPython、JavaScript、C#のトラック
- ハンズオン練習用のZavaテーマ音声サンプルファイル付き

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper音声文字起こし |
| C# | `csharp/WhisperTranscription.cs` | Whisper音声文字起こし |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper音声文字起こし |

> **注意：** このラボは **Foundry Local SDK** を使ってプログラム的にWhisperモデルをダウンロード・ロードし、その後ローカルのOpenAI互換エンドポイントに音声を送って文字起こしします。Whisperモデル（`whisper`）はFoundry Localカタログに掲載されており、完全にデバイス上で動作し、クラウドAPIキーやネットワーク接続は不要です。

---

### パート10：カスタムまたはHugging Faceモデルの使用

**ラボガイド：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAIモデルビルダーを使ったHugging Faceモデルの最適化ONNX形式へのコンパイル
- ハードウェア別コンパイル（CPU、NVIDIA GPU、DirectML、WebGPU）と量子化（int4、fp16、bf16）
- Foundry Local用のチャットテンプレート構成ファイルの作成
- コンパイル済みモデルのFoundry Localキャッシュへの追加
- CLI、REST API、OpenAI SDKを使ったカスタムモデルの実行
- 参考例：Qwen/Qwen3-0.6Bのエンドツーエンドコンパイル

---

### パート11：ローカルモデルでのツール呼び出し

**ラボガイド：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- ローカルモデルが外部関数（ツール）を呼び出せるようにする
- OpenAIの関数呼び出し形式を使ったツールスキーマの定義
- マルチターンのツール呼び出し会話フローの取り扱い
- ローカルでツールを実行して結果をモデルに返す
- ツール呼び出しシナリオに適したモデルの選択（Qwen 2.5、Phi-4-mini）
- SDKのネイティブ `ChatClient` を使ったツール呼び出し（JavaScript）

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 天気／人口ツールでのツール呼び出し |
| C# | `csharp/ToolCalling.cs` | .NETでのツール呼び出し |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClientによるツール呼び出し |

---

### パート12：Zava Creative WriterのWeb UI構築

**ラボガイド：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writerにブラウザベースのフロントエンドを追加
- Python (FastAPI)、JavaScript (Node.js HTTP)、C# (ASP.NET Core) からUIを提供
- Fetch APIとReadableStreamでブラウザでのストリーミングNDJSONを消費
- ライブエージェントステータスバッジとリアルタイム記事テキストストリーミング

**コード（共通UI）：**

| ファイル | 説明 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | ページレイアウト |
| `zava-creative-writer-local/ui/style.css` | スタイリング |
| `zava-creative-writer-local/ui/app.js` | ストリームリーダーとDOM更新ロジック |

**バックエンド追加：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 静的UI提供対応に更新 |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | オーケストレーターをラップする新HTTPサーバー |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新規ASP.NET CoreミニマルAPIプロジェクト |

---

### パート13：ワークショップ完了


**ラボガイド:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- これまでの12パートで構築した内容のまとめ
- アプリケーションを拡張するためのさらなるアイデア
- リソースやドキュメントへのリンク

---

## プロジェクト構造

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## リソース

| リソース | リンク |
|----------|------|
| Foundry Local ウェブサイト | [foundrylocal.ai](https://foundrylocal.ai) |
| モデルカタログ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| 入門ガイド | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK リファレンス | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft エージェントフレームワーク | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## ライセンス

このワークショップ資料は教育目的で提供されています。

---

**楽しい開発を！🚀**