"""
Product Agent — Foundry Local version.

Searches the local product catalog (products.json) using keyword matching
instead of Azure AI Search + embeddings. Returns relevant products for
the writer to reference in the article.
"""

import json
import sys
from pathlib import Path

# Add api root to path so we can import foundry_config
sys.path.insert(0, str(Path(__file__).resolve().parents[2]))
from foundry_config import client, model_id

# Load the local product catalog
PRODUCTS_FILE = Path(__file__).parent.parent / "writer" / "products.json"
with open(PRODUCTS_FILE, "r", encoding="utf-8") as f:
    PRODUCT_CATALOG = json.load(f)


def _keyword_search(query: str, top_k: int = 3) -> list:
    """Simple keyword-overlap product search."""
    query_words = set(query.lower().split())
    scored = []
    for product in PRODUCT_CATALOG:
        text = f"{product['title']} {product['content']}".lower()
        product_words = set(text.split())
        overlap = len(query_words & product_words)
        scored.append((overlap, product))
    scored.sort(key=lambda x: x[0], reverse=True)
    return [item[1] for item in scored[:top_k]]


def find_products(context: str):
    """Public API consumed by the orchestrator.

    1. Ask the LLM to generate search queries from the context.
    2. Search the local product catalog for each query.
    3. Return de-duplicated product list.
    """
    # Step 1 — generate queries via local LLM
    response = client.chat.completions.create(
        model=model_id,
        messages=[
            {
                "role": "system",
                "content": (
                    "You produce search queries for a product catalog. "
                    "Given a context, return a JSON array of 3-5 short search "
                    "query strings. Return ONLY the JSON array, no other text."
                ),
            },
            {"role": "user", "content": context},
        ],
        temperature=0.3,
        max_tokens=300,
    )

    raw = response.choices[0].message.content.strip()
    raw = raw.removeprefix("```json").removeprefix("```").removesuffix("```").strip()
    try:
        queries = json.loads(raw)
    except json.JSONDecodeError:
        queries = [context]

    # Step 2 — search local catalog
    seen_ids = set()
    products = []
    for q in queries:
        for p in _keyword_search(q):
            if p["id"] not in seen_ids:
                seen_ids.add(p["id"])
                products.append(p)

    return products


if __name__ == "__main__":
    context = "Can you use a selection of power tools and paints as context?"
    result = find_products(context)
    print(json.dumps(result, indent=2))
