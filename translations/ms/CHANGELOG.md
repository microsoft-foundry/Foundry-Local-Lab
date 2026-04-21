# Changelog — Bengkel Tempatan Foundry

Semua perubahan penting untuk bengkel ini didokumentasikan di bawah.

---

## 2026-03-11 — Bahagian 12 & 13, UI Web, Penulisan Semula Whisper, Pembetulan WinML/QNN, dan Pengesahan

### Ditambah
- **Bahagian 12: Membina UI Web untuk Zava Creative Writer** — panduan makmal baru (`labs/part12-zava-ui.md`) dengan latihan yang merangkumi penstriman NDJSON, `ReadableStream` pelayar, lencana status agen langsung, dan penstriman teks artikel masa nyata
- **Bahagian 13: Bengkel Lengkap** — makmal ringkasan baru (`labs/part13-workshop-complete.md`) dengan ulasan ke atas semua 12 bahagian, idea lanjut, dan pautan sumber
- **UI Zava hadapan:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — antara muka pelayar vanilla HTML/CSS/JS dikongsi yang digunakan oleh ketiga-tiga backend
- **Pelayan HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — pelayan HTTP gaya Express baru membungkus pengaturcara untuk akses berasaskan pelayar
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` dan `ZavaCreativeWriterWeb.csproj` — projek API minimum baru yang melayani UI dan penstriman NDJSON
- **Penjana sampel audio:** `samples/audio/generate_samples.py` — skrip TTS luar talian menggunakan `pyttsx3` untuk menghasilkan fail WAV bertema Zava untuk Bahagian 9
- **Sampel audio:** `samples/audio/zava-full-project-walkthrough.wav` — sampel audio baru yang lebih panjang untuk ujian transkripsi
- **Skrip pengesahan:** `validate-npu-workaround.ps1` — skrip PowerShell automatik untuk mengesahkan penyelesaian NPU/QNN bagi semua sampel C#
- **SVG rajah Mermaid:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Sokongan silang platform WinML:** Ketiga-tiga fail `.csproj` C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) kini menggunakan TFM bersyarat dan rujukan pakej yang saling eksklusif untuk sokongan silang platform. Di Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superset yang merangkumi plugin QNN EP). Di luar Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK asas). RID `win-arm64` yang dikod keras dalam projek Zava digantikan dengan pengesanan automatik. Satu penyelesaian pergantungan transitif mengecualikan aset natif dari `Microsoft.ML.OnnxRuntime.Gpu.Linux` yang mempunyai rujukan win-arm64 yang rosak. Penyelesaian cuba/tangkap NPU sebelum ini telah dibuang dari ketujuh-tujuh fail C#.

### Diubah
- **Bahagian 9 (Whisper):** Penulisan semula utama — JavaScript kini menggunakan `AudioClient` terbina dalam SDK (`model.createAudioClient()`) dan bukannya inferens Runtime ONNX manual; deskripsi seni bina, jadual perbandingan, dan rajah aliran diperbaharui untuk mencerminkan pendekatan `AudioClient` JS/C# berbanding pendekatan Runtime ONNX Python
- **Bahagian 11:** Pautan navigasi dikemas kini (kini menunjuk ke Bahagian 12); ditambah rajah SVG yang dirender untuk aliran panggilan alat dan urutan
- **Bahagian 10:** Navigasi dikemas kini untuk laluan melalui Bahagian 12 dan bukannya berakhir di bengkel
- **Whisper Python (`foundry-local-whisper.py`):** Diperluas dengan sampel audio tambahan dan pengendalian ralat yang dipertingkat
- **Whisper JavaScript (`foundry-local-whisper.mjs`):** Ditulis semula untuk menggunakan `model.createAudioClient()` dengan `audioClient.transcribe()` dan bukannya sesi Runtime ONNX manual
- **FastAPI Python (`zava-creative-writer-local/src/api/main.py`):** Dikemas kini untuk menyajikan fail UI statik bersama API
- **Konsol Zava C# (`zava-creative-writer-local/src/csharp/Program.cs`):** Penghapusan penyelesaian masalah NPU (kini dikendalikan oleh pakej WinML)
- **README.md:** Ditambah bahagian Bahagian 12 dengan jadual contoh kod dan penambahan backend; ditambah bahagian Bahagian 13; dikemas kini objektif pembelajaran dan struktur projek
- **KNOWN-ISSUES.md:** Mengeluarkan Isu yang telah diselesaikan #7 (Varian Model NPU SDK C# — kini dikendalikan oleh pakej WinML). Penomboran semula isu baki kepada #1–#6. Dikemas kini butiran persekitaran dengan .NET SDK 10.0.104
- **AGENTS.md:** Dikemas kini pokok struktur projek dengan entri baru `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); dikemas kini pakej utama C# dan butiran TFM bersyarat
- **labs/part2-foundry-local-sdk.md:** Dikemas kini contoh `.csproj` untuk menunjukkan corak silang platform penuh dengan TFM bersyarat, rujukan pakej saling eksklusif, dan nota penerangan

### Disahkan
- Ketiga-tiga projek C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) dibina dengan jayanya pada Windows ARM64
- Sampel Chat (`dotnet run chat`): model dimuatkan sebagai `phi-3.5-mini-instruct-qnn-npu:1` melalui WinML/QNN — varian NPU dimuatkan terus tanpa fallback CPU
- Sampel Agen (`dotnet run agent`): berjalan sepenuhnya dengan perbualan berbilang pusingan, kod keluar 0
- Foundry Local CLI v0.8.117 dan SDK v0.9.0 pada .NET SDK 9.0.312

---

## 2026-03-11 — Pembetulan Kod, Pembersihan Model, Rajah Mermaid, dan Pengesahan

### Dibaiki
- **Semua 21 contoh kod (7 Python, 7 JavaScript, 7 C#):** Ditambah `model.unload()` / `unload_model()` / `model.UnloadAsync()` pembersihan semasa keluar untuk menyelesaikan amaran kebocoran memori OGA (Isu Dikenali #4)
- **csharp/WhisperTranscription.cs:** Menggantikan laluan relatif rapuh `AppContext.BaseDirectory` dengan `FindSamplesDirectory()` yang menaiki direktori untuk mencari `samples/audio` dengan boleh diharap (Isu Dikenali #7)
- **csharp/csharp.csproj:** Menggantikan `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` yang dikod keras dengan pengesanan automatik fallback menggunakan `$(NETCoreSdkRuntimeIdentifier)` supaya `dotnet run` berfungsi pada mana-mana platform tanpa bendera `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Diubah
- **Bahagian 8:** Menukar gelung iterasi berasaskan evaluasi dari rajah kotak ASCII ke imej SVG terhasil
- **Bahagian 10:** Menukar rajah paip kompilasi dari anak panah ASCII ke imej SVG terhasil
- **Bahagian 11:** Menukar rajah aliran panggilan alat dan urutan ke imej SVG terhasil
- **Bahagian 10:** Memindahkan bahagian "Bengkel Lengkap!" ke Bahagian 11 (makmal akhir); menggantikannya dengan pautan "Langkah Seterusnya"
- **KNOWN-ISSUES.md:** Pengesahan semula penuh semua isu terhadap CLI v0.8.117. Mengeluarkan isu yang telah diselesaikan: Kebocoran Memori OGA (pembersihan ditambah), laluan Whisper (FindSamplesDirectory), inferens bertahan HTTP 500 (tidak boleh diperbanyak, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), batasan tool_choice (kini berfungsi dengan `"required"` dan penargetan fungsi tertentu pada qwen2.5-0.5b). Dikemas kini isu JS Whisper — kini semua fail mengembalikan output kosong/biner (regresi dari v0.9.x, keparahan dinaikkan ke Major). Dikemas kini RID #4 C# dengan penyelesaian pengesanan automatik dan pautan [#497](https://github.com/microsoft/Foundry-Local/issues/497). 7 isu terbuka kekal.
- **javascript/foundry-local-whisper.mjs:** Membaiki nama pembolehubah pembersihan (`whisperModel` → `model`)

### Disahkan
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — menjalankan dengan jayanya dengan pembersihan
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — menjalankan dengan jayanya dengan pembersihan
- C#: `dotnet build` berjaya dengan 0 amaran, 0 ralat (sasaran net9.0)
- Semua 7 fail Python lulus pemeriksaan sintaks `py_compile`
- Semua 7 fail JavaScript lulus pengesahan sintaks `node --check`

---

## 2026-03-10 — Bahagian 11: Panggilan Alat, Pengembangan API SDK, dan Liputan Model

### Ditambah
- **Bahagian 11: Panggilan Alat dengan Model Tempatan** — panduan makmal baru (`labs/part11-tool-calling.md`) dengan 8 latihan merangkumi skema alat, aliran berbilang pusingan, pelbagai panggilan alat, alat tersuai, panggilan alat ChatClient, dan `tool_choice`
- **Sampel Python:** `python/foundry-local-tool-calling.py` — panggilan alat dengan alat `get_weather`/`get_population` menggunakan OpenAI SDK
- **Sampel JavaScript:** `javascript/foundry-local-tool-calling.mjs` — panggilan alat menggunakan `ChatClient` asli SDK (`model.createChatClient()`)
- **Sampel C#:** `csharp/ToolCalling.cs` — panggilan alat menggunakan `ChatTool.CreateFunctionTool()` dengan OpenAI SDK C#
- **Bahagian 2, Latihan 7:** `ChatClient` asli — `model.createChatClient()` (JS) dan `model.GetChatClientAsync()` (C#) sebagai alternatif kepada OpenAI SDK
- **Bahagian 2, Latihan 8:** Varian model dan pemilihan perkakasan — `selectVariant()`, `variants`, jadual varian NPU (7 model)
- **Bahagian 2, Latihan 9:** Naik taraf model dan kemas kini katalog — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Bahagian 2, Latihan 10:** Model penaakulan — `phi-4-mini-reasoning` dengan contoh analisis tag `<think>`
- **Bahagian 3, Latihan 4:** `createChatClient` sebagai alternatif kepada OpenAI SDK, dengan dokumentasi corak panggilan balik penstriman
- **AGENTS.md:** Ditambah konvensyen pengkodan Panggilan Alat, ChatClient, dan Model Penaakulan

### Diubah
- **Bahagian 1:** Katalog model diperluas — ditambah phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Bahagian 2:** Jadual rujukan API diperluas — ditambah `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Bahagian 2:** Penomboran semula latihan 7-9 → 10-13 untuk memasukkan latihan baru
- **Bahagian 3:** Jadual Pengajaran Utama dikemas kini untuk memasukkan ChatClient asli
- **README.md:** Ditambah bahagian Bahagian 11 dengan jadual contoh kod; ditambah objektif pembelajaran #11; dikemas kini pokok struktur projek
- **csharp/Program.cs:** Ditambah kes `toolcall` pada router CLI dan kemas kini teks bantuan

---

## 2026-03-09 — Kemas Kini SDK v0.9.0, Bahasa Inggeris British, dan Laluan Pengesahan

### Diubah
- **Semua contoh kod (Python, JavaScript, C#):** Dikemas kini kepada API Foundry Local SDK v0.9.0 — membetulkan `await catalog.getModel()` (hilang `await`), dikemas kini corak inisialisasi `FoundryLocalManager`, membetulkan penemuan titik akhir
- **Semua panduan makmal (Bahagian 1-10):** Ditukar kepada Bahasa Inggeris British (colour, catalogue, optimised, dll.)
- **Semua panduan makmal:** Dikemas kini contoh kod SDK untuk sesuai dengan permukaan API v0.9.0
- **Semua panduan makmal:** Dikemas kini jadual rujukan API dan blok kod latihan
- **Pembetulan kritikal JavaScript:** Ditambah `await` yang hilang pada `catalog.getModel()` — mengembalikan `Promise` bukan objek `Model`, menyebabkan kegagalan senyap hilang di bawah

### Disahkan
- Semua sampel Python berjalan dengan jayanya terhadap perkhidmatan Foundry Local
- Semua sampel JavaScript berjalan dengan jayanya (Node.js 18+)
- Projek C# dibina dan berjalan pada .NET 9.0 (keserasian hadapan dari pemasangan SDK net8.0)
- 29 fail diubah dan disahkan di seluruh bengkel

---

## Indeks Fail

| Fail | Dikemas Kini Terakhir | Penerangan |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Katalog model diperluas |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Latihan baru 7-10, jadual API diperluas |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Latihan Baru 4 (ChatClient), pengajaran dikemas kini |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggeris British |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggeris British |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggeris British |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggeris British |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Rajah Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Bahasa Inggeris British |
| `labs/part10-custom-models.md` | 2026-03-11 | Rajah Mermaid, alihkan Bengkel Selesai ke Bahagian 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Makmal baru, rajah Mermaid, bahagian Bengkel Selesai |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Baru: contoh panggilan alat |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Baru: contoh panggilan alat |
| `csharp/ToolCalling.cs` | 2026-03-10 | Baru: contoh panggilan alat |
| `csharp/Program.cs` | 2026-03-10 | Ditambah arahan CLI `toolcall` |
| `README.md` | 2026-03-10 | Bahagian 11, struktur projek |
| `AGENTS.md` | 2026-03-10 | Panggilan alat + konvensyen ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Isu #7 yang diselesaikan dikeluarkan, 6 isu terbuka kekal |
| `csharp/csharp.csproj` | 2026-03-11 | TFM pelbagai platform, rujukan bersyarat WinML/SDK asas |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM pelbagai platform, pengesanan RID automatik |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM pelbagai platform, pengesanan RID automatik |
| `csharp/BasicChat.cs` | 2026-03-11 | Selesai kerja kiraan cubaan/capai NPU dikeluarkan |
| `csharp/SingleAgent.cs` | 2026-03-11 | Selesai kerja kiraan cubaan/capai NPU dikeluarkan |
| `csharp/MultiAgent.cs` | 2026-03-11 | Selesai kerja kiraan cubaan/capai NPU dikeluarkan |
| `csharp/RagPipeline.cs` | 2026-03-11 | Selesai kerja kiraan cubaan/capai NPU dikeluarkan |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Selesai kerja kiraan cubaan/capai NPU dikeluarkan |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Contoh .csproj pelbagai platform |
| `AGENTS.md` | 2026-03-11 | Kemaskini pakej C# dan butiran TFM |
| `CHANGELOG.md` | 2026-03-11 | Fail ini |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil perhatian bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber yang sahih. Untuk maklumat penting, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab terhadap sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->