# 既知の問題 — Foundry Local ワークショップ

Windows上で動作する **Snapdragon X Elite (ARM64)** デバイスにて、Foundry Local SDK v0.9.0、CLI v0.8.117、.NET SDK 10.0 を使用してこのワークショップを構築およびテストした際に発生した問題。

> **最終検証日:** 2026-03-11

---

## 1. Snapdragon X Elite CPU を ONNX Runtime が認識しない

**状況:** オープン  
**重大度:** 警告（ブロッキングなし）  
**コンポーネント:** ONNX Runtime / cpuinfo  
**再現手順:** Snapdragon X Elite ハードウェア上で Foundry Local サービスを起動するたびに発生

Foundry Local サービスが起動するたびに、次の2つの警告が発生する：

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**影響:** 警告は外観上のものであり、推論自体は正しく動作している。ただし、毎回表示されるため、ワークショップ参加者が混乱する可能性がある。ONNX Runtime の cpuinfo ライブラリを更新し、Qualcomm Oryon CPU コアを認識できるようにする必要がある。

**期待される状態:** Snapdragon X Elite はエラーレベルのメッセージを出すことなく、対応済み ARM64 CPU として認識されること。

---

## 2. SingleAgent 起動時に NullReferenceException が発生する

**状況:** オープン（断続的）  
**重大度:** 重大（クラッシュ）  
**コンポーネント:** Foundry Local C# SDK + Microsoft Agent Framework  
**再現手順:** `dotnet run agent` を実行 — モデル読み込み直後にクラッシュ

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**状況:** 37行目で `model.IsCachedAsync(default)` が呼ばれる。クラッシュは新たに `foundry service stop` 後、エージェントの初回実行時に発生し、その後の同じコードでの実行は成功。

**影響:** 断続的に発生し、SDK のサービス初期化またはカタログクエリにおける競合状態を示唆している。`GetModelAsync()` 呼び出しはサービスの準備完了前に戻ってくる可能性がある。

**期待される状態:** `GetModelAsync()` はサービスが準備完了するまでブロックするか、初期化が完了していない場合は明確なエラーメッセージを返すべきである。

---

## 3. C# SDK は明示的な RuntimeIdentifier が必要

**状況:** オープン — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) で追跡中  
**重大度:** ドキュメントの欠落  
**コンポーネント:** `Microsoft.AI.Foundry.Local` NuGet パッケージ  
**再現手順:** `.csproj` に `<RuntimeIdentifier>` を含まない .NET 8+ プロジェクトを作成する

ビルドは次のように失敗する：

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**根本原因:** RID の指定は想定済みである。SDK はネイティブバイナリを同梱しており（`Microsoft.AI.Foundry.Local.Core` と ONNX Runtime への P/Invoke）、.NET はプラットフォーム固有のライブラリを解決するために RID を知る必要がある。

これは MS Learn のドキュメントで解説されている（[How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)）が、実行指示では以下のように示されている：

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
ただし、ユーザーは毎回 `-r` フラグを覚えておく必要があり、忘れやすい。

**回避策:** `.csproj` に自動検出のフォールバックを追加し、`dotnet run` をフラグなしで動作させる：

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` は MSBuild の組み込みプロパティで、ホストマシンの RID を自動的に解決する。SDK 自身のテストプロジェクトもこのパターンを使用している。明示的な `-r` フラグが与えられた場合はそれも尊重される。

> **注:** ワークショップの `.csproj` はこのフォールバックを含み、どのプラットフォームでも `dotnet run` がすぐに動くようになっている。

**期待される状態:** MS Learn ドキュメントの `.csproj` テンプレートにはこの自動検出パターンを含め、ユーザーが `-r` フラグを覚える必要がないようにするべきである。

---

## 4. JavaScript Whisper — 音声転写が空またはバイナリ出力を返す

**状況:** オープン（リグレッション — 初報告時より悪化）  
**重大度:** 重大  
**コンポーネント:** JavaScript Whisper 実装 (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**再現手順:** `node foundry-local-whisper.mjs` を実行 — すべての音声ファイルが転写テキストではなく空またはバイナリ出力を返す

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
元々は5番目の音声ファイルのみが空を返していたが、v0.9.x 現在は5つすべてのファイルがテキストに転写されずに1バイト（`\ufffd`）を返す。Python Whisper 実装（OpenAI SDK 使用）は同じファイルを正しく転写している。

**期待される状態:** `createAudioClient()` は Python/C# 実装と同様のテキスト転写を返すべき。

---

## 5. C# SDK は net8.0 のみを提供 — .NET 9 または .NET 10 の公式ターゲット無し

**状況:** オープン  
**重大度:** ドキュメントの欠落  
**コンポーネント:** `Microsoft.AI.Foundry.Local` NuGet パッケージ v0.9.0  
**インストールコマンド:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet パッケージは単一のターゲットフレームワークのみを含む：

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
`net9.0` も `net10.0` も含まれていない。一方で、関連パッケージ `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) は `net8.0`、`net9.0`、`net10.0`、`net472`、`netstandard2.0` を出荷している。

### 互換性テスト

| ターゲットフレームワーク | ビルド | 実行 | 備考 |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | 公式にサポート済み |
| net9.0 | ✅ | ✅ | フォワードコンパチビリティによりビルド可能 — ワークショップサンプルで使用 |
| net10.0 | ✅ | ✅ | .NET 10.0.3 ランタイムでフォワードコンパチでビルド・実行可能 |

net8.0 アセンブリは .NET のフォワードコンパチビリティ機構により新しいランタイム上でも読み込まれ、ビルドは成功する。ただしこれは SDK チームによるドキュメント化やテストはされていない。

### なぜサンプルは net9.0 ターゲットか

1. **.NET 9 は最新の安定版** — ほとんどのワークショップ参加者がインストール済みである  
2. <strong>フォワード互換が機能する</strong> — NuGet の net8.0 アセンブリは .NET 9 ランタイム上で問題なく動作する  
3. **.NET 10（プレビュー/RC）** は新しすぎて、すべての人が動かせるようにしたいワークショップではターゲット外

**期待される状態:** 将来の SDK リリースでは、`Microsoft.Agents.AI.OpenAI` の例に倣い、`net8.0` と一緒に `net9.0` と `net10.0` の TFM も含め、より新しいランタイムのサポートを文書化・検証すべき。

---

## 6. JavaScript ChatClient のストリーミングはコールバックを使用、Async Iterator ではない

**状況:** オープン  
**重大度:** ドキュメントの欠落  
**コンポーネント:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` が返す `ChatClient` は `completeStreamingChat()` メソッドを持つが、<strong>非同期イテレータではなくコールバックパターン</strong>を使用している：

```javascript
// ❌ これは動作しません — 「ストリームは非同期イテラブルではありません」というエラーが発生します
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ 正しいパターン — コールバックを渡します
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**影響:** OpenAI SDK の async イテレーションパターン (`for await`) に慣れている開発者には混乱を招く可能性がある。コールバックは有効な関数でなければならず、そうでないと SDK が「Callback must be a valid function.」と例外を投げる。

**期待される状態:** SDK リファレンスでコールバックパターンを文書化すること。あるいは、OpenAI SDK と整合させるために async イテレータパターンをサポートすること。

---

## 環境詳細

| コンポーネント | バージョン |
|-----------|---------|
| OS | Windows 11 ARM64 |
| ハードウェア | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |