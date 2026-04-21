# Arahan Ejen Pengaturcaraan

Fail ini menyediakan konteks untuk ejen pengaturcaraan AI (GitHub Copilot, Copilot Workspace, Codex, dll.) yang bekerja dalam repositori ini.

## Gambaran Keseluruhan Projek

Ini adalah **bengkel praktikal** untuk membina aplikasi AI dengan [Foundry Local](https://foundrylocal.ai) — persekitaran ringan yang memuat turun, mengurus, dan menyajikan model bahasa sepenuhnya pada peranti melalui API yang serasi dengan OpenAI. Bengkel ini merangkumi panduan makmal langkah demi langkah dan contoh kod boleh dijalankan dalam Python, JavaScript, dan C#.

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

## Butiran Bahasa & Rangka Kerja

### Python
- **Lokasi:** `python/`, `zava-creative-writer-local/src/api/`
- **Kebergantungan:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Pakej utama:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Versi minimum:** Python 3.9+
- **Jalankan:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Lokasi:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Kebergantungan:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Pakej utama:** `foundry-local-sdk`, `openai`
- **Sistem modul:** Modul ES (`.mjs` fail, `"type": "module"`)
- **Versi minimum:** Node.js 18+
- **Jalankan:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Lokasi:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Fail projek:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Pakej utama:** `Microsoft.AI.Foundry.Local` (bukan Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset dengan QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Sasaran:** .NET 9.0 (TFM bersyarat: `net9.0-windows10.0.26100` di Windows, `net9.0` di tempat lain)
- **Jalankan:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Konvensyen Pengaturcaraan

### Umum
- Semua contoh kod adalah **contoh fail tunggal berdiri sendiri** — tiada perpustakaan utiliti bersama atau abstraksi.
- Setiap contoh berjalan secara berdikari setelah memasang kebergantungan sendiri.
- Kekunci API sentiasa ditetapkan kepada `"foundry-local"` — Foundry Local menggunakan ini sebagai pemegang tempat.
- URL asas menggunakan `http://localhost:<port>/v1` — port adalah dinamik dan ditemui semasa masa jalan melalui SDK (`manager.urls[0]` dalam JS, `manager.endpoint` dalam Python).
- SDK Foundry Local mengendalikan permulaan servis dan penemuan endpoint; utamakan corak SDK berbanding port yang dikodkan keras.

### Python
- Gunakan SDK `openai` dengan `OpenAI(base_url=..., api_key="not-required")`.
- Gunakan `FoundryLocalManager()` dari `foundry_local` untuk kitar hayat servis yang diurus SDK.
- Streaming: iterasi ke atas objek `stream` dengan `for chunk in stream:`.
- Tiada anotasi jenis dalam fail contoh (jaga contoh ringkas untuk pembelajar bengkel).

### JavaScript
- Syntax modul ES: `import ... from "..."`.
- Gunakan `OpenAI` dari `"openai"` dan `FoundryLocalManager` dari `"foundry-local-sdk"`.
- Corak inisialisasi SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Top-level `await` digunakan secara meluas.

### C#
- Nullable diaktifkan, implicit usings, .NET 9.
- Gunakan `FoundryLocalManager.StartServiceAsync()` untuk kitar hayat diurus SDK.
- Streaming: `CompleteChatStreaming()` dengan `foreach (var update in completionUpdates)`.
- `csharp/Program.cs` utama adalah router CLI yang menyampaikan ke kaedah statik `RunAsync()`.

### Panggilan Alat
- Hanya model tertentu menyokong panggilan alat: keluarga **Qwen 2.5** (`qwen2.5-*`) dan **Phi-4-mini** (`phi-4-mini`).
- Skema alat mengikuti format JSON panggilan fungsi OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Perbualan menggunakan corak multi-pusingan: pengguna → pembantu (tool_calls) → alat (keputusan) → pembantu (jawapan akhir).
- `tool_call_id` dalam mesej keputusan alat mesti sepadan dengan `id` dari panggilan alat model.
- Python menggunakan SDK OpenAI secara langsung; JavaScript menggunakan `ChatClient` SDK asli (`model.createChatClient()`); C# menggunakan SDK OpenAI dengan `ChatTool.CreateFunctionTool()`.

### ChatClient (Klien SDK Asli)
- JavaScript: `model.createChatClient()` mengembalikan `ChatClient` dengan `completeChat(messages, tools?)` dan `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` mengembalikan `ChatClient` standard yang boleh digunakan tanpa mengimport pakej NuGet OpenAI.
- Python tidak mempunyai ChatClient asli — gunakan SDK OpenAI dengan `manager.endpoint` dan `manager.api_key`.
- **Penting:** `completeStreamingChat` JavaScript menggunakan corak **callback**, bukan iterasi async.

### Model Penalaran
- `phi-4-mini-reasoning` membungkus pemikirannya dalam tag `<think>...</think>` sebelum jawapan akhir.
- Parse tag untuk memisahkan penalaran daripada jawapan bila perlu.

## Panduan Makmal

Fail makmal ada di `labs/` sebagai Markdown. Mereka mengikuti struktur konsisten:
- Imej header logo
- Tajuk dan matlamat
- Gambaran keseluruhan, Objektif Pembelajaran, Prasyarat
- Seksyen penerangan konsep dengan diagram
- Latihan bernombor dengan blok kod dan output dijangka
- Jadual ringkasan, Pengajaran Utama, Bacaan Lanjut
- Pautan navigasi ke bahagian seterusnya

Semasa mengedit kandungan makmal:
- Kekalkan gaya pemformatan Markdown dan hierarki seksyen yang sedia ada.
- Blok kod mesti menyatakan bahasa (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Sediakan varian bash dan PowerShell untuk arahan shell di mana OS penting.
- Gunakan gaya panggilan `> **Note:**`, `> **Tip:**`, dan `> **Troubleshooting:**`.
- Jadual menggunakan format tiang `| Header | Header |`.

## Perintah Bina & Uji

| Tindakan | Perintah |
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
| **Jana diagram** | `npx mmdc -i <input>.mmd -o <output>.svg` (memerlukan root `npm install`) |

## Kebergantungan Luaran

- **Foundry Local CLI** mesti dipasang pada mesin pembangun (`winget install Microsoft.FoundryLocal` atau `brew install foundrylocal`).
- **Servis Foundry Local** berjalan secara lokal dan mendedahkan API REST serasi OpenAI pada port dinamik.
- Tiada servis awan, kekunci API, atau langganan Azure diperlukan untuk menjalankan mana-mana contoh.
- Bahagian 10 (model khusus) juga memerlukan `onnxruntime-genai` dan memuat turun berat model dari Hugging Face.

## Fail Yang Tidak Boleh Dihantar

`.gitignore` mesti mengecualikan (dan memang mengecualikan kebanyakan):
- `.venv/` — persekitaran maya Python
- `node_modules/` — kebergantungan npm
- `models/` — output model ONNX terkompilasi (fail binari besar, dijana oleh Bahagian 10)
- `cache_dir/` — cache muat turun model Hugging Face
- `.olive-cache/` — direktori kerja Microsoft Olive
- `samples/audio/*.wav` — sampel audio yang dijana (dijana semula melalui `python samples/audio/generate_samples.py`)
- Artifak binaan Python standard (`__pycache__/`, `*.egg-info/`, `dist/`, dll.)

## Lesen

MIT — rujuk `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil maklum bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber yang sah. Untuk maklumat kritikal, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab atas sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->