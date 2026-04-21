<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Bengkel Foundry Local - Bina Apl AI Pada Peranti

Satu bengkel praktikal untuk menjalankan model bahasa pada mesin anda sendiri dan membina aplikasi pintar dengan [Foundry Local](https://foundrylocal.ai) dan [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Apakah Foundry Local?** Foundry Local adalah runtime ringan yang membolehkan anda memuat turun, mengurus, dan menyediakan model bahasa sepenuhnya pada perkakasan anda. Ia membuka API **mesra OpenAI** supaya mana-mana alat atau SDK yang menyokong OpenAI boleh disambungkan - tiada akaun awan diperlukan.

---

## Objektif Pembelajaran

Menjelang akhir bengkel ini anda akan dapat:

| # | Objektif |
|---|-----------|
| 1 | Pasang Foundry Local dan urus model dengan CLI |
| 2 | Kuasai API SDK Foundry Local untuk pengurusan model secara pengaturcaraan |
| 3 | Sambungkan ke pelayan inferens tempatan menggunakan SDK Python, JavaScript, dan C# |
| 4 | Bina saluran Retrieval-Augmented Generation (RAG) yang berasas pada data anda sendiri |
| 5 | Cipta ejen AI dengan arahan dan persona berterusan |
| 6 | Atur aliran kerja multi-ejen dengan gelung maklum balas |
| 7 | Terokai aplikasi capstone pengeluaran - Zava Creative Writer |
| 8 | Bina kerangka penilaian dengan set data emas dan pemarkahan LLM-sebagai-juri |
| 9 | Transkripsi audio dengan Whisper - pertuturan ke teks secara atas peranti menggunakan Foundry Local SDK |
| 10 | Kompil dan jalankan model tersuai atau Hugging Face dengan ONNX Runtime GenAI dan Foundry Local |
| 11 | Benarkan model tempatan memanggil fungsi luaran dengan corak panggilan alat |
| 12 | Bina UI berasaskan pelayar untuk Zava Creative Writer dengan penstriman masa nyata |

---

## Prasyarat

| Keperluan | Butiran |
|-------------|---------|
| **Perkakasan** | Minimum 8 GB RAM (16 GB disyorkan); CPU yang menyokong AVX2 atau GPU yang disokong |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, atau macOS 13+ |
| **Foundry Local CLI** | Pasang melalui `winget install Microsoft.FoundryLocal` (Windows) atau `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Lihat [panduan permulaan](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) untuk butiran. |
| **Persekitaran Bahasa** | **Python 3.9+** dan/atau **.NET 9.0+** dan/atau **Node.js 18+** |
| **Git** | Untuk memuat turun repositori ini |

---

## Memulakan

```bash
# 1. Klon repositori
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Sahkan Foundry Local telah dipasang
foundry model list              # Senarai model yang tersedia
foundry model run phi-3.5-mini  # Mulakan sembang interaktif

# 3. Pilih trek bahasa anda (lihat makmal Bahagian 2 untuk persediaan penuh)
```

| Bahasa | Mulakan Pantas |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Bahagian Bengkel

### Bahagian 1: Memulakan dengan Foundry Local

**Panduan makmal:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Apakah Foundry Local dan cara ia berfungsi
- Memasang CLI pada Windows dan macOS
- Meneroka model - menyenaraikan, memuat turun, menjalankan
- Memahami alias model dan port dinamik

---

### Bahagian 2: Pendalaman Foundry Local SDK

**Panduan makmal:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Mengapa menggunakan SDK berbanding CLI untuk pembangunan aplikasi
- Rujukan API SDK lengkap untuk Python, JavaScript, dan C#
- Pengurusan perkhidmatan, penerokaan katalog, kitar hidup model (muat turun, muat, nyah muat)
- Corak permulaan cepat: bootstrap pembina Python, `init()` JavaScript, `CreateAsync()` C#
- Metadata `FoundryModelInfo`, alias, dan pemilihan model optimum untuk perkakasan

---

### Bahagian 3: SDK dan API

**Panduan makmal:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Menyambung ke Foundry Local dari Python, JavaScript, dan C#
- Menggunakan Foundry Local SDK untuk mengurus perkhidmatan secara programatik
- Penstriman lengkap sembang melalui API mesra OpenAI
- Rujukan kaedah SDK untuk setiap bahasa

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Sembang penstriman asas |
| C# | `csharp/BasicChat.cs` | Sembang penstriman dengan .NET |
| JavaScript | `javascript/foundry-local.mjs` | Sembang penstriman dengan Node.js |

---

### Bahagian 4: Retrieval-Augmented Generation (RAG)

**Panduan makmal:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Apakah RAG dan kepentingannya
- Membina pangkalan pengetahuan dalam memori
- Pencarian tumpang tindih kata kunci dengan pemarkahan
- Menyusun arahan sistem berasas
- Menjalankan saluran RAG lengkap secara atas peranti

**Contoh kod:**

| Bahasa | Fail |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Bahagian 5: Membangun Ejen AI

**Panduan makmal:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Apakah ejen AI (berbanding panggilan LLM terus)
- Corak `ChatAgent` dan Microsoft Agent Framework
- Arahan sistem, persona, dan perbualan berbilang giliran
- Output berstruktur (JSON) dari ejen

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Ejen tunggal dengan Agent Framework |
| C# | `csharp/SingleAgent.cs` | Ejen tunggal (corak ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Ejen tunggal (corak ChatAgent) |

---

### Bahagian 6: Aliran Kerja Multi-Ejen

**Panduan makmal:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Saluran multi-ejen: Penyelidik → Penulis → Editor
- Penyusunan berperingkat dan gelung maklum balas
- Konfigurasi dikongsi dan penyerahan berstruktur
- Reka bentuk aliran kerja multi-ejen anda sendiri

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Saluran tiga ejen |
| C# | `csharp/MultiAgent.cs` | Saluran tiga ejen |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Saluran tiga ejen |

---

### Bahagian 7: Zava Creative Writer - Aplikasi Capstone

**Panduan makmal:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Aplikasi multi-ejen gaya pengeluaran dengan 4 ejen khusus
- Saluran berperingkat dengan gelung maklum balas dipandu penilai
- Output penstriman, carian katalog produk, penyerahan JSON berstruktur
- Pelaksanaan penuh dalam Python (FastAPI), JavaScript (Node.js CLI), dan C# (konsol .NET)

**Contoh kod:**

| Bahasa | Direktori | Penerangan |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Perkhidmatan web FastAPI dengan pengatur |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplikasi CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplikasi konsol .NET 9 |

---

### Bahagian 8: Pembangunan Berpandukan Penilaian

**Panduan makmal:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bina kerangka penilaian sistematik untuk ejen AI menggunakan set data emas
- Pemeriksaan berasaskan peraturan (panjang, liputan kata kunci, istilah terlarang) + pemarkahan LLM-sebagai-juri
- Perbandingan sebaris varian prompt dengan skor agregat
- Melanjutkan corak ejen Editor Zava dari Bahagian 7 ke suite ujian luar talian
- Laluan Python, JavaScript, dan C#

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Kerangka penilaian |
| C# | `csharp/AgentEvaluation.cs` | Kerangka penilaian |
| JavaScript | `javascript/foundry-local-eval.mjs` | Kerangka penilaian |

---

### Bahagian 9: Transkripsi Suara dengan Whisper

**Panduan makmal:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Transkripsi pertuturan ke teks menggunakan OpenAI Whisper yang berjalan secara tempatan
- Pemprosesan audio berfokus privasi - audio tidak pernah meninggalkan peranti anda
- Laluan Python, JavaScript, dan C# dengan `client.audio.transcriptions.create()` (Python/JS) dan `AudioClient.TranscribeAudioAsync()` (C#)
- Termasuk fail audio sampel bertema Zava untuk latihan praktikal

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transkripsi suara Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transkripsi suara Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transkripsi suara Whisper |

> **Nota:** Makmal ini menggunakan **Foundry Local SDK** untuk memuat turun dan memuat model Whisper secara programatik, kemudian menghantar audio ke titik hujung mesra OpenAI tempatan untuk transkripsi. Model Whisper (`whisper`) disenaraikan dalam katalog Foundry Local dan berjalan sepenuhnya atas peranti - tiada kekunci API awan atau akses rangkaian diperlukan.

---

### Bahagian 10: Menggunakan Model Tersuai atau Hugging Face

**Panduan makmal:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Menyusun model Hugging Face ke format ONNX dioptimumkan menggunakan pembina model ONNX Runtime GenAI
- Kompilasi khusus perkakasan (CPU, GPU NVIDIA, DirectML, WebGPU) dan kuantisasi (int4, fp16, bf16)
- Mencipta fail konfigurasi templat sembang untuk Foundry Local
- Menambah model terkompil ke cache Foundry Local
- Menjalankan model tersuai melalui CLI, REST API, dan SDK OpenAI
- Contoh rujukan: menyusun Qwen/Qwen3-0.6B dari awal hingga akhir

---

### Bahagian 11: Panggilan Alat dengan Model Tempatan

**Panduan makmal:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Benarkan model tempatan memanggil fungsi luaran (panggilan alat/fungsi)
- Takrifkan skema alat menggunakan format panggilan fungsi OpenAI
- Kendalikan aliran perbualan panggilan alat berbilang giliran
- Laksanakan panggilan alat secara tempatan dan kembalikan hasil kepada model
- Pilih model yang sesuai untuk senario panggilan alat (Qwen 2.5, Phi-4-mini)
- Gunakan `ChatClient` asli SDK untuk panggilan alat (JavaScript)

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Panggilan alat dengan alat cuaca/populasi |
| C# | `csharp/ToolCalling.cs` | Panggilan alat dengan .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Panggilan alat dengan ChatClient |

---

### Bahagian 12: Membangun UI Web untuk Zava Creative Writer

**Panduan makmal:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Tambah antaramuka pengguna berasaskan pelayar untuk Zava Creative Writer
- Sajikan UI berkongsi dari Python (FastAPI), JavaScript (Node.js HTTP), dan C# (ASP.NET Core)
- Gunakan streaming NDJSON di pelayar dengan Fetch API dan ReadableStream
- Lencana status ejen langsung dan penstriman teks artikel masa nyata

**Kod (UI berkongsi):**

| Fail | Penerangan |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Susun atur halaman |
| `zava-creative-writer-local/ui/style.css` | Gaya |
| `zava-creative-writer-local/ui/app.js` | Pembaca aliran dan logik kemas kini DOM |

**Penambahan backend:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Dikemaskini untuk menyediakan UI statik |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Pelayan HTTP baru membalut pengatur |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Projek API mudah ASP.NET Core baru |

---

### Bahagian 13: Bengkel Selesai
**Panduan makmal:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Ringkasan segala yang telah anda bina sepanjang 12 bahagian
- Idea lanjut untuk mengembangkan aplikasi anda
- Pautan kepada sumber dan dokumentasi

---

## Struktur Projek

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

## Sumber

| Sumber | Pautan |
|----------|------|
| Laman web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalog model | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Panduan memulakan | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Rujukan SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Rangka Kerja Ejen Microsoft | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lesen

Bahan bengkel ini disediakan untuk tujuan pendidikan.

---

**Selamat membina! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil perhatian bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber yang sahih. Untuk maklumat penting, terjemahan manusia profesional adalah disarankan. Kami tidak bertanggungjawab atas sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->