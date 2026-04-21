# Isu Dikenali — Bengkel Tempatan Foundry

Isu yang dihadapi semasa membina dan menguji bengkel ini pada peranti **Snapdragon X Elite (ARM64)** yang menjalankan Windows, dengan Foundry Local SDK v0.9.0, CLI v0.8.117, dan .NET SDK 10.0.

> **Disahkan terakhir:** 2026-03-11

---

## 1. CPU Snapdragon X Elite Tidak Dikenal oleh ONNX Runtime

**Status:** Terbuka  
**Keterukan:** Amaran (tidak menyekat)  
**Komponen:** ONNX Runtime / cpuinfo  
**Penghasilan Semula:** Setiap permulaan perkhidmatan Foundry Local pada perkakasan Snapdragon X Elite

Setiap kali perkhidmatan Foundry Local bermula, dua amaran dikeluarkan:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Kesan:** Amaran adalah secara kosmetik — inferens berfungsi dengan betul. Walau bagaimanapun, ia muncul setiap kali dijalankan dan boleh mengelirukan peserta bengkel. Perpustakaan ONNX Runtime cpuinfo perlu dikemas kini untuk mengenali teras CPU Qualcomm Oryon.

**Yang dijangka:** Snapdragon X Elite harus dikenali sebagai CPU ARM64 yang disokong tanpa mengeluarkan mesej tahap kesilapan.

---

## 2. NullReferenceException SingleAgent pada Larian Pertama

**Status:** Terbuka (berselang-seli)  
**Keterukan:** Kritikal (runtuh)  
**Komponen:** Foundry Local C# SDK + Microsoft Agent Framework  
**Penghasilan Semula:** Jalankan `dotnet run agent` — runtuh sejurus selepas model dimuatkan

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Konteks:** Baris 37 memanggil `model.IsCachedAsync(default)`. Runtuhan berlaku pada larian pertama agen selepas `foundry service stop` yang baru. Larian seterusnya dengan kod yang sama berjaya.

**Kesan:** Berselang-seli — mencadangkan masalah perlumbaan dalam inisialisasi perkhidmatan SDK atau pertanyaan katalog. Panggilan `GetModelAsync()` mungkin kembali sebelum perkhidmatan siap sepenuhnya.

**Yang dijangka:** `GetModelAsync()` harus sama ada menyekat sehingga perkhidmatan siap atau mengembalikan mesej ralat yang jelas jika perkhidmatan belum selesai inisialisasi.

---

## 3. SDK C# Memerlukan RuntimeIdentifier Secara Eksplisit

**Status:** Terbuka — dikesan dalam [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Keterukan:** Kekurangan dokumentasi  
**Komponen:** Pek NuGet `Microsoft.AI.Foundry.Local`  
**Penghasilan Semula:** Cipta projek .NET 8+ tanpa `<RuntimeIdentifier>` dalam `.csproj`

Pembinaan gagal dengan:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Punca utama:** Keperluan RID dijangka — SDK menghantar binari asli (P/Invoke ke dalam `Microsoft.AI.Foundry.Local.Core` dan ONNX Runtime), jadi .NET perlu mengetahui perpustakaan khusus platform mana yang hendak diselesaikan.

Ini didokumentasikan dalam MS Learn ([Cara menggunakan salinan sembang asli](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), di mana arahan jalankan menunjukkan:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Walau bagaimanapun, pengguna mesti mengingati bendera `-r` setiap kali, yang mudah terlupa.

**Penyelewengan:** Tambah fallback pengesanan automatik ke `.csproj` supaya `dotnet run` berfungsi tanpa sebarang bendera:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` adalah sifat MSBuild terbina dalam yang menyelesaikan RID mesin hos secara automatik. Projek ujian SDK sendiri sudah menggunakan corak ini. Bendera `-r` eksplisit masih dihormati apabila diberikan.

> **Nota:** `.csproj` bengkel termasuk fallback ini supaya `dotnet run` berfungsi secara terus pada mana-mana platform.

**Yang dijangka:** Templat `.csproj` dalam dokumen MS Learn harus memasukkan corak pengesanan automatik ini supaya pengguna tidak perlu mengingati bendera `-r`.

---

## 4. JavaScript Whisper — Transkripsi Audio Mengembalikan Output Kosong/Binari

**Status:** Terbuka (regresi — menjadi lebih teruk sejak laporan awal)  
**Keterukan:** Besar  
**Komponen:** Pelaksanaan JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Penghasilan Semula:** Jalankan `node foundry-local-whisper.mjs` — semua fail audio mengembalikan output kosong atau binari bukannya transkripsi teks

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Pada mulanya hanya fail audio ke-5 mengembalikan kosong; mulai v0.9.x, semua 5 fail mengembalikan satu bait (`\ufffd`) bukannya teks yang ditranskripsikan. Pelaksanaan Python Whisper menggunakan OpenAI SDK menyalin fail yang sama dengan betul.

**Yang dijangka:** `createAudioClient()` harus mengembalikan transkripsi teks yang sepadan dengan pelaksanaan Python/C#.

---

## 5. SDK C# Hanya Menghantar net8.0 — Tiada Sasaran Rasmi .NET 9 atau .NET 10

**Status:** Terbuka  
**Keterukan:** Kekurangan dokumentasi  
**Komponen:** Pek NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Arahan pemasangan:** `dotnet add package Microsoft.AI.Foundry.Local`

Pek NuGet hanya menghantar satu kerangka sasaran:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Tiada TFM `net9.0` atau `net10.0` disertakan. Berbeza dengan pek teman `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) yang menghantar `net8.0`, `net9.0`, `net10.0`, `net472`, dan `netstandard2.0`.

### Ujian Keserasian

| Kerangka Sasaran | Bina | Jalankan | Nota |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | Disokong secara rasmi |
| net9.0 | ✅ | ✅ | Dibina melalui keserasian hadapan — digunakan dalam sampel bengkel |
| net10.0 | ✅ | ✅ | Dibina dan dijalankan melalui keserasian hadapan dengan runtime .NET 10.0.3 |

Pemasangan net8.0 pada runtime yang lebih baru melalui mekanisme keserasian hadapan .NET, jadi binaan berjaya. Walau bagaimanapun, ini tidak didokumentasi dan belum diuji oleh pasukan SDK.

### Mengapa Sampel Menyasarkan net9.0

1. **.NET 9 adalah keluaran stabil terkini** — kebanyakan peserta bengkel akan memasangnya  
2. **Keserasian hadapan berfungsi** — pemasangan net8.0 dalam pek NuGet berjalan pada runtime .NET 9 tanpa isu  
3. **.NET 10 (pra-tonton/RC)** terlalu baru untuk disasarkan dalam bengkel yang perlu berfungsi untuk semua

**Yang dijangka:** Keluaran SDK akan datang harus mempertimbangkan untuk menambah TFM `net9.0` dan `net10.0` bersama `net8.0` untuk menepati corak yang digunakan oleh `Microsoft.Agents.AI.OpenAI` dan untuk menyediakan sokongan yang disahkan untuk runtime yang lebih baru.

---

## 6. Penstriman JavaScript ChatClient Menggunakan Callbacks, Bukan Async Iterators

**Status:** Terbuka  
**Keterukan:** Kekurangan dokumentasi  
**Komponen:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` yang dikembalikan oleh `model.createChatClient()` menyediakan kaedah `completeStreamingChat()`, tetapi ia menggunakan **corak callback** dan bukan mengembalikan iterable async:

```javascript
// ❌ Ini TIDAK berfungsi — menyebabkan ralat "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Corak betul — berikan callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Kesan:** Pembangun yang biasa dengan corak iterasi async OpenAI SDK (`for await`) akan menghadapi ralat mengelirukan. Callback mesti fungsi yang sah atau SDK melempar "Callback must be a valid function."

**Yang dijangka:** Dokumentasi corak callback dalam rujukan SDK. Alternatifnya, sokong corak iterable async untuk konsistensi dengan OpenAI SDK.

---

## Butiran Persekitaran

| Komponen | Versi |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Perkakasan | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil perhatian bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber yang sahih. Untuk maklumat yang kritikal, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab terhadap sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->