<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# เวิร์กช็อป Foundry Local - สร้างแอป AI บนอุปกรณ์

เวิร์กช็อปแบบลงมือปฏิบัติสำหรับการรันโมเดลภาษาในเครื่องของคุณเองและสร้างแอปอัจฉริยะด้วย [Foundry Local](https://foundrylocal.ai) และ [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)

> **Foundry Local คืออะไร?** Foundry Local คือรันไทม์ขนาดเล็กที่ช่วยให้คุณดาวน์โหลด จัดการ และให้บริการโมเดลภาษาได้อย่างเต็มที่บนฮาร์ดแวร์ของคุณเอง มันเปิดเผย **API ที่เข้ากันได้กับ OpenAI** เพื่อให้เครื่องมือหรือ SDK ใด ๆ ที่รองรับ OpenAI สามารถเชื่อมต่อได้โดยไม่ต้องใช้บัญชีคลาวด์

### 🌐 รองรับหลายภาษา

#### รองรับผ่าน GitHub Action (อัตโนมัติและอัปเดตตลอดเวลา)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[อาหรับ](../ar/README.md) | [เบงกาลี](../bn/README.md) | [บัลแกเรีย](../bg/README.md) | [พม่า (เมียนมาร์)](../my/README.md) | [จีน (ตัวย่อ)](../zh-CN/README.md) | [จีน (ตัวเต็ม, ฮ่องกง)](../zh-HK/README.md) | [จีน (ตัวเต็ม, มาเก๊า)](../zh-MO/README.md) | [จีน (ตัวเต็ม, ไต้หวัน)](../zh-TW/README.md) | [โครเอเชีย](../hr/README.md) | [เช็ก](../cs/README.md) | [เดนมาร์ก](../da/README.md) | [ดัตช์](../nl/README.md) | [เอสโตเนีย](../et/README.md) | [ฟินแลนด์](../fi/README.md) | [ฝรั่งเศส](../fr/README.md) | [เยอรมัน](../de/README.md) | [กรีก](../el/README.md) | [ฮีบรู](../he/README.md) | [ฮินดี](../hi/README.md) | [ฮังการี](../hu/README.md) | [อินโดนีเซีย](../id/README.md) | [อิตาลี](../it/README.md) | [ญี่ปุ่น](../ja/README.md) | [กันนาดา](../kn/README.md) | [เขมร](../km/README.md) | [เกาหลี](../ko/README.md) | [ลิทัวเนีย](../lt/README.md) | [มาเลย์](../ms/README.md) | [มาลายาลัม](../ml/README.md) | [มราฐี](../mr/README.md) | [เนปาล](../ne/README.md) | [ไนจีเรีย พิดจิน](../pcm/README.md) | [นอร์เวย์](../no/README.md) | [เปอร์เซีย (ฟาร์ซี)](../fa/README.md) | [โปแลนด์](../pl/README.md) | [โปรตุเกส (บราซิล)](../pt-BR/README.md) | [โปรตุเกส (โปรตุเกส)](../pt-PT/README.md) | [ปัญจาบี (กูรมุขี)](../pa/README.md) | [โรมาเนีย](../ro/README.md) | [รัสเซีย](../ru/README.md) | [เซอร์เบีย (ซีริลลิก)](../sr/README.md) | [สโลวัก](../sk/README.md) | [สโลวีเนีย](../sl/README.md) | [สเปน](../es/README.md) | [สวาฮีลี](../sw/README.md) | [สวีเดน](../sv/README.md) | [ตากาล็อก (ฟิลิปปินส์)](../tl/README.md) | [ทมิฬ](../ta/README.md) | [เทลูกู](../te/README.md) | [ไทย](./README.md) | [ตุรกี](../tr/README.md) | [ยูเครน](../uk/README.md) | [อูรดู](../ur/README.md) | [เวียดนาม](../vi/README.md)

> **ต้องการโคลนแบบโลคัล?**
>
> รีโพสิทอรีนี้รวมการแปลภาษา 50+ ภาษา ซึ่งเพิ่มขนาดดาวน์โหลดอย่างมีนัยสำคัญ หากต้องการโคลนโดยไม่รวมการแปล ให้ใช้ sparse checkout:
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
> วิธีนี้จะให้ทุกอย่างที่คุณต้องการเพื่อทำคอร์สนี้ให้เสร็จด้วยการดาวน์โหลดที่รวดเร็วยิ่งขึ้น
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## วัตถุประสงค์การเรียนรู้

เมื่อสิ้นสุดเวิร์กช็อปนี้ คุณจะสามารถ:

| # | วัตถุประสงค์ |
|---|--------------|
| 1 | ติดตั้ง Foundry Local และจัดการโมเดลด้วย CLI |
| 2 | เชี่ยวชาญกับ Foundry Local SDK API สำหรับการจัดการโมเดลแบบโปรแกรม |
| 3 | เชื่อมต่อกับเซิร์ฟเวอร์อนุมานโลคัลโดยใช้ SDK ของ Python, JavaScript และ C# |
| 4 | สร้างท่อผลิต Retrieval-Augmented Generation (RAG) ที่อิงคำตอบกับข้อมูลของคุณเอง |
| 5 | สร้างเอเย่นต์ AI ที่มีคำแนะนำและบุคลิกคงที่ |
| 6 | ประสานงานเวิร์กโฟลว์หลายเอเย่นต์ด้วยวงจรฟีดแบ็ก |
| 7 | สำรวจแอปยอดเยี่ยมในเชิงผลิต - Zava Creative Writer |
| 8 | สร้างเฟรมเวิร์กการประเมินด้วยชุดข้อมูลทองคำและการให้คะแนนแบบ LLM-as-judge |
| 9 | ถอดรหัสเสียงด้วย Whisper - พูดเป็นข้อความบนอุปกรณ์โดยใช้ Foundry Local SDK |
| 10 | คอมไพล์และรันโมเดลกำหนดเองหรือ Hugging Face ด้วย ONNX Runtime GenAI และ Foundry Local |
| 11 | เปิดใช้งานโมเดลโลคัลให้เรียกใช้งานฟังก์ชันภายนอกด้วยรูปแบบ tool-calling |
| 12 | สร้าง UI บนเบราว์เซอร์สำหรับ Zava Creative Writer พร้อมสตรีมมิ่งเรียลไทม์ |

---

## ข้อกำหนดเบื้องต้น

| ข้อกำหนด | รายละเอียด |
|----------|-------------|
| **ฮาร์ดแวร์** | แรมอย่างน้อย 8 GB (แนะนำ 16 GB); ซีพียูรองรับ AVX2 หรือตัวเร่งกราฟิก GPU ที่รองรับ |
| **ระบบปฏิบัติการ** | Windows 10/11 (x64/ARM), Windows Server 2025 หรือ macOS 13+ |
| **Foundry Local CLI** | ติดตั้งผ่าน `winget install Microsoft.FoundryLocal` (Windows) หรือ `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) ดูคำแนะนำ [เริ่มต้นใช้งาน](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) สำหรับรายละเอียด |
| **รันไทม์ภาษา** | **Python 3.9+** และ/หรือ **.NET 9.0+** และ/หรือ **Node.js 18+** |
| **Git** | สำหรับโคลนรีโพสิทอรีนี้ |

---

## เริ่มต้นใช้งาน

```bash
# 1. โคลนที่เก็บข้อมูล
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ตรวจสอบว่า Foundry Local ถูกติดตั้งแล้ว
foundry model list              # แสดงรายชื่อโมเดลที่มีอยู่
foundry model run phi-3.5-mini  # เริ่มการแชทแบบอินเทอร์แอคทีฟ

# 3. เลือกรายวิชาภาษาของคุณ (ดู Part 2 lab สำหรับการตั้งค่าเต็มรูปแบบ)
```

| ภาษา | เริ่มต้นอย่างรวดเร็ว |
|-------|---------------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ส่วนของเวิร์กช็อป

### ส่วนที่ 1: เริ่มต้นกับ Foundry Local

**คู่มือการปฏิบัติ:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local คืออะไร และทำงานอย่างไร
- การติดตั้ง CLI บน Windows และ macOS
- การสำรวจโมเดล - รายการ ดาวน์โหลด และรัน
- ความเข้าใจเกี่ยวกับนามแฝงของโมเดลและพอร์ตแบบไดนามิก

---

### ส่วนที่ 2: เจาะลึก Foundry Local SDK

**คู่มือการปฏิบัติ:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- เหตุผลที่ใช้ SDK แทน CLI สำหรับการพัฒนาแอปพลิเคชัน
- อ้างอิง API SDK เต็มรูปแบบสำหรับ Python, JavaScript และ C#
- การจัดการบริการ, การเรียกดูแคตตาล็อก, วงจรชีวิตโมเดล (ดาวน์โหลด, โหลด, ปล่อย)
- รูปแบบเริ่มต้นอย่างรวดเร็ว: ตัวสร้าง Python bootstrap, JavaScript `init()`, C# `CreateAsync()`
- ข้อมูลเมตา `FoundryModelInfo`, นามแฝง และการเลือกโมเดลที่เหมาะสมกับฮาร์ดแวร์

---

### ส่วนที่ 3: SDKs และ APIs

**คู่มือการปฏิบัติ:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- การเชื่อมต่อกับ Foundry Local จาก Python, JavaScript และ C#
- การใช้ Foundry Local SDK เพื่อจัดการบริการแบบโปรแกรม
- การสตรีมจบแชทผ่าน OpenAI-compatible API
- อ้างอิงวิธี SDK สำหรับแต่ละภาษา

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|-------|-------|-----------|
| Python | `python/foundry-local.py` | แชทสตรีมมิ่งพื้นฐาน |
| C# | `csharp/BasicChat.cs` | แชทสตรีมมิ่งกับ .NET |
| JavaScript | `javascript/foundry-local.mjs` | แชทสตรีมมิ่งกับ Node.js |

---

### ส่วนที่ 4: Retrieval-Augmented Generation (RAG)

**คู่มือการปฏิบัติ:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG คืออะไรและเหตุใดจึงสำคัญ
- การสร้างฐานความรู้ในหน่วยความจำ
- การดึงข้อมูลด้วยการทับซ้อนคำสำคัญพร้อมการให้คะแนน
- การประพันธ์พรอมต์ระบบที่ตั้งอยู่บนข้อมูลจริง
- การรันท่อ RAG แบบครบถ้วนบนอุปกรณ์

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ |
|-------|-------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ส่วนที่ 5: การสร้าง AI Agents

**คู่มือการปฏิบัติ:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI Agent คืออะไร (ต่างจากเรียกใช้ LLM โดยตรง)
- รูปแบบ `ChatAgent` และ Microsoft Agent Framework
- คำสั่งระบบ, บุคลิก และการสนทนาหลายรอบ
- ผลลัพธ์แบบมีโครงสร้าง (JSON) จากเอเย่นต์

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|-------|-------|-----------|
| Python | `python/foundry-local-with-agf.py` | เอเย่นต์เดี่ยวพร้อม Agent Framework |
| C# | `csharp/SingleAgent.cs` | เอเย่นต์เดี่ยว (รูปแบบ ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | เอเย่นต์เดี่ยว (รูปแบบ ChatAgent) |

---

### ส่วนที่ 6: เวิร์กโฟลว์หลายเอเย่นต์

**คู่มือการปฏิบัติ:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- ท่อเวิร์กโฟลว์หลายเอเย่นต์: นักวิจัย → นักเขียน → บรรณาธิการ
- การประสานลำดับและวงจรฟีดแบ็ก
- การกำหนดค่าแบบแชร์และการส่งต่อแบบมีโครงสร้าง
- การออกแบบเวิร์กโฟลว์หลายเอเย่นต์ของคุณเอง

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|-------|-------|-----------|
| Python | `python/foundry-local-multi-agent.py` | ท่อสามเอเย่นต์ |
| C# | `csharp/MultiAgent.cs` | ท่อสามเอเย่นต์ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | ท่อสามเอเย่นต์ |

---

### ส่วนที่ 7: Zava Creative Writer - แอปการศึกษาระดับสูง

**คู่มือการปฏิบัติ:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- แอปแบบการผลิตหลายเอเย่นต์ที่มีเอเย่นต์เฉพาะ 4 ตัว
- ท่อแบบลำดับพร้อมวงจรฟีดแบ็กโดยผู้ประเมิน
- การสตรีมเอาต์พุต, การค้นหาแคตตาล็อกสินค้า, การส่งต่อ JSON แบบมีโครงสร้าง
- การใช้งานเต็มรูปแบบใน Python (FastAPI), JavaScript (Node.js CLI), และ C# (.NET console)

**ตัวอย่างโค้ด:**

| ภาษา | โฟลเดอร์ | คำอธิบาย |
|-------|----------|-----------|
| Python | `zava-creative-writer-local/src/api/` | บริการเว็บ FastAPI พร้อมผู้จัดการกระบวนการ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | แอป CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | แอป console .NET 9 |

---

### ส่วนที่ 8: การพัฒนานำโดยการประเมินผล

**คู่มือการปฏิบัติ:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- สร้างเฟรมเวิร์กการประเมินแบบเป็นระบบสำหรับเอเย่นต์ AI ด้วยชุดข้อมูลทองคำ
- การตรวจสอบตามกฎ (ความยาว, การครอบคลุมคำสำคัญ, คำต้องห้าม) + การให้คะแนน LLM-as-judge
- การเปรียบเทียบแบบข้างเคียงของตัวแปรพรอมต์ด้วยบัตรคะแนนรวม
- ขยายรูปแบบเอเย่นต์ Zava Editor จากส่วนที่ 7 สู่ชุดทดสอบแบบออฟไลน์
- ใช้ได้ทั้ง Python, JavaScript และ C#

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|-------|-------|-----------|
| Python | `python/foundry-local-eval.py` | เฟรมเวิร์กประเมินผล |
| C# | `csharp/AgentEvaluation.cs` | เฟรมเวิร์กประเมินผล |
| JavaScript | `javascript/foundry-local-eval.mjs` | เฟรมเวิร์กประเมินผล |

---

### ส่วนที่ 9: การถอดเสียงเสียงด้วย Whisper

**คู่มือการปฏิบัติ:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- การถอดเสียงพูดเป็นข้อความโดยใช้ OpenAI Whisper ที่ทำงานแบบออฟไลน์
- การประมวลผลเสียงที่เน้นความเป็นส่วนตัว - เสียงจะไม่ถูกส่งออกจากอุปกรณ์ของคุณ
- ใช้ Python, JavaScript และ C# กับ `client.audio.transcriptions.create()` (Python/JS) และ `AudioClient.TranscribeAudioAsync()` (C#)
- รวมไฟล์เสียงตัวอย่างธีม Zava สำหรับฝึกปฏิบัติ

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | การถอดเสียงเสียง Whisper |
| C# | `csharp/WhisperTranscription.cs` | การถอดเสียงเสียง Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | การถอดเสียงเสียง Whisper |

> **หมายเหตุ:** ห้องปฏิบัติการนี้ใช้ **Foundry Local SDK** เพื่อดาวน์โหลดและโหลดโมเดล Whisper แบบอัตโนมัติ จากนั้นส่งเสียงไปยังจุดเชื่อมต่อท้องถิ่นที่รองรับ OpenAI สำหรับการถอดเสียง โมเดล Whisper (`whisper`) แสดงในแคตตาล็อก Foundry Local และทำงานทั้งหมดบนอุปกรณ์ - ไม่จำเป็นต้องใช้คีย์ API คลาวด์หรือการเข้าถึงเครือข่าย

---

### ส่วนที่ 10: การใช้โมเดลแบบกำหนดเองหรือ Hugging Face

**คู่มือห้องปฏิบัติการ:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- การแปลงโมเดล Hugging Face ไปเป็นรูปแบบ ONNX ที่เพิ่มประสิทธิภาพโดยใช้ ONNX Runtime GenAI model builder
- การคอมไพล์เฉพาะฮาร์ดแวร์ (CPU, NVIDIA GPU, DirectML, WebGPU) และการปรับค่าความแม่นยำ (int4, fp16, bf16)
- การสร้างไฟล์กำหนดค่ารูปแบบแชทสำหรับ Foundry Local
- เพิ่มโมเดลที่คอมไพล์แล้วลงในแคชของ Foundry Local
- เรียกใช้โมเดลกำหนดเองผ่าน CLI, REST API และ OpenAI SDK
- ตัวอย่างอ้างอิง: การคอมไพล์ Qwen/Qwen3-0.6B แบบครบวงจร

---

### ส่วนที่ 11: การเรียกใช้งานเครื่องมือด้วยโมเดลท้องถิ่น

**คู่มือห้องปฏิบัติการ:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- เปิดให้โมเดลท้องถิ่นเรียกฟังก์ชันภายนอกได้ (tool/function calling)
- กำหนดสคีมาของเครื่องมือโดยใช้รูปแบบการเรียกฟังก์ชันของ OpenAI
- จัดการการสนทนาแบบหลายรอบของการเรียกใช้งานเครื่องมือ
- ดำเนินการเรียกเครื่องมือภายในเครื่องและส่งผลลัพธ์กลับไปยังโมเดล
- เลือกโมเดลที่เหมาะสมสำหรับสถานการณ์การเรียกเครื่องมือ (Qwen 2.5, Phi-4-mini)
- ใช้ `ChatClient` ของ SDK สำหรับการเรียกใช้งานเครื่องมือ (JavaScript)

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | การเรียกใช้เครื่องมือกับเครื่องมือพยากรณ์อากาศ/ประชากร |
| C# | `csharp/ToolCalling.cs` | การเรียกใช้เครื่องมือด้วย .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | การเรียกใช้เครื่องมือด้วย ChatClient |

---

### ส่วนที่ 12: การสร้าง Web UI สำหรับ Zava Creative Writer

**คู่มือห้องปฏิบัติการ:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- เพิ่มส่วนหน้าเว็บเบราว์เซอร์ให้กับ Zava Creative Writer
- ให้บริการ UI ร่วมกันจาก Python (FastAPI), JavaScript (Node.js HTTP), และ C# (ASP.NET Core)
- ประมวลผลการสตรีม NDJSON ในเบราว์เซอร์ด้วย Fetch API และ ReadableStream
- แสดงสถานะตัวแทนแบบสดและสตรีมข้อความบทความแบบเรียลไทม์

**โค้ด (UI ร่วม):**

| ไฟล์ | คำอธิบาย |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | รูปแบบหน้า |
| `zava-creative-writer-local/ui/style.css` | การจัดรูปแบบ |
| `zava-creative-writer-local/ui/app.js` | ตัวอ่านสตรีมและตรรกะการอัพเดต DOM |

**ส่วนเสริมของ Backend:**

| ภาษา | ไฟล์ | คำอธิบาย |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | ปรับปรุงเพื่อให้บริการ UI แบบสแตติก |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | เซิร์ฟเวอร์ HTTP ใหม่ห่อหุ้ม orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | โปรเจ็กต์ ASP.NET Core minimal API ใหม่ |

---

### ส่วนที่ 13: การทำเวิร์กช็อปเสร็จสมบูรณ์

**คู่มือห้องปฏิบัติการ:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- สรุปทุกสิ่งที่คุณได้สร้างในทั้ง 12 ส่วน
- แนวคิดเพิ่มเติมในการขยายแอปพลิเคชันของคุณ
- ลิงก์ไปยังแหล่งข้อมูลและเอกสารประกอบ

---

## โครงสร้างโปรเจ็กต์

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

## แหล่งข้อมูล

| แหล่งข้อมูล | ลิงก์ |
|----------|------|
| เว็บไซต์ Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| แคตตาล็อกโมเดล | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| คู่มือเริ่มต้น | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| เอกสารอ้างอิง Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## สิทธิ์ใช้งาน

วัสดุเวิร์กช็อปนี้จัดทำขึ้นเพื่อวัตถุประสงค์ทางการศึกษา

---

**ขอให้สนุกกับการสร้างสรรค์! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อสงวนสิทธิ์**:  
เอกสารนี้ได้รับการแปลโดยใช้บริการแปลภาษาด้วย AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้ว่าเราจะพยายามให้ความถูกต้อง แต่โปรดทราบว่าการแปลอัตโนมัติอาจมีข้อผิดพลาดหรือความไม่ถูกต้อง เอกสารต้นฉบับในภาษาดั้งเดิมควรถือเป็นแหล่งข้อมูลที่เชื่อถือได้ สำหรับข้อมูลที่สำคัญ ขอแนะนำให้ใช้การแปลโดยผู้เชี่ยวชาญมนุษย์ เราไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความผิดใด ๆ ที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->