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

# Step 1: Start the Foundry Local service
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Step 2: Check if the model is already downloaded
cached = manager.list_cached_models()
catalog_info = manager.get_model_info(model_alias)
is_cached = any(m.id == catalog_info.id for m in cached) if catalog_info else False

if is_cached:
    print(f"Model already downloaded: {model_alias}")
else:
    print(f"Downloading model: {model_alias} (this may take several minutes)...")
    def on_progress(progress):
        bar_width = 30
        filled = int(progress / 100 * bar_width)
        bar = "\u2588" * filled + "\u2591" * (bar_width - filled)
        sys.stdout.write(f"\rDownloading: [{bar}] {progress:.1f}%")
        if progress >= 100:
            sys.stdout.write("\n")
        sys.stdout.flush()
    manager.download_model(model_alias, progress_callback=on_progress)
    print(f"Download complete: {model_alias}")

# Step 3: Load the model into memory
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)
model_id = manager.get_model_info(model_alias).id
print(f"Model loaded: {model_id}")

# Step 4: Create the OpenAI client pointing to the local service
client = openai.OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key
)

# Step 5: Transcribe each audio file
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
