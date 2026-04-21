![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# パート10：Foundry LocalでカスタムモデルまたはHugging Faceモデルを使用する

> **目標:** Hugging FaceモデルをFoundry Localが必要とする最適化済みONNX形式にコンパイルし、チャットテンプレートを設定し、ローカルキャッシュに追加して、CLI、REST API、OpenAI SDKを使って推論を実行するまでの手順を学びます。

## 概要

Foundry Localは厳選された事前コンパイル済みモデルのカタログを搭載していますが、そのリストに限定されるわけではありません。[Hugging Face](https://huggingface.co/)で利用可能なトランスフォーマーベースの言語モデル（あるいはPyTorch/Safetensors形式でローカルに保存されているもの）なら、最適化されたONNXモデルにコンパイルしてFoundry Local経由で提供可能です。

このコンパイルパイプラインは、`onnxruntime-genai`パッケージに含まれるコマンドラインツールである<strong>ONNX Runtime GenAI Model Builder</strong>を使用します。モデルビルダーは重い処理を担当し、ソースの重みをダウンロード、ONNX形式に変換、量子化（int4、fp16、bf16）を適用し、Foundry Localが期待する設定ファイル（チャットテンプレートやトークナイザーを含む）を生成します。

このラボでは、Hugging Faceの<strong>Qwen/Qwen3-0.6B</strong>をコンパイルし、Foundry Localに登録し、デバイス上で完全にチャットできるようにします。

---

## 学習目標

ラボの終了時には以下ができるようになります：

- カスタムモデルのコンパイルがなぜ役立つのか、いつ必要になるか説明できる
- ONNX Runtime GenAIモデルビルダーをインストールできる
- 単一コマンドでHugging Faceモデルを最適化されたONNX形式にコンパイルできる
- 主なコンパイルパラメーター（実行プロバイダー、精度）を理解できる
- `inference_model.json` というチャットテンプレート設定ファイルを作成できる
- コンパイル済みモデルをFoundry Localのキャッシュに追加できる
- CLI、REST API、OpenAI SDKを使ってカスタムモデルに対して推論実行できる

---

## 前提条件

| 要件 | 詳細 |
|-------------|---------|
| **Foundry Local CLI** | インストール済みかつ`PATH`にあること（[パート1](part1-getting-started.md)参照） |
| **Python 3.10以上** | ONNX Runtime GenAIモデルビルダーが要求する |
| **pip** | Pythonパッケージマネージャー |
| <strong>ディスク空き容量</strong> | ソースとコンパイル済みモデルファイル用に少なくとも5GBの空き |
| **Hugging Faceアカウント** | 一部モデルはダウンロード前にライセンス同意が必要。Qwen3-0.6BはApache 2.0ライセンスで自由に利用可能。 |

---

## 環境セットアップ

モデルコンパイルには複数の大きなPythonパッケージ（PyTorch、ONNX Runtime GenAI、Transformers）が必要です。システムPythonや他のプロジェクトに干渉しない専用の仮想環境を作成してください。

```bash
# リポジトリのルートから
python -m venv .venv
```

環境を有効化：

**Windows（PowerShell）:**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

依存関係の解決問題を避けるためにpipをアップグレード：

```bash
python -m pip install --upgrade pip
```

> **ヒント:** 既に前のラボから `.venv` がある場合は再利用可能です。続行前に必ず有効化してください。

---

## コンセプト：コンパイルパイプライン

Foundry LocalはONNX形式かつONNX Runtime GenAI設定のモデルを必要とします。Hugging Faceの多くのオープンソースモデルはPyTorchまたはSafetensorsの重みで配布されているため、変換ステップが必要です。

![カスタムモデルのコンパイルパイプライン](../../../images/custom-model-pipeline.svg)

### モデルビルダーが行うこと

1. Hugging Faceからソースモデルを<strong>ダウンロード</strong>（またはローカルパスから読み込み）します。
2. PyTorch / Safetensors重みをONNX形式に<strong>変換</strong>します。
3. メモリ使用量削減とスループット向上のためにモデルを小さい精度（例：int4）に<strong>量子化</strong>します。
4. Foundry Localがロード・提供できるように、ONNX Runtime GenAI設定（`genai_config.json`）、チャットテンプレート（`chat_template.jinja`）、およびすべてのトークナイザーファイルを<strong>出力</strong>します。

### ONNX Runtime GenAI Model Builder と Microsoft Oliveの比較

モデル最適化の代替ツールとして<strong>Microsoft Olive</strong>に言及することがあります。どちらもONNXモデルを生成可能ですが、それぞれ目的とトレードオフが異なります：

|  | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| <strong>パッケージ名</strong> | `onnxruntime-genai` | `olive-ai` |
| <strong>主な目的</strong> | ONNX Runtime GenAI推論用に生成AIモデルを変換・量子化 | 多数のバックエンドとハードウェアターゲットに対応したエンドツーエンドのモデル最適化フレームワーク |
| <strong>使いやすさ</strong> | 単一コマンドでワンステップ変換＋量子化 | ワークフロー形式（YAML/JSONで構成可能な多段パイプライン） |
| <strong>出力形式</strong> | ONNX Runtime GenAI形式（Foundry Local対応済み） | ジェネリックONNX、ONNX Runtime GenAI、その他ワークフローに依存 |
| <strong>対応ハードウェア</strong> | CPU、CUDA、DirectML、TensorRT RTX、WebGPU | CPU、CUDA、DirectML、TensorRT、Qualcomm QNNなど多数 |
| <strong>量子化オプション</strong> | int4、int8、fp16、fp32 | int4（AWQ、GPTQ、RTN）、int8、fp16、グラフ最適化、レイヤー調整など |
| <strong>対応モデル範囲</strong> | 生成AIモデル（LLM、SLM） | 任意のONNX変換可能モデル（ビジョン、NLP、オーディオ、多モーダル） |
| <strong>適した用途</strong> | ローカル推論向け単一モデルの迅速なコンパイル | 微調整可能な最適化を要する本番パイプライン |
| <strong>依存関係の重さ</strong> | 中程度（PyTorch、Transformers、ONNX Runtime） | より大きい（Oliveフレームワーク、ワークフロー毎に追加オプション） |
| **Foundry Local連携** | 直接連携 — 出力はすぐに互換可能 | `--use_ort_genai` フラグと追加設定が必要 |

> **なぜ本ラボはModel Builderを使用するのか:** 単一のHugging FaceモデルをコンパイルしてFoundry Localに登録する場合、Model Builderが最も簡単で確実な方法です。1コマンドでFoundry Localが期待する正確な出力フォーマットを生成します。より高度な最適化機能（精度認識量子化、グラフ処理、多段調整）が必要な場合はOliveが強力な選択肢です。詳細は[Microsoft Oliveドキュメント](https://microsoft.github.io/Olive/)を参照してください。

---

## ラボ演習

### 演習1：ONNX Runtime GenAI Model Builderをインストールする

モデルビルダーを含むONNX Runtime GenAIパッケージをインストールします：

```bash
pip install onnxruntime-genai
```

モデルビルダーが利用可能か確認します：

```bash
python -m onnxruntime_genai.models.builder --help
```

`-m`（モデル名）、`-o`（出力パス）、`-p`（精度）、`-e`（実行プロバイダー）などのパラメーターを含むヘルプ出力が表示されるはずです。

> **注意:** モデルビルダーはPyTorch、Transformersなど複数のパッケージに依存します。インストールには数分かかる場合があります。

---

### 演習2：Qwen3-0.6BをCPU向けにコンパイルする

以下のコマンドでHugging FaceからQwen3-0.6Bをダウンロードし、CPU推論用にint4量子化でコンパイルします：

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows（PowerShell）:**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### パラメーターの説明

| パラメーター | 目的 | 使用値 |
|-----------|---------|------------|
| `-m` | Hugging FaceのモデルIDまたはローカルディレクトリパス | `Qwen/Qwen3-0.6B` |
| `-o` | コンパイル済みのONNXモデルを保存するディレクトリ | `models/qwen3` |
| `-p` | コンパイル時に適用する量子化の精度 | `int4` |
| `-e` | ONNX Runtimeの実行プロバイダー（ターゲットハードウェア） | `cpu` |
| `--extra_options hf_token=false` | Hugging Face認証をスキップ（公開モデル向け） | `hf_token=false` |

> **所要時間はどのくらい？** コンパイル時間はハードウェアとモデルサイズによります。Qwen3-0.6Bをint4量子化でモダンCPU上でコンパイルすると、約5〜15分程度です。モデルが大きくなるほど長時間かかります。

コマンド完了後、`models/qwen3`ディレクトリにコンパイル済みモデルファイルが生成されているはずです。出力内容を確認：

```bash
ls models/qwen3
```

以下のファイルが存在します：
- `model.onnx` と `model.onnx.data` — コンパイル済みモデルの重み
- `genai_config.json` — ONNX Runtime GenAI設定ファイル
- `chat_template.jinja` — モデルのチャットテンプレート（自動生成）
- `tokenizer.json`、`tokenizer_config.json` — トークナイザーファイル
- その他の語彙ファイルや設定ファイル

---

### 演習3：GPU向けにコンパイルする（任意）

NVIDIA GPUでCUDA対応している場合は、高速推論のためにGPU最適化版をコンパイル可能です：

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **注意:** GPUコンパイルには`onnxruntime-gpu`とCUDAが正常にインストールされている必要があります。不足するとモデルビルダーがエラー報告します。この演習はスキップしてCPU版で続行可能です。

#### ハードウェア固有のコンパイル参照

| ターゲット | 実行プロバイダー (`-e`) | 推奨精度 (`-p`) |
|--------|---------------------------|------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` または `int4` |
| DirectML（Windows GPU） | `dml` | `fp16` または `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### 精度のトレードオフ

| 精度 | サイズ | 速度 | 精度（品質） |
|-----------|------|-------|---------|
| `fp32` | 最大 | 最も遅い | 最高精度 |
| `fp16` | 大きい | 高速（GPU向け） | 非常に良好 |
| `int8` | 小さい | 高速 | 若干の精度低下 |
| `int4` | 最小 | 最速 | 中程度の精度低下 |

ほとんどのローカル開発では、CPU上での`int4`が速度とリソース利用のバランスが最適です。本番クオリティの出力が必要ならCUDA GPU上の`fp16`が推奨されます。

---

### 演習4：チャットテンプレート設定を作成する

モデルビルダーは出力ディレクトリに`chat_template.jinja`と`genai_config.json`ファイルを自動生成します。ただし、Foundry Localはプロンプトをどう整形するか理解するために`inference_model.json`ファイルも必要です。このファイルにはモデル名とユーザーメッセージを正しい特殊トークンで包むプロンプトテンプレートが定義されています。

#### ステップ1：コンパイル済み成果物を確認する

コンパイル済みモデルディレクトリの内容を一覧表示：

```bash
ls models/qwen3
```

以下のファイルが確認できます：
- `model.onnx` と `model.onnx.data` — コンパイル済みモデルの重み
- `genai_config.json` — ONNX Runtime GenAI設定（自動生成）
- `chat_template.jinja` — モデルのチャットテンプレート（自動生成）
- `tokenizer.json`、`tokenizer_config.json` — トークナイザーファイル
- その他の設定や語彙ファイル

#### ステップ2：inference_model.jsonファイルを生成する

`models/`フォルダーと同じルートディレクトリに、`generate_chat_template.py`というPythonスクリプトを作成します：

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# チャットテンプレートを抽出するための最小限の会話を構築する
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# inference_model.json構造を構築する
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

リポジトリルートからスクリプトを実行：

```bash
python generate_chat_template.py
```

> **注意:** `transformers`パッケージは`onnxruntime-genai`の依存関係として既にインストールされています。`ImportError`が出た場合は`pip install transformers`を実行してください。

スクリプトは`models/qwen3`ディレクトリ内に`inference_model.json`ファイルを生成します。このファイルはFoundry LocalにQwen3のユーザー入力を正しい特殊トークンで包む方法を伝えます。

> **重要:** `inference_model.json`の `"Name"` フィールド（スクリプトでは `qwen3-0.6b` に設定）は、以後のすべてのコマンドやAPI呼び出しで使用する<strong>モデルの別名</strong>です。もし変更する場合は演習6〜10でモデル名を更新してください。

#### ステップ3：設定を確認する

`models/qwen3/inference_model.json`を開き、`Name`フィールドと、`assistant` と `prompt` キーを持つ `PromptTemplate` オブジェクトが含まれていることを確認します。プロンプトテンプレートには `<|im_start|>` や `<|im_end|>` のような特殊トークンが含まれているはずです（正確なトークンはモデルのチャットテンプレートに依存します）。

> **手動代替:** スクリプトを実行したくない場合は手動でファイル作成可能です。重要なのは`prompt`フィールドにモデルの完全なチャットテンプレートを `{Content}` プレースホルダー付きで含めることです。

---

### 演習5：モデルディレクトリ構造を確認する
モデルビルダーは、すべてのコンパイル済みファイルを指定した出力ディレクトリに直接配置します。最終的な構造が正しいか確認してください：

```bash
ls models/qwen3
```

ディレクトリには、以下のファイルが含まれているはずです：

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **Note:** 他のいくつかのコンパイルツールとは異なり、モデルビルダーはネストされたサブディレクトリを作成しません。すべてのファイルは出力フォルダーに直接配置されており、これはFoundry Localが期待する形と完全に一致します。

---

### 演習 6: モデルをFoundry Localキャッシュに追加する

コンパイル済みモデルのディレクトリをFoundry Localのキャッシュに追加して、モデルの場所を指定します：

```bash
foundry cache cd models/qwen3
```

モデルがキャッシュに表示されていることを確認してください：

```bash
foundry cache ls
```

カスタムモデルが、以前にキャッシュされたモデル（`phi-3.5-mini` や `phi-4-mini` など）と並んで一覧に表示されているはずです。

---

### 演習 7: カスタムモデルをCLIで実行する

新たにコンパイルしたモデル（`qwen3-0.6b` は `inference_model.json` の `Name` フィールドからのエイリアスです）で対話型チャットセッションを開始します：

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` フラグは追加の診断情報を表示します。カスタムモデルを初めてテストするときに役立ちます。モデルが正常にロードされると対話型プロンプトが表示されます。いくつかメッセージを試してください：

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

セッションを終了するには、`exit` と入力するか、`Ctrl+C` を押します。

> **Troubleshooting:** モデルのロードに失敗した場合は、以下を確認してください：
> - `genai_config.json` ファイルがモデルビルダーによって生成されていること。
> - `inference_model.json` ファイルが存在し、有効なJSONであること。
> - ONNXモデルファイルが正しいディレクトリにあること。
> - 十分な空きRAMがあること（Qwen3-0.6B int4 は約1GBが必要です）。
> - Qwen3は`<think>`タグを生成する推論モデルです。もし応答の先頭に `<think>...</think>` が付いていたら、それは正常な動作です。`inference_model.json` のプロンプトテンプレートを調整して思考出力を抑制できます。

---

### 演習 8: REST API経由でカスタムモデルにクエリを送る

演習 7 の対話型セッションを終了した場合、モデルがロードされていない可能性があります。まずFoundry Localサービスを起動し、モデルをロードしてください：

```bash
foundry service start
foundry model load qwen3-0.6b
```

サービスがどのポートで動作しているか確認します：

```bash
foundry service status
```

次にリクエストを送信します（ポート番号`5273`は異なる場合は実際のポートで置き換えてください）：

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows note:** 上記の`curl`コマンドはbashの構文です。WindowsではPowerShellの`Invoke-RestMethod`コマンドレットを使用してください。

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### 演習 9: OpenAI SDKでカスタムモデルを使う

内蔵モデルで使ったOpenAI SDKのコードと全く同じコードでカスタムモデルに接続できます（詳細は[パート3](part3-sdk-and-apis.md)を参照）。違いはモデル名だけです。

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry LocalはAPIキーを検証しません
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local は API キーを検証しません
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Key point:** Foundry LocalはOpenAI互換のAPIを公開しているので、内蔵モデルに使えるコードはそのままカスタムモデルにも使えます。`model` パラメータだけ変更してください。

---

### 演習 10: Foundry Local SDKでカスタムモデルをテストする

これまでのラボでは、Foundry Local SDKでサービス起動、エンドポイント検出、モデル管理を自動で行いました。カスタムコンパイル済みモデルでも同様にできます。SDKはサービス起動とエンドポイント検出を処理するので、コードに `localhost:5273` などのポート番号をハードコードする必要はありません。

> **Note:** 以下の例を実行する前に必ずFoundry Local SDKをインストールしてください：
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** NuGetパッケージ`Microsoft.AI.Foundry.Local` と `OpenAI` を追加
>
> それぞれのスクリプトファイルは <strong>リポジトリのルート</strong>（`models/`フォルダと同じディレクトリ）に保存してください。

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# ステップ 1: Foundry Local サービスを開始し、カスタムモデルを読み込む
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# ステップ 2: カスタムモデルのキャッシュを確認する
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# ステップ 3: モデルをメモリに読み込む
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# ステップ 4: SDKで検出されたエンドポイントを使用してOpenAIクライアントを作成する
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# ステップ 5: ストリーミングチャット補完リクエストを送信する
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

実行：

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// ステップ1: Foundry Localサービスを開始する
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ステップ2: カタログからカスタムモデルを取得する
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// ステップ3: モデルをメモリにロードする
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// ステップ4: SDKで検出されたエンドポイントを使用してOpenAIクライアントを作成する
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// ステップ5: ストリーミングチャット補完リクエストを送信する
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

実行：

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **Key point:** Foundry Local SDKはエンドポイントを動的に検出するため、ポート番号をハードコードしません。これは運用環境で推奨される方法です。カスタムモデルも内蔵カタログモデルと同様にSDK経由で動作します。

---

## コンパイルするモデルの選択

このラボではQwen3-0.6Bを例として使っています。小さくコンパイルが速い上にApache 2.0ライセンスで自由に使えるためです。ただし他にも多くのモデルがあります。以下は例です：

| モデル | Hugging Face ID | パラメーター数 | ライセンス | 備考 |
|-------|-----------------|---------------|------------|------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | 非常に小さく、コンパイルが速い。テストに最適。 |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | 品質が良く、まだコンパイルは速い。 |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | より高品質だがRAMが多く必要。 |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Faceでライセンス承諾が必要。 |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | 高品質だがダウンロードとコンパイルに時間がかかる。 |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Foundry Localカタログに既にある（比較に便利）。 |

> **ライセンス注意:** 使用前に必ずHugging Face上でモデルのライセンスを確認してください。Llamaなど一部モデルはライセンス同意と`huggingface-cli login`による認証が必要です。

---

## コンセプト: カスタムモデルを使うタイミング

| シナリオ | なぜ自分でコンパイルするか |
|----------|------------------------------|
| <strong>カタログに欲しいモデルがない場合</strong> | Foundry Localカタログは厳選済みです。欲しいモデルがなければ自分でコンパイルしましょう。 |
| <strong>ファインチューニング済みモデル</strong> | ドメイン固有のファインチューニングを行った場合は、自分で重みをコンパイルする必要があります。 |
| <strong>特定の量子化要件</strong> | カタログのデフォルトとは異なる精度・量子化戦略を使いたい場合。 |
| <strong>新しいモデルリリース</strong> | Hugging Faceで新しいモデルが出ても、Foundry Localカタログにすぐ反映されないことがあります。自分でコンパイルすればすぐ使えます。 |
| <strong>研究・実験</strong> | 本番選定前にローカルで異なるモデル構成やサイズを試す場合。 |

---

## まとめ

このラボで学んだ内容：

| 手順 | 内容 |
|------|-------|
| 1 | ONNX Runtime GenAIモデルビルダーをインストール |
| 2 | Hugging Faceの `Qwen/Qwen3-0.6B` を最適化ONNXモデルにコンパイル |
| 3 | `inference_model.json` のチャットテンプレート設定ファイルを作成 |
| 4 | コンパイル済みモデルをFoundry Localキャッシュに追加 |
| 5 | CLI経由でカスタムモデルとの対話型チャットを実行 |
| 6 | OpenAI互換REST APIからモデルへクエリ送信 |
| 7 | Python、JavaScript、C#からOpenAI SDKで接続 |
| 8 | Foundry Local SDKでエンドツーエンドテスト |

重要なのは、<strong>トランスフォーマーベースのモデルはONNX形式にコンパイルされればFoundry Local上で動作する</strong>ということです。OpenAI互換APIにより既存のアプリケーションコードに変更は不要、モデル名だけ差し替えればよいのです。

---

## 重要ポイント

| コンセプト | 詳細 |
|------------|-------|
| ONNX Runtime GenAIモデルビルダー | Hugging Faceモデルを単一コマンドでONNX形式に変換し量子化も行う |
| ONNX形式 | Foundry LocalはONNX Runtime GenAI設定付きONNXモデルを要求 |
| チャットテンプレート | `inference_model.json` はモデル用のプロンプト書式設定をFoundry Localに指示 |
| ハードウェアターゲット | CPU、NVIDIA GPU(CUDA)、DirectML(Windows GPU)、WebGPU用にコンパイル可能 |
| 量子化 | 精度を犠牲にしてサイズ削減・高速化（int4）、GPU性能を活かした高精度(fp16)も選択可 |
| API互換性 | カスタムモデルは内蔵モデルと同じOpenAI互換APIを使う |
| Foundry Local SDK | サービス起動、エンドポイント検出、モデル読み込みを自動化。カタログ・カスタム両対応 |

---

## 参考資料

| 資料 | リンク |
|------------|---------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Localカスタムモデルガイド | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3モデルファミリー | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Oliveドキュメンテーション | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## 次のステップ

[パート11: ローカルモデルでのツール呼び出し](part11-tool-calling.md) を続けて、ローカルモデルから外部関数を呼び出す方法を学習しましょう。

[← パート9: Whisper音声文字起こし](part9-whisper-voice-transcription.md) | [パート11: ツール呼び出し →](part11-tool-calling.md)