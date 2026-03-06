/**
 * Editor Agent — Foundry Local (JavaScript)
 *
 * Reviews an article and decides whether to accept or request revisions.
 */

import { client, modelId } from "./foundryConfig.mjs";

const SYSTEM_PROMPT = `You are an editor at a publishing company. Review the article and feedback below.
Decide whether to accept or request revisions.

Respond ONLY with valid JSON (no code fences) in one of these formats:

If the article is publication-ready:
{"decision": "accept", "researchFeedback": "...", "editorFeedback": "..."}

If the article needs more work:
{"decision": "revise", "researchFeedback": "...", "editorFeedback": "..."}`;

export async function edit(article, feedback) {
  const response = await client.chat.completions.create({
    model: modelId,
    messages: [
      { role: "system", content: SYSTEM_PROMPT },
      {
        role: "user",
        content: `# Article\n${article}\n\n# Feedback\n${feedback}`,
      },
    ],
    temperature: 0.2,
    max_tokens: 512,
  });

  const raw = response.choices[0].message.content
    .trim()
    .replace(/^```json\s*/, "")
    .replace(/^```\s*/, "")
    .replace(/\s*```$/, "");

  try {
    return JSON.parse(raw);
  } catch {
    return {
      decision: "accept",
      researchFeedback: "No Feedback",
      editorFeedback: raw,
    };
  }
}
