![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 8: Evaluation-Led Development wit Foundry Local

> **Goal:** Build plenti evaluation framework weh go dey test an score your AI agents chook chook, wen di same local model be di agent wey dem dey test an di judge too, so dat you fit dey try prompt wit confidence before you send am out.

## Why Evaluation-Led Development?

When you dey build AI agents, "e look like say e correct" no good reach. **Evaluation-led development** dey treat agent outputs like code: you go write tests first, measure quality, an only send am out when di scores reach one level.

Inside Zava Creative Writer (Part 7), di **Editor agent** don already dey act like light evaluator; e dey decide whether na ACCEPT or REVISE. Dis lab go turn dat pattern to become one evaluation framework wey you fit run for any agent or pipeline.

| Problem | Solution |
|---------|----------|
| Prompt changes slyly spoil quality | **Golden dataset** go catch di wahala dem |
| "E work for one example" bias | **Plenti test cases** go show di edge cases dem |
| Subjective quality check | **Rule-based + LLM-as-judge scoring** go bring numbers |
| No way to compare prompt variants | **Side-by-side eval runs** wit total scores |

---

## Key Concepts

### 1. Golden Datasets

A **golden dataset** na correct set of test cases wey get di expected outputs wey dem know well well. Each test case get:

- **Input**: Di prompt or question to send to di agent
- **Expected output**: Wetin di correct or better response suppose get (keywords, structure, facts)
- **Category**: Group dem for reporting (like "factual accuracy", "tone", "completeness")

### 2. Rule-Based Checks

Fast, sure checks wey no need LLM:

| Check | Wetin E Dey Test |
|-------|------------------|
| **Length bounds** | Response no too short (lazy) or too long (run on) |
| **Required keywords** | Response mention wetin suppose dey or correct entities |
| **Format validation** | JSON fit parse; Markdown headers dey inside |
| **Forbidden content** | No fake brand names or competitor talk |

### 3. LLM-as-Judge

Use di **same local model** to grade im own output (or output from different prompt style). Di judge go get:

- Di original question
- Di agent output
- Di grading guideline

E go give back organized score. Dis na di same Editor pattern from Part 7 but e dey run for all test cases.

### 4. Eval-Driven Iteration Loop

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Foundry Local CLI** | Installed wit model wey you don download |
| **Language runtime** | **Python 3.9+** and/or **Node.js 18+** and/or **.NET 9+ SDK** |
| **Completed** | [Part 5: Single Agents](part5-single-agents.md) and [Part 6: Multi-Agent Workflows](part6-multi-agent-workflows.md) |

---

## Lab Exercises

### Exercise 1 - Run the Evaluation Framework

Dis workshop get complete evaluation example wey go test Foundry Local agent against golden dataset about Zava DIY questions.

<details>
<summary><strong>🐍 Python</strong></summary>

**Setup:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Run:**
```bash
python foundry-local-eval.py
```

**Wetin go happen:**
1. E go connect to Foundry Local an load di model
2. E go define golden dataset wit 5 test cases (Zava product questions)
3. E go run two prompt styles for each test case
4. E go score each response wit **rule-based checks** (length, keywords, format)
5. E go score each response wit **LLM-as-judge** (di same model grade quality 1-5)
6. E go print scorecard wey compare both prompt variants

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Setup:**
```bash
cd javascript
npm install
```

**Run:**
```bash
node foundry-local-eval.mjs
```

**Same evaluation process:** golden dataset, two prompt runs, rule-based + LLM scoring, scorecard.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Setup:**
```bash
cd csharp
dotnet restore
```

**Run:**
```bash
dotnet run eval
```

**Same evaluation process:** golden dataset, two prompt runs, rule-based + LLM scoring, scorecard.

</details>

---

### Exercise 2 - Understand the Golden Dataset

Look the test cases inside di evaluation example. Each test case get:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Questions make you reason:**
1. Why di expected values na **keywords** no be full sentence?
2. How many test cases you need to get correct evaluation?
3. Which categories you go add for your own app?

---

### Exercise 3 - Understand Rule-Based vs LLM-as-Judge Scoring

Di evaluation framework dey use **two scoring layers**:

#### Rule-Based Checks (fast, sure)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-as-Judge (soft, quality-based)

Di same local model dey judge wit structured criteria:

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

**Questions make you reason:**
1. When you go trust rule-based checks pass LLM-as-judge?
2. Fit model judge e own output well? Wetin be di limits?
3. How e take compare to Editor agent pattern from Part 7?

---

### Exercise 4 - Compare Prompt Variants

Di example run **two prompt styles** for di same test cases:

| Variant | System Prompt Style |
|---------|-------------------|
| **Baseline** | Simple: "You be helpful assistant" |
| **Specialised** | Detailed: "You be Zava DIY expert wey dey recommend specific products an dey provide step-by-step aid" |

After you run am, you go see scorecard like dis:

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

**Exercises:**
1. Run evaluation an write down scores for each variant
2. Change di specialised prompt to make am more specific. Di score go better?
3. Add third prompt variant and compare all three.
4. Try change model alias (like `phi-4-mini` vs `phi-3.5-mini`) an compare results.

---

### Exercise 5 - Apply Evaluation to Your Own Agent

Use di evaluation framework as template for your own agents:

1. **Define golden dataset**: write 5 to 10 test cases wit expected keywords.
2. **Write your system prompt**: di instructions for your agent you want test.
3. **Run eval**: get baseline scores.
4. **Iterate**: adjust di prompt, run again, an compare.
5. **Set threshold**: e.g. "no send unless combined score pass 0.75".

---

### Exercise 6 - Connection to the Zava Editor Pattern

Look again di Zava Creative Writer Editor agent (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Di editor na LLM wey dey be judge for production:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Dis na **same idea** wit Part 8 LLM-as-judge, but e dey inside production pipeline no be offline test suite. Both pattern:

- Dey use structured JSON output from di model.
- Apply quality rules inside system prompt.
- Make pass/fail decision.

**Difference:** Editor dey run for production (each time request). Evaluation framework dey run for development (before you send).

---

## Key Takeaways

| Concept | Takeaway |
|---------|----------|
| **Golden datasets** | Prepare test cases early; na your regression test for AI |
| **Rule-based checks** | Fast, sure, catch obvious wahala (length, keywords, format) |
| **LLM-as-judge** | Soft quality scoring with same local model |
| **Prompt comparison** | Run many variants for same test suite to find best one |
| **On-device advantage** | All evaluation dey run local: no API cost, no rate limit, no data waka go anywhere |
| **Eval-before-ship** | Set quality level an hold release if score no reach |

---

## Next Steps

- **Scale up**: Add more test cases an categories to your golden dataset.
- **Automate**: Add evaluation join to your CI/CD pipeline.
- **Advanced judges**: Use big model as judge while you test smaller model output.
- **Track over time**: Save evaluation results to compare prompt and model versions.

---

## Next Lab

Continue go [Part 9: Voice Transcription wit Whisper](part9-whisper-voice-transcription.md) to explore on-device speech-to-text wit Foundry Local SDK.