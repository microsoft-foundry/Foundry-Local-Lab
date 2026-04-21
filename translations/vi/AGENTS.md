# Hướng Dẫn Đại Lý Mã Lệnh

Tệp này cung cấp bối cảnh cho các đại lý mã AI (GitHub Copilot, Copilot Workspace, Codex, v.v.) làm việc trong kho lưu trữ này.

## Tổng Quan Dự Án

Đây là một **workshop thực hành** để xây dựng các ứng dụng AI với [Foundry Local](https://foundrylocal.ai) — một môi trường chạy nhẹ tải xuống, quản lý và phục vụ các mô hình ngôn ngữ hoàn toàn trên thiết bị thông qua API tương thích OpenAI. Workshop bao gồm các hướng dẫn phòng thí nghiệm chi tiết theo từng bước và các mẫu mã có thể chạy được bằng Python, JavaScript và C#.

## Cấu Trúc Kho Lưu Trữ

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Chi Tiết Ngôn Ngữ & Framework

### Python
- **Vị trí:** `python/`, `zava-creative-writer-local/src/api/`
- **Phụ thuộc:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Gói chính:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Phiên bản tối thiểu:** Python 3.9+
- **Chạy:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Vị trí:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Phụ thuộc:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Gói chính:** `foundry-local-sdk`, `openai`
- **Hệ thống module:** ES modules (file `.mjs`, `"type": "module"`)
- **Phiên bản tối thiểu:** Node.js 18+
- **Chạy:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Vị trí:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **File dự án:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Gói chính:** `Microsoft.AI.Foundry.Local` (không phải Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — bao gồm thêm QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Đích:** .NET 9.0 (TFM điều kiện: `net9.0-windows10.0.26100` trên Windows, `net9.0` ở nơi khác)
- **Chạy:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Quy Ước Mã Lệnh

### Chung
- Tất cả các mẫu mã là **ví dụ đơn tệp tự chứa** — không dùng thư viện tiện ích hay trừu tượng dùng chung.
- Mỗi ví dụ chạy độc lập sau khi cài đặt các phụ thuộc riêng.
- Khóa API luôn được đặt là `"foundry-local"` — Foundry Local dùng nó làm giá trị giữ chỗ.
- URL gốc sử dụng `http://localhost:<port>/v1` — cổng là động và được phát hiện khi chạy bằng SDK (`manager.urls[0]` trong JS, `manager.endpoint` trong Python).
- SDK Foundry Local quản lý khởi động dịch vụ và khám phá endpoint; ưu tiên dùng mẫu SDK thay vì cố định cổng.

### Python
- Dùng SDK `openai` với `OpenAI(base_url=..., api_key="not-required")`.
- Dùng `FoundryLocalManager()` từ `foundry_local` để quản lý vòng đời dịch vụ qua SDK.
- Streaming: lặp trên đối tượng `stream` với `for chunk in stream:`.
- Không dùng chú thích kiểu trong file mẫu (để súc tích cho người học workshop).

### JavaScript
- Cú pháp module ES: `import ... from "..."`.
- Dùng `OpenAI` từ `"openai"` và `FoundryLocalManager` từ `"foundry-local-sdk"`.
- Mẫu khởi tạo SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Dùng `await` cấp cao toàn cục.

### C#
- Bật nullable, using ẩn, .NET 9.
- Dùng `FoundryLocalManager.StartServiceAsync()` để quản lý vòng đời qua SDK.
- Streaming: `CompleteChatStreaming()` với `foreach (var update in completionUpdates)`.
- `csharp/Program.cs` chính là bộ định tuyến CLI phân phát đến các phương thức tĩnh `RunAsync()`.

### Gọi Công Cụ
- Chỉ một số mô hình hỗ trợ gọi công cụ: **Qwen 2.5** (“qwen2.5-*”) và **Phi-4-mini** (“phi-4-mini”).
- Lược đồ công cụ tuân theo định dạng JSON gọi hàm OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Hội thoại theo mẫu nhiều lượt: user → assistant (tool_calls) → tool (kết quả) → assistant (câu trả lời cuối).
- `tool_call_id` trong tin nhắn kết quả công cụ phải khớp với `id` trong cuộc gọi công cụ của mô hình.
- Python dùng trực tiếp OpenAI SDK; JavaScript dùng SDK với `ChatClient` gốc (`model.createChatClient()`); C# dùng OpenAI SDK với `ChatTool.CreateFunctionTool()`.

### ChatClient (Khách hàng SDK Gốc)
- JavaScript: `model.createChatClient()` trả về `ChatClient` có `completeChat(messages, tools?)` và `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` trả về `ChatClient` tiêu chuẩn có thể dùng không cần import gói NuGet OpenAI.
- Python không có ChatClient gốc — dùng OpenAI SDK với `manager.endpoint` và `manager.api_key`.
- **Lưu ý:** JavaScript `completeStreamingChat` dùng mẫu callback, không phải vòng lặp async.

### Mô Hình Lý Luận
- `phi-4-mini-reasoning` gói suy nghĩ trong thẻ `<think>...</think>` trước câu trả lời cuối.
- Phân tích thẻ để tách suy luận ra khỏi câu trả lời khi cần.

## Hướng Dẫn Phòng Thí Nghiệm

File lab nằm trong `labs/` dưới dạng Markdown. Chúng theo cấu trúc nhất quán:
- Ảnh đầu logo
- Tiêu đề và gọi mục tiêu
- Tổng quan, Mục tiêu học tập, Yêu cầu trước
- Các phần giải thích khái niệm kèm hình minh họa
- Bài tập đánh số có khối mã và đầu ra mong đợi
- Bảng tóm tắt, Điểm cần nhớ, Đọc thêm
- Link điều hướng đến phần tiếp theo

Khi chỉnh sửa nội dung lab:
- Giữ nguyên kiểu định dạng Markdown hiện có và thứ tự phân cấp phần.
- Khối mã phải chỉ rõ ngôn ngữ (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Cung cấp cả biến thể bash và PowerShell cho lệnh shell khi OS quan trọng.
- Dùng kiểu gọi chú ý `> **Note:**`, `> **Tip:**`, và `> **Troubleshooting:**`.
- Bảng sử dụng định dạng ống `| Tiêu đề | Tiêu đề |`.

## Lệnh Xây Dựng & Kiểm Tra

| Hành Động | Lệnh |
|--------|---------|
| **Mẫu Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Mẫu JS** | `cd javascript && npm install && node <script>.mjs` |
| **Mẫu C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **CLI Foundry Local** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Tạo sơ đồ** | `npx mmdc -i <input>.mmd -o <output>.svg` (cần cài root `npm install`) |

## Phụ Thuộc Bên Ngoài

- **CLI Foundry Local** phải được cài trên máy dev (`winget install Microsoft.FoundryLocal` hoặc `brew install foundrylocal`).
- **Dịch vụ Foundry Local** chạy cục bộ và cung cấp API REST tương thích OpenAI trên cổng động.
- Không yêu cầu dịch vụ đám mây, khóa API, hay đăng ký Azure để chạy mẫu nào.
- Phần 10 (mô hình tùy chỉnh) còn cần `onnxruntime-genai` và tải trọng số mô hình từ Hugging Face.

## File Không Nên Commit

`.gitignore` phải loại trừ (và hầu hết đã làm):
- `.venv/` — môi trường ảo Python
- `node_modules/` — phụ thuộc npm
- `models/` — đầu ra mô hình ONNX biên dịch (file nhị phân lớn, tạo bởi Phần 10)
- `cache_dir/` — cache tải mô hình Hugging Face
- `.olive-cache/` — thư mục làm việc Microsoft Olive
- `samples/audio/*.wav` — mẫu âm thanh tạo ra (tạo lại bằng `python samples/audio/generate_samples.py`)
- Các artifact xây dựng Python tiêu chuẩn (`__pycache__/`, `*.egg-info/`, `dist/`, v.v.)

## Giấy Phép

MIT — xem `LICENSE`.