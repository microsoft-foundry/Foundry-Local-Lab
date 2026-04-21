![Foundry Lokal](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bagian 9: Transkripsi Suara dengan Whisper dan Foundry Lokal

> **Tujuan:** Menggunakan model OpenAI Whisper yang berjalan secara lokal melalui Foundry Lokal untuk mentranskripsikan file audio - sepenuhnya di perangkat, tanpa memerlukan cloud.

## Gambaran Umum

Foundry Lokal tidak hanya untuk generasi teks; ia juga mendukung model **speech-to-text**. Dalam lab ini Anda akan menggunakan model **OpenAI Whisper Medium** untuk mentranskripsikan file audio sepenuhnya di mesin Anda. Ini ideal untuk situasi seperti mentranskripsikan panggilan layanan pelanggan Zava, rekaman ulasan produk, atau sesi perencanaan workshop di mana data audio tidak boleh pernah meninggalkan perangkat Anda.


---

## Tujuan Pembelajaran

Pada akhir lab ini Anda akan mampu:

- Memahami model speech-to-text Whisper dan kemampuannya
- Mengunduh dan menjalankan model Whisper menggunakan Foundry Lokal
- Mentranskripsikan file audio menggunakan Foundry Lokal SDK dalam Python, JavaScript, dan C#
- Membangun layanan transkripsi sederhana yang berjalan sepenuhnya di perangkat
- Memahami perbedaan antara model chat/teks dan model audio di Foundry Lokal

---

## Prasyarat

| Kebutuhan | Detail |
|-------------|---------|
| **Foundry Lokal CLI** | Versi **0.8.101 atau lebih tinggi** (model Whisper tersedia mulai v0.8.101) |
| **OS** | Windows 10/11 (x64 atau ARM64) |
| **Runtime bahasa** | **Python 3.9+** dan/atau **Node.js 18+** dan/atau **.NET 9 SDK** ([Unduh .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Selesai** | [Bagian 1: Memulai](part1-getting-started.md), [Bagian 2: Pendalaman Foundry Lokal SDK](part2-foundry-local-sdk.md), dan [Bagian 3: SDK dan API](part3-sdk-and-apis.md) |

> **Catatan:** Model Whisper harus diunduh melalui **SDK** (bukan CLI). CLI tidak mendukung endpoint transkripsi audio. Periksa versi Anda dengan:
> ```bash
> foundry --version
> ```

---

## Konsep: Cara Whisper Bekerja dengan Foundry Lokal

Model OpenAI Whisper adalah model pengenalan suara umum yang dilatih pada dataset besar beragam audio. Saat dijalankan melalui Foundry Lokal:

- Model berjalan **sepenuhnya di CPU Anda** - tidak diperlukan GPU
- Audio tidak pernah keluar dari perangkat Anda - **privasi lengkap**
- Foundry Lokal SDK menangani pengunduhan model dan manajemen cache
- **JavaScript dan C#** menyediakan `AudioClient` bawaan di SDK yang menangani seluruh pipeline transkripsi — tidak perlu pengaturan ONNX manual
- **Python** menggunakan SDK untuk pengelolaan model dan ONNX Runtime untuk inferensi langsung terhadap model encoder/decoder ONNX

### Cara Pipeline Bekerja (JavaScript dan C#) — SDK AudioClient

1. **Foundry Lokal SDK** mengunduh dan menyimpan model Whisper di cache
2. `model.createAudioClient()` (JS) atau `model.GetAudioClientAsync()` (C#) membuat `AudioClient`
3. `audioClient.transcribe(path)` (JS) atau `audioClient.TranscribeAudioAsync(path)` (C#) menangani seluruh pipeline secara internal — preprocessing audio, encoder, decoder, dan decoding token
4. `AudioClient` menyediakan properti `settings.language` (set ke `"en"` untuk bahasa Inggris) untuk panduan transkripsi yang akurat

### Cara Pipeline Bekerja (Python) — ONNX Runtime

1. **Foundry Lokal SDK** mengunduh dan menyimpan berkas model Whisper ONNX di cache
2. **Preprocessing audio** mengubah audio WAV menjadi spektrogram mel (80 bin mel x 3000 frame)
3. **Encoder** memproses spektrogram mel dan menghasilkan hidden state serta tensor key/value cross-attention
4. **Decoder** berjalan autoregresif, menghasilkan satu token per waktu hingga menghasilkan token akhir teks
5. **Tokeniser** mendekode ID token output kembali ke teks yang dapat dibaca

### Varian Model Whisper

| Alias | Model ID | Perangkat | Ukuran | Deskripsi |
|-------|----------|-----------|--------|-----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-accelerated (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Optimasi CPU (disarankan untuk kebanyakan perangkat) |

> **Catatan:** Berbeda dengan model chat yang muncul sebagai default, model Whisper dikategorikan di bawah tugas `automatic-speech-recognition`. Gunakan `foundry model info whisper-medium` untuk melihat detailnya.

---

## Latihan Lab

### Latihan 0 - Dapatkan File Audio Contoh

Lab ini menyertakan file WAV yang dibangun berdasarkan skenario produk Zava DIY. Buatlah dengan skrip yang disertakan:

```bash
# Dari root repo - buat dan aktifkan .venv terlebih dahulu
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Ini membuat enam file WAV di `samples/audio/`:

| File | Skenario |
|------|----------|
| `zava-customer-inquiry.wav` | Pelanggan menanyakan tentang **Bor Tanpa Kabel Zava ProGrip** |
| `zava-product-review.wav` | Pelanggan mereview **Cat Interior Zava UltraSmooth** |
| `zava-support-call.wav` | Panggilan dukungan terkait **Lemari Perkakas Zava TitanLock** |
| `zava-project-planning.wav` | DIYer merencanakan dek dengan **Dek Komposit Zava EcoBoard** |
| `zava-workshop-setup.wav` | Tur workshop menggunakan **semua lima produk Zava** |
| `zava-full-project-walkthrough.wav` | Tur renovasi garasi mempergunakan **semua produk Zava** (~4 menit, untuk pengujian audio panjang) |

> **Tip:** Anda juga bisa menggunakan file WAV/MP3/M4A sendiri, atau merekam dengan Windows Voice Recorder.

---

### Latihan 1 - Unduh Model Whisper Menggunakan SDK

Karena ketidakcocokan CLI dengan model Whisper di versi Foundry Lokal terbaru, gunakan **SDK** untuk mengunduh dan memuat model. Pilih bahasa Anda:

<details>
<summary><b>🐍 Python</b></summary>

**Pasang SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Mulai layanan
manager = FoundryLocalManager()
manager.start_service()

# Periksa info katalog
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Periksa jika sudah di-cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Muat model ke dalam memori
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Simpan sebagai `download_whisper.py` dan jalankan:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Pasang SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Buat manajer dan mulai layanan
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Dapatkan model dari katalog
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// Muat model ke dalam memori
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Simpan sebagai `download-whisper.mjs` dan jalankan:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Pasang SDK:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **Mengapa SDK bukan CLI?** Foundry Lokal CLI tidak mendukung pengunduhan atau penyajian model Whisper secara langsung. SDK menyediakan cara yang andal untuk mengunduh dan mengelola model audio secara programatis. SDK JavaScript dan C# menyertakan `AudioClient` bawaan yang menangani seluruh pipeline transkripsi. Python menggunakan ONNX Runtime untuk inferensi langsung terhadap berkas model yang di-cache.

---

### Latihan 2 - Memahami SDK Whisper

Transkripsi Whisper menggunakan pendekatan berbeda bergantung bahasa. **JavaScript dan C#** menyediakan `AudioClient` bawaan dalam Foundry Lokal SDK yang menangani seluruh pipeline (preprocessing audio, encoder, decoder, decoding token) dalam satu panggilan metode. **Python** menggunakan Foundry Lokal SDK untuk pengelolaan model dan ONNX Runtime untuk inferensi langsung terhadap model encoder/decoder ONNX.

| Komponen | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **Paket SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Pengelolaan model** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Ekstraksi fitur** | `WhisperFeatureExtractor` + `librosa` | Ditangani oleh SDK `AudioClient` | Ditangani oleh SDK `AudioClient` |
| **Inferensi** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Dekoding token** | `WhisperTokenizer` | Ditangani oleh SDK `AudioClient` | Ditangani oleh SDK `AudioClient` |
| **Pengaturan bahasa** | Atur via `forced_ids` di token decoder | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | Path file WAV | Path file WAV | Path file WAV |
| **Output** | String teks terdekripsi | `result.text` | `result.Text` |

> **Penting:** Selalu atur bahasa di `AudioClient` (misal `"en"` untuk bahasa Inggris). Tanpa pengaturan bahasa eksplisit, model mungkin menghasilkan output kacau saat mencoba mendeteksi bahasa secara otomatis.

> **Pola SDK:** Python menggunakan `FoundryLocalManager(alias)` untuk bootstrap, lalu `get_cache_location()` untuk menemukan berkas model ONNX. JavaScript dan C# menggunakan `AudioClient` bawaan SDK — didapat lewat `model.createAudioClient()` (JS) atau `model.GetAudioClientAsync()` (C#) — yang menangani seluruh pipeline transkripsi. Lihat [Bagian 2: Pendalaman Foundry Lokal SDK](part2-foundry-local-sdk.md) untuk detail lengkap.

---

### Latihan 3 - Bangun Aplikasi Transkripsi Sederhana

Pilih bahasa dan bangun aplikasi minimal yang mentranskripsikan file audio.

> **Format audio yang didukung:** WAV, MP3, M4A. Untuk hasil terbaik, gunakan file WAV dengan sample rate 16kHz.

<details>
<summary><h3>Track Python</h3></summary>

#### Setup

```bash
cd python
python -m venv venv

# Aktifkan lingkungan virtual:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Kode Transkripsi

Buat file `foundry-local-whisper.py`:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# Langkah 1: Bootstrap - memulai layanan, mengunduh, dan memuat model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Bangun jalur ke file model ONNX yang di-cache
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Langkah 2: Muat sesi ONNX dan ekstraktor fitur
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# Langkah 3: Ekstrak fitur mel spektrogram
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Langkah 4: Jalankan encoder
enc_out = encoder.run(None, {"audio_features": input_features})
# Output pertama adalah status tersembunyi; sisanya adalah pasangan KV cross-attention
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Langkah 5: Dekode autoregresif
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcribe, notimestamps
input_ids = np.array([initial_tokens], dtype=np.int32)

# Cache KV self-attention kosong
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # akhir teks
        break
    generated.append(next_token)

    # Perbarui cache KV self-attention
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Jalankan

```bash
# Transkripsikan sebuah skenario produk Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Atau coba lainnya:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Poin Penting Python

| Metode | Tujuan |
|--------|---------|
| `FoundryLocalManager(alias)` | Bootstrap: mulai layanan, unduh, dan muat model |
| `manager.get_cache_location()` | Mendapatkan path ke berkas model ONNX yang di-cache |
| `WhisperFeatureExtractor.from_pretrained()` | Memuat ekstraktor fitur spektrogram mel |
| `ort.InferenceSession()` | Membuat sesi ONNX Runtime untuk encoder dan decoder |
| `tokenizer.decode()` | Mengonversi ID token output kembali ke teks |

</details>

<details>
<summary><h3>Track JavaScript</h3></summary>

#### Setup

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Kode Transkripsi

Buat file `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Langkah 1: Bootstrap - buat manajer, mulai layanan, dan muat model
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// Langkah 2: Buat klien audio dan transkripsi
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Pembersihan
await model.unload();
```

> **Catatan:** Foundry Lokal SDK menyediakan `AudioClient` bawaan melalui `model.createAudioClient()` yang menangani seluruh pipeline inferensi ONNX secara internal — tidak perlu mengimpor `onnxruntime-node`. Selalu atur `audioClient.settings.language = "en"` untuk memastikan transkripsi bahasa Inggris yang akurat.

#### Jalankan

```bash
# Transkripsikan skenario produk Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Atau coba yang lain:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Poin Penting JavaScript

| Metode | Tujuan |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Membuat instance manager singleton |
| `await catalog.getModel(alias)` | Mendapatkan model dari katalog |
| `model.download()` / `model.load()` | Mengunduh dan memuat model Whisper |
| `model.createAudioClient()` | Membuat audio client untuk transkripsi |
| `audioClient.settings.language = "en"` | Mengatur bahasa transkripsi (wajib untuk output akurat) |
| `audioClient.transcribe(path)` | Mentranskripsikan file audio, mengembalikan `{ text, duration }` |

</details>

<details>
<summary><h3>Track C#</h3></summary>

#### Setup

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Catatan:** Track C# menggunakan paket `Microsoft.AI.Foundry.Local` yang menyediakan `AudioClient` bawaan melalui `model.GetAudioClientAsync()`. Ini menangani seluruh pipeline transkripsi secara langsung — tidak perlu setup ONNX Runtime terpisah.

#### Kode Transkripsi

Ganti isi `Program.cs`:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### Jalankan

```bash
# Transkripsikan skenario produk Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Atau coba lainnya:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Poin Penting C#

| Metode | Tujuan |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Menginisialisasi Foundry Lokal dengan konfigurasi |
| `catalog.GetModelAsync(alias)` | Mendapatkan model dari katalog |
| `model.DownloadAsync()` | Mengunduh model Whisper |
| `model.GetAudioClientAsync()` | Mendapatkan AudioClient (bukan ChatClient!) |
| `audioClient.Settings.Language = "en"` | Mengatur bahasa transkripsi (wajib untuk output akurat) |
| `audioClient.TranscribeAudioAsync(path)` | Mentranskripsikan file audio |
| `result.Text` | Teks hasil transkripsi |
> **C# vs Python/JS:** SDK C# menyediakan `AudioClient` bawaan untuk transkripsi dalam proses melalui `model.GetAudioClientAsync()`, mirip dengan SDK JavaScript. Python menggunakan ONNX Runtime langsung untuk inferensi terhadap model encoder/decoder yang di-cache.

</details>

---

### Latihan 4 - Transkripsi Batch Semua Sampel Zava

Sekarang setelah Anda memiliki aplikasi transkripsi yang berfungsi, transkripsikan semua lima file sampel Zava dan bandingkan hasilnya.

<details>
<summary><h3>Track Python</h3></summary>

Contoh lengkap `python/foundry-local-whisper.py` sudah mendukung transkripsi batch. Saat dijalankan tanpa argumen, ia mentranskripsi semua file `zava-*.wav` di `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Contoh ini menggunakan `FoundryLocalManager(alias)` untuk bootstrap, lalu menjalankan sesi encoder dan decoder ONNX untuk setiap file.

</details>

<details>
<summary><h3>Track JavaScript</h3></summary>

Contoh lengkap `javascript/foundry-local-whisper.mjs` sudah mendukung transkripsi batch. Saat dijalankan tanpa argumen, ia mentranskripsi semua file `zava-*.wav` di `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Contoh ini menggunakan `FoundryLocalManager.create()` dan `catalog.getModel(alias)` untuk menginisialisasi SDK, lalu menggunakan `AudioClient` (dengan `settings.language = "en"`) untuk mentranskripsi setiap file.

</details>

<details>
<summary><h3>Track C#</h3></summary>

Contoh lengkap `csharp/WhisperTranscription.cs` sudah mendukung transkripsi batch. Saat dijalankan tanpa argumen file spesifik, ia mentranskripsi semua file `zava-*.wav` di `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Contoh ini menggunakan `FoundryLocalManager.CreateAsync()` dan `AudioClient` pada SDK (dengan `Settings.Language = "en"`) untuk transkripsi dalam proses.

</details>

**Yang perlu diperhatikan:** Bandingkan output transkripsi dengan teks asli di `samples/audio/generate_samples.py`. Seberapa akurat Whisper menangkap nama produk seperti "Zava ProGrip" dan istilah teknis seperti "brushless motor" atau "composite decking"?

---

### Latihan 5 - Memahami Pola Kode Kunci

Pelajari bagaimana transkripsi Whisper berbeda dari chat completions di ketiga bahasa:

<details>
<summary><b>Python - Perbedaan Kunci dari Chat</b></summary>

```python
# Penyelesaian obrolan (Bagian 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transkripsi audio (Bagian Ini):
# Menggunakan ONNX Runtime secara langsung daripada klien OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... loop decoder autoregresif ...
print(tokenizer.decode(generated_tokens))
```

**Wawasan kunci:** Model chat menggunakan API kompatibel OpenAI melalui `manager.endpoint`. Whisper menggunakan SDK untuk menemukan file model ONNX yang di-cache, lalu menjalankan inferensi langsung dengan ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Perbedaan Kunci dari Chat</b></summary>

```javascript
// Penyelesaian chat (Bagian 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transkripsi audio (Bagian ini):
// Menggunakan AudioClient bawaan SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Selalu atur bahasa untuk hasil terbaik
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Wawasan kunci:** Model chat menggunakan API kompatibel OpenAI melalui `manager.urls[0] + "/v1"`. Transkripsi Whisper menggunakan `AudioClient` SDK, diperoleh dari `model.createAudioClient()`. Atur `settings.language` untuk menghindari output kacau dari deteksi otomatis.

</details>

<details>
<summary><b>C# - Perbedaan Kunci dari Chat</b></summary>

Pendekatan C# menggunakan `AudioClient` bawaan SDK untuk transkripsi dalam proses:

**Inisialisasi model:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**Transkripsi:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Wawasan kunci:** C# menggunakan `FoundryLocalManager.CreateAsync()` dan mendapatkan `AudioClient` secara langsung — tidak perlu setup ONNX Runtime. Atur `Settings.Language` untuk menghindari output kacau akibat deteksi otomatis.

</details>

> **Ringkasan:** Python menggunakan Foundry Local SDK untuk manajemen model dan ONNX Runtime untuk inferensi langsung terhadap model encoder/decoder. JavaScript dan C# sama-sama menggunakan `AudioClient` bawaan SDK untuk transkripsi yang efisien — buat client, atur bahasa, dan panggil `transcribe()` / `TranscribeAudioAsync()`. Selalu atur properti bahasa pada AudioClient untuk hasil yang akurat.

---

### Latihan 6 - Eksperimen

Coba modifikasi berikut untuk memperdalam pemahaman Anda:

1. **Coba file audio berbeda** - rekam suara Anda menggunakan Windows Voice Recorder, simpan sebagai WAV, dan transkripsikan

2. **Bandingkan varian model** - jika Anda memiliki GPU NVIDIA, coba varian CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Bandingkan kecepatan transkripsi dengan varian CPU.

3. **Tambahkan format output** - respons JSON dapat berisi:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Buat REST API** - bungkus kode transkripsi Anda dalam server web:

   | Bahasa | Framework | Contoh |
   |--------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` dengan `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` dengan `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` dengan `IFormFile` |

5. **Multi-turn dengan transkripsi** - gabungkan Whisper dengan agen chat dari Bagian 4: transkripsikan audio dulu, lalu teruskan teks ke agen untuk analisis atau ringkasan.

---

## Referensi API Audio SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — membuat instance `AudioClient`
> - `audioClient.settings.language` — atur bahasa transkripsi (misal `"en"`)
> - `audioClient.settings.temperature` — kontrol randomness (opsional)
> - `audioClient.transcribe(filePath)` — mentranskripsi file, mengembalikan `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — streaming potongan transkripsi melalui callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — membuat instance `OpenAIAudioClient`
> - `audioClient.Settings.Language` — atur bahasa transkripsi (misal `"en"`)
> - `audioClient.Settings.Temperature` — kontrol randomness (opsional)
> - `await audioClient.TranscribeAudioAsync(filePath)` — mentranskripsi file, mengembalikan objek dengan `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — mengembalikan `IAsyncEnumerable` potongan transkripsi

> **Tip:** Selalu atur properti bahasa sebelum transkripsi. Tanpa pengaturan ini, model Whisper mencoba deteksi otomatis, yang dapat menghasilkan output kacau (karakter pengganti tunggal alih-alih teks).

---

## Perbandingan: Model Chat vs. Whisper

| Aspek | Model Chat (Bagian 3-7) | Whisper - Python | Whisper - JS / C# |
|-------|-------------------------|------------------|-------------------|
| **Jenis tugas** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Input** | Pesan teks (JSON) | File audio (WAV/MP3/M4A) | File audio (WAV/MP3/M4A) |
| **Output** | Teks hasil generate (streaming) | Teks hasil transkripsi (lengkap) | Teks hasil transkripsi (lengkap) |
| **Paket SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Metode API** | `client.chat.completions.create()` | ONNX Runtime langsung | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Pengaturan bahasa** | N/A | Token prompt decoder | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Ya | Tidak | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Keuntungan privasi** | Kode/data tetap lokal | Data audio tetap lokal | Data audio tetap lokal |

---

## Intisari Utama

| Konsep | Apa yang Anda Pelajari |
|---------|-----------------------|
| **Whisper on-device** | Speech-to-text berjalan sepenuhnya lokal, ideal untuk mentranskripsi panggilan dan ulasan produk pelanggan Zava di perangkat |
| **SDK AudioClient** | SDK JavaScript dan C# menyediakan `AudioClient` bawaan yang menangani pipeline transkripsi lengkap dalam satu panggilan |
| **Pengaturan bahasa** | Selalu atur bahasa pada AudioClient (misal `"en"`) — tanpa ini, deteksi otomatis dapat menghasilkan output kacau |
| **Python** | Menggunakan `foundry-local-sdk` untuk manajemen model + `onnxruntime` + `transformers` + `librosa` untuk inferensi ONNX langsung |
| **JavaScript** | Menggunakan `foundry-local-sdk` dengan `model.createAudioClient()` — atur `settings.language`, lalu panggil `transcribe()` |
| **C#** | Menggunakan `Microsoft.AI.Foundry.Local` dengan `model.GetAudioClientAsync()` — atur `Settings.Language`, lalu panggil `TranscribeAudioAsync()` |
| **Dukungan streaming** | SDK JS dan C# juga menyediakan `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` untuk keluaran potongan demi potongan |
| **Optimasi CPU** | Varian CPU (3,05 GB) bekerja pada perangkat Windows tanpa GPU |
| **Privasi utama** | Sempurna untuk menjaga interaksi pelanggan Zava dan data produk rahasia tetap di perangkat |

---

## Sumber Daya

| Sumber Daya | Tautan |
|-------------|--------|
| Dokumentasi Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referensi SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Model OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Situs Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Langkah Selanjutnya

Lanjutkan ke [Bagian 10: Menggunakan Model Kustom atau Hugging Face](part10-custom-models.md) untuk mengkompilasi model Anda sendiri dari Hugging Face dan menjalankannya melalui Foundry Local.