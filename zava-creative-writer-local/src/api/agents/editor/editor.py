"""
Editor Agent — Foundry Local version.

Reviews an article and decides whether to accept or request revisions,
using the local model via Foundry Local.
"""

import json
import sys
from pathlib import Path

# Add api root to path so we can import foundry_config
sys.path.insert(0, str(Path(__file__).resolve().parents[2]))
from foundry_config import client, model_id

SYSTEM_PROMPT = """\
You are an editor at a publishing company. Review the article and feedback below.
Decide whether to accept or request revisions.

Respond ONLY with valid JSON (no code fences) in one of these formats:

If the article is publication-ready:
{"decision": "accept", "researchFeedback": "...", "editorFeedback": "..."}

If the article needs more work:
{"decision": "revise", "researchFeedback": "...", "editorFeedback": "..."}
"""


def edit(article, feedback):
    """Evaluate the article and return a JSON decision."""
    response = client.chat.completions.create(
        model=model_id,
        messages=[
            {"role": "system", "content": SYSTEM_PROMPT},
            {
                "role": "user",
                "content": f"# Article\n{article}\n\n# Feedback\n{feedback}",
            },
        ],
        temperature=0.2,
        max_tokens=512,
    )

    raw = response.choices[0].message.content.strip()
    raw = raw.removeprefix("```json").removeprefix("```").removesuffix("```").strip()
    try:
        return json.loads(raw)
    except json.JSONDecodeError:
        return {
            "decision": "accept",
            "researchFeedback": "No Feedback",
            "editorFeedback": raw,
        }


if __name__ == "__main__":
    result = edit(
        "This is a sample article about DIY home improvement trends.",
        "Could use more detail about tool recommendations.",
    )
    print(json.dumps(result, indent=2))