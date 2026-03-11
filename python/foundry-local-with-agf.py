"""
Foundry Local — Single Agent with Microsoft Agent Framework

Demonstrates creating a single AI agent using FoundryLocalClient
from the Microsoft Agent Framework. The agent runs entirely on-device
via Foundry Local — no cloud required.
"""

import asyncio

from agent_framework_foundry_local import FoundryLocalClient


async def main():
    alias = "phi-4-mini"

    print("=== Basic Foundry Local Client Agent Example ===")

    # FoundryLocalClient handles service start, model download, and loading
    client = FoundryLocalClient(model_id=alias)
    print(f"Client Model ID: {client.model_id}")
    print(f"Endpoint: {client.manager.endpoint}\n")

    # Create an agent with system instructions
    agent = client.as_agent(
        name="Joker",
        instructions="You are good at telling jokes.",
    )

    # Non-streaming: get the complete response at once
    print("--- Non-streaming ---")
    result = await agent.run("Tell me a joke about a pirate.")
    print(f"Agent: {result.text}\n")

    # Streaming: get results as they are generated
    print("--- Streaming ---")
    print("Agent: ", end="", flush=True)
    async for chunk in agent.run("Tell me a joke about a programmer.", stream=True):
        if chunk.text:
            print(chunk.text, end="", flush=True)
    print()


asyncio.run(main())