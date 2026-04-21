# บันทึกการเปลี่ยนแปลง — Foundry Local Workshop

การเปลี่ยนแปลงที่สำคัญทั้งหมดของเวิร์กช็อปนี้ถูกบันทึกไว้ด้านล่าง

---

## 2026-03-11 — ส่วนที่ 12 & 13, เว็บ UI, การเขียนใหม่ของ Whisper, แก้ไข WinML/QNN, และการตรวจสอบความถูกต้อง

### เพิ่มเติม
- **ส่วนที่ 12: การสร้าง Web UI สำหรับ Zava Creative Writer** — คู่มือห้องปฏิบัติการใหม่ (`labs/part12-zava-ui.md`) พร้อมแบบฝึกหัดที่ครอบคลุมการสตรีม NDJSON, `ReadableStream` ของเบราว์เซอร์, แบนเนอร์สถานะตัวแทนแบบสด, และการสตรีมข้อความบทความแบบเรียลไทม์
- **ส่วนที่ 13: การจบเวิร์กช็อป** — สรุปห้องปฏิบัติการใหม่ (`labs/part13-workshop-complete.md`) พร้อมสรุปทั้ง 12 ส่วน ไอเดียเพิ่มเติม และลิงก์ทรัพยากร
- **ส่วนหน้า Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — อินเทอร์เฟซเบราว์เซอร์แบบ vanilla HTML/CSS/JS ร่วมกันที่ใช้โดย backend ทั้งสามตัว
- **เซิร์ฟเวอร์ HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — เซิร์ฟเวอร์ HTTP สไตล์ Express ใหม่ที่ครอบตัว orchestrator สำหรับการเข้าถึงผ่านเบราว์เซอร์
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` และ `ZavaCreativeWriterWeb.csproj` — โปรเจกต์ API แบบมินิมอลใหม่ที่ให้บริการ UI และการสตรีม NDJSON
- **ตัวสร้างตัวอย่างเสียง:** `samples/audio/generate_samples.py` — สคริปต์ TTS ออฟไลน์โดยใช้ `pyttsx3` เพื่อสร้างไฟล์ WAV ธีม Zava สำหรับส่วนที่ 9
- **ตัวอย่างเสียง:** `samples/audio/zava-full-project-walkthrough.wav` — ตัวอย่างเสียงยาวขึ้นใหม่สำหรับการทดสอบการถอดเสียง
- **สคริปต์ตรวจสอบความถูกต้อง:** `validate-npu-workaround.ps1` — สคริปต์ PowerShell อัตโนมัติเพื่อตรวจสอบวิธีแก้ไข NPU/QNN ในตัวอย่าง C# ทั้งหมด
- **ไฟล์ SVG ไดอะแกรม Mermaid:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **การสนับสนุนข้ามแพลตฟอร์ม WinML:** โฟลเดอร์ `.csproj` ทั้ง 3 ไฟล์ C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ใช้ TFM แบบมีเงื่อนไขและ package references ที่ห้ามใช้ร่วมกันเพื่อรองรับหลายแพลตฟอร์ม บน Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (ชุดซุปเปอร์เซ็ตที่รวมปลั๊กอิน QNN EP) บน Non-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK พื้นฐาน) การระบุ RID `win-arm64` แบบระบุค่าคงที่ในโปรเจกต์ Zava ถูกแทนที่ด้วยการตรวจจับอัตโนมัติ วิธีแก้ปัญหาการพึ่งพาแบบทรานซิทีฟตัด native assets จาก `Microsoft.ML.OnnxRuntime.Gpu.Linux` ที่มีการอ้างอิง `win-arm64` ที่เสียหาย วิธีแก้ไข NPU แบบ try/catch ก่อนหน้านี้ถูกเอาออกจากไฟล์ C# ทั้ง 7 ไฟล์

### เปลี่ยนแปลง
- **ส่วนที่ 9 (Whisper):** เขียนใหม่ครั้งใหญ่ — JavaScript ใช้ `AudioClient` ที่รวมใน SDK (`model.createAudioClient()`) แทนการประมาณค่า ONNX Runtime ด้วยตนเอง; ปรับปรุงคำอธิบายสถาปัตยกรรม ตารางเปรียบเทียบ และแผนภาพ pipeline เพื่อสะท้อนการใช้ `AudioClient` ใน JS/C# เทียบกับ ONNX Runtime ใน Python
- **ส่วนที่ 11:** อัปเดตลิงก์นำทาง (ชี้ไปยังส่วนที่ 12) เพิ่มไฟล์ระดับ SVG ที่เรนเดอร์สำหรับ flow การเรียกใช้เครื่องมือและลำดับเหตุการณ์
- **ส่วนที่ 10:** อัปเดตเส้นทางนำทางให้ผ่านส่วนที่ 12 แทนที่จะสิ้นสุดเวิร์กช็อป
- **Python Whisper (`foundry-local-whisper.py`):** ขยายด้วยตัวอย่างเสียงเพิ่มเติมและปรับปรุงการจัดการข้อผิดพลาด
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** เขียนใหม่เพื่อใช้ `model.createAudioClient()` พร้อมกับ `audioClient.transcribe()` แทนการใช้เซสชัน ONNX Runtime ด้วยตนเอง
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** อัปเดตเพื่อให้บริการไฟล์ UI แบบสแตติกควบคู่ไปกับ API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** ลบวิธีแก้ไข NPU (ตอนนี้จัดการโดยแพ็คเกจ WinML)
- **README.md:** เพิ่มส่วนที่ 12 พร้อมตารางตัวอย่างโค้ดและการเพิ่ม backend; เพิ่มส่วนที่ 13; อัปเดตเป้าหมายการเรียนรู้และโครงสร้างโปรเจกต์
- **KNOWN-ISSUES.md:** ลบข้อขัดข้อง #7 ที่แก้ไขแล้ว (C# SDK NPU Model Variant — ตอนนี้จัดการโดยแพ็คเกจ WinML) เปลี่ยนเลขลำดับปัญหาที่เหลือเป็น #1–#6 อัปเดตรายละเอียดสภาพแวดล้อมด้วย .NET SDK 10.0.104
- **AGENTS.md:** อัปเดตโครงสร้างโปรเจกต์ต้นไม้ด้วยรายการใหม่ `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); อัปเดตแพ็คเกจสำคัญของ C# และรายละเอียด TFM ที่มีเงื่อนไข
- **labs/part2-foundry-local-sdk.md:** อัปเดตตัวอย่าง `.csproj` เพื่อแสดงรูปแบบข้ามแพลตฟอร์มเต็มรูปแบบพร้อม TFM แบบมีเงื่อนไข, package references ห้ามใช้ร่วมกัน, และหมายเหตุอธิบาย

### ตรวจสอบความถูกต้อง
- โปรเจกต์ C# ทั้ง 3 (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) คอมไพล์สำเร็จบน Windows ARM64
- ตัวอย่างแชต (`dotnet run chat`): โหลดโมเดลเป็น `phi-3.5-mini-instruct-qnn-npu:1` ผ่าน WinML/QNN — ตัวแปร NPU โหลดโดยตรงไม่มี fallback CPU
- ตัวอย่างเอเย่นต์ (`dotnet run agent`): ทำงานครบกระบวนการสนทนาแบบหลายเทิร์น โค้ดออก 0
- Foundry Local CLI v0.8.117 และ SDK v0.9.0 บน .NET SDK 9.0.312

---

## 2026-03-11 — แก้ไขโค้ด, ทำความสะอาดโมเดล, ไดอะแกรม Mermaid และการตรวจสอบความถูกต้อง

### แก้ไข
- **โค้ดตัวอย่างทั้ง 21 ตัวอย่าง (7 Python, 7 JavaScript, 7 C#):** เพิ่ม `model.unload()` / `unload_model()` / `model.UnloadAsync()` สำหรับทำความสะอาดเมื่อออกโปรแกรมเพื่อแก้ปัญหาคำเตือนหน่วยความจำ OGA รั่ว (ปัญหาที่รู้จัก #4)
- **csharp/WhisperTranscription.cs:** แทนที่เส้นทางสัมพัทธ์ที่อ่อนแอ `AppContext.BaseDirectory` ด้วย `FindSamplesDirectory()` ที่ค้นหาขึ้นบันทึกโฟลเดอร์ `samples/audio` อย่างเชื่อถือได้ (ปัญหาที่รู้จัก #7)
- **csharp/csharp.csproj:** แทนที่ `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` ที่ระบุไว้ตายตัวด้วย fallback ตรวจจับอัตโนมัติโดยใช้ `$(NETCoreSdkRuntimeIdentifier)` เพื่อให้ `dotnet run` ทำงานได้ทุกแพลตฟอร์มโดยไม่ต้องมีธง `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### เปลี่ยนแปลง
- **ส่วนที่ 8:** แปลงลูปการวนซ้ำที่ขับเคลื่อนด้วยการประเมินผลจากไดอะแกรมกล่อง ASCII เป็นภาพ SVG ที่เรนเดอร์แล้ว
- **ส่วนที่ 10:** แปลงแผนภาพ pipeline การคอมไพล์จากลูกศร ASCII เป็นภาพ SVG ที่เรนเดอร์แล้ว
- **ส่วนที่ 11:** แปลงแผนภาพ flow การเรียกใช้เครื่องมือและลำดับเป็นภาพ SVG ที่เรนเดอร์แล้ว
- **ส่วนที่ 10:** ย้ายส่วน "เวิร์กช็อปจบแล้ว!" ไปยังส่วนที่ 11 (ห้องปฏิบัติการสุดท้าย) แทนที่ด้วยลิงก์ "ขั้นตอนถัดไป"
- **KNOWN-ISSUES.md:** ตรวจสอบใหม่ทั้งหมดสำหรับ CLI v0.8.117 ลบรไข้แล้ว: การรั่วไหลหน่วยความจำ OGA (เพิ่มการทำความสะอาด), เส้นทาง Whisper (FindSamplesDirectory), HTTP 500 sustained inference (ไม่สามารถทำซ้ำได้, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), ข้อจำกัด tool_choice (ตอนนี้ใช้งานได้กับ `"required"` และการกำหนดฟังก์ชันเฉพาะสำหรับ qwen2.5-0.5b) อัปเดตปัญหา JS Whisper — ตอนนี้ไฟล์ทั้งหมดคืนค่าว่าง/ไบนารี (ถดถอยจาก v0.9.x, เพิ่มระดับความรุนแรงเป็น Major) อัปเดต #4 C# RID ด้วย workaround ตรวจจับอัตโนมัติและลิงก์ [#497](https://github.com/microsoft/Foundry-Local/issues/497) เหลือปัญหาเปิด 7 รายการ
- **javascript/foundry-local-whisper.mjs:** แก้ไขชื่อแปรทำความสะอาด (`whisperModel` → `model`)

### ตรวจสอบความถูกต้อง
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — ทำงานได้สำเร็จพร้อมการทำความสะอาด
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — ทำงานได้สำเร็จพร้อมการทำความสะอาด
- C#: `dotnet build` สำเร็จไม่มีคำเตือนหรือข้อผิดพลาด (เป้าหมาย net9.0)
- ไฟล์ Python ทั้ง 7 ไฟล์ผ่านการตรวจสอบไวยากรณ์ `py_compile`
- ไฟล์ JavaScript ทั้ง 7 ไฟล์ผ่านการตรวจสอบไวยากรณ์ `node --check`

---

## 2026-03-10 — ส่วนที่ 11: การเรียกใช้เครื่องมือ, การขยาย SDK API, และการครอบคลุมโมเดล

### เพิ่มเติม
- **ส่วนที่ 11: การเรียกใช้เครื่องมือกับโมเดลในเครื่อง** — คู่มือห้องปฏิบัติการใหม่ (`labs/part11-tool-calling.md`) พร้อม 8 แบบฝึกหัดที่ครอบคลุมโครงร่างเครื่องมือ, flow หลายเทิร์น, การเรียกใช้เครื่องมือหลายตัว, เครื่องมือเฉพาะ, การเรียกใช้เครื่องมือ ChatClient, และ `tool_choice`
- **ตัวอย่าง Python:** `python/foundry-local-tool-calling.py` — การเรียกใช้เครื่องมือด้วยเครื่องมือ `get_weather`/`get_population` โดยใช้ OpenAI SDK
- **ตัวอย่าง JavaScript:** `javascript/foundry-local-tool-calling.mjs` — การเรียกใช้เครื่องมือโดยใช้ `ChatClient` เนทีฟของ SDK (`model.createChatClient()`)
- **ตัวอย่าง C#:** `csharp/ToolCalling.cs` — การเรียกใช้เครื่องมือโดยใช้ `ChatTool.CreateFunctionTool()` กับ OpenAI C# SDK
- **ส่วนที่ 2, แบบฝึกหัด 7:** `ChatClient` เนทีฟ — `model.createChatClient()` (JS) และ `model.GetChatClientAsync()` (C#) เป็นทางเลือกแทน OpenAI SDK
- **ส่วนที่ 2, แบบฝึกหัด 8:** ตัวแปรโมเดลและการเลือกฮาร์ดแวร์ — `selectVariant()`, `variants`, ตารางตัวแปร NPU (7 โมเดล)
- **ส่วนที่ 2, แบบฝึกหัด 9:** การอัปเกรดโมเดลและการรีเฟรชแค็ตตาล็อก — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **ส่วนที่ 2, แบบฝึกหัด 10:** โมเดลเหตุผล — `phi-4-mini-reasoning` พร้อมตัวอย่างการแยกวิเคราะห์แท็ก `<think>`
- **ส่วนที่ 3, แบบฝึกหัด 4:** `createChatClient` เป็นทางเลือกแทน OpenAI SDK พร้อมเอกสารรูปแบบ callback แบบสตรีมมิ่ง
- **AGENTS.md:** เพิ่มคอนเวนชันการเขียนโค้ด Tool Calling, ChatClient, และ Reasoning Models

### เปลี่ยนแปลง
- **ส่วนที่ 1:** ขยายแค็ตตาล็อกโมเดล — เพิ่ม phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **ส่วนที่ 2:** ขยายตารางอ้างอิง API — เพิ่ม `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **ส่วนที่ 2:** เปลี่ยนหมายเลขแบบฝึกหัดจาก 7-9 เป็น 10-13 เพื่อรองรับแบบฝึกหัดใหม่
- **ส่วนที่ 3:** อัปเดตตารางข้อสรุปสำคัญเพื่อรวม ChatClient เนทีฟ
- **README.md:** เพิ่มส่วนที่ 11 พร้อมตารางตัวอย่างโค้ด; เพิ่มเป้าหมายการเรียนรู้ #11; อัปเดตโครงสร้างโปรเจกต์
- **csharp/Program.cs:** เพิ่มกรณี `toolcall` ในตัวจัดการ CLI และอัปเดตข้อความช่วยเหลือ

---

## 2026-03-09 — การอัปเดต SDK v0.9.0, อังกฤษแบบอังกฤษ, และการตรวจสอบความถูกต้อง

### เปลี่ยนแปลง
- **โค้ดตัวอย่างทั้งหมด (Python, JavaScript, C#):** อัปเดตเป็น Foundry Local SDK v0.9.0 API — แก้ไข `await catalog.getModel()` (เพิ่มเติม `await` ที่ขาดหาย), อัปเดตรูปแบบการเริ่มต้น `FoundryLocalManager`, แก้ไขการค้นหา endpoint
- **คู่มือห้องปฏิบัติการทั้งหมด (ส่วนที่ 1-10):** แปลงเป็นอังกฤษแบบอังกฤษ (colour, catalogue, optimised ฯลฯ)
- **คู่มือห้องปฏิบัติการทั้งหมด:** อัปเดตตัวอย่างโค้ด SDK ให้ตรงกับพื้นผิว API v0.9.0
- **คู่มือห้องปฏิบัติการทั้งหมด:** อัปเดตตารางอ้างอิง API และบล็อกโค้ดแบบฝึกหัด
- **แก้ไขสำคัญ JavaScript:** เพิ่ม `await` ที่ขาดใน `catalog.getModel()` — คืนค่า `Promise` ไม่ใช่วัตถุ `Model` ทำให้เกิดความล้มเหลวเงียบในภายหลัง

### ตรวจสอบความถูกต้อง
- ตัวอย่าง Python ทุกตัวทำงานได้สำเร็จกับบริการ Foundry Local
- ตัวอย่าง JavaScript ทุกตัวทำงานได้สำเร็จ (Node.js 18+)
- โปรเจกต์ C# คอมไพล์และรันบน .NET 9.0 (รองรับจาก net8.0 SDK assembly)
- แก้ไขและตรวจสอบไฟล์ทั้งหมด 29 ไฟล์ทั่วทั้งเวิร์กช็อป

---

## ดัชนีไฟล์

| ไฟล์ | ปรับปรุงล่าสุด | คำอธิบาย |
|------|---------------|----------|
| `labs/part1-getting-started.md` | 2026-03-10 | ขยายแค็ตตาล็อกโมเดล |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | แบบฝึกหัดใหม่ 7-10, ขยายตาราง API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | แบบฝึกหัดใหม่ 4 (ChatClient), อัปเดตข้อสรุป |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + อังกฤษแบบอังกฤษ |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + อังกฤษแบบอังกฤษ |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + ภาษาอังกฤษแบบบริติช |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + ภาษาอังกฤษแบบบริติช |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | ไดอะแกรม Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + ภาษาอังกฤษแบบบริติช |
| `labs/part10-custom-models.md` | 2026-03-11 | ไดอะแกรม Mermaid, ย้าย Workshop Complete ไปยัง Part 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | ห้องทดลองใหม่, ไดอะแกรม Mermaid, ส่วน Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | ใหม่: ตัวอย่างการเรียกเครื่องมือ |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | ใหม่: ตัวอย่างการเรียกเครื่องมือ |
| `csharp/ToolCalling.cs` | 2026-03-10 | ใหม่: ตัวอย่างการเรียกเครื่องมือ |
| `csharp/Program.cs` | 2026-03-10 | เพิ่มคำสั่ง CLI `toolcall` |
| `README.md` | 2026-03-10 | Part 11, โครงสร้างโปรเจกต์ |
| `AGENTS.md` | 2026-03-10 | การเรียกเครื่องมือ + ข้อตกลง ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | ลบปัญหาที่แก้ไขแล้ว Issue #7, มีปัญหาเปิดค้างอยู่ 6 รายการ |
| `csharp/csharp.csproj` | 2026-03-11 | TFM ข้ามแพลตฟอร์ม, การอ้างอิง SDK พื้นฐาน/WinML แบบมีเงื่อนไข |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM ข้ามแพลตฟอร์ม, ตรวจจับ RID อัตโนมัติ |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM ข้ามแพลตฟอร์ม, ตรวจจับ RID อัตโนมัติ |
| `csharp/BasicChat.cs` | 2026-03-11 | ลบวิธีแก้ไข try/catch ของ NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | ลบวิธีแก้ไข try/catch ของ NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | ลบวิธีแก้ไข try/catch ของ NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | ลบวิธีแก้ไข try/catch ของ NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | ลบวิธีแก้ไข try/catch ของ NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | ตัวอย่าง .csproj ข้ามแพลตฟอร์ม |
| `AGENTS.md` | 2026-03-11 | อัปเดตรายละเอียดแพ็กเกจ C# และ TFM |
| `CHANGELOG.md` | 2026-03-11 | ไฟล์นี้ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อจำกัดความรับผิดชอบ**:  
เอกสารนี้ได้รับการแปลโดยใช้บริการแปลภาษาด้วย AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้ว่าเราจะพยายามให้มีความถูกต้อง โปรดทราบว่าการแปลแบบอัตโนมัติอาจมีข้อผิดพลาดหรือความไม่ถูกต้อง เอกสารต้นฉบับในภาษาต้นทางควรถูกพิจารณาเป็นแหล่งข้อมูลที่เชื่อถือได้ สำหรับข้อมูลที่สำคัญ ขอแนะนำให้ใช้บริการแปลโดยมนุษย์มืออาชีพ เราจะไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความผิดที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->