"""
Foundry Local — Evaluation-Led Development

Demonstrates an evaluation framework that tests AI agent quality using:
1. A golden dataset of test cases with expected keywords
2. Rule-based checks (length, keyword coverage, forbidden terms)
3. LLM-as-judge scoring (the same local model grades quality 1-5)
4. Side-by-side comparison of two prompt variants

Everything runs entirely on-device through Foundry Local.
"""

import json
import re

import openai
from foundry_local import FoundryLocalManager

# ── 1. Golden Dataset ───────────────────────────────────────────────────────
GOLDEN_DATASET = [
    {
        "input": "What tools do I need to build a wooden deck?",
        "expected": ["saw", "drill", "screws", "level", "tape measure"],
        "category": "product-recommendation",
    },
    {
        "input": "How do I fix a leaky kitchen faucet?",
        "expected": ["wrench", "washer", "plumber", "valve", "seal"],
        "category": "repair-guidance",
    },
    {
        "input": "What type of paint should I use for a bathroom?",
        "expected": ["moisture", "mildew", "semi-gloss", "primer", "ventilation"],
        "category": "product-recommendation",
    },
    {
        "input": "How do I safely use a circular saw?",
        "expected": ["safety", "glasses", "guard", "clamp", "blade"],
        "category": "safety-advice",
    },
    {
        "input": "What is the best way to organize a small workshop?",
        "expected": ["pegboard", "shelves", "storage", "tool chest", "workbench"],
        "category": "workspace-setup",
    },
]

# ── 2. Prompt Variants to Compare ──────────────────────────────────────────
PROMPT_VARIANTS = {
    "baseline": (
        "You are a helpful assistant. Answer the user's question clearly and concisely."
    ),
    "specialised": (
        "You are a Zava DIY expert and home improvement specialist. "
        "When answering questions, recommend specific tools and materials, "
        "provide step-by-step guidance, and include safety tips. "
        "Keep answers practical and actionable for a weekend DIYer."
    ),
}

# ── 3. Rule-Based Scoring ──────────────────────────────────────────────────
FORBIDDEN_TERMS = ["home depot", "lowes", "amazon"]


def score_rules(response: str, expected_keywords: list[str]) -> dict:
    """Apply deterministic rule-based checks to a response."""
    words = response.lower().split()
    word_count = len(words)
    response_lower = response.lower()

    # Length check: 50-500 words
    length_score = 1.0 if 50 <= word_count <= 500 else 0.0

    # Keyword coverage
    found = [kw for kw in expected_keywords if kw.lower() in response_lower]
    keyword_score = len(found) / len(expected_keywords) if expected_keywords else 1.0

    # Forbidden terms
    forbidden_found = [t for t in FORBIDDEN_TERMS if t in response_lower]
    forbidden_score = 0.0 if forbidden_found else 1.0

    combined = (length_score + keyword_score + forbidden_score) / 3.0

    return {
        "length_score": length_score,
        "keyword_score": keyword_score,
        "keywords_found": found,
        "keywords_missing": [kw for kw in expected_keywords if kw.lower() not in response_lower],
        "forbidden_score": forbidden_score,
        "forbidden_found": forbidden_found,
        "combined": round(combined, 2),
    }


# ── 4. LLM-as-Judge Scoring ───────────────────────────────────────────────
JUDGE_SYSTEM_PROMPT = """\
You are an impartial quality evaluator. Rate the following response on a scale of 1-5.

Rubric:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved significantly
- 4: Good response with only minor issues
- 5: Excellent, comprehensive, well-structured response

Respond ONLY with valid JSON (no code fences):
{"score": <1-5>, "reasoning": "<brief explanation>"}
"""


def llm_judge(client, model_id: str, question: str, response: str) -> dict:
    """Use the same local model to grade a response quality."""
    result = client.chat.completions.create(
        model=model_id,
        messages=[
            {"role": "system", "content": JUDGE_SYSTEM_PROMPT},
            {
                "role": "user",
                "content": (
                    f"Question: {question}\n\n"
                    f"Response to evaluate:\n{response}"
                ),
            },
        ],
        temperature=0.1,
        max_tokens=256,
    )

    raw = result.choices[0].message.content.strip()
    raw = raw.removeprefix("```json").removeprefix("```").removesuffix("```").strip()

    try:
        parsed = json.loads(raw)
        score = int(parsed.get("score", 3))
        score = max(1, min(5, score))
        return {"score": score, "reasoning": parsed.get("reasoning", "")}
    except (json.JSONDecodeError, ValueError):
        # Fallback: try to extract a number from the text
        numbers = re.findall(r"\b([1-5])\b", raw)
        return {
            "score": int(numbers[0]) if numbers else 3,
            "reasoning": raw,
        }


# ── 5. Run Agent ──────────────────────────────────────────────────────────
def run_agent(client, model_id: str, system_prompt: str, user_input: str) -> str:
    """Run a single agent call and return the response text."""
    result = client.chat.completions.create(
        model=model_id,
        messages=[
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": user_input},
        ],
        temperature=0.7,
        max_tokens=512,
    )
    return result.choices[0].message.content.strip()


# ── 6. Main Evaluation Pipeline ──────────────────────────────────────────
def main():
    alias = "phi-3.5-mini"

    print("=" * 60)
    print("  EVALUATION-LED DEVELOPMENT WITH FOUNDRY LOCAL")
    print("=" * 60)

    # Start Foundry Local
    print("\nStarting Foundry Local service...")
    manager = FoundryLocalManager()
    manager.start_service()

    cached = manager.list_cached_models()
    catalog_info = manager.get_model_info(alias)
    is_cached = any(m.id == catalog_info.id for m in cached) if catalog_info else False

    if is_cached:
        print(f"Model already downloaded: {alias}")
    else:
        print(f"Downloading model: {alias} (this may take several minutes)...")
        manager.download_model(alias)
        print(f"Download complete: {alias}")

    print(f"Loading model: {alias}...")
    manager.load_model(alias)

    client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
    model_id = manager.get_model_info(alias).id

    print(f"Model loaded: {model_id}")
    print(f"Endpoint: {manager.endpoint}")
    print(f"Test cases: {len(GOLDEN_DATASET)}")
    print(f"Prompt variants: {len(PROMPT_VARIANTS)}\n")

    # Run evaluation for each prompt variant
    results = {}

    for variant_name, system_prompt in PROMPT_VARIANTS.items():
        print(f"\n{'─' * 60}")
        print(f"  Evaluating variant: {variant_name.upper()}")
        print(f"{'─' * 60}")

        variant_results = []

        for i, test_case in enumerate(GOLDEN_DATASET, 1):
            print(f"\n  Test {i}/{len(GOLDEN_DATASET)}: {test_case['input'][:50]}...")

            # Run the agent
            response = run_agent(client, model_id, system_prompt, test_case["input"])

            # Rule-based scoring
            rule_scores = score_rules(response, test_case["expected"])
            print(f"    Rule score: {rule_scores['combined']:.2f}  "
                  f"(length={rule_scores['length_score']:.0f}, "
                  f"keywords={rule_scores['keyword_score']:.2f}, "
                  f"forbidden={rule_scores['forbidden_score']:.0f})")
            if rule_scores["keywords_found"]:
                print(f"    Keywords found: {', '.join(rule_scores['keywords_found'])}")
            if rule_scores["keywords_missing"]:
                print(f"    Keywords missing: {', '.join(rule_scores['keywords_missing'])}")

            # LLM-as-judge scoring
            judge_result = llm_judge(client, model_id, test_case["input"], response)
            print(f"    LLM judge: {judge_result['score']}/5  "
                  f"({judge_result['reasoning'][:80]}...)"
                  if len(judge_result["reasoning"]) > 80
                  else f"    LLM judge: {judge_result['score']}/5  "
                       f"({judge_result['reasoning']})")

            variant_results.append({
                "test_case": test_case,
                "response": response,
                "rule_scores": rule_scores,
                "judge_result": judge_result,
            })

        results[variant_name] = variant_results

    # ── Print Scorecard ──────────────────────────────────────────────────
    print("\n\n" + "=" * 60)
    print("  EVALUATION SCORECARD")
    print("=" * 60)
    print(f"\n  {'Variant':<16} {'Rule Score':>12} {'LLM Score':>12} {'Combined':>12}")
    print(f"  {'─' * 16} {'─' * 12} {'─' * 12} {'─' * 12}")

    for variant_name, variant_results in results.items():
        avg_rule = sum(r["rule_scores"]["combined"] for r in variant_results) / len(variant_results)
        avg_llm = sum(r["judge_result"]["score"] for r in variant_results) / len(variant_results)
        # Combined: normalise LLM score to 0-1 and average with rule score
        combined = (avg_rule + avg_llm / 5.0) / 2.0

        print(f"  {variant_name:<16} {avg_rule:>10.2f}   {avg_llm:>9.1f}/5   {combined:>10.2f}")

    print(f"\n  {'─' * 52}")

    # Per-category breakdown
    categories = sorted(set(tc["category"] for tc in GOLDEN_DATASET))
    print(f"\n  Per-Category Breakdown (Rule Score):")
    print(f"  {'Category':<24} ", end="")
    for vn in results:
        print(f"{vn:>14}", end="")
    print()
    print(f"  {'─' * 24} ", end="")
    for _ in results:
        print(f"{'─' * 14}", end="")
    print()

    for cat in categories:
        print(f"  {cat:<24} ", end="")
        for variant_results in results.values():
            cat_results = [r for r in variant_results if r["test_case"]["category"] == cat]
            if cat_results:
                avg = sum(r["rule_scores"]["combined"] for r in cat_results) / len(cat_results)
                print(f"{avg:>12.2f}  ", end="")
            else:
                print(f"{'N/A':>12}  ", end="")
        print()

    print(f"\n{'=' * 60}")
    print("  Evaluation complete!")
    print("=" * 60)


if __name__ == "__main__":
    main()
