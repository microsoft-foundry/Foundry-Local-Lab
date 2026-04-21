![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Phần 8: Phát Triển Dựa Trên Đánh Giá với Foundry Local

> **Mục tiêu:** Xây dựng một khuôn khổ đánh giá kiểm tra và chấm điểm hệ thống các đại lý AI của bạn, sử dụng cùng một mô hình cục bộ vừa làm đại lý dưới thử nghiệm vừa làm giám khảo, để bạn có thể lặp lại các câu lệnh một cách tự tin trước khi triển khai.

## Tại sao phát triển dựa trên đánh giá?

Khi xây dựng đại lý AI, "trông có vẻ đúng" không đủ tốt. **Phát triển dựa trên đánh giá** xem đầu ra của đại lý như mã nguồn: bạn viết các bài kiểm tra trước, đo lường chất lượng và chỉ triển khai khi điểm số đạt ngưỡng.

Trong Zava Creative Writer (Phần 7), đại lý **Editor** đã đóng vai trò như một người đánh giá nhẹ nhàng; nó quyết định CHẤP NHẬN hay SỬA ĐỔI. Phòng lab này chính thức hoá mô hình đó thành một khuôn khổ đánh giá có thể lặp lại mà bạn có thể áp dụng cho bất kỳ đại lý hay pipeline nào.

| Vấn đề | Giải pháp |
|---------|----------|
| Thay đổi prompt âm thầm làm giảm chất lượng | **Bộ dữ liệu vàng** phát hiện các suy giảm |
| Thành kiến "chỉ đúng với một ví dụ" | **Nhiều trường hợp thử nghiệm** phơi bày các trường hợp biên |
| Đánh giá chất lượng mang tính chủ quan | **Chấm điểm theo quy tắc + LLM làm giám khảo** cung cấp con số |
| Không có cách so sánh các biến thể prompt | **Chạy đánh giá song song** với điểm tổng hợp |

---

## Khái niệm chính

### 1. Bộ dữ liệu vàng

**Bộ dữ liệu vàng** là tập hợp các trường hợp thử nghiệm được tuyển chọn với đầu ra kỳ vọng đã biết. Mỗi trường hợp thử nghiệm gồm:

- **Đầu vào**: Prompt hoặc câu hỏi gửi tới đại lý
- **Đầu ra kỳ vọng**: Phản hồi đúng hoặc chất lượng cao phải chứa những gì (từ khóa, cấu trúc, sự kiện)
- **Danh mục**: Nhóm phục vụ báo cáo (ví dụ “độ chính xác về mặt dữ kiện”, “tông giọng”, “độ đầy đủ”)

### 2. Kiểm tra theo quy tắc

Kiểm tra nhanh, xác định được kết quả, không cần LLM:

| Kiểm tra | Kiểm tra điều gì |
|----------|------------------|
| **Giới hạn độ dài** | Phản hồi không quá ngắn (lười biếng) hoặc quá dài (lạc đề) |
| **Từ khóa bắt buộc** | Phản hồi nhắc đến các thuật ngữ hoặc thực thể kỳ vọng |
| **Xác thực định dạng** | JSON đọc được, tiêu đề Markdown có mặt |
| **Nội dung cấm** | Không có tên thương hiệu ảo, không đề cập đối thủ cạnh tranh |

### 3. LLM làm giám khảo

Sử dụng **cùng một mô hình cục bộ** để chấm điểm chính đầu ra của nó (hoặc đầu ra của biến thể prompt khác). Giám khảo nhận được:

- Câu hỏi gốc
- Phản hồi của đại lý
- Tiêu chí chấm điểm

Và trả về điểm có cấu trúc. Điều này phản chiếu mô hình Editor từ Phần 7 nhưng được áp dụng có hệ thống trên toàn bộ bộ thử nghiệm.

### 4. Vòng lặp lặp lại dựa trên đánh giá

![Vòng lặp lặp lại dựa trên đánh giá](../../../images/eval-loop-flowchart.svg)

---

## Yêu cầu chuẩn bị

| Yêu cầu | Chi tiết |
|---------|----------|
| **Foundry Local CLI** | Đã cài đặt và tải mô hình |
| **Runtime ngôn ngữ** | **Python 3.9+** và/hoặc **Node.js 18+** và/hoặc **.NET 9+ SDK** |
| **Hoàn thành** | [Phần 5: Đại lý Đơn](part5-single-agents.md) và [Phần 6: Luồng làm việc đa đại lý](part6-multi-agent-workflows.md) |

---

## Bài tập

### Bài tập 1 - Chạy Khuôn Khổ Đánh Giá

Workshop bao gồm mẫu đánh giá hoàn chỉnh kiểm tra một đại lý Foundry Local với bộ dữ liệu vàng các câu hỏi liên quan đến Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Thiết lập:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Chạy:**
```bash
python foundry-local-eval.py
```

**Diễn biến:**
1. Kết nối tới Foundry Local và tải mô hình
2. Định nghĩa bộ dữ liệu vàng gồm 5 trường hợp thử nghiệm (câu hỏi sản phẩm Zava)
3. Chạy hai biến thể prompt với mọi trường hợp thử nghiệm
4. Chấm điểm mỗi phản hồi bằng **kiểm tra theo quy tắc** (độ dài, từ khóa, định dạng)
5. Chấm điểm mỗi phản hồi bằng **LLM làm giám khảo** (cùng mô hình chấm chất lượng 1-5)
6. In bảng điểm so sánh hai biến thể prompt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Thiết lập:**
```bash
cd javascript
npm install
```

**Chạy:**
```bash
node foundry-local-eval.mjs
```

**Cùng pipeline đánh giá:** bộ dữ liệu vàng, chạy prompt kép, chấm điểm quy tắc + LLM, bảng điểm.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Thiết lập:**
```bash
cd csharp
dotnet restore
```

**Chạy:**
```bash
dotnet run eval
```

**Cùng pipeline đánh giá:** bộ dữ liệu vàng, chạy prompt kép, chấm điểm quy tắc + LLM, bảng điểm.

</details>

---

### Bài tập 2 - Hiểu bộ dữ liệu vàng

Xem xét các trường hợp thử nghiệm được định nghĩa trong mẫu đánh giá. Mỗi trường hợp thử nghiệm có:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Câu hỏi cần suy nghĩ:**
1. Tại sao giá trị kỳ vọng là **từ khóa** thay vì câu đầy đủ?
2. Cần bao nhiêu trường hợp thử nghiệm để có đánh giá đáng tin cậy?
3. Bạn sẽ thêm những danh mục nào cho ứng dụng của mình?

---

### Bài tập 3 - Hiểu chấm điểm Kiểm tra theo Quy tắc và LLM làm giám khảo

Khuôn khổ đánh giá sử dụng **hai lớp chấm điểm**:

#### Kiểm tra theo Quỹ tắc (nhanh, xác định)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM làm giám khảo (phân tích chi tiết, chất lượng)

Cùng một mô hình cục bộ đóng vai trò giám khảo với bảng tiêu chí có cấu trúc:

```
Rate this response on a scale of 1-5:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved
- 4: Good response with minor issues
- 5: Excellent, comprehensive, well-structured

Score: 4
Reasoning: The response correctly identifies all necessary tools
and provides practical advice, but could include safety equipment.
```

**Câu hỏi cần suy nghĩ:**
1. Khi nào bạn sẽ tin vào kiểm tra theo quy tắc hơn LLM làm giám khảo?
2. Mô hình có thể đáng tin để tự chấm điểm đầu ra của nó không? Hạn chế là gì?
3. Điều này so với mô hình Editor ở Phần 7 thế nào?

---

### Bài tập 4 - So sánh các biến thể Prompt

Mẫu chạy **hai biến thể prompt** với cùng bộ thử nghiệm:

| Biến thể | Phong cách prompt hệ thống |
|----------|-----------------------------|
| **Cơ bản** | Chung chung: "Bạn là trợ lý hữu ích" |
| **Chuyên biệt** | Chi tiết: "Bạn là chuyên gia Zava DIY tư vấn sản phẩm cụ thể và hướng dẫn từng bước" |

Sau khi chạy, bạn sẽ thấy bảng điểm như:

```
╔══════════════════════════════════════════════════════════════╗
║                    EVALUATION SCORECARD                      ║
╠══════════════════════════════════════════════════════════════╣
║ Prompt Variant    │ Rule Score │ LLM Score │ Combined       ║
╠═══════════════════╪════════════╪═══════════╪════════════════╣
║ Baseline          │ 0.62       │ 3.2 / 5   │ 0.62           ║
║ Specialised       │ 0.81       │ 4.1 / 5   │ 0.81           ║
╚══════════════════════════════════════════════════════════════╝
```

**Bài tập:**
1. Chạy đánh giá và ghi lại điểm của từng biến thể
2. Chỉnh biến thể chuyên biệt kỹ hơn. Điểm có cải thiện không?
3. Thêm biến thể prompt thứ ba và so sánh cả ba.
4. Thử thay đổi bí danh mô hình (ví dụ `phi-4-mini` vs `phi-3.5-mini`) và so sánh kết quả.

---

### Bài tập 5 - Áp dụng đánh giá cho đại lý của bạn

Dùng khuôn khổ đánh giá này làm mẫu cho đại lý riêng của bạn:

1. **Định nghĩa bộ dữ liệu vàng**: viết 5 đến 10 trường hợp thử nghiệm với từ khóa kỳ vọng.
2. **Viết prompt hệ thống**: hướng dẫn đại lý bạn muốn thử.
3. **Chạy đánh giá**: lấy điểm cơ sở.
4. **Lặp lại**: chỉnh sửa prompt, chạy lại và so sánh.
5. **Đặt ngưỡng**: ví dụ “không triển khai nếu điểm kết hợp dưới 0.75”.

---

### Bài tập 6 - Kết nối với Mô hình Editor của Zava

Xem lại đại lý Editor của Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Bộ chỉnh sửa là một mô hình ngôn ngữ lớn với vai trò giám khảo trong môi trường sản xuất:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Đây là **ý tưởng giống hệt** với LLM làm giám khảo trong Phần 8, nhưng nhúng vào pipeline sản xuất thay vì bộ kiểm thử offline. Cả hai mô hình đều:

- Trả về đầu ra định dạng JSON có cấu trúc từ mô hình.
- Áp dụng tiêu chí chất lượng được định nghĩa trong prompt hệ thống.
- Đưa ra quyết định đậu/rớt.

**Khác biệt:** Editor chạy trong môi trường sản xuất (trên mọi yêu cầu). Khuôn khổ đánh giá chạy trong giai đoạn phát triển (trước khi triển khai).

---

## Những điểm cần ghi nhớ

| Khái niệm | Ý nghĩa |
|-----------|---------|
| **Bộ dữ liệu vàng** | Tuyển chọn các trường hợp thử nghiệm sớm; chúng là bài kiểm tra hồi quy của AI |
| **Kiểm tra theo quy tắc** | Nhanh, xác định, phát hiện lỗi rõ ràng (độ dài, từ khóa, định dạng) |
| **LLM làm giám khảo** | Chấm điểm chất lượng tinh tế bằng cùng mô hình cục bộ |
| **So sánh prompt** | Chạy nhiều biến thể trên cùng bộ kiểm thử để chọn prompt tốt nhất |
| **Lợi thế trên thiết bị** | Toàn bộ đánh giá chạy cục bộ: không phí API, không giới hạn tần suất, dữ liệu không rời máy bạn |
| **Đánh giá trước khi xuất bản** | Đặt ngưỡng chất lượng và kiểm soát phát hành dựa trên điểm đánh giá |

---

## Bước tiếp theo

- **Mở rộng**: Thêm nhiều trường hợp và danh mục cho bộ dữ liệu vàng.
- **Tự động hóa**: Tích hợp đánh giá vào pipeline CI/CD.
- **Giám khảo nâng cao**: Dùng mô hình lớn hơn làm giám khảo khi kiểm thử đầu ra mô hình nhỏ hơn.
- **Theo dõi theo thời gian**: Lưu kết quả đánh giá để so sánh giữa các phiên bản prompt và mô hình.

---

## Phòng lab tiếp theo

Tiếp tục sang [Phần 9: Nhận dạng giọng nói với Whisper](part9-whisper-voice-transcription.md) để khám phá chuyển giọng nói thành văn bản cục bộ dùng Foundry Local SDK.