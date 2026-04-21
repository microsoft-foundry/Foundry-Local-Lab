# Contoh File Audio untuk Bagian 7 - Transkripsi Suara Whisper

File WAV ini dibuat menggunakan `pyttsx3` (text-to-speech Windows SAPI5) dan bertema seputar **produk Zava DIY** dari demo Creative Writer.

## Membuat sampel

```bash
# Dari akar repositori - membutuhkan .venv dengan pyttsx3 terpasang
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## File Sampel

| File | Skenario | Durasi |
|------|----------|--------|
| `zava-customer-inquiry.wav` | Pelanggan bertanya tentang **Zava ProGrip Cordless Drill** - torsi, masa pakai baterai, tas pembawa | ~15 detik |
| `zava-product-review.wav` | Pelanggan meninjau **Zava UltraSmooth Interior Paint** - cakupan, waktu kering, VOC rendah | ~22 detik |
| `zava-support-call.wav` | Panggilan dukungan tentang **Zava TitanLock Tool Chest** - kunci pengganti, pelapis laci tambahan | ~20 detik |
| `zava-project-planning.wav` | DIYer merencanakan dek halaman belakang dengan **Zava EcoBoard Composite Decking** & lampu BrightBeam | ~25 detik |
| `zava-workshop-setup.wav` | Penjelasan lengkap tentang bengkel menggunakan **semua lima produk Zava** | ~32 detik |
| `zava-full-project-walkthrough.wav` | Penjelasan panjang renovasi garasi menggunakan **semua produk Zava** (untuk pengujian audio panjang, lihat [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 menit |

## Catatan

- File WAV telah **ditambahkan** ke repositori (tercantum di `. Untuk membuat file .wav baru jalankan skrip di atas untuk menghasilkan skrip baru atau ubah untuk membuat skrip baru.
- Skrip menggunakan suara **Microsoft David** (Bahasa Inggris AS) pada 160 WPM untuk hasil transkripsi yang jelas.
- Semua skenario merujuk ke produk dari [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).