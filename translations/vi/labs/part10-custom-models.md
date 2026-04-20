![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Phần 10: Sử dụng mô hình tùy chỉnh hoặc Hugging Face với Foundry Local

> **Mục tiêu:** Biên dịch một mô hình Hugging Face thành định dạng ONNX tối ưu mà Foundry Local yêu cầu, cấu hình nó với mẫu chat, thêm nó vào bộ nhớ đệm cục bộ, và chạy suy luận với mô hình này bằng CLI, REST API, và OpenAI SDK.

## Tổng quan

Foundry Local đi kèm với một danh mục các mô hình được biên dịch trước đã được lựa chọn kỹ lưỡng, nhưng bạn không bị giới hạn bởi danh sách đó. Bất kỳ mô hình ngôn ngữ dựa trên transformer nào có sẵn trên [Hugging Face](https://huggingface.co/) (hoặc lưu trữ cục bộ dưới định dạng PyTorch / Safetensors) đều có thể được biên dịch thành mô hình ONNX tối ưu và phục vụ thông qua Foundry Local.

Quy trình biên dịch sử dụng **ONNX Runtime GenAI Model Builder**, một công cụ dòng lệnh được bao gồm trong gói `onnxruntime-genai`. Trình tạo mô hình đảm nhận các công việc nặng nhọc: tải trọng số nguồn, chuyển đổi sang định dạng ONNX, áp dụng định lượng (int4, fp16, bf16) và tạo ra các tập tin cấu hình (bao gồm mẫu chat và bộ phân tích token) mà Foundry Local mong đợi.

Trong phòng thí nghiệm này, bạn sẽ biên dịch **Qwen/Qwen3-0.6B** từ Hugging Face, đăng ký nó với Foundry Local và trò chuyện với nó hoàn toàn trên thiết bị của bạn.

---

## Mục tiêu học tập

Kết thúc phòng thí nghiệm này, bạn sẽ có khả năng:

- Giải thích tại sao việc biên dịch mô hình tùy chỉnh hữu ích và khi nào bạn có thể cần nó
- Cài đặt trình tạo mô hình ONNX Runtime GenAI
- Biên dịch mô hình Hugging Face sang định dạng ONNX tối ưu chỉ với một lệnh duy nhất
- Hiểu các tham số quan trọng trong quá trình biên dịch (nhà cung cấp thực thi, độ chính xác)
- Tạo tập tin cấu hình mẫu chat `inference_model.json`
- Thêm mô hình đã biên dịch vào bộ nhớ đệm Foundry Local
- Chạy suy luận với mô hình tùy chỉnh bằng CLI, REST API, và OpenAI SDK

---

## Yêu cầu trước

| Yêu cầu | Chi tiết |
|-------------|---------|
| **Foundry Local CLI** | Đã cài đặt và có trong `PATH` của bạn ([Phần 1](part1-getting-started.md)) |
| **Python 3.10+** | Yêu cầu cho trình tạo mô hình ONNX Runtime GenAI |
| **pip** | Trình quản lý gói Python |
| **Dung lượng ổ đĩa** | Tối thiểu 5 GB trống cho tệp nguồn và mô hình đã biên dịch |
| **Tài khoản Hugging Face** | Một số mô hình yêu cầu bạn chấp nhận giấy phép trước khi tải xuống. Qwen3-0.6B sử dụng giấy phép Apache 2.0 và có sẵn miễn phí. |

---

## Thiết lập môi trường

Việc biên dịch mô hình yêu cầu nhiều gói Python lớn (PyTorch, ONNX Runtime GenAI, Transformers). Tạo một môi trường ảo riêng biệt để những gói này không xung đột với Python hệ thống hoặc dự án khác.

```bash
# Từ thư mục gốc của kho lưu trữ
python -m venv .venv
```

Kích hoạt môi trường:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Nâng cấp pip để tránh các vấn đề liên quan đến giải quyết phụ thuộc:

```bash
python -m pip install --upgrade pip
```

> **Mẹo:** Nếu bạn đã có `.venv` từ các phòng thí nghiệm trước, bạn có thể tái sử dụng nó. Chỉ cần chắc chắn nó được kích hoạt trước khi tiếp tục.

---

## Khái niệm: Quy trình biên dịch

Foundry Local yêu cầu các mô hình ở định dạng ONNX với cấu hình ONNX Runtime GenAI. Hầu hết các mô hình mã nguồn mở trên Hugging Face được phân phối dưới dạng trọng số PyTorch hoặc Safetensors, vì vậy cần một bước chuyển đổi.

![Quy trình biên dịch mô hình tùy chỉnh](../../../images/custom-model-pipeline.svg)

### Trình tạo mô hình làm gì?

1. **Tải xuống** mô hình nguồn từ Hugging Face (hoặc đọc từ đường dẫn cục bộ).
2. **Chuyển đổi** trọng số PyTorch / Safetensors sang định dạng ONNX.
3. **Định lượng** mô hình ở độ chính xác nhỏ hơn (ví dụ int4) để giảm sử dụng bộ nhớ và cải thiện thông lượng.
4. **Tạo ra** cấu hình ONNX Runtime GenAI (`genai_config.json`), mẫu chat (`chat_template.jinja`), và tất cả các tập tin tokeniser để Foundry Local có thể tải và phục vụ mô hình.

### ONNX Runtime GenAI Model Builder và Microsoft Olive

Bạn có thể gặp tham chiếu đến **Microsoft Olive** như một công cụ thay thế để tối ưu hóa mô hình. Cả hai công cụ đều có thể tạo ra mô hình ONNX, nhưng chúng phục vụ mục đích khác nhau và có những đánh đổi khác nhau:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Gói** | `onnxruntime-genai` | `olive-ai` |
| **Mục đích chính** | Chuyển đổi và định lượng mô hình AI tạo sinh cho suy luận ONNX Runtime GenAI | Khung tối ưu hóa mô hình đầu cuối hỗ trợ nhiều backend và mục tiêu phần cứng |
| **Dễ sử dụng** | Lệnh đơn — chuyển đổi + định lượng một bước | Dựa trên quy trình làm việc — các pipeline đa bước có thể cấu hình qua YAML/JSON |
| **Định dạng đầu ra** | Định dạng ONNX Runtime GenAI (sẵn sàng cho Foundry Local) | ONNX tổng quát, ONNX Runtime GenAI, hoặc định dạng khác tùy quy trình |
| **Phần cứng hỗ trợ** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, và nhiều hơn nữa |
| **Tùy chọn định lượng** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, cùng với tối ưu đồ thị, tinh chỉnh theo lớp |
| **Phạm vi mô hình** | Các mô hình AI tạo sinh (LLMs, SLMs) | Bất kỳ mô hình có thể chuyển đổi sang ONNX (thị giác, NLP, âm thanh, đa mô thức) |
| **Phù hợp để** | Biên dịch nhanh cho một mô hình cho suy luận cục bộ | Pipeline sản xuất cần kiểm soát tối ưu tinh vi |
| **Phụ thuộc** | Trung bình (PyTorch, Transformers, ONNX Runtime) | Lớn hơn (thêm framework Olive và các tiện ích tùy chọn theo workflow) |
| **Tích hợp Foundry Local** | Trực tiếp — đầu ra ngay lập tức tương thích | Cần cờ `--use_ort_genai` và cấu hình bổ sung |

> **Tại sao phòng thí nghiệm này dùng Model Builder:** Với nhiệm vụ biên dịch một mô hình Hugging Face đơn lẻ và đăng ký với Foundry Local, Model Builder là con đường đơn giản và đáng tin cậy nhất. Nó tạo ra định dạng đầu ra chính xác mà Foundry Local mong đợi chỉ với một lệnh. Nếu bạn sau này cần các tính năng tối ưu nâng cao — như định lượng có nhận thức độ chính xác, cắt ghép đồ thị, hay tinh chỉnh đa bước — Olive là lựa chọn mạnh để khám phá. Xem [tài liệu Microsoft Olive](https://microsoft.github.io/Olive/) để biết thêm chi tiết.

---

## Bài tập phòng thí nghiệm

### Bài tập 1: Cài đặt ONNX Runtime GenAI Model Builder

Cài đặt gói ONNX Runtime GenAI, bao gồm công cụ trình tạo mô hình:

```bash
pip install onnxruntime-genai
```

Xác minh cài đặt bằng cách kiểm tra xem trình tạo mô hình có sẵn:

```bash
python -m onnxruntime_genai.models.builder --help
```

Bạn sẽ thấy đầu ra trợ giúp liệt kê các tham số như `-m` (tên mô hình), `-o` (đường dẫn đầu ra), `-p` (độ chính xác), và `-e` (nhà cung cấp thực thi).

> **Lưu ý:** Trình tạo mô hình phụ thuộc vào PyTorch, Transformers, và một số gói khác. Việc cài đặt có thể mất vài phút.

---

### Bài tập 2: Biên dịch Qwen3-0.6B cho CPU

Chạy lệnh sau để tải mô hình Qwen3-0.6B từ Hugging Face và biên dịch cho suy luận trên CPU với định lượng int4:

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell):**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### Mỗi tham số có tác dụng gì

| Tham số | Mục đích | Giá trị sử dụng |
|-----------|---------|------------|
| `-m` | ID mô hình Hugging Face hoặc đường dẫn thư mục cục bộ | `Qwen/Qwen3-0.6B` |
| `-o` | Thư mục nơi mô hình ONNX biên dịch được lưu | `models/qwen3` |
| `-p` | Độ chính xác định lượng áp dụng trong quá trình biên dịch | `int4` |
| `-e` | Nhà cung cấp thực thi ONNX Runtime (phần cứng mục tiêu) | `cpu` |
| `--extra_options hf_token=false` | Bỏ qua xác thực Hugging Face (được chấp nhận cho mô hình công khai) | `hf_token=false` |

> **Mất bao lâu?** Thời gian biên dịch phụ thuộc vào phần cứng và kích thước mô hình. Với Qwen3-0.6B và định lượng int4 trên CPU hiện đại, dự kiến khoảng 5 đến 15 phút. Mô hình lớn hơn mất thời gian tương ứng.

Khi lệnh hoàn thành, bạn sẽ thấy thư mục `models/qwen3` chứa các tập tin mô hình đã biên dịch. Kiểm tra kết quả:

```bash
ls models/qwen3
```

Bạn sẽ thấy các tệp bao gồm:
- `model.onnx` và `model.onnx.data` — trọng số mô hình đã biên dịch
- `genai_config.json` — cấu hình ONNX Runtime GenAI
- `chat_template.jinja` — mẫu chat của mô hình (tự động tạo)
- `tokenizer.json`, `tokenizer_config.json` — tệp tokeniser
- Các tệp từ vựng và cấu hình khác

---

### Bài tập 3: Biên dịch cho GPU (Tùy chọn)

Nếu bạn có GPU NVIDIA hỗ trợ CUDA, bạn có thể biên dịch biến thể tối ưu cho GPU để suy luận nhanh hơn:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Lưu ý:** Biên dịch GPU yêu cầu `onnxruntime-gpu` và cài đặt CUDA hoạt động. Nếu không có các thành phần này, trình tạo mô hình sẽ báo lỗi. Bạn có thể bỏ qua bài này và tiếp tục với biến thể CPU.

#### Tham khảo biên dịch theo phần cứng

| Mục tiêu | Nhà cung cấp thực thi (`-e`) | Độ chính xác khuyên dùng (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| GPU NVIDIA | `cuda` | `fp16` hoặc `int4` |
| DirectML (GPU Windows) | `dml` | `fp16` hoặc `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Đánh đổi độ chính xác

| Độ chính xác | Kích thước | Tốc độ | Chất lượng |
|-----------|------|-------|---------|
| `fp32` | Lớn nhất | Chậm nhất | Độ chính xác cao nhất |
| `fp16` | Lớn | Nhanh (GPU) | Độ chính xác rất tốt |
| `int8` | Nhỏ | Nhanh | Mất nhẹ độ chính xác |
| `int4` | Nhỏ nhất | Nhanh nhất | Mất độ chính xác trung bình |

Đối với hầu hết phát triển cục bộ, `int4` trên CPU cung cấp sự cân bằng tốt nhất giữa tốc độ và tài nguyên. Để có chất lượng sản xuất, khuyến nghị dùng `fp16` trên GPU CUDA.

---

### Bài tập 4: Tạo cấu hình mẫu chat

Trình tạo mô hình tự động tạo tệp `chat_template.jinja` và `genai_config.json` trong thư mục đầu ra. Tuy nhiên, Foundry Local cũng cần một tệp `inference_model.json` để hiểu cách định dạng lời nhắc cho mô hình của bạn. Tệp này định nghĩa tên mô hình và mẫu lời nhắc bao bọc các tin nhắn người dùng trong các token đặc biệt chính xác.

#### Bước 1: Kiểm tra kết quả biên dịch

Liệt kê nội dung thư mục mô hình đã biên dịch:

```bash
ls models/qwen3
```

Bạn sẽ thấy các tệp như:
- `model.onnx` và `model.onnx.data` — trọng số mô hình đã biên dịch
- `genai_config.json` — cấu hình ONNX Runtime GenAI (tự động tạo)
- `chat_template.jinja` — mẫu chat của mô hình (tự động tạo)
- `tokenizer.json`, `tokenizer_config.json` — tệp tokeniser
- Các tệp cấu hình và từ vựng khác

#### Bước 2: Tạo tệp inference_model.json

Tệp `inference_model.json` cho Foundry Local biết cách định dạng lời nhắc. Tạo một script Python tên `generate_chat_template.py` **trong thư mục gốc của kho mã nguồn** (cùng thư mục chứa thư mục `models/` của bạn):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Xây dựng một cuộc hội thoại tối giản để trích xuất mẫu chat
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# Xây dựng cấu trúc inference_model.json
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

Chạy script từ thư mục gốc của kho mã:

```bash
python generate_chat_template.py
```

> **Lưu ý:** Gói `transformers` đã được cài đặt tự động như phụ thuộc của `onnxruntime-genai`. Nếu bạn gặp lỗi `ImportError`, chạy `pip install transformers` trước.

Script tạo ra tập tin `inference_model.json` bên trong thư mục `models/qwen3`. Tệp này chỉ rõ cách Foundry Local bọc đầu vào người dùng với các token đặc biệt cho Qwen3.

> **Quan trọng:** Trường `"Name"` trong `inference_model.json` (được đặt là `qwen3-0.6b` trong script) là **bí danh mô hình** mà bạn sẽ dùng trong tất cả lệnh và cuộc gọi API sau này. Nếu bạn đổi tên này, hãy cập nhật tên mô hình trong các Bài tập 6–10 tương ứng.

#### Bước 3: Kiểm tra cấu hình

Mở `models/qwen3/inference_model.json` và kiểm tra xem nó có trường `Name` và một đối tượng `PromptTemplate` với các khóa `assistant` và `prompt` không. Mẫu lời nhắc nên bao gồm các token đặc biệt như `<|im_start|>` và `<|im_end|>` (các token chính xác phụ thuộc mẫu chat của mô hình).

> **Thay thế thủ công:** Nếu bạn không muốn chạy script, bạn có thể tạo tệp này thủ công. Yêu cầu chính là trường `prompt` chứa toàn bộ mẫu chat của mô hình với `{Content}` làm chỗ giữ chỗ cho tin nhắn của người dùng.

---

### Bài tập 5: Kiểm tra cấu trúc thư mục mô hình
Người xây dựng mô hình đặt tất cả các tệp đã biên dịch trực tiếp vào thư mục đầu ra mà bạn đã chỉ định. Xác nhận rằng cấu trúc cuối cùng trông đúng:

```bash
ls models/qwen3
```

Thư mục nên chứa các tệp sau:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **Lưu ý:** Khác với một số công cụ biên dịch khác, người xây dựng mô hình không tạo các thư mục con lồng nhau. Tất cả các tệp nằm trực tiếp trong thư mục đầu ra, điều này hoàn toàn phù hợp với mong đợi của Foundry Local.

---

### Bài tập 6: Thêm Mô hình vào Bộ nhớ đệm của Foundry Local

Bảo cho Foundry Local biết vị trí tìm mô hình đã biên dịch của bạn bằng cách thêm thư mục vào bộ nhớ đệm của nó:

```bash
foundry cache cd models/qwen3
```

Xác nhận rằng mô hình xuất hiện trong bộ nhớ đệm:

```bash
foundry cache ls
```

Bạn sẽ thấy mô hình tùy chỉnh của mình được liệt kê cùng với các mô hình đã được lưu trong bộ nhớ đệm trước đó (chẳng hạn như `phi-3.5-mini` hoặc `phi-4-mini`).

---

### Bài tập 7: Chạy Mô hình Tùy chỉnh với CLI

Bắt đầu phiên trò chuyện tương tác với mô hình mới được biên dịch của bạn (bí danh `qwen3-0.6b` lấy từ trường `Name` bạn đã đặt trong `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Cờ `--verbose` hiển thị thêm thông tin chẩn đoán, hữu ích khi thử nghiệm mô hình tùy chỉnh lần đầu. Nếu mô hình được tải thành công bạn sẽ thấy lời nhắc tương tác. Hãy thử gửi vài tin nhắn:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Gõ `exit` hoặc nhấn `Ctrl+C` để kết thúc phiên.

> **Khắc phục sự cố:** Nếu mô hình không tải được, kiểm tra các điều sau:
> - Tệp `genai_config.json` được tạo bởi người xây dựng mô hình.
> - Tệp `inference_model.json` tồn tại và có định dạng JSON hợp lệ.
> - Các tệp mô hình ONNX nằm ở thư mục chính xác.
> - Bạn có đủ RAM trống (Qwen3-0.6B int4 cần khoảng 1 GB).
> - Qwen3 là mô hình suy luận tạo ra các thẻ `<think>`. Nếu bạn thấy `<think>...</think>` tiền tố trong phản hồi, đây là hành vi bình thường. Mẫu lời nhắc trong `inference_model.json` có thể được điều chỉnh để ẩn đầu ra suy nghĩ này.

---

### Bài tập 8: Truy vấn Mô hình Tùy chỉnh qua REST API

Nếu bạn đã thoát phiên tương tác ở Bài tập 7, mô hình có thể không còn được tải. Hãy khởi động dịch vụ Foundry Local và tải lại mô hình trước:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Kiểm tra cổng mà dịch vụ đang chạy:

```bash
foundry service status
```

Sau đó gửi yêu cầu (thay `5273` bằng cổng của bạn nếu khác):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Lưu ý Windows:** Lệnh `curl` trên dùng cú pháp bash. Trên Windows, hãy dùng lệnh PowerShell `Invoke-RestMethod` bên dưới thay thế.

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### Bài tập 9: Sử dụng Mô hình Tùy chỉnh với OpenAI SDK

Bạn có thể kết nối tới mô hình tùy chỉnh của mình sử dụng chính mã OpenAI SDK bạn đã dùng với các mô hình tích hợp sẵn (xem [Phần 3](part3-sdk-and-apis.md)). Điểm khác duy nhất là tên mô hình.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local không xác thực khóa API
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local không xác thực các khóa API
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Điểm then chốt:** Vì Foundry Local cung cấp API tương thích OpenAI, bất kỳ mã nào làm việc với mô hình tích hợp đều hoạt động với mô hình tùy chỉnh của bạn. Bạn chỉ cần thay đổi tham số `model`.

---

### Bài tập 10: Kiểm tra Mô hình Tùy chỉnh với Foundry Local SDK

Trong các bài lab trước, bạn đã dùng Foundry Local SDK để khởi động dịch vụ, phát hiện điểm cuối, và quản lý mô hình tự động. Bạn có thể làm chính xác như vậy với mô hình tùy chỉnh đã biên dịch. SDK xử lý việc khởi động dịch vụ và phát hiện điểm cuối, nên mã của bạn không cần mã hóa cố định `localhost:5273`.

> **Lưu ý:** Đảm bảo đã cài Foundry Local SDK trước khi chạy các ví dụ này:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Thêm gói NuGet `Microsoft.AI.Foundry.Local` và `OpenAI`
>
> Lưu các tệp script **ở thư mục gốc của kho lưu trữ** (cùng thư mục với thư mục `models/` của bạn).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Bước 1: Khởi động dịch vụ Foundry Local và tải mô hình tùy chỉnh
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Bước 2: Kiểm tra bộ nhớ đệm cho mô hình tùy chỉnh
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Bước 3: Tải mô hình vào bộ nhớ
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Bước 4: Tạo client OpenAI sử dụng điểm cuối được phát hiện bởi SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Bước 5: Gửi yêu cầu hoàn thành trò chuyện theo luồng
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

Chạy nó:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// Bước 1: Bắt đầu dịch vụ Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Bước 2: Lấy mô hình tùy chỉnh từ danh mục
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Bước 3: Tải mô hình vào bộ nhớ
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Bước 4: Tạo một client OpenAI sử dụng điểm cuối được phát hiện bởi SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Bước 5: Gửi yêu cầu hoàn thành trò chuyện dạng luồng dữ liệu
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

Chạy nó:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **Điểm then chốt:** Foundry Local SDK tự động phát hiện điểm cuối nên bạn không bao giờ phải mã cứng số cổng. Đây là cách làm được khuyến nghị cho các ứng dụng sản xuất. Mô hình tùy chỉnh biên dịch của bạn hoạt động giống hệt các mô hình trong catalogue qua SDK.

---

## Lựa chọn Mô hình để Biên dịch

Qwen3-0.6B được dùng làm ví dụ tham khảo trong bài lab này vì nó nhỏ, biên dịch nhanh, và được cung cấp miễn phí theo giấy phép Apache 2.0. Tuy nhiên, bạn có thể biên dịch nhiều mô hình khác. Dưới đây là một số gợi ý:

| Mô hình | ID trên Hugging Face | Tham số | Giấy phép | Ghi chú |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Rất nhỏ, biên dịch nhanh, thích hợp để thử nghiệm |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Chất lượng tốt hơn, vẫn nhanh khi biên dịch |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Chất lượng mạnh, cần RAM nhiều hơn |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Cần đồng ý giấy phép trên Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Chất lượng cao, tải về lớn và biên dịch lâu hơn |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Đã có trong catalogue Foundry Local (hữu ích để so sánh) |

> **Nhắc nhở giấy phép:** Luôn kiểm tra giấy phép của mô hình trên Hugging Face trước khi sử dụng. Một số mô hình (như Llama) yêu cầu bạn đồng ý thỏa thuận giấy phép và đăng nhập với `huggingface-cli login` trước khi tải về.

---

## Khái niệm: Khi Nào Nên Dùng Mô hình Tùy chỉnh

| Tình huống | Tại sao nên biên dịch mô hình của riêng bạn? |
|----------|----------------------|
| **Mô hình bạn cần không có trong catalogue** | Catalogue Foundry Local được tuyển chọn. Nếu mô hình bạn muốn không nằm trong đó, hãy tự biên dịch. |
| **Mô hình đã được tinh chỉnh** | Nếu bạn đã tinh chỉnh mô hình trên dữ liệu chuyên ngành, bạn cần biên dịch lại trọng số của riêng bạn. |
| **Yêu cầu cụ thể về định lượng** | Bạn muốn độ chính xác hoặc chiến lược định lượng khác với mặc định của catalogue. |
| **Phiên bản mô hình mới hơn** | Khi có mô hình mới trên Hugging Face, có thể chưa có trong catalogue Foundry Local. Tự biên dịch giúp bạn truy cập ngay lập tức. |
| **Nghiên cứu và thử nghiệm** | Thử các kiến trúc, kích cỡ hay cấu hình mô hình khác nhau tại chỗ trước khi chọn dùng trong sản xuất. |

---

## Tóm tắt

Trong bài lab này bạn đã học cách:

| Bước | Bạn đã làm gì |
|------|-------------|
| 1 | Cài đặt trình xây dựng mô hình ONNX Runtime GenAI |
| 2 | Biên dịch `Qwen/Qwen3-0.6B` từ Hugging Face thành mô hình ONNX được tối ưu |
| 3 | Tạo tệp cấu hình mẫu hội thoại `inference_model.json` |
| 4 | Thêm mô hình đã biên dịch vào bộ nhớ đệm Foundry Local |
| 5 | Chạy trò chuyện tương tác với mô hình tùy chỉnh qua CLI |
| 6 | Truy vấn mô hình qua REST API tương thích OpenAI |
| 7 | Kết nối từ Python, JavaScript, và C# dùng OpenAI SDK |
| 8 | Kiểm tra hoàn chỉnh mô hình tùy chỉnh với Foundry Local SDK |

Điều then chốt là **bất kỳ mô hình dựa trên transformer nào cũng có thể chạy qua Foundry Local** sau khi được biên dịch sang định dạng ONNX. API tương thích OpenAI giúp mã ứng dụng hiện tại của bạn hoạt động không thay đổi; bạn chỉ cần đổi tên mô hình.

---

## Những điểm chính cần ghi nhớ

| Khái niệm | Chi tiết |
|---------|--------|
| Trình xây dựng mô hình ONNX Runtime GenAI | Chuyển mô hình Hugging Face sang định dạng ONNX với định lượng chỉ bằng một lệnh |
| Định dạng ONNX | Foundry Local yêu cầu mô hình ONNX với cấu hình ONNX Runtime GenAI |
| Mẫu hội thoại | Tệp `inference_model.json` chỉ cách Foundry Local định dạng lời nhắc cho một mô hình nhất định |
| Mục tiêu phần cứng | Biên dịch cho CPU, NVIDIA GPU (CUDA), DirectML (GPU Windows), hoặc WebGPU tùy phần cứng |
| Định lượng | Độ chính xác thấp hơn (int4) giảm kích thước và tăng tốc độ với đánh đổi chính xác; fp16 giữ chất lượng cao trên GPU |
| Tương thích API | Mô hình tùy chỉnh dùng cùng API tương thích OpenAI như mô hình tích hợp sẵn |
| Foundry Local SDK | SDK xử lý khởi động dịch vụ, phát hiện điểm cuối, và tải mô hình tự động cho cả mô hình catalogue lẫn tùy chỉnh |

---

## Đọc thêm

| Tài nguyên | Liên kết |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Hướng dẫn mô hình tùy chỉnh Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Gia đình mô hình Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Tài liệu Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Các bước tiếp theo

Tiếp tục đến [Phần 11: Gọi công cụ với Mô hình cục bộ](part11-tool-calling.md) để học cách kích hoạt mô hình cục bộ gọi các hàm bên ngoài.

[← Phần 9: Chuyển đổi giọng nói Whisper](part9-whisper-voice-transcription.md) | [Phần 11: Gọi công cụ →](part11-tool-calling.md)