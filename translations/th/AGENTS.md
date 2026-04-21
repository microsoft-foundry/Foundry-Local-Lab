# คำแนะนำสำหรับเอเจนท์โค้ดดิ้ง

ไฟล์นี้ให้บริบทสำหรับเอเจนท์โค้ดดิ้ง AI (GitHub Copilot, Copilot Workspace, Codex ฯลฯ) ที่ทำงานในรีโพสนี้

## ภาพรวมโครงการ

นี่คือ **เวิร์กช็อปเชิงปฏิบัติ** สำหรับการสร้างแอปพลิเคชัน AI ด้วย [Foundry Local](https://foundrylocal.ai) — รันไทม์น้ำหนักเบาที่ดาวน์โหลด จัดการ และให้บริการโมเดลภาษาได้ทั้งหมดบนอุปกรณ์ผ่าน API ที่เข้ากันได้กับ OpenAI เวิร์กช็อปประกอบด้วยคำแนะนำห้องแล็บทีละขั้นตอนและตัวอย่างโค้ดที่รันได้ใน Python, JavaScript, และ C#

## โครงสร้างรีโพส

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## รายละเอียดภาษาและเฟรมเวิร์ก

### Python
- **ที่ตั้ง:** `python/`, `zava-creative-writer-local/src/api/`
- **การพึ่งพิง:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **แพ็กเกจหลัก:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **เวอร์ชันขั้นต่ำ:** Python 3.9+
- **รัน:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **ที่ตั้ง:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **การพึ่งพิง:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **แพ็กเกจหลัก:** `foundry-local-sdk`, `openai`
- **ระบบโมดูล:** โมดูล ES (`.mjs` ไฟล์, `"type": "module"`)
- **เวอร์ชันขั้นต่ำ:** Node.js 18+
- **รัน:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **ที่ตั้ง:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **ไฟล์โปรเจกต์:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **แพ็กเกจหลัก:** `Microsoft.AI.Foundry.Local` (ที่ไม่ใช่ Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — ซูเปอร์เซ็ตพร้อม QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **เป้าหมาย:** .NET 9.0 (TFM เงื่อนไข: `net9.0-windows10.0.26100` บน Windows, `net9.0` ที่อื่น)
- **รัน:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## ข้อบังคับการเขียนโค้ด

### ทั่วไป
- ตัวอย่างโค้ดทั้งหมดเป็น **ตัวอย่างแบบไฟล์เดียวที่ครบตัวเอง** — ไม่มีไลบรารีที่ใช้งานร่วมหรือนามธรรม
- ตัวอย่างแต่ละตัวรันได้อย่างอิสระหลังติดตั้ง dependencies ของตัวเอง
- API key ตั้งค่าเป็น `"foundry-local"` เสมอ — Foundry Local ใช้เป็นตัวแทน
- URL พื้นฐานใช้ `http://localhost:<port>/v1` — พอร์ตแบบไดนามิกและค้นพบตอนรันไทม์ผ่าน SDK (`manager.urls[0]` ใน JS, `manager.endpoint` ใน Python)
- Foundry Local SDK จัดการเริ่มเซอร์วิสและค้นหาปลายทาง; ใช้รูปแบบ SDK แทนการตั้งพอร์ตให้ตายตัว

### Python
- ใช้ `openai` SDK กับ `OpenAI(base_url=..., api_key="not-required")`
- ใช้ `FoundryLocalManager()` จาก `foundry_local` สำหรับวงจร life cycle บริการที่จัดการโดย SDK
- การสตรีม: ใช้วนลูป `for chunk in stream:`
- ไม่มีการกำกับชนิดในไฟล์ตัวอย่าง (เพื่อให้ตัวอย่างสั้นสำหรับผู้เรียนในเวิร์กช็อป)

### JavaScript
- ไวยากรณ์โมดูล ES: `import ... from "..."`.
- ใช้ `OpenAI` จาก `"openai"` และ `FoundryLocalManager` จาก `"foundry-local-sdk"`
- รูปแบบการเริ่มต้น SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`
- การสตรีม: `for await (const chunk of stream)`
- ใช้ `await` ระดับสูงสุดตลอด

### C#
- เปิดใช้ nullable, ใช้ implicit usings, .NET 9
- ใช้ `FoundryLocalManager.StartServiceAsync()` สำหรับวงจร life cycle ที่จัดการโดย SDK
- การสตรีม: `CompleteChatStreaming()` โดยใช้ `foreach (var update in completionUpdates)`
- โค้ดหลัก `csharp/Program.cs` คือ CLI router ที่ส่งต่อไปยังเมธอด static `RunAsync()`

### การเรียกเครื่องมือ
- มีเพียงโมเดลบางตัวที่รองรับการเรียกเครื่องมือ: ครอบครัว **Qwen 2.5** (`qwen2.5-*`) และ **Phi-4-mini** (`phi-4-mini`)
- สคีมาของเครื่องมือปฏิบัติตามฟอร์แมต JSON function-calling ของ OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`)
- การสนทนาใช้รูปแบบหลายเทิร์น: user → assistant (tool_calls) → tool (results) → assistant (final answer)
- `tool_call_id` ในข้อความผลลัพธ์ของเครื่องมือจะต้องตรงกับ `id` ของการเรียกเครื่องมือในโมเดล
- Python ใช้ OpenAI SDK โดยตรง; JavaScript ใช้ `ChatClient` ดั้งเดิมของ SDK (`model.createChatClient()`); C# ใช้ OpenAI SDK กับ `ChatTool.CreateFunctionTool()`

### ChatClient (ไคลเอนต์ SDK ดั้งเดิม)
- JavaScript: `model.createChatClient()` คืนค่า `ChatClient` ที่มี `completeChat(messages, tools?)` และ `completeStreamingChat(messages, callback)`
- C#: `model.GetChatClientAsync()` คืนค่า `ChatClient` มาตรฐานที่สามารถใช้ได้โดยไม่ต้องนำเข้าแพ็กเกจ OpenAI NuGet
- Python ไม่มี ChatClient ดั้งเดิม — ใช้ OpenAI SDK ร่วมกับ `manager.endpoint` และ `manager.api_key`
- **สำคัญ:** JavaScript `completeStreamingChat` ใช้ **รูปแบบ callback** ไม่ใช่การวนลูปแบบ async

### โมเดลเหตุผล
- `phi-4-mini-reasoning` ห่อความคิดไว้ในแท็ก `<think>...</think>` ก่อนคำตอบสุดท้าย
- ต้องแยกแยะแท็กเมื่อจำเป็นเพื่อแยกเหตุผลออกจากคำตอบ

## คู่มือห้องแล็บ

ไฟล์ห้องแล็บอยู่ใน `labs/` เป็น Markdown ตามโครงสร้างที่สอดคล้องกัน:
- รูปหัวโลโก้
- หัวเรื่องและเป้าหมายบอกเป้าหมาย
- ภาพรวม, วัตถุประสงค์การเรียนรู้, ความพร้อมก่อนเรียน
- ส่วนอธิบายแนวคิดพร้อมแผนภาพ
- แบบฝึกหัดเลขลำดับพร้อมบล็อกโค้ดและผลลัพธ์ที่คาดหวัง
- ตารางสรุป, ข้อสังเกตหลัก, การอ่านเพิ่มเติม
- ลิงก์นำทางไปส่วนถัดไป

เมื่อแก้ไขเนื้อหาห้องแล็บ:
- รักษารูปแบบ Markdown ที่มีอยู่และลำดับหัวข้อ
- บล็อกโค้ดต้องระบุภาษา (`python`, `javascript`, `csharp`, `bash`, `powershell`)
- ให้คำสั่งเชลล์ทั้งแบบ bash และ PowerShell สำหรับ OS ที่แตกต่างกัน
- ใช้สไตล์บอกกล่าว `> **Note:**`, `> **Tip:**`, และ `> **Troubleshooting:**`
- ตารางใช้รูปแบบ pipe `| Header | Header |`

## คำสั่งสร้างและทดสอบ

| การกระทำ | คำสั่ง |
|--------|---------|
| **ตัวอย่าง Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **ตัวอย่าง JS** | `cd javascript && npm install && node <script>.mjs` |
| **ตัวอย่าง C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (เว็บ)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (เว็บ)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **สร้างแผนภาพ** | `npx mmdc -i <input>.mmd -o <output>.svg` (ต้องติดตั้ง `npm install` ด้วยสิทธิ์ root) |

## การพึ่งพิงภายนอก

- ต้องติดตั้ง **Foundry Local CLI** บนเครื่องนักพัฒนา (`winget install Microsoft.FoundryLocal` หรือ `brew install foundrylocal`)
- บริการ **Foundry Local** รันในเครื่องและให้บริการ REST API ที่เข้ากันกับ OpenAI บนพอร์ตแบบไดนามิก
- ไม่ต้องใช้บริการคลาวด์, API keys, หรือการสมัคร Azure ใด ๆ ในการรันตัวอย่างใด ๆ
- ส่วนที่ 10 (โมเดลที่กำหนดเอง) ต้องการ `onnxruntime-genai` เพิ่มเติมและดาวน์โหลดน้ำหนักโมเดลจาก Hugging Face

## ไฟล์ที่ไม่ควรคอมมิต

ไฟล์ `.gitignore` ควรยกเว้น (และส่วนใหญ่ยกเว้นอยู่แล้ว):
- `.venv/` — ไดเรกทอรี Python virtual environment
- `node_modules/` — dependencies ของ npm
- `models/` — ไบนารี ONNX โมเดลคอมไพล์ (ไฟล์ใหญ่ ที่สร้างโดยส่วนที่ 10)
- `cache_dir/` — แคชดาวน์โหลดโมเดล Hugging Face
- `.olive-cache/` — ไดเรกทอรีงาน Microsoft Olive
- `samples/audio/*.wav` — ตัวอย่างเสียงที่สร้างขึ้น (สร้างใหม่ผ่าน `python samples/audio/generate_samples.py`)
- ไฟล์บิลด์มาตรฐานของ Python (`__pycache__/`, `*.egg-info/`, `dist/`, ฯลฯ)

## ใบอนุญาต

MIT — ดูที่ `LICENSE`

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อจำกัดความรับผิดชอบ**:  
เอกสารนี้ได้รับการแปลโดยใช้บริการแปลภาษา AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้เราจะพยายามให้มีความถูกต้อง โปรดทราบว่าการแปลอัตโนมัติอาจมีข้อผิดพลาดหรือความคลาดเคลื่อน เอกสารต้นฉบับในภาษาต้นฉบับควรถือเป็นแหล่งข้อมูลที่เชื่อถือได้ สำหรับข้อมูลสำคัญ แนะนำให้ใช้การแปลโดยมนุษย์ผู้เชี่ยวชาญ เราไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความผิดใด ๆ ที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->