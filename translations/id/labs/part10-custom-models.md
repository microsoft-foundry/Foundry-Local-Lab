![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bagian 10: Menggunakan Model Kustom atau Hugging Face dengan Foundry Local

> **Tujuan:** Mengompilasi model Hugging Face ke dalam format ONNX yang dioptimalkan yang dibutuhkan oleh Foundry Local, mengonfigurasinya dengan template obrolan, menambahkannya ke cache lokal, dan menjalankan inferensi terhadapnya menggunakan CLI, REST API, dan OpenAI SDK.

## Ikhtisar

Foundry Local dilengkapi dengan katalog model yang telah dikompilasi secara kurasi, tetapi Anda tidak terbatas pada daftar tersebut. Model bahasa berbasis transformer apa pun yang tersedia di [Hugging Face](https://huggingface.co/) (atau disimpan secara lokal dalam format PyTorch / Safetensors) dapat dikompilasi menjadi model ONNX yang dioptimalkan dan disajikan melalui Foundry Local.

Pipeline kompilasi menggunakan **ONNX Runtime GenAI Model Builder**, sebuah alat baris perintah yang disertakan dengan paket `onnxruntime-genai`. Pembuat model menangani pekerjaan berat: mengunduh bobot sumber, mengonversinya ke format ONNX, menerapkan kuantisasi (int4, fp16, bf16), dan menghasilkan berkas konfigurasi (termasuk template obrolan dan tokeniser) yang diharapkan Foundry Local.

Dalam lab ini Anda akan mengompilasi **Qwen/Qwen3-0.6B** dari Hugging Face, mendaftarkannya ke Foundry Local, dan mengobrol dengannya sepenuhnya di perangkat Anda.

---

## Tujuan Pembelajaran

Pada akhir lab ini Anda akan dapat:

- Menjelaskan mengapa kompilasi model kustom berguna dan kapan Anda membutuhkannya
- Menginstal ONNX Runtime GenAI model builder
- Mengompilasi model Hugging Face ke format ONNX yang dioptimalkan dengan perintah tunggal
- Memahami parameter kompilasi kunci (execution provider, precision)
- Membuat berkas konfigurasi template obrolan `inference_model.json`
- Menambahkan model yang sudah dikompilasi ke cache Foundry Local
- Menjalankan inferensi terhadap model kustom menggunakan CLI, REST API, dan OpenAI SDK

---

## Prasyarat

| Persyaratan | Detail |
|-------------|---------|
| **Foundry Local CLI** | Terinstal dan ada di `PATH` Anda ([Bagian 1](part1-getting-started.md)) |
| **Python 3.10+** | Dibutuhkan oleh ONNX Runtime GenAI model builder |
| **pip** | Manajer paket Python |
| **Ruang disk** | Minimal 5 GB kosong untuk sumber dan berkas model yang sudah dikompilasi |
| **Akun Hugging Face** | Beberapa model meminta Anda menerima lisensi sebelum mengunduh. Qwen3-0.6B menggunakan lisensi Apache 2.0 dan tersedia secara bebas. |

---

## Penyiapan Lingkungan

Kompilasi model membutuhkan beberapa paket Python besar (PyTorch, ONNX Runtime GenAI, Transformers). Buat lingkungan virtual khusus agar ini tidak mengganggu Python sistem Anda atau proyek lain.

```bash
# Dari akar repositori
python -m venv .venv
```

Aktifkan lingkungan:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Perbarui pip untuk menghindari masalah penyelesaian ketergantungan:

```bash
python -m pip install --upgrade pip
```

> **Tip:** Jika Anda sudah memiliki `.venv` dari lab sebelumnya, Anda dapat menggunakannya kembali. Pastikan saja lingkungan tersebut sudah aktif sebelum melanjutkan.

---

## Konsep: Pipeline Kompilasi

Foundry Local membutuhkan model dalam format ONNX dengan konfigurasi ONNX Runtime GenAI. Sebagian besar model open-source di Hugging Face didistribusikan sebagai bobot PyTorch atau Safetensors, jadi diperlukan langkah konversi.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Apa yang Dilakukan Model Builder?

1. **Mengunduh** model sumber dari Hugging Face (atau membacanya dari path lokal).
2. **Mengonversi** bobot PyTorch / Safetensors ke format ONNX.
3. **Mengkuantisasi** model ke presisi yang lebih kecil (misalnya, int4) untuk mengurangi penggunaan memori dan meningkatkan throughput.
4. **Menghasilkan** konfigurasi ONNX Runtime GenAI (`genai_config.json`), template obrolan (`chat_template.jinja`), dan semua berkas tokeniser agar Foundry Local dapat memuat dan menyajikan model.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Anda mungkin menemukan referensi ke **Microsoft Olive** sebagai alat alternatif untuk optimasi model. Kedua alat dapat menghasilkan model ONNX, tetapi mereka melayani tujuan berbeda dan memiliki trade-off berbeda:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paket** | `onnxruntime-genai` | `olive-ai` |
| **Tujuan utama** | Mengonversi dan mengkuantisasi model AI generatif untuk inferensi ONNX Runtime GenAI | Kerangka kerja optimasi model end-to-end yang mendukung banyak backend dan target perangkat keras |
| **Kemudahan penggunaan** | Perintah tunggal — konversi + kuantisasi satu langkah | Berbasis alur kerja — pipeline multi-langkah yang dapat dikonfigurasi dengan YAML/JSON |
| **Format keluaran** | Format ONNX Runtime GenAI (siap untuk Foundry Local) | ONNX generik, ONNX Runtime GenAI, atau format lain tergantung alur kerja |
| **Target perangkat keras** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, dan lainnya |
| **Opsi kuantisasi** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus optimasi grafik, penyesuaian lapisan demi lapisan |
| **Cakupan model** | Model AI generatif (LLM, SLM) | Model apa pun yang bisa dikonversi ke ONNX (visi, NLP, audio, multimodal) |
| **Terbaik untuk** | Kompilasi model tunggal cepat untuk inferensi lokal | Pipeline produksi yang membutuhkan kontrol optimasi granular |
| **Jejak ketergantungan** | Sedang (PyTorch, Transformers, ONNX Runtime) | Lebih besar (menambahkan framework Olive, ekstra opsional per alur kerja) |
| **Integrasi Foundry Local** | Langsung — keluaran langsung kompatibel | Memerlukan flag `--use_ort_genai` dan konfigurasi tambahan |

> **Mengapa lab ini menggunakan Model Builder:** Untuk tugas mengompilasi satu model Hugging Face dan mendaftarkannya ke Foundry Local, Model Builder adalah jalur paling sederhana dan dapat diandalkan. Alat ini menghasilkan format keluaran yang tepat yang diharapkan Foundry Local dengan satu perintah. Jika Anda membutuhkan fitur optimasi lanjutan nanti — seperti kuantisasi akurasi-aware, pembedahan grafik, atau penalaan multi-langkah — Olive adalah opsi kuat yang bisa dijelajahi. Lihat [dokumentasi Microsoft Olive](https://microsoft.github.io/Olive/) untuk detail lebih lanjut.

---

## Latihan Lab

### Latihan 1: Instal ONNX Runtime GenAI Model Builder

Instal paket ONNX Runtime GenAI, yang mencakup alat pembuat model:

```bash
pip install onnxruntime-genai
```

Verifikasi instalasi dengan memastikan model builder tersedia:

```bash
python -m onnxruntime_genai.models.builder --help
```

Anda harus melihat output bantuan yang mencantumkan parameter seperti `-m` (nama model), `-o` (path keluaran), `-p` (presisi), dan `-e` (execution provider).

> **Catatan:** Model builder bergantung pada PyTorch, Transformers, dan beberapa paket lain. Instalasi mungkin memakan waktu beberapa menit.

---

### Latihan 2: Kompilasi Qwen3-0.6B untuk CPU

Jalankan perintah berikut untuk mengunduh model Qwen3-0.6B dari Hugging Face dan mengompilasinya untuk inferensi CPU dengan kuantisasi int4:

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

| Parameter | Tujuan | Nilai yang Digunakan |
|-----------|---------|----------------------|
| `-m` | ID model Hugging Face atau path direktori lokal | `Qwen/Qwen3-0.6B` |
| `-o` | Direktori tempat model ONNX yang dikompilasi disimpan | `models/qwen3` |
| `-p` | Presisi kuantisasi diterapkan selama kompilasi | `int4` |
| `-e` | Execution provider ONNX Runtime (target perangkat keras) | `cpu` |
| `--extra_options hf_token=false` | Melewati autentikasi Hugging Face (cukup untuk model publik) | `hf_token=false` |

> **Berapa lama waktu yang dibutuhkan?** Waktu kompilasi bergantung pada perangkat keras dan ukuran model Anda. Untuk Qwen3-0.6B dengan kuantisasi int4 pada CPU modern, perkiraan sekitar 5 sampai 15 menit. Model yang lebih besar membutuhkan waktu yang lebih lama secara proporsional.

Setelah perintah selesai, Anda harus melihat direktori `models/qwen3` yang berisi berkas model yang sudah dikompilasi. Verifikasi keluaran:

```bash
ls models/qwen3
```

Anda harus melihat berkas termasuk:
- `model.onnx` dan `model.onnx.data` — bobot model yang dikompilasi
- `genai_config.json` — konfigurasi ONNX Runtime GenAI
- `chat_template.jinja` — template obrolan model (auto-generated)
- `tokenizer.json`, `tokenizer_config.json` — berkas tokeniser
- Berkas kosa kata dan konfigurasi lainnya

---

### Latihan 3: Kompilasi untuk GPU (Opsional)

Jika Anda memiliki GPU NVIDIA dengan dukungan CUDA, Anda dapat mengompilasi varian yang dioptimalkan untuk GPU agar inferensi lebih cepat:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Catatan:** Kompilasi GPU membutuhkan `onnxruntime-gpu` dan instalasi CUDA yang berfungsi. Jika ini tidak tersedia, model builder akan melapor kesalahan. Anda dapat melewati latihan ini dan melanjutkan dengan varian CPU.

#### Referensi Kompilasi Spesifik Perangkat Keras

| Target | Execution Provider (`-e`) | Presisi yang Disarankan (`-p`) |
|--------|---------------------------|-------------------------------|
| CPU | `cpu` | `int4` |
| GPU NVIDIA | `cuda` | `fp16` atau `int4` |
| DirectML (GPU Windows) | `dml` | `fp16` atau `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Trade-off Presisi

| Presisi | Ukuran | Kecepatan | Kualitas |
|---------|--------|-----------|----------|
| `fp32` | Terbesar | Terlambat | Akurasi tertinggi |
| `fp16` | Besar | Cepat (GPU) | Akurasi sangat baik |
| `int8` | Kecil | Cepat | Sedikit kehilangan akurasi |
| `int4` | Paling kecil | Tercepat | Kehilangan akurasi sedang |

Untuk sebagian besar pengembangan lokal, `int4` pada CPU memberikan keseimbangan terbaik antara kecepatan dan penggunaan sumber daya. Untuk keluaran kualitas produksi, `fp16` pada GPU CUDA direkomendasikan.

---

### Latihan 4: Buat Konfigurasi Template Obrolan

Model builder secara otomatis menghasilkan berkas `chat_template.jinja` dan berkas `genai_config.json` di direktori keluaran. Namun, Foundry Local juga membutuhkan berkas `inference_model.json` agar memahami cara memformat prompt untuk model Anda. Berkas ini mendefinisikan nama model dan template prompt yang membungkus pesan pengguna dengan token khusus yang benar.

#### Langkah 1: Periksa Keluaran yang Sudah Dikompilasi

Daftar isi direktori model yang sudah dikompilasi:

```bash
ls models/qwen3
```

Anda harus melihat berkas seperti:
- `model.onnx` dan `model.onnx.data` — bobot model yang sudah dikompilasi
- `genai_config.json` — konfigurasi ONNX Runtime GenAI (auto-generated)
- `chat_template.jinja` — template obrolan model (auto-generated)
- `tokenizer.json`, `tokenizer_config.json` — berkas tokeniser
- Berkas konfigurasi dan kosa kata lainnya

#### Langkah 2: Hasilkan Berkas inference_model.json

Berkas `inference_model.json` memberi tahu Foundry Local cara memformat prompt. Buat skrip Python bernama `generate_chat_template.py` **di root repositori** (direktori yang sama dengan folder `models/` Anda):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Bangun percakapan minimal untuk mengekstrak template obrolan
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

# Bangun struktur inference_model.json
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

Jalankan skrip dari root repositori:

```bash
python generate_chat_template.py
```

> **Catatan:** Paket `transformers` sudah terpasang sebagai dependensi `onnxruntime-genai`. Jika Anda melihat `ImportError`, jalankan `pip install transformers` terlebih dahulu.

Skrip menghasilkan berkas `inference_model.json` di dalam direktori `models/qwen3`. Berkas ini memberitahu Foundry Local bagaimana membungkus input pengguna dengan token khusus yang benar untuk Qwen3.

> **Penting:** Kolom `"Name"` di `inference_model.json` (disetel ke `qwen3-0.6b` dalam skrip ini) adalah **alias model** yang akan Anda gunakan di semua perintah dan panggilan API berikutnya. Jika Anda mengubah nama ini, perbarui nama model di Latihan 6–10 sesuai.

#### Langkah 3: Verifikasi Konfigurasi

Buka `models/qwen3/inference_model.json` dan pastikan berisi kolom `Name` dan objek `PromptTemplate` dengan kunci `assistant` dan `prompt`. Template prompt harus menyertakan token khusus seperti `<|im_start|>` dan `<|im_end|>` (token pasti tergantung template obrolan model).

> **Alternatif manual:** Jika Anda lebih suka tidak menjalankan skrip, Anda dapat membuat berkas ini secara manual. Namun syarat utamanya adalah kolom `prompt` harus berisi template obrolan lengkap model dengan `{Content}` sebagai placeholder untuk pesan pengguna.

---

### Latihan 5: Verifikasi Struktur Direktori Model
Pembuat model menempatkan semua file yang sudah dikompilasi langsung ke dalam direktori output yang Anda tentukan. Verifikasi bahwa struktur akhir terlihat benar:

```bash
ls models/qwen3
```

Direktori harus berisi file-file berikut:

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

> **Catatan:** Tidak seperti beberapa alat kompilasi lainnya, pembuat model tidak membuat subdirektori bertingkat. Semua file ditempatkan langsung di dalam folder output, yang persis seperti yang diharapkan Foundry Local.

---

### Latihan 6: Tambahkan Model ke Cache Foundry Local

Beritahu Foundry Local di mana menemukan model yang sudah dikompilasi dengan menambahkan direktori ke cache-nya:

```bash
foundry cache cd models/qwen3
```

Verifikasi bahwa model muncul di cache:

```bash
foundry cache ls
```

Anda harus melihat model kustom Anda tercantum bersama model-model yang sebelumnya ada di cache (seperti `phi-3.5-mini` atau `phi-4-mini`).

---

### Latihan 7: Jalankan Model Kustom dengan CLI

Mulai sesi obrolan interaktif dengan model yang baru saja dikompilasi (alias `qwen3-0.6b` berasal dari field `Name` yang Anda tetapkan di `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Flag `--verbose` menampilkan informasi diagnostik tambahan, yang berguna saat menguji model kustom untuk pertama kalinya. Jika model berhasil dimuat, Anda akan melihat prompt interaktif. Coba kirim beberapa pesan:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Ketik `exit` atau tekan `Ctrl+C` untuk mengakhiri sesi.

> **Pemecahan masalah:** Jika model gagal dimuat, periksa hal-hal berikut:
> - File `genai_config.json` telah dibuat oleh pembuat model.
> - File `inference_model.json` ada dan berisi JSON yang valid.
> - File model ONNX berada di direktori yang benar.
> - Anda memiliki RAM yang cukup tersedia (Qwen3-0.6B int4 membutuhkan sekitar 1 GB).
> - Qwen3 adalah model penalaran yang menghasilkan tag `<think>`. Jika Anda melihat `<think>...</think>` di awal respons, itu adalah perilaku normal. Template prompt di `inference_model.json` dapat disesuaikan untuk menonaktifkan output pemikiran.

---

### Latihan 8: Query Model Kustom melalui REST API

Jika Anda keluar dari sesi interaktif pada Latihan 7, model mungkin sudah tidak dimuat lagi. Mulai layanan Foundry Local dan muat model terlebih dahulu:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Periksa port mana yang digunakan layanan:

```bash
foundry service status
```

Kemudian kirim permintaan (ganti `5273` dengan port sebenarnya jika berbeda):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Catatan Windows:** Perintah `curl` di atas menggunakan sintaks bash. Di Windows, gunakan cmdlet PowerShell `Invoke-RestMethod` berikut sebagai gantinya.

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

### Latihan 9: Gunakan Model Kustom dengan OpenAI SDK

Anda dapat menghubungkan ke model kustom Anda menggunakan kode OpenAI SDK yang sama persis seperti yang Anda gunakan untuk model bawaan (lihat [Bagian 3](part3-sdk-and-apis.md)). Perbedaannya hanya pada nama model.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Lokal tidak memvalidasi kunci API
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
  apiKey: "foundry-local", // Foundry Local tidak memvalidasi kunci API
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

> **Poin utama:** Karena Foundry Local menyediakan API yang kompatibel dengan OpenAI, kode apa pun yang berfungsi dengan model bawaan juga berfungsi dengan model kustom Anda. Anda hanya perlu mengganti parameter `model`.

---

### Latihan 10: Uji Model Kustom dengan Foundry Local SDK

Di lab sebelumnya Anda menggunakan Foundry Local SDK untuk memulai layanan, menemukan endpoint, dan mengelola model secara otomatis. Anda dapat mengikuti pola yang sama persis dengan model kustom yang sudah dikompilasi. SDK menangani startup layanan dan penemuan endpoint, jadi kode Anda tidak perlu meng-hardcode `localhost:5273`.

> **Catatan:** Pastikan Foundry Local SDK sudah terpasang sebelum menjalankan contoh ini:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Tambahkan paket NuGet `Microsoft.AI.Foundry.Local` dan `OpenAI`
>
> Simpan setiap file skrip **di root repositori** (direktori yang sama dengan folder `models/` Anda).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Langkah 1: Mulai layanan Foundry Local dan muat model kustom
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Langkah 2: Periksa cache untuk model kustom
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Langkah 3: Muat model ke dalam memori
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Langkah 4: Buat klien OpenAI menggunakan endpoint yang ditemukan oleh SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Langkah 5: Kirim permintaan penyelesaian obrolan streaming
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

Jalankan:

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

// Langkah 1: Mulai layanan Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Langkah 2: Dapatkan model kustom dari katalog
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Langkah 3: Muat model ke dalam memori
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Langkah 4: Buat klien OpenAI menggunakan endpoint yang ditemukan oleh SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Langkah 5: Kirim permintaan penyelesaian chat streaming
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

Jalankan:

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

> **Poin utama:** Foundry Local SDK menemukan endpoint secara dinamis, sehingga Anda tidak pernah meng-hardcode nomor port. Ini adalah pendekatan yang disarankan untuk aplikasi produksi. Model kustom yang sudah dikompilasi bekerja persis seperti model katalog bawaan melalui SDK.

---

## Memilih Model untuk Dikompilasi

Qwen3-0.6B digunakan sebagai contoh referensi di lab ini karena ukurannya kecil, cepat dikompilasi, dan tersedia gratis di bawah lisensi Apache 2.0. Namun, Anda dapat mengkompilasi banyak model lainnya. Berikut beberapa saran:

| Model | Hugging Face ID | Parameter | Lisensi | Catatan |
|-------|-----------------|-----------|---------|---------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Sangat kecil, kompilasi cepat, bagus untuk pengujian |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Kualitas lebih baik, masih cepat dikompilasi |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Kualitas kuat, membutuhkan RAM lebih banyak |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Memerlukan penerimaan lisensi di Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Kualitas tinggi, unduhan lebih besar dan kompilasi lebih lama |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Sudah ada di katalog Foundry Local (berguna untuk perbandingan) |

> **Pengingat lisensi:** Selalu periksa lisensi model di Hugging Face sebelum menggunakannya. Beberapa model (seperti Llama) mengharuskan Anda menerima perjanjian lisensi dan melakukan autentikasi dengan `huggingface-cli login` sebelum mengunduh.

---

## Konsep: Kapan Menggunakan Model Kustom

| Skenario | Mengapa Mengompilasi Sendiri? |
|----------|-------------------------------|
| **Model yang Anda butuhkan tidak ada di katalog** | Katalog Foundry Local dikurasi. Jika model yang Anda inginkan tidak ada, kompilasi sendiri. |
| **Model fine-tuned** | Jika Anda telah melakukan fine-tuning model dengan data khusus domain, Anda harus mengompilasi bobot sendiri. |
| **Kebutuhan kuantisasi spesifik** | Anda mungkin ingin strategi presisi atau kuantisasi yang berbeda dari default katalog. |
| **Rilis model terbaru** | Ketika model baru dirilis di Hugging Face, belum tentu sudah ada di katalog Foundry Local. Mengompilasinya sendiri memberi Anda akses segera. |
| **Riset dan eksperimen** | Mencoba arsitektur, ukuran, atau konfigurasi model berbeda secara lokal sebelum memilih untuk produksi. |

---

## Ringkasan

Di lab ini Anda belajar untuk:

| Langkah | Apa yang Anda Lakukan |
|---------|----------------------|
| 1 | Menginstal pembuat model ONNX Runtime GenAI |
| 2 | Mengompilasi `Qwen/Qwen3-0.6B` dari Hugging Face menjadi model ONNX yang dioptimalkan |
| 3 | Membuat file konfigurasi template chat `inference_model.json` |
| 4 | Menambahkan model yang dikompilasi ke cache Foundry Local |
| 5 | Menjalankan chat interaktif dengan model kustom lewat CLI |
| 6 | Melakukan query ke model lewat REST API yang kompatibel OpenAI |
| 7 | Menghubungkan dari Python, JavaScript, dan C# menggunakan OpenAI SDK |
| 8 | Menguji model kustom secara menyeluruh dengan Foundry Local SDK |

Poin utama adalah bahwa **model berbasis transformer apa pun dapat dijalankan melalui Foundry Local** setelah dikompilasi ke format ONNX. API yang kompatibel dengan OpenAI berarti seluruh kode aplikasi Anda yang sudah ada dapat bekerja tanpa perubahan; Anda hanya perlu mengganti nama model.

---

## Poin Penting

| Konsep | Detail |
|--------|--------|
| Pembuat Model ONNX Runtime GenAI | Mengonversi model Hugging Face ke format ONNX dengan kuantisasi dalam satu perintah |
| Format ONNX | Foundry Local membutuhkan model ONNX dengan konfigurasi ONNX Runtime GenAI |
| Template chat | File `inference_model.json` memberi tahu Foundry Local cara memformat prompt untuk model tertentu |
| Target hardware | Kompilasi untuk CPU, GPU NVIDIA (CUDA), DirectML (GPU Windows), atau WebGPU sesuai perangkat keras Anda |
| Kuantisasi | Presisi lebih rendah (int4) mengurangi ukuran dan meningkatkan kecepatan dengan sedikit kehilangan akurasi; fp16 mempertahankan kualitas tinggi di GPU |
| Kompatibilitas API | Model kustom menggunakan API kompatibel OpenAI yang sama seperti model bawaan |
| Foundry Local SDK | SDK menangani startup layanan, penemuan endpoint, dan pemuatan model secara otomatis untuk model katalog dan kustom |

---

## Bacaan Lebih Lanjut

| Sumber | Tautan |
|--------|--------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Panduan model kustom Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Keluarga model Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Dokumentasi Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Langkah Selanjutnya

Lanjutkan ke [Bagian 11: Pemanggilan Alat dengan Model Lokal](part11-tool-calling.md) untuk mempelajari cara mengaktifkan model lokal Anda memanggil fungsi eksternal.

[← Bagian 9: Transkripsi Suara Whisper](part9-whisper-voice-transcription.md) | [Bagian 11: Pemanggilan Alat →](part11-tool-calling.md)