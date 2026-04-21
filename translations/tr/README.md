<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Atölyesi - Cihaz Üzerinde AI Uygulamaları Geliştirme

Kendi makinenizde dil modellerini çalıştırmak ve [Foundry Local](https://foundrylocal.ai) ile [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) kullanarak akıllı uygulamalar oluşturmak için uygulamalı bir atölye.

> **Foundry Local nedir?** Foundry Local, dil modellerini tamamen donanımınızda indirip yönetmenize ve sunmanıza olanak tanıyan hafif bir çalışma zamanıdır. Herhangi bir OpenAI ile uyumlu aracı veya SDK'yı bağlayabilmesi için **OpenAI uyumlu bir API** sunar – bulut hesabı gerekmez.

### 🌐 Çok Dilli Destek

#### GitHub Action üzerinden desteklenmektedir (Otomatik ve Her Zaman Güncel)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](./README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Yerelde Klonlamayı mı Tercih Ediyorsunuz?**
>
> Bu depo 50'den fazla dil çevirisini içerdiği için indirme boyutu oldukça büyüktür. Çeviriler olmadan klonlamak için sparse checkout kullanabilirsiniz:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Bu, kursu tamamlamak için gereken her şeyi çok daha hızlı bir şekilde indirmenizi sağlar.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Öğrenme Hedefleri

Bu atölyenin sonunda şunları yapabileceksiniz:

| # | Hedef |
|---|-----------|
| 1 | Foundry Local'u kurmak ve modelleri CLI ile yönetmek |
| 2 | Programatik model yönetimi için Foundry Local SDK API'sini ustalıkla kullanmak |
| 3 | Python, JavaScript ve C# SDK'ları ile lokal çıkarım sunucusuna bağlanmak |
| 4 | Kendi verilerinizde yanıtları temel alan Birlaştırılmış Üretim (RAG) boru hattı kurmak |
| 5 | Kalıcı talimatlar ve kişiliklerle AI ajanları oluşturmak |
| 6 | Geri beslemeli döngülerle çoklu ajan iş akışlarını düzenlemek |
| 7 | Üretim aşamasında bir capstone uygulaması olan Zava Creative Writer'ı keşfetmek |
| 8 | Altın veri kümeleri ve LLM-yargıcı puanlamasıyla değerlendirme çerçeveleri oluşturmak |
| 9 | Whisper ile ses kaydını metne dönüştürmek - cihaz üzerinde sesli yazıya çeviri |
| 10 | ONNX Runtime GenAI ve Foundry Local ile özel veya Hugging Face modellerini derleyip çalıştırmak |
| 11 | Araç çağırma modeliyle yerel modellerin dış fonksiyonları çağırmasını sağlamak |
| 12 | Zava Creative Writer için gerçek zamanlı akış destekli tarayıcı tabanlı kullanıcı arayüzü oluşturmak |

---

## Ön Koşullar

| Gereksinim | Detaylar |
|-------------|---------|
| **Donanım** | Minimum 8 GB RAM (16 GB önerilir); AVX2 destekli CPU veya desteklenen bir GPU |
| **İşletim Sistemi** | Windows 10/11 (x64/ARM), Windows Server 2025 veya macOS 13+ |
| **Foundry Local CLI** | Windows için `winget install Microsoft.FoundryLocal` veya macOS için `brew tap microsoft/foundrylocal && brew install foundrylocal` ile kurulum. Detaylar için [başlangıç kılavuzu](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) bakınız. |
| **Dil çalışma zamanı** | **Python 3.9+** ve/veya **.NET 9.0+** ve/veya **Node.js 18+** |
| **Git** | Bu depoyu klonlamak için |

---

## Başlarken

```bash
# 1. Depoyu klonlayın
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local'ın yüklü olduğunu doğrulayın
foundry model list              # Mevcut modelleri listele
foundry model run phi-3.5-mini  # Etkileşimli sohbet başlat

# 3. Dil seçeneğinizi belirleyin (tam kurulum için Bölüm 2 laboratuvarına bakın)
```

| Dil | Hızlı Başlangıç |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Atölye Bölümleri

### Bölüm 1: Foundry Local ile Başlarken

**Laboratuvar rehberi:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local nedir ve nasıl çalışır
- Windows ve macOS üzerinde CLI kurulumu
- Modelleri keşfetme - listeleme, indirme, çalıştırma
- Model takma adları ve dinamik portları anlama

---

### Bölüm 2: Foundry Local SDK Derinlemesine İnceleme

**Laboratuvar rehberi:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Uygulama geliştirmede CLI yerine SDK kullanmanın nedenleri
- Python, JavaScript ve C# için kapsamlı SDK API referansı
- Servis yönetimi, katalog tarama, model yaşam döngüsü (indirme, yükleme, boşaltma)
- Hızlı başlangıç örüntüleri: Python yapıcı başlatma, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` meta verileri, takma adlar ve donanım açısından en uygun model seçimi

---

### Bölüm 3: SDK’lar ve API’lar

**Laboratuvar rehberi:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript ve C# ile Foundry Local'a bağlanma
- Foundry Local SDK ile servisi programatik olarak yönetme
- OpenAI uyumlu API ile sohbet tamamlamalarını akış halinde kullanma
- Her dil için SDK metot referansı

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Temel akış sohbeti |
| C# | `csharp/BasicChat.cs` | .NET ile akış sohbeti |
| JavaScript | `javascript/foundry-local.mjs` | Node.js ile akış sohbeti |

---

### Bölüm 4: Birleştirilmiş Üretim (RAG)

**Laboratuvar rehberi:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG nedir ve neden önemlidir
- Bellekte bilgi tabanı oluşturma
- Anahtar kelime örtüşmesi ile puanlama tabanlı alma
- Temellendirilmiş sistem istemleri oluşturma
- Cihaz üzerinde tam bir RAG boru hattı çalıştırma

**Kod örnekleri:**

| Dil | Dosya |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Bölüm 5: AI Ajanları Oluşturma

**Laboratuvar rehberi:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ajanı nedir (ham LLM çağrısına karşı)
- `ChatAgent` kalıbı ve Microsoft Agent Framework
- Sistem talimatları, kişilikler ve çok turlu sohbetler
- Ajanlardan yapılandırılmış çıktı (JSON)

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework ile tek ajan |
| C# | `csharp/SingleAgent.cs` | Tek ajan (ChatAgent kalıbı) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Tek ajan (ChatAgent kalıbı) |

---

### Bölüm 6: Çoklu Ajan İş Akışları

**Laboratuvar rehberi:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Çoklu ajan boru hatları: Araştırmacı → Yazar → Editör
- Ardışık orkestrasyon ve geri besleme döngüleri
- Paylaşılan yapılandırma ve yapılandırılmış teslimatlar
- Kendi çoklu ajan iş akışınızı tasarlama

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Üç ajanlı boru hattı |
| C# | `csharp/MultiAgent.cs` | Üç ajanlı boru hattı |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Üç ajanlı boru hattı |

---

### Bölüm 7: Zava Creative Writer - Capstone Uygulama

**Laboratuvar rehberi:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 uzmanlaşmış ajanla üretim tarzı çoklu ajan uygulaması
- Değerlendirici destekli geri besleme döngülü ardışık boru hattı
- Akış çıktısı, ürün katalog araması, yapılandırılmış JSON teslimatlar
- Python (FastAPI), JavaScript (Node.js CLI) ve C# (.NET konsol) ile tam uygulama

**Kod örnekleri:**

| Dil | Dizin | Açıklama |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Orkestratör ile FastAPI web servisi |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI uygulaması |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsol uygulaması |

---

### Bölüm 8: Değerlendirme Odaklı Geliştirme

**Laboratuvar rehberi:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Altın veri kümeleri kullanarak AI ajanları için sistematik değerlendirme çerçevesi oluşturma
- Kural tabanlı kontroller (uzunluk, anahtar kelime kapsamı, yasaklı terimler) + LLM-yargıcı puanlaması
- Prompt varyantlarının yanyana karşılaştırması ve toplam puan kartları
- Bölüm 7'deki Zava Editor ajan kalıbını çevrimdışı test takımı olarak genişletme
- Python, JavaScript ve C# yolları

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Değerlendirme çerçevesi |
| C# | `csharp/AgentEvaluation.cs` | Değerlendirme çerçevesi |
| JavaScript | `javascript/foundry-local-eval.mjs` | Değerlendirme çerçevesi |

---

### Bölüm 9: Whisper ile Ses Transkripsiyonu

**Laboratuvar rehberi:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Yerel olarak çalışan OpenAI Whisper ile konuşmadan metne transkripsiyon
- Gizliliği ön planda tutan ses işleme - ses cihazınızdan dışarı çıkmaz
- Python, JavaScript ve C# için `client.audio.transcriptions.create()` (Python/JS) ve `AudioClient.TranscribeAudioAsync()` (C#) yöntemleri
- Uygulamalı alıştırmalar için Zava temalı örnek ses dosyaları içerir

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper ses transkripsiyonu |
| C# | `csharp/WhisperTranscription.cs` | Whisper ses transkripsiyonu |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper ses transkripsiyonu |

> **Not:** Bu laboratuvar, Whisper modelini programlı şekilde indirip yüklemek için **Foundry Local SDK** kullanır; ardından sesleri transkripsiyon için yerel OpenAI uyumlu uç noktaya gönderir. Whisper modeli (`whisper`), Foundry Local kataloğunda listelenir ve tamamen cihaz üzerinde çalışır - bulut API anahtarları veya ağ erişimi gerektirmez.

---

### Bölüm 10: Özel veya Hugging Face Modellerini Kullanma

**Laboratuvar kılavuzu:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face modellerini optimizasyonlu ONNX formatına ONNX Runtime GenAI model oluşturucu ile derleme
- Donanım spesifik derleme (CPU, NVIDIA GPU, DirectML, WebGPU) ve kuantizasyon (int4, fp16, bf16)
- Foundry Local için sohbet şablonu yapılandırma dosyaları oluşturma
- Derlenmiş modelleri Foundry Local önbelleğine ekleme
- Özel modelleri CLI, REST API ve OpenAI SDK ile çalıştırma
- Referans örnek: Qwen/Qwen3-0.6B modelinin uçtan uca derlenmesi

---

### Bölüm 11: Yerel Modellerle Araç Çağırma

**Laboratuvar kılavuzu:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Yerel modellerin harici fonksiyonları (araç/fonksiyon çağırma) çağırmasını sağlama
- OpenAI fonksiyon çağırma formatını kullanarak araç şemaları tanımlama
- Çok turda araç çağırma sohbet akışını yönetme
- Araç çağrılarını yerelde yürütme ve sonuçları modele geri iletme
- Araç çağırma senaryoları için doğru modeli seçme (Qwen 2.5, Phi-4-mini)
- Araç çağırmak için SDK'nın yerel `ChatClient` kullanımı (JavaScript)

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Hava durumu/kalabalık araçları ile araç çağırma |
| C# | `csharp/ToolCalling.cs` | .NET ile araç çağırma |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ile araç çağırma |

---

### Bölüm 12: Zava Creative Writer için Web UI Oluşturma

**Laboratuvar kılavuzu:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer için tarayıcı tabanlı ön yüz ekleme
- Paylaşılan UI’yi Python (FastAPI), JavaScript (Node.js HTTP) ve C# (ASP.NET Core) üzerinden servis etme
- Tarayıcıda Fetch API ve ReadableStream ile akış halinde NDJSON tüketme
- Canlı ajan durum rozeti ve gerçek zamanlı makale metni akışı

**Kod (paylaşılan UI):**

| Dosya | Açıklama |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sayfa düzeni |
| `zava-creative-writer-local/ui/style.css` | Stil dosyası |
| `zava-creative-writer-local/ui/app.js` | Akış okuyucu ve DOM güncelleme mantığı |

**Arka uç eklemeleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Statik UI servis edecek şekilde güncellendi |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Orkestratörü saran yeni HTTP sunucusu |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Yeni ASP.NET Core minimal API projesi |

---

### Bölüm 13: Atölye Tamamlandı

**Laboratuvar kılavuzu:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Tüm 12 bölüm boyunca inşa ettiklerinizin özeti
- Uygulamalarınızı geliştirmek için ileri fikirler
- Kaynaklar ve dokümantasyona bağlantılar

---

## Proje Yapısı

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Kaynaklar

| Kaynak | Bağlantı |
|----------|------|
| Foundry Local web sitesi | [foundrylocal.ai](https://foundrylocal.ai) |
| Model kataloğu | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Başlangıç kılavuzu | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referansı | [Microsoft Learn - SDK Referansı](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lisans

Bu atölye materyali eğitim amaçlı sağlanmıştır.

---

**İyi çalışmalar! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Feragatname**:  
Bu belge, AI çeviri hizmeti [Co-op Translator](https://github.com/Azure/co-op-translator) kullanılarak çevrilmiştir. Doğruluk için çaba gösterilse de, otomatik çevirilerin hata veya yanlışlık içerebileceğini lütfen unutmayınız. Orijinal belgenin kendi ana dilindeki versiyonu yetkili kaynak olarak kabul edilmelidir. Kritik bilgiler için profesyonel insan çevirisi önerilir. Bu çevirinin kullanımı sonucu ortaya çıkabilecek yanlış anlamalar veya yanlış yorumlamalar için sorumluluk kabul edilmemektedir.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->