"""
Whisper Voice Transcription with Foundry Local
Transcribes WAV audio files using the OpenAI Whisper model running locally.
Usage: python foundry-local-whisper.py [path-to-wav-file]
"""

import sys
import os
import glob
import openai
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"

# Default: transcribe all Zava sample WAV files
samples_dir = os.path.join(os.path.dirname(__file__), "..", "samples", "audio")

# If a specific file is provided as argument, transcribe only that file
if len(sys.argv) > 1:
    audio_files = [sys.argv[1]]
    for f in audio_files:
        if not os.path.exists(f):
            print(f"Audio file not found: {f}")
            print("Usage: python foundry-local-whisper.py [path-to-wav-file]")
            sys.exit(1)
else:
    audio_files = sorted(glob.glob(os.path.join(samples_dir, "zava-*.wav")))
    if not audio_files:
        print(f"No WAV files found in {samples_dir}")
        print("Run 'python samples/audio/generate_samples.py' first to generate them.")
        sys.exit(1)

# Step 1: Bootstrap — starts the service, downloads, and loads the model
print(f"Initializing Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_id = manager.get_model_info(model_alias).id
print(f"Model ready: {model_id}")
print(f"Endpoint: {manager.endpoint}")

# Step 2: Create the OpenAI client pointing to the local service
client = openai.OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key
)

# Step 3: Transcribe each audio file
for audio_path in audio_files:
    filename = os.path.basename(audio_path)
    print(f"\n{'=' * 60}")
    print(f"File: {filename}")
    print("=" * 60)

    with open(audio_path, "rb") as f:
        result = client.audio.transcriptions.create(
            model=model_id,
            file=f
        )

    print(result.text)

print(f"\nDone — transcribed {len(audio_files)} file(s).")
