# ਚੇਂਜਲੌਗ — ਫਾਉਂਡਰੀ ਲੋਕਲ ਵਰਕਸ਼ਾਪ

ਇਸ ਵਰਕਸ਼ਾਪ ਵਿੱਚ ਸਾਰੇ ਪ੍ਰਮੁੱਖ ਬਦਲਾਵ ਹੇਠਾਂ ਦਰਜ ਕੀਤੇ ਗਏ ਹਨ।

---

## 2026-03-11 — ਹਿੱਸਾ 12 & 13, ਵੈੱਬ ਯੂਆਈ, ਵਿਸਪਰ ਰੀਰਾਈਟ, WinML/QNN ਫਿਕਸ, ਅਤੇ ਵੈਰੀਫਿਕੇਸ਼ਨ

### ਸ਼ਾਮਲ ਕੀਤਾ
- **ਹਿੱਸਾ 12: ਜਾਵਾ ਕ੍ਰੀਏਟਿਵ ਰਾਈਟਰ ਲਈ ਵੈੱਬ ਯੂਆਈ ਬਣਾਉਣਾ** — ਨਵਾਂ ਲੈਬ ਗਾਈਡ (`labs/part12-zava-ui.md`) ਜਿਸ ਵਿੱਚ ਸਟ੍ਰੀਮਿੰਗ NDJSON, ਬ੍ਰਾਉਜ਼ਰ `ReadableStream`, ਲਾਈਵ ਏਜੰਟ ਸਥਿਤੀ ਬੈਜ, ਅਤੇ ਰੀਅਲ-ਟਾਈਮ ਲੇਖ ਟੈਕਸਟ ਸਟ੍ਰੀਮਿੰਗ ਦੇ ਅਭਿਆਸ ਸ਼ਾਮਲ ਹਨ
- **ਹਿੱਸਾ 13: ਵਰਕਸ਼ਾਪ ਮੁਕੰਮਲ** — ਨਵਾਂ ਸਮਰੀ ਲੈਬ (`labs/part13-workshop-complete.md`) ਜਿਸ ਵਿੱਚ ਸਾਰੇ 12 ਹਿੱਸਿਆਂ ਦੀ ਪੁਨਰਾਵਲੋਕਨ, ਅਤਿਰਿਕਤ ਵਿਚਾਰ ਅਤੇ ਸਰੋਤ ਲਿੰਕ ਹਨ
- **ਜ਼ਾਵਾ ਯੂਆਈ ਫਰੰਟ ਐਂਡ:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — ਸਾਂਝਾ ਵੈਨਿੱਲਾ HTML/CSS/JS ਬ੍ਰਾਉਜ਼ਰ ਇੰਟਰਫੇਸ ਜੋ ਤਿੰਨਾਂ ਬੈਕਐਂਡ ਦੁਆਰਾ ਖਪਤ ਕੀਤਾ ਜਾਂਦਾ ਹੈ
- **ਜਾਵਾਸਕ੍ਰਿਪਟ HTTP ਸਰਵਰ:** `zava-creative-writer-local/src/javascript/server.mjs` — ਨਵਾਂ ਐਕਸਪ੍ਰੈਸ-ਸਟਾਈਲ HTTP ਸਰਵਰ ਜੋ ਬ੍ਰਾਉਜ਼ਰ-ਆਧਾਰਿਤ ਐਕਸੈਸ ਲਈ ਪਰਚਾਲਕ ਨੂੰ ਰੈਪ ਕਰਦਾ ਹੈ
- **C# ASP.NET ਕੋਰ ਬੈਕਐਂਡ:** `zava-creative-writer-local/src/csharp-web/Program.cs` ਅਤੇ `ZavaCreativeWriterWeb.csproj` — ਨਵਾਂ ਨਿਊਨਤਮ API ਪਰਿਯੋਜਨਾ ਜੋ ਯੂਆਈ ਅਤੇ ਸਟ੍ਰੀਮਿੰਗ NDJSON ਸੇਵਾ ਦਿੰਦੀ ਹੈ
- **ਆਡੀਓ ਸੈਂਪਲ ਜਨਰੇਟਰ:** `samples/audio/generate_samples.py` — `pyttsx3` ਵਰਤ ਕੇ ਬੰਦ ਗਈ ਟੀਟੀਐਸ ਸਕ੍ਰਿਪਟ ਜੋ ਹਿੱਸਾ 9 ਲਈ ઝાવા ਥੀਮ ਵਾਲੀਆਂ WAV ਫਾਈਲਾਂ ਜਨਰੇਟ ਕਰਦੀ ਹੈ
- **ਆਡੀਓ ਨਮੂਨਾ:** `samples/audio/zava-full-project-walkthrough.wav` — ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਟੈਸਟਿੰਗ ਲਈ ਨਵਾਂ ਲੰਮਾ ਆਡੀਓ ਸੈਂਪਲ
- **ਵੈਰੀਫਿਕੇਸ਼ਨ ਸਕ੍ਰਿਪਟ:** `validate-npu-workaround.ps1` — ਸਾਰੇ C# ਨਮੂਨਿਆਂ ਵਿੱਚ NPU/QNN ਵਰਕਅਰਾਊਂਡ ਨੂੰ ਵੈਰੀਫਾਈ ਕਰਨ ਲਈ ਆਟੋਮੇਟਿਕ ਪਾਵਰਸ਼ੈੱਲ ਸਕ੍ਰਿਪਟ
- **ਮਰਮੈਡ ਡਾਇਗਰਾਮ SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML ਕਰਾਸ-ਪਲੇਟਫਾਰਮ ਸਹਿਯੋਗ:** ਸਾਰੇ 3 C# `.csproj` ਫਾਈਲਾਂ (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ਹੁਣ ਕਰਾਸ-ਪਲੇਟਫਾਰਮ ਸਹਿਯੋਗ ਲਈ ਸ਼ਰਤੀ TFM ਅਤੇ ਆਪਸੀ ਅਸਮਰਥ ਪੈਕੇਜ ਰੈਫਰੰਸਜ਼ ਦੀ ਵਰਤੋਂ ਕਰਦੀਆਂ ਹਨ। ਵਿੰਡੋਜ਼ 'ਤੇ: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (ਸਪerset ਜਿਸ ਵਿੱਚ QNN EP ਪਲੱਗਇਨ ਸ਼ਾਮਲ ਹੈ)। ਗੈਰ-ਵਿੰਡੋਜ਼ 'ਤੇ: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (ਮੂਲ SDK)। Zava ਪਰਿਯੋਜਨਾਵਾਂ ਵਿੱਚ ਹਾਰਡਕੋਡ ਕੀਤੀ `win-arm64` RID ਨੂੰ ਆਪਣੇ ਆਪ ਪਛਾਣ ਨਾਲ ਬਦਲ ਦਿੱਤਾ ਗਿਆ। ਇੱਕ ਟ੍ਰਾਂਜ਼ੀਟਿਵ ਡਿਪੇਂਡੇਸੀ ਵਰਕਅਰਾਊਂਡ `Microsoft.ML.OnnxRuntime.Gpu.Linux` ਤੋਂ ਅਸਲੀ ਐਸੈਟਾਂ ਨੂੰ ਬਾਹਰ ਰੱਖਦਾ ਹੈ ਜਿਸ ਵਿੱਚ ਇੱਕ ਟੁੱਟਾ ਹੋਇਆ win-arm64 ਰੈਫਰੰਸ ਹੈ। ਪਿਛਲੇ ਕੋਸ਼ਿਸ਼/ਪਕੜ NPU ਵਰਕਅਰਾਊਂਡ ਨੂੰ ਸਾਰੇ 7 C# ਫਾਈਲਾਂ ਤੋਂ ਹਟਾ ਦਿੱਤਾ ਗਿਆ ਹੈ।

### ਬਦਲਿਆ ਗਿਆ
- **ਹਿੱਸਾ 9 (ਵਿਸਪਰ):** ਮੁੱਖ ਰੀਰਾਈਟ — ਜਾਵਾਸਕ੍ਰਿਪਟ ਹੁਣ SDK ਦੇ ਬਿਲਟ-ਇਨ `AudioClient` (`model.createAudioClient()`) ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ ਮੈਨੂਅਲ ONNX ਰਨਟਾਈਮ ਅਨੁਮਾਨ ਦੀ ਬਜਾਏ; JS/C# `AudioClient` ਦ੍ਰਿਸ਼ਟੀਕੋਣ ਵਿੱਥੋਂ Python ONNX Runtime ਵਿੱਥੋਂ ਅਪਡੇਟ ਕੀਤੀਆਂ ਵਿਵਰਣ, ਤੁਲਨਾ ਟੇਬਲਾਂ, ਅਤੇ ਪਾਈਪਲਾਈਨ ਡਾਇਗਰਾਮਾਂ
- **ਹਿੱਸਾ 11:** ਨੈਵੀਗੇਸ਼ਨ ਲਿੰਕ ਅਪਡੇਟ ਕੀਤੀ ਗਈਆਂ (ਹੁਣ ਹਿੱਸਾ 12 ਵੱਲ ਇਸ਼ਾਰਾ ਕਰਦਾ ਹੈ); ਟੂਲ ਕਾਲਿੰਗ ਫ਼ਲੋ ਅਤੇ ਸੀਕਵੈਂਸ ਲਈ SVG ਡਾਇਗਰਾਮ ਜੋੜੇ ਗਏ
- **ਹਿੱਸਾ 10:** ਨੈਵੀਗੇਸ਼ਨ ਨੂੰ ਹਿੱਸਾ 12 ਰਾਹੀਂ ਰੂਟ ਕਰਨ ਲਈ ਅਪਡੇਟ ਕੀਤਾ ਗਿਆ, ਵਰਕਸ਼ਾਪ ਖਤਮ ਕਰਨ ਦੀ ਥਾਂ
- **ਪਾਇਥਨ ਵਿਸਪਰ (`foundry-local-whisper.py`):** ਵਾਧੂ ਆਡੀਓ ਨਮੂਨਿਆਂ ਅਤੇ ਸੁਧਾਰੀ ਹੋਈ ਗਲਤੀ ਸੰਭਾਲ ਨਾਲ ਵਿਸਥਾਰ ਕੀਤਾ ਗਿਆ
- **ਜਾਵਾਸਕ੍ਰਿਪਟ ਵਿਸਪਰ (`foundry-local-whisper.mjs`):** `model.createAudioClient()` ਨਾਲ ਵੀਰਲਿਖਤ ਅਤੇ `audioClient.transcribe()` ਦੀ ਵਰਤੋਂ ਨਾਲ ਮੈਨੂਅਲ ONNX ਰਨਟਾਈਮ ਸੈਸ਼ਨ ਦੀ ਬਜਾਏ ਉਤਪਾਦਿਤ ਕੀਤਾ ਗਿਆ
- **ਪਾਇਥਨ ਫਾਸਟAPI (`zava-creative-writer-local/src/api/main.py`):** API ਨਾਲ ਸਾਥ-ਸਾਥ ਸਥਿਰ ਯੂਆਈ ਫਾਈਲਾਂ ਦੀ ਸੇਵਾ ਕਰਨ ਲਈ ਅਪਡੇਟ ਕੀਤਾ ਗਿਆ
- **ਜ਼ਾਵਾ C# ਕਨਸੋਲ (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU ਵਰਕਅਰਾਊਂਡ ਹਟਾ ਦਿੱਤਾ (ਹੁਣ WinML ਪੈਕੇਜ ਦੁਆਰਾ ਸੰਭਾਲਿਆ ਜਾਂਦਾ ਹੈ)
- **README.md:** ਹਿੱਸਾ 12 ਵਿੱਚ ਕੋਡ ਸੈਂਪਲ ਟੇਬਲਾਂ ਅਤੇ ਬੈਕਐਂਡ ਸ਼ਾਮਲਤਾ ਨਾਲ ਹਿੱਸਾ ਜੋੜਿਆ; ਹਿੱਸਾ 13 ਸ਼ਾਮਲ ਕੀਤਾ; ਸਿੱਖਣ ਦੀ ਉਦੇਸ਼ ਅਤੇ ਪ੍ਰਾਜੈਕਟ ਢਾਂਚੇ ਨੂੰ ਅਪਡੇਟ ਕੀਤਾ
- **KNOWN-ISSUES.md:** ਹੱਲ ਕੀਤੇ ਮੁੱਦੇ #7 (C# SDK NPU ਮਾਡਲ ਵੈਰੀਅੰਟ — ਹੁਣ WinML ਪੈਕੇਜ ਦੁਆਰਾ ਸੰਭਾਲਿਆ) ਨੂੰ ਹਟਾ ਦਿੱਤਾ; ਬਾਕੀ ਮੁੱਦਿਆਂ ਨੂੰ #1-#6 ਵਿੱਥੋਂ ਦੁਬਾਰਾ ਨੰਬਰ ਦਿੱਤੇ; ਮਾਹੌਲ ਦੇ ਵੇਰਵੇ .NET SDK 10.0.104 ਨਾਲ ਅਪਡੇਟ ਕੀਤੇ
- **AGENTS.md:** ਨਵੇਂ `zava-creative-writer-local` ਦਰਜਾਵਾਂ (`ui/`, `csharp-web/`, `server.mjs`) ਨਾਲ ਪ੍ਰਾਜੈਕਟ ਢਾਂਚਾ ਟ੍ਰੀ ਅਪਡੇਟ ਕੀਤਾ; C# ਮੁੱਖ ਪੈਕੇਜਾਂ ਅਤੇ ਸ਼ਰਤੀ TFM ਵੇਰਵੇ ਅਪਡੇਟ ਕੀਤੇ
- **labs/part2-foundry-local-sdk.md:** `.csproj` ਉਦਾਹਰਨ ਨੂੰ ਪੂਰੇ ਕਰਾਸ-ਪਲੇਟਫਾਰਮ ਪੈਟਰਨ ਨਾਲ ਸ਼ਰਤੀ TFM, ਆਪਸੀ ਅਸਮਰਥ ਪੈਕੇਜ ਰੈਫਰੰਸ ਅਤੇ ਵਿਆਖਿਆਤਮਕ ਨੋਟ ਦੇ ਕੇ ਅਪਡੇਟ ਕੀਤਾ

### ਵੈਰੀਫਾਈ ਕੀਤਾ
- ਸਾਰੇ 3 C# ਪਰਿਯੋਜਨਾਵਾਂ (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) Windows ARM64 'ਤੇ ਸਫਲਤਾਪੂਰਵਕ ਬਣਦੇ ਹਨ
- ਚੈਟ ਸੈਂਪਲ (`dotnet run chat`): ਮਾਡਲ WinML/QNN ਰਾਹੀਂ `phi-3.5-mini-instruct-qnn-npu:1` ਵਜੋਂ ਲੋਡ ਹੁੰਦਾ ਹੈ — NPU ਵੈਰੀਅੰਟ ਡਾਇਰੈਕਟਲੀ ਬਿਨਾਂ CPU ਫਾਲਬੈਕ ਦੇ ਲੋਡ ਹੁੰਦਾ ਹੈ
- ਏਜੰਟ ਸੈਂਪਲ (`dotnet run agent`): ਮਲਟੀ-ਟਰਨ ਗੱਲਬਾਤ ਨਾਲ ਸਕੂਰੀਆਵੰਤ ਚੱਲਦਾ ਹੈ, ਬਾਹਰ ਨਿਕਲਣ ਕੋਡ 0
- Foundry Local CLI v0.8.117 ਅਤੇ SDK v0.9.0 .NET SDK 9.0.312 'ਤੇ

---

## 2026-03-11 — ਕੋਡ ਫਿਕਸ, ਮਾਡਲ ਸਾਫ-ਸੁਥਰੇ, ਮਰਮੈਡ ਡਾਇਗਰਾਮ, ਅਤੇ ਵੈਰੀਫਿਕੇਸ਼ਨ

### ਠੀਕ ਕੀਤਾ
- **ਸਾਰੇ 21 ਕੋਡ ਸੈਂਪਲ (7 ਪਾਇਥਨ, 7 ਜਾਵਾਸਕ੍ਰਿਪਟ, 7 C#):** ਬਾਹਰ ਨਿਕਲਣ 'ਤੇ `model.unload()` / `unload_model()` / `model.UnloadAsync()` ਕੰਮਕਾਜ ਜੋੜਿਆ, OGA ਮੈਮੋਰੀ ਲੀਕ ਚੇਤਾਵਨੀ ਹਟਾਉਣ ਲਈ (ਮਾਲੂਮ ਮੁੱਦਾ #4)
- **csharp/WhisperTranscription.cs:** ਨਾਜੁਕ `AppContext.BaseDirectory` ਸਾਧਾਰਨ ਰਾਹ ਨੂੰ `FindSamplesDirectory()` ਨਾਲ ਬਦਲਿਆ ਜੋ `samples/audio` ਨੂੰ ਭਰੋਸੇਯੋਗ ਤਰੀਕੇ ਨਾਲ ਲੱਭਦਾ ਹੈ (ਮਾਲੂਮ ਮੁੱਦਾ #7)
- **csharp/csharp.csproj:** ਹਾਰਡਕੋਡ `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` ਨੂੰ `$(NETCoreSdkRuntimeIdentifier)` ਨਾਲ ਆਪਣੇ ਆਪ ਪਛਾਣ ਵਾਲੇ ਫਾਲਬੈਕ ਨਾਲ ਬਦਲਿਆ ਤਾਂ ਜੋ `dotnet run` ਕਿਸੇ ਵੀ ਪਲੇਟਫਾਰਮ 'ਤੇ -r ਜ਼ਿੰਨ੍ਹਾਂ ਤੋਂ ਬਿਨਾਂ ਕੰਮ ਕਰੇ ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### ਬਦਲਿਆ ਗਿਆ
- **ਹਿੱਸਾ 8:** ASCII ਬਾਕਸ ਡਾਇਗਰਾਮ ਤੋਂ ਮੁੱਲਾਂਕਣ ਪ੍ਰੇਰਿਤ ਇਤਰਾਟਿਵ ਲੂਪ ਨੂੰ SVG ਚਿੱਤਰ ਵਿੱਚ ਬਦਲਿਆ
- **ਹਿੱਸਾ 10:** ਕਾਰਜਪਾਲਨ ਪਾਈਪਲਾਈਨ ਡਾਇਗਰਾਮ ਨੂੰ ASCII ਤੀਰਾਂ ਤੋਂ SVG ਚਿੱਤਰ ਵਿੱਚ ਬਦਲਿਆ
- **ਹਿੱਸਾ 11:** ਟੂਲ ਕਾਲਿੰਗ ਫ਼ਲੋ ਅਤੇ ਸੀਕਵੈਂਸ ਡਾਇਗਰਾਮਾਂ ਨੂੰ SVG ਚਿੱਤਰ ਵਿੱਚ ਬਦਲਿਆ
- **ਹਿੱਸਾ 10:** "ਵਰਕਸ਼ਾਪ ਮੁਕੰਮਲ!" ਹਿੱਸਾ ਹਿੱਸਾ 11 (ਅੰਤਿਮ ਲੈਬ) ਬਣਾਇਆ ਗਿਆ; ਇਸ ਦੀ ਥਾਂ "ਅਗਲੇ ਕਦਮ" ਲਿੰਕ ਜੋੜਿਆ
- **KNOWN-ISSUES.md:** CLI v0.8.117 ਲਈ ਸਾਰੇ ਮੁੱਦਿਆਂ ਦੀ ਪੂਰੀ ਵੈਰੀਫਿਕੇਸ਼ਨ; ਹੱਲ ਕੀਤੇ: OGA ਮੈਮੋਰੀ ਲੀਕ (ਸਾਫਾਈ ਜੋੜੀ), ਵਿਸਪਰ ਪਾਥ (FindSamplesDirectory), HTTP 500 ਟਿਕਾਊ ਅਨੁਮਾਨ (ਦੋਹਰਾਏ ਨਹੀਂ ਜਾ ਸਕਦਾ, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice ਸੀਮਾਵਾਂ (ਹੁਣ `"required"` ਅਤੇ ਖਾਸ ਫੰਕਸ਼ਨ ਟਾਰਗੇਟਿੰਗ ਨਾਲ qwen2.5-0.5b 'ਤੇ ਕੰਮ ਕਰਦਾ ਹੈ)। ਜਾਵਾਸਕ੍ਰਿਪਟ ਵਿਸਪਰ ਮੁੱਦਾ ਅਪਡੇਟ ਕੀਤਾ — ਸਾਰੇ ਫਾਈਲਾਂ ਖਾਲੀ/ਬਾਈਨਰੀ ਆਉਟਪੁੱਟ ਨੂੰ ਵਾਪਸ ਕਰਦੀਆਂ ਹਨ (v0.9.x ਤੋਂ ਰੀਗ੍ਰੈਸ਼ਨ, ਗੰਭੀਰਤਾ ਵੱਧੀ ਹੋਈ)। #4 C# RID ਨੂੰ ਆਪਣੇ ਆਪ ਪਛਾਣ ਵਰਕਅਰਾਊਂਡ ਅਤੇ [#497](https://github.com/microsoft/Foundry-Local/issues/497) ਲਿੰਕ ਨਾਲ ਅਪਡੇਟ ਕੀਤਾ; 7 ਖੁੱਲ੍ਹੇ ਮੁੱਦੇ ਬਚੇ
- **javascript/foundry-local-whisper.mjs:** ਸਾਫਾਈ ਵਾਰੀਏਬਲ ਨਾਮ ਸਹੀ ਕੀਤਾ (`whisperModel` → `model`)

### ਵੈਰੀਫਾਈ ਕੀਤਾ
- ਪਾਇਥਨ: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — ਸਾਫਾਈ ਨਾਲ ਸਫਲ ਚੱਲੇ
- ਜਾਵਾਸਕ੍ਰਿਪਟ: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — ਸਾਫਾਈ ਨਾਲ ਸਫਲ ਚੱਲੇ
- C#: `dotnet build` 0 ਚੇਤਾਵਨੀਆਂ, 0 ਗਲਤੀਆਂ ਨਾਲ ਸਫਲ (net9.0 ਟਾਰਗੇਟ)
- ਸਾਰੇ 7 ਪਾਇਥਨ ਫਾਈਲਾਂ `py_compile` ਸਿਂਟੈਕਸ ਜਾਂਚ ਪਾਸ ਕੀਤੀਆਂ
- ਸਾਰੇ 7 ਜਾਵਾਸਕ੍ਰਿਪਟ ਫਾਈਲਾਂ `node --check` ਸਿੰਟੈਕਸ ਵੈਧਤਾ ਪਾਸ ਕੀਤੀ

---

## 2026-03-10 — ਹਿੱਸਾ 11: ਟੂਲ ਕਾਲਿੰਗ, SDK API ਵਿਸਥਾਰ, ਅਤੇ ਮਾਡਲ ਕਵਰੇਜ

### ਸ਼ਾਮਲ ਕੀਤਾ
- **ਹਿੱਸਾ 11: ਲੋਕਲ ਮਾਡਲਾਂ ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ** — ਨਵਾਂ ਲੈਬ ਗਾਈਡ (`labs/part11-tool-calling.md`) ਜਿਸ ਵਿੱਚ 8 ਅਭਿਆਸ ਹੈ ਜਿਨ੍ਹਾਂ ਵਿੱਚ ਟੂਲ ਸਕੀਮਾਂ, ਬਹੁ-ਟਰਨ ਫਲੋ, ਕਈ ਟੂਲ ਕਾਲਾਂ, ਕਸਟਮ ਟੂਲ, ChatClient ਟੂਲ ਕਾਲ, ਅਤੇ `tool_choice` ਸ਼ਾਮਲ ਹਨ
- **ਪਾਇਥਨ ਸੈਂਪਲ:** `python/foundry-local-tool-calling.py` — OpenAI SDK ਨਾਲ `get_weather`/`get_population` ਟੂਲ ਕਾਲਿੰਗ
- **ਜਾਵਾਸਕ੍ਰਿਪਟ ਸੈਂਪਲ:** `javascript/foundry-local-tool-calling.mjs` — SDK ਦੇ ਜਨਮਸিদ্ধ `ChatClient` (`model.createChatClient()`) ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ
- **C# ਸੈਂਪਲ:** `csharp/ToolCalling.cs` — OpenAI C# SDK ਦੇ ਨਾਲ `ChatTool.CreateFunctionTool()` ਦੁਆਰਾ ਟੂਲ ਕਾਲਿੰਗ
- **ਹਿੱਸਾ 2, ਅਭਿਆਸ 7:** ਜਨਮਸਿੱਧ `ChatClient` — `model.createChatClient()` (JS) ਅਤੇ `model.GetChatClientAsync()` (C#) OpenAI SDK ਲਈ ਵਿਕਲਪ ਵਜੋਂ
- **ਹਿੱਸਾ 2, ਅਭਿਆਸ 8:** ਮਾਡਲ ਵੈਰੀਅੰਟ ਅਤੇ ਹਾਰਡਵੇਅਰ ਚੋਣ — `selectVariant()`, `variants`, NPU ਵੈਰੀਅੰਟ ਟੇਬਲ (7 ਮਾਡਲ)
- **ਹਿੱਸਾ 2, ਅਭਿਆਸ 9:** ਮਾਡਲ ਅੱਪਗ੍ਰੇਡ ਅਤੇ ਕੈਟਾਲਾਗ ਰੀਫ੍ਰੈਸ਼ — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **ਹਿੱਸਾ 2, ਅਭਿਆਸ 10:** ਤਰਕਸ਼ੀਲ ਮਾਡਲ — `<think>` ਟੈਗ ਪਾਰਸਿੰਗ ਉਦਾਹਰਣਾਂ ਨਾਲ `phi-4-mini-reasoning`
- **ਹਿੱਸਾ 3, ਅਭਿਆਸ 4:** `createChatClient` OpenAI SDK ਲਈ ਵਿਕਲਪ ਵਜੋਂ, ਸਟ੍ਰੀਮਿੰਗ ਕਾਲਬੈਕ ਪੈਟਰਨ ਡੌਕਯੂਮੈਂਟੇਸ਼ਨ ਨਾਲ
- **AGENTS.md:** ਟੂਲ ਕਾਲਿੰਗ, ChatClient ਅਤੇ ਤਰਕਸ਼ੀਲ ਮਾਡਲਾਂ ਦੇ ਕੋਡਿੰਗ ਪ੍ਰਥਾਵਾਂ ਸ਼ਾਮਲ ਕੀਤੀਆਂ

### ਬਦਲਿਆ ਗਿਆ
- **ਹਿੱਸਾ 1:** ਮਾਡਲ ਕੈਟਾਲਾਗ ਵਿਸਥਾਰ — `phi-4-mini-reasoning`, `gpt-oss-20b`, `phi-4`, `qwen2.5-7b`, `qwen2.5-coder-7b`, `whisper-large-v3-turbo` ਸ਼ਾਮਲ ਕੀਤੇ
- **ਹਿੱਸਾ 2:** API ਰੈਫਰੰਸ ਟੇਬਲਾਂ ਵਿਸਥਾਰ — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` ਸ਼ਾਮਲ ਕੀਤੇ
- **ਹਿੱਸਾ 2:** ਅਭਿਆਸ 7-9 ਦੇ ਨੰਬਰ 10-13 ਵਿੱਚ ਬਦਲੇ ਗਏ ਨਵੇਂ ਅਭਿਆਸ ਲਈ
- **ਹਿੱਸਾ 3:** ਮੁੱਖ ਸਿੱਖਣ ਵਾਲੀਆਂ ਗੱਲਾਂ ਦੀ ਸੂਚੀ ਨੂੰ ਜਨਮਸਿੱਧ ChatClient ਸਮੇਤ ਅਪਡੇਟ ਕੀਤਾ
- **README.md:** ਹਿੱਸਾ 11 ਵਿੱਚ ਕੋਡ ਸੈਂਪਲ ਟੇਬਲ ਜੋੜਿਆ; ਸਿੱਖਣ ਦਾ ਲਕੜੀ #11 ਜੋੜਿਆ; ਪ੍ਰਾਜੈਕਟ ਢਾਂਚਾ ਟ੍ਰੀ ਅਪਡੇਟ ਕੀਤਾ
- **csharp/Program.cs:** CLI ਰੂਟਰ ਵਿੱਚ `toolcall` ਕੇਸ ਸ਼ਾਮਲ ਕੀਤਾ ਅਤੇ ਸਹਾਇਤਾ ਲਿਖਤ ਅਪਡੇਟ ਕੀਤੀ

---

## 2026-03-09 — SDK v0.9.0 ਅਪਡੇਟ, ਬਰਿਟਿਸ਼ ਅੰਗਰੇਜ਼ੀ, ਅਤੇ ਵੈਰੀਫਿਕੇਸ਼ਨ

### ਬਦਲਿਆ ਗਿਆ
- **ਸਾਰੇ ਕੋਡ ਸੈਂਪਲ (ਪਾਇਥਨ, ਜਾਵਾਸਕ੍ਰਿਪਟ, C#):** Foundry Local SDK v0.9.0 API ਲਈ ਅਪਡੇਟ ਕੀਤਾ — `await catalog.getModel()` ਦੀ ਗਲਤੀ ਠੀਕ ਕੀਤੀ ਜੋ `await` ਗੁੰਮ ਸੀ, `FoundryLocalManager` ਇਨਿਸ਼ੀਅਲਾਈਜ਼ ਪੈਟਰਨ, ਐਂਡਪੌਇੰਟ ਡਿਸਕਵਰੀ ਅਪਡੇਟ ਕੀਤੀ
- **ਸਾਰੇ ਲੈਬ ਗਾਈਡ (ਹਿੱਸੇ 1-10):** ਬਰਿਟਿਸ਼ ਅੰਗਰੇਜ਼ੀ ਵਿੱਚ ਰੂਪਾਂਤਰਿਤ (colour, catalogue, optimised, ਆਦਿ)
- **ਸਾਰੇ ਲੈਬ ਗਾਈਡ:** SDK ਕੋਡ ਉਦਾਹਰਣ v0.9.0 API ਸਰਫੇਸ ਨਾਲ ਸਹਿਮਤ ਅਪਡੇਟ ਕੀਤੇ
- **ਸਾਰੇ ਲੈਬ ਗਾਈਡ:** API ਰੈਫਰੰਸ ਟੇਬਲ ਅਤੇ ਅਭਿਆਸ ਕੋਡ ਬਲਾਕ ਅਪਡੇਟ ਕੀਤੇ
- **ਜਾਵਾਸਕ੍ਰਿਪਟ ਮਹੱਤਵਪੂਰਨ ਫਿਕਸ:** `catalog.getModel()` ਤੇ ਗੁੰਮ `await` ਜੋੜਿਆ — ਜੋ ਇੱਕ `Promise` ਵਾਪਸ ਕਰਦਾ ਸੀ ਮਾਡਲ ਨਹੀਂ, ਜਿਸ ਨਾਲ ਅੱਗੇ ਆਮ ਤੌਰ ਤੇ ਚੁੱਪ ਪੰਜਾਬੀ ਗੱਲਤੀਆਂ ਹੁੰਦੀਆਂ ਸਨ

### ਵੈਰੀਫਾਈ ਕੀਤਾ
- ਸਾਰੇ ਪਾਇਥਨ ਸੈਂਪਲ Foundry Local ਸੇਵਾ ਨਾਲ ਸਫਲ ਚੱਲਦੇ ਹਨ
- ਸਾਰੇ ਜਾਵਾਸਕ੍ਰਿਪਟ ਸੈਂਪਲ (Node.js 18+ ਦੇ ਨਾਲ) ਸਫਲ ਚੱਲਦੇ ਹਨ
- C# ਪਰਿਯੋਜਨਾ .NET 9.0 'ਤੇ ਬਣਦੀ ਅਤੇ ਚੱਲਦੀ ਹੈ (net8.0 SDK ਸੰਯੋਜਨ ਤੋਂ ਅੱਗੇ ਸੰਗਤ)
- ਵਰਕਸ਼ਾਪ ਵਿੱਚ 29 ਫਾਈਲਾਂ ਸੋਧੀਆਂ ਅਤੇ ਵੈਰੀਫਾਈ ਕੀਤੀਆਂ ਗਈਆਂ

---

## ਫਾਈਲ ਇੰਡੈਕਸ

| ਫਾਈਲ | ਆਖਰੀ ਵਾਰ ਅਪਡੇਟ | ਵੇਰਵਾ |
|------|----------------|--------|
| `labs/part1-getting-started.md` | 2026-03-10 | ਮਾਡਲ ਕੈਟਾਲਾਗ ਵਿਸਥਾਰ |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | ਨਵੇਂ ਅਭਿਆਸ 7-10, API ਟੇਬਲਾਂ ਵਿਸਥਾਰ |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | ਨਵਾਂ ਅਭਿਆਸ 4 (ChatClient), ਅਪਡੇਟ ਕੀਤੇ ਮੁੱਖ ਨਕਤੇ |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + ਬਰਿਟਿਸ਼ ਅੰਗਰੇਜ਼ੀ |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + ਬਰਿਟਿਸ਼ ਅੰਗਰੇਜ਼ੀ |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + ਬ੍ਰਿਤਿਸ਼ ਅੰਗਰੇਜ਼ੀ |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + ਬ੍ਰਿਤਿਸ਼ ਅੰਗਰੇਜ਼ੀ |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid ਡਾਇਗ੍ਰਾਮ |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + ਬ੍ਰਿਤਿਸ਼ ਅੰਗਰੇਜ਼ੀ |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid ਡਾਇਗ੍ਰਾਮ, ਵਰਕਸ਼ਾਪ ਮੁਕੰਮਲ ਨੂੰ ਹਿੱਸਾ 11 ਵਿੱਚ ਸ਼ਿਫਟ ਕੀਤਾ ਗਿਆ |
| `labs/part11-tool-calling.md` | 2026-03-11 | ਨਵਾਂ ਲੈਬ, Mermaid ਡਾਇਗ੍ਰਾਮ, ਵਰਕਸ਼ਾਪ ਮੁਕੰਮਲ ਸੈਕਸ਼ਨ |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | ਨਵਾਂ: ਟੂਲ ਕਾਲਿੰਗ ਨਮੂਨਾ |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | ਨਵਾਂ: ਟੂਲ ਕਾਲਿੰਗ ਨਮੂਨਾ |
| `csharp/ToolCalling.cs` | 2026-03-10 | ਨਵਾਂ: ਟੂਲ ਕਾਲਿੰਗ ਨਮੂਨਾ |
| `csharp/Program.cs` | 2026-03-10 | ਸ਼ਾਮਲ ਕੀਤਾ `toolcall` CLI ਕਮਾਂਡ |
| `README.md` | 2026-03-10 | ਹਿੱਸਾ 11, ਪ੍ਰੋਜੈਕਟ ਦੀ ਸੰਰચਨਾ |
| `AGENTS.md` | 2026-03-10 | ਟੂਲ ਕਾਲਿੰਗ + ChatClient ਰਿਵਾਜ |
| `KNOWN-ISSUES.md` | 2026-03-11 | ਮਿਟਾ ਦਿੱਤਾ ਹੱਲ ਕੀਤਾ ਮਾਮਲਾ #7, 6 ਖੁੱਲ੍ਹੇ ਮਾਮਲੇ ਬਕਾਇਦਾ ਹਨ |
| `csharp/csharp.csproj` | 2026-03-11 | ਕ੍ਰਾਸ-ਪ्लੈਟਫਾਰਮ TFM, WinML/base SDK ਸਸ਼ਰਤ ਸੰਦਰਭ |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | ਕ੍ਰਾਸ-ਪ्लੈਟਫਾਰਮ TFM, ਆਟੋ-ਡਿਟੈਕਟ RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | ਕ੍ਰਾਸ-ਪ्लੈਟਫਾਰਮ TFM, ਆਟੋ-ਡਿਟੈਕਟ RID |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU ਟ੍ਰਾਈ/ਕੈਚ ਵਰਕਅਰਾਉਂਡ ਹਟਾਇਆ |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU ਟ੍ਰਾਈ/ਕੈਚ ਵਰਕਅਰਾਉਂਡ ਹਟਾਇਆ |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU ਟ੍ਰਾਈ/ਕੈਚ ਵਰਕਅਰਾਉਂਡ ਹਟਾਇਆ |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU ਟ੍ਰਾਈ/ਕੈਚ ਵਰਕਅਰਾਉਂਡ ਹਟਾਇਆ |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU ਟ੍ਰਾਈ/ਕੈਚ ਵਰਕਅਰਾਉਂਡ ਹਟਾਇਆ |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | ਕ੍ਰਾਸ-ਪ्लੈਟਫਾਰਮ .csproj ਉਦਾਹਰਨ |
| `AGENTS.md` | 2026-03-11 | C# ਪੈਕੇਜਜ਼ ਅਤੇ TFM ਵੇਰਵੇ ਅਪਡੇਟ ਕੀਤੇ ਗਏ |
| `CHANGELOG.md` | 2026-03-11 | ਇਹ ਫਾਇਲ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਅਸੁਵੀਕਾਰਤਾ**:  
ਇਹ ਦਸਤਾਵੇਜ਼ AI ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਅਨੁਵਾਦ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਟ੍ਰਾਈਵ ਕਰਦੇ ਹਾਂ ਕਿ ਸਹੀ ਹੋਵੇ, ਕਿਰਪਾ ਕਰਕੇ ਜਾਣੂ ਰਹੋ ਕਿ ਸੁਚਾਲਿਤ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਸਪਸ਼ਟਤਾਵਾਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਇਸ ਦੇ ਮੂਲ ਦਸਤਾਵੇਜ਼ ਨੂੰ ਇਸ ਦੀ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਅਥਾਰਟੀਟੇਟਿਵ ਸਰੋਤ ਮੰਨਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਮਹੱਤਵਪੂਰਨ ਜਾਣਕਾਰੀ ਲਈ ਪ੍ਰੋਫੈਸ਼ਨਲ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫਾਰਿਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਅਸੀਂ ਇਸ ਅਨੁਵਾਦ ਦੀ ਵਰਤੋਂ ਨਾਲ ਪੈਦਾ ਹੋਣ ਵਾਲੀਆਂ ਕਿਸੇ ਵੀ ਗਲਤਫਹਿਮੀਆਂ ਜਾਂ ਗਲਤ ਵਿਆਖਿਆਵਾਂ ਲਈ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->