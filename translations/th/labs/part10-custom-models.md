![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ส่วนที่ 10: การใช้โมเดลกำหนดเองหรือ Hugging Face กับ Foundry Local

> **เป้าหมาย:** คอมไพล์โมเดล Hugging Face เป็นรูปแบบ ONNX ที่ปรับแต่งแล้วซึ่ง Foundry Local ต้องการ กำหนดค่าโมเดลด้วยแม่แบบแชท เพิ่มเข้าไปในแคชท้องถิ่น และรันการอนุมานกับโมเดลโดยใช้ CLI, REST API และ OpenAI SDK

## ภาพรวม

Foundry Local มาพร้อมกับแคตตาล็อกที่คัดสรรและคอมไพล์ไว้ล่วงหน้าแล้ว แต่คุณไม่จำกัดเฉพาะรายชื่อเหล่านั้น โมเดลภาษาแบบทรานส์ฟอร์เมอร์ใดๆ ที่มีอยู่บน [Hugging Face](https://huggingface.co/) (หรือเก็บไว้ในเครื่องในรูปแบบ PyTorch / Safetensors) สามารถถูกคอมไพล์เป็นโมเดล ONNX ที่ปรับแต่งแล้วและให้บริการผ่าน Foundry Local ได้

กระบวนการคอมไพล์ใช้ **ONNX Runtime GenAI Model Builder** เครื่องมือคำสั่งที่รวมอยู่ในแพ็กเกจ `onnxruntime-genai` ตัวสร้างโมเดลนี้จะจัดการงานหนัก: ดาวน์โหลดเวทซอร์ส แปลงเป็นรูปแบบ ONNX ใช้การควอนไทซ์ (int4, fp16, bf16) และสร้างไฟล์การกำหนดค่า (รวมถึงแม่แบบแชทและตัวแยกโทเค็น) ที่ Foundry Local คาดหวัง

ในการทดลองนี้ คุณจะคอมไพล์ **Qwen/Qwen3-0.6B** จาก Hugging Face ลงทะเบียนกับ Foundry Local และสนทนาแบบเต็มที่บนอุปกรณ์ของคุณ

---

## วัตถุประสงค์การเรียนรู้

เมื่อสิ้นสุดการทดลองนี้ คุณจะสามารถ:

- อธิบายว่าเหตุใดการคอมไพล์โมเดลกำหนดเองจึงมีประโยชน์และเมื่อใดที่คุณอาจจำเป็นต้องใช้
- ติดตั้ง ONNX Runtime GenAI model builder
- คอมไพล์โมเดล Hugging Face เป็นรูปแบบ ONNX ที่ปรับแต่งด้วยคำสั่งเดียว
- เข้าใจพารามิเตอร์สำคัญของการคอมไพล์ (execution provider, precision)
- สร้างไฟล์กำหนดค่า `inference_model.json` สำหรับแม่แบบแชท
- เพิ่มโมเดลที่คอมไพล์แล้วลงในแคชของ Foundry Local
- รันการอนุมานกับโมเดลกำหนดเองโดยใช้ CLI, REST API และ OpenAI SDK

---

## ความต้องการเบื้องต้น

| ความต้องการ | รายละเอียด |
|-------------|-------------|
| **Foundry Local CLI** | ติดตั้งและอยู่ใน `PATH` ของคุณ ([ส่วนที่ 1](part1-getting-started.md)) |
| **Python 3.10+** | จำเป็นสำหรับ ONNX Runtime GenAI model builder |
| **pip** | ตัวจัดการแพ็กเกจ Python |
| **พื้นที่ดิสก์** | อย่างน้อย 5 GB สำหรับไฟล์ต้นฉบับและโมเดลที่คอมไพล์ |
| **บัญชี Hugging Face** | บางโมเดลต้องยอมรับใบอนุญาตก่อนดาวน์โหลด Qwen3-0.6B ใช้ใบอนุญาต Apache 2.0 และสามารถใช้งานได้ฟรี |

---

## การตั้งค่าสภาพแวดล้อม

การคอมไพล์โมเดลต้องใช้แพ็กเกจ Python ขนาดใหญ่หลายตัว (PyTorch, ONNX Runtime GenAI, Transformers) แนะนำให้สร้างสภาพแวดล้อมเสมือนเฉพาะเพื่อลดการชนกันกับ Python ระบบหรือโปรเจ็กต์อื่นๆ

```bash
# จากรากฐานของที่เก็บข้อมูล
python -m venv .venv
```

เปิดใช้งานสภาพแวดล้อม:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

อัปเกรด pip เพื่อหลีกเลี่ยงปัญหาการแก้ไขความขึ้นต่อกัน:

```bash
python -m pip install --upgrade pip
```

> **เคล็ดลับ:** หากคุณมี `.venv` จากแลบก่อนหน้าแล้ว คุณสามารถใช้ซ้ำได้ ขอเพียงเปิดใช้งานก่อนดำเนินการต่อ

---

## แนวคิด: กระบวนการคอมไพล์

Foundry Local ต้องการโมเดลในรูปแบบ ONNX พร้อมการกำหนดค่า ONNX Runtime GenAI โมเดลเปิดเผยส่วนใหญ่บน Hugging Face จะถูกแจกจ่ายในรูปแบบเวทน้ำหนัก PyTorch หรือ Safetensors ดังนั้นจึงต้องมีขั้นตอนการแปลง

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### ตัวสร้างโมเดลทำอะไร?

1. **ดาวน์โหลด** โมเดลต้นทางจาก Hugging Face (หรืออ่านจากเส้นทางในเครื่อง)
2. **แปลง** เวทน้ำหนัก PyTorch / Safetensors เป็น ONNX
3. **ควอนไทซ์** โมเดลเป็นความแม่นยำขนาดเล็กลง (เช่น int4) เพื่อลดการใช้หน่วยความจำและเพิ่มความเร็ว
4. **สร้าง** การกำหนดค่า ONNX Runtime GenAI (`genai_config.json`), แม่แบบแชท (`chat_template.jinja`), และไฟล์ตัวแยกโทเค็นทั้งหมดเพื่อให้ Foundry Local สามารถโหลดและให้บริการโมเดลได้

### ONNX Runtime GenAI Model Builder เทียบกับ Microsoft Olive

คุณอาจพบการอ้างอิงถึง **Microsoft Olive** ในฐานะเครื่องมือทดแทนสำหรับการเพิ่มประสิทธิภาพโมเดล ทั้งสองเครื่องมือสามารถสร้างโมเดล ONNX ได้ แต่ทำหน้าที่แตกต่างกันและมีข้อแลกเปลี่ยนต่างกัน:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **แพ็กเกจ** | `onnxruntime-genai` | `olive-ai` |
| **วัตถุประสงค์หลัก** | แปลงและควอนไทซ์โมเดล AI สร้างสรรค์สำหรับการอนุมาน ONNX Runtime GenAI | เฟรมเวิร์กเพิ่มประสิทธิภาพโมเดลแบบครบวงจรรองรับ backend และฮาร์ดแวร์หลายชนิด |
| **ความง่ายในการใช้งาน** | คำสั่งเดียว — แปลงและควอนไทซ์ขั้นตอนเดียว | กระบวนการทำงาน — ท่อหลายขั้นตอนที่ปรับแต่งได้ด้วย YAML/JSON |
| **รูปแบบผลลัพธ์** | รูปแบบ ONNX Runtime GenAI (พร้อมใช้งานสำหรับ Foundry Local) | ONNX ทั่วไป, ONNX Runtime GenAI, หรือรูปแบบอื่นขึ้นอยู่กับเวิร์กโฟลว์ |
| **เป้าหมายฮาร์ดแวร์** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN และอื่นๆ |
| **ตัวเลือกการควอนไทซ์** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, รวมถึงการปรับแต่งกราฟและชั้น |
| **ขอบเขตของโมเดล** | โมเดล AI สร้างสรรค์ (LLMs, SLMs) | โมเดลที่แปลงเป็น ONNX ได้แทบทุกชนิด (วิชั่น, NLP, เสียง, มัลติโมดอล) |
| **เหมาะกับ** | คอมไพล์โมเดลเดี่ยวอย่างรวดเร็วสำหรับอนุมานในเครื่อง | ท่อการผลิตต้องการการควบคุมเพิ่มประสิทธิภาพละเอียด |
| **การขึ้นต่อกัน** | ปานกลาง (PyTorch, Transformers, ONNX Runtime) | มากกว่า (เพิ่มเฟรมเวิร์ก Olive และตัวเลือกเสริมตามเวิร์กโฟลว์) |
| **การบูรณาการกับ Foundry Local** | โดยตรง — เอาต์พุตพร้อมใช้งานทันที | ต้องใช้ `--use_ort_genai` และการกำหนดค่าเพิ่มเติม |

> **ทำไมแลบนี้จึงใช้ Model Builder:** สำหรับงานคอมไพล์โมเดล Hugging Face เดี่ยวและลงทะเบียนกับ Foundry Local, Model Builder เป็นวิธีที่ง่ายและเชื่อถือได้ที่สุด มันสร้างผลลัพธ์ตามรูปแบบที่ Foundry Local คาดหวังในคำสั่งเดียว หากคุณต้องการฟีเจอร์เพิ่มประสิทธิภาพขั้นสูงในภายหลัง เช่น ควอนไทซ์ที่คำนึงถึงความแม่นยำ การปรับแต่งกราฟ หรือการปรับแต่งหลายรอบ Olive เป็นตัวเลือกที่ทรงพลัง ดูรายละเอียดเพิ่มเติมได้ใน [เอกสาร Microsoft Olive](https://microsoft.github.io/Olive/)

---

## แบบฝึกหัดในแลบ

### แบบฝึกหัด 1: ติดตั้ง ONNX Runtime GenAI Model Builder

ติดตั้งแพ็กเกจ ONNX Runtime GenAI ซึ่งประกอบด้วยเครื่องมือสร้างโมเดล:

```bash
pip install onnxruntime-genai
```

ตรวจสอบการติดตั้งโดยตรวจสอบว่าเครื่องมือสร้างโมเดลพร้อมใช้งาน:

```bash
python -m onnxruntime_genai.models.builder --help
```

คุณควรเห็นข้อความช่วยเหลือที่แสดงพารามิเตอร์เช่น `-m` (ชื่อโมเดล), `-o` (พาธเอาต์พุต), `-p` (ความแม่นยำ), และ `-e` (execution provider)

> **หมายเหตุ:** ตัวสร้างโมเดลต้องพึ่งพา PyTorch, Transformers และแพ็กเกจอื่นๆ การติดตั้งอาจใช้เวลาสองสามนาที

---

### แบบฝึกหัด 2: คอมไพล์ Qwen3-0.6B สำหรับ CPU

รันคำสั่งต่อไปนี้เพื่อดาวน์โหลดโมเดล Qwen3-0.6B จาก Hugging Face และคอมไพล์สำหรับอนุมาน CPU โดยใช้ควอนไทซ์ int4:

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell):**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### ความหมายของแต่ละพารามิเตอร์

| พารามิเตอร์ | จุดประสงค์ | ค่าที่ใช้ |
|-------------|------------|------------|
| `-m` | รหัสโมเดล Hugging Face หรือพาธไดเรกทอรีในเครื่อง | `Qwen/Qwen3-0.6B` |
| `-o` | โฟลเดอร์ที่บันทึกโมเดล ONNX ที่คอมไพล์ | `models/qwen3` |
| `-p` | ความแม่นยำของควอนไทซ์ที่ใช้เมื่อคอมไพล์ | `int4` |
| `-e` | execution provider สำหรับ ONNX Runtime (ฮาร์ดแวร์เป้าหมาย) | `cpu` |
| `--extra_options hf_token=false` | ข้ามการยืนยันตัวตน Hugging Face (ใช้ได้กับโมเดลสาธารณะ) | `hf_token=false` |

> **ใช้เวลานานเท่าไร?** เวลาคอมไพล์ขึ้นอยู่กับฮาร์ดแวร์และขนาดโมเดล สำหรับ Qwen3-0.6B กับควอนไทซ์ int4 บน CPU สมัยใหม่ คาดว่าจะใช้เวลาประมาณ 5 ถึง 15 นาที โมเดลใหญ่กว่าจะใช้เวลานานขึ้นตามสัดส่วน

เมื่อคำสั่งเสร็จสิ้น คุณควรเห็นไดเรกทอรี `models/qwen3` ซึ่งมีไฟล์โมเดลคอมไพล์ ตรวจสอบเอาต์พุต:

```bash
ls models/qwen3
```

คุณจะเห็นไฟล์รวมถึง:  
- `model.onnx` และ `model.onnx.data` — เวทน้ำหนักของโมเดลที่คอมไพล์แล้ว  
- `genai_config.json` — การกำหนดค่า ONNX Runtime GenAI  
- `chat_template.jinja` — แม่แบบแชทของโมเดล (สร้างอัตโนมัติ)  
- `tokenizer.json`, `tokenizer_config.json` — ไฟล์ตัวแยกโทเค็น  
- ไฟล์คำศัพท์และการกำหนดค่าอื่นๆ

---

### แบบฝึกหัด 3: คอมไพล์สำหรับ GPU (ไม่บังคับ)

ถ้าคุณมี NVIDIA GPU ที่รองรับ CUDA คุณสามารถคอมไพล์เวอร์ชันปรับแต่งสำหรับ GPU เพื่อให้อนุมานเร็วขึ้น:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **หมายเหตุ:** การคอมไพล์ GPU ต้องมี `onnxruntime-gpu` และติดตั้ง CUDA ที่ใช้งานได้ หากไม่พบ เครื่องมือสร้างโมเดลจะแจ้งข้อผิดพลาด คุณสามารถข้ามแบบฝึกหัดนี้ไปและใช้รุ่น CPU แทนได้

#### การอ้างอิงการคอมไพล์เฉพาะฮาร์ดแวร์

| เป้าหมาย | execution provider (`-e`) | ความแม่นยำที่แนะนำ (`-p`) |
|----------|---------------------------|----------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` หรือ `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` หรือ `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### การแลกเปลี่ยนความแม่นยำ

| ความแม่นยำ | ขนาด | ความเร็ว | คุณภาพ |
|-------------|-------|----------|---------|
| `fp32` | ใหญ่ที่สุด | ช้าที่สุด | แม่นยำสูงสุด |
| `fp16` | ใหญ่ | เร็ว (GPU) | แม่นยำดีมาก |
| `int8` | เล็ก | เร็ว | สูญเสียความแม่นยำน้อย |
| `int4` | เล็กที่สุด | เร็วที่สุด | สูญเสียความแม่นยำปานกลาง |

สำหรับการพัฒนาในเครื่องส่วนใหญ่ `int4` บน CPU ให้ความสมดุลที่ดีที่สุดระหว่างความเร็วและการใช้ทรัพยากร สำหรับผลลัพธ์คุณภาพการผลิต แนะนำใช้ `fp16` บน CUDA GPU

---

### แบบฝึกหัด 4: สร้างแม่แบบแชทสำหรับกำหนดค่า

ตัวสร้างโมเดลจะสร้างไฟล์ `chat_template.jinja` และ `genai_config.json` ในโฟลเดอร์ผลลัพธ์โดยอัตโนมัติ แต่ Foundry Local ยังต้องการไฟล์ `inference_model.json` เพื่อเข้าใจวิธีจัดรูปแบบคำสั่งสำหรับโมเดลนี้ ไฟล์นี้ระบุชื่อโมเดลและแม่แบบคำสั่งที่ครอบข้อความผู้ใช้ด้วยโทเค็นพิเศษที่ถูกต้อง

#### ขั้นตอนที่ 1: ตรวจสอบผลลัพธ์ที่คอมไพล์แล้ว

แสดงเนื้อหาในโฟลเดอร์โมเดลที่คอมไพล์:

```bash
ls models/qwen3
```

คุณควรเห็นไฟล์ เช่น:  
- `model.onnx` และ `model.onnx.data` — เวทน้ำหนักโมเดลที่คอมไพล์แล้ว  
- `genai_config.json` — การกำหนดค่า ONNX Runtime GenAI (สร้างอัตโนมัติ)  
- `chat_template.jinja` — แม่แบบแชทของโมเดล (สร้างอัตโนมัติ)  
- `tokenizer.json`, `tokenizer_config.json` — ไฟล์ตัวแยกโทเค็น  
- ไฟล์การกำหนดค่าและคำศัพท์อื่นๆ

#### ขั้นตอนที่ 2: สร้างไฟล์ inference_model.json

ไฟล์ `inference_model.json` บอก Foundry Local ว่าจะจัดรูปแบบคำสั่งอย่างไร สร้างสคริปต์ Python ชื่อ `generate_chat_template.py` **ที่โฟลเดอร์รากของคลังข้อมูล** (ไดเรกทอรีเดียวกับที่มีโฟลเดอร์ `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# สร้างการสนทนาขั้นพื้นฐานเพื่อดึงแม่แบบแชท
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# สร้างโครงสร้าง inference_model.json
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

รันสคริปต์จากโฟลเดอร์รากของคลังข้อมูล:

```bash
python generate_chat_template.py
```

> **หมายเหตุ:** แพ็กเกจ `transformers` ได้ถูกติดตั้งแล้วเป็นส่วนหนึ่งของ `onnxruntime-genai` หากพบ `ImportError` ให้รัน `pip install transformers` ก่อน

สคริปต์จะสร้างไฟล์ `inference_model.json` ภายในโฟลเดอร์ `models/qwen3` ไฟล์นี้บอก Foundry Local วิธีห่อข้อความผู้ใช้ด้วยโทเค็นพิเศษที่ถูกต้องสำหรับ Qwen3

> **สำคัญ:** ฟิลด์ `"Name"` ใน `inference_model.json` (ตั้งค่าเป็น `qwen3-0.6b` ในสคริปต์นี้) คือ **นามแฝงโมเดล** ที่คุณจะใช้ในคำสั่งและเรียก API ต่อไป หากคุณเปลี่ยนชื่อนี้ โปรดอัปเดตชื่อโมเดลในแบบฝึกหัด 6–10 ด้วย

#### ขั้นตอนที่ 3: ตรวจสอบการกำหนดค่า

เปิดไฟล์ `models/qwen3/inference_model.json` และยืนยันว่ามีฟิลด์ `Name` และวัตถุ `PromptTemplate` ที่มีคีย์ `assistant` และ `prompt` แม่แบบคำสั่งควรมีโทเค็นพิเศษเช่น `<|im_start|>` และ `<|im_end|>` (โทเค็นที่แน่นอนขึ้นอยู่กับแม่แบบแชทของโมเดล)

> **ทางเลือกแบบแมนนวล:** หากคุณไม่ต้องการรันสคริปต์ คุณสามารถสร้างไฟล์ด้วยตนเองได้ ขอเพียงให้ฟิลด์ `prompt` มีแม่แบบแชทเต็มรูปแบบของโมเดล โดยมี `{Content}` เป็นที่ว่างสำหรับข้อความของผู้ใช้

---

### แบบฝึกหัด 5: ตรวจสอบโครงสร้างไดเรกทอรีโมเดล
เครื่องมือสร้างโมเดลจะวางไฟล์ที่คอมไพล์ทั้งหมดโดยตรงลงในไดเรกทอรีผลลัพธ์ที่คุณระบุ ตรวจสอบให้แน่ใจว่าโครงสร้างสุดท้ายดูถูกต้อง:

```bash
ls models/qwen3
```

ไดเรกทอรีควรมีไฟล์ดังต่อไปนี้:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **หมายเหตุ:** แตกต่างจากเครื่องมือคอมไพล์บางอย่าง เครื่องมือสร้างโมเดลจะไม่สร้างไดเรกทอรีย่อยแบบซ้อนกัน ไฟล์ทั้งหมดจะอยู่ในโฟลเดอร์ผลลัพธ์โดยตรง ซึ่งเป็นสิ่งที่ Foundry Local คาดหวังอย่างชัดเจน

---

### แบบฝึกหัด 6: เพิ่มโมเดลไปยังแคชของ Foundry Local

บอก Foundry Local ว่าจะหาโมเดลที่คอมไพล์ของคุณได้ที่ไหนโดยการเพิ่มไดเรกทอรีไปยังแคช:

```bash
foundry cache cd models/qwen3
```

ตรวจสอบว่าโมเดลปรากฏในแคชแล้ว:

```bash
foundry cache ls
```

คุณควรเห็นโมเดลที่กำหนดเองของคุณปรากฏในรายการพร้อมกับโมเดลที่เก็บไว้ก่อนหน้านี้ (เช่น `phi-3.5-mini` หรือ `phi-4-mini`)

---

### แบบฝึกหัด 7: รันโมเดลที่กำหนดเองผ่าน CLI

เริ่มเซสชันแชทแบบโต้ตอบกับโมเดลที่คุณเพิ่งคอมไพล์ (นามแฝง `qwen3-0.6b` มาจากฟิลด์ `Name` ที่คุณตั้งค่าใน `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

แฟล็ก `--verbose` จะแสดงข้อมูลวินิจฉัยเพิ่มเติมซึ่งมีประโยชน์เมื่อทดสอบโมเดลที่กำหนดเองเป็นครั้งแรก หากโมเดลโหลดได้สำเร็จ คุณจะเห็นพรอมต์โต้ตอบ ลองส่งข้อความบางข้อความ:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

พิมพ์ `exit` หรือกด `Ctrl+C` เพื่อสิ้นสุดเซสชัน

> **การแก้ปัญหา:** หากโมเดลโหลดไม่สำเร็จ ตรวจสอบสิ่งต่อไปนี้:
> - ไฟล์ `genai_config.json` ถูกสร้างโดยเครื่องมือสร้างโมเดล
> - ไฟล์ `inference_model.json` มีอยู่และเป็น JSON ที่ถูกต้อง
> - ไฟล์โมเดล ONNX อยู่ในไดเรกทอรีที่ถูกต้อง
> - คุณมี RAM เพียงพอ (Qwen3-0.6B int4 ต้องการประมาณ 1 GB)
> - Qwen3 เป็นโมเดลการใช้เหตุผลซึ่งสร้างแท็ก `<think>` หากคุณเห็น `<think>...</think>` นำหน้าคำตอบ นี่เป็นพฤติกรรมปกติ เทมเพลตพรอมต์ใน `inference_model.json` สามารถปรับแต่งเพื่อปิดการแสดงผลการคิดได้

---

### แบบฝึกหัด 8: คิวรีโมเดลที่กำหนดเองผ่าน REST API

หากคุณออกจากเซสชันโต้ตอบในแบบฝึกหัด 7 โมเดลอาจไม่ได้ถูกโหลดอีกต่อไป เริ่มบริการ Foundry Local และโหลดโมเดลก่อน:

```bash
foundry service start
foundry model load qwen3-0.6b
```

ตรวจสอบว่าบริการกำลังทำงานบนพอร์ตใด:

```bash
foundry service status
```

จากนั้นส่งคำขอ (แทนที่ `5273` ด้วยพอร์ตของคุณหากแตกต่าง):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **หมายเหตุสำหรับ Windows:** คำสั่ง `curl` ข้างต้นใช้ไวยากรณ์ bash บน Windows ให้ใช้ cmdlet `Invoke-RestMethod` ของ PowerShell แทน

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### แบบฝึกหัด 9: ใช้โมเดลที่กำหนดเองกับ OpenAI SDK

คุณสามารถเชื่อมต่อกับโมเดลที่กำหนดเองโดยใช้โค้ด OpenAI SDK เดียวกันกับที่ใช้กับโมเดลในตัว (ดู [ส่วนที่ 3](part3-sdk-and-apis.md)) ความแตกต่างมีเพียงชื่อโมเดลเท่านั้น

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local ไม่ตรวจสอบความถูกต้องของคีย์ API
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local ไม่ตรวจสอบความถูกต้องของคีย์ API
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **ประเด็นสำคัญ:** เนื่องจาก Foundry Local เปิด API ที่เข้ากันได้กับ OpenAI โค้ดใดๆ ที่ทำงานกับโมเดลในตัวก็จะทำงานกับโมเดลที่กำหนดเองของคุณได้เช่นกัน คุณเพียงแค่เปลี่ยนพารามิเตอร์ `model`

---

### แบบฝึกหัด 10: ทดสอบโมเดลที่กำหนดเองด้วย Foundry Local SDK

ในห้องปฏิบัติการก่อนหน้านี้ คุณใช้ Foundry Local SDK เพื่อเริ่มบริการ, ค้นหาจุดสิ้นสุด และจัดการโมเดลโดยอัตโนมัติ คุณสามารถทำตามรูปแบบเดียวกันนี้กับโมเดลที่คอมไพล์เอง SDK จะจัดการการเริ่มต้นบริการและการค้นพบจุดสิ้นสุด ดังนั้นโค้ดของคุณไม่จำเป็นต้องระบุ `localhost:5273` โดยตรง

> **หมายเหตุ:** ตรวจสอบให้แน่ใจว่าได้ติดตั้ง Foundry Local SDK ก่อนรันตัวอย่างเหล่านี้:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** เพิ่มแพ็กเกจ NuGet `Microsoft.AI.Foundry.Local` และ `OpenAI`
>
> บันทึกไฟล์สคริปต์แต่ละไฟล์ **ที่โฟลเดอร์รากของรีโพสิตอรี** (โฟลเดอร์เดียวกับ `models/` ของคุณ)

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# ขั้นตอนที่ 1: เริ่มบริการ Foundry Local และโหลดโมเดลที่กำหนดเอง
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# ขั้นตอนที่ 2: ตรวจสอบแคชสำหรับโมเดลที่กำหนดเอง
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# ขั้นตอนที่ 3: โหลดโมเดลเข้าสู่หน่วยความจำ
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# ขั้นตอนที่ 4: สร้างไคลเอนต์ OpenAI โดยใช้จุดสิ้นสุดที่ค้นพบจาก SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# ขั้นตอนที่ 5: ส่งคำขอการสนทนาแบบสตรีมมิ่งเพื่อทำงานให้เสร็จสมบูรณ์
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

รันมัน:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// ขั้นตอนที่ 1: เริ่มบริการ Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ขั้นตอนที่ 2: ดึงโมเดลที่กำหนดเองจากแคตตาล็อก
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// ขั้นตอนที่ 3: โหลดโมเดลเข้าสู่หน่วยความจำ
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// ขั้นตอนที่ 4: สร้างไคลเอนต์ OpenAI โดยใช้ปลายทางที่ SDK ค้นพบ
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// ขั้นตอนที่ 5: ส่งคำขอการสนทนาแบบสตรีมมิ่ง
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

รันมัน:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **ประเด็นสำคัญ:** Foundry Local SDK ค้นหาจุดสิ้นสุดแบบไดนามิก จึงไม่จำเป็นต้องระบุหมายเลขพอร์ตแบบคงที่ นี่คือแนวทางที่แนะนำสำหรับแอปพลิเคชันใช้งานจริง โมเดลที่คอมไพล์เองของคุณทำงานเหมือนกับโมเดลในแค็ตตาล็อกผ่าน SDK

---

## การเลือกโมเดลเพื่อคอมไพล์

Qwen3-0.6B ถูกใช้เป็นตัวอย่างอ้างอิงในห้องปฏิบัติการนี้เพราะมีขนาดเล็ก คอมไพล์เร็ว และถูกลิขสิทธิ์ภายใต้ใบอนุญาต Apache 2.0 อย่างไรก็ตาม คุณสามารถคอมไพล์โมเดลอื่นๆ ได้มากมาย ต่อไปนี้คือคำแนะนำบางส่วน:

| โมเดล | Hugging Face ID | พารามิเตอร์ | ใบอนุญาต | หมายเหตุ |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | ขนาดเล็กมาก คอมไพล์เร็ว เหมาะสำหรับทดสอบ |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | คุณภาพดีกว่า แต่ยังคอมไพล์เร็ว |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | คุณภาพสูง ต้องการ RAM มากขึ้น |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | ต้องยอมรับใบอนุญาตบน Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | คุณภาพสูง ไฟล์ดาวน์โหลดใหญ่และคอมไพล์นาน |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | มีอยู่ในแค็ตตาล็อก Foundry Local แล้ว (เป็นประโยชน์สำหรับการเปรียบเทียบ) |

> **เตือนเรื่องใบอนุญาต:** ตรวจสอบใบอนุญาตของโมเดลบน Hugging Face เสมอก่อนใช้งาน โมเดลบางตัว (เช่น Llama) ต้องให้คุณยอมรับข้อตกลงใบอนุญาตและเข้าสู่ระบบด้วย `huggingface-cli login` ก่อนดาวน์โหลด

---

## แนวคิด: เมื่อใดควรใช้โมเดลที่กำหนดเอง

| สถานการณ์ | เหตุผลในการคอมไพล์เอง |
|------------|-------------------------|
| **โมเดลที่คุณต้องการไม่มีในแค็ตตาล็อก** | แค็ตตาล็อก Foundry Local มีการคัดเลือก ถ้าโมเดลที่คุณต้องการไม่มี ให้คอมไพล์เอง |
| **โมเดลที่ผ่านการปรับแต่ง** | ถ้าคุณได้ปรับแต่งโมเดลด้วยข้อมูลเฉพาะทาง คุณต้องคอมไพล์น้ำหนักโมเดลเอง |
| **ความต้องการการควิกไทเซชันเฉพาะ** | คุณอาจต้องการความแม่นยำหรือวิธีการควิกไทเซชันที่ต่างจากค่าเริ่มต้นในแค็ตตาล็อก |
| **โมเดลเวอร์ชันใหม่ล่าสุด** | เมื่อมีโมเดลใหม่ที่ออกบน Hugging Face อาจยังไม่มีในแค็ตตาล็อก Foundry Local การคอมไพล์เองจะทำให้คุณเข้าถึงได้ทันที |
| **งานวิจัยและทดลอง** | การลองสถาปัตยกรรมโมเดล ขนาด หรือการตั้งค่าต่างๆ บนเครื่องคุณก่อนตัดสินใจใช้จริง |

---

## สรุป

ในห้องปฏิบัติการนี้คุณได้เรียนรู้วิธี:

| ขั้นตอน | สิ่งที่คุณทำ |
|---------|--------------|
| 1 | ติดตั้ง ONNX Runtime GenAI model builder |
| 2 | คอมไพล์ `Qwen/Qwen3-0.6B` จาก Hugging Face เป็นโมเดล ONNX ที่ปรับแต่งแล้ว |
| 3 | สร้างไฟล์กำหนดค่าเทมเพลตแชท `inference_model.json` |
| 4 | เพิ่มโมเดลที่คอมไพล์ไปยังแคช Foundry Local |
| 5 | รันแชทโต้ตอบกับโมเดลที่กำหนดเองผ่าน CLI |
| 6 | คิวรีโมเดลผ่าน REST API ที่เข้ากันได้กับ OpenAI |
| 7 | เชื่อมต่อจาก Python, JavaScript, และ C# โดยใช้ OpenAI SDK |
| 8 | ทดสอบโมเดลที่กำหนดเองครบวงจรกับ Foundry Local SDK |

ข้อสรุปสำคัญคือ **โมเดลประเภท Transformer ใดๆ ก็สามารถรันผ่าน Foundry Local ได้** เมื่อถูกคอมไพล์เป็นแบบ ONNX แล้ว API ที่เข้ากันได้กับ OpenAI หมายความว่าโค้ดแอปพลิเคชันเดิมของคุณยังใช้งานได้โดยไม่ต้องแก้ไข เพียงแค่เปลี่ยนชื่อโมเดลเท่านั้น

---

## ประเด็นสำคัญ

| แนวคิด | รายละเอียด |
|---------|-------------|
| ONNX Runtime GenAI Model Builder | แปลงโมเดล Hugging Face เป็นรูปแบบ ONNX พร้อมการควิกไทเซชันด้วยคำสั่งเดียว |
| รูปแบบ ONNX | Foundry Local ต้องการโมเดล ONNX ที่มีการกำหนดค่า ONNX Runtime GenAI |
| เทมเพลตแชท | ไฟล์ `inference_model.json` แจ้ง Foundry Local ว่าจะจัดรูปแบบพรอมต์อย่างไรสำหรับโมเดลนั้นๆ |
| ฮาร์ดแวร์เป้าหมาย | คอมไพล์สำหรับ CPU, GPU NVIDIA (CUDA), DirectML (GPU Windows), หรือ WebGPU ขึ้นกับฮาร์ดแวร์ของคุณ |
| การควิกไทเซชัน | ความแม่นยำต่ำลง (int4) ลดขนาดและเพิ่มความเร็วแลกกับความถูกต้องบางส่วน; fp16 คงคุณภาพสูงบน GPU |
| ความเข้ากันได้ของ API | โมเดลที่กำหนดเองใช้ API ที่เข้ากันได้กับ OpenAI เหมือนโมเดลในตัว |
| Foundry Local SDK | SDK จัดการการเริ่มบริการ ค้นหาจุดสิ้นสุด และโหลดโมเดลโดยอัตโนมัติสำหรับทั้งโมเดลแค็ตตาล็อกและโมเดลที่กำหนดเอง |

---

## เอกสารเพิ่มเติม

| แหล่งข้อมูล | ลิงก์ |
|--------------|-------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| คู่มือโมเดลที่กำหนดเองของ Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| ครอบครัวโมเดล Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| เอกสาร Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## ขั้นตอนถัดไป

ดำเนินการต่อไปที่ [ส่วนที่ 11: การเรียกเครื่องมือด้วยโมเดลในเครื่อง](part11-tool-calling.md) เพื่อเรียนรู้วิธีเปิดใช้โมเดลในเครื่องของคุณให้เรียกฟังก์ชันภายนอกได้

[← ส่วนที่ 9: การถอดเสียงเสียง Whisper](part9-whisper-voice-transcription.md) | [ส่วนที่ 11: การเรียกเครื่องมือ →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อจำกัดความรับผิดชอบ**:  
เอกสารนี้ถูกแปลโดยใช้บริการแปลภาษาด้วย AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้ว่าเราจะพยายามให้ความถูกต้องสูงสุด แต่โปรดทราบว่าการแปลอัตโนมัติอาจมีข้อผิดพลาดหรือความไม่ถูกต้อง เอกสารต้นฉบับในภาษาต้นทางควรถือเป็นแหล่งข้อมูลที่เชื่อถือได้ สำหรับข้อมูลที่สำคัญ ควรใช้การแปลโดยมนุษย์ผู้เชี่ยวชาญ เราจะไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความผิดใด ๆ ที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->