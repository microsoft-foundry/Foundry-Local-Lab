<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# เวิร์กช็อป Foundry Local - สร้างแอป AI บนเครื่อง

เวิร์กช็อปแบบลงมือทำสำหรับการรันโมเดลภาษาในเครื่องของคุณเองและสร้างแอปพลิเคชันอัจฉริยะด้วย [Foundry Local](https://foundrylocal.ai) และ [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)

> **Foundry Local คืออะไร?** Foundry Local คือรันไทม์น้ำหนักเบาที่ช่วยให้คุณดาวน์โหลด จัดการ และให้บริการโมเดลภาษาได้ทั้งหมดบนฮาร์ดแวร์ของคุณเอง มันเปิดเผย **API ที่เข้ากันได้กับ OpenAI** เพื่อให้เครื่องมือหรือ SDK ใด ๆ ที่รองรับ OpenAI สามารถเชื่อมต่อได้โดยไม่ต้องใช้บัญชีคลาวด์

---

## วัตถุประสงค์การเรียนรู้

เมื่อสิ้นสุดเวิร์กช็อปนี้ คุณจะสามารถ:

| # | วัตถุประสงค์ |
|---|--------------|
| 1 | ติดตั้ง Foundry Local และจัดการโมเดลด้วย CLI |
| 2 | เชี่ยวชาญ API ของ Foundry Local SDK สำหรับการจัดการโมเดลด้วยโปรแกรม |
| 3 | เชื่อมต่อกับเซิร์ฟเวอร์ประมวลผลในเครื่องโดยใช้ SDK สำหรับ Python, JavaScript, และ C# |
| 4 | สร้าง Retrieval-Augmented Generation (RAG) pipeline ที่อิงคำตอบจากข้อมูลของคุณเอง |
| 5 | สร้างเอเย่นต์ AI ที่มีคำสั่งถาวรและบุคลิกภาพ |
| 6 | ประสานงานเวิร์กโฟลว์หลายเอเย่นต์พร้อมวงจรตอบกลับ |
| 7 | สำรวจแอปคัปสโตนสำหรับการใช้งานจริง - Zava Creative Writer |
| 8 | สร้างกรอบการประเมินด้วยชุดข้อมูลทองคำและการให้คะแนน LLM-as-judge |
| 9 | ถอดเสียงเสียงด้วย Whisper - แปลงเสียงเป็นข้อความในเครื่องโดยใช้ Foundry Local SDK |
| 10 | คอมไพล์และรันโมเดลที่กำหนดเองหรือ Hugging Face ด้วย ONNX Runtime GenAI และ Foundry Local |
| 11 | เปิดให้โมเดลในเครื่องเรียกฟังก์ชันภายนอกด้วยรูปแบบการเรียกใช้เครื่องมือ |
| 12 | สร้าง UI บนเบราว์เซอร์สำหรับ Zava Creative Writer พร้อมการสตรีมแบบเรียลไทม์ |

---

## ความต้องการเบื้องต้น

| ความต้องการ | รายละเอียด |
|-------------|------------|
| **ฮาร์ดแวร์** | RAM อย่างน้อย 8 GB (แนะนำ 16 GB); CPU ที่รองรับ AVX2 หรือ GPU ที่รองรับ |
| **ระบบปฏิบัติการ** | Windows 10/11 (x64/ARM), Windows Server 2025 หรือ macOS 13 ขึ้นไป |
| **Foundry Local CLI** | ติดตั้งโดยใช้ `winget install Microsoft.FoundryLocal` (Windows) หรือ `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) ดูคำแนะนำเริ่มต้นที่ [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| **รันไทม์ภาษา** | **Python 3.9+** และ/หรือ **.NET 9.0+** และ/หรือ **Node.js 18+** |
| **Git** | สำหรับโคลนรีโพสิทอรีนี้ |

---

## เริ่มต้นใช้งาน

```bash
# 1. โคลนที่เก็บข้อมูล
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ตรวจสอบว่า Foundry Local ได้ถูกติดตั้งแล้ว
foundry model list              # แสดงรายชื่อโมเดลที่มีอยู่
foundry model run phi-3.5-mini  # เริ่มการสนทนาแบบโต้ตอบ

# 3. เลือกเส้นทางภาษาของคุณ (ดูแล Part 2 สำหรับการตั้งค่าเต็ม)
```

| ภาษา | เริ่มต้นอย่างรวดเร็ว |
|-------|---------------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ส่วนต่าง ๆ ของเวิร์กช็อป

### ส่วนที่ 1: เริ่มต้นกับ Foundry Local

**คู่มือการทำแลป:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local คืออะไรและทำงานอย่างไร
- ติดตั้ง CLI บน Windows และ macOS
- สำรวจโมเดล - การแสดงรายการ ดาวน์โหลด และรัน
- เข้าใจนามแฝงโมเดลและพอร์ตแบบไดนามิก

---

### ส่วนที่ 2: เจาะลึก Foundry Local SDK

**คู่มือการทำแลป:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- ทำไมต้องใช้ SDK แทน CLI ในการพัฒนาแอป
- อ้างอิง API SDK เต็มรูปแบบสำหรับ Python, JavaScript และ C#
- การจัดการเซอร์วิส, การเรียกดูแคตตาล็อก และวงจรชีวิตโมเดล (ดาวน์โหลด, โหลด, ปลดโหลด)
- รูปแบบเริ่มต้นอย่างรวดเร็ว: คอนสตรัคเตอร์ของ Python, `init()` ของ JavaScript, `CreateAsync()` ของ C#
- เมตาดาต้า `FoundryModelInfo`, นามแฝง และการเลือกโมเดลที่เหมาะสมกับฮาร์ดแวร์

---

### ส่วนที่ 3: SDK และ API

**คู่มือการทำแลป:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- การเชื่อมต่อกับ Foundry Local จาก Python, JavaScript และ C#
- ใช้ Foundry Local SDK เพื่อจัดการเซอร์วิสแบบโปรแกรม
- สตรีมการสนทนาแบบ chat ผ่าน API ที่เข้ากันได้กับ OpenAI
- อ้างอิงเมธอด SDK สำหรับแต่ละภาษา

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|--------|--------|----------|
| Python | `python/foundry-local.py` | การสตรีมแชทพื้นฐาน |
| C# | `csharp/BasicChat.cs` | การสตรีมแชทด้วย .NET |
| JavaScript | `javascript/foundry-local.mjs` | การสตรีมแชทด้วย Node.js |

---

### ส่วนที่ 4: Retrieval-Augmented Generation (RAG)

**คู่มือการทำแลป:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG คืออะไรและทำไมจึงสำคัญ
- สร้างฐานความรู้ในหน่วยความจำ
- การดึงข้อมูลโดยใช้คำหลักซ้อนทับพร้อมคะแนน
- การเรียบเรียงพรอมต์ระบบที่มีหลักฐาน
- รัน RAG pipeline แบบสมบูรณ์ในเครื่อง

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ |
|--------|---------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ส่วนที่ 5: การสร้างเอเย่นต์ AI

**คู่มือการทำแลป:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI agent คืออะไร (เทียบกับการเรียก LLM อย่างตรงไปตรงมา)
- รูปแบบ `ChatAgent` และ Microsoft Agent Framework
- คำสั่งระบบ บุคลิกภาพ และการสนทนาแบบหลายรอบ
- ผลลัพธ์แบบมีโครงสร้าง (JSON) จากเอเย่นต์

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|--------|--------|----------|
| Python | `python/foundry-local-with-agf.py` | เอเย่นต์เดียวกับ Agent Framework |
| C# | `csharp/SingleAgent.cs` | เอเย่นต์เดียว (รูปแบบ ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | เอเย่นต์เดียว (รูปแบบ ChatAgent) |

---

### ส่วนที่ 6: เวิร์กโฟลว์หลายเอเย่นต์

**คู่มือการทำแลป:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline หลายเอเย่นต์: นักวิจัย → นักเขียน → บรรณาธิการ
- การจัดลำดับและวงจรตอบกลับ
- การแชร์การตั้งค่าและการส่งมอบแบบมีโครงสร้าง
- ออกแบบเวิร์กโฟลว์หลายเอเย่นต์ของคุณเอง

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|--------|--------|------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline สามเอเย่นต์ |
| C# | `csharp/MultiAgent.cs` | Pipeline สามเอเย่นต์ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline สามเอเย่นต์ |

---

### ส่วนที่ 7: Zava Creative Writer - แอปคัปสโตน

**คู่มือการทำแลป:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- แอปหลายเอเย่นต์สไตล์การผลิตที่มีเอเย่นต์เฉพาะทาง 4 ตัว
- Pipeline ตามลำดับด้วยวงจรตอบกลับที่ขับเคลื่อนโดย evaluator
- สตรีมเอาต์พุต การค้นหาคาตาล็อกสินค้า และการส่งมอบ JSON แบบมีโครงสร้าง
- การใช้งานเต็มรูปแบบใน Python (FastAPI), JavaScript (Node.js CLI) และ C# (.NET console)

**ตัวอย่างโค้ด:**

| ภาษา | โฟลเดอร์ | คำอธิบาย |
|--------|-----------|------------|
| Python | `zava-creative-writer-local/src/api/` | บริการเว็บ FastAPI พร้อม orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | แอป Node.js CLI |
| C# | `zava-creative-writer-local/src/csharp/` | แอปคอนโซล .NET 9 |

---

### ส่วนที่ 8: การพัฒนาที่ขับเคลื่อนด้วยการประเมิน

**คู่มือการทำแลป:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- สร้างกรอบการประเมินอย่างเป็นระบบสำหรับเอเย่นต์ AI ด้วยชุดข้อมูลทองคำ
- การตรวจสอบตามกฎ (ความยาว, การครอบคลุมคำสำคัญ, คำต้องห้าม) + การให้คะแนนโดย LLM-as-judge
- เปรียบเทียบพรอมต์แบบเคียงข้างพร้อมสกอร์การ์ดรวม
- ขยายรูปแบบเอเย่นต์ Zava Editor จากส่วนที่ 7 เป็นชุดทดสอบแบบออฟไลน์
- เส้นทาง Python, JavaScript และ C#

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|--------|--------|------------|
| Python | `python/foundry-local-eval.py` | กรอบการประเมิน |
| C# | `csharp/AgentEvaluation.cs` | กรอบการประเมิน |
| JavaScript | `javascript/foundry-local-eval.mjs` | กรอบการประเมิน |

---

### ส่วนที่ 9: ถอดเสียงด้วย Whisper

**คู่มือการทำแลป:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- การถอดเสียงพูดเป็นข้อความโดยใช้ OpenAI Whisper ที่รันในเครื่อง
- การประมวลผลเสียงที่เน้นความเป็นส่วนตัว - เสียงไม่ถูกส่งออกจากอุปกรณ์ของคุณ
- เส้นทาง Python, JavaScript และ C# พร้อม `client.audio.transcriptions.create()` (Python/JS) และ `AudioClient.TranscribeAudioAsync()` (C#)
- รวมไฟล์เสียงตัวอย่างธีม Zava สำหรับฝึกปฏิบัติ

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|--------|--------|------------|
| Python | `python/foundry-local-whisper.py` | การถอดเสียงด้วย Whisper |
| C# | `csharp/WhisperTranscription.cs` | การถอดเสียงด้วย Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | การถอดเสียงด้วย Whisper |

> **หมายเหตุ:** แลปนี้ใช้ **Foundry Local SDK** เพื่อดาวน์โหลดและโหลดโมเดล Whisper แบบโปรแกรม จากนั้นส่งเสียงไปยัง endpoint ในเครื่องที่เข้ากันได้กับ OpenAI สำหรับการถอดเสียง โมเดล Whisper (`whisper`) ถูกระบุในแคตตาล็อก Foundry Local และรันทั้งหมดบนเครื่อง — ไม่ต้องใช้คีย์ API คลาวด์หรือการเข้าถึงเครือข่าย

---

### ส่วนที่ 10: การใช้โมเดลที่กำหนดเองหรือ Hugging Face

**คู่มือการทำแลป:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- การคอมไพล์โมเดล Hugging Face เป็นรูปแบบ ONNX ที่ปรับแต่งโดยใช้ ONNX Runtime GenAI model builder
- การคอมไพล์เฉพาะฮาร์ดแวร์ (CPU, NVIDIA GPU, DirectML, WebGPU) และการควอนติเซชัน (int4, fp16, bf16)
- การสร้างไฟล์กำหนดค่าช่องแชทสำหรับ Foundry Local
- การเพิ่มโมเดลที่คอมไพล์แล้วในแคชของ Foundry Local
- การรันโมเดลกำหนดเองผ่าน CLI, REST API และ OpenAI SDK
- ตัวอย่างอ้างอิง: คอมไพล์ Qwen/Qwen3-0.6B แบบครบวงจร

---

### ส่วนที่ 11: การเรียกใช้เครื่องมือกับโมเดลภายในเครื่อง

**คู่มือการทำแลป:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- เปิดให้โมเดลในเครื่องเรียกใช้งานฟังก์ชันภายนอก (เรียกเครื่องมือ/ฟังก์ชัน)
- กำหนดสคีมาเครื่องมือโดยใช้รูปแบบการเรียกใช้ฟังก์ชัน OpenAI
- จัดการการสนทนาแบบหลายรอบสำหรับการเรียกเครื่องมือ
- ดำเนินการเรียกเครื่องมือในเครื่องและส่งผลลัพธ์กลับไปยังโมเดล
- เลือกโมเดลที่เหมาะสำหรับสถานการณ์เรียกเครื่องมือ (Qwen 2.5, Phi-4-mini)
- ใช้ `ChatClient` ของ SDK สำหรับการเรียกเครื่องมือ (JavaScript)

**ตัวอย่างโค้ด:**

| ภาษา | ไฟล์ | คำอธิบาย |
|--------|--------|----------|
| Python | `python/foundry-local-tool-calling.py` | การเรียกเครื่องมือด้วยเครื่องมือสภาพอากาศ/ประชากร |
| C# | `csharp/ToolCalling.cs` | การเรียกเครื่องมือด้วย .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | การเรียกเครื่องมือด้วย ChatClient |

---

### ส่วนที่ 12: สร้าง Web UI สำหรับ Zava Creative Writer

**คู่มือการทำแลป:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- เพิ่มส่วนหน้าเว็บบนเบราว์เซอร์สำหรับ Zava Creative Writer
- ให้บริการ UI ร่วมจาก Python (FastAPI), JavaScript (Node.js HTTP), และ C# (ASP.NET Core)
- ใช้สตรีม NDJSON ในเบราว์เซอร์ด้วย Fetch API และ ReadableStream
- แสดงสถานะเอเย่นต์สดและสตรีมเนื้อหาบทความแบบเรียลไทม์

**โค้ด (UI ร่วม):**

| ไฟล์ | คำอธิบาย |
|--------|------------|
| `zava-creative-writer-local/ui/index.html` | เค้าโครงหน้า |
| `zava-creative-writer-local/ui/style.css` | การตกแต่งสไตล์ |
| `zava-creative-writer-local/ui/app.js` | ตัวอ่านสตรีมและตรรกะอัปเดต DOM |

**ส่วนเสริมแบ็คเอนด์:**

| ภาษา | ไฟล์ | คำอธิบาย |
|--------|--------|------------|
| Python | `zava-creative-writer-local/src/api/main.py` | อัปเดตเพื่อให้บริการ UI แบบสแตติก |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | เซิร์ฟเวอร์ HTTP ใหม่ที่ครอบ orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | โครงการ API แบบมินิมัล ASP.NET Core ใหม่ |

---

### ส่วนที่ 13: เสร็จสิ้นเวิร์กช็อป
**คำแนะนำห้องปฏิบัติการ:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- สรุปทุกอย่างที่คุณได้สร้างขึ้นในทั้งหมด 12 ส่วน
- ไอเดียเพิ่มเติมสำหรับการขยายแอปพลิเคชันของคุณ
- ลิงก์ไปยังทรัพยากรและเอกสารประกอบ

---

## โครงสร้างโปรเจกต์

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

## ทรัพยากร

| ทรัพยากร | ลิงก์ |
|----------|------|
| เว็บไซต์ Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| แคตตาล็อกโมเดล | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| คู่มือเริ่มต้นใช้งาน | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| อ้างอิง SDK ของ Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## ใบอนุญาต

วัสดุสำหรับเวิร์กช็อปนี้จัดทำเพื่อวัตถุประสงค์ทางการศึกษา

---

**ขอให้สร้างสรรค์อย่างมีความสุข! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อจำกัดความรับผิดชอบ**:  
เอกสารฉบับนี้ได้รับการแปลโดยใช้บริการแปลภาษา AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้ว่าเราจะพยายามให้ความถูกต้องสูงสุด แต่โปรดทราบว่าการแปลอัตโนมัติอาจมีข้อผิดพลาดหรือความไม่ถูกต้อง เอกสารต้นฉบับในภาษาดั้งเดิมถือเป็นแหล่งข้อมูลที่น่าเชื่อถือ สำหรับข้อมูลที่สำคัญ ควรใช้บริการแปลโดยผู้เชี่ยวชาญมนุษย์ เราไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความที่ผิดพลาดที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->