![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 8: Foundry Localによる評価主導開発

> **目標:** 同じローカルモデルをテスト対象エージェントと審査役の両方に使用して、AIエージェントを体系的にテスト・評価するフレームワークを構築し、出荷前にプロンプトを自信を持って反復できるようにすること。

## なぜ評価主導開発なのか？

AIエージェントを構築する際に、「まあ大体合っている」では不十分です。<strong>評価主導開発</strong>ではエージェントの出力をコードのように扱い、まずテストを書き、品質を測定し、スコアがしきい値を満たして初めて出荷します。

Zava Creative Writer（Part 7）では、<strong>Editorエージェント</strong>がすでに軽量な評価者として機能し、ACCEPTまたはREVISEを判断しています。このラボではそのパターンを形式化し、任意のエージェントやパイプラインに適用可能な再現可能な評価フレームワークにします。

| 問題 | 解決策 |
|---------|----------|
| プロンプト変更で品質が知らずに壊れる | <strong>ゴールデンデータセット</strong>でリグレッションを検知 |
| 「一つの例で動く」バイアス | <strong>複数のテストケース</strong>でエッジケースを検出 |
| 主観的な品質評価 | <strong>ルールベース＋LLMによる審査スコア</strong>で数値化 |
| プロンプトバリエーションの比較ができない | 集計スコア付きの<strong>並列評価実行</strong> |

---

## キーコンセプト

### 1. ゴールデンデータセット

<strong>ゴールデンデータセット</strong>は、期待される出力が既知の厳選されたテストケースの集合です。各テストケースには以下が含まれます：

- <strong>入力</strong>：エージェントに送るプロンプトや質問
- <strong>期待出力</strong>：正しい、もしくは高品質な応答に含まれるべき内容（キーワード、構造、事実）
- <strong>カテゴリ</strong>：報告用のグループ分け（例：「事実の正確性」「トーン」「網羅性」）

### 2. ルールベースチェック

LLMを使用せず高速で決定的に検証できるチェック：

| チェック | 内容 |
|-------|--------------|
| <strong>長さの範囲</strong> | 応答が短すぎる（手抜き）や長すぎる（冗長）ではないか |
| <strong>必須キーワード</strong> | 期待される用語やエンティティを含むか |
| <strong>フォーマット検証</strong> | JSONが解析可能か、Markdownの見出しがあるか |
| <strong>禁止コンテンツ</strong> | 幻想的ブランド名や競合他社の言及がないか |

### 3. LLMによる審査

<strong>同じローカルモデル</strong>を使って自身の出力や異なるプロンプトバリエーションの出力を評価します。審査役には以下を渡します：

- 元の質問
- エージェントの応答
- 評価基準

そして構造化されたスコアを返します。これはPart 7のEditorパターンに似ていますが、テストスイート全体に体系的に適用されます。

### 4. 評価主導のイテレーションループ

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## 前提条件

| 要件 | 詳細 |
|-------------|---------|
| **Foundry Local CLI** | モデルをダウンロード済みでインストールされている |
| <strong>言語実行環境</strong> | **Python 3.9+**、および/または **Node.js 18+**、および/または **.NET 9+ SDK** |
| <strong>完了済み</strong> | [Part 5: Single Agents](part5-single-agents.md) と [Part 6: Multi-Agent Workflows](part6-multi-agent-workflows.md) |

---

## ラボ演習

### 演習 1 - 評価フレームワークを実行する

ワークショップにはFoundry LocalエージェントをZava DIYに関するゴールデンデータセットに対してテストする完全な評価サンプルが含まれています。

<details>
<summary><strong>🐍 Python</strong></summary>

**セットアップ:**
```bash
cd python
python -m venv venv

# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS：
source venv/bin/activate

pip install -r requirements.txt
```

**実行:**
```bash
python foundry-local-eval.py
```

**何が起こるか:**
1. Foundry Localに接続しモデルをロードする
2. 5つのテストケース（Zava製品質問）からなるゴールデンデータセットを定義する
3. 各テストケースに対して2つのプロンプトバリエーションを実行する
4. <strong>ルールベースチェック</strong>（長さ、キーワード、フォーマット）で各応答をスコアリングする
5. **LLMによる審査**（同じモデルが品質を1-5で評価）によりスコアリングする
6. 両プロンプトバリエーションのスコアカードを表示する

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**セットアップ:**
```bash
cd javascript
npm install
```

**実行:**
```bash
node foundry-local-eval.mjs
```

**同様の評価パイプライン:** ゴールデンデータセット、2つのプロンプト実行、ルールベース＋LLMスコアリング、スコアカード。

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**セットアップ:**
```bash
cd csharp
dotnet restore
```

**実行:**
```bash
dotnet run eval
```

**同様の評価パイプライン:** ゴールデンデータセット、2つのプロンプト実行、ルールベース＋LLMスコアリング、スコアカード。

</details>

---

### 演習 2 - ゴールデンデータセットを理解する

評価サンプルで定義されたテストケースを調べてみましょう。各テストケースには：

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**検討すべき質問:**
1. なぜ期待値は全文ではなく<strong>キーワード</strong>なのか？
2. 信頼できる評価にはいくつのテストケースが必要か？
3. 自分のアプリケーションに入れたいカテゴリは何か？

---

### 演習 3 - ルールベースチェックとLLM審査スコアの理解

評価フレームワークは<strong>2層のスコアリング</strong>を使います：

#### ルールベースチェック（高速・決定的）

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLMによる審査（ニュアンスあり・定性的）

同じローカルモデルが構造化されたルーブリックで審査役を務めます：

```
Rate this response on a scale of 1-5:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved
- 4: Good response with minor issues
- 5: Excellent, comprehensive, well-structured

Score: 4
Reasoning: The response correctly identifies all necessary tools
and provides practical advice, but could include safety equipment.
```

**検討すべき質問:**
1. いつルールベースチェックをLLM審査より信用するか？
2. モデルは自身の出力を信頼して審査できるか？限界は何か？
3. Part 7のEditorエージェントパターンと比べてどうか？

---

### 演習 4 - プロンプトバリエーションの比較

サンプルでは<strong>2つのプロンプトバリエーション</strong>を同じテストケースに対して実行します：

| バリエーション | システムプロンプトのスタイル |
|---------|-------------------|
| **Baseline** | 汎用：「あなたは役立つアシスタントです」 |
| **Specialised** | 詳細：「あなたはZava DIYの専門家で、特定の製品を推奨し段階的に案内します」 |

実行後、以下のようなスコアカードが表示されます：

```
╔══════════════════════════════════════════════════════════════╗
║                    EVALUATION SCORECARD                      ║
╠══════════════════════════════════════════════════════════════╣
║ Prompt Variant    │ Rule Score │ LLM Score │ Combined       ║
╠═══════════════════╪════════════╪═══════════╪════════════════╣
║ Baseline          │ 0.62       │ 3.2 / 5   │ 0.62           ║
║ Specialised       │ 0.81       │ 4.1 / 5   │ 0.81           ║
╚══════════════════════════════════════════════════════════════╝
```

**演習:**
1. 評価を実行し、各バリエーションのスコアを確認する
2. 専門的プロンプトをさらに具体的に改良してみる。スコアは改善するか？
3. 3つ目のプロンプトバリエーションを追加して全て比較する
4. モデルのエイリアス（例：`phi-4-mini` vs `phi-3.5-mini`）を変えて比較する

---

### 演習 5 - 自分のエージェントに評価を適用する

評価フレームワークを自分のエージェント用のテンプレートとして使います：

1. <strong>ゴールデンデータセットを定義</strong>：期待キーワード付きのテストケースを5〜10件書く
2. <strong>システムプロンプトを書く</strong>：テストしたいエージェント指示を書く
3. <strong>評価を実行</strong>：ベースラインスコアを取得する
4. <strong>イテレーション</strong>：プロンプトを調整し、再実行して比較する
5. <strong>しきい値設定</strong>：例として「合計スコア0.75未満なら出荷しない」など

---

### 演習 6 - Zava Editorパターンとの接続

Zava Creative WriterのEditorエージェント（`zava-creative-writer-local/src/api/agents/editor/editor.py`）を振り返ってみましょう：

```python
# エディターは本番環境でLLMを審判として使用しています：
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

これはPart 8のLLM審査と<strong>同じコンセプト</strong>ですが、オフラインのテストスイートでなく本番パイプラインに組み込まれています。両者とも：

- モデルから構造化されたJSON出力を使用
- システムプロンプトに定義された品質基準を適用
- 合否判定を行う

**違い：** Editorは本番（全リクエスト）で動作し、評価フレームワークは開発中（出荷前）に動作します。

---

## 重要ポイントまとめ

| コンセプト | 要点 |
|---------|----------|
| <strong>ゴールデンデータセット</strong> | 初期にテストケースを厳選しAIのリグレッションテストに使う |
| <strong>ルールベースチェック</strong> | 高速・決定的で明らかな失敗（長さ、キーワード、フォーマット）を検知 |
| **LLMによる審査** | 同じローカルモデルで複雑な品質評価を行う |
| <strong>プロンプト比較</strong> | 同じテストスイートに複数バリエーションを走らせて最良を選択 |
| <strong>デバイス内の利点</strong> | すべての評価がローカルで実行されAPIコストなし、制限なし、データ流出なし |
| <strong>出荷前の評価</strong> | 品質しきい値を設定し評価スコアでリリースを制御 |

---

## 次のステップ

- <strong>スケールアップ</strong>：ゴールデンデータセットにテストケースとカテゴリを追加
- <strong>自動化</strong>：評価をCI/CDパイプラインに統合
- <strong>高度な審査役</strong>：より大きいモデルを審査役に使い小さいモデルの出力を評価
- <strong>経時追跡</strong>：評価結果を保存しプロンプトやモデルのバージョンごとに比較

---

## 次のラボ

[Part 9: Whisperでの音声文字起こし](part9-whisper-voice-transcription.md)に進み、Foundry Local SDKを使ったオンデバイスの音声認識を体験しましょう。