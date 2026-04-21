![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagian 12: Membangun UI Web untuk Zava Creative Writer

> **Matlamat:** Tambah hadapan berasaskan pelayar kepada Zava Creative Writer supaya anda boleh menonton saluran multi-ejen berjalan secara masa nyata, dengan lencana status ejen secara langsung dan teks artikel yang distrim, semuanya dihidangkan dari satu pelayan web tempatan.

Dalam [Bahagian 7](part7-zava-creative-writer.md) anda telah meneroka Zava Creative Writer sebagai **aplikasi CLI** (JavaScript, C#) dan **API tanpa kepala** (Python). Dalam makmal ini anda akan menyambungkan hadapan **vanila HTML/CSS/JavaScript** yang dikongsi kepada setiap backend supaya pengguna boleh berinteraksi dengan saluran melalui pelayar dan bukan terminal.

---

## Apa yang Akan Anda Pelajari

| Objektif | Penerangan |
|-----------|-------------|
| Hidangkan fail statik dari backend | Pasang direktori HTML/CSS/JS di sebelah laluan API anda |
| Gunakan streaming NDJSON dalam pelayar | Gunakan Fetch API dengan `ReadableStream` untuk membaca JSON yang dibahagikan baris |
| Protokol penstriman penyatuan | Pastikan backend Python, JavaScript, dan C# menghasilkan format mesej yang sama |
| Kemas kini UI secara progresif | Kemas kini lencana status ejen dan strimkan teks artikel token demi token |
| Tambah lapisan HTTP ke aplikasi CLI | Bungkus logik pengaturcara sedia ada dalam pelayan gaya Express (JS) atau API minimal ASP.NET Core (C#) |

---

## Seni Bina

UI adalah satu set fail statik tunggal (`index.html`, `style.css`, `app.js`) yang dikongsi oleh ketiga-tiga backend. Setiap backend mendedahkan dua laluan yang sama:

![Seni Bina UI Zava — hadapan dikongsi dengan tiga backend](../../../images/part12-architecture.svg)

| Laluan | Kaedah | Tujuan |
|-------|--------|---------|
| `/` | GET | Menghidangkan UI statik |
| `/api/article` | POST | Menjalankan saluran pelbagai ejen dan menstrim NDJSON |

Hadapan menghantar badan JSON dan membaca tindak balas sebagai aliran mesej JSON berbaris. Setiap mesej mempunyai medan `type` yang digunakan UI untuk mengemas kini panel yang betul:

| Jenis mesej | Maksud |
|-------------|---------|
| `message` | Kemas kini status (contoh "Memulakan tugas ejen penyelidik...") |
| `researcher` | Keputusan penyelidikan sudah sedia |
| `marketing` | Keputusan carian produk sudah sedia |
| `writer` | Penulis bermula atau selesai (mengandungi `{ start: true }` atau `{ complete: true }`) |
| `partial` | Token tunggal distrim dari Penulis (mengandungi `{ text: "..." }`) |
| `editor` | Keputusan editor sudah sedia |
| `error` | Sesuatu tidak kena |

![Penghalaan jenis mesej dalam pelayar](../../../images/part12-message-types.svg)

![Urusan penstriman — Komunikasi Pelayar ke Backend](../../../images/part12-streaming-sequence.svg)

---

## Prasyarat

- Lengkapkan [Bahagian 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI dipasang dan model `phi-3.5-mini` dimuat turun
- Pelayar web moden (Chrome, Edge, Firefox, atau Safari)

---

## UI yang Dikongsi

Sebelum menyentuh mana-mana kod backend, luangkan masa untuk meneroka hadapan yang akan digunakan oleh ketiga-tiga laluan bahasa. Fail-fail ini terletak di `zava-creative-writer-local/ui/`:

| Fail | Tujuan |
|------|---------|
| `index.html` | Susun atur halaman: borang input, lencana status ejen, kawasan output artikel, panel butiran boleh lipat |
| `style.css` | Gaya minimal dengan warna lencana status (menunggu, berjalan, selesai, ralat) |
| `app.js` | Panggilan fetch, pembaca baris `ReadableStream`, dan logik kemas kini DOM |

> **Petua:** Buka `index.html` terus di pelayar anda untuk pratonton susun atur. Tiada apa akan berfungsi lagi kerana tiada backend, tetapi anda boleh lihat strukturnya.

### Bagaimana Pembaca Aliran Berfungsi

Fungsi utama dalam `app.js` membaca badan tindak balas sebahagian demi sebahagian dan memisahkannya pada sempadan baris baru:

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
    buffer = lines.pop(); // simpan baris yang tergantung tidak lengkap

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

Setiap mesej yang diurai dihantar ke `handleMessage()`, yang mengemas kini elemen DOM yang berkaitan berdasarkan `msg.type`.

---

## Latihan

### Latihan 1: Jalankan Backend Python dengan UI

Varian Python (FastAPI) sudah mempunyai titik akhir API penstriman. Perubahan sahaja ialah memasang folder `ui/` sebagai fail statik.

**1.1** Pergi ke direktori API Python dan pasang kebergantungan:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Mulakan pelayan:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Buka pelayar anda di `http://localhost:8000`. Anda harus melihat UI Zava Creative Writer dengan tiga medan teks dan butang "Generate Article".

**1.4** Klik **Generate Article** menggunakan nilai lalai. Perhatikan lencana status ejen berubah daripada "Menunggu" ke "Berjalan" kepada "Selesai" apabila setiap ejen menyelesaikan tugas, dan lihat teks artikel distrim ke panel output token demi token.

> **Penyelesaian Masalah:** Jika halaman memaparkan tindak balas JSON bukannya UI, pastikan anda menjalankan `main.py` yang dikemas kini yang memasang fail statik. Titik akhir `/api/article` masih berfungsi pada laluan asalnya; pemasangan fail statik menghidangkan UI pada laluan lain.

**Bagaimana ia berfungsi:** `main.py` yang dikemas kini menambah satu baris di bahagian bawah:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Ini menghidangkan setiap fail dari `zava-creative-writer-local/ui/` sebagai aset statik, dengan `index.html` sebagai dokumen lalai. Laluan POST `/api/article` didaftarkan sebelum pemasangan statik, jadi ia diutamakan.

---

### Latihan 2: Tambah Pelayan Web kepada Varian JavaScript

Varian JavaScript sekarang adalah aplikasi CLI (`main.mjs`). Fail baru, `server.mjs`, membungkus modul ejen yang sama di belakang pelayan HTTP dan menghidangkan UI yang dikongsi.

**2.1** Pergi ke direktori JavaScript dan pasang kebergantungan:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Mulakan pelayan web:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Anda harus melihat:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Buka `http://localhost:3000` di pelayar dan klik **Generate Article**. UI yang sama berfungsi sama dengan backend JavaScript.

**Kajilah kod:** Buka `server.mjs` dan perhatikan corak utama:

- **Hidangan fail statik** menggunakan modul Node.js terbina dalam `http`, `fs`, dan `path` tanpa kerangka kerja luaran.
- **Perlindungan laluan traversal** menormalkan laluan yang diminta dan mengesahkan ia kekal dalam direktori `ui/`.
- **Penstriman NDJSON** menggunakan pembantu `sendLine()` yang menyahserial objek, menghilangkan baris baru dalaman, dan menambah baris baru di hujungnya.
- **Pengaturcaraan ejen** menggunakan semula modul `researcher.mjs`, `product.mjs`, `writer.mjs`, dan `editor.mjs` sedia ada tanpa berubah.

<details>
<summary Petikan utama dari server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Penyelidik
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Penulis (penstriman)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Latihan 3: Tambah API Minimal kepada Varian C#

Varian C# sekarang adalah aplikasi konsol. Projek baru, `csharp-web`, menggunakan API minimal ASP.NET Core untuk mendedahkan saluran yang sama sebagai perkhidmatan web.

**3.1** Pergi ke projek web C# dan pulihkan pakej:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Jalankan pelayan web:

```bash
dotnet run
```

```powershell
dotnet run
```

Anda harus melihat:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Buka `http://localhost:5000` di pelayar dan klik **Generate Article**.

**Kajilah kod:** Buka `Program.cs` dalam direktori `csharp-web` dan perhatikan:

- Fail projek menggunakan `Microsoft.NET.Sdk.Web` bukannya `Microsoft.NET.Sdk`, yang menambah sokongan ASP.NET Core.
- Fail statik dihidangkan melalui `UseDefaultFiles` dan `UseStaticFiles` yang menunjuk ke direktori `ui/` yang dikongsi.
- Titik akhir `/api/article` menulis baris NDJSON terus ke `HttpContext.Response` dan membilas selepas setiap baris untuk penstriman masa nyata.
- Semua logik ejen (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) sama seperti versi konsol.

<details>
<summary Petikan utama dari csharp-web/Program.cs</summary>

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

### Latihan 4: Terokai Lencana Status Ejen

Sekarang anda mempunyai UI berfungsi, lihat bagaimana hadapan mengemas kini lencana status.

**4.1** Buka `zava-creative-writer-local/ui/app.js` dalam penyunting anda.

**4.2** Cari fungsi `handleMessage()`. Perhatikan bagaimana ia memetakan jenis mesej kepada kemas kini DOM:

| Jenis mesej | Tindakan UI |
|-------------|-------------|
| `message` yang mengandungi "researcher" | Tetapkan lencana Researcher kepada "Berjalan" |
| `researcher` | Tetapkan lencana Researcher kepada "Selesai" dan isi panel Keputusan Penyelidikan |
| `marketing` | Tetapkan lencana Product Search kepada "Selesai" dan isi panel Padanan Produk |
| `writer` dengan `data.start` | Tetapkan lencana Writer kepada "Berjalan" dan kosongkan output artikel |
| `partial` | Tambah teks token ke output artikel |
| `writer` dengan `data.complete` | Tetapkan lencana Writer kepada "Selesai" |
| `editor` | Tetapkan lencana Editor kepada "Selesai" dan isi panel Maklum Balas Editor |

**4.3** Buka panel boleh lipat "Keputusan Penyelidikan", "Padanan Produk", dan "Maklum Balas Editor" di bawah artikel untuk memeriksa JSON mentah yang dihasilkan setiap ejen.

---

### Latihan 5: Sesuaikan UI (Pilihan)

Cuba satu atau lebih penambahbaikan ini:

**5.1 Tambah kiraan perkataan.** Selepas Penulis selesai, paparkan kiraan perkataan artikel di bawah panel output. Anda boleh mengira ini dalam `handleMessage` apabila `type === "writer"` dan `data.complete` adalah benar:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Tambah penunjuk cuba semula.** Apabila Editor meminta semakan, saluran berjalan semula. Paparkan banner "Revision 1" atau "Revision 2" dalam panel status. Dengar untuk jenis `message` yang mengandungi "Revision" dan kemas kini elemen DOM baru.

**5.3 Mod gelap.** Tambah butang togol dan kelas `.dark` pada `<body>`. Tulis semula warna latar belakang, teks, dan panel dalam `style.css` dengan pemilih `body.dark`.

---

## Rumusan

| Apa yang anda buat | Bagaimana |
|--------------------|-----------|
| Hidangkan UI dari backend Python | Pasang folder `ui/` dengan `StaticFiles` dalam FastAPI |
| Tambah pelayan HTTP kepada varian JavaScript | Cipta `server.mjs` menggunakan modul terbina dalam Node.js `http` |
| Tambah API web kepada varian C# | Cipta projek `csharp-web` baru dengan API minimal ASP.NET Core |
| Gunakan streaming NDJSON dalam pelayar | Guna `fetch()` dengan `ReadableStream` dan parsing JSON baris demi baris |
| Kemas kini UI secara masa nyata | Petakan jenis mesej ke kemas kini DOM (lencana, teks, panel boleh lipat) |

---

## Poin Penting

1. **Hadapan statik berkongsi** boleh berfungsi dengan mana-mana backend yang bercakap protokol penstriman yang sama, mengukuhkan nilai corak API yang serasi OpenAI.
2. **Newline-delimited JSON (NDJSON)** adalah format penstriman lancar yang berfungsi secara asli dengan API `ReadableStream` pelayar.
3. Varian **Python** memerlukan perubahan paling sedikit kerana sudah ada titik akhir FastAPI; varian JavaScript dan C# memerlukan pembungkus HTTP nipis.
4. Menjaga UI sebagai **vanilla HTML/CSS/JS** mengelakkan alat bina, kebergantungan kerangka, dan kerumitan tambahan untuk pelajar bengkel.
5. Modul ejen yang sama (Researcher, Product, Writer, Editor) digunakan semula tanpa diubah; hanya lapisan penghantaran yang berubah.

---

## Bacaan Lanjut

| Sumber | Pautan |
|--------|--------|
| MDN: Menggunakan Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Fail Statik | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Fail Statik | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Spesifikasi NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Teruskan ke [Bahagian 13: Bengkel Selesai](part13-workshop-complete.md) untuk rumusan segala yang anda telah bina sepanjang bengkel ini.

---
[← Bahagian 11: Panggilan Alat](part11-tool-calling.md) | [Bahagian 13: Bengkel Selesai →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil perhatian bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya hendaklah dianggap sebagai sumber yang sahih. Untuk maklumat penting, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab atas sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->