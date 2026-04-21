# Các Tệp Âm Thanh Mẫu cho Phần 7 - Chuyển Văn Bản Giọng Nói Whisper

Các tệp WAV này được tạo ra bằng cách sử dụng `pyttsx3` (Windows SAPI5 chuyển văn bản thành giọng nói) và có chủ đề xoay quanh **sản phẩm DIY của Zava** từ bản demo Creative Writer.

## Tạo các mẫu

```bash
# Từ thư mục gốc của kho - yêu cầu có .venv với pyttsx3 đã được cài đặt
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Tệp mẫu

| Tệp | Tình huống | Thời lượng |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Khách hàng hỏi về **Máy khoan không dây Zava ProGrip** - moment xoắn, thời lượng pin, hộp đựng | ~15 giây |
| `zava-product-review.wav` | Khách hàng đánh giá **Sơn nội thất Zava UltraSmooth** - độ phủ, thời gian khô, VOC thấp | ~22 giây |
| `zava-support-call.wav` | Cuộc gọi hỗ trợ về **Tủ dụng cụ Zava TitanLock** - chìa khóa thay thế, tấm lót ngăn kéo thêm | ~20 giây |
| `zava-project-planning.wav` | Người làm DIY lên kế hoạch xây sân sau với **Sàn composite Zava EcoBoard** & đèn BrightBeam | ~25 giây |
| `zava-workshop-setup.wav` | Hướng dẫn thiết lập một xưởng làm việc hoàn chỉnh sử dụng **tất cả năm sản phẩm Zava** | ~32 giây |
| `zava-full-project-walkthrough.wav` | Hướng dẫn sửa sang gara nâng cao sử dụng **tất cả sản phẩm Zava** (cho kiểm tra âm thanh dài, xem [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 phút |

## Ghi chú

- Các tệp WAV được **đưa vào repo** (liệt kê trong `. Để tạo các tệp .wav mới, chạy script phía trên để tạo lại các script mới hoặc chỉnh sửa để tạo script mới.
- Script sử dụng giọng **Microsoft David** (tiếng Anh Mỹ) với 160 từ/phút để có kết quả chuyển văn bản rõ ràng.
- Tất cả các tình huống tham chiếu sản phẩm từ [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).