<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Bengkel Foundry Local - Bina Aplikasi AI Pada Peranti

Satu bengkel amali untuk menjalankan model bahasa pada mesin anda sendiri dan membina aplikasi pintar dengan [Foundry Local](https://foundrylocal.ai) dan [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Apakah Foundry Local?** Foundry Local ialah runtime ringan yang membolehkan anda memuat turun, mengurus, dan menghidangkan model bahasa sepenuhnya pada perkakasan anda. Ia mendedahkan API **serasi OpenAI** supaya mana-mana alat atau SDK yang menggunakan OpenAI boleh disambungkan - tiada akaun awan diperlukan.

### 🌐 Sokongan Pelbagai Bahasa

#### Disokong melalui Tindakan GitHub (Automatik & Sentiasa Terkini)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](./README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Lebih suka Klon Secara Tempatan?**
>
> Repositori ini merangkumi lebih 50 terjemahan bahasa yang secara signifikan meningkatkan saiz muat turun. Untuk klon tanpa terjemahan, gunakan sparse checkout:
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
> Ini memberikan anda segala yang diperlukan untuk menyelesaikan kursus dengan muat turun yang lebih pantas.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Objektif Pembelajaran

Menjelang akhir bengkel ini anda akan dapat:

| # | Objektif |
|---|-----------|
| 1 | Pasang Foundry Local dan uruskan model menggunakan CLI |
| 2 | Kuasai API Foundry Local SDK untuk pengurusan model secara programatik |
| 3 | Sambungkan ke pelayan inferens tempatan menggunakan SDK Python, JavaScript, dan C# |
| 4 | Bina saluran Penghasilan Ditambah Pencarian (RAG) yang kedudukan jawapan berdasarkan data anda sendiri |
| 5 | Cipta ejen AI dengan arahan berterusan dan persona |
| 6 | Orkestrasi aliran kerja multi-ejen dengan gelung maklum balas |
| 7 | Terokai aplikasi capstone produksi - Zava Creative Writer |
| 8 | Bina kerangka penilaian dengan set data emas dan penilaian LLM-sebagai-hakim |
| 9 | Transkripsi audio dengan Whisper - pertuturan-ke-teks pada peranti menggunakan Foundry Local SDK |
| 10 | Kompilasi dan jalankan model khusus atau Hugging Face dengan ONNX Runtime GenAI dan Foundry Local |
| 11 | Benarkan model tempatan memanggil fungsi luaran dengan corak panggilan alat |
| 12 | Bina UI berasaskan pelayar untuk Zava Creative Writer dengan penstriman masa nyata |

---

## Prasyarat

| Keperluan | Perincian |
|-------------|---------|
| **Perkakasan** | Minimum 8 GB RAM (16 GB disyorkan); CPU AVX2-mampu atau GPU yang disokong |
| **Sistem Pengendalian** | Windows 10/11 (x64/ARM), Windows Server 2025, atau macOS 13+ |
| **Foundry Local CLI** | Pasang melalui `winget install Microsoft.FoundryLocal` (Windows) atau `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Lihat [panduan mula](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) untuk butiran. |
| **Runtime Bahasa** | **Python 3.9+** dan/atau **.NET 9.0+** dan/atau **Node.js 18+** |
| **Git** | Untuk mengklon repositori ini |

---

## Memulakan

```bash
# 1. Klon repositori
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Sahkan Foundry Local telah dipasang
foundry model list              # Senaraikan model yang ada
foundry model run phi-3.5-mini  # Mulakan sembang interaktif

# 3. Pilih laluan bahasa anda (rujuk makmal Bahagian 2 untuk tetapan penuh)
```

| Bahasa | Mula Cepat |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Bahagian Bengkel

### Bahagian 1: Memulakan dengan Foundry Local

**Panduan makmal:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Apakah Foundry Local dan bagaimana ia berfungsi
- Memasang CLI pada Windows dan macOS
- Meneroka model - senarai, muat turun, jalankan
- Memahami alias model dan port dinamik

---

### Bahagian 2: Penyelaman Mendalam Foundry Local SDK

**Panduan makmal:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Mengapa menggunakan SDK berbanding CLI untuk pembangunan aplikasi
- Rujukan penuh API SDK untuk Python, JavaScript, dan C#
- Pengurusan perkhidmatan, melayari katalog, kitaran hidup model (muat turun, muat, nyahmuat)
- Corak mula cepat: bootstrap konstruktor Python, `init()` JavaScript, `CreateAsync()` C#
- Metadata `FoundryModelInfo`, alias, dan pemilihan model optima perkakasan

---

### Bahagian 3: SDK dan API

**Panduan makmal:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Menyambung ke Foundry Local dari Python, JavaScript, dan C#
- Menggunakan Foundry Local SDK untuk mengurus perkhidmatan secara programatik
- Penstriman penyelesaian sembang melalui API serasi OpenAI
- Rujukan kaedah SDK untuk setiap bahasa

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Sembang penstriman asas |
| C# | `csharp/BasicChat.cs` | Sembang penstriman dengan .NET |
| JavaScript | `javascript/foundry-local.mjs` | Sembang penstriman dengan Node.js |

---

### Bahagian 4: Penghasilan Ditambah Pencarian (RAG)

**Panduan makmal:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Apakah RAG dan mengapa ia penting
- Membina pangkalan pengetahuan dalam ingatan
- Pencarian tumpang tindih kata kunci dengan pemarkahan
- Menyusun arahan sistem berasaskan data
- Menjalankan saluran RAG penuh pada peranti

**Contoh kod:**

| Bahasa | Fail |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Bahagian 5: Membina Ejen AI

**Panduan makmal:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Apakah ejen AI (berbanding panggilan LLM mentah)
- Corak `ChatAgent` dan Microsoft Agent Framework
- Arahan sistem, persona, dan perbualan berbilang giliran
- Output berstruktur (JSON) daripada ejen

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Ejen tunggal dengan Agent Framework |
| C# | `csharp/SingleAgent.cs` | Ejen tunggal (corak ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Ejen tunggal (corak ChatAgent) |

---

### Bahagian 6: Aliran Kerja Multi-Ejen

**Panduan makmal:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Saluran multi-ejen: Penyelidik → Penulis → Penyunting
- Orkestrasi berurutan dan gelung maklum balas
- Konfigurasi berkongsi dan serah tangan berstruktur
- Reka bentuk aliran kerja multi-ejen sendiri

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Saluran tiga ejen |
| C# | `csharp/MultiAgent.cs` | Saluran tiga ejen |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Saluran tiga ejen |

---

### Bahagian 7: Zava Creative Writer - Aplikasi Capstone

**Panduan makmal:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Aplikasi multi-ejen gaya produksi dengan 4 ejen khusus
- Saluran berurutan dengan gelung maklum balas dipacu penilai
- Output penstriman, carian katalog produk, serah tangan JSON berstruktur
- Pelaksanaan penuh dalam Python (FastAPI), JavaScript (Node.js CLI), dan C# (konsol .NET)

**Contoh kod:**

| Bahasa | Direktori | Penerangan |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Perkhidmatan web FastAPI dengan pengorkestrasi |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplikasi CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplikasi konsol .NET 9 |

---

### Bahagian 8: Pembangunan Berpandukan Penilaian

**Panduan makmal:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bina kerangka penilaian sistematik untuk ejen AI menggunakan set data emas
- Semakan berasaskan peraturan (panjang, liputan kata kunci, istilah dilarang) + penilaian LLM-sebagai-hakim
- Perbandingan sebaris varian arahan dengan skor agregat
- Memperluas corak ejen Penyunting Zava dari Bahagian 7 ke suite ujian luar talian
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
- Transkripsi ucapan ke teks menggunakan OpenAI Whisper yang dijalankan secara setempat
- Pemprosesan audio berpandukan privasi - audio tidak pernah meninggalkan peranti anda
- Jejak Python, JavaScript, dan C# dengan `client.audio.transcriptions.create()` (Python/JS) dan `AudioClient.TranscribeAudioAsync()` (C#)
- Merangkumi fail audio contoh bertema Zava untuk latihan praktikal

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transkripsi suara Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transkripsi suara Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transkripsi suara Whisper |

> **Nota:** Makmal ini menggunakan **Foundry Local SDK** untuk memuat turun dan memuatkan model Whisper secara program, kemudian menghantar audio ke titik akhir OpenAI yang serasi secara setempat untuk transkripsi. Model Whisper (`whisper`) disenaraikan dalam katalog Foundry Local dan dijalankan sepenuhnya pada peranti - tiada kunci API awan atau akses rangkaian diperlukan.

---

### Bahagian 10: Menggunakan Model Tersuai atau Hugging Face

**Panduan makmal:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Menyusun model Hugging Face ke format ONNX yang dioptimumkan menggunakan pembina model ONNX Runtime GenAI
- Penyusunan khusus perkakasan (CPU, GPU NVIDIA, DirectML, WebGPU) dan kuantisasi (int4, fp16, bf16)
- Membuat fail konfigurasi templat-bualan untuk Foundry Local
- Menambah model yang disusun ke cache Foundry Local
- Menjalankan model tersuai melalui CLI, REST API, dan OpenAI SDK
- Contoh rujukan: penyusunan Qwen/Qwen3-0.6B dari awal hingga akhir

---

### Bahagian 11: Panggilan Alat dengan Model Setempat

**Panduan makmal:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Membolehkan model setempat memanggil fungsi luaran (panggilan alat/fungsi)
- Mendefinisikan skema alat menggunakan format panggilan fungsi OpenAI
- Mengendalikan aliran perbualan panggilan alat berbilang giliran
- Melaksanakan panggilan alat secara setempat dan mengembalikan keputusan kepada model
- Memilih model yang tepat untuk senario panggilan alat (Qwen 2.5, Phi-4-mini)
- Menggunakan `ChatClient` natif SDK untuk panggilan alat (JavaScript)

**Contoh kod:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Panggilan alat dengan alat cuaca/populasi |
| C# | `csharp/ToolCalling.cs` | Panggilan alat dengan .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Panggilan alat dengan ChatClient |

---

### Bahagian 12: Membangunkan UI Web untuk Penulis Kreatif Zava

**Panduan makmal:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Menambah antaramuka hadapan berasaskan penyemak imbas untuk Penulis Kreatif Zava
- Menyajikan UI dikongsi dari Python (FastAPI), JavaScript (Node.js HTTP), dan C# (ASP.NET Core)
- Menggunakan streaming NDJSON di penyemak imbas dengan Fetch API dan ReadableStream
- Lencana status agen langsung dan penstriman teks artikel masa nyata

**Kod (UI dikongsi):**

| Fail | Penerangan |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Susun atur halaman |
| `zava-creative-writer-local/ui/style.css` | Gaya |
| `zava-creative-writer-local/ui/app.js` | Pembaca aliran dan logik kemaskini DOM |

**Penambahan backend:**

| Bahasa | Fail | Penerangan |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Dikemas kini untuk menyajikan UI statik |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Pelayan HTTP baru membalut pengaturcara |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Projek API minimal ASP.NET Core baru |

---

### Bahagian 13: Bengkel Lengkap

**Panduan makmal:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Ringkasan segala yang telah anda bina merentasi semua 12 bahagian
- Idea lanjut untuk memperluas aplikasi anda
- Pautan ke sumber dan dokumentasi

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
| Panduan permulaan | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
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
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil perhatian bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidakakuratan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber yang sahih. Untuk maklumat penting, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab atas sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->