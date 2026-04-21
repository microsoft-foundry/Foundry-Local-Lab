<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Hội thảo Foundry Local - Xây dựng ứng dụng AI trên thiết bị

Một hội thảo thực hành để chạy các mô hình ngôn ngữ trên máy của bạn và xây dựng các ứng dụng thông minh với [Foundry Local](https://foundrylocal.ai) và [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Foundry Local là gì?** Foundry Local là một runtime nhẹ cho phép bạn tải về, quản lý và phục vụ các mô hình ngôn ngữ hoàn toàn trên phần cứng của bạn. Nó cung cấp một **API tương thích OpenAI** để bất kỳ công cụ hay SDK nào hỗ trợ OpenAI có thể kết nối - không cần tài khoản đám mây.

---

## Mục tiêu học tập

Cuối buổi hội thảo này, bạn sẽ có thể:

| # | Mục tiêu |
|---|-----------|
| 1 | Cài đặt Foundry Local và quản lý các mô hình bằng CLI |
| 2 | Thông thạo API SDK Foundry Local để quản lý mô hình lập trình |
| 3 | Kết nối đến máy chủ suy diễn cục bộ bằng SDK Python, JavaScript và C# |
| 4 | Xây dựng pipeline Retrieval-Augmented Generation (RAG) dựa trên dữ liệu riêng của bạn |
| 5 | Tạo ra các tác nhân AI với các hướng dẫn và nhân cách cố định |
| 6 | Điều phối các quy trình làm việc đa tác nhân với vòng phản hồi |
| 7 | Khám phá ứng dụng thực tế cuối kỳ - Zava Creative Writer |
| 8 | Xây dựng framework đánh giá với bộ dữ liệu chuẩn và điểm số LLM-as-judge |
| 9 | Phiên âm âm thanh với Whisper - chuyển giọng nói thành văn bản trên thiết bị bằng Foundry Local SDK |
| 10 | Biên dịch và chạy các mô hình tùy chỉnh hoặc Hugging Face với ONNX Runtime GenAI và Foundry Local |
| 11 | Cho phép các mô hình cục bộ gọi đến các hàm bên ngoài với mẫu gọi công cụ |
| 12 | Xây dựng giao diện người dùng trên trình duyệt cho Zava Creative Writer với streaming thời gian thực |

---

## Yêu cầu

| Yêu cầu | Chi tiết |
|-------------|---------|
| **Phần cứng** | Tối thiểu 8 GB RAM (khuyến nghị 16 GB); CPU hỗ trợ AVX2 hoặc GPU tương thích |
| **Hệ điều hành** | Windows 10/11 (x64/ARM), Windows Server 2025 hoặc macOS 13+ |
| **Foundry Local CLI** | Cài đặt bằng `winget install Microsoft.FoundryLocal` (Windows) hoặc `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Xem hướng dẫn [bắt đầu](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) để biết chi tiết. |
| **Ngôn ngữ runtime** | **Python 3.9+** và/hoặc **.NET 9.0+** và/hoặc **Node.js 18+** |
| **Git** | Để clone kho lưu trữ này |

---

## Bắt đầu

```bash
# 1. Sao chép kho lưu trữ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Xác nhận Foundry Local đã được cài đặt
foundry model list              # Liệt kê các mô hình có sẵn
foundry model run phi-3.5-mini  # Bắt đầu trò chuyện tương tác

# 3. Chọn ngôn ngữ bạn muốn học (xem Phần 2 phòng thí nghiệm để cài đặt đầy đủ)
```

| Ngôn ngữ | Bắt đầu nhanh |
|----------|---------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Các phần của hội thảo

### Phần 1: Bắt đầu với Foundry Local

**Hướng dẫn lab:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local là gì và nó hoạt động thế nào
- Cài đặt CLI trên Windows và macOS
- Khám phá mô hình - liệt kê, tải xuống, chạy
- Hiểu alias của mô hình và cổng động

---

### Phần 2: Tìm hiểu sâu về Foundry Local SDK

**Hướng dẫn lab:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Tại sao dùng SDK hơn CLI để phát triển ứng dụng
- Tham khảo toàn bộ API SDK cho Python, JavaScript và C#
- Quản lý dịch vụ, duyệt danh mục, vòng đời mô hình (tải xuống, nạp, giải phóng)
- Các mẫu bắt đầu nhanh: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Metadata `FoundryModelInfo`, alias và chọn mô hình tối ưu cho phần cứng

---

### Phần 3: SDK và API

**Hướng dẫn lab:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Kết nối với Foundry Local từ Python, JavaScript và C#
- Dùng Foundry Local SDK để quản lý dịch vụ lập trình
- Streaming chat completion qua API tương thích OpenAI
- Tham khảo các phương thức SDK cho từng ngôn ngữ

**Ví dụ mã:**

| Ngôn ngữ | File | Mô tả |
|----------|------|-------|
| Python | `python/foundry-local.py` | Chat streaming cơ bản |
| C# | `csharp/BasicChat.cs` | Chat streaming với .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat streaming với Node.js |

---

### Phần 4: Retrieval-Augmented Generation (RAG)

**Hướng dẫn lab:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG là gì và tại sao quan trọng
- Xây dựng cơ sở tri thức trong bộ nhớ
- Truy xuất dựa trên chồng chéo từ khóa kèm điểm số
- Soạn thảo các prompt hệ thống căn cứ
- Chạy pipeline RAG hoàn chỉnh trên thiết bị

**Ví dụ mã:**

| Ngôn ngữ | File |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Phần 5: Xây dựng các tác nhân AI

**Hướng dẫn lab:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Tác nhân AI là gì (so với gọi trực tiếp LLM)
- Mẫu `ChatAgent` và Microsoft Agent Framework
- Hướng dẫn hệ thống, nhân cách và hội thoại đa lượt
- Đầu ra có cấu trúc (JSON) từ các tác nhân

**Ví dụ mã:**

| Ngôn ngữ | File | Mô tả |
|----------|------|-------|
| Python | `python/foundry-local-with-agf.py` | Tác nhân đơn với Agent Framework |
| C# | `csharp/SingleAgent.cs` | Tác nhân đơn (mẫu ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Tác nhân đơn (mẫu ChatAgent) |

---

### Phần 6: Quy trình làm việc đa tác nhân

**Hướng dẫn lab:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline đa tác nhân: Researcher → Writer → Editor
- Điều phối tuần tự và vòng phản hồi
- Cấu hình chia sẻ và bàn giao có cấu trúc
- Thiết kế quy trình đa tác nhân của bạn

**Ví dụ mã:**

| Ngôn ngữ | File | Mô tả |
|----------|------|-------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline ba tác nhân |
| C# | `csharp/MultiAgent.cs` | Pipeline ba tác nhân |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline ba tác nhân |

---

### Phần 7: Zava Creative Writer - Ứng dụng Capstone

**Hướng dẫn lab:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Ứng dụng đa tác nhân kiểu sản xuất với 4 tác nhân chuyên biệt
- Pipeline tuần tự với vòng phản hồi dựa trên người đánh giá
- Streaming đầu ra, tìm kiếm danh mục sản phẩm, bàn giao JSON có cấu trúc
- Triển khai hoàn chỉnh bằng Python (FastAPI), JavaScript (Node.js CLI) và C# (.NET console)

**Ví dụ mã:**

| Ngôn ngữ | Thư mục | Mô tả |
|----------|---------|-------|
| Python | `zava-creative-writer-local/src/api/` | Dịch vụ web FastAPI với bộ điều phối |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Ứng dụng CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Ứng dụng console .NET 9 |

---

### Phần 8: Phát triển dẫn dắt bằng đánh giá

**Hướng dẫn lab:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Xây dựng framework đánh giá có hệ thống cho tác nhân AI sử dụng bộ dữ liệu vàng
- Kiểm tra theo quy tắc (độ dài, phủ từ khóa, từ cấm) + điểm số LLM-as-judge
- So sánh song song các biến thể prompt với bảng điểm tổng hợp
- Mở rộng mẫu tác nhân Zava Editor từ Phần 7 thành bộ kiểm tra offline
- Theo dõi Python, JavaScript và C#

**Ví dụ mã:**

| Ngôn ngữ | File | Mô tả |
|----------|------|-------|
| Python | `python/foundry-local-eval.py` | Framework đánh giá |
| C# | `csharp/AgentEvaluation.cs` | Framework đánh giá |
| JavaScript | `javascript/foundry-local-eval.mjs` | Framework đánh giá |

---

### Phần 9: Phiên âm giọng nói với Whisper

**Hướng dẫn lab:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Phiên âm giọng nói sang văn bản bằng OpenAI Whisper chạy cục bộ
- Xử lý âm thanh ưu tiên riêng tư - âm thanh không rời thiết bị của bạn
- Theo dõi Python, JavaScript và C# với `client.audio.transcriptions.create()` (Python/JS) và `AudioClient.TranscribeAudioAsync()` (C#)
- Bao gồm các file âm thanh mẫu theo chủ đề Zava để thực hành

**Ví dụ mã:**

| Ngôn ngữ | File | Mô tả |
|----------|------|-------|
| Python | `python/foundry-local-whisper.py` | Phiên âm giọng nói Whisper |
| C# | `csharp/WhisperTranscription.cs` | Phiên âm giọng nói Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Phiên âm giọng nói Whisper |

> **Lưu ý:** Lab này sử dụng **Foundry Local SDK** để tải xuống và nạp mô hình Whisper lập trình, sau đó gửi âm thanh tới endpoint tương thích OpenAI cục bộ để phiên âm. Mô hình Whisper (`whisper`) có trong danh mục Foundry Local và chạy hoàn toàn trên thiết bị - không cần khóa API đám mây hay truy cập mạng.

---

### Phần 10: Sử dụng mô hình tùy chỉnh hoặc Hugging Face

**Hướng dẫn lab:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Biên dịch mô hình Hugging Face sang định dạng ONNX tối ưu bằng ONNX Runtime GenAI model builder
- Biên dịch theo phần cứng (CPU, NVIDIA GPU, DirectML, WebGPU) và lượng tử hóa (int4, fp16, bf16)
- Tạo file cấu hình chat-template cho Foundry Local
- Thêm mô hình đã biên dịch vào cache Foundry Local
- Chạy các mô hình tùy chỉnh qua CLI, REST API và OpenAI SDK
- Ví dụ tham khảo: biên dịch Qwen/Qwen3-0.6B end-to-end

---

### Phần 11: Gọi công cụ với mô hình cục bộ

**Hướng dẫn lab:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Cho phép mô hình cục bộ gọi các hàm bên ngoài (gọi công cụ/hàm)
- Định nghĩa schema công cụ theo định dạng gọi hàm OpenAI
- Xử lý luồng hội thoại gọi công cụ đa lượt
- Thực thi gọi công cụ tại chỗ và trả kết quả cho mô hình
- Chọn mô hình phù hợp cho kịch bản gọi công cụ (Qwen 2.5, Phi-4-mini)
- Dùng `ChatClient` gốc của SDK để gọi công cụ (JavaScript)

**Ví dụ mã:**

| Ngôn ngữ | File | Mô tả |
|----------|------|-------|
| Python | `python/foundry-local-tool-calling.py` | Gọi công cụ với công cụ thời tiết/dân số |
| C# | `csharp/ToolCalling.cs` | Gọi công cụ với .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Gọi công cụ với ChatClient |

---

### Phần 12: Xây dựng giao diện web cho Zava Creative Writer

**Hướng dẫn lab:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Thêm giao diện front-end trên trình duyệt cho Zava Creative Writer
- Phục vụ UI chia sẻ từ Python (FastAPI), JavaScript (HTTP Node.js) và C# (ASP.NET Core)
- Tiêu thụ streaming NDJSON trong trình duyệt bằng Fetch API và ReadableStream
- Biểu tượng trạng thái tác nhân trực tiếp và streaming văn bản bài viết thời gian thực

**Mã nguồn (UI chia sẻ):**

| File | Mô tả |
|------|-------|
| `zava-creative-writer-local/ui/index.html` | Bố cục trang |
| `zava-creative-writer-local/ui/style.css` | Định kiểu |
| `zava-creative-writer-local/ui/app.js` | Bộ đọc stream và logic cập nhật DOM |

**Bổ sung backend:**

| Ngôn ngữ | File | Mô tả |
|----------|------|-------|
| Python | `zava-creative-writer-local/src/api/main.py` | Cập nhật để phục vụ UI tĩnh |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | HTTP server mới bọc bộ điều phối |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Dự án API ASP.NET Core minimal mới |

---

### Phần 13: Hoàn thành hội thảo
**Hướng dẫn thực hành:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Tóm tắt tất cả những gì bạn đã xây dựng qua 12 phần
- Ý tưởng mở rộng thêm cho các ứng dụng của bạn
- Liên kết tới các tài nguyên và tài liệu

---

## Cấu trúc Dự án

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Tài nguyên

| Tài nguyên | Liên kết |
|----------|------|
| Trang web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Danh mục mô hình | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Hướng dẫn bắt đầu | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Tham khảo SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Giấy phép

Tài liệu hội thảo này được cung cấp cho mục đích giáo dục.

---

**Chúc bạn xây dựng thành công! 🚀**