![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# パート9：WhisperとFoundry Localを使った音声文字起こし

> **目的:** Foundry Localを通じてローカルで動作するOpenAI Whisperモデルを用いて、オーディオファイルを完全にデバイス内で文字起こしすること。クラウドは一切不要。

## 概要

Foundry Localはテキスト生成だけでなく、<strong>音声認識</strong>モデルにも対応しています。このラボでは、<strong>OpenAI Whisper Medium</strong>モデルを使用して、オーディオファイルをあなたのマシン上で完全に文字起こしします。これは、Zavaのカスタマーサービスコールや製品レビュー録音、ワークショップの計画など、オーディオデータがデバイス外に出てはならないシナリオに最適です。

---

## 学習目標

このラボを終える頃には、以下のことができるようになります：

- Whisper音声認識モデルの仕組みと機能を理解する
- Foundry Localを使ってWhisperモデルをダウンロードし実行する
- Foundry Local SDKを使ってPython、JavaScript、C#で音声ファイルを文字起こしする
- 完全オンデバイスで動作する簡単な文字起こしサービスを構築する
- Foundry Localにおけるチャット/テキストモデルと音声モデルの違いを理解する

---

## 必要条件

| 必須条件 | 詳細 |
|-------------|---------|
| **Foundry Local CLI** | バージョン **0.8.101以降** （Whisperモデルはv0.8.101以降から利用可能） |
| **OS** | Windows 10/11 (x64 または ARM64) |
| <strong>ランタイム</strong> | **Python 3.9以上** および/または **Node.js 18以上** および/または **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| <strong>事前完了</strong> | [Part 1: Getting Started](part1-getting-started.md)、[Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md)、および [Part 3: SDKs and APIs](part3-sdk-and-apis.md) |

> **注意:** Whisperモデルのダウンロードは<strong>SDK</strong>経由で行う必要があります（CLIではできません）。CLIは音声文字起こしエンドポイントに対応していません。バージョン確認は次のコマンドで:
> ```bash
> foundry --version
> ```

---

## コンセプト: WhisperとFoundry Localの連携

OpenAI Whisperモデルは多様な音声データに基づく汎用音声認識モデルです。Foundry Local上で利用すると：

- モデルは<strong>完全にCPU上で実行</strong>され、GPUは不要
- オーディオデータはデバイス外に一切出ず、<strong>完全なプライバシー</strong>が保たれる
- Foundry Local SDKがモデルのダウンロードとキャッシュ管理を担当
- <strong>JavaScriptとC#</strong>はSDKに組み込まれた`AudioClient`を備え、文字起こしパイプライン全体を手動ONNXセットアップなしで処理
- <strong>Python</strong>はSDKでモデル管理を行い、ONNX Runtimeでエンコーダー/デコーダーONNXモデルに直接推論

### パイプラインの動作（JavaScriptとC#） — SDKのAudioClient

1. <strong>Foundry Local SDK</strong>がWhisperモデルをダウンロードしキャッシュ
2. `model.createAudioClient()` (JS)、または `model.GetAudioClientAsync()` (C#)で`AudioClient`を作成
3. `audioClient.transcribe(path)` (JS)、または `audioClient.TranscribeAudioAsync(path)` (C#)が内部で文字起こし全パイプラインを処理 — 音声前処理、エンコーダ、デコーダ、トークンデコード
4. `AudioClient`は`settings.language`プロパティ（英語の場合は`"en"`）を提供し、正確な文字起こしを補助

### パイプラインの動作（Python） — ONNX Runtime

1. <strong>Foundry Local SDK</strong>がWhisperのONNXモデルファイル群をダウンロードしキャッシュ
2. <strong>音声前処理</strong>でWAV音声をメルスペクトログラムに変換（80メルビン×3000フレーム）
3. <strong>エンコーダ</strong>がメルスペクトログラムを処理し、隠れ状態とクロスアテンションのキー/バリューテンソルを生成
4. <strong>デコーダ</strong>が自己回帰的に1トークンずつ生成し、テキスト終了トークンまで進行
5. <strong>トークナイザー</strong>が出力トークンIDを人間が読めるテキストにデコード

### Whisperモデルのバリエーション

| エイリアス | モデルID | デバイス | サイズ | 説明 |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPUアクセラレーション(CUDA)対応 |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU最適化（ほとんどのデバイス向け推奨） |

> **注意:** チャットモデルのようにデフォルトでリストされるわけではなく、Whisperモデルは`automatic-speech-recognition`タスクに分類されています。詳細は `foundry model info whisper-medium` で確認できます。

---

## ラボ演習

### 演習 0 - サンプル音声ファイルの取得

このラボではZava DIY製品シナリオに基づく事前作成済みWAVファイルを使用します。付属スクリプトで生成しましょう:

```bash
# リポジトリのルートから - まず .venv を作成してアクティベートします
python -m venv .venv

# Windows（PowerShell）：
.venv\Scripts\Activate.ps1
# macOS：
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

これにより`samples/audio/`に6つのWAVファイルが作成されます：

| ファイル名 | シナリオ |
|------|----------|
| `zava-customer-inquiry.wav` | お客様による<strong>Zava ProGrip コードレスドリル</strong>への問い合わせ |
| `zava-product-review.wav` | お客様による<strong>Zava UltraSmooth 室内壁用塗料</strong>のレビュー |
| `zava-support-call.wav` | <strong>Zava TitanLock ツールチェスト</strong>に関するサポートコール |
| `zava-project-planning.wav` | DIYユーザーによる<strong>Zava EcoBoard コンポジットデッキ</strong>のデッキ計画 |
| `zava-workshop-setup.wav` | <strong>Zava製品5種全て</strong>を使ったワークショップの解説 |
| `zava-full-project-walkthrough.wav` | <strong>Zava製品全て</strong>を使ったガレージリノベーション詳細解説（約4分、長尺音声テスト用） |

> **ヒント:** ご自身のWAV/MP3/M4AファイルやWindowsのボイスレコーダーで録音したものも使用可能です。

---

### 演習 1 - SDKを使ってWhisperモデルをダウンロードする

新しいFoundry LocalバージョンのCLIがWhisperモデル対応していないため、<strong>SDK</strong>を利用してモデルをダウンロードしロードします。言語を選択してください：

<details>
<summary><b>🐍 Python</b></summary>

**SDKのインストール：**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# サービスを開始する
manager = FoundryLocalManager()
manager.start_service()

# カタログ情報を確認する
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# すでにキャッシュされているか確認する
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# モデルをメモリにロードする
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py`として保存し実行：
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDKのインストール：**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// マネージャーを作成してサービスを開始する
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// カタログからモデルを取得する
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// モデルをメモリにロードする
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs`として保存し実行：
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDKのインストール：**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **なぜCLIではなくSDKなのか？** Foundry Local CLIはWhisperモデルの直接ダウンロードや提供に対応していません。SDKを使用すると音声モデルをプログラム的に確実にダウンロード・管理できます。JavaScriptとC#のSDKは組み込み`AudioClient`で文字起こしパイプライン全体を処理。PythonはONNX Runtimeで直接キャッシュモデルファイルに推論を行います。

---

### 演習 2 - Whisper SDKの理解

Whisperの文字起こしは言語によって異なるアプローチをとります。<strong>JavaScriptとC#</strong>はFoundry Local SDKに組み込まれた`AudioClient`で音声前処理からデコードまで全て1メソッドで処理。<strong>Python</strong>はSDKでモデル管理を行い、ONNX Runtimeを使用してエンコーダ・デコーダのONNXモデルに直接推論します。

| コンポーネント | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDKパッケージ** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| <strong>モデル管理</strong> | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| <strong>特徴抽出</strong> | `WhisperFeatureExtractor` + `librosa` | SDKの`AudioClient`が処理 | SDKの`AudioClient`が処理 |
| <strong>推論</strong> | `ort.InferenceSession` (エンコーダ + デコーダ) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| <strong>トークンデコード</strong> | `WhisperTokenizer` | SDKの`AudioClient`が処理 | SDKの`AudioClient`が処理 |
| <strong>言語設定</strong> | デコーダーの`forced_ids`で指定 | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| <strong>入力</strong> | WAVファイルパス | WAVファイルパス | WAVファイルパス |
| <strong>出力</strong> | デコード済みテキスト文字列 | `result.text` | `result.Text` |

> **重要:** `AudioClient`の言語設定（例："en"）は必ず行ってください。言語を明示しないと、モデルは言語を自動検出しようとして文字化けする可能性があります。

> **SDKの使い方:** Pythonは`FoundryLocalManager(alias)`で起動し、`get_cache_location()`でONNXモデルファイルのパスを取得。JavaScriptとC#はSDKの`AudioClient`を `model.createAudioClient()`（JS）または `model.GetAudioClientAsync()`（C#）で取得し、文字起こしパイプライン全体を処理します。[Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md)参照。

---

### 演習 3 - シンプルな文字起こしアプリを作る

言語トラックを選択し、音声ファイルを文字起こしする最小限のアプリケーションを作成します。

> **対応音声フォーマット:** WAV, MP3, M4A。推奨は16kHzサンプルレートのWAV。

<details>
<summary><h3>Pythonトラック</h3></summary>

#### セットアップ

```bash
cd python
python -m venv venv

# 仮想環境を有効化する:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### 文字起こしコード

`foundry-local-whisper.py`というファイルを作成：

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# ステップ1: ブートストラップ - サービスを開始し、モデルをダウンロードして読み込みます
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# キャッシュされたONNXモデルファイルへのパスを構築する
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# ステップ2: ONNXセッションと特徴抽出器を読み込む
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# ステップ3: メルスペクトログラム特徴を抽出する
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# ステップ4: エンコーダを実行する
enc_out = encoder.run(None, {"audio_features": input_features})
# 最初の出力は隠れ状態で、残りはクロスアテンションのKVペアです
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# ステップ5: オートレグレッシブデコーディング
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, 転写, タイムスタンプなし
input_ids = np.array([initial_tokens], dtype=np.int32)

# 空のセルフアテンションKVキャッシュ
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # テキストの終わり
        break
    generated.append(next_token)

    # セルフアテンションKVキャッシュを更新する
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### 実行

```bash
# Zava製品のシナリオを転写する
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# または他のものを試す:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Pythonでのポイント

| メソッド | 目的 |
|--------|---------|
| `FoundryLocalManager(alias)` | ブートストラップ：サービス起動、モデルダウンロード、ロード |
| `manager.get_cache_location()` | キャッシュ済みONNXモデルファイルのパス取得 |
| `WhisperFeatureExtractor.from_pretrained()` | メルスペクトログラム特徴抽出器のロード |
| `ort.InferenceSession()` | エンコーダ・デコーダ用ONNX Runtimeセッション作成 |
| `tokenizer.decode()` | 出力トークンIDをテキストに変換 |

</details>

<details>
<summary><h3>JavaScriptトラック</h3></summary>

#### セットアップ

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### 文字起こしコード

`foundry-local-whisper.mjs`というファイルを作成：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// ステップ1: ブートストラップ - マネージャーを作成し、サービスを開始し、モデルを読み込む
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// ステップ2: オーディオクライアントを作成して文字起こしを行う
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// クリーンアップ
await model.unload();
```

> **注意:** Foundry Local SDKは`model.createAudioClient()`経由で組み込みの`AudioClient`を提供しており、ONNX推論パイプライン全体を内包します。`onnxruntime-node`のインポートは不要です。正確な英語文字起こしのため、`audioClient.settings.language = "en"` を必ず設定してください。

#### 実行

```bash
# Zava製品のシナリオを書き起こす
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# または他のものを試す：
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScriptでのポイント

| メソッド | 目的 |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | マネージャーシングルトンの作成 |
| `await catalog.getModel(alias)` | カタログからモデル取得 |
| `model.download()` / `model.load()` | Whisperモデルのダウンロードとロード |
| `model.createAudioClient()` | 文字起こし用AudioClientの作成 |
| `audioClient.settings.language = "en"` | 文字起こし言語の設定（正確な出力に必須） |
| `audioClient.transcribe(path)` | 音声ファイルを文字起こし（戻り値は `{ text, duration }`） |

</details>

<details>
<summary><h3>C#トラック</h3></summary>

#### セットアップ

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **注意:** C#トラックは`Microsoft.AI.Foundry.Local`パッケージを利用し、`model.GetAudioClientAsync()`で組み込みの`AudioClient`を取得します。これにより文字起こしパイプライン全体がプロセス内処理され、別途ONNX Runtimeセットアップは不要です。

#### 文字起こしコード

`Program.cs`の内容を以下に置き換え：

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### 実行

```bash
# Zava製品のシナリオを書き起こす
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# または他のものを試してください:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C#でのポイント

| メソッド | 目的 |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Localを構成して初期化 |
| `catalog.GetModelAsync(alias)` | カタログからモデル取得 |
| `model.DownloadAsync()` | Whisperモデルをダウンロード |
| `model.GetAudioClientAsync()` | AudioClient(チャットクライアントではなく)を取得 |
| `audioClient.Settings.Language = "en"` | 文字起こし言語を設定（正確な出力に必須） |
| `audioClient.TranscribeAudioAsync(path)` | 音声ファイルを文字起こし |
| `result.Text` | 文字起こしされたテキスト |
> **C# vs Python/JS:** C# SDK は JavaScript SDK と同様に、`model.GetAudioClientAsync()` を介したインプロセスの文字起こし用に組み込みの `AudioClient` を提供します。Python はキャッシュされたエンコーダー/デコーダーモデルに対して直接推論するために ONNX Runtime を使用します。

</details>

---

### 演習 4 - すべての Zava サンプルを一括文字起こしする

動作する文字起こしアプリができたので、5つの Zava サンプルファイルすべてを文字起こしして結果を比較しましょう。

<details>
<summary><h3>Pythonトラック</h3></summary>

完全なサンプル `python/foundry-local-whisper.py` はすでに一括文字起こしに対応しています。引数なしで実行すると、`samples/audio/` 内のすべての `zava-*.wav` ファイルを文字起こしします:

```bash
cd python
python foundry-local-whisper.py
```

このサンプルは `FoundryLocalManager(alias)` を使ってブートストラップし、その後ファイルごとにエンコーダーとデコーダーの ONNX セッションを実行します。

</details>

<details>
<summary><h3>JavaScriptトラック</h3></summary>

完全なサンプル `javascript/foundry-local-whisper.mjs` はすでに一括文字起こしに対応しています。引数なしで実行すると、`samples/audio/` 内のすべての `zava-*.wav` ファイルを文字起こしします:

```bash
cd javascript
node foundry-local-whisper.mjs
```

このサンプルは `FoundryLocalManager.create()` と `catalog.getModel(alias)` を用いて SDK を初期化し、`AudioClient`（`settings.language = "en"`）を使って各ファイルを文字起こしします。

</details>

<details>
<summary><h3>C#トラック</h3></summary>

完全なサンプル `csharp/WhisperTranscription.cs` はすでに一括文字起こしに対応しています。特定のファイル引数なしで実行すると、`samples/audio/` 内のすべての `zava-*.wav` ファイルを文字起こしします:

```bash
cd csharp
dotnet run whisper
```

このサンプルは `FoundryLocalManager.CreateAsync()` と SDK の `AudioClient`（`Settings.Language = "en"`）を使用してインプロセスの文字起こしを行います。

</details>

**チェックポイント:** 文字起こしの出力を `samples/audio/generate_samples.py` の元テキストと比較してください。"Zava ProGrip" のような製品名や "brushless motor" や "composite decking" のような専門用語を Whisper がどの程度正確に捉えられているかに注目しましょう。

---

### 演習 5 - 主要コードパターンの理解

Whisper の文字起こしがチャット補完とどのように異なるかを3言語で学びましょう:

<details>
<summary><b>Python - チャットとの主な違い</b></summary>

```python
# チャット補完（パート2-6）：
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# 音声文字起こし（このパート）：
# OpenAIクライアントの代わりにONNX Runtimeを直接使用
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# …自己回帰デコーダーループ…
print(tokenizer.decode(generated_tokens))
```

**重要なポイント:** チャットモデルは `manager.endpoint` 経由で OpenAI 互換APIを使用します。Whisper は SDK を用いてキャッシュ済み ONNX モデルファイルを見つけ、ONNX Runtime で直接推論を実行します。

</details>

<details>
<summary><b>JavaScript - チャットとの主な違い</b></summary>

```javascript
// チャット補完（パート2-6）:
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// 音声文字起こし（このパート）:
// SDKの組み込みAudioClientを使用
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // 最良の結果のために常に言語を設定してください
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**重要なポイント:** チャットモデルは `manager.urls[0] + "/v1"` 経由で OpenAI 互換APIを使用します。Whisper の文字起こしは SDK の `AudioClient`（`model.createAudioClient()` で取得）を使います。自動検出による乱れを避けるために `settings.language` を設定します。

</details>

<details>
<summary><b>C# - チャットとの主な違い</b></summary>

C# では SDK 組み込みの `AudioClient` を用いてインプロセスの文字起こしを行います：

**モデル初期化:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**文字起こし:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**重要なポイント:** C# は `FoundryLocalManager.CreateAsync()` を使い、直接 `AudioClient` を取得します — ONNX Runtime のセットアップ不要です。自動検出による乱れを防ぐため必ず `Settings.Language` を設定してください。

</details>

> **まとめ:** Python はモデル管理に Foundry Local SDK を使い、エンコーダー/デコーダーモデルに対して ONNX Runtime で直接推論します。JavaScript と C# はどちらも SDK の組み込み `AudioClient` を活用し、クライアントを作成し言語設定をし、 `transcribe()` / `TranscribeAudioAsync()` を呼び出します。正確な結果を得るために AudioClient で必ず言語プロパティを設定してください。

---

### 演習 6 - 実験

理解を深めるために次の変更を試してみてください:

1. <strong>異なる音声ファイルを試す</strong> - Windows のボイスレコーダーで自分の声を録音し WAV で保存して文字起こしする

2. <strong>モデルバリエーションの比較</strong> - NVIDIA GPU を持っているなら CUDA 変種を試す:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   文字起こし速度を CPU 版と比較してください。

3. <strong>出力フォーマットの追加</strong> - JSON レスポンスに以下を含めることができます:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API の構築** - 文字起こしコードをウェブサーバーにラップする:

   | 言語 | フレームワーク | 例 |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` と `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` と `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` と `IFormFile` |

5. <strong>マルチターンの文字起こし連携</strong> - Part 4 のチャットエージェントと組み合わせて、まず音声を文字起こししてからテキストをエージェントに渡して解析や要約を行う。

---

## SDK オーディオ API リファレンス

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — `AudioClient` インスタンスを作成
> - `audioClient.settings.language` — 文字起こし言語を設定（例："en"）
> - `audioClient.settings.temperature` — ランダム性の制御（任意）
> - `audioClient.transcribe(filePath)` — ファイルを文字起こし、`{ text, duration }` を返す
> - `audioClient.transcribeStreaming(filePath, callback)` — コールバックを通じて文字起こしのチャンクをストリーム配信
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — `OpenAIAudioClient` インスタンスを作成
> - `audioClient.Settings.Language` — 文字起こし言語を設定（例："en"）
> - `audioClient.Settings.Temperature` — ランダム性の制御（任意）
> - `await audioClient.TranscribeAudioAsync(filePath)` — ファイルを文字起こし、`.Text` プロパティを持つオブジェクトを返す
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — 文字起こしチャンクの `IAsyncEnumerable` を返す

> **ヒント:** 文字起こし前に必ず言語プロパティを設定してください。設定しないと Whisper モデルが自動検出を試み、乱れた出力（一文字の代替文字のみなど）になることがあります。

---

## 比較: チャットモデル vs. Whisper

| 項目 | チャットモデル (パート3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| <strong>タスクタイプ</strong> | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| <strong>入力</strong> | テキストメッセージ (JSON) | 音声ファイル (WAV/MP3/M4A) | 音声ファイル (WAV/MP3/M4A) |
| <strong>出力</strong> | 生成されたテキスト (ストリーミング可) | 文字起こしテキスト (完全) | 文字起こしテキスト (完全) |
| **SDKパッケージ** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **APIメソッド** | `client.chat.completions.create()` | ONNX Runtime 直接呼び出し | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| <strong>言語設定</strong> | 対応なし | デコーダーのプロンプトトークン | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| <strong>ストリーミング</strong> | あり | なし | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| <strong>プライバシーの利点</strong> | コードとデータはローカルに保持 | 音声データはローカルに保持 | 音声データはローカルに保持 |

---

## 主要ポイントまとめ

| 概念 | 学んだこと |
|---------|-----------------|
| **Whisperオンデバイス** | 音声からテキストへの変換が完全にローカルで動作し、Zavaの顧客通話や製品レビューのオンデバイス文字起こしに最適 |
| **SDK AudioClient** | JavaScript と C# SDK は組み込みの `AudioClient` を提供し、単一の呼び出しで完全な文字起こしパイプラインを処理 |
| <strong>言語設定</strong> | 文字起こし前に必ず AudioClient の言語（例："en"）を設定すること — 設定しないと自動検出で乱れた結果になる可能性がある |
| **Python** | モデル管理に `foundry-local-sdk`、推論に `onnxruntime` と `transformers`、音声処理に `librosa` を使用し直接 ONNX 推論を実行 |
| **JavaScript** | `foundry-local-sdk` を用い、`model.createAudioClient()` で AudioClient を生成し、`settings.language` を設定してから `transcribe()` を呼ぶ |
| **C#** | `Microsoft.AI.Foundry.Local` で `model.GetAudioClientAsync()` を呼び AudioClient を取得し、`Settings.Language` を設定してから `TranscribeAudioAsync()` を呼ぶ |
| <strong>ストリーミング対応</strong> | JS と C# SDK はチャンク単位の出力用に `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` を提供 |
| **CPU最適化** | CPU 版は (3.05 GB) でどの Windows デバイスでも動作可能（GPU不要） |
| <strong>プライバシーファースト</strong> | Zava の顧客とのやり取りや独自製品データをデバイス上に保持するのに最適 |

---

## リソース

| リソース | リンク |
|----------|------|
| Foundry Local ドキュメント | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK リファレンス | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper モデル | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local ウェブサイト | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 次のステップ

[Part 10: Using Custom or Hugging Face Models](part10-custom-models.md) に進み、Hugging Face から独自モデルをコンパイルし Foundry Local で実行する方法を学びましょう。