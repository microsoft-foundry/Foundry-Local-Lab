# Bölüm 7 için Örnek Ses Dosyaları - Whisper Ses Transkripsiyonu

Bu WAV dosyaları, `pyttsx3` (Windows SAPI5 metin-konuşma) kullanılarak oluşturulmuş ve Creative Writer demosundaki **Zava DIY ürünleri** temalıdır.

## Örnekleri oluşturun

```bash
# Depo kökünden - pyttsx3 yüklü .venv gerektirir
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Örnek dosyalar

| Dosya | Durum | Süre |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Müşterinin **Zava ProGrip Kablosuz Matkap** hakkında sorduğu sorular - tork, pil ömrü, taşıma çantası | ~15 sn |
| `zava-product-review.wav` | Müşterinin **Zava UltraSmooth İç Mekan Boyası** değerlendirmesi - kapatıcılık, kuruma süresi, düşük VOC | ~22 sn |
| `zava-support-call.wav` | Destek çağrısı, **Zava TitanLock Alet Çantası** hakkında - yedek anahtarlar, ekstra çekmece astarları | ~20 sn |
| `zava-project-planning.wav` | Bir DIY kullanıcısının arka bahçe güvertesi planlaması, **Zava EcoBoard Kompozit Güverte** ve BrightBeam ışıklarla | ~25 sn |
| `zava-workshop-setup.wav` | **Tüm beş Zava ürünü** kullanılarak tam bir atölye turu | ~32 sn |
| `zava-full-project-walkthrough.wav` | **Tüm Zava ürünleri** kullanılarak genişletilmiş garaj yenileme turu (uzun ses testi için bkz. [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 dk |

## Notlar

- WAV dosyaları **repoya dahil edilmiştir** (`. Bu sayfada yeni .wav dosyaları oluşturmak için yukarıdaki betiği çalıştırarak yeni betikler oluşturabilir veya düzenleyebilirsiniz.
- Betik, net transkripsiyon sonuçları için 160 WPM hızında **Microsoft David** (ABD İngilizcesi) sesi kullanır.
- Tüm senaryolar [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) dosyasındaki ürünlere referans verir.