"""
Shared Foundry Local configuration for all agents.

Initializes the FoundryLocalManager once and provides an OpenAI-compatible
client that every agent can import.
"""

import sys
import openai
from foundry_local import FoundryLocalManager

# Model alias — Foundry Local picks the best variant for your hardware
MODEL_ALIAS = "phi-3.5-mini"

# Step 1: Create manager and start the Foundry Local service
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Step 2: Check if the model is already downloaded
cached = manager.list_cached_models()
catalog_info = manager.get_model_info(MODEL_ALIAS)
is_cached = any(m.id == catalog_info.id for m in cached) if catalog_info else False

if is_cached:
    print(f"Model already downloaded: {MODEL_ALIAS}")
else:
    print(f"Downloading model: {MODEL_ALIAS} (this may take several minutes)...")

    def _on_progress(progress):
        bar_width = 30
        filled = int(progress / 100 * bar_width)
        bar = "█" * filled + "░" * (bar_width - filled)
        sys.stdout.write(f"\rDownloading: [{bar}] {progress:.1f}%")
        if progress >= 100:
            sys.stdout.write("\n")
        sys.stdout.flush()

    manager.download_model(MODEL_ALIAS, progress_callback=_on_progress)
    print(f"Download complete: {MODEL_ALIAS}")

# Step 3: Load the model into memory
print(f"Loading model: {MODEL_ALIAS}...")
manager.load_model(MODEL_ALIAS)
model_id = manager.get_model_info(MODEL_ALIAS).id
print(f"Model ready: {model_id}")

# Shared OpenAI client pointing at the local endpoint
client = openai.OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)
