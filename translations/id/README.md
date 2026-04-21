<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Workshop Foundry Local - Membangun Aplikasi AI Secara Lokal

Sebuah workshop langsung untuk menjalankan model bahasa di mesin Anda sendiri dan membangun aplikasi cerdas dengan [Foundry Local](https://foundrylocal.ai) dan [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Apa itu Foundry Local?** Foundry Local adalah runtime ringan yang memungkinkan Anda mengunduh, mengelola, dan menyajikan model bahasa sepenuhnya di perangkat keras Anda. Ini menampilkan **API kompatibel OpenAI** sehingga alat atau SDK apa pun yang berbicara OpenAI dapat terhubung - tanpa perlu akun cloud.

### 🌐 Dukungan Multi-Bahasa

#### Didukung melalui GitHub Action (Otomatis & Selalu Terbaru)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](./README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Lebih suka Mengkloning Secara Lokal?**
>
> Repository ini mencakup lebih dari 50 terjemahan bahasa yang secara signifikan meningkatkan ukuran unduhan. Untuk mengkloning tanpa terjemahan, gunakan sparse checkout:
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
> Ini memberikan semua yang Anda butuhkan untuk menyelesaikan kursus dengan unduhan yang jauh lebih cepat.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Tujuan Pembelajaran

Di akhir workshop ini Anda akan dapat:

| # | Tujuan |
|---|-----------|
| 1 | Menginstal Foundry Local dan mengelola model dengan CLI |
| 2 | Menguasai API SDK Foundry Local untuk pengelolaan model secara programatik |
| 3 | Terhubung ke server inferensi lokal menggunakan SDK Python, JavaScript, dan C# |
| 4 | Membangun pipeline Retrieval-Augmented Generation (RAG) yang mengakar pada data Anda sendiri |
| 5 | Membuat agen AI dengan instruksi dan persona yang persisten |
| 6 | Mengorkestrasi alur kerja multi-agen dengan loop umpan balik |
| 7 | Menjelajahi aplikasi produksi capstone - Zava Creative Writer |
| 8 | Membangun kerangka kerja evaluasi dengan dataset emas dan penilaian LLM-sebagai-juri |
| 9 | Mentranskripsikan audio dengan Whisper - pengubahan suara-ke-teks secara lokal menggunakan Foundry Local SDK |
| 10 | Mengompilasi dan menjalankan model kustom atau Hugging Face dengan ONNX Runtime GenAI dan Foundry Local |
| 11 | Mengaktifkan model lokal untuk memanggil fungsi eksternal dengan pola pemanggilan alat |
| 12 | Membangun UI berbasis browser untuk Zava Creative Writer dengan streaming waktu nyata |

---

## Prasyarat

| Persyaratan | Detail |
|-------------|---------|
| **Perangkat Keras** | Minimal RAM 8 GB (direkomendasikan 16 GB); CPU AVX2-capable atau GPU yang didukung |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, atau macOS 13+ |
| **Foundry Local CLI** | Instal melalui `winget install Microsoft.FoundryLocal` (Windows) atau `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Lihat [panduan memulai](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) untuk detailnya. |
| **Runtime Bahasa** | **Python 3.9+** dan/atau **.NET 9.0+** dan/atau **Node.js 18+** |
| **Git** | Untuk mengkloning repository ini |

---

## Memulai

```bash
# 1. Kloning repositori
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifikasi Foundry Local telah terpasang
foundry model list              # Daftar model yang tersedia
foundry model run phi-3.5-mini  # Mulai obrolan interaktif

# 3. Pilih jalur bahasa Anda (lihat lab Bagian 2 untuk pengaturan lengkap)
```

| Bahasa | Mulai Cepat |
|----------|-------------|
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

- Mengapa menggunakan SDK dibanding CLI untuk pengembangan aplikasi
- Referensi API SDK lengkap untuk Python, JavaScript, dan C#
- Manajemen layanan, penjelajahan katalog, siklus hidup model (unduh, muat, lepaskan)
- Pola cepat mulai: bootstrap konstruktor Python, `init()` JavaScript, `CreateAsync()` C#
- Metadata `FoundryModelInfo`, alias, dan pemilihan model optimal untuk perangkat keras

---

### Bagian 3: SDK dan API

**Panduan lab:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Terhubung ke Foundry Local dari Python, JavaScript, dan C#
- Menggunakan Foundry Local SDK untuk mengelola layanan secara programatik
- Streaming penyelesaian chat melalui API kompatibel OpenAI
- Referensi metode SDK untuk setiap bahasa

**Contoh kode:**

| Bahasa | File | Deskripsi |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat streaming dasar |
| C# | `csharp/BasicChat.cs` | Chat streaming dengan .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat streaming dengan Node.js |

---

### Bagian 4: Retrieval-Augmented Generation (RAG)

**Panduan lab:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Apa itu RAG dan mengapa penting
- Membangun basis pengetahuan dalam memori
- Pengambilan berdasarkan tumpang tindih kata kunci dengan penilaian
- Menyusun prompt sistem yang berakar
- Menjalankan pipeline RAG lengkap secara lokal

**Contoh kode:**

| Bahasa | File |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Bagian 5: Membangun Agen AI

**Panduan lab:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Apa itu agen AI (versus panggilan LLM biasa)
- Pola `ChatAgent` dan Microsoft Agent Framework
- Instruksi sistem, persona, dan percakapan multi-turun
- Output terstruktur (JSON) dari agen

**Contoh kode:**

| Bahasa | File | Deskripsi |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agen tunggal dengan Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agen tunggal (pola ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agen tunggal (pola ChatAgent) |

---

### Bagian 6: Alur Kerja Multi-Agen

**Panduan lab:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline multi-agen: Peneliti → Penulis → Editor
- Orkestrasi berurutan dan loop umpan balik
- Konfigurasi bersama dan serah terima terstruktur
- Mendesain alur kerja multi-agen Anda sendiri

**Contoh kode:**

| Bahasa | File | Deskripsi |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline tiga agen |
| C# | `csharp/MultiAgent.cs` | Pipeline tiga agen |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline tiga agen |

---

### Bagian 7: Zava Creative Writer - Aplikasi Capstone

**Panduan lab:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Aplikasi multi-agen bergaya produksi dengan 4 agen khusus
- Pipeline berurutan dengan loop umpan balik yang dipandu evaluator
- Output streaming, pencarian katalog produk, serah terima JSON terstruktur
- Implementasi lengkap dalam Python (FastAPI), JavaScript (Node.js CLI), dan C# (konsol .NET)

**Contoh kode:**

| Bahasa | Direktori | Deskripsi |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Layanan web FastAPI dengan orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplikasi CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplikasi konsol .NET 9 |

---

### Bagian 8: Pengembangan Berbasis Evaluasi

**Panduan lab:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Membangun kerangka evaluasi sistematis untuk agen AI menggunakan dataset emas
- Pemeriksaan berbasis aturan (panjang, cakupan kata kunci, istilah terlarang) + penilaian LLM-sebagai-juri
- Perbandingan samping-demi-samping varian prompt dengan kartu skor agregat
- Memperluas pola agen Editor Zava dari Bagian 7 menjadi suite pengujian offline
- Jalur Python, JavaScript, dan C#

**Contoh kode:**

| Bahasa | File | Deskripsi |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Kerangka evaluasi |
| C# | `csharp/AgentEvaluation.cs` | Kerangka evaluasi |
| JavaScript | `javascript/foundry-local-eval.mjs` | Kerangka evaluasi |

---

### Bagian 9: Transkripsi Suara dengan Whisper

**Panduan lab:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Transkripsi suara-ke-teks menggunakan OpenAI Whisper yang dijalankan secara lokal
- Pemrosesan audio yang mengutamakan privasi - audio tidak pernah keluar dari perangkat Anda
- Trek Python, JavaScript, dan C# dengan `client.audio.transcriptions.create()` (Python/JS) dan `AudioClient.TranscribeAudioAsync()` (C#)
- Termasuk file audio sampel bertema Zava untuk praktik langsung

**Contoh kode:**

| Bahasa | Berkas | Deskripsi |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transkripsi suara Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transkripsi suara Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transkripsi suara Whisper |

> **Catatan:** Laboratorium ini menggunakan **Foundry Local SDK** untuk mengunduh dan memuat model Whisper secara programatik, lalu mengirimkan audio ke endpoint lokal yang kompatibel dengan OpenAI untuk transkripsi. Model Whisper (`whisper`) tercantum dalam katalog Foundry Local dan berjalan sepenuhnya di perangkat - tidak memerlukan kunci API cloud atau akses jaringan.

---

### Bagian 10: Menggunakan Model Kustom atau Hugging Face

**Panduan lab:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Mengompilasi model Hugging Face ke format ONNX yang dioptimalkan menggunakan pembangun model ONNX Runtime GenAI
- Kompilasi spesifik perangkat keras (CPU, GPU NVIDIA, DirectML, WebGPU) dan kuantisasi (int4, fp16, bf16)
- Membuat berkas konfigurasi chat-template untuk Foundry Local
- Menambahkan model yang telah dikompilasi ke cache Foundry Local
- Menjalankan model kustom melalui CLI, REST API, dan OpenAI SDK
- Contoh referensi: kompilasi Qwen/Qwen3-0.6B secara end-to-end

---

### Bagian 11: Pemanggilan Alat dengan Model Lokal

**Panduan lab:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Mengaktifkan model lokal untuk memanggil fungsi eksternal (pemanggilan alat/fungsi)
- Mendefinisikan skema alat menggunakan format pemanggilan fungsi OpenAI
- Menangani alur percakapan pemanggilan alat multi-tahap
- Menjalankan pemanggilan alat secara lokal dan mengembalikan hasil ke model
- Memilih model yang tepat untuk skenario pemanggilan alat (Qwen 2.5, Phi-4-mini)
- Menggunakan `ChatClient` bawaan SDK untuk pemanggilan alat (JavaScript)

**Contoh kode:**

| Bahasa | Berkas | Deskripsi |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Pemanggilan alat dengan alat cuaca/populasi |
| C# | `csharp/ToolCalling.cs` | Pemanggilan alat dengan .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Pemanggilan alat dengan ChatClient |

---

### Bagian 12: Membangun UI Web untuk Zava Creative Writer

**Panduan lab:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Menambahkan antarmuka depan berbasis browser untuk Zava Creative Writer
- Menyajikan UI berbagi dari Python (FastAPI), JavaScript (Node.js HTTP), dan C# (ASP.NET Core)
- Mengonsumsi streaming NDJSON di browser dengan Fetch API dan ReadableStream
- Lencana status agen langsung dan streaming teks artikel secara real-time

**Kode (UI bersama):**

| Berkas | Deskripsi |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Tata letak halaman |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Pembaca stream dan logika pembaruan DOM |

**Penambahan backend:**

| Bahasa | Berkas | Deskripsi |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Diperbarui untuk menyajikan UI statis |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Server HTTP baru membungkus pengelola orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Proyek API minimal ASP.NET Core baru |

---

### Bagian 13: Workshop Selesai

**Panduan lab:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Ringkasan semua yang telah Anda bangun dalam 12 bagian
- Ide lanjutan untuk memperluas aplikasi Anda
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

Materi workshop ini disediakan untuk tujuan edukasi.

---

**Selamat membangun! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan layanan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Meskipun kami berusaha untuk akurasi, harap diperhatikan bahwa terjemahan otomatis mungkin mengandung kesalahan atau ketidakakuratan. Dokumen asli dalam bahasa aslinya harus dianggap sebagai sumber otoritatif. Untuk informasi penting, disarankan menggunakan terjemahan profesional oleh manusia. Kami tidak bertanggung jawab atas kesalahpahaman atau penafsiran yang salah yang timbul dari penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->