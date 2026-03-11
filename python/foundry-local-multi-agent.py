"""
Foundry Local — Multi-Agent Workflow with Microsoft Agent Framework

Demonstrates a multi-agent pipeline running entirely on-device using
FoundryLocalClient:
  1. Researcher agent  — gathers background information
  2. Writer agent      — drafts an article from the research
  3. Editor agent      — reviews and provides feedback

Each agent has its own system instructions and persona. The agents
collaborate sequentially: Researcher → Writer → Editor.
"""

import asyncio

from agent_framework_foundry_local import FoundryLocalClient


async def main():
    # ── Start Foundry Local ──────────────────────────────────────────────
    alias = "phi-4-mini"

    print("=== Multi-Agent Workflow with Foundry Local ===")

    # FoundryLocalClient handles service start, model download, and loading
    client = FoundryLocalClient(model_id=alias)
    print(f"Model: {client.model_id}")
    print(f"Endpoint: {client.manager.endpoint}\n")

    # ── Define agents ────────────────────────────────────────────────────
    researcher = client.as_agent(
        name="Researcher",
        instructions=(
            "You are a research assistant. When given a topic, provide a concise "
            "collection of key facts, statistics, and background information. "
            "Organize your findings as bullet points."
        ),
    )

    writer = client.as_agent(
        name="Writer",
        instructions=(
            "You are a skilled blog writer. Using the research notes provided, "
            "write a short, engaging blog post (3-4 paragraphs). "
            "Include a catchy title. Do not make up facts beyond what is given."
        ),
    )

    editor = client.as_agent(
        name="Editor",
        instructions=(
            "You are a senior editor. Review the blog post below for clarity, "
            "grammar, and factual consistency with the research notes. "
            "Provide a brief editorial verdict: ACCEPT if the post is "
            "publication-ready, or REVISE with specific suggestions."
        ),
    )

    topic = "The history and future of renewable energy"

    # ── Agent workflow: Researcher → Writer → Editor ─────────────────────
    print("=" * 60)
    print(f"📋 Topic: {topic}")
    print("=" * 60)

    # Step 1 — Research
    print("\n🔍 Researcher is gathering information...")
    research_result = await researcher.run(
        f"Research the following topic and provide key facts:\n{topic}"
    )
    print(f"\n--- Research Notes ---\n{research_result.text}\n")

    # Step 2 — Write
    print("✍️  Writer is drafting the article...")
    writer_result = await writer.run(
        f"Write a blog post based on these research notes:\n\n{research_result.text}"
    )
    print(f"\n--- Draft Article ---\n{writer_result.text}\n")

    # Step 3 — Edit
    print("📝 Editor is reviewing the article...")
    editor_result = await editor.run(
        f"Review this article for quality and accuracy.\n\n"
        f"Research notes:\n{research_result.text}\n\n"
        f"Article:\n{writer_result.text}"
    )
    print(f"\n--- Editor Verdict ---\n{editor_result.text}\n")

    print("=" * 60)
    print("✅ Multi-agent workflow complete!")


asyncio.run(main())
