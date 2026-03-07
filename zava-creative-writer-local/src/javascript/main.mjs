/**
 * Zava Creative Writer — Orchestrator (JavaScript)
 *
 * Runs the multi-agent pipeline: Researcher → Product → Writer → Editor
 * with an optional feedback loop (max 2 retries).
 *
 * Usage: node main.mjs
 */

import { research } from "./researcher.mjs";
import { findProducts } from "./product.mjs";
import { write, processWriterOutput } from "./writer.mjs";
import { edit } from "./editor.mjs";

// ── Default inputs ──────────────────────────────────────────────────────────────────
const researchContext =
  "Can you find the latest DIY home improvement trends and weekend renovation projects?";
const productContext =
  "Can you use a selection of power tools and paints as context?";
const assignment =
  "Write a fun and engaging article that includes the research and product information. " +
  "The article should be between 800 and 1000 words.";

const SEP = "=".repeat(60);

async function run() {
  console.log(SEP);
  console.log("Zava Creative Writer — Multi-Agent Pipeline");
  console.log(SEP);

  let feedback = "No Feedback";

  // ── Step 1: Research ────────────────────────────────────────────────
  console.log("\n[Researcher] Gathering information...");
  let researchResult = await research(researchContext, feedback);
  const webCount = researchResult.web?.length ?? 0;
  console.log(`[Researcher] Found ${webCount} source(s).`);

  // ── Step 2: Product search ──────────────────────────────────────────
  console.log("\n[Product] Searching product catalog...");
  const productResult = await findProducts(productContext);
  console.log(
    `[Product] Found ${productResult.length} product(s): ${productResult.map((p) => p.title).join(", ")}`
  );

  // ── Step 3: Writer ──────────────────────────────────────────────────
  console.log("\n[Writer] Drafting article...\n");
  let fullText = "";
  for await (const token of write(
    researchContext,
    researchResult,
    productContext,
    productResult,
    assignment,
    feedback
  )) {
    process.stdout.write(token);
    fullText += token;
  }
  console.log();

  let processed = processWriterOutput(fullText);

  // ── Step 4: Editor ──────────────────────────────────────────────────
  console.log("\n[Editor] Reviewing article...");
  let editorResponse = await edit(processed.article, processed.feedback);
  console.log(
    `[Editor] Decision: ${editorResponse.decision?.toUpperCase()}`
  );
  if (editorResponse.editorFeedback) {
    console.log(`[Editor] Feedback: ${editorResponse.editorFeedback}`);
  }

  // ── Feedback loop (max 2 retries) ───────────────────────────────────
  let retryCount = 0;
  while (
    editorResponse.decision?.toLowerCase().startsWith("revise") &&
    retryCount < 2
  ) {
    retryCount++;
    console.log(`\n--- Revision ${retryCount} ---`);

    const researchFeedback =
      (editorResponse.researchFeedback || "No Feedback").slice(0, 500);
    const editorFeedback = (editorResponse.editorFeedback || "No Feedback").slice(0, 500);

    console.log("[Researcher] Re-researching with feedback...");
    researchResult = await research(researchContext, researchFeedback);

    console.log("[Writer] Re-drafting article...\n");
    fullText = "";
    for await (const token of write(
      researchContext,
      researchResult,
      productContext,
      productResult,
      assignment,
      editorFeedback
    )) {
      process.stdout.write(token);
      fullText += token;
    }
    console.log();

    processed = processWriterOutput(fullText);

    console.log("[Editor] Reviewing revised article...");
    editorResponse = await edit(processed.article, processed.feedback);
    console.log(
      `[Editor] Decision: ${editorResponse.decision?.toUpperCase()}`
    );
    if (editorResponse.editorFeedback) {
      console.log(`[Editor] Feedback: ${editorResponse.editorFeedback}`);
    }
  }

  console.log(`\n${SEP}`);
  console.log("Multi-agent pipeline complete!");
  console.log(SEP);
}

run();
