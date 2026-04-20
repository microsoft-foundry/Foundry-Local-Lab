![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第8部分：使用 Foundry Local 進行評價主導的開發

> **目標：** 建立一個評價框架，系統性地測試和評分你的 AI 代理，使用相同的本地模型作為測試代理和評審，讓你能在出貨前有信心地反覆調整提示詞。

## 為什麼採用評價主導開發？

建立 AI 代理時， 「看起來差不多」是不夠的。<strong>評價主導開發</strong>將代理的輸出視為程式碼：先寫測試、衡量品質，只在分數達標時才發佈。

在 Zava 創意寫手（第7部分）中，<strong>編輯代理</strong>已經充當輕量級評審，決定是接受還是修改。本實驗室將該模式形式化，成為可套用於任何代理或流程的重複評價框架。

| 問題 | 解決方案 |
|---------|----------|
| 提示詞變更悄悄破壞品質 | <strong>金色數據集</strong> 捕捉回歸 |
| 「只對單一例子有效」偏差 | <strong>多個測試案例</strong> 揭露邊緣情況 |
| 主觀品質評估 | **規則化 + LLM 作為評審打分** 提供量化數值 |
| 無法比較提示詞變體 | <strong>並列評價執行</strong> 並給出綜合分數 |

---

## 核心概念

### 1. 金色數據集

<strong>金色數據集</strong>是經過精心挑選的測試案例集合，附帶已知的期望輸出。每個測試案例包含：

- <strong>輸入</strong>：傳送給代理的提示詞或問題
- <strong>期望輸出</strong>：正確或高品質回應應包含的內容（關鍵字、結構、事實）
- <strong>分類</strong>：報告用的分組（例如「事實準確度」、「語氣」、「完整性」）

### 2. 規則檢查

快速、確定性的檢查，不依賴 LLM：

| 檢查項目 | 測試內容 |
|-------|--------------|
| <strong>長度範圍</strong> | 回應不會過短（懶惰）或過長（囉唆） |
| <strong>必需關鍵字</strong> | 回應中包含預期的詞語或實體 |
| <strong>格式驗證</strong> | JSON 可解析、Markdown 有標題 |
| <strong>禁止內容</strong> | 沒有幻覺出的品牌名、沒有競爭對手提及 |

### 3. LLM 作為評審

使用<strong>相同的本地模型</strong>對自己的輸出（或不同提示詞變體的輸出）進行打分。評審會收到：

- 原始問題
- 代理的回應
- 打分標準

並回傳結構化的分數。這與第7部分的編輯者模式相似，但系統性地應用於測試套件。

### 4. 評價驅動的迭代循環

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## 先決條件

| 要求 | 詳細資訊 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝且已下載模型 |
| <strong>語言執行環境</strong> | **Python 3.9+** 和／或 **Node.js 18+** 和／或 **.NET 9+ SDK** |
| <strong>已有完成內容</strong> | [第5部分：單代理](part5-single-agents.md) 與 [第6部分：多代理工作流程](part6-multi-agent-workflows.md) |

---

## 實驗練習

### 練習1 - 執行評價框架

工作坊包含完整評價範例，測試 Foundry Local 代理對 Zava DIY 相關問題的金色數據集。

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

**流程：**
1. 連接 Foundry Local 並載入模型
2. 定義包含 5 個測試案例（金色數據集，Zava 產品問題）
3. 針對每個測試案執行兩個提示詞變體
4. 對每個回應進行<strong>規則檢查</strong>打分（長度、關鍵字、格式）
5. 再以<strong>LLM 作為評審</strong>（同一模型進行 1-5 分品質評分）
6. 列印分數卡，比較兩個提示詞變體

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

**相同評價流程：** 金色數據集、雙提示詞執行、規則 + LLM 打分、分數卡。

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

**相同評價流程：** 金色數據集、雙提示詞執行、規則 + LLM 打分、分數卡。

</details>

---

### 練習2 - 理解金色數據集

檢視評價範例中定義的測試案例。每個案例包含：

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**思考問題：**
1. 為什麼期望值是<strong>關鍵字</strong>而非完整句子？
2. 需要多少測試案例才能有可靠評價？
3. 你會為自己的應用新增哪些分類？

---

### 練習3 - 理解規則檢查與 LLM 作為評審的打分差異

評價框架使用<strong>兩層打分</strong>：

#### 規則檢查（快速，確定性）

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM 作為評審（細緻，質性）

同一個本地模型依照結構化評分標準扮演評審角色：

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
1. 你什麼時候會信任規則檢查勝過 LLM 作為評審？
2. 模型能可靠地評判自己的輸出嗎？有什麼限制？
3. 這與第7部分的編輯者模式差異在哪？

---

### 練習4 - 比較提示詞變體

範例對同一測試案例執行<strong>兩種提示詞變體</strong>：

| 變體 | 系統提示詞風格 |
|---------|-------------------|
| <strong>基線</strong> | 通用：「你是一個有幫助的助手」 |
| <strong>專業</strong> | 詳細：「你是 Zava DIY 專家，推薦特定產品並提供步驟指引」 |

執行後會看到類似的分數卡：

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

**練習內容：**
1. 執行評價並記錄各變體分數
2. 修改專業提示詞使其更具體。分數是否提升？
3. 增加第三個提示詞變體並比較三者。
4. 嘗試更改模型別名（例如 `phi-4-mini` 對比 `phi-3.5-mini`）並比較結果。

---

### 練習5 - 將評價套用於你的代理

以評價框架作為範本，為你自己的代理：

1. <strong>定義你的金色數據集</strong>：編寫 5 到 10 個測試案例及期望關鍵字。
2. <strong>撰寫你的系統提示詞</strong>：代理指令。
3. <strong>執行評價</strong>：獲得基線分數。
4. <strong>迭代調整</strong>：微調提示詞，重新執行並比較。
5. <strong>訂下門檻</strong>：例如「綜合分數低於 0.75 不發佈」。

---

### 練習6 - 與 Zava 編輯者模式的連結

回顧 Zava 創意寫手的編輯代理（`zava-creative-writer-local/src/api/agents/editor/editor.py`）：

```python
# 編輯器是一個生產中的大型語言模型作為裁判器：
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

這是第8部分 LLM 作為評審的<strong>相同概念</strong>，但置於生產流程中，而非離線測試套件。這兩者模式：

- 使用模型的結構化 JSON 輸出。
- 套用系統提示詞定義的品質標準。
- 做出通過／不通過決策。

**差異：** 編輯者在生產環境執行（每次請求時）；評價框架在開發期執行（出貨前）。

---

## 重要重點

| 概念 | 重點 |
|---------|----------|
| <strong>金色數據集</strong> | 早期策劃測試案例；它們是你的 AI 回歸測試 |
| <strong>規則檢查</strong> | 快速、確定性且捕捉明顯錯誤（長度、關鍵字、格式） |
| **LLM 作為評審** | 使用相同本地模型進行細膩的品質評分 |
| <strong>提示詞比較</strong> | 多個變體對同一測試套件執行，挑選最佳 |
| <strong>本地優勢</strong> | 全部評價本機執行：無 API 費用、無速率限制、資料不外洩 |
| <strong>出貨前評價</strong> | 設定品質門檻，以評分為關卡把關發佈 |

---

## 下一步

- <strong>擴充規模</strong>：在金色數據集加入更多測試案例與分類。
- <strong>自動化</strong>：將評價整合進 CI/CD 流程。
- <strong>高階評審</strong>：使用更大模型當評審，同時測試較小模型輸出。
- <strong>長期追蹤</strong>：保存評價結果，比較不同提示詞及模型版本。

---

## 下一個實驗

繼續前往 [第9部分：用 Whisper 進行語音轉錄](part9-whisper-voice-transcription.md)，探索使用 Foundry Local SDK 在本機執行語音轉文字。