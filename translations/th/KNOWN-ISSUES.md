# ปัญหาที่ทราบ — Foundry Local Workshop

ปัญหาที่พบขณะสร้างและทดสอบเวิร์กช็อปนี้บนอุปกรณ์ **Snapdragon X Elite (ARM64)** ที่รัน Windows โดยใช้ Foundry Local SDK v0.9.0, CLI v0.8.117 และ .NET SDK 10.0

> **ตรวจสอบล่าสุด:** 2026-03-11

---

## 1. CPU Snapdragon X Elite ไม่ถูกจดจำโดย ONNX Runtime

**สถานะ:** เปิด
**ความรุนแรง:** เตือน (ไม่กีดขวาง)
**องค์ประกอบ:** ONNX Runtime / cpuinfo
**การทำซ้ำ:** ทุกครั้งที่เริ่มบริการ Foundry Local บนฮาร์ดแวร์ Snapdragon X Elite

ทุกครั้งที่บริการ Foundry Local เริ่มทำงาน จะมีคำเตือนสองรายการแจ้งออกมา:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**ผลกระทบ:** คำเตือนเป็นเพียงผิวเผิน — การประมวลผล inference ทำงานได้อย่างถูกต้อง อย่างไรก็ตามคำเตือนเหล่านี้จะปรากฏในทุกครั้งที่รัน ซึ่งอาจทำให้ผู้เข้าร่วมเวิร์กช็อปสับสนได้ ไลบรารี ONNX Runtime cpuinfo จำเป็นต้องได้รับการอัปเดตเพื่อจดจำคอร์ CPU Qualcomm Oryon

**ที่คาดหวัง:** Snapdragon X Elite ควรถูกจดจำว่าเป็น CPU แบบ ARM64 ที่รองรับโดยไม่แสดงข้อความระดับข้อผิดพลาด

---

## 2. SingleAgent NullReferenceException ในครั้งแรกที่รัน

**สถานะ:** เปิด (เป็นบางครั้ง)
**ความรุนแรง:** รุนแรง (แอปพลิเคชันล่ม)
**องค์ประกอบ:** Foundry Local C# SDK + Microsoft Agent Framework
**การทำซ้ำ:** รัน `dotnet run agent` — ล่มทันทีหลังจากโหลดโมเดล

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**บริบท:** บรรทัดที่ 37 เรียก `model.IsCachedAsync(default)` การล่มเกิดขึ้นครั้งแรกที่รันเอเจนต์หลังจากหยุดบริการด้วยคำสั่ง `foundry service stop` การรันครั้งถัดไปกับโค้ดเดียวกันสำเร็จ

**ผลกระทบ:** เป็นบางครั้ง — บ่งชี้ถึงเงื่อนไขการแข่งขัน (race condition) ในการเริ่มต้นบริการหรือการสืบค้นแค็ตตาล็อกของ SDK การเรียก `GetModelAsync()` อาจคืนค่าก่อนที่บริการจะพร้อมทำงานเต็มที่

**ที่คาดหวัง:** `GetModelAsync()` ควรบล็อกจนกว่าบริการจะพร้อม หรือต้องคืนข้อความแจ้งข้อผิดพลาดที่ชัดเจนหากบริการยังไม่เริ่มเสร็จ

---

## 3. C# SDK ต้องระบุ RuntimeIdentifier อย่างชัดเจน

**สถานะ:** เปิด — ติดตามใน [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)
**ความรุนแรง:** ข้อเท็จจริงด้านเอกสารขาดหาย
**องค์ประกอบ:** แพ็กเกจ NuGet `Microsoft.AI.Foundry.Local`
**การทำซ้ำ:** สร้างโปรเจกต์ .NET 8+ โดยไม่มี `<RuntimeIdentifier>` ในไฟล์ `.csproj`

การคอมไพล์ล้มเหลวพร้อมข้อความ:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**สาเหตุหลัก:** การระบุ RID เป็นสิ่งที่คาดหวัง — SDK มี native binaries (เรียกผ่าน P/Invoke ไปยัง `Microsoft.AI.Foundry.Local.Core` และ ONNX Runtime) ดังนั้น .NET จึงต้องรู้ว่าจะโหลดไลบรารีเฉพาะแพลตฟอร์มใด

ส่วนนี้มีการอธิบายไว้ใน MS Learn ([วิธีใช้ native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)) ซึ่งคำสั่งรันจะแสดงว่า:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

อย่างไรก็ตาม ผู้ใช้ต้องจำสลัก `-r` ทุกครั้งซึ่งจำได้ยาก

**วิธีแก้ปัญหา:** เพิ่ม fallback ตรวจจับอัตโนมัติใน `.csproj` ของคุณเพื่อให้ `dotnet run` ทำงานได้โดยไม่ต้องใช้ flag ใด ๆ:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` เป็นพร็อพเพอร์ตี้ MSBuild ที่กำหนดค่า RID ของโฮสต์เครื่องโดยอัตโนมัติ โปรเจกต์ทดสอบของ SDK เองก็ใช้รูปแบบนี้แล้ว flag `-r` ก็ยังสามารถใช้งานได้ตามปกติเมื่อระบุอย่างชัดเจน

> **หมายเหตุ:** `.csproj` ของเวิร์กช็อปรวม fallback นี้ไว้ด้วยเพื่อให้ `dotnet run` ใช้งานได้ในทุกแพลตฟอร์มทันที

**ที่คาดหวัง:** เทมเพลต `.csproj` ในเอกสาร MS Learn ควรรวมรูปแบบตรวจจับอัตโนมัตินี้ไว้ เพื่อผู้ใช้ไม่ต้องจำ flag `-r`

---

## 4. JavaScript Whisper — การถอดเสียงไฟล์เสียงส่งผลลัพธ์ว่างหรือไบนารี่

**สถานะ:** เปิด (ถดถอย — แย่ลงเทียบกับรายงานเริ่มแรก)
**ความรุนแรง:** สำคัญ
**องค์ประกอบ:** การใช้งาน JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**การทำซ้ำ:** รัน `node foundry-local-whisper.mjs` — ไฟล์เสียงทุกไฟล์ส่งผลลัพธ์เป็นค่าว่างหรือข้อมูลไบนารี่แทนข้อความที่ถอดเสียง

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

ในตอนแรก มีเพียงไฟล์เสียงลำดับที่ 5 เท่านั้นที่คืนค่าว่าง; ตั้งแต่เวอร์ชัน v0.9.x ไฟล์เสียงทั้ง 5 ส่งกลับไบต์เดียว (`\ufffd`) แทนข้อความที่ถอดเสียง การใช้งาน Python Whisper โดย OpenAI SDK ถอดเสียงไฟล์เดียวกันได้ถูกต้อง

**ที่คาดหวัง:** `createAudioClient()` ควรคืนข้อความถอดเสียงที่ตรงกับการใช้งานใน Python/C#

---

## 5. C# SDK จัดส่งเฉพาะ net8.0 — ไม่มีเป้าหมาย .NET 9 หรือ .NET 10 อย่างเป็นทางการ

**สถานะ:** เปิด
**ความรุนแรง:** ข้อเท็จจริงด้านเอกสารขาดหาย
**องค์ประกอบ:** แพ็กเกจ NuGet `Microsoft.AI.Foundry.Local` v0.9.0
**คำสั่งติดตั้ง:** `dotnet add package Microsoft.AI.Foundry.Local`

แพ็กเกจ NuGet มีเป้าหมายเฟรมเวิร์กเพียงตัวเดียว:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

ไม่มี TFM `net9.0` หรือ `net10.0` รวมอยู่ ในทางตรงกันข้าม แพ็กเกจคู่ `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) มีเป้าหมาย `net8.0`, `net9.0`, `net10.0`, `net472`, และ `netstandard2.0`

### การทดสอบความเข้ากันได้

| Target Framework | สร้าง | รัน | หมายเหตุ |
|-----------------|-------|-----|---------|
| net8.0          | ✅    | ✅  | รองรับอย่างเป็นทางการ |
| net9.0          | ✅    | ✅  | สร้างด้วย forward-compat — ใช้ในตัวอย่างเวิร์กช็อป |
| net10.0         | ✅    | ✅  | สร้างและรันด้วย forward-compat รวมกับ .NET 10.0.3 runtime |

แอสเซมบลี net8.0 โหลดบน runtime ใหม่ๆ ผ่านกลไก forward-compatibility ของ .NET ดังนั้นจึงสร้างสำเร็จ แต่ไม่ถูกบันทึกไว้และไม่ผ่านการทดสอบจากทีม SDK

### ทำไมตัวอย่างจึงตั้งเป้า net9.0

1. **.NET 9 เป็นรุ่นเสถียรล่าสุด** — ผู้เข้าร่วมเวิร์กช็อปส่วนใหญ่จะติดตั้งไว้แล้ว
2. **forward compatibility ทำงานได้** — แอสเซมบลี net8.0 ในแพ็กเกจ NuGet รันบน runtime .NET 9 ได้อย่างไม่มีปัญหา
3. **.NET 10 (พรีวิว/RC)** ใหม่เกินไปที่จะใช้ในเวิร์กช็อปที่ควรทำงานบนเครื่องใครก็ได้

**ที่คาดหวัง:** รุ่น SDK ในอนาคตควรเพิ่ม TFM `net9.0` และ `net10.0` ร่วมกับ `net8.0` เพื่อให้สอดคล้องกับรูปแบบการใช้งานใน `Microsoft.Agents.AI.OpenAI` และเพื่อให้รองรับ runtime รุ่นใหม่ได้อย่างผ่านการทดสอบ

---

## 6. JavaScript ChatClient Streaming ใช้ callback แทน async iterators

**สถานะ:** เปิด
**ความรุนแรง:** ข้อเท็จจริงด้านเอกสารขาดหาย
**องค์ประกอบ:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` ที่คืนค่าจาก `model.createChatClient()` ให้เมธอด `completeStreamingChat()` ซึ่งใช้ **รูปแบบ callback** แทนที่จะคืน async iterable

```javascript
// ❌ นี่ไม่ทำงาน — โยนข้อผิดพลาด "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ รูปแบบที่ถูกต้อง — ส่ง callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**ผลกระทบ:** นักพัฒนาที่คุ้นเคยกับรูปแบบการวนลูป async ของ OpenAI SDK (`for await`) จะพบข้อผิดพลาดที่สับสน ต้องส่ง callback ที่เป็นฟังก์ชันถูกต้อง มิฉะนั้น SDK จะโยนข้อผิดพลาด "Callback must be a valid function."

**ที่คาดหวัง:** ควรบันทึกรูปแบบ callback ไว้ในเอกสารอ้างอิง SDK หรือสนับสนุนรูปแบบ async iterable เพื่อความสอดคล้องกับ OpenAI SDK

---

## รายละเอียดสภาพแวดล้อม

| องค์ประกอบ | เวอร์ชัน |
|-------------|-----------|
| OS          | Windows 11 ARM64 |
| ฮาร์ดแวร์    | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK    | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js     | 18+ |
| ONNX Runtime| 1.18+ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อจำกัดความรับผิดชอบ**:  
เอกสารนี้ได้รับการแปลโดยใช้บริการแปลด้วย AI [Co-op Translator](https://github.com/Azure/co-op-translator) ในขณะที่เรามุ่งมั่นความถูกต้อง โปรดทราบว่าการแปลอัตโนมัติอาจมีข้อผิดพลาดหรือความคลาดเคลื่อนได้ เอกสารต้นฉบับในภาษาต้นทางควรถือเป็นแหล่งข้อมูลที่เชื่อถือได้ สำหรับข้อมูลที่มีความสำคัญ แนะนำให้ใช้การแปลโดยมนุษย์มืออาชีพ พวกเราไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความที่คลาดเคลื่อนใด ๆ ที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->