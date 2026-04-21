![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bölüm 8: Foundry Local ile Değerlendirme Odaklı Geliştirme

> **Amaç:** Test edilen ajan ve hakem olarak aynı yerel modeli kullanarak AI ajanlarınızı sistematik bir şekilde test eden ve puanlayan bir değerlendirme çerçevesi oluşturmak, böylece teslim etmeden önce tekliflerle ilgili özgüvenle iterasyon yapabilmek.

## Değerlendirme Odaklı Geliştirme Neden?

AI ajanları oluştururken, "görünüşte doğru" yeterli değildir. **Değerlendirme odaklı geliştirme**, ajan çıktılarıyla kod gibi ilgilenir: önce testler yazarsınız, kaliteyi ölçersiniz ve sadece puanlar eşik değerini geçtiğinde teslimatı yaparsınız.

Zava Creative Writer’da (Bölüm 7), **Editör ajanı** zaten hafif bir değerlendirici gibi davranmaktadır; KABUL veya DÜZELT kararını verir. Bu laboratuvar, bu deseni herhangi bir ajan veya iş akışına uygulayabileceğiniz tekrarlanabilir bir değerlendirme çerçevesine resmileştirir.

| Sorun | Çözüm |
|---------|----------|
| Tek bir örnekte yapılan değişiklikler kaliteyi sessizce bozar | **Altın veri seti** gerilemeleri yakalar |
| "Bir örnekte çalışıyor" önyargısı | **Birden çok test durumu** köşe durumları ortaya çıkarır |
| Öznel kalite değerlendirmesi | **Kurallara dayalı + LLM-hakem puanlaması** rakamlar sağlar |
| Teklif varyantlarını karşılaştırma yok | **Yanyana değerlendirme çalıştırmaları** ile toplu puanlar |

---

## Ana Kavramlar

### 1. Altın Veri Setleri

**Altın veri seti**, bilinen beklenen çıktılara sahip özenle seçilmiş test durumları setidir. Her test durumu şunları içerir:

- **Girdi**: Ajanın cevaplaması için gönderilen teklif veya soru
- **Beklenen çıktı**: Doğru veya yüksek kaliteli yanıtın içermesi gerekenler (anahtar kelimeler, yapı, gerçekler)
- **Kategori**: Raporlama için gruplama (örneğin "gerçek doğruluk", "tonlama", "tamlık")

### 2. Kurallara Dayalı Kontroller

LLM gerektirmeyen hızlı ve deterministik kontroller:

| Kontrol | Test Edilen |
|-------|--------------|
| **Uzunluk sınırları** | Yanıt çok kısa (tembel) veya çok uzun (uzun uzadıya) değil |
| **Gerekli anahtar kelimeler** | Yanıt beklenen terim veya varlıkları içeriyor |
| **Format doğrulama** | JSON çözümlenebilir, Markdown başlıkları mevcut |
| **Yasaklı içerik** | Halüsinasyonlu marka isimleri yok, rakip anmaları yok |

### 3. LLM-hakem

Kendi çıktısını (veya farklı bir teklif varyantının çıktısını) değerlendirmek için **aynı yerel modeli** kullan. Hakem şunları alır:

- Orijinal soru
- Ajanın yanıtı
- Değerlendirme kriterleri

Ve yapılandırılmış bir puan döner. Bu, Bölüm 7’deki Editör deseninin sistematik test takımı genelinde uygulanmasıdır.

### 4. Değerlendirme Odaklı İterasyon Döngüsü

![Değerlendirme odaklı iterasyon döngüsü](../../../images/eval-loop-flowchart.svg)

---

## Ön Gereksinimler

| Gereksinim | Ayrıntılar |
|-------------|---------|
| **Foundry Local CLI** | Model indirip kurulu |
| **Dil çalışma zamanı** | **Python 3.9+** ve/veya **Node.js 18+** ve/veya **.NET 9+ SDK** |
| **Tamamlanmış** | [Bölüm 5: Tekli Ajanlar](part5-single-agents.md) ve [Bölüm 6: Çoklu Ajan İş Akışları](part6-multi-agent-workflows.md) |

---

## Laboratuvar Alıştırmaları

### Alıştırma 1 - Değerlendirme Çerçevesini Çalıştır

Atölye, Zava DIY ile ilgili sorulardan oluşan altın veri setine karşı Foundry Local ajanını test eden eksiksiz bir değerlendirme örneği içerir.

<details>
<summary><strong>🐍 Python</strong></summary>

**Kurulum:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Çalıştır:**
```bash
python foundry-local-eval.py
```

**Ne Olur:**
1. Foundry Local’a bağlanır ve modeli yükler
2. 5 test durumundan oluşan bir altın veri seti tanımlar (Zava ürün soruları)
3. Her test durumu için iki teklif varyantını çalıştırır
4. Her yanıta **kurallara dayalı kontrollerle** (uzunluk, anahtar kelimeler, format) puan verir
5. Her yanıta **LLM-hakemle** puan verir (aynı model kaliteyi 1-5 arası değerlendirir)
6. Her iki teklif varyantını karşılaştıran bir puan kartı yazdırır

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Kurulum:**
```bash
cd javascript
npm install
```

**Çalıştır:**
```bash
node foundry-local-eval.mjs
```

**Aynı değerlendirme hattı:** altın veri seti, çift teklif çalıştırma, kurallara dayalı + LLM puanlama, puan kartı.

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Kurulum:**
```bash
cd csharp
dotnet restore
```

**Çalıştır:**
```bash
dotnet run eval
```

**Aynı değerlendirme hattı:** altın veri seti, çift teklif çalıştırma, kurallara dayalı + LLM puanlama, puan kartı.

</details>

---

### Alıştırma 2 - Altın Veri Setini Anla

Değerlendirme örneğinde tanımlı test durumlarını inceleyin. Her test durumu şunlara sahiptir:

```
{
  "input":    "What tools do I need to build a deck?",
  "expected": ["saw", "drill", "screws", "level"],
  "category": "product-recommendation"
}
```

**Düşünülmesi gereken sorular:**
1. Neden beklenen değerler tam cümleler değil de **anahtar kelimeler**?
2. Güvenilir bir değerlendirme için kaç test durumu gereklidir?
3. Kendi uygulaman için hangi kategorileri eklerdin?

---

### Alıştırma 3 - Kurallara Dayalı ve LLM-hakem Puanlamasını Anla

Değerlendirme çerçevesi **iki puanlama katmanı** kullanır:

#### Kurallara Dayalı Kontroller (hızlı, deterministik)

```
✓ Length: 50-500 words       → 1 point
✓ Keywords: 3/4 found        → 0.75 points  
✗ Forbidden: mentions "Home Depot" → 0 points
─────────────────────────────
Rule score: 0.58 / 1.0
```

#### LLM-hakem (ince, niteliksel)

Aynı yerel model yapılandırılmış bir rubrik ile hakem görevindedir:

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

**Düşünülmesi gereken sorular:**
1. Ne zaman kurallara dayalı kontrolleri LLM-hakeme tercih edersin?
2. Bir model kendi çıktısını güvenilir şekilde değerlendirebilir mi? Sınırlamalar nelerdir?
3. Bu, Bölüm 7’deki Editör ajan desenine nasıl benziyor?

---

### Alıştırma 4 - Teklif Varyantlarını Karşılaştır

Örnek, aynı test durumlarına karşı **iki teklif varyantı** çalıştırır:

| Varyant | Sistem Teklif Stili |
|---------|-------------------|
| **Temel** | Genel: "Yardımcı bir asistansın" |
| **Özel** | Ayrıntılı: "Adım adım rehberlik veren ve belirli ürünler öneren bir Zava DIY uzmanısın" |

Çalıştırdıktan sonra şöyle bir puan kartı göreceksiniz:

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

**Alıştırmalar:**
1. Değerlendirmeyi çalıştır ve her varyant için puanları not et
2. Özel teklifi daha da spesifik hale getir. Puan artıyor mu?
3. Üçüncü bir teklif varyantı ekle ve üçünü karşılaştır.
4. Model takma adını değiştirip (örneğin `phi-4-mini` ve `phi-3.5-mini`) sonuçları karşılaştır.

---

### Alıştırma 5 - Değerlendirmeyi Kendi Ajanına Uygula

Değerlendirme çerçevesini kendi ajanların için şablon olarak kullan:

1. **Altın veri setini tanımla**: Beklenen anahtar kelimeler ile 5 ila 10 test durumu yaz.
2. **Sistem teklifini yaz**: Test etmek istediğin ajan talimatları.
3. **Değerlendirmeyi çalıştır**: Temel puanları al.
4. **İterasyon yap**: Teklifi ayarla, tekrar çalıştır ve karşılaştır.
5. **Eşik belirle**: Örneğin "0.75 altındaki birleşik puanla teslim etme".

---

### Alıştırma 6 - Zava Editör Deseni ile Bağlantı

Zava Creative Writer’ın Editör ajanına (`zava-creative-writer-local/src/api/agents/editor/editor.py`) bak:

```python
# Editör, üretimde bir LLM-yargıçtır:
{"decision": "accept/revise", "editorFeedback": "...", "researchFeedback": "..."}
```

Bu, Bölüm 8’in LLM-hakem konseptiyle **aynı kavramdır**, ancak çevrimdışı test takımı yerine üretim hattına gömülüdür. İki desen:

- Modelden yapılandırılmış JSON çıktısı kullanır.
- Sistem teklifinde tanımlanmış kalite kriterlerini uygular.
- Geçti/kaldı kararı verir.

**Fark:** Editör üretimde çalışır (her istekte). Değerlendirme çerçevesi geliştirirken (teslim etmeden önce).

---

## Temel Çıkarımlar

| Kavram | Çıkarım |
|---------|----------|
| **Altın veri setleri** | Test durumlarını erken seç; bunlar AI için gerileme testleridir |
| **Kurallara dayalı kontroller** | Hızlı, deterministik ve bariz hataları yakalar (uzunluk, anahtar kelimeler, format) |
| **LLM-hakem** | Aynı yerel modeli kullanarak incelikli kalite puanlaması yapar |
| **Teklif karşılaştırması** | Aynı test takımıyla birden çok varyant çalıştır, en iyisini seç |
| **Cihaz üstü avantaj** | Tüm değerlendirmeler yerelde: API maliyeti, oran sınırı, veri dışarı çıkması yok |
| **Teslim öncesi değerlendirme** | Kalite eşikleri belirle ve değerlendirme puanlarına göre sürümleri kapat |

---

## Sonraki Adımlar

- **Ölçeklendir**: Altın veri setine daha fazla test durumu ve kategori ekle.
- **Otomatize et**: Değerlendirmeyi CI/CD hattına entegre et.
- **Gelişmiş hakemler**: Daha küçük model çıktısını test ederken daha büyük modeli hakem olarak kullan.
- **Zamana karşı takip et**: Değerlendirme sonuçlarını kaydederek teklif ve model sürümleri arasında karşılaştır.

---

## Sonraki Laboratuvar

[Part 9: Whisper ile Ses Transkripsiyonu](part9-whisper-voice-transcription.md) bölümüne devam ederek Foundry Local SDK kullanarak cihazda konuşmadan metne dönüştürmeyi keşfedin.