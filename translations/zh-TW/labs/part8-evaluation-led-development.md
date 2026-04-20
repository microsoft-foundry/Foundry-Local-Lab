![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第 8 部分：使用 Foundry Local 進行以評估為主導的開發

> **目標：** 建立一個評估框架，系統性地測試並評分您的 AI 代理，使用相同的本地模型同時作為被測代理和評審，讓您在發佈前能夠自信地迭代提示。

## 為什麼要以評估為主導的開發？

在構建 AI 代理時，「看起來還可以」是不夠的。<strong>以評估為主導的開發</strong> 將代理輸出視如程式碼：先撰寫測試，衡量品質，只有在分數達到門檻時才發佈。

在 Zava 創意寫作員（第 7 部分）中，<strong>編輯代理</strong> 已經扮演輕量級評估員的角色；決定接受或修改。此實驗室將該模式正式化為可重複套用於任何代理或流程的評估框架。

| 問題 | 解決方案 |
|---------|----------|
| 提示更改悄無聲息破壞品質 | <strong>黃金數據集</strong> 捕捉回歸 |
| 「只在一個範例有效」偏差 | <strong>多個測試案例</strong> 揭示邊緣情況 |
| 主觀的品質評估 | **基於規則 + LLM 作為評審評分** 提供數字 |
| 無法比較提示變體 | <strong>並排評估執行</strong> 並彙總分數 |

---

## 主要概念

### 1. 黃金數據集

<strong>黃金數據集</strong> 是一組精心策劃的測試案例，帶有已知的預期輸出。每個測試案例包含：

- <strong>輸入</strong>：傳送給代理的提示或問題
- <strong>預期輸出</strong>：正確或高品質回答應包含的內容（關鍵字、結構、事實）
- <strong>分類</strong>：用於報告的分組（例如「事實準確性」、「語氣」、「完整性」）

### 2. 基於規則的檢查

快速且確定性的檢查，不需要使用 LLM：

| 檢查項目 | 檢測內容 |
|-------|--------------|
| <strong>長度限制</strong> | 回答不應太短（懶惰）或太長（囉嗦） |
| <strong>必要關鍵字</strong> | 回答須提及預期的詞彙或實體 |
| <strong>格式驗證</strong> | JSON 可解析，Markdown 標題存在 |
| <strong>禁止內容</strong> | 無幻覺品牌名稱，無競爭對手提及 |

### 3. LLM 作為評審

使用<strong>相同的本地模型</strong>評分其自身輸出（或不同提示變體的輸出）。評審接收到：

- 原始問題
- 代理的回答
- 評分標準

並返回結構化分數。此模式反映第 7 部分中的編輯器模式，但系統化應用於整個測試套件。

### 4. 以評估驅動的迭代循環

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## 前置條件

| 要求 | 詳細說明 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝並下載模型 |
| <strong>語言執行環境</strong> | **Python 3.9+** 和/或 **Node.js 18+** 和/或 **.NET 9+ SDK** |
| <strong>已完成</strong> | [第 5 部分：單一代理](part5-single-agents.md) 與 [第 6 部分：多代理工作流](part6-multi-agent-workflows.md) |

---

## 實驗練習

### 練習 1 - 執行評估框架

工作坊包含一個完整評估範例，針對 Foundry Local 代理進行測試，測試對象是一組與 Zava DIY 相關問題的黃金數據集。

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

**會發生什麼：**
1. 連接到 Foundry Local 並載入模型
2. 定義包含 5 個測試案例（Zava 產品問題）的黃金數據集
3. 對每個測試案例運行兩個提示變體
4. 使用 <strong>基於規則檢查</strong> 評分每個回應（長度、關鍵字、格式）
5. 使用 **LLM 作為評審** 評分每個回應（使用相同模型評分 1-5 級品質）
6. 印出比較兩個提示變體的分數卡

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

**相同的評估流程：** 黃金數據集、雙提示執行、基於規則 + LLM 評分、分數卡。

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

**相同的評估流程：** 黃金數據集、雙提示執行、基於規則 + LLM 評分、分數卡。

</details>

---

### 練習 2 - 理解黃金數據集

檢視評估範例中定義的測試案例。每個測試案例包含：

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**思考問題：**
1. 為什麼預期值是 <strong>關鍵字</strong> 而非完整句子？
2. 可靠的評估需要多少個測試案例？
3. 您會為自己的應用新增哪些分類？

---

### 練習 3 - 理解基於規則與 LLM 作為評審的評分差異

評估框架使用 <strong>兩個評分層次</strong>：

#### 基於規則的檢查（快速、確定性）

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM 作為評審（細膩、質性）

相同的本地模型充當帶有結構化評分標準的評審：

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
1. 什麼時候會信任基於規則的檢查勝過 LLM 作為評審？
2. 模型能否可靠評判自己的輸出？有哪些限制？
3. 與第 7 部分的編輯器代理模式相比如何？

---

### 練習 4 - 比較提示變體

範例中對相同測試案例執行了 <strong>兩個提示變體</strong>：

| 變體 | 系統提示風格 |
|---------|-------------------|
| <strong>基線</strong> | 通用：「你是個樂於助人的助理」 |
| <strong>專業版</strong> | 詳細：「你是 Zava DIY 專家，推薦具體產品並提供逐步指導」 |

執行後您會看到類似的分數卡：

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
1. 執行評估並記錄每個變體的分數
2. 將專業版提示修改得更具體。分數會改善嗎？
3. 新增第三個提示變體，並比較三者。
4. 嘗試更換模型代號（例如 `phi-4-mini` 對比 `phi-3.5-mini`）並比較結果。

---

### 練習 5 - 將評估應用於您自己的代理

使用評估框架作為模板為您自己的代理服務：

1. <strong>定義您的黃金數據集</strong>：撰寫 5 至 10 個測試案例，包含預期關鍵字。
2. <strong>撰寫系統提示</strong>：定義您想測試的代理指令。
3. <strong>執行評估</strong>：取得基線分數。
4. <strong>迭代</strong>：調整提示，重新執行並比較。
5. <strong>設置門檻</strong>：例如「整體分數低於 0.75 不發佈」。

---

### 練習 6 - 與 Zava 編輯器模式的連結

回顧 Zava 創意寫作員的編輯器代理（`zava-creative-writer-local/src/api/agents/editor/editor.py`）：

```python
# 編輯器在生產環境中充當大型語言模型（LLM）作為裁判的角色：
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

這與第 8 部分的 LLM 作為評審<strong>是相同概念</strong>，但嵌入於生產流程中，而非離線測試套件。兩者模式：

- 使用模型產生結構化的 JSON 輸出。
- 套用系統提示定義的品質標準。
- 作出通過/不通過決定。

**差異在於：** 編輯器在生產中執行（每個請求）；評估框架在開發中執行（發佈前）。

---

## 主要收穫

| 概念 | 收穫 |
|---------|----------|
| <strong>黃金數據集</strong> | 早期策劃測試案例；它們是您的 AI 回歸測試 |
| <strong>基於規則檢查</strong> | 快速、確定性，捕捉明顯失敗（長度、關鍵字、格式） |
| **LLM 作為評審** | 使用相同的本地模型進行細膩品質評分 |
| <strong>提示比較</strong> | 對相同測試套件執行多種提示變體，篩選最佳 |
| <strong>本地執行優勢</strong> | 全部評估本地執行：零 API 費用、無速率限制、資料不外流 |
| <strong>發佈前評估</strong> | 設置品質門檻，根據評估分數決定是否釋出 |

---

## 下一步

- <strong>擴增規模</strong>：向黃金數據集添加更多測試案例和分類。
- <strong>自動化</strong>：將評估整合入 CI/CD 流程。
- <strong>進階評審</strong>：在測試小模型輸出時，使用較大型模型作為評審。
- <strong>追蹤歷史</strong>：保存評估結果以比較不同提示和模型版本。

---

## 下一個實驗室

繼續前往 [第 9 部分：使用 Whisper 進行語音轉錄](part9-whisper-voice-transcription.md)，探索如何用 Foundry Local SDK 在設備端進行語音轉文字。