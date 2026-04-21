# 変更履歴 — Foundry Local ワークショップ

このワークショップのすべての注目すべき変更点は以下に記録されています。

---

## 2026-03-11 — パート12＆13、Web UI、Whisper書き換え、WinML/QNN修正、および検証

### 追加
- **パート12：Zavaクリエイティブライター用Web UI構築** — ストリーミングNDJSON、ブラウザーの`ReadableStream`、ライブエージェントステータスバッジ、リアルタイム記事テキストストリーミングを扱う新しいラボガイド（`labs/part12-zava-ui.md`）
- **パート13：ワークショップ完了** — 全12パートのまとめ、さらなるアイデア、リソースリンクを含む新しいまとめラボ（`labs/part13-workshop-complete.md`）
- **Zava UIフロントエンド：** `zava-creative-writer-local/ui/index.html`、`style.css`、`app.js` — 3つのバックエンドすべてで使用される共有のバニラHTML/CSS/JSブラウザーインターフェース
- **JavaScript HTTPサーバー：** `zava-creative-writer-local/src/javascript/server.mjs` — ブラウザー経由アクセス向けオーケストレーターをラップする新しいExpressスタイルHTTPサーバー
- **C# ASP.NET Coreバックエンド：** `zava-creative-writer-local/src/csharp-web/Program.cs` および `ZavaCreativeWriterWeb.csproj` — UIとNDJSONストリーミングを提供する新しいミニマルAPIプロジェクト
- **オーディオサンプル生成器：** `samples/audio/generate_samples.py` — `pyttsx3` を使用したオフラインTTSスクリプトで、パート9向けZavaテーマのWAVファイルを生成
- **オーディオサンプル：** `samples/audio/zava-full-project-walkthrough.wav` — 転写テスト用の新しい長めのオーディオサンプル
- **検証スクリプト：** `validate-npu-workaround.ps1` — すべてのC#サンプルに対するNPU/QNN回避策を自動検証するPowerShellスクリプト
- **Mermaid図SVG：** `images/part12-architecture.svg`、`part12-message-types.svg`、`part12-streaming-sequence.svg`
- **WinMLクロスプラットフォームサポート：** 3つのC# `.csproj` ファイル（`csharp/csharp.csproj`、`zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`、`zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`）が条件付きTFMと相互排他のパッケージ参照を採用しクロスプラットフォームを実現。Windows上では `net9.0-windows10.0.26100` TFM と `Microsoft.AI.Foundry.Local.WinML` （QNN EPプラグインを含むスーパーセット）、非Windows上では `net9.0` TFM と `Microsoft.AI.Foundry.Local`（基本SDK）。Zavaプロジェクトのハードコーディングされた`win-arm64` RIDは自動検出に置き換え。壊れたwin-arm64参照を持つ`Microsoft.ML.OnnxRuntime.Gpu.Linux`のネイティブアセットを除外する移行依存性回避策を導入。7つのC#ファイルすべてから以前のtry/catchによるNPU回避策を削除。

### 変更
- **パート9（Whisper）：** 大幅な書き換え — JavaScriptは手動ONNX Runtime推論ではなくSDK組み込みの`AudioClient`（`model.createAudioClient()`）を使用に変更。JS/C#の`AudioClient`方式とPython ONNX Runtime方式の比較テーブル、アーキテクチャ説明、パイプライン図を更新
- **パート11：** ナビゲーションリンク更新（現在はパート12を指す）；ツール呼び出しフローとシーケンスのSVGレンダリング図を追加
- **パート10：** ワークショップ終了ではなくパート12経由にナビゲーションルートを更新
- **Python Whisper（`foundry-local-whisper.py`）：** 追加オーディオサンプルと改善されたエラーハンドリングで拡張
- **JavaScript Whisper（`foundry-local-whisper.mjs`）：** 手動ONNX Runtimeセッションではなく`model.createAudioClient()`の`audioClient.transcribe()`を使用するよう書き換え
- **Python FastAPI（`zava-creative-writer-local/src/api/main.py`）：** APIに加えて静的UIファイルを提供するよう更新
- **Zava C#コンソール（`zava-creative-writer-local/src/csharp/Program.cs`）：** NPU回避策を削除（現在はWinMLパッケージが対応）
- **README.md：** パート12セクションをコードサンプルテーブルとバックエンド追加込みで追加；パート13セクションを追加；学習目標とプロジェクト構造を更新
- **KNOWN-ISSUES.md：** 解決済みIssue #7（C# SDK NPUモデルバリアント - 現在はWinMLパッケージが対応）を削除。残りの問題を#1～#6に再番号付け。.NET SDK 10.0.104に合わせて環境詳細を更新
- **AGENTS.md：** プロジェクト構造ツリーに新しい`zava-creative-writer-local`エントリ（`ui/`、`csharp-web/`、`server.mjs`）を追加；C#主要パッケージと条件付きTFMの詳細を更新
- **labs/part2-foundry-local-sdk.md：** 条件付きTFM、相互排他パッケージ参照、説明注記を含む完全なクロスプラットフォームパターンを示す`.csproj`例に更新

### 検証済み
- 3つのC#プロジェクト（`csharp`、`ZavaCreativeWriter`、`ZavaCreativeWriterWeb`）がWindows ARM64で正常ビルド
- チャットサンプル（`dotnet run chat`）：モデルはWinML/QNN経由で`phi-3.5-mini-instruct-qnn-npu:1`としてロード — CPUフォールバックなしでNPUバリアントを直接ロード
- エージェントサンプル（`dotnet run agent`）：マルチターン会話で終了コード0で正常実行
- Foundry Local CLI v0.8.117およびSDK v0.9.0が.NET SDK 9.0.312で動作

---

## 2026-03-11 — コード修正、モデルクリーンアップ、Mermaid図、検証

### 修正
- **すべての21コードサンプル（7 Python、7 JavaScript、7 C#）：** 終了時に`model.unload()` / `unload_model()` / `model.UnloadAsync()`によるクリーンアップを追加しOGAメモリリーク警告を解消（既知の問題 #4）
- **csharp/WhisperTranscription.cs：** 脆弱な`AppContext.BaseDirectory`相対パスを、ディレクトリを遡って`samples/audio`を確実に見つける`FindSamplesDirectory()`に置換（既知の問題 #7）
- **csharp/csharp.csproj：** ハードコードされた`<RuntimeIdentifier>win-arm64</RuntimeIdentifier>`を`$(NETCoreSdkRuntimeIdentifier)`による自動検出フォールバックに置換。これにより`dotnet run`が任意のプラットフォームで`-r`オプション不要で動作（[Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)）

### 変更
- **パート8：** 評価駆動ループをASCIIボックス図からレンダリング済みSVG画像に変換
- **パート10：** コンパイルパイプライン図をASCII矢印からレンダリング済みSVG画像に変換
- **パート11：** ツール呼び出しフローとシーケンス図をレンダリング済みSVG画像に変換
- **パート10：** 「Workshop Complete!」セクションを最終ラボのパート11に移動；代わりに「次のステップ」リンクを設置
- **KNOWN-ISSUES.md：** CLI v0.8.117に基づく全問題の再検証。解決済み問題を削除：OGAメモリリーク（クリーンアップ追加済み）、Whisperパス（FindSamplesDirectory）、HTTP 500持続推論（再現不能、[#494](https://github.com/microsoft/Foundry-Local/issues/494)）、tool_choice制限（現在は`"required"`およびqwen2.5-0.5bの特定関数ターゲティングに対応）。JS Whisper問題を更新 — すべてのファイルが空またはバイナリ出力を返す（v0.9.xからの後退、重大度をMajorに引き上げ）。C# RIDの自動検出回避策を追加し[#497](https://github.com/microsoft/Foundry-Local/issues/497) をリンク。未解決は7件。
- **javascript/foundry-local-whisper.mjs：** クリーンアップ変数名を修正（`whisperModel`→`model`）

### 検証済み
- Python：`foundry-local.py`、`foundry-local-rag.py`、`foundry-local-tool-calling.py` がクリーンアップ付きで正常実行
- JavaScript：`foundry-local.mjs`、`foundry-local-rag.mjs`、`foundry-local-tool-calling.mjs` がクリーンアップ付きで正常実行
- C#：`dotnet build`が警告・エラーなしで成功（net9.0ターゲット）
- 7つのPythonファイルすべてが`py_compile`構文チェック合格
- 7つのJavaScriptファイルすべてが`node --check`構文検証合格

---

## 2026-03-10 — パート11：ツール呼び出し、SDK API拡張、およびモデル対応範囲

### 追加
- **パート11：ローカルモデルでのツール呼び出し** — ツールスキーマ、多段階フロー、複数ツール呼び出し、カスタムツール、ChatClientツール呼び出し、`tool_choice`を扱う8つの演習を含む新規ラボガイド（`labs/part11-tool-calling.md`）
- **Pythonサンプル：** `python/foundry-local-tool-calling.py` — OpenAI SDKを使った`get_weather`/`get_population`ツール呼び出し
- **JavaScriptサンプル：** `javascript/foundry-local-tool-calling.mjs` — SDKネイティブの`ChatClient`（`model.createChatClient()`）を使ったツール呼び出し
- **C#サンプル：** `csharp/ToolCalling.cs` — OpenAI C# SDKの`ChatTool.CreateFunctionTool()`を使ったツール呼び出し
- **パート2、演習7：** ネイティブ`ChatClient` — `model.createChatClient()`（JS）および`model.GetChatClientAsync()`（C#）をOpenAI SDKの代替として紹介
- **パート2、演習8：** モデルバリアントとハードウェア選択 — `selectVariant()`、`variants`、NPUバリアント表（7モデル）
- **パート2、演習9：** モデルアップグレードとカタログ更新 — `is_model_upgradeable()`、`upgrade_model()`、`updateModels()`
- **パート2、演習10：** 推論モデル — `<think>`タグ解析例付き`phi-4-mini-reasoning`
- **パート3、演習4：** OpenAI SDKの代替としての`createChatClient`とストリーミングコールバックパターンのドキュメント
- **AGENTS.md：** ツール呼び出し、ChatClient、および推論モデルのコーディング規約を追加

### 変更
- **パート1：** モデルカタログ拡大 — phi-4-mini-reasoning、gpt-oss-20b、phi-4、qwen2.5-7b、qwen2.5-coder-7b、whisper-large-v3-turboを追加
- **パート2：** APIリファレンステーブル拡張 — `createChatClient`、`createAudioClient`、`removeFromCache`、`selectVariant`、`variants`、`isLoaded`、`stopWebService`、`is_model_upgradeable`、`upgrade_model`、`httpx_client`、`getModels`、`getCachedModels`、`getLoadedModels`、`updateModels`、`GetModelVariantAsync`、`UpdateModelsAsync` を追加
- **パート2：** 演習7-9を10-13に再番号付けし新演習のスペースを確保
- **パート3：** 主要なポイント表にネイティブChatClientを追加
- **README.md：** パート11セクションをコードサンプルテーブル付きで追加；学習目標#11を追加；プロジェクト構造図を更新
- **csharp/Program.cs：** CLIルーターに`toolcall`ケースを追加しヘルプテキスト更新

---

## 2026-03-09 — SDK v0.9.0アップデート、英国英語化、および検証

### 変更
- **すべてのコードサンプル（Python、JavaScript、C#）：** Foundry Local SDK v0.9.0 APIに更新 — `await catalog.getModel()`の`await`漏れ修正、`FoundryLocalManager`初期化パターン更新、エンドポイント検出修正
- **すべてのラボガイド（パート1-10）：** 英国英語化（colour、catalogue、optimisedなど）
- **すべてのラボガイド：** SDKコード例をv0.9.0 APIサーフェスに合わせ更新
- **すべてのラボガイド：** APIリファレンステーブルと演習コードブロックを更新
- **JavaScriptクリティカル修正：** `catalog.getModel()`の`await`不足を修正 — Promiseオブジェクトが返され静かな失敗を起こしていた

### 検証済み
- すべてのPythonサンプルがFoundry Localサービスで正常実行
- すべてのJavaScriptサンプルが正常実行（Node.js 18以上）
- C#プロジェクトが.NET 9.0でビルドおよび実行（net8.0 SDKの後方互換）
- ワークショップ内29ファイルを修正し検証完了

---

## ファイル索引

| ファイル | 最終更新日 | 説明 |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | モデルカタログ拡大 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | 新演習7-10、APIテーブル拡大 |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | 新演習4（ChatClient）、要点更新 |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + 英国英語 |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + 英国英語 |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + 英国英語 |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + 英国英語 |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid 図 |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + 英国英語 |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid 図、ワークショップ完了をパート11に移動 |
| `labs/part11-tool-calling.md` | 2026-03-11 | 新規ラボ、Mermaid 図、ワークショップ完了セクション |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | 新規：ツール呼び出しサンプル |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | 新規：ツール呼び出しサンプル |
| `csharp/ToolCalling.cs` | 2026-03-10 | 新規：ツール呼び出しサンプル |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLIコマンドを追加 |
| `README.md` | 2026-03-10 | パート11、プロジェクト構造 |
| `AGENTS.md` | 2026-03-10 | ツール呼び出し + ChatClient の規約 |
| `KNOWN-ISSUES.md` | 2026-03-11 | 解決済み Issue #7 を削除、未解決の問題6件が残る |
| `csharp/csharp.csproj` | 2026-03-11 | クロスプラットフォーム TFM、WinML/ベース SDK 条件付き参照 |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | クロスプラットフォーム TFM、自動 RID 検出 |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | クロスプラットフォーム TFM、自動 RID 検出 |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU の try/catch 回避策を削除 |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU の try/catch 回避策を削除 |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU の try/catch 回避策を削除 |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU の try/catch 回避策を削除 |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU の try/catch 回避策を削除 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | クロスプラットフォーム .csproj の例 |
| `AGENTS.md` | 2026-03-11 | C# パッケージと TFM の詳細を更新 |
| `CHANGELOG.md` | 2026-03-11 | このファイル |