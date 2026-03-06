"""
Researcher Agent — Foundry Local version.

Uses the local LLM to generate research notes on a given topic.
Replaces the Azure AI Projects + Bing Grounding workflow with a
purely local implementation.
"""

import json
import sys
from pathlib import Path

# Add api root to path so we can import foundry_config
sys.path.insert(0, str(Path(__file__).resolve().parents[2]))
from foundry_config import client, model_id

SYSTEM_PROMPT = """\
You are a research assistant equipped with broad knowledge.
Given a topic, produce a structured JSON response with relevant findings.

Return ONLY valid JSON (no markdown code fences) with this structure:
{"web": [
  {"url": "", "name": "Source Title", "description": "Concise summary of the finding."},
  ...
]}
Include 3-5 items. If feedback is provided, refine your research accordingly.
"""


def execute_research(instructions: str, feedback: str = "No feedback"):
    """Run the researcher agent and return parsed results."""
    user_content = f"Topic: {instructions}"
    if feedback and feedback != "No feedback":
        user_content += f"\n\nPrevious feedback to address: {feedback}"

    response = client.chat.completions.create(
        model=model_id,
        messages=[
            {"role": "system", "content": SYSTEM_PROMPT},
            {"role": "user", "content": user_content},
        ],
        temperature=0.7,
        max_tokens=1500,
    )

    raw = response.choices[0].message.content
    try:
        parsed = json.loads(raw)
        return parsed.get("web", [])
    except json.JSONDecodeError:
        # Model may wrap in code fences — strip them
        cleaned = raw.strip().removeprefix("```json").removeprefix("```").removesuffix("```").strip()
        try:
            parsed = json.loads(cleaned)
            return parsed.get("web", [])
        except json.JSONDecodeError:
            return [{"url": "", "name": "Raw Research", "description": raw}]


def research(instructions: str, feedback: str = "No feedback"):
    """Public API consumed by the orchestrator."""
    r = execute_research(instructions=instructions, feedback=feedback)
    return {"web": r, "entities": [], "news": []}


if __name__ == "__main__":
    if len(sys.argv) < 2:
        instructions = "Can you find the latest DIY home improvement trends and weekend renovation projects?"
    else:
        instructions = sys.argv[1]
    result = execute_research(instructions=instructions)
    print(json.dumps(result, indent=2))