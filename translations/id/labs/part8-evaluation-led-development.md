![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bagian 8: Pengembangan Berbasis Evaluasi dengan Foundry Local

> **Tujuan:** Membangun kerangka evaluasi yang secara sistematis menguji dan memberi skor pada agen AI Anda, menggunakan model lokal yang sama baik sebagai agen yang diuji maupun sebagai hakim, sehingga Anda dapat mengulangi prompt dengan percaya diri sebelum mengirimkan.

## Mengapa Pengembangan Berbasis Evaluasi?

Saat membangun agen AI, "terlihat cukup benar" tidaklah cukup. **Pengembangan berbasis evaluasi** memperlakukan output agen seperti kode: Anda menulis tes terlebih dahulu, mengukur kualitas, dan hanya mengirimkan ketika skor memenuhi ambang batas.

Dalam Zava Creative Writer (Bagian 7), **agen Editor** sudah bertindak sebagai evaluator ringan; ia memutuskan TERIMA atau REVISI. Lab ini meresmikan pola itu menjadi kerangka evaluasi yang dapat diulang yang bisa Anda terapkan ke agen atau pipeline mana pun.

| Masalah | Solusi |
|---------|----------|
| Perubahan prompt merusak kualitas secara diam-diam | **Dataset emas** menangkap regresi |
| Bias "Bekerja pada satu contoh" | **Beberapa kasus uji** mengungkap kasus pinggiran |
| Penilaian kualitas yang subjektif | **Aturan + penilaian LLM-sebagai-hakim** memberikan angka |
| Tidak ada cara untuk membandingkan varian prompt | **Jalankan evaluasi berdampingan** dengan skor agregat |

---

## Konsep Utama

### 1. Dataset Emas

**Dataset emas** adalah seperangkat kasus uji yang dikurasi dengan output yang diharapkan diketahui. Setiap kasus uji mencakup:

- **Input**: Prompt atau pertanyaan yang dikirim ke agen
- **Output yang diharapkan**: Apa yang harus dikandung respons yang benar atau berkualitas tinggi (kata kunci, struktur, fakta)
- **Kategori**: Pengelompokan untuk pelaporan (mis. "akurasi faktual", "nada", "kelengkapan")

### 2. Pemeriksaan Berbasis Aturan

Pemeriksaan cepat dan deterministik yang tidak memerlukan LLM:

| Pemeriksaan | Apa yang Diuji |
|-------------|----------------|
| **Batas panjang** | Respons tidak terlalu pendek (malas) atau terlalu panjang (bertele-tele) |
| **Kata kunci wajib** | Respons menyebutkan istilah atau entitas yang diharapkan |
| **Validasi format** | JSON dapat diurai, header Markdown ada |
| **Konten terlarang** | Tidak ada nama merek halusinasi, tidak ada sebutan pesaing |

### 3. LLM-sebagai-Hakim

Gunakan **model lokal yang sama** untuk menilai outputnya sendiri (atau output dari varian prompt berbeda). Hakim menerima:

- Pertanyaan asli
- Respons agen
- Kriteria penilaian

Dan mengembalikan skor terstruktur. Ini mencerminkan pola Editor dari Bagian 7 tetapi diterapkan secara sistematis di seluruh suite uji.

### 4. Loop Iterasi Berbasis Evaluasi

![Eval-driven iteration loop](../../../images/eval-loop-flowchart.svg)

---

## Prasyarat

| Persyaratan | Detail |
|-------------|---------|
| **Foundry Local CLI** | Terpasang dengan model yang diunduh |
| **Runtime bahasa** | **Python 3.9+** dan/atau **Node.js 18+** dan/atau **.NET 9+ SDK** |
| **Selesai** | [Bagian 5: Agen Tunggal](part5-single-agents.md) dan [Bagian 6: Alur Kerja Multi-Agen](part6-multi-agent-workflows.md) |

---

## Latihan Lab

### Latihan 1 - Jalankan Kerangka Evaluasi

Workshop ini mencakup sampel evaluasi lengkap yang menguji agen Foundry Local terhadap dataset emas pertanyaan terkait Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Setup:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Jalankan:**
```bash
python foundry-local-eval.py
```

**Apa yang terjadi:**
1. Terhubung ke Foundry Local dan memuat model
2. Mendefinisikan dataset emas dengan 5 kasus uji (pertanyaan produk Zava)
3. Menjalankan dua varian prompt terhadap setiap kasus uji
4. Memberi skor setiap respons dengan **pemeriksaan berbasis aturan** (panjang, kata kunci, format)
5. Memberi skor setiap respons dengan **LLM-sebagai-hakim** (model yang sama memberi nilai kualitas 1-5)
6. Mencetak kartu skor yang membandingkan kedua varian prompt

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Setup:**
```bash
cd javascript
npm install
```

**Jalankan:**
```bash
node foundry-local-eval.mjs
```

**Pipeline evaluasi yang sama:** dataset emas, dua kali jalankan prompt, penilaian berbasis aturan + LLM, kartu skor.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Setup:**
```bash
cd csharp
dotnet restore
```

**Jalankan:**
```bash
dotnet run eval
```

**Pipeline evaluasi yang sama:** dataset emas, dua kali jalankan prompt, penilaian berbasis aturan + LLM, kartu skor.

</details>

---

### Latihan 2 - Pahami Dataset Emas

Periksa kasus uji yang didefinisikan dalam sampel evaluasi. Setiap kasus uji memiliki:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Pertanyaan yang perlu dipertimbangkan:**
1. Mengapa nilai yang diharapkan adalah **kata kunci** daripada kalimat lengkap?
2. Berapa banyak kasus uji yang diperlukan untuk evaluasi yang andal?
3. Kategori apa yang akan Anda tambahkan untuk aplikasi Anda sendiri?

---

### Latihan 3 - Pahami Penilaian Berbasis Aturan vs LLM-sebagai-Hakim

Kerangka evaluasi menggunakan **dua lapisan penilaian**:

#### Pemeriksaan Berbasis Aturan (cepat, deterministik)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-sebagai-Hakim (bernuansa, kualitatif)

Model lokal yang sama bertindak sebagai hakim dengan rubrik terstruktur:

```
Rate this response on a scale of 1-5:
- 1: Completely wrong or irrelevant
- 2: Partially correct but missing key information
- 3: Adequate but could be improved
- 4: Good response with minor issues
- 5: Excellent, comprehensive, well-structured

Score: 4
Reasoning: The response correctly identifies all necessary tools
and provides practical advice, but could include safety equipment.
```

**Pertanyaan yang perlu dipertimbangkan:**
1. Kapan Anda akan mempercayai pemeriksaan berbasis aturan daripada LLM-sebagai-hakim?
2. Apakah sebuah model dapat menilai outputnya sendiri dengan andal? Apa keterbatasannya?
3. Bagaimana ini dibandingkan dengan pola agen Editor dari Bagian 7?

---

### Latihan 4 - Bandingkan Varian Prompt

Sampel menjalankan **dua varian prompt** terhadap kasus uji yang sama:

| Varian | Gaya System Prompt |
|---------|-------------------|
| **Baseline** | Umum: "Anda adalah asisten yang membantu" |
| **Spesialis** | Detail: "Anda adalah ahli Zava DIY yang merekomendasikan produk spesifik dan memberikan panduan langkah demi langkah" |

Setelah menjalankan, Anda akan melihat kartu skor seperti:

```
╔══════════════════════════════════════════════════════════════╗
║                    EVALUATION SCORECARD                      ║
╠══════════════════════════════════════════════════════════════╣
║ Prompt Variant    │ Rule Score │ LLM Score │ Combined       ║
╠═══════════════════╪════════════╪═══════════╪════════════════╣
║ Baseline          │ 0.62       │ 3.2 / 5   │ 0.62           ║
║ Specialised       │ 0.81       │ 4.1 / 5   │ 0.81           ║
╚══════════════════════════════════════════════════════════════╝
```

**Latihan:**
1. Jalankan evaluasi dan catat skor untuk setiap varian
2. Modifikasi prompt spesialis agar lebih spesifik. Apakah skor meningkat?
3. Tambahkan varian prompt ketiga dan bandingkan ketiganya.
4. Coba ubah alias model (misal `phi-4-mini` vs `phi-3.5-mini`) dan bandingkan hasil.

---

### Latihan 5 - Terapkan Evaluasi pada Agen Anda Sendiri

Gunakan kerangka evaluasi sebagai template untuk agen Anda sendiri:

1. **Definisikan dataset emas Anda**: tulis 5 sampai 10 kasus uji dengan kata kunci yang diharapkan.
2. **Tulis system prompt Anda**: instruksi untuk agen yang ingin Anda uji.
3. **Jalankan evaluasi**: dapatkan skor baseline.
4. **Iterasi**: sesuaikan prompt, jalankan ulang, dan bandingkan.
5. **Tetapkan ambang batas**: misalnya "jangan kirim jika skor gabungan di bawah 0,75".

---

### Latihan 6 - Koneksi dengan Pola Editor Zava

Lihat kembali pada agen Editor Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Editor adalah LLM-sebagai-hakim yang sedang diproduksi:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Ini adalah **konsep yang sama** dengan LLM-sebagai-hakim di Bagian 8, tetapi tertanam di pipeline produksi alih-alih dalam suite uji offline. Kedua pola:

- Menggunakan keluaran JSON terstruktur dari model.
- Menerapkan kriteria kualitas yang didefinisikan dalam system prompt.
- Membuat keputusan lulus/gagal.

**Perbedaannya:** Editor berjalan di produksi (pada setiap permintaan). Kerangka evaluasi berjalan di pengembangan (sebelum Anda kirim).

---

## Poin-Poin Penting

| Konsep | Intisari |
|---------|----------|
| **Dataset emas** | Kurasi kasus uji sejak awal; ini adalah tes regresi Anda untuk AI |
| **Pemeriksaan berbasis aturan** | Cepat, deterministik, dan menangkap kegagalan jelas (panjang, kata kunci, format) |
| **LLM-sebagai-hakim** | Penilaian kualitas bernuansa menggunakan model lokal yang sama |
| **Perbandingan prompt** | Jalankan beberapa varian terhadap suite uji yang sama untuk memilih yang terbaik |
| **Keunggulan on-device** | Semua evaluasi berjalan lokal: tanpa biaya API, tanpa batasan kecepatan, tanpa data keluar dari mesin Anda |
| **Eval-sebelum-kirim** | Tetapkan ambang kualitas dan kendalikan rilis berdasarkan skor evaluasi |

---

## Langkah Berikutnya

- **Skalakan**: Tambah kasus uji dan kategori ke dataset emas Anda.
- **Otomatisasi**: Integrasikan evaluasi ke pipeline CI/CD Anda.
- **Hakim canggih**: Gunakan model yang lebih besar sebagai hakim saat menguji output model yang lebih kecil.
- **Lacak dari waktu ke waktu**: Simpan hasil evaluasi untuk membandingkan versi prompt dan model.

---

## Lab Berikutnya

Lanjutkan ke [Bagian 9: Transkripsi Suara dengan Whisper](part9-whisper-voice-transcription.md) untuk mengeksplorasi ucapan-ke-teks secara lokal menggunakan Foundry Local SDK.