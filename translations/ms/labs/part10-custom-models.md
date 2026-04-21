![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagian 10: Menggunakan Model Tersuai atau Hugging Face dengan Foundry Local

> **Matlamat:** Memadatkan model Hugging Face ke dalam format ONNX yang dioptimumkan yang diperlukan oleh Foundry Local, konfigurasikannya dengan templat sembang, tambahkannya ke cache tempatan, dan jalankan inferensinya menggunakan CLI, REST API, dan OpenAI SDK.

## Gambaran Keseluruhan

Foundry Local dihantar bersama katalog terpilih model yang sudah dipadatkan, tetapi anda tidak terhad kepada senarai itu sahaja. Mana-mana model bahasa berasaskan transformer yang tersedia di [Hugging Face](https://huggingface.co/) (atau disimpan secara tempatan dalam format PyTorch / Safetensors) boleh dipadatkan ke dalam model ONNX yang dioptimumkan dan dihantar melalui Foundry Local.

Saluran pemadatan menggunakan **ONNX Runtime GenAI Model Builder**, satu alat baris perintah yang disertakan dalam pakej `onnxruntime-genai`. Model builder mengendalikan kerja berat: memuat turun berat sumber, menukarnya ke format ONNX, menerapkan kuantisasi (int4, fp16, bf16), dan mengeluarkan fail konfigurasi (termasuk templat sembang dan tokeniser) yang dijangka oleh Foundry Local.

Dalam makmal ini anda akan memadatkan **Qwen/Qwen3-0.6B** dari Hugging Face, mendaftarkannya dengan Foundry Local, dan berbual dengannya sepenuhnya di peranti anda.

---

## Objektif Pembelajaran

Menjelang akhir makmal ini anda akan dapat:

- Terangkan mengapa pemadatan model tersuai berguna dan bila anda mungkin memerlukannya
- Pasang ONNX Runtime GenAI model builder
- Memadatkan model Hugging Face ke format ONNX yang dioptimumkan dengan satu arahan
- Memahami parameter utama pemadatan (penyedia pelaksanaan, ketepatan)
- Mencipta fail konfigurasi templat sembang `inference_model.json`
- Menambah model yang dipadatkan ke cache Foundry Local
- Menjalankan inferens terhadap model tersuai menggunakan CLI, REST API, dan OpenAI SDK

---

## Prasyarat

| Keperluan | Butiran |
|-------------|---------|
| **Foundry Local CLI** | Dipasang dan pada `PATH` anda ([Bahagian 1](part1-getting-started.md)) |
| **Python 3.10+** | Diperlukan oleh ONNX Runtime GenAI model builder |
| **pip** | Pengurus pakej Python |
| **Ruang cakera** | Sekurang-kurangnya 5 GB ruang kosong untuk fail model sumber dan yang dipadatkan |
| **Akaun Hugging Face** | Beberapa model memerlukan anda menerima lesen sebelum memuat turun. Qwen3-0.6B menggunakan lesen Apache 2.0 dan boleh diakses secara percuma. |

---

## Penyediaan Persekitaran

Pemadatan model memerlukan beberapa pakej Python yang besar (PyTorch, ONNX Runtime GenAI, Transformers). Cipta persekitaran maya khusus supaya ini tidak mengganggu Python sistem anda atau projek lain.

```bash
# Dari akar repositori
python -m venv .venv
```

Aktifkan persekitaran:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Tingkatkan pip untuk mengelakkan isu penyelesaian kebergantungan:

```bash
python -m pip install --upgrade pip
```

> **Petua:** Jika anda sudah mempunyai `.venv` dari makmal sebelumnya, anda boleh menggunakannya semula. Pastikan ia diaktifkan sebelum meneruskan.

---

## Konsep: Saluran Pemadatan

Foundry Local memerlukan model dalam format ONNX dengan konfigurasi ONNX Runtime GenAI. Kebanyakan model sumber terbuka di Hugging Face diedarkan sebagai berat PyTorch atau Safetensors, jadi langkah penukaran diperlukan.

![Saluran pemadatan model tersuai](../../../images/custom-model-pipeline.svg)

### Apa Yang Dilakukan Model Builder?

1. **Muat turun** model sumber dari Hugging Face (atau membacanya dari laluan tempatan).
2. **Menukar** berat PyTorch / Safetensors ke format ONNX.
3. **Mengkuantisasi** model ke ketepatan lebih kecil (contohnya, int4) untuk mengurangkan penggunaan memori dan memperbaiki kelajuan.
4. **Mengeluarkan** konfigurasi ONNX Runtime GenAI (`genai_config.json`), templat sembang (`chat_template.jinja`), dan semua fail tokeniser supaya Foundry Local boleh memuat dan menghantar model.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Anda mungkin menemui rujukan kepada **Microsoft Olive** sebagai alat alternatif untuk pengoptimuman model. Kedua-dua alat boleh menghasilkan model ONNX, tetapi mereka berkhidmat tujuan yang berbeza dan mempunyai pertukaran berbeza:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pakej** | `onnxruntime-genai` | `olive-ai` |
| **Tujuan utama** | Menukar dan mengkuantisasi model AI generatif untuk inferens ONNX Runtime GenAI | Rangka kerja pengoptimuman model hujung ke hujung menyokong pelbagai backend dan sasaran perkakasan |
| **Kemudahan penggunaan** | Arahan tunggal — penukaran + kuantisasi satu langkah | Berdasarkan aliran kerja — saluran berbilang laluan yang boleh dikonfigurasikan dengan YAML/JSON |
| **Format output** | Format ONNX Runtime GenAI (sedia untuk Foundry Local) | ONNX generik, ONNX Runtime GenAI, atau format lain bergantung aliran kerja |
| **Sasaran perkakasan** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, dan lain-lain |
| **Pilihan kuantisasi** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, serta pengoptimuman graf, penalaan lapisan demi lapisan |
| **Skop model** | Model AI generatif (LLM, SLM) | Mana-mana model boleh ditukar ke ONNX (penglihatan, NLP, audio, multimodal) |
| **Terbaik untuk** | Pemadatan model tunggal pantas untuk inferens tempatan | Saluran pengeluaran yang memerlukan kawalan pengoptimuman terperinci |
| **Jejak kebergantungan** | Sederhana (PyTorch, Transformers, ONNX Runtime) | Lebih besar (menambah rangka kerja Olive, tambahan pilihan mengikut aliran kerja) |
| **Integrasi Foundry Local** | Terus — output terus serasi | Memerlukan flag `--use_ort_genai` dan konfigurasi tambahan |

> **Kenapa makmal ini menggunakan Model Builder:** Untuk tugas memadatkan satu model Hugging Face dan mendaftarkannya dengan Foundry Local, Model Builder ialah cara paling mudah dan boleh dipercayai. Ia menghasilkan format output tepat yang dijangka Foundry Local dalam satu arahan. Jika anda kemudian memerlukan ciri pengoptimuman lanjutan — seperti kuantisasi yang peka ketepatan, pembedahan graf, atau penalaan berbilang laluan — Olive adalah pilihan yang kuat untuk diterokai. Lihat [dokumentasi Microsoft Olive](https://microsoft.github.io/Olive/) untuk butiran lanjut.

---

## Latihan Makmal

### Latihan 1: Pasang ONNX Runtime GenAI Model Builder

Pasang pakej ONNX Runtime GenAI, yang merangkumi alat model builder:

```bash
pip install onnxruntime-genai
```

Sahkan pemasangan dengan memeriksa bahawa model builder tersedia:

```bash
python -m onnxruntime_genai.models.builder --help
```

Anda harus melihat keluaran bantuan yang menyenaraikan parameter seperti `-m` (nama model), `-o` (laluan output), `-p` (ketepatan), dan `-e` (penyedia pelaksanaan).

> **Nota:** Model builder bergantung kepada PyTorch, Transformers, dan beberapa pakej lain. Pemasangan mungkin mengambil masa beberapa minit.

---

### Latihan 2: Padatkan Qwen3-0.6B untuk CPU

Jalankan arahan berikut untuk memuat turun model Qwen3-0.6B dari Hugging Face dan memadatkannya untuk inferens CPU dengan kuantisasi int4:

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

#### Fungsi Setiap Parameter

| Parameter | Tujuan | Nilai Digunakan |
|-----------|---------|------------|
| `-m` | ID model Hugging Face atau laluan direktori tempatan | `Qwen/Qwen3-0.6B` |
| `-o` | Direktori tempat model ONNX yang dipadatkan disimpan | `models/qwen3` |
| `-p` | Ketepatan kuantisasi yang digunakan semasa pemadatan | `int4` |
| `-e` | Penyedia pelaksanaan ONNX Runtime (perkakasan sasaran) | `cpu` |
| `--extra_options hf_token=false` | Melangkau pengesahan Hugging Face (sesuai untuk model awam) | `hf_token=false` |

> **Berapa lama masa diambil?** Masa pemadatan bergantung kepada perkakasan anda dan saiz model. Untuk Qwen3-0.6B dengan kuantisasi int4 pada CPU moden, jangkaan sekitar 5 hingga 15 minit. Model lebih besar mengambil masa yang sebanding lebih lama.

Setelah arahan selesai, anda harus melihat direktori `models/qwen3` yang mengandungi fail model yang dipadatkan. Sahkan outputnya:

```bash
ls models/qwen3
```

Anda akan melihat fail termasuk:
- `model.onnx` dan `model.onnx.data` — berat model yang dipadatkan
- `genai_config.json` — konfigurasi ONNX Runtime GenAI
- `chat_template.jinja` — templat sembang model (dihasilkan automatik)
- `tokenizer.json`, `tokenizer_config.json` — fail tokeniser
- Fail kosa kata dan konfigurasi lain

---

### Latihan 3: Padatkan untuk GPU (Pilihan)

Jika anda mempunyai GPU NVIDIA dengan sokongan CUDA, anda boleh memadatkan varian dioptimumkan GPU untuk inferens lebih cepat:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Nota:** Pemadatan GPU memerlukan `onnxruntime-gpu` dan pemasangan CUDA yang berfungsi. Jika ini tidak ada, model builder akan melaporkan ralat. Anda boleh langkau latihan ini dan teruskan dengan varian CPU.

#### Rujukan Pemadatan Spesifik Perkakasan

| Sasaran | Penyedia Pelaksanaan (`-e`) | Ketepatan Disyorkan (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| GPU NVIDIA | `cuda` | `fp16` atau `int4` |
| DirectML (GPU Windows) | `dml` | `fp16` atau `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Pertukaran Ketepatan

| Ketepatan | Saiz | Kelajuan | Kualiti |
|-----------|------|-------|---------|
| `fp32` | Paling besar | Paling perlahan | Ketepatan tertinggi |
| `fp16` | Besar | Cepat (GPU) | Ketepatan sangat baik |
| `int8` | Kecil | Cepat | Kehilangan ketepatan sedikit |
| `int4` | Paling kecil | Paling cepat | Kehilangan ketepatan sederhana |

Untuk kebanyakan pembangunan tempatan, `int4` pada CPU menyediakan keseimbangan terbaik antara kelajuan dan penggunaan sumber. Untuk output berkualiti pengeluaran, `fp16` pada GPU CUDA disyorkan.

---

### Latihan 4: Cipta Konfigurasi Templat Sembang

Model builder secara automatik menghasilkan fail `chat_template.jinja` dan fail `genai_config.json` dalam direktori output. Namun, Foundry Local juga memerlukan fail `inference_model.json` untuk memahami cara memformat arahan untuk model anda. Fail ini menentukan nama model dan templat arahan yang membungkus mesej pengguna dalam token khas yang betul.

#### Langkah 1: Periksa Output yang Dipadatkan

Senaraikan kandungan direktori model yang dipadatkan:

```bash
ls models/qwen3
```

Anda harus melihat fail seperti:
- `model.onnx` dan `model.onnx.data` — berat model yang dipadatkan
- `genai_config.json` — konfigurasi ONNX Runtime GenAI (dihasilkan automatik)
- `chat_template.jinja` — templat sembang model (dihasilkan automatik)
- `tokenizer.json`, `tokenizer_config.json` — fail tokeniser
- Pelbagai fail konfigurasi dan kosa kata lain

#### Langkah 2: Jana Fail inference_model.json

Fail `inference_model.json` memberitahu Foundry Local bagaimana memformat arahan. Cipta skrip Python bernama `generate_chat_template.py` **di akar repositori** (direktori yang sama dengan folder `models/` anda):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Bina perbualan minimum untuk mengekstrak templat sembang
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

# Bina struktur inference_model.json
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

Jalankan skrip dari akar repositori:

```bash
python generate_chat_template.py
```

> **Nota:** Pakej `transformers` sudah dipasang sebagai kebergantungan `onnxruntime-genai`. Jika anda menerima `ImportError`, jalankan `pip install transformers` dahulu.

Skrip menghasilkan fail `inference_model.json` di dalam direktori `models/qwen3`. Fail ini memberitahu Foundry Local bagaimana membungkus input pengguna dalam token khas yang betul untuk Qwen3.

> **Penting:** Medan `"Name"` dalam `inference_model.json` (ditetapkan kepada `qwen3-0.6b` dalam skrip ini) ialah **alias model** yang akan anda gunakan dalam semua arahan dan panggilan API berikutnya. Jika anda menukar nama ini, kemas kini nama model dalam Latihan 6–10 dengan sewajarnya.

#### Langkah 3: Sahkan Konfigurasi

Buka `models/qwen3/inference_model.json` dan sahkan ia mengandungi medan `Name` dan objek `PromptTemplate` dengan kunci `assistant` dan `prompt`. Templat arahan harus termasuk token khas seperti `<|im_start|>` dan `<|im_end|>` (token tepat bergantung pada templat sembang model).

> **Alternatif manual:** Jika anda lebih suka tidak menjalankan skrip, anda boleh mencipta fail ini secara manual. Keperluan utama adalah medan `prompt` mengandungi templat sembang penuh model dengan `{Content}` sebagai tempat letak untuk mesej pengguna.

---

### Latihan 5: Sahkan Struktur Direktori Model
Pembina model meletakkan semua fail yang telah disusun terus ke dalam direktori output yang anda nyatakan. Sahkan bahawa struktur akhir kelihatan betul:

```bash
ls models/qwen3
```

Direktori harus mengandungi fail-fail berikut:

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

> **Nota:** Berbeza dengan beberapa alat penyusunan lain, pembina model tidak membuat subdirektori bersarang. Semua fail diletakkan terus di dalam folder output, yang merupakan apa yang diharapkan oleh Foundry Local.

---

### Latihan 6: Tambah Model ke Cache Foundry Local

Beritahu Foundry Local di mana untuk mencari model yang telah anda susun dengan menambahkan direktori ke dalam cachenya:

```bash
foundry cache cd models/qwen3
```

Sahkan bahawa model muncul dalam cache:

```bash
foundry cache ls
```

Anda harus melihat model tersuai anda disenaraikan bersama dengan model-model cache yang sebelumnya (seperti `phi-3.5-mini` atau `phi-4-mini`).

---

### Latihan 7: Jalankan Model Tersuai menggunakan CLI

Mulakan sesi chat interaktif dengan model yang baru anda susun (alias `qwen3-0.6b` datang dari medan `Name` yang anda tetapkan dalam `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Bendera `--verbose` menunjukkan maklumat diagnostik tambahan, yang berguna ketika menguji model tersuai buat pertama kali. Jika model berjaya dimuatkan anda akan melihat prompt interaktif. Cuba beberapa mesej:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Taip `exit` atau tekan `Ctrl+C` untuk mengakhiri sesi.

> **Penyelesaian masalah:** Jika model gagal dimuatkan, periksa perkara berikut:
> - Fail `genai_config.json` dijana oleh pembina model.
> - Fail `inference_model.json` wujud dan adalah JSON yang sah.
> - Fail model ONNX berada dalam direktori yang betul.
> - Anda mempunyai RAM yang mencukupi (Qwen3-0.6B int4 memerlukan kira-kira 1 GB).
> - Qwen3 adalah model penalaran yang menghasilkan tag `<think>`. Jika anda melihat `<think>...</think>` yang diprefix pada respons, ini adalah kelakuan normal. Template prompt dalam `inference_model.json` boleh disesuaikan untuk menghapus output pemikiran.

---

### Latihan 8: Pertanyaan Model Tersuai melalui REST API

Jika anda keluar dari sesi interaktif dalam Latihan 7, model mungkin tidak lagi dimuatkan. Mulakan perkhidmatan Foundry Local dan muat model terlebih dahulu:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Periksa port yang perkhidmatan sedang berjalan:

```bash
foundry service status
```

Kemudian hantar permintaan (gantikan `5273` dengan port sebenar anda jika berbeza):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Nota Windows:** Perintah `curl` di atas menggunakan sintaks bash. Di Windows, gunakan cmdlet PowerShell `Invoke-RestMethod` di bawah sebagai gantinya.

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

### Latihan 9: Gunakan Model Tersuai dengan OpenAI SDK

Anda boleh menyambung ke model tersuai anda menggunakan kod OpenAI SDK yang sama seperti anda gunakan untuk model terbina dalam (lihat [Bahagian 3](part3-sdk-and-apis.md)). Perbezaan satu-satunya adalah nama model.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local tidak mengesahkan kekunci API
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
  apiKey: "foundry-local", // Foundry Local tidak mengesahkan kekunci API
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

> **Perkara utama:** Kerana Foundry Local mendedahkan API yang serasi dengan OpenAI, mana-mana kod yang berfungsi dengan model terbina dalam juga berfungsi dengan model tersuai anda. Anda hanya perlu menukar parameter `model`.

---

### Latihan 10: Uji Model Tersuai dengan Foundry Local SDK

Dalam makmal sebelum ini anda menggunakan Foundry Local SDK untuk memulakan perkhidmatan, mencari titik akhir, dan mengurus model secara automatik. Anda boleh mengikuti corak yang sama dengan model yang telah disusun tersuai. SDK mengendalikan permulaan perkhidmatan dan penemuan titik akhir, jadi kod anda tidak perlu memasukkan `localhost:5273` secara keras.

> **Nota:** Pastikan Foundry Local SDK dipasang sebelum menjalankan contoh ini:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Tambah pakej NuGet `Microsoft.AI.Foundry.Local` dan `OpenAI`
>
> Simpan setiap fail skrip **di akar repositori** (direktori yang sama dengan folder `models/` anda).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Langkah 1: Mulakan perkhidmatan Foundry Local dan muatkan model tersuai
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Langkah 2: Semak cache untuk model tersuai
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Langkah 3: Muatkan model ke dalam memori
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Langkah 4: Cipta klien OpenAI menggunakan titik akhir yang ditemui oleh SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Langkah 5: Hantar permintaan penyempurnaan sembang penstriman
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

Jalankan ia:

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

// Langkah 1: Mula perkhidmatan Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Langkah 2: Dapatkan model tersuai dari katalog
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Langkah 3: Muatkan model ke dalam memori
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Langkah 4: Cipta klien OpenAI menggunakan titik akhir yang ditemui oleh SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Langkah 5: Hantar permintaan penyempurnaan chat penstriman
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

Jalankan ia:

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

> **Perkara utama:** Foundry Local SDK menemui titik akhir secara dinamik, jadi anda tidak pernah memasukkan nombor port secara keras. Ini adalah pendekatan yang disyorkan untuk aplikasi produksi. Model tersuai anda berfungsi sama seperti model katalog terbina melalui SDK.

---

## Memilih Model untuk Disusun

Qwen3-0.6B digunakan sebagai contoh rujukan dalam makmal ini kerana ia kecil, pantas untuk disusun, dan tersedia secara percuma di bawah lesen Apache 2.0. Walau bagaimanapun, anda boleh menyusun banyak model lain. Berikut adalah beberapa cadangan:

| Model | ID Hugging Face | Parameter | Lesen | Nota |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Sangat kecil, penyusunan cepat, sesuai untuk ujian |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Kualiti lebih baik, masih pantas disusun |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Kualiti kuat, memerlukan lebih RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Memerlukan penerimaan lesen di Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Kualiti tinggi, muat turun lebih besar dan penyusunan lebih lama |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Sudah dalam katalog Foundry Local (berguna untuk perbandingan) |

> **Peringatan lesen:** Sentiasa periksa lesen model di Hugging Face sebelum menggunakannya. Sesetengah model (seperti Llama) memerlukan anda menerima perjanjian lesen dan mengesahkan dengan `huggingface-cli login` sebelum memuat turun.

---

## Konsep: Bila Menggunakan Model Tersuai

| Senario | Kenapa Susun Sendiri? |
|----------|----------------------|
| **Model yang anda perlukan tidak ada dalam katalog** | Katalog Foundry Local dikurasi. Jika model yang anda mahukan tiada dalam senarai, susun sendiri. |
| **Model yang disesuaikan** | Jika anda telah menyesuaikan model berdasarkan data domain tertentu, anda perlu menyusun berat tersuai anda sendiri. |
| **Keperluan kuantisasi khusus** | Anda mungkin mahukan strategi ketepatan atau kuantisasi yang berbeza daripada lalai katalog. |
| **Keluaran model terkini** | Apabila model baru dikeluarkan di Hugging Face, ia mungkin belum dalam katalog Foundry Local. Menyusunnya sendiri memberi anda akses segera. |
| **Penyelidikan dan eksperimen** | Mencuba pelbagai seni bina model, saiz, atau konfigurasi secara tempatan sebelum membuat pilihan produksi. |

---

## Ringkasan

Dalam makmal ini anda belajar bagaimana untuk:

| Langkah | Apa yang Anda Lakukan |
|------|-------------|
| 1 | Memasang pembina model ONNX Runtime GenAI |
| 2 | Menyusun `Qwen/Qwen3-0.6B` dari Hugging Face ke dalam model ONNX yang dioptimumkan |
| 3 | Membuat fail konfigurasi template chat `inference_model.json` |
| 4 | Menambah model yang disusun ke cache Foundry Local |
| 5 | Menjalankan chat interaktif dengan model tersuai melalui CLI |
| 6 | Menyoal model melalui REST API yang serasi OpenAI |
| 7 | Menyambung dari Python, JavaScript, dan C# menggunakan OpenAI SDK |
| 8 | Menguji model tersuai secara menyeluruh dengan Foundry Local SDK |

Intisari utama ialah **mana-mana model berasaskan transformer boleh dijalankan melalui Foundry Local** setelah ia disusun ke format ONNX. API yang serasi dengan OpenAI bermakna semua kod aplikasi sedia ada anda berfungsi tanpa sebarang perubahan; anda hanya perlu menukar nama model.

---

## Intipati Utama

| Konsep | Perincian |
|---------|--------|
| Pembina Model ONNX Runtime GenAI | Menukar model Hugging Face ke format ONNX dengan kuantisasi dalam satu perintah |
| Format ONNX | Foundry Local memerlukan model ONNX dengan konfigurasi ONNX Runtime GenAI |
| Templat chat | Fail `inference_model.json` memberitahu Foundry Local bagaimana memformat prompt untuk model tertentu |
| Sasaran perkakasan | Susun untuk CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), atau WebGPU bergantung pada perkakasan anda |
| Kuantisasi | Ketepatan rendah (int4) mengurangkan saiz dan mempercepat dengan sedikit pengorbanan ketepatan; fp16 mengekalkan kualiti tinggi pada GPU |
| Keserasian API | Model tersuai menggunakan API yang sama serasi OpenAI seperti model terbina dalam |
| Foundry Local SDK | SDK mengendalikan permulaan perkhidmatan, penemuan titik akhir, dan pemuatan model secara automatik untuk katalog dan model tersuai |

---

## Bacaan Lanjut

| Sumber | Pautan |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Panduan model tersuai Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Keluarga model Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Dokumentasi Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Langkah Seterusnya

Teruskan ke [Bahagian 11: Pemanggilan Alat dengan Model Tempatan](part11-tool-calling.md) untuk belajar bagaimana membolehkan model tempatan anda memanggil fungsi luaran.

[← Bahagian 9: Transkripsi Suara Whisper](part9-whisper-voice-transcription.md) | [Bahagian 11: Pemanggilan Alat →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil perhatian bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidakakuratan. Dokumen asal dalam bahasa asalnya hendaklah dianggap sebagai sumber yang sahih. Untuk maklumat penting, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab atas sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->