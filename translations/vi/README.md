<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Hội Thảo Foundry Local - Xây Dựng Ứng Dụng AI Trên Thiết Bị

Một hội thảo thực hành về chạy các mô hình ngôn ngữ trên máy của bạn và xây dựng các ứng dụng thông minh với [Foundry Local](https://foundrylocal.ai) và [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Foundry Local là gì?** Foundry Local là một runtime nhẹ cho phép bạn tải xuống, quản lý và phục vụ các mô hình ngôn ngữ hoàn toàn trên phần cứng của bạn. Nó cung cấp một **API tương thích với OpenAI** để bất kỳ công cụ hoặc SDK nào hỗ trợ OpenAI đều có thể kết nối - không cần tài khoản đám mây.

### 🌐 Hỗ Trợ Đa Ngôn Ngữ

#### Hỗ trợ qua GitHub Action (Tự động & Luôn Cập Nhật)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Tiếng Ả Rập](../ar/README.md) | [Tiếng Bengal](../bn/README.md) | [Tiếng Bulgaria](../bg/README.md) | [Tiếng Miến Điện (Myanmar)](../my/README.md) | [Tiếng Trung (Giản Thể)](../zh-CN/README.md) | [Tiếng Trung (Phồn Thể, Hong Kong)](../zh-HK/README.md) | [Tiếng Trung (Phồn Thể, Macau)](../zh-MO/README.md) | [Tiếng Trung (Phồn Thể, Đài Loan)](../zh-TW/README.md) | [Tiếng Croatia](../hr/README.md) | [Tiếng Séc](../cs/README.md) | [Tiếng Đan Mạch](../da/README.md) | [Tiếng Hà Lan](../nl/README.md) | [Tiếng Estonia](../et/README.md) | [Tiếng Phần Lan](../fi/README.md) | [Tiếng Pháp](../fr/README.md) | [Tiếng Đức](../de/README.md) | [Tiếng Hy Lạp](../el/README.md) | [Tiếng Do Thái](../he/README.md) | [Tiếng Hindi](../hi/README.md) | [Tiếng Hungary](../hu/README.md) | [Tiếng Indonesia](../id/README.md) | [Tiếng Ý](../it/README.md) | [Tiếng Nhật](../ja/README.md) | [Tiếng Kannada](../kn/README.md) | [Tiếng Khmer](../km/README.md) | [Tiếng Hàn](../ko/README.md) | [Tiếng Lithuania](../lt/README.md) | [Tiếng Mã Lai](../ms/README.md) | [Tiếng Malayalam](../ml/README.md) | [Tiếng Marathi](../mr/README.md) | [Tiếng Nepal](../ne/README.md) | [Pidgin Nigeria](../pcm/README.md) | [Tiếng Na Uy](../no/README.md) | [Tiếng Ba Tư (Farsi)](../fa/README.md) | [Tiếng Ba Lan](../pl/README.md) | [Tiếng Bồ Đào Nha (Brazil)](../pt-BR/README.md) | [Tiếng Bồ Đào Nha (Bồ Đào Nha)](../pt-PT/README.md) | [Tiếng Punjabi (Gurmukhi)](../pa/README.md) | [Tiếng Romania](../ro/README.md) | [Tiếng Nga](../ru/README.md) | [Tiếng Serbia (Chữ Cyrillic)](../sr/README.md) | [Tiếng Slovakia](../sk/README.md) | [Tiếng Slovenia](../sl/README.md) | [Tiếng Tây Ban Nha](../es/README.md) | [Tiếng Swahili](../sw/README.md) | [Tiếng Thụy Điển](../sv/README.md) | [Tiếng Tagalog (Philippines)](../tl/README.md) | [Tiếng Tamil](../ta/README.md) | [Tiếng Telugu](../te/README.md) | [Tiếng Thái](../th/README.md) | [Tiếng Thổ Nhĩ Kỳ](../tr/README.md) | [Tiếng Ukraina](../uk/README.md) | [Tiếng Urdu](../ur/README.md) | [Tiếng Việt](./README.md)

> **Muốn Clone về máy?**
>
> Kho lưu trữ này bao gồm hơn 50 bản dịch ngôn ngữ, làm tăng đáng kể kích thước tải xuống. Để clone mà không có bản dịch, hãy dùng sparse checkout:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Cách này cung cấp mọi thứ bạn cần để hoàn thành khoá học với tốc độ tải xuống nhanh hơn nhiều.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Mục Tiêu Học Tập

Kết thúc hội thảo, bạn sẽ có khả năng:

| # | Mục Tiêu |
|---|-----------|
| 1 | Cài đặt Foundry Local và quản lý mô hình với CLI |
| 2 | Thành thạo API Foundry Local SDK để quản lý mô hình lập trình |
| 3 | Kết nối tới máy chủ suy luận cục bộ bằng SDK Python, JavaScript, và C# |
| 4 | Xây dựng pipeline Retrieval-Augmented Generation (RAG) lấy câu trả lời dựa trên dữ liệu của riêng bạn |
| 5 | Tạo các tác nhân AI với hướng dẫn và nhân cách cố định |
| 6 | Điều phối các workflow đa tác nhân với vòng phản hồi |
| 7 | Khám phá ứng dụng capstone sản xuất - Zava Creative Writer |
| 8 | Xây dựng framework đánh giá với bộ dữ liệu vàng và chấm điểm LLM-as-judge |
| 9 | Ghi âm chuyển đổi giọng nói sang chữ viết với Whisper - trên thiết bị dùng Foundry Local SDK |
| 10 | Biên dịch và chạy các mô hình tùy chỉnh hoặc Hugging Face với ONNX Runtime GenAI và Foundry Local |
| 11 | Cho phép các mô hình cục bộ gọi hàm bên ngoài với mẫu tool-calling |
| 12 | Xây dựng giao diện trình duyệt cho Zava Creative Writer với truyền dữ liệu thời gian thực |

---

## Yêu Cầu Tiền Đề

| Yêu Cầu | Chi Tiết |
|-------------|---------|
| **Phần cứng** | Tối thiểu 8 GB RAM (khuyên dùng 16 GB); CPU hỗ trợ AVX2 hoặc GPU được hỗ trợ |
| **Hệ điều hành** | Windows 10/11 (x64/ARM), Windows Server 2025, hoặc macOS 13 trở lên |
| **CLI Foundry Local** | Cài đặt qua `winget install Microsoft.FoundryLocal` (Windows) hoặc `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Xem hướng dẫn [bắt đầu](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) để biết chi tiết. |
| **Runtime ngôn ngữ** | **Python 3.9+** và/hoặc **.NET 9.0+** và/hoặc **Node.js 18+** |
| **Git** | Để clone kho lưu trữ này |

---

## Bắt Đầu

```bash
# 1. Sao chép kho lưu trữ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Xác minh rằng Foundry Local đã được cài đặt
foundry model list              # Liệt kê các mô hình có sẵn
foundry model run phi-3.5-mini  # Bắt đầu một cuộc trò chuyện tương tác

# 3. Chọn ngôn ngữ bạn muốn (xem Phần 2 phòng thí nghiệm để thiết lập đầy đủ)
```

| Ngôn ngữ | Khởi Đầu Nhanh |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Phần Hội Thảo

### Phần 1: Bắt Đầu với Foundry Local

**Hướng dẫn lab:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local là gì và cách nó hoạt động
- Cài đặt CLI trên Windows và macOS
- Khám phá các mô hình - liệt kê, tải xuống, chạy
- Hiểu về bí danh mô hình và cổng động

---

### Phần 2: Tìm Hiểu Sâu về Foundry Local SDK

**Hướng dẫn lab:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Tại sao sử dụng SDK thay vì CLI để phát triển ứng dụng
- Tham khảo API SDK đầy đủ cho Python, JavaScript và C#
- Quản lý dịch vụ, duyệt danh mục, vòng đời mô hình (tải xuống, tải vào, gỡ bỏ)
- Mẫu khởi động nhanh: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Metadata `FoundryModelInfo`, bí danh và lựa chọn mô hình tối ưu phần cứng

---

### Phần 3: SDK và API

**Hướng dẫn lab:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Kết nối tới Foundry Local từ Python, JavaScript và C#
- Sử dụng Foundry Local SDK để quản lý dịch vụ lập trình
- Truyền phát trò chuyện hoàn thành qua API tương thích OpenAI
- Tham khảo phương thức SDK cho từng ngôn ngữ

**Ví dụ code:**

| Ngôn ngữ | Tệp | Mô tả |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Trò chuyện truyền phát cơ bản |
| C# | `csharp/BasicChat.cs` | Trò chuyện truyền phát với .NET |
| JavaScript | `javascript/foundry-local.mjs` | Trò chuyện truyền phát với Node.js |

---

### Phần 4: Retrieval-Augmented Generation (RAG)

**Hướng dẫn lab:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG là gì và tại sao nó quan trọng
- Xây dựng cơ sở tri thức trong bộ nhớ
- Tìm kiếm chồng lặp từ khoá với chấm điểm
- Soạn prompt hệ thống có căn cứ
- Chạy pipeline RAG đầy đủ trên thiết bị

**Ví dụ code:**

| Ngôn ngữ | Tệp |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Phần 5: Xây Dựng Các Tác Nhân AI

**Hướng dẫn lab:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Tác nhân AI là gì (so với gọi LLM thô)
- Mẫu `ChatAgent` và Microsoft Agent Framework
- Hướng dẫn hệ thống, nhân cách, và hội thoại đa lượt
- Đầu ra có cấu trúc (JSON) từ các tác nhân

**Ví dụ code:**

| Ngôn ngữ | Tệp | Mô tả |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Tác nhân đơn với Agent Framework |
| C# | `csharp/SingleAgent.cs` | Tác nhân đơn (mẫu ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Tác nhân đơn (mẫu ChatAgent) |

---

### Phần 6: Workflow Đa Tác Nhân

**Hướng dẫn lab:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline đa tác nhân: Nhà nghiên cứu → Người viết → Biên tập viên
- Điều phối tuần tự và vòng phản hồi
- Cấu hình chung và bàn giao có cấu trúc
- Thiết kế workflow đa tác nhân riêng của bạn

**Ví dụ code:**

| Ngôn ngữ | Tệp | Mô tả |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline ba tác nhân |
| C# | `csharp/MultiAgent.cs` | Pipeline ba tác nhân |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline ba tác nhân |

---

### Phần 7: Zava Creative Writer - Ứng Dụng Capstone

**Hướng dẫn lab:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Ứng dụng đa tác nhân kiểu sản xuất với 4 tác nhân chuyên biệt
- Pipeline tuần tự với vòng phản hồi do người đánh giá dẫn dắt
- Đầu ra truyền phát, tìm kiếm danh mục sản phẩm, bàn giao JSON có cấu trúc
- Triển khai đầy đủ bằng Python (FastAPI), JavaScript (Node.js CLI), và C# (console .NET)

**Ví dụ code:**

| Ngôn ngữ | Thư mục | Mô tả |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Dịch vụ web FastAPI với bộ điều phối |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Ứng dụng CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Ứng dụng console .NET 9 |

---

### Phần 8: Phát Triển Dựa Trên Đánh Giá

**Hướng dẫn lab:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Xây dựng framework đánh giá hệ thống cho các tác nhân AI với bộ dữ liệu vàng
- Kiểm tra dựa trên luật (độ dài, phủ từ khoá, từ cấm) + chấm điểm LLM-as-judge
- So sánh song song các biến thể prompt với bảng điểm tổng hợp
- Mở rộng mẫu tác nhân Zava Editor từ Phần 7 thành bộ kiểm thử offline
- Các track Python, JavaScript và C#

**Ví dụ code:**

| Ngôn ngữ | Tệp | Mô tả |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Framework đánh giá |
| C# | `csharp/AgentEvaluation.cs` | Framework đánh giá |
| JavaScript | `javascript/foundry-local-eval.mjs` | Framework đánh giá |

---

### Phần 9: Chuyển Đổi Giọng Nói Sang Văn Bản với Whisper

**Hướng dẫn lab:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Chuyển giọng nói thành văn bản sử dụng OpenAI Whisper chạy cục bộ  
- Xử lý âm thanh ưu tiên bảo mật - âm thanh không bao giờ rời khỏi thiết bị của bạn  
- Các track Python, JavaScript và C# với `client.audio.transcriptions.create()` (Python/JS) và `AudioClient.TranscribeAudioAsync()` (C#)  
- Bao gồm các file âm thanh mẫu chủ đề Zava để thực hành trực tiếp  

**Ví dụ mã nguồn:**  

| Ngôn ngữ | File | Mô tả |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | Chuyển giọng nói bằng Whisper |  
| C# | `csharp/WhisperTranscription.cs` | Chuyển giọng nói bằng Whisper |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | Chuyển giọng nói bằng Whisper |  

> **Lưu ý:** Phòng lab này sử dụng **Foundry Local SDK** để lập trình tải xuống và nạp mô hình Whisper, sau đó gửi âm thanh tới điểm cuối tương thích OpenAI cục bộ để chuyển đổi thành văn bản. Mô hình Whisper (`whisper`) được liệt kê trong danh mục Foundry Local và chạy hoàn toàn trên thiết bị - không cần khóa API đám mây hoặc truy cập mạng.  

---

### Phần 10: Sử dụng Mô hình Tuỳ chỉnh hoặc Hugging Face  

**Hướng dẫn lab:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- Biên dịch các mô hình Hugging Face sang định dạng ONNX tối ưu bằng trình xây dựng mô hình ONNX Runtime GenAI  
- Biên dịch theo phần cứng cụ thể (CPU, NVIDIA GPU, DirectML, WebGPU) và lượng tử hoá (int4, fp16, bf16)  
- Tạo file cấu hình mẫu chat cho Foundry Local  
- Thêm mô hình đã biên dịch vào bộ nhớ đệm Foundry Local  
- Chạy các mô hình tùy chỉnh qua CLI, REST API và OpenAI SDK  
- Ví dụ tham khảo: biên dịch Qwen/Qwen3-0.6B đầu cuối  

---

### Phần 11: Gọi Công cụ với Mô hình Cục bộ  

**Hướng dẫn lab:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- Cho phép mô hình cục bộ gọi các hàm bên ngoài (gọi công cụ/hàm)  
- Định nghĩa schema công cụ theo định dạng gọi hàm OpenAI  
- Xử lý luồng hội thoại gọi công cụ đa lượt  
- Thực thi gọi công cụ cục bộ và trả kết quả cho mô hình  
- Chọn mô hình phù hợp cho các kịch bản gọi công cụ (Qwen 2.5, Phi-4-mini)  
- Sử dụng `ChatClient` có sẵn trong SDK để gọi công cụ (JavaScript)  

**Ví dụ mã nguồn:**  

| Ngôn ngữ | File | Mô tả |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | Gọi công cụ với công cụ thời tiết/dân số |  
| C# | `csharp/ToolCalling.cs` | Gọi công cụ với .NET |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Gọi công cụ với ChatClient |  

---

### Phần 12: Xây dựng Giao diện Web cho Zava Creative Writer  

**Hướng dẫn lab:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- Thêm front end chạy trên trình duyệt cho Zava Creative Writer  
- Phục vụ UI dùng chung từ Python (FastAPI), JavaScript (Node.js HTTP), và C# (ASP.NET Core)  
- Tiêu thụ streaming NDJSON trên trình duyệt với Fetch API và ReadableStream  
- Biểu tượng trạng thái tác vụ và phát trực tiếp văn bản bài viết ở thời gian thực  

**Mã nguồn (UI dùng chung):**  

| File | Mô tả |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | Bố cục trang |  
| `zava-creative-writer-local/ui/style.css` | Định kiểu |  
| `zava-creative-writer-local/ui/app.js` | Đọc stream và logic cập nhật DOM |  

**Bổ sung backend:**  

| Ngôn ngữ | File | Mô tả |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | Cập nhật để phục vụ UI tĩnh |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Server HTTP mới bao bọc orchestrator |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Dự án API minimal ASP.NET Core mới |  

---

### Phần 13: Hoàn tất Workshop  

**Hướng dẫn lab:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- Tóm tắt tất cả những gì bạn đã xây dựng trong 12 phần  
- Ý tưởng mở rộng ứng dụng  
- Liên kết tới tài nguyên và tài liệu  

---

## Cấu trúc dự án  

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

Tài liệu workshop này được cung cấp cho mục đích giáo dục.  

---

**Chúc bạn xây dựng thành công! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Từ chối trách nhiệm**:  
Tài liệu này đã được dịch bằng dịch vụ dịch thuật AI [Co-op Translator](https://github.com/Azure/co-op-translator). Mặc dù chúng tôi cố gắng đảm bảo độ chính xác, xin lưu ý rằng bản dịch tự động có thể chứa lỗi hoặc không chính xác. Tài liệu gốc bằng ngôn ngữ bản địa vẫn được coi là nguồn đáng tin cậy. Đối với các thông tin quan trọng, khuyến nghị sử dụng dịch vụ dịch thuật chuyên nghiệp của con người. Chúng tôi không chịu trách nhiệm về bất kỳ sự hiểu nhầm hoặc diễn giải sai nào phát sinh từ việc sử dụng bản dịch này.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->