# Kodlama Ajanı Talimatları

Bu dosya, bu depoda çalışan AI kodlama ajanları (GitHub Copilot, Copilot Workspace, Codex, vb.) için bağlam sağlar.

## Proje Genel Bakış

Bu, [Foundry Local](https://foundrylocal.ai) ile AI uygulamaları geliştirmek için **pratik bir atölye çalışmasıdır** — OpenAI uyumlu API üzerinden tamamen cihazda dil modelleri indiren, yöneten ve sunan hafif bir çalışma zamanı. Atölye çalışması adım adım laboratuvar kılavuzları ve Python, JavaScript ve C# dillerinde çalıştırılabilir kod örnekleri içerir.

## Depo Yapısı

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Dil ve Çerçeve Detayları

### Python
- **Konum:** `python/`, `zava-creative-writer-local/src/api/`
- **Bağımlılıklar:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Ana paketler:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimum sürüm:** Python 3.9+
- **Çalıştır:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Konum:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Bağımlılıklar:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Ana paketler:** `foundry-local-sdk`, `openai`
- **Modül sistemi:** ES modülleri (`.mjs` dosyaları, `"type": "module"`)
- **Minimum sürüm:** Node.js 18+
- **Çalıştır:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Konum:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Proje dosyaları:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Ana paketler:** `Microsoft.AI.Foundry.Local` (Windows dışı), `Microsoft.AI.Foundry.Local.WinML` (Windows — QNN EP içeren genişletilmiş sürüm), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Hedef:** .NET 9.0 (koşullu TFM: Windows'ta `net9.0-windows10.0.26100`, diğerlerinde `net9.0`)
- **Çalıştır:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Kodlama Konvansiyonları

### Genel
- Tüm kod örnekleri **kendine özgü tek dosya örnekleridir** — paylaşılan yardımcı kütüphaneler veya soyutlamalar yoktur.
- Her örnek, kendi bağımlılıkları yüklendikten sonra bağımsız olarak çalıştırılabilir.
- API anahtarları her zaman `"foundry-local"` olarak ayarlanır — Foundry Local bunu yer tutucu olarak kullanır.
- Temel URL'ler `http://localhost:<port>/v1` formatındadır — port dinamik olup çalışma zamanında SDK aracılığıyla bulunur (`manager.urls[0]` JS'de, `manager.endpoint` Python'da).
- Foundry Local SDK servis başlatmayı ve uç nokta keşfini yönetir; sabit kodlanmış portlar yerine SDK desenlerini tercih edin.

### Python
- `openai` SDK'sını `OpenAI(base_url=..., api_key="not-required")` ile kullanın.
- SDK tarafından yönetilen servis yaşam döngüsü için `foundry_local` içinden `FoundryLocalManager()` kullanın.
- Yayınlama: `stream` nesnesi üzerinde `for chunk in stream:` ile yineleyin.
- Örnek dosyalarda tip açıklaması yoktur (atölye öğrencileri için örnekler kısa tutulur).

### JavaScript
- ES modül sözdizimi: `import ... from "..."`.
- `"openai"`'dan `OpenAI` ve `"foundry-local-sdk"`'den `FoundryLocalManager` kullanın.
- SDK başlatma deseni: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Yayınlama: `for await (const chunk of stream)`.
- Tüm kodda en üst seviyede `await` kullanılır.

### C#
- Nullables etkin, örtük using'ler, .NET 9.
- SDK tarafından yönetilen yaşam döngüsü için `FoundryLocalManager.StartServiceAsync()` kullanın.
- Yayınlama: `CompleteChatStreaming()` içinde `foreach (var update in completionUpdates)`.
- Ana `csharp/Program.cs` CLI yönlendiricidir ve statik `RunAsync()` metodlarına çağrı yapar.

### Araç Çağrısı
- Sadece belli modeller araç çağrısını destekler: **Qwen 2.5** ailesi (`qwen2.5-*`) ve **Phi-4-mini** (`phi-4-mini`).
- Araç şemaları OpenAI fonksiyon çağrısı JSON formatını takip eder (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Konuşma çoklu turlu desen kullanır: kullanıcı → asistan (tool_calls) → araç (sonuçlar) → asistan (nihai cevap).
- Araç sonucu mesajlarındaki `tool_call_id`, modelin araç çağrısından dönen `id` ile eşleşmelidir.
- Python doğrudan OpenAI SDK kullanır; JavaScript SDK'nın yerel `ChatClient`ını (`model.createChatClient()`) kullanır; C# OpenAI SDK ve `ChatTool.CreateFunctionTool()` ile kullanır.

### ChatClient (Yerel SDK İstemcisi)
- JavaScript: `model.createChatClient()` bir `ChatClient` döner; `completeChat(messages, tools?)` ve `completeStreamingChat(messages, callback)` metodlarına sahiptir.
- C#: `model.GetChatClientAsync()` standart bir `ChatClient` döner, OpenAI NuGet paketi içe aktarıma gerek yoktur.
- Python’un yerel ChatClient’ı yoktur — OpenAI SDK'sı `manager.endpoint` ve `manager.api_key` ile kullanılır.
- **Önemli:** JavaScript `completeStreamingChat` **geri çağırma deseni** ile çalışır, async yineleme değil.

### Akıl Yürütme Modelleri
- `phi-4-mini-reasoning`, son cevaptan önce düşünmesini `<think>...</think>` etiketleri içinde sarar.
- Gerekirse akıl yürütme ve cevabı ayırmak için etiketleri analiz edin.

## Laboratuvar Kılavuzları

Laboratuvar dosyaları `labs/` içinde Markdown formatındadır. Tutarlı bir yapı izlerler:
- Logo başlık resmi
- Başlık ve hedef bildirimi
- Genel Bakış, Öğrenme Hedefleri, Önkoşullar
- Diyagramlı kavram açıklama bölümleri
- Kod blokları ve beklenen çıktı ile numaralandırılmış egzersizler
- Özet tablo, Temel Çıkarımlar, İleri Okuma
- Sonraki bölüme gezinme bağlantısı

Laboratuvar içeriği düzenlenirken:
- Mevcut Markdown biçimlendirme ve bölüm hiyerarşisini koruyun.
- Kod blokları dil belirtmelidir (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- İşletim sistemine göre kabuk komutları için hem bash hem PowerShell varyantları sağlayın.
- `> **Not:**`, `> **İpucu:**`, `> **Sorun Giderme:**` çağrı stilleri kullanın.
- Tablolar `| Başlık | Başlık |` boru formatında olmalıdır.

## Derleme ve Test Komutları

| Eylem | Komut |
|--------|---------|
| **Python örnekleri** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS örnekleri** | `cd javascript && npm install && node <script>.mjs` |
| **C# örnekleri** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Diyagram üretimi** | `npx mmdc -i <input>.mmd -o <output>.svg` (root ile `npm install` gerektirir) |

## Dış Bağımlılıklar

- **Foundry Local CLI** geliştiricinin makinesine kurulmalıdır (`winget install Microsoft.FoundryLocal` veya `brew install foundrylocal`).
- **Foundry Local servisi** yerel olarak çalışır ve dinamik port üzerinden OpenAI uyumlu REST API sunar.
- Örneklerin hiçbiri için bulut servisleri, API anahtarları veya Azure abonelikleri gerekmez.
- Bölüm 10 (özel modeller) ek olarak `onnxruntime-genai` gerektirir ve model ağırlıklarını Hugging Face'den indirir.

## Commit Edilmemesi Gereken Dosyalar

`.gitignore` aşağıdakileri hariç tutar (çoğunlukla zaten hariç tutulur):
- `.venv/` — Python sanal ortamları
- `node_modules/` — npm bağımlılıkları
- `models/` — derlenmiş ONNX model çıktısı (büyük ikili dosyalar, Bölüm 10 tarafından üretilir)
- `cache_dir/` — Hugging Face model indirme önbelleği
- `.olive-cache/` — Microsoft Olive çalışma dizini
- `samples/audio/*.wav` — oluşturulmuş ses örnekleri (`python samples/audio/generate_samples.py` ile yeniden oluşturulur)
- Standart Python derleme ürünleri (`__pycache__/`, `*.egg-info/`, `dist/`, vb.)

## Lisans

MIT — bkz. `LICENSE`.