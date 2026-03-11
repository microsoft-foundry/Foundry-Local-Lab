/**
 * Foundry Local — Retrieval-Augmented Generation (RAG) Example
 *
 * Demonstrates a simple RAG pipeline that runs entirely on-device:
 * 1. A small knowledge base of text chunks is defined locally.
 * 2. A user question is answered by first retrieving the most relevant
 *    chunks (simple keyword overlap scoring) and then sending them as
 *    context to a local model via Foundry Local.
 *
 * No cloud services, embeddings API, or vector database required.
 */

import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

// ── 1. Local knowledge base ────────────────────────────────────────────────
const KNOWLEDGE_BASE = [
  {
    title: "Foundry Local Overview",
    content:
      "Foundry Local brings the power of Azure AI Foundry to your local " +
      "device without requiring an Azure subscription. It allows you to " +
      "run Generative AI models directly on your local hardware with no " +
      "sign-up required, keeping all data processing on-device for " +
      "enhanced privacy and security.",
  },
  {
    title: "Supported Hardware",
    content:
      "Foundry Local automatically selects the best model variant for " +
      "your hardware. If you have an Nvidia CUDA GPU it downloads the " +
      "CUDA-optimized model. For a Qualcomm NPU it downloads the " +
      "NPU-optimized model. Otherwise it uses the CPU-optimized model. " +
      "Performance is optimized through ONNX Runtime and hardware " +
      "acceleration.",
  },
  {
    title: "OpenAI-Compatible API",
    content:
      "Foundry Local exposes an OpenAI-compatible REST API so you can " +
      "use the standard OpenAI Python, JavaScript or C# SDKs to " +
      "interact with local models. The endpoint is dynamically assigned " +
      "— always obtain it from the SDK's manager.endpoint property " +
      "rather than hard-coding a port number.",
  },
  {
    title: "Model Catalog",
    content:
      "You can browse all available models at foundrylocal.ai or by " +
      "running 'foundry model list' in your terminal. Popular models " +
      "include Phi-3.5-mini, Phi-4-mini, Qwen 2.5, Mistral, and " +
      "DeepSeek-R1. Models are downloaded on first use and cached " +
      "locally for future sessions.",
  },
  {
    title: "Installation",
    content:
      "On Windows install Foundry Local with: " +
      "winget install Microsoft.FoundryLocal. " +
      "On macOS install with: " +
      "brew install microsoft/foundrylocal/foundrylocal. " +
      "You can also download installers from the GitHub releases page.",
  },
];

// ── 2. Simple keyword retrieval ────────────────────────────────────────────
function retrieve(query, topK = 2) {
  const queryWords = new Set(query.toLowerCase().split(/\s+/));
  const scored = KNOWLEDGE_BASE.map((chunk) => {
    const chunkWords = new Set(chunk.content.toLowerCase().split(/\s+/));
    let overlap = 0;
    for (const w of queryWords) {
      if (chunkWords.has(w)) overlap++;
    }
    return { overlap, chunk };
  });
  scored.sort((a, b) => b.overlap - a.overlap);
  return scored.slice(0, topK).map((s) => s.chunk);
}

// ── 3. Generation with retrieved context ───────────────────────────────────
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

  const client = new OpenAI({
    baseURL: manager.urls[0] + "/v1",
    apiKey: "foundry-local",
  });

  // User question
  const question =
    "How do I install Foundry Local and what hardware does it support?";
  console.log(`Question: ${question}\n`);

  // Retrieve relevant context
  const contextChunks = retrieve(question);
  const contextText = contextChunks
    .map((c) => `### ${c.title}\n${c.content}`)
    .join("\n\n");
  console.log("--- Retrieved Context ---");
  console.log(contextText);
  console.log("-------------------------\n");

  // Build prompt with retrieved context
  const systemPrompt =
    "You are a helpful assistant. Answer the user's question using ONLY " +
    "the information provided in the context below. If the context does " +
    "not contain enough information, say so.\n\n" +
    `Context:\n${contextText}`;

  // Generate a response
  const stream = await client.chat.completions.create({
    model: model.id,
    messages: [
      { role: "system", content: systemPrompt },
      { role: "user", content: question },
    ],
    stream: true,
  });

  process.stdout.write("Answer: ");
  for await (const chunk of stream) {
    if (chunk.choices[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
  console.log();
}

main();
