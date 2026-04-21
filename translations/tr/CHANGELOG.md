# Sürüm Notları — Foundry Local Atölye

Bu atölyedeki tüm önemli değişiklikler aşağıda belgelenmiştir.

---

## 2026-03-11 — Bölüm 12 ve 13, Web UI, Whisper Yeniden Yazımı, WinML/QNN Düzeltmesi ve Doğrulama

### Eklendi
- **Bölüm 12: Zava Creative Writer için Web UI Oluşturma** — akış NDJSON, tarayıcı `ReadableStream`, canlı ajan durum rozeti ve gerçek zamanlı makale metni akışı içeren egzersizlerle yeni laboratuvar rehberi (`labs/part12-zava-ui.md`)
- **Bölüm 13: Atölye Tamamlandı** — tüm 12 bölümün özeti, ileri fikirler ve kaynak bağlantıları içeren yeni özet laboratuvar (`labs/part13-workshop-complete.md`)
- **Zava UI ön yüzü:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — üç backend tarafından kullanılan vanilla HTML/CSS/JS tarayıcı arayüzü
- **JavaScript HTTP sunucusu:** `zava-creative-writer-local/src/javascript/server.mjs` — tarayıcı tabanlı erişim için düzenleyici etrafında yeni Express tarzı HTTP sunucusu
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` ve `ZavaCreativeWriterWeb.csproj` — UI ve NDJSON akışı sağlayan yeni minimal API projesi
- **Ses örneği üreticisi:** `samples/audio/generate_samples.py` — Bölüm 9 için Zava temalı WAV dosyaları üretmek için `pyttsx3` kullanan çevrimdışı TTS betiği
- **Ses örneği:** `samples/audio/zava-full-project-walkthrough.wav` — transkripsiyon testi için yeni daha uzun ses örneği
- **Doğrulama betiği:** `validate-npu-workaround.ps1` — tüm C# örneklerinde NPU/QNN geçici çözümünü doğrulamak için otomatik PowerShell betiği
- **Mermaid diyagram SVG’leri:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML çapraz platform desteği:** 3 C# `.csproj` dosyası (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) artık çapraz platform desteği için koşullu TFM ve birbirini dışlayan paket referansları kullanmakta. Windows üzerinde: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (QNN EP eklentisini içeren süper set). Windows dışı platformlarda: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (temel SDK). Zava projelerindeki sabit `win-arm64` RID otomatik algılama ile değiştirildi. Kırık win-arm64 referansı olan `Microsoft.ML.OnnxRuntime.Gpu.Linux` içindeki yerel varlıkları dışlayan geçirimsel bağımlılık geçici çözümü eklendi. Önceki try/catch NPU geçici çözümü tüm 7 C# dosyasından kaldırıldı.

### Değiştirildi
- **Bölüm 9 (Whisper):** Büyük yeniden yazım — JavaScript artık SDK’nın yerleşik `AudioClient` (`model.createAudioClient()`) kullanıyor; elle ONNX Runtime çıkarımı yerine; JS/C# `AudioClient` yaklaşımı ile Python ONNX Runtime yaklaşımını yansıtan mimari açıklamalar, karşılaştırma tabloları ve işlem adımları güncellendi
- **Bölüm 11:** Navigasyon bağlantıları güncellendi (şimdi Bölüm 12’ye yönlendiriyor); araç çağrısı akışı ve ardışık diyagramların renderlenmiş SVG’leri eklendi
- **Bölüm 10:** Navigasyon, atölyeyi bitirmek yerine Bölüm 12 üzerinden yönlendirecek şekilde güncellendi
- **Python Whisper (`foundry-local-whisper.py`):** Ek ses örnekleri ve geliştirilmiş hata işleme ile genişletildi
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Elle ONNX Runtime oturumları yerine `model.createAudioClient()` ve `audioClient.transcribe()` kullanacak şekilde yeniden yazıldı
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** API ile birlikte statik UI dosyalarını sunacak şekilde güncellendi
- **Zava C# konsol (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU geçici çözümü kaldırıldı (şimdi WinML paketi tarafından yönetiliyor)
- **README.md:** Bölüm 12 için kod örneği tabloları ve backend eklemeleri eklendi; Bölüm 13 bölümü eklendi; öğrenme hedefleri ve proje yapısı güncellendi
- **KNOWN-ISSUES.md:** Çözülmüş Sorun #7 (C# SDK NPU Model Varyantı — şimdi WinML paketi tarafından yönetiliyor) kaldırıldı. Kalan sorunlar #1–#6 olarak yeniden numaralandırıldı. Ortam detayları .NET SDK 10.0.104 ile güncellendi
- **AGENTS.md:** Proje yapı ağacı yeni `zava-creative-writer-local` girdileri (`ui/`, `csharp-web/`, `server.mjs`) ile güncellendi; C# ana paketler ve koşullu TFM detayları güncellendi
- **labs/part2-foundry-local-sdk.md:** `.csproj` örneği tam çapraz platform desenini koşullu TFM, birbirini dışlayan paket referansları ve açıklayıcı not ile gösterecek şekilde güncellendi

### Doğrulandı
- Tüm 3 C# projesi (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) Windows ARM64 üzerinde başarılı şekilde derlendi
- Sohbet örneği (`dotnet run chat`): model WinML/QNN aracılığıyla `phi-3.5-mini-instruct-qnn-npu:1` olarak yüklendi — NPU varyantı CPU düşme olmadan doğrudan yüklendi
- Ajan örneği (`dotnet run agent`): çok tur konuşmalı uçtan uca başarılı işletim, çıkış kodu 0
- Foundry Local CLI v0.8.117 ve SDK v0.9.0 .NET SDK 9.0.312 üzerinde

---

## 2026-03-11 — Kod Düzeltmeleri, Model Temizliği, Mermaid Diyagramları ve Doğrulama

### Düzeltildi
- **Tüm 21 kod örneği (7 Python, 7 JavaScript, 7 C#):** Çıkışta OGA bellek sızıntısı uyarılarını çözmek için `model.unload()` / `unload_model()` / `model.UnloadAsync()` temizliği eklendi (Bilinen Sorun #4)
- **csharp/WhisperTranscription.cs:** Kırılgan `AppContext.BaseDirectory` göreli yolu, `samples/audio` dizinini güvenilir şekilde bulmak için yukarı doğru dizinlerde dolaşan `FindSamplesDirectory()` ile değiştirildi (Bilinen Sorun #7)
- **csharp/csharp.csproj:** Sabit `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` otomatik algılama geri dönüşü ile `$(NETCoreSdkRuntimeIdentifier)` kullanılarak değiştirildi; böylece `dotnet run` platformdan bağımsız `-r` bayrağı olmadan çalışıyor ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Değiştirildi
- **Bölüm 8:** ASCII kutu diyagramından işleyen SVG görüntüsüne dönüştürülmüş değerlendirme tabanlı yineleme döngüsü
- **Bölüm 10:** ASCII oklarından işleyen SVG görüntüsüne dönüştürülmüş derleme hattı diyagramı
- **Bölüm 11:** Araç çağrısı akışı ve ardışık diyagramlar işleyen SVG şeklinde dönüştürüldü
- **Bölüm 10:** “Atölye Tamamlandı!” bölümü Bölüm 11’e taşındı (son laboratuvar); yerine “Sonraki Adımlar” bağlantısı eklendi
- **KNOWN-ISSUES.md:** CLI v0.8.117’ye karşı tüm sorunlar yeniden doğrulandı. Çözülenler kaldırıldı: OGA Bellek Sızıntısı (temizlik eklendi), Whisper yolu (FindSamplesDirectory), HTTP 500 sürekli çıkarım (tekrarlanamadı, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice kısıtlamaları (şimdi `"required"` ve qwen2.5-0.5b’ye özgü fonksiyon hedeflemesi ile çalışıyor). JavaScript Whisper sorunu güncellendi — tüm dosyalar boş/ikili çıktı dönüyor (v0.9.x’den regresyon, ciddiyet Major olarak yükseltildi). #4 C# RID otomatik algılama geçici çözümü ve [#497](https://github.com/microsoft/Foundry-Local/issues/497) bağlantısı ile güncellendi. 7 açık sorun kaldı.
- **javascript/foundry-local-whisper.mjs:** Temizlik değişken adı düzeltildi (`whisperModel` → `model`)

### Doğrulandı
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — temizlikle başarılı şekilde çalışıyor
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — temizlikle başarılı şekilde çalışıyor
- C#: `dotnet build` net9.0 hedefi ile 0 uyarı, 0 hata ile başarılı
- Tüm 7 Python dosyası `py_compile` sözdizimi kontrolünden geçti
- Tüm 7 JavaScript dosyası `node --check` sözdizimi doğrulamasından geçti

---

## 2026-03-10 — Bölüm 11: Araç Çağrısı, SDK API Genişlemesi ve Model Kapsamı

### Eklendi
- **Bölüm 11: Yerel Modellerle Araç Çağrısı** — araç şemaları, çok tur akışı, birden fazla araç çağrısı, özel araçlar, ChatClient araç çağrısı ve `tool_choice` içeren 8 egzersiz ile yeni laboratuvar rehberi (`labs/part11-tool-calling.md`)
- **Python örneği:** `python/foundry-local-tool-calling.py` — OpenAI SDK ile `get_weather`/`get_population` araç çağrısı
- **JavaScript örneği:** `javascript/foundry-local-tool-calling.mjs` — SDK’nın yerel `ChatClient` (`model.createChatClient()`) kullanarak araç çağrısı
- **C# örneği:** `csharp/ToolCalling.cs` — OpenAI C# SDK ile `ChatTool.CreateFunctionTool()` kullanarak araç çağrısı
- **Bölüm 2, Egzersiz 7:** Yerel `ChatClient` — `model.createChatClient()` (JS) ve `model.GetChatClientAsync()` (C#), OpenAI SDK’ya alternatifler olarak
- **Bölüm 2, Egzersiz 8:** Model varyantları ve donanım seçimi — `selectVariant()`, `variants`, NPU varyant tablosu (7 model)
- **Bölüm 2, Egzersiz 9:** Model yükseltmeleri ve katalog yenileme — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Bölüm 2, Egzersiz 10:** Akıl yürütme modelleri — `<think>` etiketi ayrıştırma örnekleri ile `phi-4-mini-reasoning`
- **Bölüm 3, Egzersiz 4:** OpenAI SDK’ya alternatif olarak `createChatClient`, akış çağrısı geri arama deseni belgeleri ile
- **AGENTS.md:** Araç Çağrısı, ChatClient ve Akıl Yürütme Modelleri kodlama konvansiyonları eklendi

### Değiştirildi
- **Bölüm 1:** Model kataloğu genişletildi — phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo eklendi
- **Bölüm 2:** API referans tabloları genişletildi — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` eklendi
- **Bölüm 2:** Egzersizler 7-9 → 10-13 olarak yeniden numaralandırıldı
- **Bölüm 3:** Yerel ChatClient dâhil edilerek Önemli Çıkarımlar tablosu güncellendi
- **README.md:** Bölüm 11 kod örneği tablosu eklendi; öğrenme hedefi #11 eklendi; proje yapı ağacı güncellendi
- **csharp/Program.cs:** CLI yönlendiriciye `toolcall` durumu eklendi ve yardım metni güncellendi

---

## 2026-03-09 — SDK v0.9.0 Güncellemesi, Britanya İngilizcesi ve Doğrulama

### Değiştirildi
- **Tüm kod örnekleri (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API’sına güncellendi — `await catalog.getModel()` düzeltildi (await eksikti), `FoundryLocalManager` başlangıç desenleri güncellendi, endpoint keşfi düzeltildi
- **Tüm laboratuvar rehberleri (Bölümler 1-10):** Britanya İngilizcesine dönüştürüldü (colour, catalogue, optimised vb.)
- **Tüm laboratuvar rehberleri:** SDK kod örnekleri v0.9.0 API yüzeyi ile uyumlu hale getirildi
- **Tüm laboratuvar rehberleri:** API referans tabloları ve egzersiz kod blokları güncellendi
- **JavaScript kritik düzeltme:** `catalog.getModel()` üzerindeki eksik `await` eklendi — `Promise` döndürüyordu, `Model` nesnesi değil; bu nedeniyle sessiz hatalar oluşuyordu

### Doğrulandı
- Tüm Python örnekleri Foundry Local servisine karşı başarıyla çalışıyor
- Tüm JavaScript örnekleri başarıyla çalışıyor (Node.js 18+)
- C# projesi .NET 9.0 üzerinde derleniyor ve çalışıyor (net8.0 SDK paketinden ileriye uyumlu)
- Atölye genelinde toplam 29 dosya değiştirilip doğrulandı

---

## Dosya Dizini

| Dosya | Son Güncelleme | Açıklama |
|------|---------------|----------|
| `labs/part1-getting-started.md` | 2026-03-10 | Genişletilmiş model kataloğu |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Yeni egzersizler 7-10, genişletilmiş API tabloları |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Yeni Egzersiz 4 (ChatClient), güncellenmiş çıkarımlar |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Britanya İngilizcesi |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Britanya İngilizcesi |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diyagramı |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diyagramı, Atölye Tamamlandı bölümü 11. Bölüme taşındı |
| `labs/part11-tool-calling.md` | 2026-03-11 | Yeni laboratuvar, Mermaid diyagramları, Atölye Tamamlandı bölümü |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Yeni: araç çağırma örneği |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Yeni: araç çağırma örneği |
| `csharp/ToolCalling.cs` | 2026-03-10 | Yeni: araç çağırma örneği |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI komutu eklendi |
| `README.md` | 2026-03-10 | 11. Bölüm, proje yapısı |
| `AGENTS.md` | 2026-03-10 | Araç çağırma + ChatClient kullanımları |
| `KNOWN-ISSUES.md` | 2026-03-11 | Çözülen Sorun #7 kaldırıldı, 6 açık sorun kaldı |
| `csharp/csharp.csproj` | 2026-03-11 | Platformlar arası TFM, WinML/temel SDK koşullu referanslar |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Platformlar arası TFM, otomatik RID algılama |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Platformlar arası TFM, otomatik RID algılama |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU try/catch çözümü kaldırıldı |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU try/catch çözümü kaldırıldı |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU try/catch çözümü kaldırıldı |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU try/catch çözümü kaldırıldı |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU try/catch çözümü kaldırıldı |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Platformlar arası .csproj örneği |
| `AGENTS.md` | 2026-03-11 | Güncellenmiş C# paketleri ve TFM detayları |
| `CHANGELOG.md` | 2026-03-11 | Bu dosya |