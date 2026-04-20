![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Phần 11: Gọi Công Cụ với Các Mô Hình Cục Bộ

> **Mục tiêu:** Cho phép mô hình cục bộ của bạn gọi các hàm bên ngoài (công cụ) để có thể truy xuất dữ liệu thời gian thực, thực hiện tính toán hoặc tương tác với API — tất cả đều chạy riêng tư trên thiết bị của bạn.

## Gọi Công Cụ Là Gì?

Gọi công cụ (còn được gọi là **gọi hàm**) cho phép mô hình ngôn ngữ yêu cầu thực thi các hàm bạn định nghĩa. Thay vì đoán câu trả lời, mô hình nhận ra khi nào một công cụ sẽ hữu ích và trả về một yêu cầu có cấu trúc để mã của bạn thực thi. Ứng dụng của bạn chạy hàm, gửi kết quả trở lại, và mô hình tích hợp thông tin đó vào câu trả lời cuối cùng của nó.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Mẫu này rất cần thiết để xây dựng các đại lý có thể:

- **Tra cứu dữ liệu trực tiếp** (thời tiết, giá cổ phiếu, truy vấn cơ sở dữ liệu)
- **Thực hiện tính toán chính xác** (toán học, chuyển đổi đơn vị)
- **Thực hiện hành động** (gửi email, tạo vé, cập nhật hồ sơ)
- **Truy cập hệ thống riêng tư** (API nội bộ, hệ thống tệp)

---

## Cách Gọi Công Cụ Hoạt Động

Luồng gọi công cụ có bốn giai đoạn:

| Giai đoạn | Điều xảy ra |
|-----------|-------------|
| **1. Định nghĩa công cụ** | Bạn mô tả các hàm khả dụng bằng JSON Schema — tên, mô tả và tham số |
| **2. Mô hình quyết định** | Mô hình nhận tin nhắn của bạn cùng với định nghĩa công cụ. Nếu một công cụ hữu ích, nó trả về phản hồi `tool_calls` thay vì câu trả lời văn bản |
| **3. Thực thi cục bộ** | Mã của bạn phân tích cuộc gọi công cụ, chạy hàm và thu thập kết quả |
| **4. Câu trả lời cuối cùng** | Bạn gửi kết quả công cụ trở lại cho mô hình, mô hình tạo câu trả lời cuối cùng |

> **Điểm chính:** Mô hình không bao giờ thực thi mã. Nó chỉ *yêu cầu* gọi một công cụ. Ứng dụng của bạn quyết định có thực hiện yêu cầu đó hay không — điều này giúp bạn kiểm soát hoàn toàn.

---

## Mô Hình Nào Hỗ Trợ Gọi Công Cụ?

Không phải tất cả các mô hình đều hỗ trợ gọi công cụ. Trong danh mục Foundry Local hiện tại, các mô hình sau có khả năng gọi công cụ:

| Mô hình | Kích thước | Gọi Công Cụ |
|---------|------------|:----------:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **Mẹo:** Cho phòng thí nghiệm này, chúng ta sử dụng **qwen2.5-0.5b** — nó nhỏ (822 MB tải về), nhanh và hỗ trợ gọi công cụ đáng tin cậy.

---

## Mục Tiêu Học Tập

Kết thúc phòng thí nghiệm này, bạn sẽ có thể:

- Giải thích mẫu gọi công cụ và tại sao nó quan trọng với các đại lý AI
- Định nghĩa schema công cụ bằng định dạng gọi hàm của OpenAI
- Xử lý luồng hội thoại gọi công cụ đa lượt
- Thực thi các cuộc gọi công cụ cục bộ và trả kết quả về mô hình
- Lựa chọn mô hình phù hợp cho các kịch bản gọi công cụ

---

## Yêu Cầu Tiên Quyết

| Yêu cầu | Chi tiết |
|---------|----------|
| **Foundry Local CLI** | Đã cài đặt và có trong `PATH` của bạn ([Phần 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Đã cài đặt SDK Python, JavaScript hoặc C# ([Phần 2](part2-foundry-local-sdk.md)) |
| **Mô hình gọi công cụ** | qwen2.5-0.5b (sẽ được tải tự động) |

---

## Bài Tập

### Bài Tập 1 — Hiểu Luồng Gọi Công Cụ

Trước khi viết mã, hãy nghiên cứu sơ đồ tuần tự này:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Những quan sát chính:**

1. Bạn định nghĩa công cụ trước dưới dạng các đối tượng JSON Schema
2. Phản hồi của mô hình chứa `tool_calls` thay vì nội dung thông thường
3. Mỗi cuộc gọi công cụ có một `id` duy nhất bạn phải tham chiếu khi trả về kết quả
4. Mô hình thấy tất cả các tin nhắn trước *cộng* với kết quả công cụ khi tạo câu trả lời cuối cùng
5. Có thể có nhiều cuộc gọi công cụ trong một phản hồi

> **Thảo luận:** Tại sao mô hình trả về cuộc gọi công cụ thay vì thực thi hàm trực tiếp? Những lợi ích an ninh nào mà điều này mang lại?

---

### Bài Tập 2 — Định Nghĩa Schema Công Cụ

Công cụ được định nghĩa theo định dạng gọi hàm chuẩn của OpenAI. Mỗi công cụ cần:

- **`type`**: Luôn là `"function"`
- **`function.name`**: Tên hàm mô tả (ví dụ `get_weather`)
- **`function.description`**: Mô tả rõ ràng — mô hình dùng cái này để quyết định khi nào gọi công cụ
- **`function.parameters`**: Đối tượng JSON Schema mô tả các tham số mong đợi

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```


> **Thực hành tốt nhất cho mô tả công cụ:**
> - Cụ thể: "Lấy thời tiết hiện tại cho một thành phố" tốt hơn "Lấy thời tiết"
> - Mô tả tham số rõ ràng: mô hình đọc mô tả này để điền giá trị chính xác
> - Đánh dấu tham số bắt buộc và tùy chọn — giúp mô hình biết phải hỏi gì

---

### Bài Tập 3 — Chạy Ví Dụ Gọi Công Cụ

Mỗi ví dụ ngôn ngữ định nghĩa hai công cụ (`get_weather` và `get_population`), gửi một câu hỏi kích hoạt sử dụng công cụ, chạy công cụ cục bộ và gửi kết quả về cho câu trả lời cuối cùng.

<details>
<summary><strong>🐍 Python</strong></summary>

**Yêu cầu tiên quyết:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Chạy:**
```bash
python foundry-local-tool-calling.py
```

**Kết quả mong đợi:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Giải thích mã** (`python/foundry-local-tool-calling.py`):

```python
# Định nghĩa tools như một danh sách các sơ đồ hàm
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# Gửi cùng với tools — mô hình có thể trả về tool_calls thay vì nội dung
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Kiểm tra xem mô hình có muốn gọi tool không
if response.choices[0].message.tool_calls:
    # Thực thi công cụ và gửi kết quả trở lại
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Yêu cầu tiên quyết:**
```bash
cd javascript
npm install
```

**Chạy:**
```bash
node foundry-local-tool-calling.mjs
```

**Kết quả mong đợi:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Giải thích mã** (`javascript/foundry-local-tool-calling.mjs`):

Ví dụ này sử dụng `ChatClient` của Foundry Local SDK thay vì SDK của OpenAI, thể hiện phương pháp tiện lợi `createChatClient()`:

```javascript
// Lấy một ChatClient trực tiếp từ đối tượng mô hình
const chatClient = model.createChatClient();

// Gửi với công cụ — ChatClient xử lý định dạng tương thích với OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Kiểm tra các cuộc gọi công cụ
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Thực thi công cụ và gửi lại kết quả
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Yêu cầu tiên quyết:**
```bash
cd csharp
dotnet restore
```

**Chạy:**
```bash
dotnet run toolcall
```

**Kết quả mong đợi:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Giải thích mã** (`csharp/ToolCalling.cs`):

C# sử dụng trợ giúp `ChatTool.CreateFunctionTool` để định nghĩa công cụ:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### Bài Tập 4 — Luồng Hội Thoại Gọi Công Cụ

Hiểu cấu trúc thông điệp là rất quan trọng. Đây là luồng hoàn chỉnh, hiển thị mảng `messages` ở mỗi giai đoạn:

**Giai đoạn 1 — Yêu cầu ban đầu:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Giai đoạn 2 — Mô hình trả lời với `tool_calls` (không phải nội dung):**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**Giai đoạn 3 — Bạn thêm tin nhắn trợ lý VÀ kết quả công cụ:**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**Giai đoạn 4 — Mô hình tạo câu trả lời cuối cùng sử dụng kết quả công cụ.**

> **Quan trọng:** `tool_call_id` trong tin nhắn công cụ phải khớp với `id` từ cuộc gọi công cụ. Đây cách mô hình liên kết kết quả với yêu cầu.

---

### Bài Tập 5 — Nhiều Cuộc Gọi Công Cụ

Mô hình có thể yêu cầu nhiều cuộc gọi công cụ trong một phản hồi duy nhất. Hãy thử thay đổi tin nhắn người dùng để kích hoạt nhiều cuộc gọi:

```python
# Trong Python — thay đổi thông điệp người dùng:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// Trong JavaScript — thay đổi thông điệp người dùng:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Mô hình sẽ trả về hai `tool_calls` — một cho `get_weather` và một cho `get_population`. Mã của bạn đã xử lý điều này vì nó lặp qua tất cả các cuộc gọi công cụ.

> **Thử nhé:** Sửa tin nhắn người dùng và chạy lại mẫu. Mô hình có gọi cả hai công cụ không?

---

### Bài Tập 6 — Thêm Công Cụ Của Bạn

Mở rộng một trong các ví dụ với công cụ mới. Ví dụ, thêm công cụ `get_time`:

1. Định nghĩa schema công cụ:
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. Thêm logic thực thi:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Trong một ứng dụng thực tế, sử dụng thư viện múi giờ
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... các công cụ hiện có ...
```

3. Thêm công cụ vào mảng `tools` và thử với: "Mấy giờ ở Tokyo?"

> **Thử thách:** Thêm công cụ thực hiện phép tính, ví dụ `convert_temperature` chuyển đổi giữa độ C và F. Thử với: "Chuyển 100°F sang độ C."

---

### Bài Tập 7 — Gọi Công Cụ với ChatClient của SDK (JavaScript)

Ví dụ JavaScript đã sử dụng `ChatClient` gốc của SDK thay vì SDK của OpenAI. Đây là tính năng tiện lợi loại bỏ việc phải tạo client OpenAI thủ công:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient được tạo trực tiếp từ đối tượng mô hình
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat chấp nhận các công cụ như tham số thứ hai
const response = await chatClient.completeChat(messages, tools);
```

So sánh với cách tiếp cận Python dùng SDK OpenAI một cách rõ ràng:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Cả hai cách đều hợp lệ. `ChatClient` tiện lợi hơn; SDK OpenAI cung cấp đầy đủ tham số của OpenAI.

> **Thử làm:** Sửa ví dụ JavaScript dùng SDK OpenAI thay vì `ChatClient`. Bạn cần `import OpenAI from "openai"` và tạo client với endpoint từ `manager.urls[0]`.

---

### Bài Tập 8 — Hiểu Tham Số tool_choice

Tham số `tool_choice` kiểm soát liệu mô hình *bắt buộc* phải sử dụng công cụ hay có thể tự do chọn:

| Giá trị | Hành vi |
|---------|---------|
| `"auto"` | Mô hình quyết định có gọi công cụ hay không (mặc định) |
| `"none"` | Mô hình không gọi công cụ nào dù có cung cấp |
| `"required"` | Mô hình phải gọi ít nhất một công cụ |
| `{"type": "function", "function": {"name": "get_weather"}}` | Mô hình phải gọi công cụ chỉ định |

Thử từng tùy chọn trong ví dụ Python:

```python
# Ép mô hình gọi get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Lưu ý:** Không phải tất cả tùy chọn `tool_choice` đều được mọi mô hình hỗ trợ. Nếu mô hình không hỗ trợ `"required"`, nó có thể bỏ qua và hành xử như `"auto"`.

---

## Các Sai Lầm Thường Gặp

| Vấn đề | Giải pháp |
|---------|-----------|
| Mô hình không bao giờ gọi công cụ | Hãy chắc bạn đang dùng mô hình hỗ trợ gọi công cụ (ví dụ qwen2.5-0.5b). Kiểm tra bảng trên. |
| `tool_call_id` không khớp | Luôn dùng `id` từ phản hồi cuộc gọi công cụ, không dùng giá trị cứng |
| Mô hình trả về JSON trong `arguments` bị lỗi | Mô hình nhỏ hơn thỉnh thoảng tạo JSON không hợp lệ. Đặt `JSON.parse()` trong try/catch |
| Mô hình gọi công cụ không tồn tại | Thêm bộ xử lý mặc định trong hàm `execute_tool` của bạn |
| Vòng lặp gọi công cụ vô hạn | Đặt giới hạn số lượt tối đa (ví dụ 5) để tránh vòng lặp vô tận |

---

## Những Điều Cần Nhớ

1. **Gọi công cụ** cho phép mô hình yêu cầu thực thi hàm thay vì đoán câu trả lời
2. Mô hình **không bao giờ thực thi mã**; ứng dụng của bạn quyết định chạy gì
3. Công cụ được định nghĩa dưới dạng **JSON Schema** theo định dạng gọi hàm của OpenAI
4. Hội thoại theo **mẫu đa lượt**: người dùng, trợ lý (tool_calls), công cụ (kết quả), trợ lý (câu trả lời cuối)
5. Luôn sử dụng **mô hình hỗ trợ gọi công cụ** (Qwen 2.5, Phi-4-mini)
6. SDK cung cấp `createChatClient()` tiện lợi để tạo yêu cầu gọi công cụ mà không cần tạo client OpenAI

---

Tiếp tục đến [Phần 12: Xây dựng UI Web cho Zava Creative Writer](part12-zava-ui.md) để thêm giao diện trình duyệt cho pipeline đa đại lý với phát trực tiếp thời gian thực.

---

[← Phần 10: Mô Hình Tùy Chỉnh](part10-custom-models.md) | [Phần 12: Giao Diện Zava Writer →](part12-zava-ui.md)