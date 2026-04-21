# Changelog — Foundry Local Workshop

Semua perubahan penting pada workshop ini didokumentasikan di bawah.

---

## 2026-03-11 — Bagian 12 & 13, Web UI, Penulisan Ulang Whisper, Perbaikan WinML/QNN, dan Validasi

### Ditambahkan
- **Bagian 12: Membangun Web UI untuk Zava Creative Writer** — panduan lab baru (`labs/part12-zava-ui.md`) dengan latihan yang mencakup streaming NDJSON, `ReadableStream` browser, lencana status agen langsung, dan streaming teks artikel waktu nyata
- **Bagian 13: Workshop Selesai** — ringkasan lab baru (`labs/part13-workshop-complete.md`) dengan rekap semua 12 bagian, gagasan lanjutan, dan tautan sumber daya
- **Antarmuka depan UI Zava:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — antarmuka browser vanilla HTML/CSS/JS bersama yang digunakan oleh ketiga backend
- **Server HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — server HTTP bergaya Express baru yang membungkus orchestrator untuk akses berbasis browser
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` dan `ZavaCreativeWriterWeb.csproj` — proyek API minimal baru yang melayani UI dan streaming NDJSON
- **Generator sampel audio:** `samples/audio/generate_samples.py` — skrip TTS offline menggunakan `pyttsx3` untuk menghasilkan file WAV bertema Zava untuk Bagian 9
- **Sampel audio:** `samples/audio/zava-full-project-walkthrough.wav` — sampel audio lebih panjang baru untuk pengujian transkripsi
- **Skrip validasi:** `validate-npu-workaround.ps1` — skrip PowerShell otomatis untuk memvalidasi solusi work-around NPU/QNN di semua contoh C#
- **Diagram Mermaid SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Dukungan WinML lintas platform:** Ketiga file C# `.csproj` (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) sekarang menggunakan TFM kondisional dan referensi paket yang eksklusif secara mutual untuk dukungan lintas platform. Di Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superset yang termasuk plugin EP QNN). Di non-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK dasar). RID `win-arm64` yang di-hardcode di proyek Zava digantikan dengan deteksi otomatis. Solusi work-around ketergantungan transitif mengecualikan aset asli dari `Microsoft.ML.OnnxRuntime.Gpu.Linux` yang memiliki referensi `win-arm64` yang rusak. Solusi try/catch NPU sebelumnya telah dihapus dari semua 7 file C#.

### Diubah
- **Bagian 9 (Whisper):** Penulisan ulang besar — JavaScript sekarang menggunakan `AudioClient` bawaan SDK (`model.createAudioClient()`) bukan infersi ONNX Runtime manual; deskripsi arsitektur, tabel perbandingan, dan diagram pipeline diperbarui untuk mencerminkan pendekatan JS/C# `AudioClient` dibandingkan dengan pendekatan Python ONNX Runtime
- **Bagian 11:** Tautan navigasi diperbarui (sekarang mengarah ke Bagian 12); diagram SVG yang dirender untuk alur pemanggilan alat dan urutan ditambahkan
- **Bagian 10:** Navigasi diperbarui untuk melewati Bagian 12 alih-alih mengakhiri workshop
- **Python Whisper (`foundry-local-whisper.py`):** Diperluas dengan sampel audio tambahan dan perbaikan penanganan kesalahan
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Ditulis ulang untuk menggunakan `model.createAudioClient()` dengan `audioClient.transcribe()` alih-alih sesi ONNX Runtime manual
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Diperbarui untuk melayani berkas UI statis bersama API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** Workaround NPU dihapus (sekarang ditangani oleh paket WinML)
- **README.md:** Ditambahkan bagian Bagian 12 dengan tabel contoh kode dan penambahan backend; ditambahkan bagian Bagian 13; diperbarui tujuan pembelajaran dan struktur proyek
- **KNOWN-ISSUES.md:** Dihapus Issue #7 yang telah diselesaikan (Varian Model NPU SDK C# — sekarang ditangani oleh paket WinML). Nomor masalah yang tersisa diubah menjadi #1–#6. Detail lingkungan diperbarui dengan .NET SDK 10.0.104
- **AGENTS.md:** Struktur pohon proyek diperbarui dengan entri baru `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); paket utama C# dan detail TFM kondisional diperbarui
- **labs/part2-foundry-local-sdk.md:** Contoh `.csproj` diperbarui untuk menampilkan pola lintas platform penuh dengan TFM kondisional, referensi paket saling eksklusif, dan catatan penjelas

### Diverifikasi
- Ketiga proyek C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) berhasil dibangun di Windows ARM64
- Sampel chat (`dotnet run chat`): model dimuat sebagai `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — varian NPU dimuat langsung tanpa fallback CPU
- Sampel agen (`dotnet run agent`): berjalan menyeluruh dengan percakapan multi-giliran, kode keluar 0
- Foundry Local CLI v0.8.117 dan SDK v0.9.0 pada .NET SDK 9.0.312

---

## 2026-03-11 — Perbaikan Kode, Pembersihan Model, Diagram Mermaid, dan Validasi

### Diperbaiki
- **Semua 21 contoh kode (7 Python, 7 JavaScript, 7 C#):** Ditambahkan pembersihan `model.unload()` / `unload_model()` / `model.UnloadAsync()` saat keluar untuk mengatasi peringatan kebocoran memori OGA (Known Issue #4)
- **csharp/WhisperTranscription.cs:** Ganti jalur relatif rapuh `AppContext.BaseDirectory` dengan `FindSamplesDirectory()` yang menelusuri direktori ke atas guna menemukan `samples/audio` dengan andal (Known Issue #7)
- **csharp/csharp.csproj:** Ganti `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` yang di-hardcode dengan fallback deteksi otomatis menggunakan `$(NETCoreSdkRuntimeIdentifier)` sehingga `dotnet run` berjalan di platform mana pun tanpa opsi `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Diubah
- **Bagian 8:** Mengubah loop iterasi berbasis evaluasi dari diagram kotak ASCII ke gambar SVG yang dirender
- **Bagian 10:** Mengubah diagram pipeline kompilasi dari panah ASCII ke gambar SVG yang dirender
- **Bagian 11:** Mengubah diagram alur pemanggilan alat dan urutan menjadi gambar SVG yang dirender
- **Bagian 10:** Memindahkan bagian "Workshop Complete!" ke Bagian 11 (lab final); diganti dengan tautan "Langkah Selanjutnya"
- **KNOWN-ISSUES.md:** Validasi ulang penuh semua isu terhadap CLI v0.8.117. Dihapus yang terselesaikan: Kebocoran Memori OGA (pembersihan ditambahkan), jalur Whisper (FindSamplesDirectory), inferensi HTTP 500 berkelanjutan (tidak dapat direproduksi, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), keterbatasan tool_choice (sekarang berfungsi dengan `"required"` dan target fungsi spesifik pada qwen2.5-0.5b). Isu JS Whisper diperbarui — sekarang semua berkas menghasilkan output kosong/biner (regresi dari v0.9.x, tingkat keparahan dinaikkan jadi Major). RID C# #4 diperbarui dengan solusi deteksi otomatis dan tautan [#497](https://github.com/microsoft/Foundry-Local/issues/497). Tersisa 7 isu terbuka.
- **javascript/foundry-local-whisper.mjs:** Memperbaiki nama variabel pembersihan (`whisperModel` → `model`)

### Diverifikasi
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — berjalan sukses dengan pembersihan
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — berjalan sukses dengan pembersihan
- C#: `dotnet build` berhasil tanpa peringatan atau error (target net9.0)
- Ketujuh berkas Python lolos pemeriksaan sintaks `py_compile`
- Ketujuh berkas JavaScript lolos validasi sintaks `node --check`

---

## 2026-03-10 — Bagian 11: Pemanggilan Alat, Perluasan API SDK, dan Cakupan Model

### Ditambahkan
- **Bagian 11: Pemanggilan Alat dengan Model Lokal** — panduan lab baru (`labs/part11-tool-calling.md`) dengan 8 latihan mencakup skema alat, alur multi-giliran, pemanggilan banyak alat, alat khusus, pemanggilan alat ChatClient, dan `tool_choice`
- **Contoh Python:** `python/foundry-local-tool-calling.py` — pemanggilan alat dengan alat `get_weather`/`get_population` menggunakan OpenAI SDK
- **Contoh JavaScript:** `javascript/foundry-local-tool-calling.mjs` — pemanggilan alat menggunakan `ChatClient` asli SDK (`model.createChatClient()`)
- **Contoh C#:** `csharp/ToolCalling.cs` — pemanggilan alat menggunakan `ChatTool.CreateFunctionTool()` dengan OpenAI C# SDK
- **Bagian 2, Latihan 7:** `ChatClient` asli — `model.createChatClient()` (JS) dan `model.GetChatClientAsync()` (C#) sebagai alternatif OpenAI SDK
- **Bagian 2, Latihan 8:** Varian model dan pemilihan perangkat keras — `selectVariant()`, `variants`, tabel varian NPU (7 model)
- **Bagian 2, Latihan 9:** Pembaruan model dan penyegaran katalog — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Bagian 2, Latihan 10:** Model penalaran — `phi-4-mini-reasoning` dengan contoh parsing tag `<think>`
- **Bagian 3, Latihan 4:** `createChatClient` sebagai alternatif OpenAI SDK, dengan dokumentasi pola callback streaming
- **AGENTS.md:** Ditambahkan konvensi pengkodean Pemanggilan Alat, ChatClient, dan Model Penalaran

### Diubah
- **Bagian 1:** Katalog model diperluas — tambahkan phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Bagian 2:** Tabel referensi API diperluas — tambahkan `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Bagian 2:** Mengubah nomor latihan 7-9 → 10-13 untuk mengakomodasi latihan baru
- **Bagian 3:** Memperbarui tabel Intisari Kunci untuk memasukkan ChatClient asli
- **README.md:** Ditambahkan bagian Bagian 11 dengan tabel contoh kode; ditambahkan tujuan pembelajaran #11; memperbarui struktur pohon proyek
- **csharp/Program.cs:** Menambahkan kasus `toolcall` ke router CLI dan memperbarui teks bantuan

---

## 2026-03-09 — Pembaruan SDK v0.9.0, Bahasa Inggris British, dan Validasi

### Diubah
- **Semua contoh kode (Python, JavaScript, C#):** Diperbarui ke API Foundry Local SDK v0.9.0 — memperbaiki `await catalog.getModel()` (sebelumnya kurang `await`), memperbarui pola inisialisasi `FoundryLocalManager`, memperbaiki penemuan endpoint
- **Semua panduan lab (Bagian 1-10):** Diubah ke bahasa Inggris British (colour, catalogue, optimised, dll)
- **Semua panduan lab:** Memperbarui contoh kode SDK untuk menyesuaikan permukaan API v0.9.0
- **Semua panduan lab:** Memperbarui tabel referensi API dan blok kode latihan
- **Perbaikan kritis JavaScript:** Menambahkan `await` yang hilang pada `catalog.getModel()` — mengembalikan `Promise` bukan objek `Model`, menyebabkan kegagalan diam-diam di hilir

### Diverifikasi
- Semua contoh Python berjalan sukses terhadap layanan Foundry Local
- Semua contoh JavaScript berjalan sukses (Node.js 18+)
- Proyek C# dibangun dan berjalan pada .NET 9.0 (kompatibilitas maju dari assembly SDK net8.0)
- 29 berkas dimodifikasi dan diverifikasi di seluruh workshop

---

## Indeks Berkas

| Berkas | Terakhir Diperbarui | Deskripsi |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Katalog model diperluas |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Latihan baru 7-10, tabel API diperluas |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Latihan Baru 4 (ChatClient), intisari diperbarui |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggris British |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggris British |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggris British |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggris British |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Diagram Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggris British |
| `labs/part10-custom-models.md` | 2026-03-11 | Diagram Mermaid, memindahkan Workshop Complete ke Bagian 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Lab baru, diagram Mermaid, bagian Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Baru: contoh pemanggilan alat |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Baru: contoh pemanggilan alat |
| `csharp/ToolCalling.cs` | 2026-03-10 | Baru: contoh pemanggilan alat |
| `csharp/Program.cs` | 2026-03-10 | Menambahkan perintah CLI `toolcall` |
| `README.md` | 2026-03-10 | Bagian 11, struktur proyek |
| `AGENTS.md` | 2026-03-10 | Pemanggilan alat + konvensi ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Menghapus Masalah #7 yang telah diselesaikan, 6 masalah terbuka tersisa |
| `csharp/csharp.csproj` | 2026-03-11 | TFM lintas platform, referensi kondisional WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM lintas platform, pendeteksian otomatis RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM lintas platform, pendeteksian otomatis RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Menghapus solusi coba/tangkap NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Menghapus solusi coba/tangkap NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Menghapus solusi coba/tangkap NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Menghapus solusi coba/tangkap NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Menghapus solusi coba/tangkap NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Contoh .csproj lintas platform |
| `AGENTS.md` | 2026-03-11 | Memperbarui paket C# dan detail TFM |
| `CHANGELOG.md` | 2026-03-11 | File ini |