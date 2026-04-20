![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第 8 部分：使用 Foundry Local 的評價主導開發

> **目標：** 建立一個評價框架，系統地測試和評分你的 AI 代理，使用相同的本地模型作為受測代理和評審，讓你在發佈前能自信地迭代提示詞。

## 為什麼要採用評價主導開發？

在建構 AI 代理時，「看起來差不多」是不夠的。<strong>評價主導開發</strong>將代理的輸出視同程式碼：先寫測試、量度質量，只有當分數達標才發佈。

在 Zava 創意寫手（第 7 部分）中，<strong>編輯代理</strong>已經作為輕量評審；決定接受或修改。本實驗室將此模式正式化為可重複應用於任何代理或流程的評價框架。

| 問題 | 解決方案 |
|---------|----------|
| 提示詞更改悄悄破壞質量 | <strong>黃金數據集</strong> 捕捉退化 |
| 「只在一個例子有效」偏誤 | <strong>多個測試案例</strong> 揭露邊緣情況 |
| 主觀質量評估 | **規則基與 LLM 擔任評審** 提供數據 |
| 無法比較提示詞版本 | <strong>並排評價對比執行</strong> 及綜合打分 |

---

## 主要概念

### 1. 黃金數據集

<strong>黃金數據集</strong>是經精心策劃且帶有已知預期輸出的測試案例集合。每個測試案例包括：

- <strong>輸入</strong>：發送給代理的提示或問題
- <strong>預期輸出</strong>：正確或高質量回覆應包含的內容（關鍵字、結構、事實）
- <strong>分類</strong>：用於報告的分組（例如「事實準確性」、「語氣」、「完整性」）

### 2. 規則基檢查

快速且確定性的檢查，不依賴 LLM：

| 檢查項目 | 測試內容 |
|-------|--------------|
| <strong>長度範圍</strong> | 回覆不過短（懶散）或過長（囉嗦） |
| <strong>必須出現關鍵字</strong> | 回覆提及預期詞彙或實體 |
| <strong>格式驗證</strong> | JSON 可解析、Markdown 有標題 |
| <strong>禁用內容</strong> | 禁止虛構品牌名、禁提競爭對手 |

### 3. LLM 作為評審

使用<strong>相同本地模型</strong>對自己的輸出（或不同提示詞版本的輸出）進行評分。評審會收到：

- 原始問題
- 代理回覆
- 評分標準

回傳結構化分數。此模式與第 7 部分的編輯代理相似，但系統性套用於整個測試套件。

### 4. 評價驅動迭代循環

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## 前置條件

| 要求 | 詳細資訊 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝且已下載模型 |
| <strong>語言執行環境</strong> | **Python 3.9+** 和/或 **Node.js 18+** 和/或 **.NET 9+ SDK** |
| <strong>完成項目</strong> | [第 5 部分：單一代理](part5-single-agents.md) 與 [第 6 部分：多代理工作流](part6-multi-agent-workflows.md) |

---

## 實驗練習

### 練習 1 - 執行評價框架

本工作坊包含完整評價範例，測試 Foundry Local 代理對 Zava DIY 相關問題的黃金數據集。

<details>
<summary><strong>🐍 Python</strong></summary>

**設定：**
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
python foundry-local-eval.py
```

**過程：**
1. 連接至 Foundry Local 並載入模型
2. 定義包含 5 個測試案例（Zava 產品問題）的黃金數據集
3. 對每個測試案例執行兩個提示詞版本
4. 使用<strong>規則基檢查</strong>評分回覆（長度、關鍵字、格式）
5. 使用<strong>LLM 作為評審</strong>評分回覆（同一模型以 1-5 分評質量）
6. 印出比較兩個提示詞版本的成績單

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**設定：**
```bash
cd javascript
npm install
```

**執行：**
```bash
node foundry-local-eval.mjs
```

**相同評價流程：** 黃金數據集、雙提示詞測試、規則基 + LLM 評分、成績單。

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**設定：**
```bash
cd csharp
dotnet restore
```

**執行：**
```bash
dotnet run eval
```

**相同評價流程：** 黃金數據集、雙提示詞測試、規則基 + LLM 評分、成績單。

</details>

---

### 練習 2 - 理解黃金數據集

檢視評價範例中定義的測試案例。每個案例包含：

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**思考問題：**
1. 為何預期值是<strong>關鍵字</strong>而非完整句子？
2. 需要多少案例來達成可靠評價？
3. 你會為自己的應用加入什麼分類？

---

### 練習 3 - 理解規則基與 LLM 作為評審的評分

評價框架使用<strong>兩層評分</strong>：

#### 規則基檢查（快速且確定性）

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM 作為評審（細緻且具質感）

同一本地模型以結構化標準作評審：

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

**思考問題：**
1. 何時會更信任規則基檢查勝過 LLM 評審？
2. 模型能否可靠自評輸出？限制在哪？
3. 與第7部分的編輯代理模式比較，差異為何？

---

### 練習 4 - 比較提示詞版本

範例以<strong>兩個提示詞版本</strong>測試相同案例：

| 版本 | 系統提示風格 |
|---------|-------------------|
| <strong>基線</strong> | 通用：「你是一名樂於助人的助理」 |
| <strong>專業</strong> | 詳盡：「你是 Zava DIY 專家，推薦指定產品並提供分步指導」 |

執行後會看到類似的成績單：

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

**練習：**
1. 執行評價，記錄每個版本得分
2. 修改專業提示詞更加具體，分數是否提升？
3. 增加第三個提示詞版本，比較三者結果
4. 嘗試更換模型別名（如 `phi-4-mini` 與 `phi-3.5-mini`）並比較結果

---

### 練習 5 - 將評價應用於自有代理

以評價框架作為模板，應用到你自己的代理：

1. <strong>定義黃金數據集</strong>：撰寫 5 至 10 個含預期關鍵字的測試案例。
2. <strong>編寫系統提示詞</strong>：設定你要測試的代理指令。
3. <strong>執行評價</strong>：取得基線分數。
4. <strong>迭代</strong>：調整提示詞，重跑並比較。
5. <strong>設定門檻</strong>：如「綜合分低於 0.75 不得發佈」。

---

### 練習 6 - 與 Zava 編輯者模式的連結

回頭看 Zava 創意寫手的編輯代理（`zava-creative-writer-local/src/api/agents/editor/editor.py`）：

```python
# 編輯器是在生產環境中作為大型語言模型評審者的角色：
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

這與第 8 部分的 LLM 作為評審<strong>同一概念</strong>，但嵌入生產流程而非離線測試套件。兩者模式皆：

- 使用模型的結構化 JSON 輸出。
- 按系統提示定義質量標準。
- 做出通過/不通過決定。

**差異：** 編輯者在生產中執行（每個請求），評價框架於開發期間執行（發佈前）。

---

## 重要結論

| 概念 | 重點 |
|---------|----------|
| <strong>黃金數據集</strong> | 早期策劃測試案例；它們是 AI 回歸測試的依據 |
| <strong>規則基檢查</strong> | 快速、確定且能捕捉明顯失誤（長度、關鍵字、格式） |
| **LLM 作為評審** | 利用相同本地模型進行細緻質量評分 |
| <strong>提示詞比較</strong> | 對相同測試套執行多版本，挑選最佳方案 |
| <strong>本地優勢</strong> | 全部評價皆本地執行：無 API 費用、無限速、資料不外洩 |
| <strong>發佈前評價</strong> | 設定質量門檻，以評分控管發佈 |

---

## 後續步驟

- <strong>擴大規模</strong>：新增更多測試案例與分類至黃金數據集。
- <strong>自動化</strong>：將評價整合進 CI/CD 流程。
- <strong>高階評審</strong>：測試較小模型輸出時，使用更大模型擔任評審。
- <strong>長期追蹤</strong>：保存評價結果以比較提示詞與模型版本演進。

---

## 下一堂實驗室

繼續前往 [第 9 部分：使用 Whisper 進行語音轉錄](part9-whisper-voice-transcription.md)，探索使用 Foundry Local SDK 於裝置端進行語音轉文字。