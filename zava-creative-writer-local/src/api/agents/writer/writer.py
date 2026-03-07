"""
Writer Agent — Foundry Local version.

Takes research notes + product information from Zava Retail and writes
an engaging article, using the local model via Foundry Local.
"""

import sys
from pathlib import Path
import logging

# Add api root to path so we can import foundry_config
sys.path.insert(0, str(Path(__file__).resolve().parents[2]))
from foundry_config import client, model_id

# Keep prompts within the model's context window on revision passes
MAX_RESEARCH_CHARS = 1500
MAX_PRODUCT_CHARS = 150
MAX_FEEDBACK_CHARS = 500
MAX_TOKENS = 1500

SYSTEM_PROMPT = """\
You are an expert copywriter for Zava Retail, a DIY and home improvement company.
You take research from a web researcher as well as product information from the Zava product catalog
to produce a fun and engaging article that can be used as a magazine article or a blog post.
The goal is to engage DIY enthusiasts and provide them with a fun, informative article about
home improvement, renovation projects, and the tools and materials that make them possible.
The article should be between 800 and 1000 words.

After the article, add a line with "---" and then provide brief feedback notes about what could be
improved in the article.
"""


def write(researchContext, research, productContext, products, assignment, feedback="No Feedback"):
    """Generate the article as a streaming response."""
    # Build a concise context string from the research and products
    research_text = ""
    if isinstance(research, dict) and "web" in research:
        for item in research["web"]:
            research_text += f"- {item.get('name', '')}: {item.get('description', '')}\n"
    elif isinstance(research, list):
        for item in research:
            research_text += f"- {item.get('name', '')}: {item.get('description', '')}\n"
    research_text = research_text[:MAX_RESEARCH_CHARS]

    product_text = ""
    if isinstance(products, list):
        for p in products:
            product_text += f"- {p.get('title', '')}: {p.get('content', '')[:MAX_PRODUCT_CHARS]}\n"

    trimmed_feedback = (feedback or "No Feedback")[:MAX_FEEDBACK_CHARS]

    user_message = (
        f"# Assignment\n{assignment}\n\n"
        f"# Research Context\n{researchContext}\n\n"
        f"# Web Research\n{research_text}\n\n"
        f"# Product Context\n{productContext}\n\n"
        f"# Products\n{product_text}\n\n"
        f"# Feedback from editor\n{trimmed_feedback}"
    )

    try:
        stream = client.chat.completions.create(
            model=model_id,
            messages=[
                {"role": "system", "content": SYSTEM_PROMPT},
                {"role": "user", "content": user_message},
            ],
            max_tokens=MAX_TOKENS,
            stream=True,
        )
        for chunk in stream:
            if chunk.choices[0].delta.content is not None:
                yield chunk.choices[0].delta.content
    except Exception:
        # Log full exception details server-side, but return a generic message to the user.
        logging.exception("Writer agent encountered an error while generating the article.")
        yield "An internal error occurred while generating the article."


def process(writer_output):
    """Split the raw output into article and feedback sections."""
    result = writer_output.split("---")
    article = str(result[0]).strip()
    feedback = str(result[1]).strip() if len(result) > 1 else "No Feedback"
    return {"article": article, "feedback": feedback}


if __name__ == "__main__":
    result = write(
        "DIY home improvement trends",
        {"web": [{"name": "Test", "description": "Test research"}]},
        "power tools and paints",
        [{"title": "Zava ProGrip Cordless Drill", "content": "A powerful cordless drill for every DIY project."}],
        "Write a fun article about weekend DIY projects.",
    )
    full = ""
    for chunk in result:
        full += chunk
        print(chunk, end="", flush=True)
    print()
    print(process(full))
