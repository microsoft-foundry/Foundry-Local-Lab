# Bilinen Sorunlar — Foundry Local Workshop

Windows üzerinde çalışan Snapdragon X Elite (ARM64) cihazda Foundry Local SDK v0.9.0, CLI v0.8.117 ve .NET SDK 10.0 ile bu atölye çalışması oluşturulurken ve test edilirken karşılaşılan sorunlar.

> **Son doğrulama:** 2026-03-11

---

## 1. Snapdragon X Elite CPU ONNX Runtime Tarafından Tanınmıyor

**Durum:** Açık
**Şiddet:** Uyarı (engelleyici değil)
**Bileşen:** ONNX Runtime / cpuinfo
**Yeniden Üretme:** Snapdragon X Elite donanımında her Foundry Local servis başlatma

Her Foundry Local servisi başlatıldığında iki uyarı verilir:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**Etkisi:** Uyarılar kozmetiktir — çıkarım doğru çalışır. Ancak, her çalıştırmada göründükleri için atölye katılımcılarını yanıltabilir. ONNX Runtime cpuinfo kütüphanesi Qualcomm Oryon CPU çekirdeklerini tanıyacak şekilde güncellenmeli.

**Beklenen:** Snapdragon X Elite, hata seviyesinde mesaj vermeden desteklenen bir ARM64 CPU olarak tanınmalıdır.

---

## 2. SingleAgent İlk Çalıştırmada NullReferenceException Hatası

**Durum:** Açık (aralıklı)
**Şiddet:** Kritik (çökme)
**Bileşen:** Foundry Local C# SDK + Microsoft Agent Framework
**Yeniden Üretme:** `dotnet run agent` çalıştır — model yüklemeden hemen sonra çöküyor

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**Bağlam:** 37. satırda `model.IsCachedAsync(default)` çağrılır. Çökme, `foundry service stop` sonrası ajanı ilk çalıştırmada oldu. Aynı kodla sonraki çalıştırmalar başarılı oldu.

**Etkisi:** Aralıklı — SDK'nın servis başlatma veya katalog sorgusu sırasında bir yarış durumu olduğunu gösterir. `GetModelAsync()` çağrısı servis tam hazır olmadan dönebilir.

**Beklenen:** `GetModelAsync()`, servis hazır olana kadar engellemeli veya servis tamamen başlatılmadıysa açık bir hata mesajı döndürmeli.

---

## 3. C# SDK Açıkça RuntimeIdentifier Gerektiriyor

**Durum:** Açık — [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) üzerinde izleniyor
**Şiddet:** Dokümantasyon eksikliği
**Bileşen:** `Microsoft.AI.Foundry.Local` NuGet paketi
**Yeniden Üretme:** `.csproj` içerisinde `<RuntimeIdentifier>` olmadan .NET 8+ proje oluşturma

Derleme aşağıdaki hata ile başarısız olur:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**Temel sebep:** RID gerekliliği beklenmektedir — SDK, yerel ikili dosyalar (P/Invoke aracılığıyla `Microsoft.AI.Foundry.Local.Core` ve ONNX Runtime) içerdiğinden, .NET platforma özgü kütüphaneyi çözümlemek zorundadır.

Bu, MS Learn’de belgelenmiştir ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), burada çalışma talimatlarında şunu gösterir:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

Ancak kullanıcılar her seferinde `-r` bayrağını hatırlamak zorundadır; bu kolay unutulabilir.

**Çözüm:** `dotnet run` parametresiz çalışsın diye `.csproj`'nize otomatik algılama geri dönüşü ekleyin:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` dahili bir MSBuild özelliğidir ve çalışma ortamının RID’sini otomatik çözer. SDK’nın test projeleri zaten bu kalıbı kullanır. Sağlandığında açık `-r` bayrakları yine de geçerlidir.

> **Not:** Atölye `.csproj` dosyası bu geri dönüşü içerir ve böylece `dotnet run` herhangi bir platformda kutudan çıktığı gibi çalışır.

**Beklenen:** MS Learn dokümanlarındaki `.csproj` şablonu bu otomatik algılama kalıbını içermeli, böylece kullanıcılar `-r` bayrağını hatırlamak zorunda kalmasın.

---

## 4. JavaScript Whisper — Ses Transkripsiyonu Boş/Veya İkili Çıktı Döndürüyor

**Durum:** Açık (gerileme — ilk rapordan bu yana kötüleşti)
**Şiddet:** Önemli
**Bileşen:** JavaScript Whisper uygulaması (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**Yeniden Üretme:** `node foundry-local-whisper.mjs` çalıştır — tüm ses dosyaları metin transkripsiyonu yerine boş veya ikili çıktı döndürüyor

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

Başlangıçta sadece 5. ses dosyası boş dönüyordu; v0.9.x’de tüm 5 dosya, metin yerine tek bayt (`\ufffd`) döndürüyor. OpenAI SDK kullanan Python Whisper uygulaması aynı dosyaları doğru şekilde transkribe ediyor.

**Beklenen:** `createAudioClient()` Python/C# uygulamalarıyla uyumlu olarak metin transkripsiyonu döndürmeli.

---

## 5. C# SDK Sadece net8.0 Gönderiyor — Resmi .NET 9 veya .NET 10 Hedefi Yok

**Durum:** Açık
**Şiddet:** Dokümantasyon eksikliği
**Bileşen:** `Microsoft.AI.Foundry.Local` NuGet paketi v0.9.0
**Kurulum komutu:** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet paketi yalnızca tek bir hedef framework gönderiyor:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

`net9.0` veya `net10.0` TFM dahil edilmemiş. Oysa, eşlik eden paket `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) `net8.0`, `net9.0`, `net10.0`, `net472` ve `netstandard2.0` gönderiyor.

### Uyumluluk Testi

| Hedef Framework | Derleme | Çalışma | Notlar |
|-----------------|---------|---------|--------|
| net8.0 | ✅ | ✅ | Resmi olarak destekleniyor |
| net9.0 | ✅ | ✅ | İleri uyumluluk ile derleniyor — atölye örneklerinde kullanılıyor |
| net10.0 | ✅ | ✅ | .NET 10.0.3 runtime ile ileri uyumlulukla derlenip çalışıyor |

net8.0 derlemesi .NET’in ileri uyumluluk mekanizmasıyla yeni runtime’larda yüklenebiliyor, bu yüzden derleme başarılı. Ancak bu, SDK ekibi tarafından belgelenmemiş ve test edilmemiştir.

### Örneklerin net9.0 Kullanmasının Sebebi

1. **.NET 9 en güncel kararlı sürüm** — çoğu atölye katılımcısında yüklü olacak
2. **İleri uyumluluk çalışıyor** — NuGet’taki net8.0 derlemesi .NET 9 runtime’da sorunsuz çalışıyor
3. **.NET 10 (önizleme/RC)** herkes için çalışması gereken bir atölyede hedef olarak çok yeni

**Beklenen:** Gelecek SDK sürümleri, `Microsoft.Agents.AI.OpenAI` paketinde kullanılan desenle uyumlu olması ve daha yeni runtime’lar için doğrulanmış destek sağlaması amacıyla `net8.0` yanında `net9.0` ve `net10.0` TFM’leri eklemeyi değerlendirmelidir.

---

## 6. JavaScript ChatClient Yayını Callback Kullanıyor, Async Iterator Değil

**Durum:** Açık
**Şiddet:** Dokümantasyon eksikliği
**Bileşen:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

`model.createChatClient()` tarafından döndürülen `ChatClient` bir `completeStreamingChat()` metodu sağlar, ancak bu bir async iterable döndürmek yerine **callback kalıbı** kullanıyor:

```javascript
// ❌ Bu ÇALIŞMAZ — "stream async iterable değil" hatası verir
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Doğru desen — bir geri çağırma fonksiyonu geçirin
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**Etkisi:** OpenAI SDK’nın async iterasyon desenine aşina geliştiriciler kafa karıştırıcı hatalarla karşılaşacak. Callback geçerli bir fonksiyon olmalı yoksa SDK “Callback must be a valid function.” hatası fırlatıyor.

**Beklenen:** SDK referansında callback deseni belgelenmeli. Alternatif olarak, tutarlılık için OpenAI SDK ile uyumlu async iterable deseni desteklenmeli.

---

## Ortam Detayları

| Bileşen | Versiyon |
|---------|----------|
| İşletim Sistemi | Windows 11 ARM64 |
| Donanım | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |