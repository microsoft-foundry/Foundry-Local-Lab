<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local ワークショップ - デバイス上でAIアプリを構築する

自身のマシンで言語モデルを実行し、[Foundry Local](https://foundrylocal.ai) と [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) を使ってインテリジェントなアプリケーションを構築するハンズオンワークショップです。

> **Foundry Localとは？** Foundry Localは、言語モデルを完全に自身のハードウェア上でダウンロード、管理、提供できる軽量ランタイムです。<strong>OpenAI互換API</strong>を公開しており、OpenAIに対応した任意のツールやSDKから接続可能で、クラウドアカウントは不要です。

### 🌐 多言語サポート

#### GitHub Actionsを使ったサポート（自動かつ常に最新）

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](./README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **ローカルでクローンしたいですか？**
>
> このリポジトリには50以上の言語翻訳が含まれており、ダウンロードサイズが大幅に増加します。翻訳なしでクローンするにはスパースチェックアウトを使用してください：
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> これにより、コースを完了するために必要なものを高速でダウンロードできます。
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## 学習目標

このワークショップを終了すると、以下のことができるようになります：

| # | 目標 |
|---|-----------|
| 1 | Foundry Localをインストールし、CLIでモデルを管理する |
| 2 | Foundry Local SDK APIを習得し、プログラマブルなモデル管理を行う |
| 3 | Python、JavaScript、C# SDKを使ってローカル推論サーバーに接続する |
| 4 | 自身のデータに基づく回答を行うリトリーバル拡張生成（RAG）パイプラインを構築する |
| 5 | 永続的な指示やペルソナを持つAIエージェントを作成する |
| 6 | フィードバックループを持つマルチエージェントワークフローをオーケストレーションする |
| 7 | 実践的なキャップストーンアプリ「Zava Creative Writer」を探求する |
| 8 | ゴールデンデータセットやLLMジャッジスコアによる評価フレームワークを構築する |
| 9 | Whisperで音声を文字起こし - Foundry Local SDKによるオンデバイス音声認識 |
| 10 | ONNX Runtime GenAIとFoundry Localを使ってカスタムまたはHugging Faceモデルをコンパイル・実行する |
| 11 | ローカルモデルからツール呼び出しパターンによる外部関数呼び出しを可能にする |
| 12 | Zava Creative WriterのブラウザベースUIをリアルタイムストリーミングで構築する |

---

## 前提条件

| 要件 | 詳細 |
|-------------|---------|
| <strong>ハードウェア</strong> | 8GB RAM以上（16GB推奨）、AVX2対応CPUまたは対応GPU |
| **OS** | Windows 10/11 (x64/ARM)、Windows Server 2025、または macOS 13以上 |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) または `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) でインストール。詳細は [はじめにガイド](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) を参照。 |
| <strong>言語ランタイム</strong> | **Python 3.9+** および/または **.NET 9.0+** および/または **Node.js 18+** |
| **Git** | 本リポジトリのクローン用 |

---

## はじめに

```bash
# 1. リポジトリをクローンする
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local がインストールされていることを確認する
foundry model list              # 利用可能なモデルを一覧表示する
foundry model run phi-3.5-mini  # インタラクティブチャットを開始する

# 3. 使用する言語トラックを選択する（完全なセットアップについてはパート2のラボを参照）
```

| 言語 | クイックスタート |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ワークショップのパート構成

### パート1：Foundry Localのはじめかた

**ラボガイド：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Localとは何か、その仕組み
- WindowsとmacOSでCLIをインストールする方法
- モデル探索 - リスト表示、ダウンロード、実行
- モデルのエイリアスと動的ポートの理解

---

### パート2：Foundry Local SDKを深掘り

**ラボガイド：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- アプリケーション開発にCLIではなくSDKを使う理由
- Python、JavaScript、C#の完全なSDK APIリファレンス
- サービス管理、カタログ閲覧、モデルライフサイクル（ダウンロード、ロード、アンロード）
- クイックスタートパターン：Pythonコンストラクタブートストラップ、JavaScriptの `init()`, C#の `CreateAsync()`
- `FoundryModelInfo` メタデータ、エイリアス、ハードウェアに最適なモデル選択

---

### パート3：SDKとAPI

**ラボガイド：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python、JavaScript、C#からFoundry Localに接続する方法
- Foundry Local SDKを使用してプログラムからサービスを管理する
- OpenAI互換APIを使用したストリーミングチャット完了
- 各言語のSDKメソッドリファレンス

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local.py` | 基本的なストリーミングチャット |
| C# | `csharp/BasicChat.cs` | .NETでのストリーミングチャット |
| JavaScript | `javascript/foundry-local.mjs` | Node.jsでのストリーミングチャット |

---

### パート4：リトリーバル拡張生成（RAG）

**ラボガイド：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAGとは何か、そしてその重要性
- インメモリの知識ベースを構築する
- キーワード重なりによる検索とスコアリング
- 根拠のあるシステムプロンプトの作成
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

- AIエージェントとは（生のLLM呼び出しと比較して）
- `ChatAgent`パターンとMicrosoft Agent Frameworkの活用
- システム指示、ペルソナ、マルチターン会話
- エージェントから構造化出力(JSON)を得る

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
- 逐次オーケストレーションとフィードバックループ
- 共有設定と構造化された受け渡し
- 独自のマルチエージェントワークフロー設計

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | 三者エージェントパイプライン |
| C# | `csharp/MultiAgent.cs` | 三者エージェントパイプライン |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三者エージェントパイプライン |

---

### パート7：Zava Creative Writer - キャップストーンアプリ

**ラボガイド：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4つの専門エージェントを持つ実践的なマルチエージェントアプリ
- 逐次パイプラインと評価者主導のフィードバックループ
- ストリーミング出力、製品カタログ検索、構造化されたJSON受け渡し
- Python (FastAPI)、JavaScript (Node.js CLI)、C# (.NETコンソール)での完全実装

**コードサンプル：**

| 言語 | ディレクトリ | 説明 |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | オーケストレーターを備えたFastAPIウェブサービス |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLIアプリケーション |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9コンソールアプリケーション |

---

### パート8：評価主導の開発

**ラボガイド：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- ゴールデンデータセットによるAIエージェントの体系的評価フレームワーク構築
- ルールベースチェック（文字数、キーワード網羅、禁止語）＋LLMジャッジスコアリング
- プロンプトバリアントの並列比較と総合スコアカード
- パート7のZava Editorエージェントパターンをオフラインテストスイートに拡張
- Python、JavaScript、C#トラック対応

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | 評価フレームワーク |
| C# | `csharp/AgentEvaluation.cs` | 評価フレームワーク |
| JavaScript | `javascript/foundry-local-eval.mjs` | 評価フレームワーク |

---

### パート9：Whisperによる音声文字起こし

**ラボガイド：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- ローカル実行されるOpenAI Whisperを使った音声認識文字起こし
- プライバシーファーストなオーディオ処理 - 音声はデバイス外に送信されません
- Python、JavaScript、C#での`client.audio.transcriptions.create()`（Python/JS）および`AudioClient.TranscribeAudioAsync()`（C#）による利用
- 実践練習用のZavaテーマのサンプル音声ファイル付き

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper音声文字起こし |
| C# | `csharp/WhisperTranscription.cs` | Whisper音声文字起こし |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper音声文字起こし |

> **注意：** このラボでは **Foundry Local SDK** を利用してプログラム的にWhisperモデルをダウンロード・読み込みし、ローカルのOpenAI互換エンドポイントへ音声を送信して文字起こしを行います。Whisperモデル（`whisper`）はFoundry Localカタログにあり、完全にデバイス上で動作します。クラウドAPIキーやネットワークアクセスは不要です。

---

### パート 10: カスタムまたはHugging Faceモデルの使用

**ラボガイド：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAIモデルビルダーを用いたHugging Faceモデルの最適化ONNX形式へのコンパイル
- ハードウェア特化のコンパイル（CPU、NVIDIA GPU、DirectML、WebGPU）および量子化（int4、fp16、bf16）
- Foundry Local用チャットテンプレート設定ファイルの作成
- コンパイル済モデルのFoundry Localキャッシュへの追加
- CLI、REST API、OpenAI SDKからのカスタムモデル実行
- 参考例：Qwen/Qwen3-0.6Bのエンドツーエンドコンパイル

---

### パート 11: ローカルモデルでのツール呼び出し

**ラボガイド：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- ローカルモデルから外部関数の呼び出し（ツール／関数呼び出し）の有効化
- OpenAIの関数呼び出しフォーマットでのツールスキーマ定義
- マルチターンのツール呼び出し対話フローの処理
- ローカルでのツール呼び出し実行とモデルへの結果返却
- ツール呼び出しシナリオに適したモデル選択（Qwen 2.5、Phi-4-mini）
- SDKのネイティブ`ChatClient`を使ったツール呼び出し（JavaScript）

**コードサンプル：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 天気・人口ツールを使ったツール呼び出し |
| C# | `csharp/ToolCalling.cs` | .NETによるツール呼び出し |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClientを用いたツール呼び出し |

---

### パート 12: ZavaクリエイティブライターのWeb UI構築

**ラボガイド：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zavaクリエイティブライターにブラウザベースのフロントエンドを追加
- Python (FastAPI)、JavaScript (Node.js HTTP)、C# (ASP.NET Core)から共有UIを配信
- ブラウザでFetch APIとReadableStreamを使ったNDJSONのストリーミング処理
- ライブエージェントステータスバッジとリアルタイム記事テキストストリーム表示

**コード（共有UI）：**

| ファイル | 説明 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | ページレイアウト |
| `zava-creative-writer-local/ui/style.css` | スタイリング |
| `zava-creative-writer-local/ui/app.js` | ストリームリーダーとDOM更新ロジック |

**バックエンド追加：**

| 言語 | ファイル | 説明 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 静的UI配信に対応して更新 |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | オーケストレーターをラップする新HTTPサーバー |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新規ASP.NET CoreミニマルAPIプロジェクト |

---

### パート 13: ワークショップ完了

**ラボガイド：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 12パート全体の構築内容のまとめ
- アプリケーション拡張のためのさらなるアイデア
- 参考資料とドキュメントへのリンク

---

## プロジェクト構成

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
| Foundry Local公式サイト | [foundrylocal.ai](https://foundrylocal.ai) |
| モデルカタログ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| はじめにガイド | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDKリファレンス | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## ライセンス

このワークショップ資料は教育目的で提供されています。

---

**楽しい開発を！🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**免責事項**:  
本書類は AI 翻訳サービス [Co-op Translator](https://github.com/Azure/co-op-translator) を使用して翻訳されています。正確性を期していますが、自動翻訳には誤りや不正確な点が含まれる可能性があることをご了承ください。原文はその言語における正式な文書として扱われるべきです。重要な情報については専門の人間翻訳をお勧めします。本翻訳の利用による誤解や誤訳については一切責任を負いかねます。
<!-- CO-OP TRANSLATOR DISCLAIMER END -->