/**
 * Foundry Local — Multi-Agent Workflow (JavaScript)
 *
 * Demonstrates a multi-agent pipeline running entirely on-device:
 *   1. Researcher agent  — gathers background information
 *   2. Writer agent      — drafts an article from the research
 *   3. Editor agent      — reviews and provides feedback
 *
 * Each agent has its own system instructions and persona. The agents
 * collaborate sequentially: Researcher → Writer → Editor.
 */

import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

// ── Simple ChatAgent class ─────────────────────────────────────────────────
class ChatAgent {
  constructor({ client, modelId, instructions, name }) {
    this.client = client;
    this.modelId = modelId;
    this.instructions = instructions;
    this.name = name;
  }

  async run(userMessage) {
    const response = await this.client.chat.completions.create({
      model: this.modelId,
      messages: [
        { role: "system", content: this.instructions },
        { role: "user", content: userMessage },
      ],
    });
    return { text: response.choices[0].message.content };
  }
}

// ── Main workflow ──────────────────────────────────────────────────────────
async function main() {
  const alias = "phi-3.5-mini";

  // Step 1: Create a FoundryLocalManager and start the service
  console.log("Starting Foundry Local service...");
  FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
  const manager = FoundryLocalManager.instance;
  await manager.startWebService();

  // Step 2: Get the model from the catalog
  const catalog = manager.catalog;
  const model = await catalog.getModel(alias);

  if (model.isCached) {
    console.log(`Model already downloaded: ${alias}`);
  } else {
    console.log(
      `Downloading model: ${alias} (this may take several minutes)...`
    );
    await model.download();
    console.log(`Download complete: ${alias}`);
  }

  // Step 3: Load the model into memory
  console.log(`Loading model: ${alias}...`);
  await model.load();
  console.log(`Model: ${model.id}`);
  console.log(`Endpoint: ${manager.urls[0]}\n`);

  const client = new OpenAI({
    baseURL: manager.urls[0] + "/v1",
    apiKey: "foundry-local",
  });

  // ── Define agents ──────────────────────────────────────────────────────
  const researcher = new ChatAgent({
    client,
    modelId: model.id,
    instructions:
      "You are a research assistant. When given a topic, provide a concise " +
      "collection of key facts, statistics, and background information. " +
      "Organize your findings as bullet points.",
    name: "Researcher",
  });

  const writer = new ChatAgent({
    client,
    modelId: model.id,
    instructions:
      "You are a skilled blog writer. Using the research notes provided, " +
      "write a short, engaging blog post (3-4 paragraphs). " +
      "Include a catchy title. Do not make up facts beyond what is given.",
    name: "Writer",
  });

  const editor = new ChatAgent({
    client,
    modelId: model.id,
    instructions:
      "You are a senior editor. Review the blog post below for clarity, " +
      "grammar, and factual consistency with the research notes. " +
      "Provide a brief editorial verdict: ACCEPT if the post is " +
      "publication-ready, or REVISE with specific suggestions.",
    name: "Editor",
  });

  const topic = "The history and future of renewable energy";

  // ── Agent workflow: Researcher → Writer → Editor ───────────────────────
  console.log("=".repeat(60));
  console.log(`Topic: ${topic}`);
  console.log("=".repeat(60));

  // Step 1 — Research
  console.log("\nResearcher is gathering information...");
  const researchResult = await researcher.run(
    `Research the following topic and provide key facts:\n${topic}`
  );
  console.log(`\n--- Research Notes ---\n${researchResult.text}\n`);

  // Step 2 — Write
  console.log("Writer is drafting the article...");
  const writerResult = await writer.run(
    `Write a blog post based on these research notes:\n\n${researchResult.text}`
  );
  console.log(`\n--- Draft Article ---\n${writerResult.text}\n`);

  // Step 3 — Edit
  console.log("Editor is reviewing the article...");
  const editorResult = await editor.run(
    `Review this article for quality and accuracy.\n\n` +
      `Research notes:\n${researchResult.text}\n\n` +
      `Article:\n${writerResult.text}`
  );
  console.log(`\n--- Editor Verdict ---\n${editorResult.text}\n`);

  console.log("=".repeat(60));
  console.log("Multi-agent workflow complete!");
}

main();
