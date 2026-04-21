![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagian 9: Transkripsi Suara dengan Whisper dan Foundry Local

> **Matlamat:** Gunakan model OpenAI Whisper yang dijalankan secara tempatan melalui Foundry Local untuk mentranskripsikan fail audio - sepenuhnya di peranti, tanpa perlu awan.

## Gambaran Keseluruhan

Foundry Local bukan sahaja untuk penjanaan teks; ia juga menyokong model **ucapan-ke-teks**. Dalam makmal ini anda akan menggunakan model **OpenAI Whisper Medium** untuk mentranskripsikan fail audio sepenuhnya pada mesin anda. Ini sesuai untuk senario seperti mentranskripsikan panggilan khidmat pelanggan Zava, rakaman ulasan produk, atau sesi perancangan bengkel di mana data audio tidak boleh keluar dari peranti anda.


---

## Objektif Pembelajaran

Menjelang tamat makmal ini anda akan dapat:

- Memahami model ucapan-ke-teks Whisper dan keupayaannya
- Memuat turun dan menjalankan model Whisper menggunakan Foundry Local
- Mentranskripsikan fail audio menggunakan Foundry Local SDK dalam Python, JavaScript, dan C#
- Membina perkhidmatan transkripsi mudah yang berjalan sepenuhnya di peranti
- Memahami perbezaan antara model sembang/teks dan model audio dalam Foundry Local

---

## Prasyarat

| Keperluan | Butiran |
|-------------|---------|
| **Foundry Local CLI** | Versi **0.8.101 atau ke atas** (Model Whisper tersedia dari v0.8.101 ke atas) |
| **OS** | Windows 10/11 (x64 atau ARM64) |
| **Persekitaran bahasa** | **Python 3.9+** dan/atau **Node.js 18+** dan/atau **.NET 9 SDK** ([Muat turun .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Selesai** | [Bahagian 1: Memulakan](part1-getting-started.md), [Bahagian 2: Foundry Local SDK Mendalam](part2-foundry-local-sdk.md), dan [Bahagian 3: SDK dan API](part3-sdk-and-apis.md) |

> **Nota:** Model Whisper mesti dimuat turun melalui **SDK** (bukan CLI). CLI tidak menyokong titik akhir transkripsi audio. Semak versi anda dengan:
> ```bash
> foundry --version
> ```

---

## Konsep: Bagaimana Whisper Berfungsi dengan Foundry Local

Model OpenAI Whisper adalah model pengecaman ucapan tujuan umum yang dilatih pada dataset audio pelbagai yang besar. Apabila dijalankan melalui Foundry Local:

- Model dijalankan **sepenuhnya pada CPU anda** - tiada GPU diperlukan
- Audio tidak pernah keluar dari peranti anda - **privasi sepenuhnya**
- Foundry Local SDK mengendalikan muat turun model dan pengurusan cache
- **JavaScript dan C#** menyediakan `AudioClient` terbina dalam SDK yang mengendalikan keseluruhan saluran transkripsi — tiada tetapan ONNX manual diperlukan
- **Python** menggunakan SDK untuk pengurusan model dan ONNX Runtime untuk inferens terus pada model ONNX penyandi/peleraian

### Bagaimana Saluran Berfungsi (JavaScript dan C#) — SDK AudioClient

1. **Foundry Local SDK** memuat turun dan menyimpan model Whisper dalam cache
2. `model.createAudioClient()` (JS) atau `model.GetAudioClientAsync()` (C#) mencipta `AudioClient`
3. `audioClient.transcribe(path)` (JS) atau `audioClient.TranscribeAudioAsync(path)` (C#) mengendalikan saluran penuh secara dalaman — prapemprosesan audio, penyandi, peleraian, dan pengekodan token
4. `AudioClient` mendedahkan sifat `settings.language` (diset kepada `"en"` untuk Bahasa Inggeris) untuk membimbing transkripsi tepat

### Bagaimana Saluran Berfungsi (Python) — ONNX Runtime

1. **Foundry Local SDK** memuat turun dan menyimpan fail model ONNX Whisper dalam cache
2. **Prapemprosesan audio** menukar audio WAV menjadi spektrogram mel (80 bin mel x 3000 bingkai)
3. **Penyandi** memproses spektrogram mel dan menghasilkan keadaan tersembunyi serta tensor utama/nilai perhatian silang
4. **Peleraian** berjalan autoregresif, menghasilkan satu token pada satu masa sehingga menghasilkan token tamat teks
5. **Tokeniser** mengekod balik ID token output menjadi teks yang boleh dibaca

### Variasi Model Whisper

| Alias | ID Model | Peranti | Saiz | Penerangan |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Dipercepatkan GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Dioptimumkan CPU (disyorkan untuk kebanyakan peranti) |

> **Nota:** Tidak seperti model sembang yang disenaraikan secara lalai, model Whisper dikategorikan di bawah tugas `automatic-speech-recognition`. Gunakan `foundry model info whisper-medium` untuk melihat butiran.

---

## Latihan Makmal

### Latihan 0 - Dapatkan Fail Audio Contoh

Makmal ini termasuk fail WAV terbina berdasarkan senario produk Zava DIY. Hasilkan ia dengan skrip yang disertakan:

```bash
# Dari akar repo - buat dan aktifkan .venv terlebih dahulu
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Ia menghasilkan enam fail WAV dalam `samples/audio/`:

| Fail | Senario |
|------|----------|
| `zava-customer-inquiry.wav` | Pelanggan bertanya tentang **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Pelanggan mengulas **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | Panggilan sokongan mengenai **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | DIYer merancang dek dengan **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Tinjauan bengkel menggunakan **semua lima produk Zava** |
| `zava-full-project-walkthrough.wav` | Tinjauan renovasi garaj lanjutan menggunakan **semua produk Zava** (~4 minit, untuk ujian audio panjang) |

> **Petua:** Anda juga boleh menggunakan fail WAV/MP3/M4A milik anda, atau merakam sendiri dengan Windows Voice Recorder.

---

### Latihan 1 - Muat Turun Model Whisper Menggunakan SDK

Disebabkan ketidakserasian CLI dengan model Whisper dalam versi Foundry Local terkini, gunakan **SDK** untuk memuat turun dan memuat model. Pilih bahasa anda:

<details>
<summary><b>🐍 Python</b></summary>

**Pasang SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Mula perkhidmatan
manager = FoundryLocalManager()
manager.start_service()

# Semak maklumat katalog
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Semak jika sudah disimpan dalam cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Muatkan model ke dalam memori
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

// Cipta pengurus dan mulakan perkhidmatan
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

// Muatkan model ke dalam memori
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

> **Kenapa SDK bukannya CLI?** Foundry Local CLI tidak menyokong memuat turun atau menyajikan model Whisper secara langsung. SDK menyediakan cara yang boleh dipercayai untuk memuat turun dan mengurus model audio secara program. SDK JavaScript dan C# menyertakan `AudioClient` terbina dalam yang mengendalikan seluruh saluran transkripsi. Python menggunakan ONNX Runtime untuk inferens terus pada fail model yang disimpan dalam cache.

---

### Latihan 2 - Fahami SDK Whisper

Transkripsi Whisper menggunakan pendekatan berbeza bergantung pada bahasa. **JavaScript dan C#** menyediakan `AudioClient` terbina dalam Foundry Local SDK yang mengendalikan saluran penuh (prapemprosesan audio, penyandi, peleraian, pengekodan token) dalam satu panggilan metode. **Python** menggunakan Foundry Local SDK untuk pengurusan model dan ONNX Runtime untuk inferens terus pada model penyandi/peleraian ONNX.

| Komponen | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **Pakej SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Pengurusan model** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Ekstraksi ciri** | `WhisperFeatureExtractor` + `librosa` | Dikendalikan oleh SDK `AudioClient` | Dikendalikan oleh SDK `AudioClient` |
| **Inferens** | `ort.InferenceSession` (penyandi + peleraian) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Decoding token** | `WhisperTokenizer` | Dikendalikan oleh SDK `AudioClient` | Dikendalikan oleh SDK `AudioClient` |
| **Tetapan bahasa** | Tetapkan melalui `forced_ids` dalam token peleraian | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | Laluan fail WAV | Laluan fail WAV | Laluan fail WAV |
| **Output** | Teks yang telah didekod | `result.text` | `result.Text` |

> **Penting:** Sentiasa tetapkan bahasa pada `AudioClient` (contoh `"en"` untuk Bahasa Inggeris). Tanpa tetapan bahasa yang jelas, model mungkin menghasilkan output yang kacau kerana ia cuba mengesan bahasa secara automatik.

> **Corak SDK:** Python menggunakan `FoundryLocalManager(alias)` untuk memulakan, kemudian `get_cache_location()` untuk mencari fail model ONNX. JavaScript dan C# menggunakan `AudioClient` terbina dalam SDK — diperoleh melalui `model.createAudioClient()` (JS) atau `model.GetAudioClientAsync()` (C#) — yang mengendalikan seluruh saluran transkripsi. Lihat [Bahagian 2: Foundry Local SDK Mendalam](part2-foundry-local-sdk.md) untuk maklumat penuh.

---

### Latihan 3 - Bina Aplikasi Transkripsi Mudah

Pilih bahasa anda dan bina aplikasi minimum yang mentranskripsikan fail audio.

> **Format audio disokong:** WAV, MP3, M4A. Untuk hasil terbaik, gunakan fail WAV dengan kadar sampel 16kHz.

<details>
<summary><h3>Laluan Python</h3></summary>

#### Persediaan

```bash
cd python
python -m venv venv

# Aktifkan persekitaran maya:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Kod Transkripsi

Bina fail `foundry-local-whisper.py`:

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

# Langkah 1: Bootstrap - mula perkhidmatan, muat turun, dan muat model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Bina laluan ke fail model ONNX yang di-cache
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Langkah 2: Muat sesi ONNX dan pengekstrak ciri
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

# Langkah 3: Ekstrak ciri spektrogram mel
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Langkah 4: Jalankan penekoder
enc_out = encoder.run(None, {"audio_features": input_features})
# Output pertama adalah keadaan tersembunyi; yang selebihnya adalah pasangan KV perhatian silang
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Langkah 5: Penyahkodan autoregresif
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transkripsi, tanpa cap masa
input_ids = np.array([initial_tokens], dtype=np.int32)

# Cache KV perhatian diri kosong
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

    if next_token == 50257:  # tamat teks
        break
    generated.append(next_token)

    # Kemas kini cache KV perhatian diri
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Jalankan

```bash
# Transkripsikan senario produk Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Atau cuba yang lain:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Penting untuk Python

| Metode | Tujuan |
|--------|---------|
| `FoundryLocalManager(alias)` | Mulakan: mula perkhidmatan, muat turun, dan muat model |
| `manager.get_cache_location()` | Dapatkan laluan fail model ONNX dalam cache |
| `WhisperFeatureExtractor.from_pretrained()` | Muat pengekstrak ciri spektrogram mel |
| `ort.InferenceSession()` | Buat sesi ONNX Runtime untuk penyandi dan peleraian |
| `tokenizer.decode()` | Tukar ID token output kembali kepada teks |

</details>

<details>
<summary><h3>Laluan JavaScript</h3></summary>

#### Persediaan

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Kod Transkripsi

Bina fail `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Langkah 1: Bootstrap - cipta pengurus, mula perkhidmatan, dan muatkan model
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

// Langkah 2: Cipta klien audio dan transkripsi
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

> **Nota:** Foundry Local SDK menyediakan `AudioClient` terbina dalam melalui `model.createAudioClient()` yang mengendalikan keseluruhan saluran inferens ONNX secara dalaman — tiada import `onnxruntime-node` diperlukan. Sentiasa tetapkan `audioClient.settings.language = "en"` untuk memastikan transkripsi bahasa Inggeris tepat.

#### Jalankan

```bash
# Transkripsikan scenario produk Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Atau cuba yang lain:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Penting untuk JavaScript

| Metode | Tujuan |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Cipta manager singleton |
| `await catalog.getModel(alias)` | Dapatkan model dari katalog |
| `model.download()` / `model.load()` | Muat turun dan muat model Whisper |
| `model.createAudioClient()` | Cipta klien audio untuk transkripsi |
| `audioClient.settings.language = "en"` | Tetapkan bahasa transkripsi (diperlukan untuk output tepat) |
| `audioClient.transcribe(path)` | Transkripsi fail audio, mengembalikan `{ text, duration }` |

</details>

<details>
<summary><h3>Laluan C#</h3></summary>

#### Persediaan

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Nota:** Laluan C# menggunakan pakej `Microsoft.AI.Foundry.Local` yang menyediakan `AudioClient` terbina dalam melalui `model.GetAudioClientAsync()`. Ini mengendalikan saluran transkripsi penuh secara dalam proses — tiada tetapan ONNX Runtime berasingan diperlukan.

#### Kod Transkripsi

Gantikan kandungan `Program.cs`:

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
# Transkripsikan senario produk Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Atau cuba yang lain:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Penting untuk C#

| Metode | Tujuan |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Memulakan Foundry Local dengan konfigurasi |
| `catalog.GetModelAsync(alias)` | Dapatkan model dari katalog |
| `model.DownloadAsync()` | Muat turun model Whisper |
| `model.GetAudioClientAsync()` | Dapatkan AudioClient (bukan ChatClient!) |
| `audioClient.Settings.Language = "en"` | Tetapkan bahasa transkripsi (diperlukan untuk output tepat) |
| `audioClient.TranscribeAudioAsync(path)` | Transkripsi fail audio |
| `result.Text` | Teks yang telah ditranskripsi |
> **C# vs Python/JS:** SDK C# menyediakan `AudioClient` terbina dalam untuk transkripsi dalam proses melalui `model.GetAudioClientAsync()`, serupa dengan SDK JavaScript. Python menggunakan ONNX Runtime secara langsung untuk inferens terhadap model pengekod/pendekod yang di-cache.

</details>

---

### Latihan 4 - Transkripsi Berkelompok Semua Sampel Zava

Kini setelah anda mempunyai aplikasi transkripsi yang berfungsi, transkripsi semua lima fail sampel Zava dan bandingkan hasilnya.

<details>
<summary><h3>Jejak Python</h3></summary>

Contoh penuh `python/foundry-local-whisper.py` sudah menyokong transkripsi berkelompok. Apabila dijalankan tanpa argumen, ia menyalin semua fail `zava-*.wav` dalam `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Contoh menggunakan `FoundryLocalManager(alias)` untuk bootstrapping, kemudian menjalankan sesi ONNX pengekod dan pendekod untuk setiap fail.

</details>

<details>
<summary><h3>Jejak JavaScript</h3></summary>

Contoh penuh `javascript/foundry-local-whisper.mjs` sudah menyokong transkripsi berkelompok. Apabila dijalankan tanpa argumen, ia menyalin semua fail `zava-*.wav` dalam `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Contoh menggunakan `FoundryLocalManager.create()` dan `catalog.getModel(alias)` untuk memulakan SDK, kemudian menggunakan `AudioClient` (dengan `settings.language = "en"`) untuk menyalin setiap fail.

</details>

<details>
<summary><h3>Jejak C#</h3></summary>

Contoh penuh `csharp/WhisperTranscription.cs` sudah menyokong transkripsi berkelompok. Apabila dijalankan tanpa argumen fail tertentu, ia menyalin semua fail `zava-*.wav` dalam `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Contoh menggunakan `FoundryLocalManager.CreateAsync()` dan `AudioClient` SDK (dengan `Settings.Language = "en"`) untuk transkripsi dalam proses.

</details>

**Apa yang perlu diperhatikan:** Bandingkan output transkripsi dengan teks asal dalam `samples/audio/generate_samples.py`. Sejauh manakah ketepatan Whisper menangkap nama produk seperti "Zava ProGrip" dan istilah teknikal seperti "brushless motor" atau "composite decking"?

---

### Latihan 5 - Fahami Corak Kod Utama

Kaji bagaimana transkripsi Whisper berbeza daripada chat completions dalam ketiga-tiga bahasa:

<details>
<summary><b>Python - Perbezaan Utama dari Chat</b></summary>

```python
# Lengkapkan sembang (Bahagian 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transkripsi audio (Bahagian ini):
# Menggunakan ONNX Runtime secara langsung dan bukannya klien OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... gelung pengekod autoregresif ...
print(tokenizer.decode(generated_tokens))
```

**Pengajaran utama:** Model chat menggunakan API yang serasi OpenAI melalui `manager.endpoint`. Whisper menggunakan SDK untuk mencari fail model ONNX yang di-cache, kemudian menjalankan inferens secara langsung dengan ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Perbezaan Utama dari Chat</b></summary>

```javascript
// Lengkapkan sembang (Bahagian 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transkripsi audio (Bahagian ini):
// Menggunakan AudioClient terbina dalam SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Sentiasa tetapkan bahasa untuk hasil terbaik
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Pengajaran utama:** Model chat menggunakan API yang serasi OpenAI melalui `manager.urls[0] + "/v1"`. Transkripsi Whisper menggunakan `AudioClient` SDK, diperoleh dari `model.createAudioClient()`. Tetapkan `settings.language` untuk mengelakkan output yang tidak jelas daripada pengesanan automatik.

</details>

<details>
<summary><b>C# - Perbezaan Utama dari Chat</b></summary>

Pendekatan C# menggunakan `AudioClient` terbina dalam SDK untuk transkripsi dalam proses:

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

**Pengajaran utama:** C# menggunakan `FoundryLocalManager.CreateAsync()` dan mendapatkan `AudioClient` secara langsung — tiada persediaan ONNX Runtime diperlukan. Tetapkan `Settings.Language` untuk mengelakkan output yang tidak jelas daripada pengesanan automatik.

</details>

> **Ringkasan:** Python menggunakan Foundry Local SDK untuk pengurusan model dan ONNX Runtime untuk inferens langsung terhadap model pengekod/pendekod. JavaScript dan C# kedua-duanya menggunakan `AudioClient` terbina dalam SDK untuk transkripsi terpadu — cipta klien, tetapkan bahasa, dan panggil `transcribe()` / `TranscribeAudioAsync()`. Sentiasa tetapkan sifat bahasa pada AudioClient untuk hasil yang tepat.

---

### Latihan 6 - Cuba-cuba

Cuba pengubahsuaian ini untuk memperdalam pemahaman anda:

1. **Cuba fail audio yang berbeza** - rakam diri anda bercakap menggunakan Windows Voice Recorder, simpan sebagai WAV, dan transkripsikan

2. **Bandingkan varian model** - jika anda mempunyai GPU NVIDIA, cuba varian CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Bandingkan kelajuan transkripsi dengan varian CPU.

3. **Tambah format output** - respons JSON boleh mengandungi:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Bina REST API** - bungkus kod transkripsi anda dalam pelayan web:

   | Bahasa | Rangka Kerja | Contoh |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` dengan `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` dengan `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` dengan `IFormFile` |

5. **Multi-pusingan dengan transkripsi** - gabungkan Whisper dengan ejen sembang dari Bahagian 4: transkripsikan audio dahulu, kemudian hantar teks ke ejen untuk analisis atau ringkasan.

---

## Rujukan API Audio SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — mencipta instans `AudioClient`
> - `audioClient.settings.language` — tetapkan bahasa transkripsi (contoh: `"en"`)
> - `audioClient.settings.temperature` — kawal kebarangkalian rawak (pilihan)
> - `audioClient.transcribe(filePath)` — transkripsikan fail, mengembalikan `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — alirkan potongan transkripsi melalui callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — mencipta instans `OpenAIAudioClient`
> - `audioClient.Settings.Language` — tetapkan bahasa transkripsi (contoh: `"en"`)
> - `audioClient.Settings.Temperature` — kawal kebarangkalian rawak (pilihan)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transkripsikan fail, mengembalikan objek dengan `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — mengembalikan `IAsyncEnumerable` potongan transkripsi

> **Tip:** Sentiasa tetapkan sifat bahasa sebelum transkripsi. Tanpanya, model Whisper cuba pengesanan automatik, yang boleh menghasilkan output yang tidak jelas (satu aksara pengganti tunggal menggantikan teks).

---

## Perbandingan: Model Chat vs Whisper

| Aspek | Model Chat (Bahagian 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Jenis tugasan** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Input** | Mesej teks (JSON) | Fail audio (WAV/MP3/M4A) | Fail audio (WAV/MP3/M4A) |
| **Output** | Teks dijana (dialirkan) | Teks ditranskripsikan (lengkap) | Teks ditranskripsikan (lengkap) |
| **Pakej SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Kaedah API** | `client.chat.completions.create()` | ONNX Runtime terus | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Tetapan bahasa** | Tidak ada | Token permulaan decoder | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Penstriman** | Ada | Tidak ada | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Kelebihan privasi** | Kod/data kekal setempat | Data audio kekal setempat | Data audio kekal setempat |

---

## Pengajaran Utama

| Konsep | Apa Yang Anda Pelajari |
|---------|-----------------|
| **Whisper pada peranti** | Ucapan-ke-teks berjalan sepenuhnya secara setempat, ideal untuk menyalin panggilan pelanggan Zava dan ulasan produk secara setempat |
| **SDK AudioClient** | SDK JavaScript dan C# menyediakan `AudioClient` terbina dalam yang mengendalikan keseluruhan rangkaian transkripsi dalam satu panggilan |
| **Tetapan bahasa** | Sentiasa tetapkan bahasa AudioClient (contoh: `"en"`) — tanpa itu, pengesanan automatik mungkin menghasilkan output yang tidak jelas |
| **Python** | Menggunakan `foundry-local-sdk` untuk pengurusan model + `onnxruntime` + `transformers` + `librosa` untuk inferens ONNX langsung |
| **JavaScript** | Menggunakan `foundry-local-sdk` dengan `model.createAudioClient()` — tetapkan `settings.language`, kemudian panggil `transcribe()` |
| **C#** | Menggunakan `Microsoft.AI.Foundry.Local` dengan `model.GetAudioClientAsync()` — tetapkan `Settings.Language`, kemudian panggil `TranscribeAudioAsync()` |
| **Sokongan penstriman** | SDK JS dan C# juga menawarkan `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` untuk output secara potongan |
| **Dioptimumkan CPU** | Varian CPU (3.05 GB) boleh digunakan pada mana-mana peranti Windows tanpa GPU |
| **Keutamaan privasi** | Sempurna untuk mengekalkan interaksi pelanggan Zava dan data produk proprietari secara setempat |

---

## Sumber

| Sumber | Pautan |
|----------|------|
| Dokumentasi Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Rujukan SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Model OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Laman web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Langkah Seterusnya

Teruskan ke [Bahagian 10: Menggunakan Model Tersuai atau Hugging Face](part10-custom-models.md) untuk menyusun model anda sendiri dari Hugging Face dan jalankan mereka melalui Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila maklum bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya hendaklah dianggap sebagai sumber yang sahih. Untuk maklumat penting, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab atas sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->