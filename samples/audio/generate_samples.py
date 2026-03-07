"""
Generate sample WAV files for Part 9 — Whisper Voice Transcription lab.

Scenarios are based on Zava DIY products (ProGrip Drill, UltraSmooth Paint,
TitanLock Tool Chest, EcoBoard Decking, BrightBeam LED Work Light).

Uses pyttsx3 (Windows SAPI5) for offline text-to-speech synthesis.
Run:  python samples/audio/generate_samples.py
"""

import os
import pyttsx3

OUTPUT_DIR = os.path.dirname(os.path.abspath(__file__))

# ── Sample scripts ──────────────────────────────────────────────────────────
SAMPLES = [
    {
        "filename": "zava-customer-inquiry.wav",
        "description": "Customer calling about the Zava ProGrip Cordless Drill",
        "text": (
            "Hi, I'm interested in the Zava ProGrip Cordless Drill. "
            "I've got a big deck-building project coming up this summer and I need "
            "something that can handle driving screws into hardwood all day long. "
            "Can you tell me about the torque and battery life? "
            "Also, does it come with a carrying case? "
            "I saw it has a brushless motor which sounds great for durability."
        ),
    },
    {
        "filename": "zava-product-review.wav",
        "description": "Customer reviewing the Zava UltraSmooth Interior Paint",
        "text": (
            "I just finished painting my living room with Zava UltraSmooth Interior Paint "
            "and I have to say, I'm really impressed. "
            "The coverage was amazing. I only needed one coat to completely cover the old colour. "
            "It dried to the touch in about thirty minutes, just like they said. "
            "The low V-O-C formula meant there was barely any smell, "
            "which was great because I have two young kids at home. "
            "I went with the Coastal Blue shade and it looks absolutely beautiful. "
            "Cleanup was easy too, just soap and water. Highly recommend."
        ),
    },
    {
        "filename": "zava-support-call.wav",
        "description": "Customer support call about the Zava TitanLock Tool Chest",
        "text": (
            "Hello, I'm calling about my Zava TitanLock Tool Chest. "
            "I purchased it about three months ago and overall it's been fantastic. "
            "The ball-bearing drawer slides are super smooth and I love the built-in power strip. "
            "However, I seem to have lost one of the tubular keys for the lock. "
            "Is it possible to order a replacement key? "
            "Also, I wanted to ask if you sell additional foam drawer liners separately. "
            "I'd like to line the top compartment as well."
        ),
    },
    {
        "filename": "zava-project-planning.wav",
        "description": "A DIYer planning a backyard project with Zava EcoBoard Decking",
        "text": (
            "Okay, so for the backyard project I'm thinking about using the Zava EcoBoard "
            "Composite Decking in the Driftwood Grey colour. "
            "The deck area is going to be roughly twelve feet by sixteen feet. "
            "Each board is five and a half inches wide by sixteen feet long, "
            "so I'll need about twenty-six boards to cover the full area. "
            "I really like that it's made from ninety-five percent recycled materials "
            "and I won't need to seal or stain it every year like my old timber deck. "
            "Plus the hidden fastener grooves should give it a really clean look. "
            "I'll need to pick up some Zava BrightBeam work lights too so I can work in the evenings."
        ),
    },
    {
        "filename": "zava-workshop-setup.wav",
        "description": "Describing a complete Zava workshop setup",
        "text": (
            "Let me walk you through my workshop setup using all Zava products. "
            "First, I've got the Zava TitanLock Tool Chest right in the centre. "
            "It keeps everything organised and the rolling casters make it easy to move around. "
            "On top of the chest I keep my Zava ProGrip Cordless Drill with both batteries charged. "
            "For lighting, I have two Zava BrightBeam LED Work Lights on tripods. "
            "They put out five thousand lumens each, so the whole garage is lit up like daylight. "
            "I also keep a gallon of Zava UltraSmooth Paint on the shelf for touch-ups. "
            "And out the back door, you can see the deck I built last summer "
            "with Zava EcoBoard Composite Decking in Midnight Walnut. "
            "The whole setup cost less than I expected and everything is built to last."
        ),
    },
]


def _make_engine():
    """Create and configure a fresh pyttsx3 engine instance."""
    engine = pyttsx3.init()
    engine.setProperty("rate", 160)
    engine.setProperty("volume", 0.95)
    voices = engine.getProperty("voices")
    for voice in voices:
        if "english" in voice.name.lower() and (
            "united states" in voice.name.lower() or "david" in voice.name.lower()
        ):
            engine.setProperty("voice", voice.id)
            return engine, voice.name
    if voices:
        engine.setProperty("voice", voices[0].id)
        return engine, voices[0].name
    return engine, "default"


def generate_wav_files():
    """Generate WAV files from the sample scripts using pyttsx3.

    A fresh engine is created for each file to avoid the pyttsx3 hang
    that can occur with consecutive save_to_file + runAndWait calls.
    """
    print(f"Output directory: {OUTPUT_DIR}\n")

    for i, sample in enumerate(SAMPLES, 1):
        filepath = os.path.join(OUTPUT_DIR, sample["filename"])

        # Skip if already generated
        if os.path.exists(filepath) and os.path.getsize(filepath) > 0:
            size_kb = os.path.getsize(filepath) / 1024
            print(f"[{i}/{len(SAMPLES)}] SKIP (exists): {sample['filename']} ({size_kb:.0f} KB)")
            continue

        engine, voice_name = _make_engine()
        print(f"[{i}/{len(SAMPLES)}] Generating: {sample['filename']}")
        print(f"  Voice:    {voice_name}")
        print(f"  Scenario: {sample['description']}")

        engine.save_to_file(sample["text"], filepath)
        engine.runAndWait()
        engine.stop()
        del engine

        if os.path.exists(filepath):
            size_kb = os.path.getsize(filepath) / 1024
            print(f"  Created:  {size_kb:.0f} KB\n")
        else:
            print(f"  WARNING: File was not created!\n")

    print("Done! Generated {0} sample WAV files.".format(len(SAMPLES)))


if __name__ == "__main__":
    generate_wav_files()
