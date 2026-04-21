# ಚೇಣ್‌ಲೋಗ್ — ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಕಾರ್ಯಾಗಾರ

ಈ ಕಾರ್ಯಾಗಾರಕ್ಕೆ ಸಂಬಂಧಿಸಿದ ಎಲ್ಲಾ ಮುಖ್ಯ ಬದಲಾವಣೆಗಳು ಕೆಳಗಿವೆ ದಾಖಲಿಸಲಾಗಿದೆ.

---

## 2026-03-11 — ಭಾಗ 12 & 13, ವೆಬ್ UI, ವಿಶ್ಪರ್ ಮರುಲೇಖನ, WinML/QNN ತಿದ್ದಣೆ, ಮತ್ತು ಪರಿಶೀಲನೆ

### ಸೇರಿಸಲಾಗಿದೆ
- **ಭಾಗ 12: ಜವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್‌ಗೆ ವೆಬ್ UI ನಿರ್ಮಾಣ** — ಪ್ರಾಯೋಗಿಕ ಪಾಠಕ್ರಮ (`labs/part12-zava-ui.md`) ಸ್ಟ್ರೀಮಿಂಗ್ NDJSON, ಬ್ರೌಸರ್ `ReadableStream`, ಲೈವ್ ಏಜೆಂಟ್ ಸ್ಥಿತಿ ಬ್ಯಾಜ್‌ಗಳು, ಮತ್ತು ರಿಯಲ್-ಟೈಮ್ ಲೇಖನ ಪಠ್ಯ ಸ್ಟ್ರೀಮಿಂಗ್ ಆವರಿಸಿರುವ ಅಭ್ಯಾಸಗಳೊಂದಿಗೆ
- **ಭಾಗ 13: ಕಾರ್ಯಾಗಾರ ಪೂರ್ಣಗೊಂಡಿದೆ** — ಎಲ್ಲಾ 12 ಭಾಗಗಳ ಸಮ್ಮಿಶ್ರ ಪಾಠಕ್ರಮ (`labs/part13-workshop-complete.md`), ಹೆಚ್ಚಿನ ತತ್ವಗತ ವಿಚಾರಗಳು ಮತ್ತು ಸಂಪನ್ಮೂಲ ಲಿಂಕ್‌ಗಳು
- **ಜವಾ UI ಮುಂಭಾಗ:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — vseh ಮೂರು ಬ್ಯಾಕ್ಎಂಡ್ಗಳಿಂದ ಬಳಕೆಯಾಗುವ ವಾಂಜನಿಲಿ HTML/CSS/JS ಬ್ರೌಸರ್ ಅಂತರ್ಜಾಲ ಮುಖಾಂತ
- **ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ HTTP ಸರ್ವರ್:** `zava-creative-writer-local/src/javascript/server.mjs` — ಬ್ರೌಸರ್ ಆಧಾರಿತ ಪ್ರವೇಶಕ್ಕಾಗಿ ಪ್ರೇರಕೆ ಕಾರ್ಯನಿರ್ವಹಣೆಯೊಂದಿಗೆ ಹೊಸ ಎಕ್ಸ್‌ಪ್ರೆಸ್-ಶೈಲಿ HTTP ಸರ್ವರ್
- **C# ASP.NET ಕೋರ್ ಬ್ಯಾಕ್ಎಂಡ್:** `zava-creative-writer-local/src/csharp-web/Program.cs` ಮತ್ತು `ZavaCreativeWriterWeb.csproj` — UI ಮತ್ತು ಸ್ಟ್ರೀಮಿಂಗ್ NDJSON ಅನ್ನು ಸೇವಿಸುವ ಸಣ್ಣ API ಯೋಜನೆ
- **ಆಡಿಯೋ ಮಾದರಿ ರಚನೆಕಾರು:** `samples/audio/generate_samples.py` — ಪೈಥಾನ್ ಆಧಾರಿತ TTS ಸ್ಕ್ರಿಪ್ಟ್ `pyttsx3` ಬಳಸಿ ಭಾಗ 9 ಗೆ ಜವಾ ಥೀಮ್‌ಡ್ WAV ಫೈಲ್‌ಗಳು ರಚಿಸಲು
- **ಆಡಿಯೋ ಮಾದರಿ:** `samples/audio/zava-full-project-walkthrough.wav` — ಲಂಬಗೊಳಿಸಿದ ಹೊಸ ಆಡಿಯೋ ಮಾದರಿ ಪರಿವೀಕ್ಷಣೆಗೆ
- **ಪರಿಶೀಲನಾ ಸ್ಕ್ರಿಪ್ಟ್:** `validate-npu-workaround.ps1` — ಎಲ್ಲಾ C# ಮಾದರಿಗಳಲ್ಲಿ NPU/QNN ಕಾರ್ಯಚಟುವಟಿಕೆ ಪರಿಶೀಲಿಸಲು ಸ್ವಯಂಚಾಲಿತ ಪವರ್‌ಶೆಲ್ ಸ್ಕ್ರಿಪ್ಟ್
- **ಮರಿಡ್ ಡಯಾಗ್ರಾಮ್ SVGಗಳು:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML ಕ್ರಾಸ್-ಪ್ಲಾಟ್‌ಫಾರ್ಮ್ ಬೆಂಬಲ:** ಎಲ್ಲಾ 3 C# `.csproj` ಫೈಲ್‌ಗಳು (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ಈಗ ಷರತ್ತಿನ TFM ಮತ್ತು ಪರಸ್ಪರ ವಿಭಿನ್ನ ಪ್ಯಾಕೇಜ್ ಉಲ್ಲೇಖಗಳನ್ನು ಬಳಸುತ್ತಿವೆ. ವಿಂಡೋಸ್‌ನಲ್ಲಿ: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (QNN EP ಪ್ಲಗಿನ್‌ನ್ನು ಒಳಗೊಂಡಿರುವ ಸೂಪರ್‌ಸೆಟ್). ವಿಂಡೋಸ್ ಅಲ್ಲದ: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (ಮೂಲ SDK). Zava ಯೋಜನೆಗಳಲ್ಲಿ ಹಾರ್ಡ್ಕೋಡ್ ಮಾಡಿದ `win-arm64` RIDನ್ನು ಸ್ವಯಂಗುರುತಿಸುವಾಗಿ ಬದಲಾಯಿಸಲಾಗಿದೆ. ಟ್ರಾನ್ಸಿಟಿವ್ ಅವಲಂಬನೆ ಪರಿಹಾರ ಶಾಸ್ತ್ರೀಯವಾಗಿ `Microsoft.ML.OnnxRuntime.Gpu.Linux` ನಿಂದ ಸ್ವದೇಶಿ ಆಸ್ತಿಗಳನ್ನು ಹೊರಗಿಟ್ಟು win-arm64 ಉಲ್ಲೇಖದ ದೋಷವನ್ನು ಸರಿಪಡಿಸಲಾಗಿದೆ. ಮೊದಲಿನ try/catch NPU ಪರಿಹಾರವನ್ನು ಎಲ್ಲಾ 7 C# ಫೈಲ್‌ಗಳಿಂದ ತೆಗೆದುಹಾಕಲಾಗಿದೆ.

### ಬದಲಾವಣೆಗೊಂಡಿದೆ
- **ಭಾಗ 9 (ವಿಶ್ಪರ್):** ಪ್ರಮುಖ ಮರುಲೇಖನ — ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಈಗ SDKಯ ನಿರ್ದಿಷ್ಟ `AudioClient` (`model.createAudioClient()`) ಬಳಸಿ, ಕೈಯಿಂದ ONNX ರನ್‌ಟೈಮ್ ನಿರ್ಣಯದ ಬದಲು; ವಾಸ್ತುಶಿಲ್ಪ ವಿವರಣೆಗಳು, ಹೋಲಿಕೆ सारಣಿಗಳು, ಮತ್ತು পাইಪ್ಲೈನ್ ಡಯಾಗ್ರಾಮ್‌ಗಳು JS/C# `AudioClient` ರೀತಿಯನ್ನು Python ONNX ರನ್‌ಟೈಮ್ ವಿಧಾನಕ್ಕೆ ಹೋಲಿಕೆಯಾದ್ದರಿಂದ ನವೀಕರಿಸಲಾಗಿದೆ
- **ಭಾಗ 11:** ನವೀಕರಿಸಿದ ನಾವಿಗೇಶನ್ ಲಿಂಕ್‌ಗಳು (ಈಗ ಭಾಗ 12 ಗೆ ಸೂಚಿಸುತ್ತದೆ); ಉಪಕರಣ ಕರೆಗಳ ಪ್ರವಾಹ ಮತ್ತು ಕ್ರಮಗಳನ್ನು ಹೊಂದಿರುವ SVG ಡಯಾಗ್ರಾಮ್‌ಗಳನ್ನು ಸೇರಿಸಲಾಗಿದೆ
- **ಭಾಗ 10:** ಕಾರ್ಯಾಗಾರ ಮುಕ್ತಾಯಗೊಳ್ಳದೇ ಭಾಗ 12 ಮೂಲಕ ಮಾರ್ಗ ನಿರ್ದೇಶಿಸುವಂತೆ ನವೀಕರಿಸಲಾಗಿದೆ
- **ಪೈಥಾನ್ ವಿಶ್ಪರ್ (`foundry-local-whisper.py`):** ಹೆಚ್ಚಿನ ಆಡಿಯೋ ಮಾದರಿ ಮತ್ತು ಉತ್ತಮ ದೋಷ ನಿರ್ವಹಣೆಗಾಗಿ ವಿಸ್ತರಿಸಲಾಗಿದೆ
- **ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ವಿಶ್ಪರ್ (`foundry-local-whisper.mjs`):** ಕೈಯಿಂದ ONNX ರನ್‌ಟೈಮ್ ಸೆಷನ್‌ಗಳ ಬದಲು `model.createAudioClient()` ಜೊತೆಗೆ `audioClient.transcribe()` ಬಳಕೆಗಾಗಿ ಪುನರರೆಖನ
- **ಪೈಥಾನ್ ಫಾಸ್ಟ್API (`zava-creative-writer-local/src/api/main.py`):** API ಜೊತೆಗೆ ಸ್ಥೈರ್ಯ UI ಫೈಲ್‌ಗಳನ್ನು ಸೇವಿಸಲು ನವೀಕರಣ
- **ಜವಾ C# ಕಾನ್ಸೋಲ್ (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU ತಿದ್ದುಪಡಿ ತೆಗೆದುಹಾಕಿಕೆ (ಈಗ WinML ಪ್ಯಾಕೇಜ್ ಮೂಲಕ ನಿರ್ವಹಿಸಲಾಗುತ್ತದೆ)
- **README.md:** ಭಾಗ 12 ವಿಭಾಗ ಸೇರಿಸಲಾಗಿದೆ ಕೋಡ್ ಮಾದರಿ सारಣಿಗಳೊಂದಿಗೆ ಮತ್ತು ಬ್ಯಾಕ್ಎಂಡ್ ಸೇರಿಕೆ; ಭಾಗ 13 ವಿಭಾಗ ಸೇರಿಸಲಾಗಿದೆ; ಅಧ್ಯಯನ ಉದ್ದೇಶಗಳು ಮತ್ತು ಯೋಜನೆ ರಚನೆ ನವೀಕರಿಸಲಾಗಿದೆ
- **KNOWN-ISSUES.md:** Issue #7 (C# SDK NPU ಮಾದರಿ ಬದಲೆ — ಈಗ WinML ಪ್ಯಾಕೇಜ್ ಮೂಲಕ ನಿಯಂತ್ರಣ) ತೆಗೆದುಹಾಕಲಾಗಿದೆ. ಉಳಿದ ಸಮಸ್ಯೆಗಳನ್ನು #1–#6 ಗೆ ನೂತನ ಸಂಖ್ಯಾಕರಣೆ. ಪರಿಸರ ವಿವರ .NET SDK 10.0.104 ನೊಂದಿಗೆ ನವೀಕರಿಸಲಾಗಿದೆ
- **AGENTS.md:** ಹೊಸ `zava-creative-writer-local` ಸೇರಿಕೆಗಳೊಂದಿಗೆ ಯೋಜನೆ ರಚನೆ ಮರ, C# ಪ್ರಮುಖ ಪ್ಯಾಕೇಜ್‌ಗಳು ಮತ್ತು ಷರತ್ತಿನ TFM ವಿವರಗಳ ನವೀಕರಣ
- **labs/part2-foundry-local-sdk.md:** ಪೂರ್ಣ ಕ್ರಾಸ್-ಪ್ಲಾಟ್‌ಫಾರ್ಮ್ ಮಾದರಿಯನ್ನು Conditional TFM, ಪರಸ್ಪರ ವಿಭಿನ್ನ ಪ್ಯಾಕೇಜ್ ಉಲ್ಲೇಖಗಳೊಂದಿಗೆ ಮತ್ತು وضحي ಟಿಪ್ಪಣಿಯೊಂದಿಗೆ ನವೀಕರಿಸಿದ `.csproj` ಉದಾಹರಣೆ

### ದೃಢೀಕರಣಗೊಂಡಿದೆ
- ಎಲ್ಲಾ 3 C# ಯೋಜನೆಗಳು (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) ವಿಂಡೋಸ್ ARM64 ನಲ್ಲಿ ಯಶಸ್ವಿಯಾಗಿ ನಿರ್ಮಾಣ
- ಚಾಟ್ ಮಾದರಿ (`dotnet run chat`): ಮೋಡಲ್ WinML/QNN ಮೂಲಕ `phi-3.5-mini-instruct-qnn-npu:1` ಆಗಿ ಲೋಡ್ ಆಗಿದೆ — NPU ಬದಲೆ CPU ಬದಲಾವಣೆ ಇಲ್ಲದೆ ಲೋಡ್ ಆಗುತ್ತದೆ
- ಏಜೆಂಟ್ ಮಾದರಿ (`dotnet run agent`): ಬಹುಬಲವಾದ ಸಂಭಾಷಣೆಯೊಂದಿಗೆ ಪೂರ್ಣವಾಗಿ ಕಾರ್ಯನಿರ್ವಹಣ, ನಿರ್ಗಮನ ಕೋಡ್ 0
- Foundry Local CLI v0.8.117 ಮತ್ತು SDK v0.9.0 .NET SDK 9.0.312 ಮೇಲೆ

---

## 2026-03-11 — ಕೋಡ್ ತಿದ್ದಣೆಗಳು, ಮಾದರಿ ಶುಚಿಗೊಳಿಕೆ, ಮರಿಡ್ ಡಯಾಗ್ರಾಮ್‌ಗಳು, ಮತ್ತು ಪರಿಶೀಲನೆ

### ತಿದ್ದಿಸಲಾಗಿದೆ
- **ಎಲ್ಲ 21 ಕೋಡ್ ಮಾದರಿಗಳು (7 ಪೈಥಾನ್, 7 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್, 7 C#):** ನಿರ್ಗಮನದಲ್ಲಿ `model.unload()` / `unload_model()` / `model.UnloadAsync()` ಕ್ಲೀನಪ್ ಸೇರಿಸಲಾಗಿದೆ, OGA ಮೆಮೊರಿ ಲೀಕ್ ಎಚ್ಚರಿಕೆಗಳನ್ನು ಪರಿಹರಿಸಲು (ಜ್ಞಾನ ವಾರ್ತೆ #4)
- **csharp/WhisperTranscription.cs:** ಭಂಗುರ `AppContext.BaseDirectory` ಸಂಬಂಧಿತ ಮಾರ್ಗವನ್ನು `FindSamplesDirectory()` ಗಾಗಿ ಬದಲಾಯಿಸಲಾಗಿದೆ, ಅದು ಮೇಲಿನ ಡೈರೆಕ್ಟರಿಗಳನ್ನು ಕಾಲ್ಮೇಲು `samples/audio` ಅನ್ನು ನಿಖರವಾಗಿ ಕಂಡುಹಿಡಿಯುತ್ತದೆ (ಜ್ಞಾನ ವಾರ್ತೆ #7)
- **csharp/csharp.csproj:** ಹಾರ್ಡ್‌ಕೋಡ್ ಮಾಡಿದ `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` ಅನ್ನು ನೈರ್ಮಲ್ಯದಿಂದ ಪತ್ತೆಹಚ್ಚುವ `$(NETCoreSdkRuntimeIdentifier)` ಬಳಸಿ ಬದಲಾಯಿಸಲಾಗಿದೆ, ಆದ್ದರಿಂದ `dotnet run` ಯಾವುದೇ ವೇದಿಕೆಯನ್ನೂ -r ಬ್ಯಾಜಿನಿಲ್ಲದೇ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### ಬದಲಾವಣೆಗೊಂಡಿದೆ
- **ಭಾಗ 8:** ASCII ಬಾಕ್ಸ್ ಡಯಾಗ್ರಾಮ್‌ನಿಂದ ಇವ್ಯಾಲುಯೇಷನ್ ಚಾಲಿತ ಪುನರಾವೃತ್ತಿ ಲೂಪ್ನ್ನು ರೆಂಡರ್ ಮಾಡಿದ SVG ಚಿತ್ರಕ್ಕೆ ಪರಿವರ್ತನೆ
- **ಭಾಗ 10:** ಸಂಯೋಜನೆ ಪೈಪ್‌ಲೈನ್ ಡಯಾಗ್ರಾಮ್ ASCII ಬಾಣಗಳಿಂದ SVG ಚಿತ್ರಕ್ಕೆ ಪರಿವರ್ತನೆ
- **ಭಾಗ 11:** ಉಪಕರಣ-ಕರೆ ಪ್ರವಾಹ ಮತ್ತು ಕ್ರಮ ಡಯಾಗ್ರಾಮ್‌ಗಳನ್ನು SVG ಚಿತ್ರಗಳಿಗೆ ಪರಿವರ್ತನೆ
- **ಭಾಗ 10:** "ಕಾರ್ಯಾಗಾರ ಪೂರ್ಣ!" ವಿಭಾಗವನ್ನು ಭಾಗ 11 (ಕೊನೆಗಿನ ಪಾಠಕ್ರಮ) ಗೆ ಸಾಗಿಸಲಾಗಿದೆ; "ಮುಂದಿನ ಹಂತಗಳು" ಲಿಂಕ್‌ನೊಂದಿಗೆ ಬದಲಾಯಿಸಲಾಗಿದೆ
- **KNOWN-ISSUES.md:** ಎಲ್ಲಾ ಸಮಸ್ಯೆಗಳ CLI v0.8.117 ಗೆ ಪೂರ್ಣ ಮಾತ್ರ ಪೂರಿತ ಪರಿಶೀಲನೆ. ಪರಿಹರಿಸಲಾದವು ಅಳಿಸಲಾಗಿದೆ: OGA ಮೆಮೊರಿ ಲೀಕ್ (ಕ್ಲೀಗಅಪ್ ಸೇರಿಸಲಾಗಿದೆ), Whisper ಮಾರ್ಗ (FindSamplesDirectory), HTTP 500 ನಿರಂತರ ನಿರ್ಣಯ (ಮರುಉತ್ಪತ್ತಿ ಸಾಧ್ಯವಿಲ್ಲ, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice ಮಿತಿ (ಇದೀಗ `"required"` ಮತ್ತು ವಿಶೇಷ ಕಾರ್ಯ ನಿಗದಿಯಾಗಿರುವ qwen2.5-0.5b ಯೊಂದಿಗೆ ಕೆಲಸ ಮಾಡುತ್ತದೆ). JS ವಿಶ್ಪರ್ ಸಮಸ್ಯೆ ನವೀಕರಿಸಲಾಗಿದೆ — ಈಗ ಎಲ್ಲಾ ಫೈಲ್‌ಗಳು ಖಾಲಿ/ಬೈನರಿ ಔಟ್‌ಪುಟ್ ನೀಡುತ್ತವೆ (v0.9.x ಯಿಂದ ಹಿಂಬಾಗು, ಗಂಭೀರತೆ ಮುಖ್ಯವಾಗಿದೆ). #4 C# RID ನವೀಕರಿಸಿದ ಸ್ವಯಂಗುರುತ ಪಧ್ಧತಿ ಮತ್ತು [#497](https://github.com/microsoft/Foundry-Local/issues/497) ಲಿಂಕ್ ಸೇರಿವೆ. 7 ತೆರಚಿನ ಸಮಸ್ಯೆಗಳು ಉಳಿದಿವೆ.
- **javascript/foundry-local-whisper.mjs:** ಕ್ಲೀನಪ್ ವೈರಿಯಬಲ್ ಹೆಸರು ಸರಿಪಡಿಸಲಾಗಿದೆ (`whisperModel` → `model`)

### ದೃಢೀಕರಣಗೊಂಡಿದೆ
- ಪೈಥಾನ್: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — ಕ್ಲೀನಪ್ ಜೊತೆ ಯಶಸ್ವಿಯಾಗಿ ಕಾರ್ಯ
- ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — ಕ್ಲೀನಪ್ ಜೊತೆ ಯಶಸ್ವಿಯಾಗಿ ಕಾರ್ಯ
- C#: `dotnet build` 0 ಎಚ್ಚರಿಕೆಗಳು, 0 ದೋಷಗಳೊಂದಿಗೆ ಯಶಸ್ವಿ (net9.0 ಗುರಿ)
- ಎಲ್ಲಾ 7 ಪೈಥಾನ್ ಫೈಲ್‌ಗಳು `py_compile` ವ್ಯಾಕರಣ ಪರಿಶೀಲನೆ ಪಾಸು
- ಎಲ್ಲಾ 7 ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಫೈಲ್‌ಗಳು `node --check` ವ್ಯಾಕರಣ ಪರಿಶೀಲನೆ ಪಾಸು

---

## 2026-03-10 — ಭಾಗ 11: ಉಪಕರಣ ಕರೆದೊಯ್ಯುವುದು, SDK API ವಿಸ್ತರಣೆ, ಮತ್ತು ಮಾದರಿ ವ್ಯಾಪ್ತಿಯು

### ಸೇರಿಸಲಾಗಿದೆ
- **ಭಾಗ 11: ಸ್ಥಳೀಯ ಮಾದರಿಗಳೊಂದಿಗೆ ಉಪಕರಣ ಕರೆದೊಯ್ಯುವುದು** — 8 ಅಭ್ಯಾಸಗಳೊಂದಿಗೆ ಹೊಸ ಪಾಠಕ್ರಮ (`labs/part11-tool-calling.md`), ಉಪಕರಣ schemas, ಬಹುಬಲವಾದ ಪ್ರವಾಹ, ಬಹು ಉಪಕರಣ ಕರೆಯುವಿಕೆ, ಕಸ್ಟಮ್ ಉಪಕರಣಗಳು, ChatClient ಉಪಕರಣ ಕರೆ, ಮತ್ತು `tool_choice`
- **ಪೈಥಾನ್ ಮಾದರಿ:** `python/foundry-local-tool-calling.py` — OpenAI SDK ಬಳಸಿಕೊಂಡು `get_weather`/`get_population` ಉಪಕರಣಗಳು
- **ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮಾದರಿ:** `javascript/foundry-local-tool-calling.mjs` — SDK ಮೂಲ `ChatClient` (`model.createChatClient()`) ಬಳಸಿ ಉಪಕರಣ ಕರೆದೊಯ್ಯುವಿಕೆ
- **C# ಮಾದರಿ:** `csharp/ToolCalling.cs` — OpenAI C# SDK ಬಳಸಿ `ChatTool.CreateFunctionTool()` ಉಪಕರಣ ಕರೆ
- **ಭಾಗ 2, ಅಭ್ಯಾಸ 7:** ಮೂಲ `ChatClient` — JSನಲ್ಲಿ `model.createChatClient()` ಮತ್ತು C# ನಲ್ಲಿ `model.GetChatClientAsync()` OpenAI SDK ಗೆ ಬದಲಿ
- **ಭಾಗ 2, ಅಭ್ಯಾಸ 8:** ಮಾದರಿ ಬದಲಾವಣೆಗಳು ಮತ್ತು ಹಾರ್ಡ್‌ವೇರ್ ಆಯ್ಕೆ — `selectVariant()`, `variants`, 7 ಮಾದರಿಗಳ NPU ಬದಲಿ ಟೇಬಲ್
- **ಭಾಗ 2, ಅಭ್ಯಾಸ 9:** ಮಾದರಿ ಅಪ್‌ಗ್ರೇಡ್ ಮತ್ತು ಕ್ಯಾಟಲಾಗ್ ನವೀಕರಣ — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **ಭಾಗ 2, ಅಭ್ಯಾಸ 10:** ತರ್ಕ ಮಾದರಿಗಳು — `<think>` ಟ್ಯಾಗ್ ಪಾರ್ಸಿಂಗ್ ಉದಾಹರಣೆಗಳೊಂದಿಗೆ `phi-4-mini-reasoning`
- **ಭಾಗ 3, ಅಭ್ಯಾಸ 4:** OpenAI SDKಗೆ ಬದಲಿ `createChatClient` ಜೊತೆಗೆ ಸ್ಟ್ರೀಮಿಂಗ್ ಕಾಲ್ಬ್ಯಾಕ್ ಪ್ಯಾಟ್‌ರನ್ ಡಾಕ್ಯುಮೆಂಟೇಶನ್
- **AGENTS.md:** ಉಪಕರಣ ಕರೆದೊಯ್ಯುವುದು, ChatClient, ಮತ್ತು ತರ್ಕ ಮಾದರಿಗಳ ಕೋಡಿಂಗ್ ಪರಂಪರೆ ಸೇರಿಸಲಾಗಿದೆ

### ಬದಲಾವಣೆಗೊಂಡಿದೆ
- **ಭಾಗ 1:** ಮಾದರಿ ಕ್ಯಾಟಲಾಗ್ ವಿಸ್ತರಣೆ — phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo ಸೇರಿಸಲಾಗಿದೆ
- **ಭಾಗ 2:** API ಉಲ್ಲೇಖ ಸಂರಚನೆ ತರಕಳು ವಿಸ್ತರಣೆ — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` ಸೇರಿಸಲಾಗಿದೆ
- **ಭಾಗ 2:** ಅಭ್ಯಾಸ ಸಂಖ್ಯೆಗಳನ್ನು 7-9 ರಿಂದ 10-13 ಗೆ ಮರುಸಂಖ್ಯಾಕರೆ
- **ಭಾಗ 3:** ಮೂಲ ChatClient ಸೇರಿಸಿದ ರುಜುಗಳು ಟೇಬಲ್ ನವೀಕರಣ
- **README.md:** ಭಾಗ 11 ವಿಭಾಗ ಕೋಡ್ ಮಾದರಿ ಟೇಬಲ್ ಜೊತೆಗೆ ಸೇರಿಸಲಾಗಿದೆ; ಅಧ್ಯಯನ ಉದ್ದೇಶ #11 ಸೇರಿದೆ; ಯೋಜನೆ ರಚನೆ ಮರ ನವೀಕರಣ
- **csharp/Program.cs:** CLI ಮಾರ್ಗದರ್ಶಕದಲ್ಲಿ `toolcall` ಪ್ರಕರಣ ಸೇರಿಸಲಾಗಿದೆ ಮತ್ತು ಸಹಾಯ ಪಠ್ಯ ನವೀಕರಣ

---

## 2026-03-09 — SDK v0.9.0 ನವೀಕರಣ, ಬ್ರಿಟಿಷ್ ಇಂಗ್ಲೀಷ್, ಮತ್ತು ಪರಿಶೀಲನೆಗೊಳಿಸುವ ಪ್ರಕ್ರಿಯೆ

### ಬದಲಾವಣೆಗೊಂಡಿದೆ
- **ಎಲ್ಲ ಕೋಡ್ ಮಾದರಿಗಳು (ಪೈಥಾನ್, ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್, C#):** Foundry Local SDK v0.9.0 API ಗೆ ನವೀಕರಿಸಲಾಗಿದೆ — `await catalog.getModel()` ದೋಷ ಸರಿಪಡಿಸಲಾಗಿದೆ (ಹಿಂದೆ `await` ಇಲ್ಲದಿತ್ತು), `FoundryLocalManager` ಪ್ರಾರಂಭ ಮಾದರಿಗಳು, ಎಂಡಪಾಯಿಂಟ್ ಪತ್ತೆ ಹಿಂಪಡೆಯಲಾಗಿದೆ
- **ಎಲ್ಲಾ ಪಾಠಕ್ರಮ ಮಾರ್ಗದರ್ಶಿಗಳು (ಭಾಗ 1-10):** ಬ್ರಿಟಿಷ್ ಇಂಗ್ಲೀಷ್ ಗೆ ಪರಿವರ್ತನೆ (colour, catalogue, optimised ಇತ್ಯಾದಿ)
- **ಎಲ್ಲಾ ಪಾಠ ಕ್ರಮ ಗೈಡ್‌ಗಳು:** SDK ಕೋಡ್ ಉದಾಹರಣೆಗಳನ್ನು v0.9.0 API ಸರ್ಫೇಸ್ ಗೆ ನವೀಕರಿಸಲಾಗಿದೆ
- **ಎಲ್ಲಾ ಪಾಠ ಕ್ರಮ ಗೈಡ್‌ಗಳು:** API ಉಲ್ಲೇಖ ಟೇಬಲ್‌ಗಳು ಮತ್ತು ಅಭ್ಯಾಸ ಕೋಡ್ ಬ್ಲಾಕ್‌ಗಳು ನವೀಕರಣ
- **ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಪ್ರಮುಖ ತಿದ್ದಣೆ:** `catalog.getModel()` ಮೇಲೆ ತಪ್ಪಿದ `await` ಸೇರಿಸಲಾಗಿದೆ — `Promise` ಹಿಂದಿರುಗಿಸುತ್ತಿತ್ತು, `Model` ವಸ್ತುವು ಅಲ್ಲ, ಇದು ನಿರ್ಬಂಧಗೊಳ್ಳದ ವೈಫಲ್ಯಗಳನ್ನುಂಟುಮಾಡಿತು

### ದೃಢೀಕರಣಗೊಂಡಿದೆ
- ಎಲ್ಲಾ ಪೈಥಾನ್ ಮಾದರಿಗಳು Foundry Local ಸೇವೆಯ ಮೇಲೆ ಯಶಸ್ವಿಯಾಗಿ ಚಾಲನೆ
- ಎಲ್ಲಾ ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮಾದರಿಗಳು ಯಶಸ್ವಿಯಾಗಿ (Node.js 18+)
- C# ಯೋಜನೆ .NET 9.0 ರಲ್ಲಿ ನಿರ್ಮಾಣ ಮತ್ತು ಚಾಲನೆ (net8.0 SDK ಅಸೆಂಬ್ಲಿಯಿಂದ ಮುಂದುವರಿದ)
- ಕಾರ್ಯಾಗಾರದಲ್ಲಿ 29 ಫೈಲ್‌ಗಳು ಪರಿಷ್ಕೃತವಾಗಿ ಪರಿಶೀಲನೆಗೊಳಿಸಲಾಗಿದೆ

---

## ಫೈಲ್ ಸೂಚಿ

| ಫೈಲ್ | ಕೊನೆಯ ಅಪ್ಡೇಟ್ | ವಿವರಣೆ |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | ಮಾದರಿ ಕ್ಯಾಟಲಾಗ್ ವಿಸ್ತರಣೆ |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | ಹೊಸ ಅಭ್ಯಾಸಗಳು 7-10, API ಟೇಬಲ್ ವಿಸ್ತರಣೆ |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | ಹೊಸ ಅಭ್ಯಾಸ 4 (ChatClient), ತೆಗೆದ ತತ್ವಗಳು |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + ಬ್ರಿಟಿಷ್ ಇಂಗ್ಲೀಷ್ |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + ಬ್ರಿಟಿಷ್ ಇಂಗ್ಲೀಷ್ |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + ಬ್ರಿಟಿಷ್ ಇಂಗ್ಲಿಷ್ |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + ಬ್ರಿಟಿಷ್ ಇಂಗ್ಲಿಷ್ |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | ಮೆರ್ಮೇಡ್ ಡಯಾಗ್ರಾಂ |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + ಬ್ರಿಟಿಷ್ ಇಂಗ್ಲಿಷ್ |
| `labs/part10-custom-models.md` | 2026-03-11 | ಮೆರ್ಮೇಡ್ ಡಯಾಗ್ರಾಂ, ವರ್ಕ್‌ಶಾಪ್ ಪೂರ್ಣಗೊಂಡಿದೆ ಎಂದನ್ನು ಭಾಗ 11ಕ್ಕೆ ಸ್ಥಳಾಂತರಿಸಲಾಗಿದೆ |
| `labs/part11-tool-calling.md` | 2026-03-11 | ಹೊಸ ಪ್ರಯೋಗಾಲಯ, ಮೆರ್ಮೇಡ್ ಡಯಾಗ್ರಾಂಗಳು, ವರ್ಕ್‌ಶಾಪ್ ಪೂರ್ಣಗೊಂಡಿದೆ ವಿಭಾಗ |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | ಹೊಸದು: ಟೂಲ್ ಕಾಲಿಂಗ್ ಮಾದರಿ |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | ಹೊಸದು: ಟೂಲ್ ಕಾಲಿಂಗ್ ಮಾದರಿ |
| `csharp/ToolCalling.cs` | 2026-03-10 | ಹೊಸದು: ಟೂಲ್ ಕಾಲಿಂಗ್ ಮಾದರಿ |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI ಆಜ್ಞೆಯನ್ನು ಸೇರಿಸಲಾಗಿದೆ |
| `README.md` | 2026-03-10 | ಭಾಗ 11, ಪ್ರಾಜೆಕ್ಟ್ ರಚನೆ |
| `AGENTS.md` | 2026-03-10 | ಟೂಲ್ ಕಾಲಿಂಗ್ + ಚಾಟ್ ಕ್ಲೈಯಂಟ್ ನಿಯಮಗಳು |
| `KNOWN-ISSUES.md` | 2026-03-11 | ಪರಿಹರಿಸಿದ ಸಮಸ್ಯೆ #7 ನೋತದಿದ್ದು, 6 ತೆರೆಯಲಾದ ಸಮಸ್ಯೆಗಳು ಉಳಿದಿವೆ |
| `csharp/csharp.csproj` | 2026-03-11 | کراس-ಪ್ಲಾಟ್‌ಫಾರ್ಮ್ TFM, WinML/ಬೇಸ್ SDK ನಿರ್ದಿಷ್ಟ ಉಲ್ಲೇಖಗಳು |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | کراس-ಪ್ಲಾಟ್‌ಫಾರ್ಮ್ TFM, ಸ್ವಯಂ-ಗುರುತುಮಾಡುವ RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | کراس-ಪ್ಲಾಟ್‌ಫಾರ್ಮ್ TFM, ಸ್ವಯಂ-ಗುರುತುಮಾಡುವ RID |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU ಪ್ರಯತ್ನ/ಹೇಳಿಕೊಳ್ಳುವ ಕೆಲಸ ತೆಗೆದುಹಾಕಲಾಗಿದೆ |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU ಪ್ರಯತ್ನ/ಹೇಳಿಕೊಳ್ಳುವ ಕೆಲಸ ತೆಗೆದುಹಾಕಲಾಗಿದೆ |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU ಪ್ರಯತ್ನ/ಹೇಳಿಕೊಳ್ಳುವ ಕೆಲಸ ತೆಗೆದುಹಾಕಲಾಗಿದೆ |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU ಪ್ರಯತ್ನ/ಹೇಳಿಕೊಳ್ಳುವ ಕೆಲಸ ತೆಗೆದುಹಾಕಲಾಗಿದೆ |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU ಪ್ರಯತ್ನ/ಹೇಳಿಕೊಳ್ಳುವ ಕೆಲಸ ತೆಗೆದುಹಾಕಲಾಗಿದೆ |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | کراس-ಪ್ಲಾಟ್‌ಫಾರ್ಮ್ .csproj ಉದಾಹರಣೆ |
| `AGENTS.md` | 2026-03-11 | C# ಪ್ಯಾಕೇಜುಗಳು ಮತ್ತು TFM ವಿವರಗಳನ್ನು ನವೀಕರಿಸಲಾಗಿದ್ದು |
| `CHANGELOG.md` | 2026-03-11 | ಈ ಫೈಲ್ |