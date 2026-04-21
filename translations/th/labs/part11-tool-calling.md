![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ตอนที่ 11: การเรียกใช้เครื่องมือด้วยโมเดลภายในเครื่อง

> **เป้าหมาย:** เปิดใช้งานโมเดลภายในเครื่องของคุณให้เรียกใช้ฟังก์ชันภายนอก (เครื่องมือ) เพื่อให้สามารถดึงข้อมูลเรียลไทม์ ทำการคำนวณ หรือโต้ตอบกับ API — ทั้งหมดนี้ทำงานแบบส่วนตัวบนอุปกรณ์ของคุณ

## การเรียกใช้เครื่องมือคืออะไร?

การเรียกใช้เครื่องมือ (เรียกอีกอย่างว่า **การเรียกใช้ฟังก์ชัน**) ช่วยให้โมเดลภาษาเรียกร้องให้ดำเนินการฟังก์ชันที่คุณกำหนด แทนที่จะเดาคำตอบ โมเดลจะรู้ว่าเมื่อไหร่ที่เครื่องมือจะช่วยได้และส่งคำขอที่มีโครงสร้างให้โค้ดของคุณเรียกใช้ ฟังก์ชันจะถูกเรียกในแอปพลิเคชันของคุณ ส่งผลลัพธ์กลับ และโมเดลจะนำข้อมูลนั้นมารวมในคำตอบสุดท้าย

![Tool-calling flow](../../../images/tool-calling-flow.svg)

รูปแบบนี้เป็นสิ่งจำเป็นสำหรับการสร้างเอเจนต์ที่สามารถ:

- **ค้นหาข้อมูลสด** (สภาพอากาศ ราคาหุ้น การค้นหาฐานข้อมูล)
- **ทำการคำนวณอย่างแม่นยำ** (คณิตศาสตร์ การแปลงหน่วย)
- **ดำเนินการต่างๆ** (ส่งอีเมล สร้างตั๋ว อัปเดตข้อมูล)
- **เข้าถึงระบบส่วนตัว** (API ภายใน ระบบไฟล์)

---

## วิธีการทำงานของการเรียกใช้เครื่องมือ

กระบวนการเรียกใช้เครื่องมือมีสี่ขั้นตอน:

| ขั้นตอน | สิ่งที่เกิดขึ้น |
|---------|----------------|
| **1. กำหนดเครื่องมือ** | อธิบายฟังก์ชันที่ใช้งานได้โดยใช้ JSON Schema — ชื่อ คำอธิบาย และพารามิเตอร์ |
| **2. โมเดลตัดสินใจ** | โมเดลได้รับข้อความของคุณพร้อมกับคำจำกัดความของเครื่องมือ ถ้าเครื่องมือช่วยได้ โมเดลจะส่ง `tool_calls` แทนคำตอบข้อความธรรมดา |
| **3. ดำเนินการในเครื่อง** | โค้ดของคุณวิเคราะห์คำเรียกเครื่องมือ เรียกใช้ฟังก์ชัน และเก็บผลลัพธ์ |
| **4. คำตอบสุดท้าย** | คุณส่งผลลัพธ์เครื่องมือกลับไปยังโมเดล ซึ่งจะสร้างคำตอบสุดท้าย |

> **จุดสำคัญ:** โมเดลไม่เคยเรียกใช้โค้ดเอง มันเพียงแค่ *ร้องขอ* ให้เรียกใช้เครื่องมือ แอปพลิเคชันของคุณเป็นผู้ตัดสินใจว่าจะตอบสนองคำขอนั้นหรือไม่ — ทำให้คุณควบคุมได้ทั้งหมด

---

## โมเดลใดที่รองรับการเรียกใช้เครื่องมือ?

ไม่ใช่ทุกโมเดลจะรองรับการเรียกใช้เครื่องมือ ในแคตตาล็อก Foundry Local ปัจจุบัน โมเดลต่อไปนี้รองรับความสามารถเรียกใช้เครื่องมือ:

| โมเดล | ขนาด | การเรียกใช้เครื่องมือ |
|-------|-------|:---------------------:|
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

> **เคล็ดลับ:** สำหรับแลปนี้เราใช้ **qwen2.5-0.5b** — โมเดลขนาดเล็ก (ดาวน์โหลด 822 MB), รวดเร็ว และรองรับการเรียกใช้เครื่องมือได้อย่างน่าเชื่อถือ

---

## เป้าหมายการเรียนรู้

เมื่อจบแลปนี้คุณจะสามารถ:

- อธิบายรูปแบบการเรียกใช้เครื่องมือและเหตุผลว่าทำไมถึงสำคัญสำหรับเอเจนต์ AI
- กำหนดสคีม่าเครื่องมือโดยใช้รูปแบบการเรียกใช้ฟังก์ชันของ OpenAI
- จัดการกับการสนทนาแบบหลายเทิร์นสำหรับการเรียกใช้เครื่องมือ
- เรียกใช้เครื่องมือในเครื่องและส่งผลลัพธ์กลับสู่โมเดล
- เลือกโมเดลที่เหมาะสมสำหรับสถานการณ์เรียกใช้เครื่องมือ

---

## ข้อกำหนดเบื้องต้น

| ข้อกำหนด | รายละเอียด |
|-----------|------------|
| **Foundry Local CLI** | ติดตั้งและอยู่ใน `PATH` ของคุณ ([ตอนที่ 1](part1-getting-started.md)) |
| **Foundry Local SDK** | ติดตั้ง SDK ของ Python, JavaScript หรือ C# ([ตอนที่ 2](part2-foundry-local-sdk.md)) |
| **โมเดลสำหรับเรียกเครื่องมือ** | qwen2.5-0.5b (จะดาวน์โหลดอัตโนมัติ) |

---

## แบบฝึกหัด

### แบบฝึกหัด 1 — ทำความเข้าใจกระบวนการเรียกใช้เครื่องมือ

ก่อนเขียนโค้ด ให้ศึกษาผังลำดับนี้:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**ข้อสังเกตหลัก:**

1. คุณกำหนดเครื่องมือไว้ล่วงหน้าในรูปแบบวัตถุ JSON Schema
2. การตอบกลับของโมเดลประกอบด้วย `tool_calls` แทนเนื้อหาปกติ
3. การเรียกใช้เครื่องมือแต่ละครั้งมี `id` เฉพาะที่คุณต้องอ้างอิงเมื่อส่งผลลัพธ์กลับ
4. โมเดลเห็นข้อความทั้งหมดก่อนหน้า *พร้อมด้วย* ผลลัพธ์ของเครื่องมือเวลาสร้างคำตอบสุดท้าย
5. อาจมีการเรียกใช้เครื่องมือหลายครั้งในคำตอบเดียว

> **อภิปราย:** ทำไมโมเดลถึงส่งคืนการเรียกใช้เครื่องมือแทนการเรียกฟังก์ชันโดยตรง? ข้อได้เปรียบด้านความปลอดภัยที่ได้รับคืออะไร?

---

### แบบฝึกหัด 2 — การกำหนดสคีม่าเครื่องมือ

เครื่องมือถูกกำหนดโดยใช้รูปแบบการเรียกใช้ฟังก์ชันมาตรฐานของ OpenAI เครื่องมือแต่ละชิ้นต้องมี:

- **`type`**: กำหนดเป็น `"function"` เสมอ
- **`function.name`**: ชื่อฟังก์ชันที่อธิบายได้ (เช่น `get_weather`)
- **`function.description`**: คำอธิบายชัดเจน — โมเดลใช้ส่วนนี้ตัดสินใจว่าจะเรียกเครื่องมือเมื่อใด
- **`function.parameters`**: วัตถุ JSON Schema ที่ระบุพารามิเตอร์ที่คาดหวัง

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

> **แนวปฏิบัติที่ดีสำหรับคำอธิบายเครื่องมือ:**
> - ให้ชัดเจน: "ดึงสภาพอากาศปัจจุบันสำหรับเมืองที่ระบุ" ดีกว่า "ดึงสภาพอากาศ"
> - อธิบายพารามิเตอร์อย่างชัดเจน: โมเดลอ่านคำอธิบายเหล่านี้เพื่อเติมค่าอย่างถูกต้อง
> - กำหนดพารามิเตอร์ที่ต้องการและไม่ต้องการ — ช่วยให้โมเดลตัดสินใจว่าจะถามอะไร

---

### แบบฝึกหัด 3 — รันตัวอย่างการเรียกใช้เครื่องมือ

ตัวอย่างแต่ละภาษาใช้เครื่องมือสองตัว (`get_weather` และ `get_population`), ส่งคำถามที่กระตุ้นการใช้เครื่องมือ, ทำการเรียกใช้เครื่องมือภายในเครื่อง, และส่งผลลัพธ์กลับสำหรับคำตอบสุดท้าย

<details>
<summary><strong>🐍 Python</strong></summary>

**ข้อกำหนดเบื้องต้น:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**รัน:**
```bash
python foundry-local-tool-calling.py
```

**ผลลัพธ์ที่คาดหวัง:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**คำอธิบายโค้ด** (`python/foundry-local-tool-calling.py`):

```python
# กำหนดเครื่องมือเป็นรายการของสคีมาฟังก์ชัน
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

# ส่งพร้อมกับเครื่องมือ — โมเดลอาจส่ง tool_calls แทนเนื้อหา
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# ตรวจสอบว่าโมเดลต้องการเรียกใช้เครื่องมือหรือไม่
if response.choices[0].message.tool_calls:
    # เรียกใช้เครื่องมือและส่งผลลัพธ์กลับ
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**ข้อกำหนดเบื้องต้น:**
```bash
cd javascript
npm install
```

**รัน:**
```bash
node foundry-local-tool-calling.mjs
```

**ผลลัพธ์ที่คาดหวัง:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**คำอธิบายโค้ด** (`javascript/foundry-local-tool-calling.mjs`):

ตัวอย่างนี้ใช้ `ChatClient` ของ Foundry Local SDK แทน OpenAI SDK โดยใช้เมธอดสะดวก `createChatClient()`:

```javascript
// รับ ChatClient โดยตรงจากวัตถุโมเดล
const chatClient = model.createChatClient();

// ส่งด้วยเครื่องมือ — ChatClient จัดการรูปแบบที่เข้ากันได้กับ OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// ตรวจสอบการเรียกใช้เครื่องมือ
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // ดำเนินการเครื่องมือและส่งผลลัพธ์กลับคืน
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**ข้อกำหนดเบื้องต้น:**
```bash
cd csharp
dotnet restore
```

**รัน:**
```bash
dotnet run toolcall
```

**ผลลัพธ์ที่คาดหวัง:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**คำอธิบายโค้ด** (`csharp/ToolCalling.cs`):

ใน C# ใช้ตัวช่วย `ChatTool.CreateFunctionTool` สำหรับกำหนดเครื่องมือ:

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

### แบบฝึกหัด 4 — กระแสสนทนาเรียกใช้เครื่องมือ

เข้าใจโครงสร้างข้อความเป็นสิ่งสำคัญ ด้านล่างคือกระแสเต็มที่แสดงอาเรย์ `messages` ในแต่ละขั้นตอน:

**ขั้นตอน 1 — คำขอเริ่มต้น:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**ขั้นตอน 2 — โมเดลตอบกลับด้วย tool_calls (ไม่ใช่เนื้อหา):**
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

**ขั้นตอน 3 — คุณเพิ่มข้อความผู้ช่วยและผลลัพธ์ของเครื่องมือ:**
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

**ขั้นตอน 4 — โมเดลสร้างคำตอบสุดท้ายโดยใช้ผลลัพธ์เครื่องมือ**

> **สำคัญ:** `tool_call_id` ในข้อความเครื่องมือจะต้องตรงกับ `id` จากการเรียกใช้เครื่องมือ วิธีนี้ทำให้โมเดลเชื่อมโยงผลลัพธ์กับคำขอได้

---

### แบบฝึกหัด 5 — การเรียกใช้เครื่องมือหลายตัว

โมเดลสามารถร้องขอเครื่องมือหลายตัวในคำตอบเดียว ลองเปลี่ยนข้อความผู้ใช้เพื่อกระตุ้นการเรียกหลายครั้ง:

```python
# ในภาษาไพธอน — เปลี่ยนข้อความผู้ใช้:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// ใน JavaScript — เปลี่ยนข้อความผู้ใช้:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

โมเดลควรส่งคืนสอง `tool_calls` — ตัวหนึ่งสำหรับ `get_weather` และอีกตัวสำหรับ `get_population` โค้ดของคุณรองรับสิ่งนี้แล้วเพราะวนลูปผ่านการเรียกเครื่องมือทั้งหมด

> **ลองดู:** เปลี่ยนข้อความผู้ใช้และรันตัวอย่างอีกครั้ง โมเดลเรียกใช้ทั้งสองเครื่องมือหรือไม่?

---

### แบบฝึกหัด 6 — เพิ่มเครื่องมือของคุณเอง

ขยายหนึ่งในตัวอย่างด้วยเครื่องมือใหม่ เช่น เพิ่มเครื่องมือ `get_time`:

1. กำหนดสคีม่าเครื่องมือ:
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

2. เพิ่มตรรกะการดำเนินการ:
```python
# ไพทอน
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # ในแอปจริง ให้ใช้ไลบรารีโซนเวลา
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... เครื่องมือที่มีอยู่ ...
```

3. เพิ่มเครื่องมือลงในอาเรย์ `tools` และทดสอบด้วย: "ตอนนี้เวลาเท่าไหร่ที่โตเกียว?"

> **ท้าทาย:** เพิ่มเครื่องมือสำหรับคำนวณ เช่น `convert_temperature` ที่แปลงระหว่างเซลเซียสกับฟาเรนไฮต์ ทดสอบด้วย: "แปลง 100°F เป็นเซลเซียส"

---

### แบบฝึกหัด 7 — การเรียกใช้เครื่องมือด้วย ChatClient ใน SDK (JavaScript)

ตัวอย่าง JavaScript ใช้ `ChatClient` ของ SDK แทน OpenAI SDK ซึ่งเป็นฟีเจอร์ที่สะดวก ช่วยให้ไม่ต้องสร้างไคลเอนต์ OpenAI เอง:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient ถูกสร้างขึ้นโดยตรงจากวัตถุโมเดล
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat รับเครื่องมือเป็นพารามิเตอร์ที่สอง
const response = await chatClient.completeChat(messages, tools);
```

เปรียบเทียบกับวิธี Python ที่ใช้ OpenAI SDK โดยตรง:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

ทั้งสองรูปแบบเหมาะสม `ChatClient` สะดวกกว่า ส่วน OpenAI SDK ให้เข้าถึงพารามิเตอร์ OpenAI ได้ครบถ้วน

> **ลองดู:** แก้ตัวอย่าง JavaScript ให้ใช้ OpenAI SDK แทน `ChatClient` คุณจะต้อง `import OpenAI from "openai"` และสร้างไคลเอนต์ด้วย endpoint จาก `manager.urls[0]`

---

### แบบฝึกหัด 8 — ทำความเข้าใจ `tool_choice`

พารามิเตอร์ `tool_choice` ควบคุมว่าโมเดล *ต้อง* ใช้เครื่องมือหรือเลือกใช้เองได้:

| ค่า | พฤติกรรม |
|------|----------|
| `"auto"` | โมเดลตัดสินใจว่าจะเรียกใช้เครื่องมือหรือไม่ (ค่าเริ่มต้น) |
| `"none"` | โมเดลจะไม่เรียกใช้เครื่องมือใดๆ แม้ว่ามีให้ |
| `"required"` | โมเดลต้องเรียกใช้เครื่องมืออย่างน้อยหนึ่งครั้ง |
| `{"type": "function", "function": {"name": "get_weather"}}` | โมเดลต้องเรียกใช้เครื่องมือที่ระบุ |

ลองแต่ละตัวเลือกในตัวอย่าง Python:

```python
# บังคับโมเดลให้เรียกใช้ get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **หมายเหตุ:** ตัวเลือก `tool_choice` บางค่าอาจไม่ได้รับการสนับสนุนในทุกโมเดล หากโมเดลไม่รองรับ `"required"` อาจละเลยการตั้งค่านี้และทำงานเหมือน `"auto"`

---

## ปัญหาทั่วไป

| ปัญหา | วิธีแก้ไข |
|--------|-----------|
| โมเดลไม่เคยเรียกใช้เครื่องมือ | ตรวจสอบว่าคุณใช้โมเดลที่รองรับการเรียกเครื่องมือ (เช่น qwen2.5-0.5b) ดูตารางด้านบน |
| `tool_call_id` ไม่ตรงกัน | ใช้ `id` จากการตอบ `tool_call` เสมอ อย่าใช้ค่าที่กำหนดเอง |
| โมเดลส่ง JSON ไม่ถูกต้องใน `arguments` | โมเดลขนาดเล็กบางครั้งสร้าง JSON ผิดพลาด ห่อ `JSON.parse()` ด้วย try/catch |
| โมเดลเรียกใช้เครื่องมือที่ไม่มี | เพิ่ม handler ดีฟอลต์ในฟังก์ชัน `execute_tool` ของคุณ |
| วนลูปเรียกใช้เครื่องมือไม่สิ้นสุด | ตั้งจำนวนรอบสูงสุด (เช่น 5) เพื่อป้องกันลูปทำงานไม่หยุด |

---

## ข้อสรุปสำคัญ

1. **การเรียกใช้เครื่องมือ** ให้โมเดลร้องขอเรียกฟังก์ชันแทนการเดาคำตอบ
2. โมเดล **ไม่เคยเรียกใช้โค้ด** แอปพลิเคชันเป็นผู้ควบคุมว่าอะไรจะถูกเรียกใช้
3. เครื่องมือถูกกำหนดเป็นวัตถุ **JSON Schema** ตามรูปแบบการเรียกฟังก์ชันของ OpenAI
4. การสนทนาใช้รูปแบบ **หลายเทิร์น**: ผู้ใช้ → ผู้ช่วย (tool_calls) → เครื่องมือ (ผลลัพธ์) → ผู้ช่วย (คำตอบสุดท้าย)
5. ใช้โมเดลที่ **รองรับการเรียกใช้เครื่องมือ** เสมอ (Qwen 2.5, Phi-4-mini)
6. เมธอด `createChatClient()` ใน SDK ให้วิธีสะดวกสำหรับร้องขอเรียกเครื่องมือโดยไม่ต้องสร้างไคลเอนต์ OpenAI เอง

---

ไปต่อที่ [ตอนที่ 12: สร้าง UI เว็บสำหรับ Zava Creative Writer](part12-zava-ui.md) เพื่อเพิ่มอินเทอร์เฟซเบราว์เซอร์ให้กับสายการทำงานมัลติเอเจนต์ที่สตรีมข้อมูลเรียลไทม์

---

[← ตอนที่ 10: โมเดลกำหนดเอง](part10-custom-models.md) | [ตอนที่ 12: UI นักเขียน Zava →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อจำกัดความรับผิดชอบ**:  
เอกสารฉบับนี้ได้รับการแปลโดยใช้บริการแปลภาษา AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้ว่าเราจะพยายามให้ความถูกต้องสูงสุด แต่โปรดทราบว่าการแปลอัตโนมัติอาจมีข้อผิดพลาดหรือความไม่ถูกต้อง เอกสารต้นฉบับในภาษาต้นทางควรถือเป็นแหล่งข้อมูลที่เชื่อถือได้ สำหรับข้อมูลที่สำคัญ ขอแนะนำให้ใช้บริการแปลโดยมนุษย์มืออาชีพ เราไม่รับผิดชอบใดๆ ต่อความเข้าใจผิดหรือการตีความผิดที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->