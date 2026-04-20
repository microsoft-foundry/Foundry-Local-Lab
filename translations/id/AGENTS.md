# Petunjuk Agen Pengkodean

Berkas ini memberikan konteks untuk agen pengkodean AI (GitHub Copilot, Copilot Workspace, Codex, dll.) yang bekerja di repositori ini.

## Gambaran Proyek

Ini adalah **workshop langsung** untuk membangun aplikasi AI dengan [Foundry Local](https://foundrylocal.ai) — sebuah runtime ringan yang mengunduh, mengelola, dan menyajikan model bahasa sepenuhnya di perangkat melalui API yang kompatibel dengan OpenAI. Workshop ini mencakup panduan lab langkah demi langkah dan contoh kode yang dapat dijalankan dalam Python, JavaScript, dan C#.

## Struktur Repositori

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Detail Bahasa & Kerangka Kerja

### Python
- **Lokasi:** `python/`, `zava-creative-writer-local/src/api/`
- **Ketergantungan:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Paket utama:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Versi minimum:** Python 3.9+
- **Jalankan:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Lokasi:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Ketergantungan:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Paket utama:** `foundry-local-sdk`, `openai`
- **Sistem modul:** Modul ES (`.mjs` files, `"type": "module"`)
- **Versi minimum:** Node.js 18+
- **Jalankan:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Lokasi:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Berkas proyek:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Paket utama:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset dengan QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Target:** .NET 9.0 (TFM kondisional: `net9.0-windows10.0.26100` di Windows, `net9.0` di tempat lain)
- **Jalankan:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Konvensi Pengkodean

### Umum
- Semua contoh kode adalah **contoh berkas tunggal yang berdiri sendiri** — tidak ada pustaka atau abstraksi utilitas bersama.
- Setiap contoh berjalan secara mandiri setelah menginstal ketergantungannya sendiri.
- Kunci API selalu disetel ke `"foundry-local"` — Foundry Local menggunakan ini sebagai placeholder.
- URL dasar menggunakan `http://localhost:<port>/v1` — port bersifat dinamis dan ditemukan saat runtime melalui SDK (`manager.urls[0]` di JS, `manager.endpoint` di Python).
- Foundry Local SDK menangani startup layanan dan penemuan endpoint; lebih disukai pola SDK daripada port yang dikodekan secara keras.

### Python
- Gunakan SDK `openai` dengan `OpenAI(base_url=..., api_key="not-required")`.
- Gunakan `FoundryLocalManager()` dari `foundry_local` untuk siklus hidup layanan yang dikelola SDK.
- Streaming: iterasi atas objek `stream` dengan `for chunk in stream:`.
- Tidak ada anotasi tipe dalam berkas contoh (jaga contohnya ringkas untuk pelajar workshop).

### JavaScript
- Sintaks modul ES: `import ... from "..."`.
- Gunakan `OpenAI` dari `"openai"` dan `FoundryLocalManager` dari `"foundry-local-sdk"`.
- Pola inisialisasi SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- `await` tingkat atas digunakan secara menyeluruh.

### C#
- Nullable enabled, implicit usings, .NET 9.
- Gunakan `FoundryLocalManager.StartServiceAsync()` untuk siklus hidup yang dikelola SDK.
- Streaming: `CompleteChatStreaming()` dengan `foreach (var update in completionUpdates)`.
- `csharp/Program.cs` utama adalah router CLI yang mendispatch ke metode statis `RunAsync()`.

### Pemanggilan Alat
- Hanya model tertentu yang mendukung pemanggilan alat: keluarga **Qwen 2.5** (`qwen2.5-*`) dan **Phi-4-mini** (`phi-4-mini`).
- Skema alat mengikuti format JSON panggilan fungsi OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Percakapan menggunakan pola multi-giliran: pengguna → asisten (tool_calls) → alat (hasil) → asisten (jawaban akhir).
- `tool_call_id` dalam pesan hasil alat harus cocok dengan `id` dari panggilan alat model.
- Python menggunakan SDK OpenAI langsung; JavaScript menggunakan `ChatClient` asli dari SDK (`model.createChatClient()`); C# menggunakan SDK OpenAI dengan `ChatTool.CreateFunctionTool()`.

### ChatClient (Klien SDK Asli)
- JavaScript: `model.createChatClient()` mengembalikan `ChatClient` dengan `completeChat(messages, tools?)` dan `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` mengembalikan `ChatClient` standar yang bisa digunakan tanpa mengimpor paket NuGet OpenAI.
- Python tidak memiliki ChatClient asli — gunakan SDK OpenAI dengan `manager.endpoint` dan `manager.api_key`.
- **Penting:** `completeStreamingChat` JavaScript menggunakan pola **callback**, bukan iterasi asinkron.

### Model Penalaran
- `phi-4-mini-reasoning` membungkus pemikirannya dalam tag `<think>...</think>` sebelum jawaban akhir.
- Parsing tag untuk memisahkan penalaran dari jawaban bila diperlukan.

## Panduan Lab

Berkas lab berada di `labs/` dalam format Markdown. Mereka mengikuti struktur konsisten:
- Gambar header logo
- Judul dan tujuan yang disorot
- Gambaran, Tujuan Pembelajaran, Prasyarat
- Seksi penjelasan konsep dengan diagram
- Latihan bernomor dengan blok kode dan output yang diharapkan
- Tabel ringkasan, Poin Penting, Bacaan Lanjutan
- Tautan navigasi ke bagian selanjutnya

Saat mengedit konten lab:
- Pertahankan gaya format Markdown dan hierarki seksi yang ada.
- Blok kode harus menentukan bahasa (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Berikan varian bash dan PowerShell untuk perintah shell jika OS berpengaruh.
- Gunakan gaya penandaan `> **Note:**`, `> **Tip:**`, dan `> **Troubleshooting:**`.
- Tabel menggunakan format `| Header | Header |` dengan pipa.

## Perintah Build & Test

| Aksi | Perintah |
|--------|---------|
| **Contoh Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Contoh JS** | `cd javascript && npm install && node <script>.mjs` |
| **Contoh C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generate diagram** | `npx mmdc -i <input>.mmd -o <output>.svg` (memerlukan `npm install` root) |

## Ketergantungan Eksternal

- **Foundry Local CLI** harus diinstal di mesin pengembang (`winget install Microsoft.FoundryLocal` atau `brew install foundrylocal`).
- **Layanan Foundry Local** berjalan secara lokal dan menyediakan API REST kompatibel OpenAI pada port dinamis.
- Tidak diperlukan layanan cloud, kunci API, atau langganan Azure untuk menjalankan contoh apa pun.
- Bagian 10 (model kustom) juga memerlukan `onnxruntime-genai` dan mengunduh bobot model dari Hugging Face.

## Berkas Yang Tidak Boleh Dikomit

`.gitignore` harus mengecualikan (dan sebagian besar sudah):
- `.venv/` — lingkungan virtual Python
- `node_modules/` — ketergantungan npm
- `models/` — output model ONNX terkompilasi (berkas binari besar, dihasilkan oleh Bagian 10)
- `cache_dir/` — cache unduhan model Hugging Face
- `.olive-cache/` — direktori kerja Microsoft Olive
- `samples/audio/*.wav` — contoh audio yang dihasilkan (dihasilkan ulang lewat `python samples/audio/generate_samples.py`)
- Artifak build Python standar (`__pycache__/`, `*.egg-info/`, `dist/`, dll.)

## Lisensi

MIT — lihat `LICENSE`.