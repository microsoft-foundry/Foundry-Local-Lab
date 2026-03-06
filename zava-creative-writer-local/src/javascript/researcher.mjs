/**
 * Researcher Agent — Foundry Local (JavaScript)
 *
 * Uses the local LLM to generate research notes on a given topic.
 */

import { client, modelId } from "./foundryConfig.mjs";

const SYSTEM_PROMPT = `You are a research assistant equipped with broad knowledge.
Given a topic, produce a structured JSON response with relevant findings.

Return ONLY valid JSON (no markdown code fences) with this structure:
{"web": [
  {"url": "", "name": "Source Title", "description": "Concise summary of the finding."},
  ...
]}
Include 3-5 items. If feedback is provided, refine your research accordingly.`;

export async function research(instructions, feedback = "No feedback") {
  let userContent = `Topic: ${instructions}`;
  if (feedback && feedback !== "No feedback") {
    userContent += `\n\nPrevious feedback to address: ${feedback}`;
  }

  const response = await client.chat.completions.create({
    model: modelId,
    messages: [
      { role: "system", content: SYSTEM_PROMPT },
      { role: "user", content: userContent },
    ],
    temperature: 0.7,
    max_tokens: 1500,
  });

  const raw = response.choices[0].message.content;
  try {
    const parsed = JSON.parse(raw);
    return { web: parsed.web || [], entities: [], news: [] };
  } catch {
    // Model may wrap in code fences — strip them
    const cleaned = raw
      .trim()
      .replace(/^```json\s*/, "")
      .replace(/^```\s*/, "")
      .replace(/\s*```$/, "");
    try {
      const parsed = JSON.parse(cleaned);
      return { web: parsed.web || [], entities: [], news: [] };
    } catch {
      return {
        web: [{ url: "", name: "Raw Research", description: raw }],
        entities: [],
        news: [],
      };
    }
  }
}
