/**
 * Writer Agent — Foundry Local (JavaScript)
 *
 * Takes research notes + product information and writes an engaging article,
 * streaming tokens as they arrive.
 */

import { client, modelId } from "./foundryConfig.mjs";

// Keep prompts within the model's context window on revision passes
const MAX_RESEARCH_CHARS = 1500;
const MAX_PRODUCT_CHARS = 150;
const MAX_FEEDBACK_CHARS = 500;
const MAX_TOKENS = 1500;

const SYSTEM_PROMPT = `You are an expert copywriter for Zava Retail, a DIY and home improvement company.
You take research from a web researcher as well as product information from the Zava product catalog
to produce a fun and engaging article that can be used as a magazine article or a blog post.
The goal is to engage DIY enthusiasts and provide them with a fun, informative article about
home improvement, renovation projects, and the tools and materials that make them possible.
The article should be between 800 and 1000 words.

After the article, add a line with "---" and then provide brief feedback notes about what could be
improved in the article.`;

export async function* write(
  researchContext,
  research,
  productContext,
  products,
  assignment,
  feedback = "No Feedback"
) {
  // Build context strings
  let researchText = "";
  const webItems = Array.isArray(research)
    ? research
    : research?.web || [];
  for (const item of webItems) {
    researchText += `- ${item.name || ""}: ${item.description || ""}\n`;
  }
  researchText = researchText.slice(0, MAX_RESEARCH_CHARS);

  let productText = "";
  if (Array.isArray(products)) {
    for (const p of products) {
      productText += `- ${p.title || ""}: ${(p.content || "").slice(0, MAX_PRODUCT_CHARS)}\n`;
    }
  }

  const trimmedFeedback = (feedback || "No Feedback").slice(0, MAX_FEEDBACK_CHARS);

  const userMessage =
    `# Assignment\n${assignment}\n\n` +
    `# Research Context\n${researchContext}\n\n` +
    `# Web Research\n${researchText}\n\n` +
    `# Product Context\n${productContext}\n\n` +
    `# Products\n${productText}\n\n` +
    `# Feedback from editor\n${trimmedFeedback}`;

  const stream = await client.chat.completions.create({
    model: modelId,
    messages: [
      { role: "system", content: SYSTEM_PROMPT },
      { role: "user", content: userMessage },
    ],
    max_tokens: MAX_TOKENS,
    stream: true,
  });

  try {
    for await (const chunk of stream) {
      const text = chunk.choices[0]?.delta?.content;
      if (text) yield text;
    }
  } catch (err) {
    if (err?.code === "ERR_STREAM_PREMATURE_CLOSE") {
      console.warn("\n[Writer] Stream closed early — using partial output.");
    } else {
      throw err;
    }
  }
}

export function processWriterOutput(fullText) {
  const parts = fullText.split("---");
  const article = (parts[0] || "").trim();
  const feedback = parts.length > 1 ? parts[1].trim() : "No Feedback";
  return { article, feedback };
}
