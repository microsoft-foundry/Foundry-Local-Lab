![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bagian 12: Membangun UI Web untuk Zava Creative Writer

> **Tujuan:** Menambahkan front end berbasis browser ke Zava Creative Writer sehingga Anda dapat melihat pipeline multi-agen berjalan secara real time, dengan lencana status agen langsung dan teks artikel yang dialirkan, semuanya disajikan dari satu server web lokal.

Di [Bagian 7](part7-zava-creative-writer.md) Anda telah mengeksplorasi Zava Creative Writer sebagai **aplikasi CLI** (JavaScript, C#) dan **API headless** (Python). Dalam lab ini Anda akan menghubungkan front end **HTML/CSS/JavaScript vanilla** yang sama ke setiap backend sehingga pengguna dapat berinteraksi dengan pipeline melalui browser alih-alih terminal.

---

## Apa yang Akan Anda Pelajari

| Tujuan | Deskripsi |
|-----------|-------------|
| Melayani file statis dari backend | Memasang direktori HTML/CSS/JS di samping rute API Anda |
| Mengonsumsi streaming NDJSON di browser | Menggunakan Fetch API dengan `ReadableStream` untuk membaca JSON yang dipisahkan baris |
| Protokol streaming terpadu | Memastikan backend Python, JavaScript, dan C# mengeluarkan format pesan yang sama |
| Pembaruan UI progresif | Memperbarui lencana status agen dan streaming teks artikel token demi token |
| Menambahkan lapisan HTTP ke aplikasi CLI | Membungkus logika orkestrasor yang ada dalam server gaya Express (JS) atau API minimal ASP.NET Core (C#) |

---

## Arsitektur

UI adalah satu set file statis (`index.html`, `style.css`, `app.js`) yang dibagikan oleh ketiga backend. Setiap backend mengekspos dua rute yang sama:

![Arsitektur UI Zava — front end bersama dengan tiga backend](../../../images/part12-architecture.svg)

| Rute | Metode | Tujuan |
|-------|--------|---------|
| `/` | GET | Menyajikan UI statis |
| `/api/article` | POST | Menjalankan pipeline multi-agen dan streaming NDJSON |

Front end mengirimkan body JSON dan membaca respons sebagai aliran pesan JSON yang dipisahkan baris. Setiap pesan memiliki field `type` yang digunakan UI untuk memperbarui panel yang benar:

| Jenis pesan | Arti |
|-------------|---------|
| `message` | Pembaruan status (misalnya "Memulai tugas agen peneliti...") |
| `researcher` | Hasil penelitian sudah siap |
| `marketing` | Hasil pencarian produk sudah siap |
| `writer` | Penulis mulai atau selesai (berisi `{ start: true }` atau `{ complete: true }`) |
| `partial` | Satu token yang dialirkan dari Writer (berisi `{ text: "..." }`) |
| `editor` | Putusan editor sudah siap |
| `error` | Terjadi kesalahan |

![Routing tipe pesan di browser](../../../images/part12-message-types.svg)

![Urutan streaming — Komunikasi Browser ke Backend](../../../images/part12-streaming-sequence.svg)

---

## Prasyarat

- Selesaikan [Bagian 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI terpasang dan model `phi-3.5-mini` sudah diunduh
- Browser web modern (Chrome, Edge, Firefox, atau Safari)

---

## UI Bersama

Sebelum menyentuh kode backend apa pun, luangkan waktu untuk menjelajahi front end yang akan digunakan oleh ketiga bahasa. File-file berada di `zava-creative-writer-local/ui/`:

| File | Tujuan |
|------|---------|
| `index.html` | Tata letak halaman: formulir input, lencana status agen, area keluaran artikel, panel detail yang dapat dilipat |
| `style.css` | Styling minimal dengan status-badge warna (menunggu, berjalan, selesai, error) |
| `app.js` | Panggilan fetch, pembaca baris `ReadableStream`, dan logika pembaruan DOM |

> **Tips:** Buka `index.html` langsung di browser Anda untuk pratinjau tata letak. Tidak ada yang akan berfungsi karena belum ada backend, tetapi Anda bisa melihat strukturnya.

### Cara Kerja Pembaca Stream

Fungsi kunci di `app.js` membaca body respons potongan demi potongan dan memisahkan pada batas baris baru:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // pertahankan baris terakhir yang tidak lengkap

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

Setiap pesan yang diparsing diteruskan ke `handleMessage()`, yang memperbarui elemen DOM terkait berdasarkan `msg.type`.

---

## Latihan

### Latihan 1: Jalankan Backend Python dengan UI

Varian Python (FastAPI) sudah memiliki endpoint API streaming. Satu-satunya perubahan adalah memasang folder `ui/` sebagai file statis.

**1.1** Masuk ke direktori API Python dan pasang dependensi:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Jalankan server:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Buka browser Anda di `http://localhost:8000`. Anda harus melihat UI Zava Creative Writer dengan tiga kolom teks dan tombol "Generate Article".

**1.4** Klik **Generate Article** dengan nilai default. Amati lencana status agen berubah dari "Waiting" ke "Running" ke "Done" saat setiap agen menyelesaikan pekerjaannya, dan teks artikel dialirkan ke panel keluaran token demi token.

> **Pemecahan Masalah:** Jika halaman menampilkan respons JSON bukan UI, pastikan Anda menjalankan `main.py` terbaru yang memasang file statis. Endpoint `/api/article` tetap berfungsi di jalur aslinya; pemasangan file statis menyajikan UI pada setiap rute lain.

**Cara kerja:** `main.py` yang diperbarui menambahkan satu baris di bagian bawah:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Ini menyajikan setiap file dari `zava-creative-writer-local/ui/` sebagai aset statis, dengan `index.html` sebagai dokumen default. Rute POST `/api/article` terdaftar sebelum pemasangan statis, sehingga memiliki prioritas.

---

### Latihan 2: Tambahkan Web Server ke Varian JavaScript

Varian JavaScript saat ini adalah aplikasi CLI (`main.mjs`). File baru, `server.mjs`, membungkus modul agen yang sama di belakang server HTTP dan menyajikan UI bersama.

**2.1** Masuk ke direktori JavaScript dan pasang dependensi:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Jalankan server web:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Anda akan melihat:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Buka `http://localhost:3000` di browser Anda dan klik **Generate Article**. UI yang sama berfungsi persis sama dengan backend JavaScript.

**Pelajari kode:** Buka `server.mjs` dan perhatikan pola utamanya:

- **Melayani file statis** menggunakan modul built-in Node.js `http`, `fs`, dan `path` tanpa framework eksternal.
- **Perlindungan traversal path** menormalkan path yang diminta dan memverifikasi tetap berada di dalam direktori `ui/`.
- **Streaming NDJSON** menggunakan helper `sendLine()` yang menyerialisasi setiap objek, menghapus baris baru internal, dan menambahkan baris baru di akhir.
- **Orkestrasi agen** menggunakan ulang modul `researcher.mjs`, `product.mjs`, `writer.mjs`, dan `editor.mjs` yang ada tanpa perubahan.

<details>
<summary>Cuplikan kunci dari server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Peneliti
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Penulis (menyiarkan)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Latihan 3: Tambahkan API Minimal ke Varian C#

Varian C# saat ini adalah aplikasi konsol. Proyek baru, `csharp-web`, menggunakan API minimal ASP.NET Core untuk mengekspos pipeline yang sama sebagai layanan web.

**3.1** Masuk ke proyek web C# dan pulihkan paket:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Jalankan server web:

```bash
dotnet run
```

```powershell
dotnet run
```

Anda akan melihat:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Buka `http://localhost:5000` di browser Anda dan klik **Generate Article**.

**Pelajari kode:** Buka `Program.cs` di direktori `csharp-web` dan perhatikan:

- File proyek menggunakan `Microsoft.NET.Sdk.Web` bukan `Microsoft.NET.Sdk`, yang menambahkan dukungan ASP.NET Core.
- File statis disajikan melalui `UseDefaultFiles` dan `UseStaticFiles` yang mengarah ke direktori `ui/` bersama.
- Endpoint `/api/article` menulis baris NDJSON langsung ke `HttpContext.Response` dan melakukan flush setelah setiap baris untuk streaming waktu nyata.
- Semua logika agen (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) sama seperti versi konsol.

<details>
<summary>Cuplikan kunci dari csharp-web/Program.cs</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### Latihan 4: Jelajahi Lencana Status Agen

Sekarang setelah Anda memiliki UI yang berfungsi, lihat bagaimana front end memperbarui lencana status.

**4.1** Buka `zava-creative-writer-local/ui/app.js` di editor Anda.

**4.2** Temukan fungsi `handleMessage()`. Perhatikan bagaimana ia memetakan tipe pesan ke pembaruan DOM:

| Jenis pesan | Aksi UI |
|-------------|-----------|
| `message` yang mengandung "researcher" | Mengatur lencana Researcher ke "Running" |
| `researcher` | Mengatur lencana Researcher ke "Done" dan mengisi panel Research Results |
| `marketing` | Mengatur lencana Product Search ke "Done" dan mengisi panel Product Matches |
| `writer` dengan `data.start` | Mengatur lencana Writer ke "Running" dan mengosongkan keluaran artikel |
| `partial` | Menambahkan teks token ke keluaran artikel |
| `writer` dengan `data.complete` | Mengatur lencana Writer ke "Done" |
| `editor` | Mengatur lencana Editor ke "Done" dan mengisi panel Editor Feedback |

**4.3** Buka panel yang dapat dilipat "Research Results", "Product Matches", dan "Editor Feedback" di bawah artikel untuk melihat JSON mentah yang dihasilkan setiap agen.

---

### Latihan 5: Sesuaikan UI (Tambahan)

Coba satu atau lebih peningkatan ini:

**5.1 Tambahkan jumlah kata.** Setelah Writer selesai, tampilkan jumlah kata artikel di bawah panel keluaran. Anda dapat menghitung ini di `handleMessage` saat `type === "writer"` dan `data.complete` bernilai true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Tambahkan indikator percobaan ulang.** Ketika Editor meminta revisi, pipeline berjalan ulang. Tampilkan banner "Revision 1" atau "Revision 2" di panel status. Dengarkan tipe `message` yang mengandung "Revision" dan perbarui elemen DOM baru.

**5.3 Mode gelap.** Tambahkan tombol toggle dan kelas `.dark` ke `<body>`. Timpahkan warna latar belakang, teks, dan panel di `style.css` menggunakan selektor `body.dark`.

---

## Ringkasan

| Apa yang Anda lakukan | Bagaimana |
|-------------|-----|
| Menyajikan UI dari backend Python | Memasang folder `ui/` dengan `StaticFiles` di FastAPI |
| Menambahkan server HTTP ke varian JavaScript | Membuat `server.mjs` menggunakan modul `http` bawaan Node.js |
| Menambahkan web API ke varian C# | Membuat proyek baru `csharp-web` dengan API minimal ASP.NET Core |
| Mengonsumsi streaming NDJSON di browser | Menggunakan `fetch()` dengan `ReadableStream` dan parsing JSON baris demi baris |
| Memperbarui UI secara real time | Memetakan tipe pesan ke pembaruan DOM (lencana, teks, panel lipat) |

---

## Poin Penting

1. **Front end statis bersama** dapat bekerja dengan backend mana pun yang berbicara protokol streaming yang sama, memperkuat nilai pola API kompatibel OpenAI.
2. **Newline-delimited JSON (NDJSON)** adalah format streaming sederhana yang bekerja secara native dengan API browser `ReadableStream`.
3. Varian **Python** memerlukan perubahan paling sedikit karena sudah memiliki endpoint FastAPI; varian JavaScript dan C# memerlukan pembungkus HTTP tipis.
4. Menjaga UI sebagai **vanilla HTML/CSS/JS** menghindari alat build, ketergantungan framework, dan kompleksitas tambahan untuk peserta workshop.
5. Modul agen yang sama (Researcher, Product, Writer, Editor) digunakan ulang tanpa perubahan; hanya lapisan transportasi yang berubah.

---

## Bacaan Lebih Lanjut

| Sumber | Link |
|----------|------|
| MDN: Menggunakan Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Spesifikasi NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Lanjutkan ke [Bagian 13: Workshop Selesai](part13-workshop-complete.md) untuk ikhtisar semua yang telah Anda bangun selama workshop ini.

---
[← Bagian 11: Pemanggilan Alat](part11-tool-calling.md) | [Bagian 13: Lokakarya Selesai →](part13-workshop-complete.md)