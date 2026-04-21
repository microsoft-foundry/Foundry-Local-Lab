# Masalah yang Dikenal — Foundry Local Workshop

Masalah yang ditemui saat membangun dan menguji workshop ini pada perangkat **Snapdragon X Elite (ARM64)** yang menjalankan Windows, dengan Foundry Local SDK v0.9.0, CLI v0.8.117, dan .NET SDK 10.0.

> **Terakhir divalidasi:** 2026-03-11

---

## 1. CPU Snapdragon X Elite Tidak Dikenali oleh ONNX Runtime

**Status:** Terbuka  
**Tingkat Keparahan:** Peringatan (tidak menghalangi)  
**Komponen:** ONNX Runtime / cpuinfo  
**Reproduksi:** Setiap kali layanan Foundry Local dimulai pada perangkat Snapdragon X Elite

Setiap kali layanan Foundry Local dimulai, dua peringatan dikeluarkan:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Dampak:** Peringatan ini bersifat kosmetik — inferensi berjalan dengan benar. Namun, mereka muncul setiap kali dijalankan dan dapat membingungkan peserta workshop. Perlu pembaruan pada pustaka cpuinfo ONNX Runtime agar dapat mengenali inti CPU Qualcomm Oryon.

**Yang Diharapkan:** Snapdragon X Elite harus dikenali sebagai CPU ARM64 yang didukung tanpa menghasilkan pesan kesalahan level error.

---

## 2. SingleAgent NullReferenceException pada Jalankan Pertama

**Status:** Terbuka (sporadis)  
**Tingkat Keparahan:** Kritis (crash)  
**Komponen:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reproduksi:** Jalankan `dotnet run agent` — crash segera setelah pemuatan model

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Konteks:** Baris 37 memanggil `model.IsCachedAsync(default)`. Crash terjadi pada saat jalankan pertama agen setelah `foundry service stop` yang baru. Jalankan berikutnya dengan kode yang sama berhasil.

**Dampak:** Sporadis — menunjukkan potensi kondisi balapan (race condition) dalam inisialisasi layanan SDK atau query katalog. Pemanggilan `GetModelAsync()` mungkin kembali sebelum layanan benar-benar siap.

**Yang Diharapkan:** `GetModelAsync()` harus memblokir sampai layanan siap atau mengembalikan pesan kesalahan yang jelas jika layanan belum selesai inisialisasi.

---

## 3. C# SDK Membutuhkan RuntimeIdentifier yang Eksplisit

**Status:** Terbuka — dilacak di [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Tingkat Keparahan:** Kekurangan dokumentasi  
**Komponen:** Paket NuGet `Microsoft.AI.Foundry.Local`  
**Reproduksi:** Buat proyek .NET 8+ tanpa `<RuntimeIdentifier>` dalam `.csproj`

Build gagal dengan:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Penyebab utama:** Persyaratan RID memang diharapkan — SDK mengemas binari native (P/Invoke ke `Microsoft.AI.Foundry.Local.Core` dan ONNX Runtime), sehingga .NET perlu mengetahui pustaka spesifik platform mana yang harus dipakai.

Hal ini terdokumentasi di MS Learn ([Cara menggunakan native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), di mana instruksi menjalankan menunjukkan:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Namun, pengguna harus selalu mengingat flag `-r` setiap kali, yang mudah terlupa.

**Solusi sementara:** Tambahkan fallback auto-deteksi ke `.csproj` Anda agar `dotnet run` bekerja tanpa flag apapun:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` adalah properti MSBuild bawaan yang secara otomatis menentukan RID mesin host. Proyek pengujian SDK sendiri sudah menggunakan pola ini. Flag `-r` yang eksplisit tetap dihormati bila disediakan.

> **Catatan:** `.csproj` workshop sudah memasukkan fallback ini sehingga `dotnet run` langsung berfungsi di semua platform.

**Yang Diharapkan:** Template `.csproj` di dokumentasi MS Learn harus menyertakan pola auto-deteksi ini agar pengguna tidak perlu ingat flag `-r`.

---

## 4. JavaScript Whisper — Transkripsi Audio Menghasilkan Output Kosong/Biner

**Status:** Terbuka (regresi — memburuk sejak laporan awal)  
**Tingkat Keparahan:** Besar  
**Komponen:** Implementasi JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reproduksi:** Jalankan `node foundry-local-whisper.mjs` — semua file audio mengembalikan output kosong atau biner bukan transkripsi teks

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Awalnya hanya file audio ke-5 yang mengembalikan kosong; per v0.9.x, semua 5 file mengembalikan satu byte (`\ufffd`) bukan teks transkripsi. Implementasi Python Whisper menggunakan SDK OpenAI menyalin file yang sama dengan benar.

**Yang Diharapkan:** `createAudioClient()` harus mengembalikan transkripsi teks yang cocok dengan implementasi Python/C#.

---

## 5. C# SDK Hanya Mengemas net8.0 — Tidak Ada Target .NET 9 atau .NET 10 Resmi

**Status:** Terbuka  
**Tingkat Keparahan:** Kekurangan dokumentasi  
**Komponen:** Paket NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Perintah instalasi:** `dotnet add package Microsoft.AI.Foundry.Local`

Paket NuGet hanya mengemas satu framework target:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
Tidak termasuk TFM `net9.0` atau `net10.0`. Sebaliknya, paket pendamping `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) menyertakan `net8.0`, `net9.0`, `net10.0`, `net472`, dan `netstandard2.0`.

### Pengujian Kompatibilitas

| Framework Target | Build | Run | Catatan |
|-----------------|-------|-----|---------|
| net8.0 | ✅ | ✅ | Resmi didukung |
| net9.0 | ✅ | ✅ | Build via forward-compat — dipakai di contoh workshop |
| net10.0 | ✅ | ✅ | Build dan jalankan via forward-compat dengan runtime .NET 10.0.3 |

Assembly net8.0 dapat dimuat di runtime yang lebih baru berkat mekanisme forward-compatibilitas .NET, sehingga build berhasil. Namun, hal ini tidak terdokumentasi dan belum diuji oleh tim SDK.

### Mengapa Contoh Menargetkan net9.0

1. **.NET 9 adalah rilis stabil terbaru** — mayoritas peserta workshop sudah menginstalnya  
2. **Forward compatibility bekerja** — assembly net8.0 dalam paket NuGet berjalan di runtime .NET 9 tanpa masalah  
3. **.NET 10 (preview/RC)** terlalu baru untuk ditargetkan dalam workshop yang harus kompatibel bagi semua orang

**Yang Diharapkan:** Rilis SDK mendatang mempertimbangkan penambahan TFM `net9.0` dan `net10.0` bersama `net8.0` agar sesuai pola yang digunakan `Microsoft.Agents.AI.OpenAI` dan memberikan dukungan yang tervalidasi untuk runtime baru.

---

## 6. JavaScript ChatClient Streaming Menggunakan Callback, Bukan Async Iterator

**Status:** Terbuka  
**Tingkat Keparahan:** Kekurangan dokumentasi  
**Komponen:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`

`ChatClient` yang dikembalikan oleh `model.createChatClient()` menyediakan metode `completeStreamingChat()`, tetapi menggunakan **pola callback** dan bukan mengembalikan async iterable:

```javascript
// ❌ Ini TIDAK bekerja — melempar "stream bukan iterable async"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Pola yang benar — berikan callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Dampak:** Pengembang yang biasa dengan pola iterasi async OpenAI SDK (`for await`) akan mengalami error membingungkan. Callback harus berupa fungsi valid atau SDK melempar kesalahan "Callback must be a valid function."

**Yang Diharapkan:** Dokumentasikan pola callback ini dalam referensi SDK. Atau, dukung pola async iterable agar konsisten dengan OpenAI SDK.

---

## Rincian Lingkungan

| Komponen | Versi |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Hardware | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |