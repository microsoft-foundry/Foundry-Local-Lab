# Changelog — Foundry Local Workshop

Tất cả các thay đổi đáng chú ý đối với workshop này được ghi lại phía dưới.

---

## 2026-03-11 — Phần 12 & 13, Giao diện Web, Viết lại Whisper, Sửa WinML/QNN, và Xác thực

### Đã thêm
- **Phần 12: Xây dựng giao diện Web cho Zava Creative Writer** — hướng dẫn lab mới (`labs/part12-zava-ui.md`) với các bài tập bao gồm streaming NDJSON, `ReadableStream` trình duyệt, huy hiệu trạng thái tác giả trực tiếp, và streaming văn bản bài báo thời gian thực
- **Phần 13: Hoàn tất Workshop** — lab tóm tắt mới (`labs/part13-workshop-complete.md`) với tổng kết tất cả 12 phần, ý tưởng bổ sung, và liên kết tài nguyên
- **Giao diện front end Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — giao diện trình duyệt vanilla HTML/CSS/JS dùng chung cho cả ba backend
- **Máy chủ HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — máy chủ HTTP kiểu Express mới bọc orchestrator để truy cập qua trình duyệt
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` và `ZavaCreativeWriterWeb.csproj` — dự án API tối giản phục vụ UI và streaming NDJSON
- **Trình tạo mẫu âm thanh:** `samples/audio/generate_samples.py` — script TTS offline dùng `pyttsx3` tạo các file WAV chủ đề Zava cho Phần 9
- **Mẫu âm thanh:** `samples/audio/zava-full-project-walkthrough.wav` — mẫu âm thanh dài hơn mới để thử nghiệm chuyển văn bản thành giọng nói
- **Script xác thực:** `validate-npu-workaround.ps1` — script PowerShell tự động để xác thực giải pháp thay thế NPU/QNN trên tất cả các mẫu C#
- **Các biểu đồ Mermaid dạng SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Hỗ trợ WinML đa nền tảng:** Tất cả 3 file `.csproj` C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) hiện sử dụng TFM điều kiện và tham chiếu gói loại trừ lẫn nhau cho hỗ trợ đa nền tảng. Trên Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (bộ mở rộng bao gồm plugin QNN EP). Trên non-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK cơ bản). RID `win-arm64` được mã hóa cứng trong dự án Zava đã được thay thế bằng phát hiện tự động. Một giải pháp thay thế phụ thuộc xuyên suốt loại trừ tài sản native từ `Microsoft.ML.OnnxRuntime.Gpu.Linux` có tham chiếu bị lỗi win-arm64. Giải pháp try/catch trước đây cho NPU đã bị loại bỏ khỏi tất cả 7 file C#.

### Đã thay đổi
- **Phần 9 (Whisper):** Viết lại lớn — JavaScript hiện sử dụng `AudioClient` tích hợp trong SDK (`model.createAudioClient()`) thay vì infer thủ công với ONNX Runtime; cập nhật mô tả kiến trúc, bảng so sánh, và sơ đồ pipeline để so sánh cách tiếp cận JS/C# `AudioClient` với cách Python dùng ONNX Runtime
- **Phần 11:** Cập nhật liên kết điều hướng (hiện trỏ tới Phần 12); thêm các biểu đồ SVG đã render cho luồng gọi công cụ và chuỗi sự kiện
- **Phần 10:** Cập nhật điều hướng chuyển qua Phần 12 thay vì kết thúc workshop
- **Python Whisper (`foundry-local-whisper.py`):** Mở rộng với mẫu âm thanh khác và cải thiện xử lý lỗi
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Viết lại để sử dụng `model.createAudioClient()` với `audioClient.transcribe()` thay vì phiên ONNX Runtime thủ công
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Cập nhật để phục vụ các file UI tĩnh kèm theo API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** Loại bỏ giải pháp thay thế NPU (hiện xử lý bởi gói WinML)
- **README.md:** Thêm phần 12 với bảng mẫu code và bổ sung backend; thêm phần 13; cập nhật mục tiêu học tập và cấu trúc dự án
- **KNOWN-ISSUES.md:** Loại bỏ Issue #7 đã giải quyết (biến thể model NPU C# — hiện xử lý bởi WinML). Đánh lại số các issue còn lại từ #1–#6. Cập nhật chi tiết môi trường với .NET SDK 10.0.104
- **AGENTS.md:** Cập nhật cây cấu trúc dự án với các mục mới trong `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); cập nhật các gói chính C# và chi tiết TFM điều kiện
- **labs/part2-foundry-local-sdk.md:** Cập nhật ví dụ `.csproj` hiển thị mẫu giao diện đa nền tảng đầy đủ với TFM điều kiện, tham chiếu gói loại trừ lẫn nhau, và chú thích giải thích

### Đã xác thực
- Tất cả 3 dự án C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) build thành công trên Windows ARM64
- Mẫu chat (`dotnet run chat`): model tải là `phi-3.5-mini-instruct-qnn-npu:1` qua WinML/QNN — biến thể NPU tải trực tiếp không cần fallback CPU
- Mẫu agent (`dotnet run agent`): chạy end-to-end với hội thoại đa lượt, mã thoát 0
- Foundry Local CLI v0.8.117 và SDK v0.9.0 trên .NET SDK 9.0.312

---

## 2026-03-11 — Sửa Mã, Dọn Dẹp Model, Biểu Đồ Mermaid, và Xác Thực

### Đã sửa
- **Tất cả 21 mẫu code (7 Python, 7 JavaScript, 7 C#):** Thêm `model.unload()` / `unload_model()` / `model.UnloadAsync()` dọn dẹp khi thoát để giải quyết cảnh báo rò rỉ bộ nhớ OGA (Vấn đề đã biết #4)
- **csharp/WhisperTranscription.cs:** Thay thế đường dẫn tương đối không chắc `AppContext.BaseDirectory` bằng `FindSamplesDirectory()` duyệt thư mục lên trên để định vị thư mục `samples/audio` chắc chắn hơn (Vấn đề đã biết #7)
- **csharp/csharp.csproj:** Thay thế `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` mã hóa cứng bằng fallback tự động dùng `$(NETCoreSdkRuntimeIdentifier)` để `dotnet run` hoạt động trên mọi nền tảng mà không cần cờ `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Đã thay đổi
- **Phần 8:** Chuyển vòng lặp đánh giá chạy theo bảng chữ ASCII thành hình SVG đã render
- **Phần 10:** Chuyển biểu đồ pipeline biên dịch từ mũi tên ASCII sang hình SVG đã render
- **Phần 11:** Chuyển biểu đồ luồng gọi công cụ và chuỗi sự kiện sang hình SVG đã render
- **Phần 10:** Di chuyển phần "Workshop Complete!" sang Phần 11 (lab cuối); thay bằng liên kết "Next Steps"
- **KNOWN-ISSUES.md:** Xác thực lại toàn bộ các vấn đề với CLI v0.8.117. Loại bỏ các vấn đề đã giải quyết: Rò rỉ bộ nhớ OGA (đã thêm dọn dẹp), đường dẫn Whisper (FindSamplesDirectory), HTTP 500 khi infer liên tục (không tái hiện được, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), giới hạn tool_choice (hiện hoạt động với `"required"` và chức năng cụ thể trên qwen2.5-0.5b). Cập nhật sự cố JS Whisper — giờ toàn bộ file trả về output rỗng/nhị phân (suy giảm từ v0.9.x, mức độ nghiêm trọng tăng lên Major). Cập nhật #4 RID C# với giải pháp tự động phát hiện và link [#497](https://github.com/microsoft/Foundry-Local/issues/497). Còn 7 vấn đề mở.
- **javascript/foundry-local-whisper.mjs:** Sửa tên biến dọn dẹp (`whisperModel` → `model`)

### Đã xác thực
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — chạy thành công với dọn dẹp
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — chạy thành công với dọn dẹp
- C#: `dotnet build` thành công không cảnh báo lỗi (target net9.0)
- 7 file Python đều qua kiểm tra cú pháp `py_compile`
- 7 file JavaScript đều qua kiểm tra cú pháp `node --check`

---

## 2026-03-10 — Phần 11: Gọi Công Cụ, Mở Rộng API SDK, và Bao Phủ Model

### Đã thêm
- **Phần 11: Gọi Công Cụ với Model Local** — hướng dẫn lab mới (`labs/part11-tool-calling.md`) với 8 bài tập bao gồm schemas công cụ, luồng đa lượt, gọi nhiều công cụ, công cụ tùy chỉnh, gọi công cụ qua ChatClient, và `tool_choice`
- **Mẫu Python:** `python/foundry-local-tool-calling.py` — gọi công cụ với các công cụ `get_weather`/`get_population` dùng OpenAI SDK
- **Mẫu JavaScript:** `javascript/foundry-local-tool-calling.mjs` — gọi công cụ dùng `ChatClient` native của SDK (`model.createChatClient()`)
- **Mẫu C#:** `csharp/ToolCalling.cs` — gọi công cụ dùng `ChatTool.CreateFunctionTool()` với OpenAI C# SDK
- **Phần 2, Bài tập 7:** ChatClient native — `model.createChatClient()` (JS) và `model.GetChatClientAsync()` (C#) làm lựa chọn thay thế OpenAI SDK
- **Phần 2, Bài tập 8:** Biến thể model và chọn phần cứng — `selectVariant()`, `variants`, bảng biến thể NPU (7 model)
- **Phần 2, Bài tập 9:** Nâng cấp model và làm mới catalog — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Phần 2, Bài tập 10:** Model suy luận — `phi-4-mini-reasoning` với ví dụ phân tích thẻ `<think>`
- **Phần 3, Bài tập 4:** `createChatClient` làm lựa chọn thay thế OpenAI SDK, với doc pattern callback streaming
- **AGENTS.md:** Thêm các quy ước viết code Tool Calling, ChatClient, và Reasoning Models

### Đã thay đổi
- **Phần 1:** Mở rộng catalog model — thêm phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Phần 2:** Mở rộng bảng tham khảo API — thêm `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Phần 2:** Đánh lại số bài tập 7-9 → 10-13 để bổ sung bài tập mới
- **Phần 3:** Cập nhật bảng điểm chính để bao gồm ChatClient native
- **README.md:** Thêm phần 11 với bảng mẫu code; thêm mục tiêu học tập #11; cập nhật cây cấu trúc dự án
- **csharp/Program.cs:** Thêm trường hợp `toolcall` vào router CLI và cập nhật văn bản trợ giúp

---

## 2026-03-09 — Cập nhật SDK v0.9.0, Tiếng Anh Anh, và Đợt Xác Thực

### Đã thay đổi
- **Tất cả mẫu code (Python, JavaScript, C#):** Cập nhật API Foundry Local SDK v0.9.0 — sửa lỗi thiếu `await` trong `catalog.getModel()`, cập nhật mẫu khởi tạo `FoundryLocalManager`, sửa lỗi phát hiện endpoint
- **Tất cả hướng dẫn lab (Phần 1-10):** Chuyển sang tiếng Anh Anh (colour, catalogue, optimised, v.v.)
- **Tất cả hướng dẫn lab:** Cập nhật ví dụ code SDK để khớp API v0.9.0
- **Tất cả hướng dẫn lab:** Cập nhật bảng tham khảo API và các đoạn code bài tập
- **Sửa lỗi nghiêm trọng JavaScript:** Thêm `await` bị thiếu cho `catalog.getModel()` — trả về `Promise` không phải đối tượng `Model`, gây lỗi ngấm ngầm phía sau

### Đã xác thực
- Tất cả mẫu Python chạy thành công với dịch vụ Foundry Local
- Tất cả mẫu JavaScript chạy thành công (Node.js 18+)
- Dự án C# build và chạy trên .NET 9.0 (tương thích ngược với net8.0 SDK assembly)
- 29 file được sửa đổi và xác thực trên toàn bộ workshop

---

## Thư mục Tập tin

| Tập tin | Cập nhật lần cuối | Mô tả |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Mở rộng catalog model |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Các bài tập mới 7-10, mở rộng bảng API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Bài tập 4 mới (ChatClient), cập nhật điểm chính |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + tiếng Anh Anh |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + tiếng Anh Anh |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Tiếng Anh Anh |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Tiếng Anh Anh |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Sơ đồ Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Tiếng Anh Anh |
| `labs/part10-custom-models.md` | 2026-03-11 | Sơ đồ Mermaid, chuyển Mục Workshop Complete sang Phần 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Lab mới, sơ đồ Mermaid, phần Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Mẫu gọi công cụ mới |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Mẫu gọi công cụ mới |
| `csharp/ToolCalling.cs` | 2026-03-10 | Mẫu gọi công cụ mới |
| `csharp/Program.cs` | 2026-03-10 | Thêm lệnh CLI `toolcall` |
| `README.md` | 2026-03-10 | Phần 11, cấu trúc dự án |
| `AGENTS.md` | 2026-03-10 | Gọi công cụ + quy ước ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Đã bỏ Issue #7 đã giải quyết, còn lại 6 issue chưa giải quyết |
| `csharp/csharp.csproj` | 2026-03-11 | TFM đa nền tảng, tham chiếu WinML/base SDK có điều kiện |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM đa nền tảng, tự động phát hiện RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM đa nền tảng, tự động phát hiện RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Bỏ cách xử lý try/catch NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Bỏ cách xử lý try/catch NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Bỏ cách xử lý try/catch NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Bỏ cách xử lý try/catch NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Bỏ cách xử lý try/catch NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Ví dụ .csproj đa nền tảng |
| `AGENTS.md` | 2026-03-11 | Cập nhật chi tiết gói C# và TFM |
| `CHANGELOG.md` | 2026-03-11 | Tệp này |