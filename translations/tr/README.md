<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Atölyesi - AI Uygulamalarını Cihaz Üzerinde Oluşturun

Dil modellerini kendi makinenizde çalıştırmak ve [Foundry Local](https://foundrylocal.ai) ile [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) kullanarak akıllı uygulamalar geliştirmek için uygulamalı bir atölye.

> **Foundry Local nedir?** Foundry Local, dil modellerini tamamen donanımınızda indirip yönetmenizi ve sunmanızı sağlayan hafif bir çalışma zamanı ortamıdır. OpenAI ile uyumlu bir **API** sunar, böylece OpenAI ile uyumlu herhangi bir araç veya SDK bağlantı kurabilir - bulut hesabı gerekmez.

---

## Öğrenme Hedefleri

Bu atölye sonunda şunları yapabileceksiniz:

| # | Hedef |
|---|-----------|
| 1 | Foundry Local'u kurmak ve CLI ile modelleri yönetmek |
| 2 | Programatik model yönetimi için Foundry Local SDK API'sini öğrenmek |
| 3 | Python, JavaScript ve C# SDK'larıyla yerel çıkarım sunucusuna bağlanmak |
| 4 | Kendi verilerinizle cevapları destekleyen Retrieval-Augmented Generation (RAG) hattı oluşturmak |
| 5 | Kalıcı talimatlar ve kişiliklere sahip AI ajanları oluşturmak |
| 6 | Geri bildirim döngüleriyle çoklu ajan iş akışlarını düzenlemek |
| 7 | Üretim düzeyinde bir bitirme uygulaması olan Zava Creative Writer'ı keşfetmek |
| 8 | Altın veri setleri ve hakem olarak LLM puanlaması ile değerlendirme çerçeveleri oluşturmak |
| 9 | Foundry Local SDK kullanarak cihazda Whisper ile sesten yazıya dökme yapmak |
| 10 | ONNX Runtime GenAI ve Foundry Local ile özel veya Hugging Face modellerini derleyip çalıştırmak |
| 11 | Yerel modellerin araç çağrısı desenini kullanarak dış fonksiyonları çağırmasını sağlamak |
| 12 | Zava Creative Writer için gerçek zamanlı akışa sahip tarayıcı tabanlı bir UI oluşturmak |

---

## Önkoşullar

| Gereksinim | Detaylar |
|-------------|---------|
| **Donanım** | En az 8 GB RAM (16 GB önerilir); AVX2 destekli CPU veya desteklenen GPU |
| **İŞLETİM SİSTEMİ** | Windows 10/11 (x64/ARM), Windows Server 2025 veya macOS 13+ |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) veya `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) ile kurulum. Detaylar için [başlangıç rehberi](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) ziyaret edin. |
| **Dil çalışma zamanı** | **Python 3.9+** ve/veya **.NET 9.0+** ve/veya **Node.js 18+** |
| **Git** | Bu repoyu klonlamak için |

---

## Başlarken

```bash
# 1. Depoyu klonlayın
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local'ın yüklü olduğunu doğrulayın
foundry model list              # Mevcut modelleri listele
foundry model run phi-3.5-mini  # Etkileşimli sohbete başla

# 3. Dil yolunuzu seçin (tam kurulum için Bölüm 2 laboratuvarına bakın)
```

| Dil | Hızlı Başlangıç |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Atölye Bölümleri

### Bölüm 1: Foundry Local ile Başlamak

**Laboratuvar rehberi:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local nedir ve nasıl çalışır
- Windows ve macOS üzerinde CLI kurulumu
- Modelleri keşfetmek - listeleme, indirme, çalıştırma
- Model takma adları ve dinamik portlar hakkında kavrayış

---

### Bölüm 2: Foundry Local SDK Derinlemesine

**Laboratuvar rehberi:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Uygulama geliştirme için CLI yerine neden SDK kullanılır
- Python, JavaScript ve C# için tam SDK API referansı
- Hizmet yönetimi, katalog taraması, model yaşam döngüsü (indirme, yükleme, boşaltma)
- Hızlı başlangıç kalıpları: Python kurucu bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` meta verisi, takma adlar ve donanım açısından optimal model seçimi

---

### Bölüm 3: SDK'lar ve API'ler

**Laboratuvar rehberi:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript ve C# ile Foundry Local'a bağlanmak
- Foundry Local SDK ile programatik olarak servis yönetimi
- OpenAI uyumlu API üzerinden akışlı sohbet tamamlama
- Her dil için SDK yöntem referansı

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Temel akışlı sohbet |
| C# | `csharp/BasicChat.cs` | .NET ile akışlı sohbet |
| JavaScript | `javascript/foundry-local.mjs` | Node.js ile akışlı sohbet |

---

### Bölüm 4: Retrieval-Augmented Generation (RAG)

**Laboratuvar rehberi:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG nedir ve neden önemlidir
- Bellek içi bilgi tabanı oluşturmak
- Anahtar kelime örtüşmesiyle geri çağırma ve puanlama
- Dayanaklı sistem istemleri oluşturmak
- Cihaz üzerinde tam bir RAG hattı çalıştırmak

**Kod örnekleri:**

| Dil | Dosya |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Bölüm 5: AI Ajanları Oluşturmak

**Laboratuvar rehberi:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ajan nedir (ham LLM çağrısına göre)
- `ChatAgent` kalıbı ve Microsoft Agent Framework
- Sistem talimatları, kişilikler ve çok turlu sohbetler
- Ajanlardan gelen yapılandırılmış çıktı (JSON)

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework ile tek ajan |
| C# | `csharp/SingleAgent.cs` | Tek ajan (ChatAgent kalıbı) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Tek ajan (ChatAgent kalıbı) |

---

### Bölüm 6: Çok Ajanlı İş Akışları

**Laboratuvar rehberi:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Çok ajanlı hatlar: Araştırmacı → Yazar → Editör
- Ardışık düzenleme ve geri bildirim döngüleri
- Paylaşılan yapılandırma ve yapılandırılmış devirler
- Kendi çok ajanlı iş akışınızı tasarlamak

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Üç ajanlı hat |
| C# | `csharp/MultiAgent.cs` | Üç ajanlı hat |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Üç ajanlı hat |

---

### Bölüm 7: Zava Creative Writer - Bitirme Uygulaması

**Laboratuvar rehberi:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 uzman ajanla üretim tarzı çok ajanlı uygulama
- Değerlendirici odaklı geri bildirim döngüleri ile arzılı hat
- Akışlı çıktı, ürün katalog araması, yapılandırılmış JSON devirleri
- Python (FastAPI), JavaScript (Node.js CLI) ve C# (.NET konsol) tam uygulaması

**Kod örnekleri:**

| Dil | Dizin | Açıklama |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Orkestratörlü FastAPI web servisi |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI uygulaması |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsol uygulaması |

---

### Bölüm 8: Değerlendirme Odaklı Geliştirme

**Laboratuvar rehberi:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Altın veri setleri ile AI ajanları için sistematik değerlendirme çerçevesi oluşturmak
- Kural tabanlı kontroller (uzunluk, anahtar kelime kapsamı, yasaklı terimler) + LLM hakem puanlaması
- İstem çeşitlerinin yan yana karşılaştırması ve toplam puan kartları
- Bölüm 7’den Zava Editör ajan kalıbını çevrimdışı test süitine genişletme
- Python, JavaScript ve C# dalları

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Değerlendirme çerçevesi |
| C# | `csharp/AgentEvaluation.cs` | Değerlendirme çerçevesi |
| JavaScript | `javascript/foundry-local-eval.mjs` | Değerlendirme çerçevesi |

---

### Bölüm 9: Whisper ile Ses Transkripsiyonu

**Laboratuvar rehberi:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Yerel olarak çalışan OpenAI Whisper kullanarak sesten yazıya dökme
- Gizlilik öncelikli ses işleme - ses cihazınızdan hiç çıkmaz
- Python, JavaScript ve C# dallarında `client.audio.transcriptions.create()` (Python/JS) ve `AudioClient.TranscribeAudioAsync()` (C#) kullanımı
- Uygulamalı pratik için Zava temalı örnek ses dosyaları dahil

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper ses transkripsiyonu |
| C# | `csharp/WhisperTranscription.cs` | Whisper ses transkripsiyonu |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper ses transkripsiyonu |

> **Not:** Bu laboratuvar **Foundry Local SDK** kullanarak Whisper modelini programatik olarak indirip yükler, ardından transkripsiyon için yerel OpenAI uyumlu uç noktaya ses gönderir. Whisper modeli (`whisper`), Foundry Local kataloğunda listelenmiştir ve tamamen cihaz üzerinde çalışır - bulut API anahtarı veya ağ erişimi gerekmez.

---

### Bölüm 10: Özel veya Hugging Face Modellerinin Kullanımı

**Laboratuvar rehberi:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face modellerini ONNX Runtime GenAI model derleyicisi ile optimize edilmiş ONNX formatına derlemek
- Donanım spesifik derleme (CPU, NVIDIA GPU, DirectML, WebGPU) ve nicemleme (int4, fp16, bf16)
- Foundry Local için sohbet şablonu yapılandırma dosyaları oluşturmak
- Derlenmiş modelleri Foundry Local önbelleğine eklemek
- Özel modelleri CLI, REST API ve OpenAI SDK ile çalıştırmak
- Referans örnek: Qwen/Qwen3-0.6B uçtan uca derleme

---

### Bölüm 11: Yerel Modeller ile Araç Çağrısı

**Laboratuvar rehberi:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Yerel modellerin dış fonksiyonları çağırmasını sağlamak (araç/fonksiyon çağrısı)
- OpenAI fonksiyon çağrısı formatını kullanarak araç şemalarını tanımlamak
- Çok turlu araç çağrısı sohbet akışını yönetmek
- Araç çağrılarını yerel olarak yürütmek ve sonuçları modele döndürmek
- Araç çağrısı senaryoları için doğru modeli seçmek (Qwen 2.5, Phi-4-mini)
- Araç çağrısı için SDK'nın yerel `ChatClient` kullanılabilir (JavaScript)

**Kod örnekleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Hava durumu/nüfus araçları ile araç çağrısı |
| C# | `csharp/ToolCalling.cs` | .NET ile araç çağrısı |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ile araç çağrısı |

---

### Bölüm 12: Zava Creative Writer için Web UI Oluşturma

**Laboratuvar rehberi:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer için tarayıcı tabanlı ön yüz eklemek
- Paylaşılan UI'yı Python (FastAPI), JavaScript (Node.js HTTP) ve C# (ASP.NET Core) üzerinden sunmak
- Tarayıcıda Fetch API ve ReadableStream ile akış türevi NDJSON tüketmek
- Canlı ajan durum rozetleri ve gerçek zamanlı makale metni akışı

**Kod (paylaşılan UI):**

| Dosya | Açıklama |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sayfa düzeni |
| `zava-creative-writer-local/ui/style.css` | Stil dosyası |
| `zava-creative-writer-local/ui/app.js` | Akış okuyucu ve DOM güncelleme mantığı |

**Backend eklemeleri:**

| Dil | Dosya | Açıklama |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Statik UI sunumu için güncellendi |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Orkestratörü sararak yeni HTTP sunucusu |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Yeni ASP.NET Core minimal API projesi |

---

### Bölüm 13: Atölye Tamamlandı
**Laboratuvar kılavuzu:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Tüm 12 bölüm boyunca inşa ettiğiniz her şeyin özeti
- Uygulamalarınızı genişletmek için ileri fikirler
- Kaynaklar ve dokümantasyon bağlantıları

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
|--------|----------|
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

Bu çalışma atölyesi materyali eğitim amaçlı sağlanmaktadır.

---

**İyi çalışmalar! 🚀**