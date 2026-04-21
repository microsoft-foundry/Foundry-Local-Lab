<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Workshop Foundry Local - Membangun Aplikasi AI Di Perangkat

Workshop langsung untuk menjalankan model bahasa di mesin Anda sendiri dan membangun aplikasi cerdas dengan [Foundry Local](https://foundrylocal.ai) dan [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Apa itu Foundry Local?** Foundry Local adalah runtime ringan yang memungkinkan Anda mengunduh, mengelola, dan menyajikan model bahasa sepenuhnya di perangkat keras Anda. Ini menyediakan **API yang kompatibel dengan OpenAI** sehingga alat atau SDK apa pun yang mendukung OpenAI dapat terhubung - tanpa memerlukan akun cloud.

---

## Tujuan Pembelajaran

Pada akhir workshop ini Anda akan mampu:

| # | Tujuan |
|---|---------|
| 1 | Menginstal Foundry Local dan mengelola model dengan CLI |
| 2 | Menguasai API SDK Foundry Local untuk manajemen model secara programatik |
| 3 | Terhubung ke server inferensi lokal menggunakan SDK Python, JavaScript, dan C# |
| 4 | Membangun pipeline Retrieval-Augmented Generation (RAG) yang mendasarkan jawaban pada data Anda sendiri |
| 5 | Membuat agen AI dengan instruksi dan persona yang persisten |
| 6 | Mengorkestrasi alur kerja multi-agen dengan loop umpan balik |
| 7 | Mengeksplorasi aplikasi produksi capstone - Zava Creative Writer |
| 8 | Membangun kerangka evaluasi dengan dataset emas dan penilaian LLM-as-judge |
| 9 | Mentranskripsikan audio dengan Whisper - pengubahan suara ke teks pada perangkat menggunakan SDK Foundry Local |
| 10 | Mengkompilasi dan menjalankan model kustom atau Hugging Face dengan ONNX Runtime GenAI dan Foundry Local |
| 11 | Mengaktifkan model lokal untuk memanggil fungsi eksternal dengan pola panggilan alat |
| 12 | Membangun UI berbasis browser untuk Zava Creative Writer dengan streaming waktu nyata |

---

## Prasyarat

| Persyaratan | Detail |
|-------------|---------|
| **Perangkat Keras** | Minimum 8 GB RAM (disarankan 16 GB); CPU dengan AVX2 atau GPU yang didukung |
| **Sistem Operasi** | Windows 10/11 (x64/ARM), Windows Server 2025, atau macOS 13+ |
| **Foundry Local CLI** | Instal via `winget install Microsoft.FoundryLocal` (Windows) atau `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Lihat [panduan memulai](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) untuk detailnya. |
| **Runtime bahasa** | **Python 3.9+** dan/atau **.NET 9.0+** dan/atau **Node.js 18+** |
| **Git** | Untuk mengkloning repositori ini |

---

## Memulai

```bash
# 1. Kloning repositori
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Pastikan Foundry Local terinstal
foundry model list              # Daftar model yang tersedia
foundry model run phi-3.5-mini  # Mulai obrolan interaktif

# 3. Pilih jalur bahasa Anda (lihat lab Bagian 2 untuk pengaturan lengkap)
```

| Bahasa | Mulai Cepat |
|--------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Bagian Workshop

### Bagian 1: Memulai dengan Foundry Local

**Panduan lab:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Apa itu Foundry Local dan cara kerjanya
- Menginstal CLI di Windows dan macOS
- Menjelajahi model - daftar, unduh, jalankan
- Memahami alias model dan port dinamis

---

### Bagian 2: Pendalaman SDK Foundry Local

**Panduan lab:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Mengapa menggunakan SDK dibandingkan CLI untuk pengembangan aplikasi
- Referensi API SDK lengkap untuk Python, JavaScript, dan C#
- Manajemen layanan, penjelajahan katalog, siklus hidup model (unduh, muat, lepas muat)
- Pola mulai cepat: bootstrap konstruktor Python, JavaScript `init()`, C# `CreateAsync()`
- Metadata `FoundryModelInfo`, alias, dan pemilihan model optimal berdasarkan perangkat keras

---

### Bagian 3: SDK dan API

**Panduan lab:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Menghubungkan Foundry Local dari Python, JavaScript, dan C#
- Menggunakan SDK Foundry Local untuk mengelola layanan secara programatik
- Streaming penyelesaian obrolan melalui API kompatibel OpenAI
- Referensi metode SDK untuk tiap bahasa

**Contoh kode:**

| Bahasa | Berkas | Deskripsi |
|--------|---------|-----------|
| Python | `python/foundry-local.py` | Obrolan streaming dasar |
| C# | `csharp/BasicChat.cs` | Obrolan streaming dengan .NET |
| JavaScript | `javascript/foundry-local.mjs` | Obrolan streaming dengan Node.js |

---

### Bagian 4: Retrieval-Augmented Generation (RAG)

**Panduan lab:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Apa itu RAG dan mengapa penting
- Membangun basis pengetahuan in-memory
- Pengambilan tumpang tindih kata kunci dengan penilaian
- Menyusun prompt sistem yang berbasis data
- Menjalankan pipeline RAG lengkap di perangkat

**Contoh kode:**

| Bahasa | Berkas |
|--------|---------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Bagian 5: Membangun Agen AI

**Panduan lab:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Apa itu agen AI (dibandingkan dengan panggilan raw LLM)
- Pola `ChatAgent` dan Microsoft Agent Framework
- Instruksi sistem, persona, dan percakapan multi-turun
- Output terstruktur (JSON) dari agen

**Contoh kode:**

| Bahasa | Berkas | Deskripsi |
|--------|---------|-----------|
| Python | `python/foundry-local-with-agf.py` | Agen tunggal dengan Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agen tunggal (pola ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agen tunggal (pola ChatAgent) |

---

### Bagian 6: Alur Kerja Multi-Agen

**Panduan lab:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline multi-agen: Peneliti → Penulis → Editor
- Orkestrasi berurutan dan loop umpan balik
- Konfigurasi bersama dan penyerahan terstruktur
- Merancang alur kerja multi-agen Anda sendiri

**Contoh kode:**

| Bahasa | Berkas | Deskripsi |
|--------|---------|-----------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline tiga agen |
| C# | `csharp/MultiAgent.cs` | Pipeline tiga agen |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline tiga agen |

---

### Bagian 7: Zava Creative Writer - Aplikasi Capstone

**Panduan lab:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Aplikasi multi-agen gaya produksi dengan 4 agen khusus
- Pipeline berurutan dengan loop umpan balik yang dikendalikan evaluator
- Output streaming, pencarian katalog produk, penyerahan JSON terstruktur
- Implementasi penuh dalam Python (FastAPI), JavaScript (Node.js CLI), dan C# (konsol .NET)

**Contoh kode:**

| Bahasa | Direktori | Deskripsi |
|--------|-----------|-----------|
| Python | `zava-creative-writer-local/src/api/` | Layanan web FastAPI dengan pengatur |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplikasi CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplikasi konsol .NET 9 |

---

### Bagian 8: Pengembangan Berbasis Evaluasi

**Panduan lab:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Membangun kerangka evaluasi sistematis untuk agen AI menggunakan dataset emas
- Pemeriksaan berbasis aturan (panjang, cakupan kata kunci, istilah terlarang) + penilaian LLM-as-judge
- Perbandingan berdampingan varian prompt dengan kartu skor agregat
- Memperluas pola agen Editor Zava dari Bagian 7 ke suite tes offline
- Jalur Python, JavaScript, dan C#

**Contoh kode:**

| Bahasa | Berkas | Deskripsi |
|--------|---------|-----------|
| Python | `python/foundry-local-eval.py` | Kerangka evaluasi |
| C# | `csharp/AgentEvaluation.cs` | Kerangka evaluasi |
| JavaScript | `javascript/foundry-local-eval.mjs` | Kerangka evaluasi |

---

### Bagian 9: Transkripsi Suara dengan Whisper

**Panduan lab:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Transkripsi suara ke teks menggunakan OpenAI Whisper yang berjalan secara lokal
- Pemrosesan audio dengan prioritas privasi - audio tidak pernah meninggalkan perangkat Anda
- Jalur Python, JavaScript, dan C# dengan `client.audio.transcriptions.create()` (Python/JS) dan `AudioClient.TranscribeAudioAsync()` (C#)
- Termasuk berkas audio contoh bertema Zava untuk latihan langsung

**Contoh kode:**

| Bahasa | Berkas | Deskripsi |
|--------|---------|-----------|
| Python | `python/foundry-local-whisper.py` | Transkripsi suara Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transkripsi suara Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transkripsi suara Whisper |

> **Catatan:** Lab ini menggunakan **Foundry Local SDK** untuk mengunduh dan memuat model Whisper secara programatik, kemudian mengirim audio ke endpoint lokal yang kompatibel dengan OpenAI untuk transkripsi. Model Whisper (`whisper`) tercantum dalam katalog Foundry Local dan berjalan sepenuhnya di perangkat - tidak memerlukan kunci API cloud atau akses jaringan.

---

### Bagian 10: Menggunakan Model Kustom atau Hugging Face

**Panduan lab:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Mengompilasi model Hugging Face ke format ONNX yang dioptimalkan menggunakan pembangun model ONNX Runtime GenAI
- Kompilasi spesifik perangkat keras (CPU, GPU NVIDIA, DirectML, WebGPU) dan kuantisasi (int4, fp16, bf16)
- Membuat berkas konfigurasi template obrolan untuk Foundry Local
- Menambahkan model yang dikompilasi ke cache Foundry Local
- Menjalankan model kustom melalui CLI, REST API, dan OpenAI SDK
- Contoh referensi: kompilasi Qwen/Qwen3-0.6B secara end-to-end

---

### Bagian 11: Panggilan Alat dengan Model Lokal

**Panduan lab:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Mengizinkan model lokal memanggil fungsi eksternal (panggilan alat/fungsi)
- Mendefinisikan skema alat menggunakan format panggilan fungsi OpenAI
- Menangani alur percakapan panggilan alat multi-turun
- Mengeksekusi panggilan alat secara lokal dan mengembalikan hasil ke model
- Memilih model yang tepat untuk skenario panggilan alat (Qwen 2.5, Phi-4-mini)
- Menggunakan `ChatClient` asli SDK untuk panggilan alat (JavaScript)

**Contoh kode:**

| Bahasa | Berkas | Deskripsi |
|--------|---------|-----------|
| Python | `python/foundry-local-tool-calling.py` | Panggilan alat dengan alat cuaca/populasi |
| C# | `csharp/ToolCalling.cs` | Panggilan alat dengan .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Panggilan alat dengan ChatClient |

---

### Bagian 12: Membangun UI Web untuk Zava Creative Writer

**Panduan lab:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Menambahkan front end berbasis browser ke Zava Creative Writer
- Menyajikan UI bersama dari Python (FastAPI), JavaScript (Node.js HTTP), dan C# (ASP.NET Core)
- Mengkonsumsi streaming NDJSON di browser dengan Fetch API dan ReadableStream
- Lencana status agen langsung dan streaming teks artikel waktu nyata

**Kode (UI bersama):**

| Berkas | Deskripsi |
|--------|-----------|
| `zava-creative-writer-local/ui/index.html` | Tata letak halaman |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Pembaca streaming dan logika pembaruan DOM |

**Tambahan Backend:**

| Bahasa | Berkas | Deskripsi |
|--------|---------|-----------|
| Python | `zava-creative-writer-local/src/api/main.py` | Diperbarui untuk menyajikan UI statis |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Server HTTP baru yang membungkus orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Proyek API minimal ASP.NET Core baru |

---

### Bagian 13: Workshop Selesai
**Panduan lab:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Ringkasan dari semua yang telah Anda bangun di semua 12 bagian
- Ide lebih lanjut untuk memperluas aplikasi Anda
- Tautan ke sumber daya dan dokumentasi

---

## Struktur Proyek

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

## Sumber Daya

| Sumber Daya | Tautan |
|----------|------|
| Situs web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalog model | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Panduan memulai | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referensi SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lisensi

Materi workshop ini disediakan untuk tujuan pendidikan.

---

**Selamat membangun! 🚀**