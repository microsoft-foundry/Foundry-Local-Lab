/**
 * Foundry Local — Evaluation-Led Development (JavaScript)
 *
 * Demonstrates an evaluation framework that tests AI agent quality using:
 *   1. A golden dataset of test cases with expected keywords
 *   2. Rule-based checks (length, keyword coverage, forbidden terms)
 *   3. LLM-as-judge scoring (the same local model grades quality 1-5)
 *   4. Side-by-side comparison of two prompt variants
 *
 * Everything runs entirely on-device through Foundry Local.
 */

import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

// ── 1. Golden Dataset ──────────────────────────────────────────────────────
const GOLDEN_DATASET = [
  {
    input: "What tools do I need to build a wooden deck?",
    expected: ["saw", "drill", "screws", "level", "tape measure"],
    category: "product-recommendation",
  },
  {
    input: "How do I fix a leaky kitchen faucet?",
    expected: ["wrench", "washer", "plumber", "valve", "seal"],
    category: "repair-guidance",
  },
  {
    input: "What type of paint should I use for a bathroom?",
    expected: ["moisture", "mildew", "semi-gloss", "primer", "ventilation"],
    category: "product-recommendation",
  },
  {
    input: "How do I safely use a circular saw?",
    expected: ["safety", "glasses", "guard", "clamp", "blade"],
    category: "safety-advice",
  },
  {
    input: "What is the best way to organize a small workshop?",
    expected: ["pegboard", "shelves", "storage", "tool chest", "workbench"],
    category: "workspace-setup",
  },
];

// ── 2. Prompt Variants to Compare ─────────────────────────────────────────
const PROMPT_VARIANTS = {
  baseline:
    "You are a helpful assistant. Answer the user's question clearly and concisely.",
  specialised:
    "You are a Zava DIY expert and home improvement specialist. " +
    "When answering questions, recommend specific tools and materials, " +
    "provide step-by-step guidance, and include safety tips. " +
    "Keep answers practical and actionable for a weekend DIYer.",
};

// ── 3. Rule-Based Scoring ─────────────────────────────────────────────────
const FORBIDDEN_TERMS = ["home depot", "lowes", "amazon"];

function scoreRules(response, expectedKeywords) {
  const words = response.toLowerCase().split(/\s+/);
  const wordCount = words.length;
  const responseLower = response.toLowerCase();

  // Length check: 50-500 words
  const lengthScore = wordCount >= 50 && wordCount <= 500 ? 1.0 : 0.0;

  // Keyword coverage
  const found = expectedKeywords.filter((kw) =>
    responseLower.includes(kw.toLowerCase())
  );
  const missing = expectedKeywords.filter(
    (kw) => !responseLower.includes(kw.toLowerCase())
  );
  const keywordScore =
    expectedKeywords.length > 0 ? found.length / expectedKeywords.length : 1.0;

  // Forbidden terms
  const forbiddenFound = FORBIDDEN_TERMS.filter((t) =>
    responseLower.includes(t)
  );
  const forbiddenScore = forbiddenFound.length > 0 ? 0.0 : 1.0;

  const combined = (lengthScore + keywordScore + forbiddenScore) / 3.0;

  return {
    lengthScore,
    keywordScore,
    keywordsFound: found,
    keywordsMissing: missing,
    forbiddenScore,
    forbiddenFound,
    combined: Math.round(combined * 100) / 100,
  };
}

// ── 4. LLM-as-Judge Scoring ──────────────────────────────────────────────
const JUDGE_SYSTEM_PROMPT = `You are an impartial quality evaluator. Rate the following response on a scale of 1-5.

Rubric:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved significantly
- 4: Good response with only minor issues
- 5: Excellent, comprehensive, well-structured response

Respond ONLY with valid JSON (no code fences):
{"score": <1-5>, "reasoning": "<brief explanation>"}`;

async function llmJudge(client, modelId, question, response) {
  const result = await client.chat.completions.create({
    model: modelId,
    messages: [
      { role: "system", content: JUDGE_SYSTEM_PROMPT },
      {
        role: "user",
        content: `Question: ${question}\n\nResponse to evaluate:\n${response}`,
      },
    ],
    temperature: 0.1,
    max_tokens: 256,
  });

  let raw = result.choices[0].message.content.trim();
  raw = raw
    .replace(/^```json\s*/i, "")
    .replace(/^```\s*/i, "")
    .replace(/\s*```$/i, "")
    .trim();

  try {
    const parsed = JSON.parse(raw);
    const score = Math.max(1, Math.min(5, parseInt(parsed.score) || 3));
    return { score, reasoning: parsed.reasoning || "" };
  } catch {
    const numbers = raw.match(/\b([1-5])\b/g);
    return {
      score: numbers ? parseInt(numbers[0]) : 3,
      reasoning: raw,
    };
  }
}

// ── 5. Run Agent ─────────────────────────────────────────────────────────
async function runAgent(client, modelId, systemPrompt, userInput) {
  const result = await client.chat.completions.create({
    model: modelId,
    messages: [
      { role: "system", content: systemPrompt },
      { role: "user", content: userInput },
    ],
    temperature: 0.7,
    max_tokens: 512,
  });
  return result.choices[0].message.content.trim();
}

// ── 6. Main Evaluation Pipeline ─────────────────────────────────────────
async function main() {
  const alias = "phi-3.5-mini";

  console.log("=".repeat(60));
  console.log("  EVALUATION-LED DEVELOPMENT WITH FOUNDRY LOCAL");
  console.log("=".repeat(60));

  // Start Foundry Local
  console.log("\nStarting Foundry Local service...");
  const manager = new FoundryLocalManager();
  await manager.startService();

  const cachedModels = await manager.listCachedModels();
  const catalogInfo = await manager.getModelInfo(alias);
  const isAlreadyCached = cachedModels.some((m) => m.id === catalogInfo?.id);

  if (isAlreadyCached) {
    console.log(`Model already downloaded: ${alias}`);
  } else {
    console.log(
      `Downloading model: ${alias} (this may take several minutes)...`
    );
    await manager.downloadModel(alias);
    console.log(`Download complete: ${alias}`);
  }

  console.log(`Loading model: ${alias}...`);
  const modelInfo = await manager.loadModel(alias);
  console.log(`Model loaded: ${modelInfo.id}`);
  console.log(`Endpoint: ${manager.endpoint}`);
  console.log(`Test cases: ${GOLDEN_DATASET.length}`);
  console.log(
    `Prompt variants: ${Object.keys(PROMPT_VARIANTS).length}\n`
  );

  const client = new OpenAI({
    baseURL: manager.endpoint,
    apiKey: manager.apiKey,
  });

  // Run evaluation for each prompt variant
  const results = {};

  for (const [variantName, systemPrompt] of Object.entries(PROMPT_VARIANTS)) {
    console.log(`\n${"─".repeat(60)}`);
    console.log(`  Evaluating variant: ${variantName.toUpperCase()}`);
    console.log("─".repeat(60));

    const variantResults = [];

    for (let i = 0; i < GOLDEN_DATASET.length; i++) {
      const testCase = GOLDEN_DATASET[i];
      console.log(
        `\n  Test ${i + 1}/${GOLDEN_DATASET.length}: ${testCase.input.substring(0, 50)}...`
      );

      // Run the agent
      const response = await runAgent(
        client,
        modelInfo.id,
        systemPrompt,
        testCase.input
      );

      // Rule-based scoring
      const ruleScores = scoreRules(response, testCase.expected);
      console.log(
        `    Rule score: ${ruleScores.combined.toFixed(2)}  ` +
          `(length=${ruleScores.lengthScore.toFixed(0)}, ` +
          `keywords=${ruleScores.keywordScore.toFixed(2)}, ` +
          `forbidden=${ruleScores.forbiddenScore.toFixed(0)})`
      );
      if (ruleScores.keywordsFound.length > 0) {
        console.log(
          `    Keywords found: ${ruleScores.keywordsFound.join(", ")}`
        );
      }
      if (ruleScores.keywordsMissing.length > 0) {
        console.log(
          `    Keywords missing: ${ruleScores.keywordsMissing.join(", ")}`
        );
      }

      // LLM-as-judge scoring
      const judgeResult = await llmJudge(
        client,
        modelInfo.id,
        testCase.input,
        response
      );
      const reasoningSnippet =
        judgeResult.reasoning.length > 80
          ? judgeResult.reasoning.substring(0, 80) + "..."
          : judgeResult.reasoning;
      console.log(
        `    LLM judge: ${judgeResult.score}/5  (${reasoningSnippet})`
      );

      variantResults.push({
        testCase,
        response,
        ruleScores,
        judgeResult,
      });
    }

    results[variantName] = variantResults;
  }

  // ── Print Scorecard ────────────────────────────────────────────────────
  console.log("\n\n" + "=".repeat(60));
  console.log("  EVALUATION SCORECARD");
  console.log("=".repeat(60));

  const header = `  ${"Variant".padEnd(16)} ${"Rule Score".padStart(12)} ${"LLM Score".padStart(12)} ${"Combined".padStart(12)}`;
  console.log(header);
  console.log(
    `  ${"─".repeat(16)} ${"─".repeat(12)} ${"─".repeat(12)} ${"─".repeat(12)}`
  );

  for (const [variantName, variantResults] of Object.entries(results)) {
    const avgRule =
      variantResults.reduce((sum, r) => sum + r.ruleScores.combined, 0) /
      variantResults.length;
    const avgLlm =
      variantResults.reduce((sum, r) => sum + r.judgeResult.score, 0) /
      variantResults.length;
    const combined = (avgRule + avgLlm / 5.0) / 2.0;

    console.log(
      `  ${variantName.padEnd(16)} ${avgRule.toFixed(2).padStart(10)}   ${(avgLlm.toFixed(1) + "/5").padStart(9)}   ${combined.toFixed(2).padStart(10)}`
    );
  }

  console.log(`\n  ${"─".repeat(52)}`);

  // Per-category breakdown
  const categories = [
    ...new Set(GOLDEN_DATASET.map((tc) => tc.category)),
  ].sort();
  console.log("\n  Per-Category Breakdown (Rule Score):");

  let catHeader = `  ${"Category".padEnd(24)} `;
  for (const vn of Object.keys(results)) {
    catHeader += vn.padStart(14);
  }
  console.log(catHeader);

  let catSep = `  ${"─".repeat(24)} `;
  for (const _ of Object.keys(results)) {
    catSep += "─".repeat(14);
  }
  console.log(catSep);

  for (const cat of categories) {
    let line = `  ${cat.padEnd(24)} `;
    for (const variantResults of Object.values(results)) {
      const catResults = variantResults.filter(
        (r) => r.testCase.category === cat
      );
      if (catResults.length > 0) {
        const avg =
          catResults.reduce((sum, r) => sum + r.ruleScores.combined, 0) /
          catResults.length;
        line += avg.toFixed(2).padStart(12) + "  ";
      } else {
        line += "N/A".padStart(12) + "  ";
      }
    }
    console.log(line);
  }

  console.log(`\n${"=".repeat(60)}`);
  console.log("  Evaluation complete!");
  console.log("=".repeat(60));
}

main();
