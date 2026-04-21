# Fail Audio Sampel untuk Bahagian 7 - Transkripsi Suara Whisper

Fail WAV ini dijana menggunakan `pyttsx3` (Windows SAPI5 teks-ke-ucapan) dan bertemakan produk **Zava DIY** dari demo Creative Writer.

## Jana sampel

```bash
# Dari akar repositori - memerlukan .venv dengan pyttsx3 dipasang
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Fail sampel

| Fail | Senario | Tempoh |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Pelanggan bertanya tentang **Zava ProGrip Cordless Drill** - tork, hayat bateri, bekas pembawa | ~15 saat |
| `zava-product-review.wav` | Pelanggan mengulas tentang **Zava UltraSmooth Interior Paint** - liputan, masa pengeringan, VOC rendah | ~22 saat |
| `zava-support-call.wav` | Panggilan sokongan tentang **Zava TitanLock Tool Chest** - kunci gantian, pelapik laci tambahan | ~20 saat |
| `zava-project-planning.wav` | DIYer merancang dek halaman belakang dengan **Zava EcoBoard Composite Decking** & lampu BrightBeam | ~25 saat |
| `zava-workshop-setup.wav` | Rundingan bengkel lengkap menggunakan **semua lima produk Zava** | ~32 saat |
| `zava-full-project-walkthrough.wav` | Rundingan pengubahsuaian garaj lanjutan menggunakan **semua produk Zava** (untuk ujian audio panjang, lihat [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Nota

- Fail WAV **ditambah** dalam repo (tersenarai di `. Jalankan skrip di atas untuk menghasilkan skrip baru .wav atau ubahsuai untuk buat skrip baru.
- Skrip menggunakan suara **Microsoft David** (Bahasa Inggeris AS) pada 160 WPM untuk keputusan transkripsi yang jelas.
- Semua senario merujuk produk dari [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila maklum bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber utama yang sahih. Untuk maklumat penting, disarankan menggunakan terjemahan profesional oleh manusia. Kami tidak bertanggungjawab atas sebarang salah faham atau salah tafsir yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->