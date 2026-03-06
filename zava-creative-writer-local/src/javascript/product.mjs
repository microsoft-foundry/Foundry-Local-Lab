/**
 * Product Agent — Foundry Local (JavaScript)
 *
 * Searches the local product catalog using keyword matching.
 * Returns relevant products for the writer to reference.
 */

import { client, modelId } from "./foundryConfig.mjs";
import { PRODUCT_CATALOG } from "./products.mjs";

function keywordSearch(query, topK = 3) {
  const queryWords = new Set(query.toLowerCase().split(/\s+/));
  const scored = PRODUCT_CATALOG.map((product) => {
    const text = `${product.title} ${product.content}`.toLowerCase();
    const productWords = new Set(text.split(/\s+/));
    let overlap = 0;
    for (const w of queryWords) {
      if (productWords.has(w)) overlap++;
    }
    return { overlap, product };
  });
  scored.sort((a, b) => b.overlap - a.overlap);
  return scored.slice(0, topK).map((s) => s.product);
}

export async function findProducts(context) {
  // Step 1 — generate search queries via local LLM
  const response = await client.chat.completions.create({
    model: modelId,
    messages: [
      {
        role: "system",
        content:
          "You produce search queries for a product catalog. " +
          "Given a context, return a JSON array of 3-5 short search " +
          "query strings. Return ONLY the JSON array, no other text.",
      },
      { role: "user", content: context },
    ],
    temperature: 0.3,
    max_tokens: 300,
  });

  const raw = response.choices[0].message.content
    .trim()
    .replace(/^```json\s*/, "")
    .replace(/^```\s*/, "")
    .replace(/\s*```$/, "");

  let queries;
  try {
    queries = JSON.parse(raw);
  } catch {
    queries = [context];
  }

  // Step 2 — search local catalog, deduplicate
  const seenIds = new Set();
  const products = [];
  for (const q of queries) {
    for (const p of keywordSearch(q)) {
      if (!seenIds.has(p.id)) {
        seenIds.add(p.id);
        products.push(p);
      }
    }
  }
  return products;
}
