![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagian 8: Pembangunan Dipacu Penilaian dengan Foundry Local

> **Matlamat:** Membina rangka kerja penilaian yang menguji dan menilai agen AI anda secara sistematik, menggunakan model tempatan yang sama sebagai agen yang diuji dan juga sebagai hakim, supaya anda boleh melakukan iterasi pada arahan dengan yakin sebelum penghantaran.

## Mengapa Pembangunan Dipacu Penilaian?

Apabila membina agen AI, "nampak betul" tidak mencukupi. **Pembangunan dipacu penilaian** menganggap output agen seperti kod: anda menulis ujian terlebih dahulu, mengukur kualiti, dan hanya menghantar apabila skor memenuhi ambang.

Dalam Zava Creative Writer (Bahagian 7), **agen Editor** sudah bertindak sebagai penilai ringan; ia memutuskan TERIMA atau SEMAK SEMULA. Makmal ini memformalkan corak tersebut menjadi rangka kerja penilaian yang boleh diulang yang boleh anda gunakan untuk mana-mana agen atau aliran kerja.

| Masalah | Penyelesaian |
|---------|-------------|
| Perubahan arahan merosakkan kualiti secara senyap | **Set data emas** mengesan regresi |
| Bias "Berfungsi pada satu contoh" | **Beberapa kes ujian** mendedahkan kes tepi |
| Penilaian kualiti subjektif | **Skor berasaskan peraturan + LLM sebagai hakim** memberikan nombor |
| Tiada cara untuk membandingkan varian arahan | **Jalankan penilaian sebelah menyebelah** dengan skor agregat |

---

## Konsep Utama

### 1. Set Data Emas

**Set data emas** ialah sekumpulan kes ujian yang dipilih dengan teliti dengan output yang diketahui dijangka. Setiap kes ujian termasuk:

- **Input**: Arahan atau soalan untuk dihantar kepada agen
- **Output dijangka**: Apa yang sepatutnya terkandung dalam jawapan yang betul atau berkualiti tinggi (kata kunci, struktur, fakta)
- **Kategori**: Kumpulan untuk pelaporan (contoh: "ketepatan fakta", "nada", "kesempurnaan")

### 2. Semakan Berasaskan Peraturan

Semakan pantas dan deterministik yang tidak memerlukan LLM:

| Semakan | Apa yang Diuji |
|---------|----------------|
| **Had panjang** | Respons tidak terlalu pendek (malas) atau terlalu panjang (berleluasa) |
| **Kata kunci diperlukan** | Respons menyebut istilah atau entiti yang dijangka |
| **Pengesahan format** | JSON boleh diurai, tajuk Markdown wujud |
| **Kandungan yang dilarang** | Tiada nama jenama halusinasi, tiada sebutan pesaing |

### 3. LLM-sebagai-Hakim

Gunakan **model tempatan yang sama** untuk menilai output sendiri (atau output dari varian arahan yang berbeza). Hakim menerima:

- Soalan asal
- Respons agen
- Kriteria penggredan

Dan mengembalikan skor berstruktur. Ini meniru corak Editor dari Bahagian 7 tetapi digunakan secara sistematik ke seluruh set ujian.

### 4. Gelung Iterasi Dipacu Penilaian

![Gelung iterasi dipacu penilaian](../../../images/eval-loop-flowchart.svg)

---

## Prasyarat

| Keperluan | Butiran |
|-----------|---------|
| **Foundry Local CLI** | Dipasang dengan model dimuat turun |
| **Persekitaran bahasa** | **Python 3.9+** dan/atau **Node.js 18+** dan/atau **.NET 9+ SDK** |
| **Selesai** | [Bahagian 5: Agen Tunggal](part5-single-agents.md) dan [Bahagian 6: Aliran Kerja Multi-Agen](part6-multi-agent-workflows.md) |

---

## Latihan Makmal

### Latihan 1 - Jalankan Rangka Kerja Penilaian

Bengkel ini termasuk contoh penilaian lengkap yang menguji agen Foundry Local terhadap set data emas soalan berkaitan Zava DIY.

<details>
<summary><strong>🐍 Python</strong></summary>

**Persediaan:**
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

**Apa yang berlaku:**
1. Berhubung dengan Foundry Local dan memuatkan model
2. Mendefinisikan set data emas dengan 5 kes ujian (soalan produk Zava)
3. Menjalankan dua varian arahan terhadap setiap kes ujian
4. Menilai setiap respons dengan **semakan berasaskan peraturan** (panjang, kata kunci, format)
5. Menilai setiap respons dengan **LLM-sebagai-hakim** (model yang sama memberi skor kualiti 1-5)
6. Mencetak kad skor membandingkan kedua-dua varian arahan

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Persediaan:**
```bash
cd javascript
npm install
```

**Jalankan:**
```bash
node foundry-local-eval.mjs
```

**Saluran penilaian sama:** set data emas, larian prompt berganda, penilaian berasaskan peraturan + LLM, kad skor.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Persediaan:**
```bash
cd csharp
dotnet restore
```

**Jalankan:**
```bash
dotnet run eval
```

**Saluran penilaian sama:** set data emas, larian prompt berganda, penilaian berasaskan peraturan + LLM, kad skor.

</details>

---

### Latihan 2 - Fahami Set Data Emas

Periksa kes ujian yang ditakrif dalam contoh penilaian. Setiap kes ujian mempunyai:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Soalan untuk difikirkan:**
1. Mengapa nilai dijangka adalah **kata kunci** dan bukan ayat penuh?
2. Berapa banyak kes ujian yang anda perlukan untuk penilaian yang boleh dipercayai?
3. Kategori apa yang anda akan tambah untuk aplikasi anda sendiri?

---

### Latihan 3 - Fahami Penilaian Berasaskan Peraturan vs LLM-sebagai-Hakim

Rangka kerja penilaian menggunakan **dua lapisan penilaian**:

#### Semakan Berasaskan Peraturan (pantat, deterministik)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-sebagai-Hakim (teliti, kualitatif)

Model tempatan yang sama bertindak sebagai hakim dengan rubrik berstruktur:

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

**Soalan untuk difikirkan:**
1. Bila anda akan mempercayai semakan berasaskan peraturan daripada LLM-sebagai-hakim?
2. Bolehkan sesebuah model menilai output sendiri dengan boleh dipercayai? Apakah hadnya?
3. Bagaimana ia dibandingkan dengan corak agen Editor dari Bahagian 7?

---

### Latihan 4 - Bandingkan Varian Prompt

Contoh menjalankan **dua varian arahan** terhadap kes ujian yang sama:

| Varian | Gaya Prompt Sistem |
|---------|-------------------|
| **Asas** | Am: "Anda ialah pembantu yang membantu" |
| **Khusus** | Terperinci: "Anda adalah pakar Zava DIY yang mengesyorkan produk tertentu dan menyediakan panduan langkah demi langkah" |

Selepas menjalankan, anda akan melihat kad skor seperti:

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
1. Jalankan penilaian dan catat skor setiap varian
2. Ubah arahan khusus supaya lebih spesifik. Adakah skor bertambah baik?
3. Tambah varian prompt ketiga dan bandingkan ketiga-tiganya.
4. Cuba tukar alias model (contoh `phi-4-mini` vs `phi-3.5-mini`) dan bandingkan hasil.

---

### Latihan 5 - Gunakan Penilaian untuk Agen Sendiri

Gunakan rangka kerja penilaian sebagai templat untuk agen anda sendiri:

1. **Tentukan set data emas anda**: tulis 5 hingga 10 kes ujian dengan kata kunci dijangka.
2. **Tulis arahan sistem anda**: arahan agen yang ingin anda uji.
3. **Jalankan penilaian**: dapatkan skor asas.
4. **Iterasi**: ubah suai arahan, jalankan semula, dan banding.
5. **Tetapkan ambang**: contoh "jangan hantar skor gabungan di bawah 0.75".

---

### Latihan 6 - Sambungan ke Corak Editor Zava

Lihat semula agen Editor Zava Creative Writer (`zava-creative-writer-local/src/api/agents/editor/editor.py`):

```python
# Penyunting adalah LLM-sebagai-hakim dalam produksi:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Ini adalah **konsep yang sama** seperti LLM-sebagai-hakim di Bahagian 8, tetapi tertanam dalam aliran kerja produksi dan bukan set ujian luar talian. Kedua-dua corak:

- Menggunakan output JSON berstruktur daripada model.
- Menerapkan kriteria kualiti yang ditetapkan dalam arahan sistem.
- Membuat keputusan lulus/gagal.

**Bezanya:** Editor dijalankan di produksi (pada setiap permintaan). Rangka kerja penilaian dijalankan semasa pembangunan (sebelum anda menghantar).

---

## Intisari Utama

| Konsep | Intisari |
|---------|---------|
| **Set data emas** | Pilih kes ujian awal; ia adalah ujian regresi anda untuk AI |
| **Semakan berasaskan peraturan** | Pantas, deterministik, dan mengesan kegagalan jelas (panjang, kata kunci, format) |
| **LLM-sebagai-hakim** | Penilaian kualiti teliti menggunakan model tempatan yang sama |
| **Perbandingan prompt** | Jalankan pelbagai varian terhadap set ujian yang sama untuk memilih terbaik |
| **Kelebihan di peranti** | Semua penilaian dijalankan secara tempatan: tiada kos API, tiada had kadar, tiada data keluar dari mesin anda |
| **Penilaian sebelum penghantaran** | Tetapkan ambang kualiti dan kawal pelepasan berdasarkan skor penilaian |

---

## Langkah Seterusnya

- **Skala naik**: Tambah lebih banyak kes ujian dan kategori dalam set data emas anda.
- **Automasi**: Integrasi penilaian ke dalam pipeline CI/CD anda.
- **Hakim lanjutan**: Gunakan model lebih besar sebagai hakim semasa menguji output model lebih kecil.
- **Jejak dari masa ke masa**: Simpan keputusan penilaian untuk dibandingkan antara versi prompt dan model.

---

## Makmal Seterusnya

Teruskan ke [Bahagian 9: Transkripsi Suara dengan Whisper](part9-whisper-voice-transcription.md) untuk meneroka penukaran suara ke teks di peranti menggunakan Foundry Local SDK.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil perhatian bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber yang sahih. Untuk maklumat kritikal, terjemahan manusia profesional adalah disyorkan. Kami tidak bertanggungjawab terhadap sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->