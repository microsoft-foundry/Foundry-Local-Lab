# Các Vấn Đề Đã Biết — Foundry Local Workshop

Các vấn đề gặp phải khi xây dựng và thử nghiệm workshop này trên thiết bị **Snapdragon X Elite (ARM64)** chạy Windows, với Foundry Local SDK v0.9.0, CLI v0.8.117, và .NET SDK 10.0.

> **Xác nhận lần cuối:** 2026-03-11

---

## 1. CPU Snapdragon X Elite Không Được ONNX Runtime Nhận Diện

**Trạng thái:** Mở  
**Mức độ nghiêm trọng:** Cảnh báo (không chặn)  
**Thành phần:** ONNX Runtime / cpuinfo  
**Phương pháp tái tạo:** Mỗi lần khởi động dịch vụ Foundry Local trên phần cứng Snapdragon X Elite

Mỗi lần dịch vụ Foundry Local khởi động, hai cảnh báo được phát ra:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Tác động:** Các cảnh báo chỉ mang tính thẩm mỹ — phép suy luận vẫn hoạt động bình thường. Tuy nhiên, chúng xuất hiện mỗi lần chạy và có thể gây nhầm lẫn cho người tham gia workshop. Thư viện cpuinfo của ONNX Runtime cần được cập nhật để nhận diện đúng các lõi CPU Qualcomm Oryon.

**Mong đợi:** Snapdragon X Elite nên được nhận diện là CPU ARM64 được hỗ trợ mà không phát ra các thông điệp lỗi mức độ cao.

---

## 2. NullReferenceException trên SingleAgent khi Chạy Lần Đầu

**Trạng thái:** Mở (thỉnh thoảng xảy ra)  
**Mức độ nghiêm trọng:** Nghiêm trọng (crash)  
**Thành phần:** Foundry Local C# SDK + Microsoft Agent Framework  
**Phương pháp tái tạo:** Chạy `dotnet run agent` — crash ngay sau khi tải mô hình

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Ngữ cảnh:** Dòng 37 gọi `model.IsCachedAsync(default)`. Lỗi xảy ra ở lần chạy đầu tiên của agent sau khi `foundry service stop` mới. Các lần chạy tiếp theo với cùng mã nguồn thì thành công.

**Tác động:** Xảy ra không đều — cho thấy có thể có race condition trong quá trình khởi tạo dịch vụ hoặc truy vấn catalog của SDK. Lời gọi `GetModelAsync()` có thể trả về trước khi dịch vụ hoàn toàn sẵn sàng.

**Mong đợi:** `GetModelAsync()` nên hoặc là chặn cho đến khi dịch vụ sẵn sàng hoặc trả về thông báo lỗi rõ ràng nếu dịch vụ chưa hoàn thành việc khởi tạo.

---

## 3. C# SDK Yêu Cầu Chỉ Định RuntimeIdentifier Rõ Ràng

**Trạng thái:** Mở — được theo dõi tại [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Mức độ nghiêm trọng:** Thiếu sót tài liệu  
**Thành phần:** Gói NuGet `Microsoft.AI.Foundry.Local`  
**Phương pháp tái tạo:** Tạo dự án .NET 8+ mà không có `<RuntimeIdentifier>` trong `.csproj`

Xây dựng bị lỗi với:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Nguyên nhân gốc:** Yêu cầu RID là điều bắt buộc — SDK đi kèm các tệp nhị phân gốc (P/Invoke vào `Microsoft.AI.Foundry.Local.Core` và ONNX Runtime), nên .NET cần biết thư viện đặc thù nền tảng để phân giải.

Điều này đã được ghi trong MS Learn ([Cách sử dụng native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), nơi hướng dẫn chạy có đoạn:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Tuy nhiên, người dùng phải nhớ flag `-r` mỗi lần, điều này rất dễ quên.

**Giải pháp tạm thời:** Thêm fallback tự động phát hiện vào `.csproj` để `dotnet run` hoạt động mà không cần bất kỳ flag nào:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` là thuộc tính MSBuild tích hợp sẵn tự động phân giải RID của máy chủ. Các dự án thử của SDK cũng dùng pattern này. Các flag `-r` tùy chọn vẫn được tôn trọng khi cung cấp.

> **Lưu ý:** `.csproj` của workshop bao gồm fallback này để `dotnet run` hoạt động ngay lập tức trên mọi nền tảng.

**Mong đợi:** Mẫu `.csproj` trên tài liệu MS Learn nên bao gồm pattern tự động phát hiện này để người dùng không phải nhớ flag `-r`.

---

## 4. JavaScript Whisper — Phiên Âm Âm Thanh Trả Về Kết Quả Trống hoặc Dữ Liệu Nhị Phân

**Trạng thái:** Mở (regression — xấu hơn so với báo cáo đầu)  
**Mức độ nghiêm trọng:** Quan trọng  
**Thành phần:** Triển khai Whisper trong JavaScript (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Phương pháp tái tạo:** Chạy `node foundry-local-whisper.mjs` — tất cả các file âm thanh trả về output trống hoặc dữ liệu nhị phân thay vì văn bản phiên âm

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Trước đó chỉ file âm thanh thứ 5 trả về trống; đến phiên bản v0.9.x, cả 5 file đều trả về một byte duy nhất (`\ufffd`) thay vì văn bản phiên âm. Phiên bản Whisper Python sử dụng OpenAI SDK có thể phiên âm chính xác các file này.

**Mong đợi:** `createAudioClient()` nên trả về văn bản phiên âm tương đương với các triển khai Python/C#.

---

## 5. C# SDK Chỉ Bao Gồm net8.0 — Không Có Mục Tiêu .NET 9 hoặc .NET 10 Chính Thức

**Trạng thái:** Mở  
**Mức độ nghiêm trọng:** Thiếu sót tài liệu  
**Thành phần:** Gói NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Lệnh cài đặt:** `dotnet add package Microsoft.AI.Foundry.Local`

Gói NuGet chỉ bao gồm một framework mục tiêu duy nhất:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Không bao gồm TFM `net9.0` hoặc `net10.0`. Trong khi đó, gói bạn đồng hành `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) bao gồm `net8.0`, `net9.0`, `net10.0`, `net472`, và `netstandard2.0`.

### Kiểm Thử Tương Thích

| Framework mục tiêu | Biên dịch | Chạy | Ghi chú |
|--------------------|-----------|-------|---------|
| net8.0             | ✅        | ✅    | Hỗ trợ chính thức |
| net9.0             | ✅        | ✅    | Biên dịch qua forward-compat — dùng trong mẫu workshop |
| net10.0            | ✅        | ✅    | Biên dịch và chạy qua forward-compat với runtime .NET 10.0.3 |

Thư viện net8.0 được load trên các runtime mới hơn qua cơ chế forward-compat của .NET, nên build thành công. Tuy nhiên điều này chưa được tài liệu hóa hoặc kiểm tra bởi nhóm SDK.

### Tại Sao Mẫu Dùng net9.0

1. **.NET 9 là phiên bản ổn định mới nhất** — phần lớn người tham gia workshop sẽ có cài đặt
2. **Forward compatibility hoạt động** — thư viện net8.0 của NuGet chạy trên runtime .NET 9 bình thường
3. **.NET 10 (preview/RC)** còn quá mới để nhắm tới trong workshop cần hoạt động ổn định với mọi người

**Mong đợi:** Các bản phát hành SDK tương lai nên cân nhắc thêm các TFM `net9.0` và `net10.0` bên cạnh `net8.0` để phù hợp với mẫu của `Microsoft.Agents.AI.OpenAI` và cung cấp hỗ trợ được xác thực cho các runtime mới.

---

## 6. JavaScript ChatClient Streaming Dùng Callback, Không Phải Async Iterator

**Trạng thái:** Mở  
**Mức độ nghiêm trọng:** Thiếu sót tài liệu  
**Thành phần:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` trả về bởi `model.createChatClient()` cung cấp phương thức `completeStreamingChat()`, nhưng sử dụng **mẫu callback** thay vì trả về async iterable:

```javascript
// ❌ Điều này KHÔNG hoạt động — phát sinh lỗi "luồng không thể lặp bất đồng bộ"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Mẫu đúng — truyền một hàm gọi lại
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Tác động:** Các nhà phát triển quen với mẫu lặp bất đồng bộ (`for await`) của OpenAI SDK sẽ gặp lỗi khó hiểu. Callback phải là hàm hợp lệ, nếu không SDK sẽ báo "Callback must be a valid function."

**Mong đợi:** Tài liệu tham khảo SDK nên ghi chép rõ mẫu callback. Hoặc hỗ trợ mẫu async iterable để đồng nhất với OpenAI SDK.

---

## Chi Tiết Môi Trường

| Thành phần | Phiên bản |
|------------|-----------|
| OS | Windows 11 ARM64 |
| Phần cứng | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |