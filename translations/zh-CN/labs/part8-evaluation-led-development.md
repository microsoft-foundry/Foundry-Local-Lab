![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第8部分：使用Foundry Local进行评估驱动开发

> **目标：** 构建一个评估框架，系统地测试和评分您的AI代理，使用相同的本地模型作为被测试的代理和评审，从而让您能够在发布前有信心地迭代提示词。

## 为什么要进行评估驱动开发？

构建AI代理时，“看起来差不多”是不够的。<strong>评估驱动开发</strong>将代理输出视为代码：先编写测试，衡量质量，只有分数达到阈值时才发布。

在Zava创意写作员（第7部分）中，<strong>编辑代理</strong>已经作为轻量级评审者；它决定接受或修改。本实验将此模式形式化为可重复应用于任何代理或流程的评估框架。

| 问题 | 解决方案 |
|---------|----------|
| 提示词更改会悄然破坏质量 | <strong>黄金数据集</strong>捕捉回归问题 |
| “只对一个例子有效”的偏差 | <strong>多个测试用例</strong>揭示边缘情况 |
| 主观的质量评估 | <strong>基于规则 + LLM作为评审评分</strong>提供量化数据 |
| 无法比较提示词变体 | <strong>并行评估运行</strong>并汇总分数 |

---

## 关键概念

### 1. 黄金数据集

<strong>黄金数据集</strong>是一组策划好的测试用例，包含已知的预期输出。每个测试用例包括：

- <strong>输入</strong>：发送给代理的提示或问题
- <strong>预期输出</strong>：正确或高质量回答应包含的内容（关键词、结构、事实）
- <strong>类别</strong>：用于报告的分组（如“事实准确性”、“语气”、“完整性”）

### 2. 基于规则的检查

快速、确定性的检查，不依赖大语言模型：

| 检查项 | 测试内容 |
|-------|--------------|
| <strong>长度范围</strong> | 回答不应过短（懒惰）或过长（啰嗦） |
| <strong>必需关键词</strong> | 回答提及预期的术语或实体 |
| <strong>格式验证</strong> | JSON可解析，Markdown标题存在 |
| <strong>禁止内容</strong> | 无幻觉品牌名，无竞争对手提及 |

### 3. LLM作为评审

使用<strong>相同的本地模型</strong>对其自身输出（或不同提示词变体的输出）进行评分。评审收到：

- 原始问题
- 代理的回答
- 评分标准

并返回结构化分数。这反映了第7部分中的编辑者模式，但系统地应用于整个测试套件。

### 4. 评估驱动的迭代循环

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## 前置条件

| 要求 | 详情 |
|-------------|---------|
| **Foundry Local CLI** | 已安装并下载好模型 |
| <strong>语言运行环境</strong> | **Python 3.9+** 和/或 **Node.js 18+** 和/或 **.NET 9+ SDK** |
| <strong>完成进度</strong> | [第5部分：单代理](part5-single-agents.md) 和 [第6部分：多代理工作流](part6-multi-agent-workflows.md) |

---

## 实验练习

### 练习1 - 运行评估框架

该工作坊包含完整的评估示例，测试Foundry Local代理针对Zava DIY相关问题的黄金数据集。

<details>
<summary><strong>🐍 Python</strong></summary>

**设置：**
```bash
cd python
python -m venv venv

# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS：
source venv/bin/activate

pip install -r requirements.txt
```

**运行：**
```bash
python foundry-local-eval.py
```

**运行过程：**
1. 连接Foundry Local并加载模型
2. 定义包含5个测试用例的黄金数据集（Zava产品问题）
3. 使用两个提示词变体针对每个测试用例执行
4. 对每个回答应用<strong>基于规则的检查</strong>（长度、关键词、格式）
5. 使用<strong>LLM作为评审</strong>对每个回答评分（同一模型评定质量1-5分）
6. 打印比较两种提示词变体的评分卡

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**设置：**
```bash
cd javascript
npm install
```

**运行：**
```bash
node foundry-local-eval.mjs
```

**相同的评估流程：** 黄金数据集，双提示词运行，基于规则 + LLM评分，评分卡。

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**设置：**
```bash
cd csharp
dotnet restore
```

**运行：**
```bash
dotnet run eval
```

**相同的评估流程：** 黄金数据集，双提示词运行，基于规则 + LLM评分，评分卡。

</details>

---

### 练习2 - 理解黄金数据集

查看评估样例中定义的测试用例。每个测试用例包含：

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**思考问题：**
1. 为什么预期值是<strong>关键词</strong>而非完整句子？
2. 可靠评估需要多少测试用例？
3. 您会为自己的应用添加哪些类别？

---

### 练习3 - 理解基于规则与LLM作为评审的评分

评估框架使用<strong>两层评分</strong>：

#### 基于规则检查（快速、确定性）

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM作为评审（细致、定性）

相同的本地模型作为评审，使用结构化评分标准：

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

**思考问题：**
1. 何时会信任基于规则的检查胜于LLM作为评审？
2. 模型能否可靠地评判自己的输出？有哪些限制？
3. 这与第7部分中的编辑代理模式有何异同？

---

### 练习4 - 比较提示词变体

示例中针对相同测试用例运行了<strong>两个提示词变体</strong>：

| 变体 | 系统提示风格 |
|---------|-------------------|
| <strong>基础版</strong> | 通用：“你是一个乐于助人的助手” |
| <strong>专业版</strong> | 详细：“你是一个Zava DIY专家，推荐具体产品并提供步骤指导” |

运行后，您会看到类似以下的评分卡：

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

**练习：**
1. 运行评估并记录每个变体的分数
2. 修改专业提示，使其更具体。分数是否提升？
3. 添加第三个提示词变体并比较三者
4. 尝试更换模型别名（如`phi-4-mini` vs `phi-3.5-mini`），并比较结果

---

### 练习5 - 将评估应用于您自己的代理

将评估框架作为模板应用于您自己的代理：

1. <strong>定义您的黄金数据集</strong>：编写5到10个带预期关键词的测试用例
2. <strong>写您的系统提示</strong>：您想测试的代理指令
3. <strong>运行评估</strong>：获得基准分
4. <strong>迭代</strong>：调整提示词，重新运行，比较结果
5. <strong>设定阈值</strong>：如“不低于0.75的综合分才发布”

---

### 练习6 - 与Zava编辑者模式的联系

回顾Zava创意写作员的编辑代理（`zava-creative-writer-local/src/api/agents/editor/editor.py`）：

```python
# 编辑器是一个用于生产的LLM作为裁判：
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

这与第8部分的LLM作为评审是<strong>相同的概念</strong>，但嵌入于生产流程，而非离线测试套件。两个模式都：

- 使用模型的结构化JSON输出。
- 应用系统提示中定义的质量标准。
- 做出通过/不通过决策。

**区别在于：** 编辑器在生产环境（每次请求时）运行。评估框架在开发阶段（发布前）运行。

---

## 主要收获

| 概念 | 收获 |
|---------|----------|
| <strong>黄金数据集</strong> | 早期策划测试用例；它们是AI的回归测试 |
| <strong>基于规则检查</strong> | 快速、确定性，捕捉显而易见的失败（长度、关键词、格式） |
| **LLM作为评审** | 使用相同本地模型进行细致质量评分 |
| <strong>提示词比较</strong> | 同一测试套件中运行多个变体，选出最优者 |
| <strong>本地运行优势</strong> | 所有评估本地运行：无API费用，无频率限制，无数据外泄 |
| <strong>发布前评估</strong> | 设定质量阈值，基于评估分数控制发布 |

---

## 后续步骤

- <strong>规模扩展</strong>：为黄金数据集增加更多测试用例和类别
- <strong>自动化</strong>：将评估集成到CI/CD流水线
- <strong>高级评审</strong>：使用更大模型作为评审，同时测试小模型输出
- <strong>历史追踪</strong>：保存评估结果以比较不同提示和模型版本

---

## 下一个实验

继续阅读[第9部分：使用Whisper进行语音转录](part9-whisper-voice-transcription.md)，探索使用Foundry Local SDK的设备端语音转文字功能。