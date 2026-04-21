![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bölüm 12: Zava Creative Writer için Web UI Oluşturma

> **Hedef:** Çoklu ajan boru hattının gerçek zamanlı çalışmasını izleyebilmek için, canlı ajan durum rozetleri ve akış halinde makale metni ile tek bir yerel web sunucusundan servis edilen tarayıcı tabanlı bir ön yüz eklemek.

[7. Bölüm](part7-zava-creative-writer.md) da Zava Creative Writer'ı **CLI uygulaması** (JavaScript, C#) ve **headless API** (Python) olarak incelediniz. Bu laboratuvarda, kullanıcıların terminal yerine tarayıcı üzerinden boru hattıyla etkileşime girebilmesi için paylaşılan bir **vanilla HTML/CSS/JavaScript** ön yüzü her arka uç ile bağlayacaksınız.

---

## Öğrenecekleriniz

| Amaç | Açıklama |
|-----------|-------------|
| Arka uçtan statik dosyaları servis etmek | API rotanızın yanına bir HTML/CSS/JS dizini bağlama |
| Tarayıcıda akış halinde NDJSON tüketmek | Yeni satırlarla ayrılmış JSON'u okumak için Fetch API ve `ReadableStream` kullanma |
| Birleştirilmiş akış protokolü | Python, JavaScript ve C# arka uçlarının aynı mesaj formatını üretmesini sağlama |
| İlerleyici UI güncellemeleri | Ajan durum rozetlerini güncelleme ve makale metnini token token akıtma |
| CLI uygulamasına HTTP katmanı eklemek | Mevcut düzenleyici mantığını Express tarzı sunucu (JS) veya ASP.NET Core minimal API (C#) ile sarmak |

---

## Mimari

UI, üç arka uç tarafından paylaşılan tek bir statik dosya setidir (`index.html`, `style.css`, `app.js`). Her arka uç aynı iki rotayı sunar:

![Zava UI mimarisi — üç arka uç ile paylaşılan ön yüz](../../../images/part12-architecture.svg)

| Rota | Metot | Amaç |
|-------|--------|---------|
| `/` | GET | Statik UI'yı servis eder |
| `/api/article` | POST | Çoklu ajan boru hattını çalıştırır ve NDJSON akışı yapar |

Ön yüz bir JSON gövde gönderir ve yanıtı yeni satırlarla ayrılmış JSON mesajları akışı olarak okur. Her mesajda UI'ın doğru paneli güncellemek için kullandığı bir `type` alanı vardır:

| Mesaj tipi | Anlamı |
|-------------|---------|
| `message` | Durum güncellemesi (örn. "Araştırmacı ajan görevi başlatılıyor...") |
| `researcher` | Araştırma sonuçları hazır |
| `marketing` | Ürün arama sonuçları hazır |
| `writer` | Yazar başladı veya tamamladı (`{ start: true }` veya `{ complete: true }` içerir) |
| `partial` | Yazardan tek bir akış halinde token ( `{ text: "..." }` içerir) |
| `editor` | Editör kararı hazır |
| `error` | Bir sorun oluştu |

![Mesaj tiplerine göre tarayıcıda yönlendirme](../../../images/part12-message-types.svg)

![Akış sırası — Tarayıcıdan Arka Uca iletişim](../../../images/part12-streaming-sequence.svg)

---

## Önkoşullar

- [7. Bölüm: Zava Creative Writer](part7-zava-creative-writer.md) tamamlandı
- Foundry Local CLI yüklü ve `phi-3.5-mini` modeli indirildi
- Modern bir web tarayıcısı (Chrome, Edge, Firefox veya Safari)

---

## Paylaşılan UI

Herhangi bir arka uç koduna dokunmadan önce, üç dil yolunun da kullanacağı ön yüzü keşfetmek için bir an ayırın. Dosyalar `zava-creative-writer-local/ui/` dizinindedir:

| Dosya | Amaç |
|------|---------|
| `index.html` | Sayfa düzeni: giriş formu, ajan durum rozetleri, makale çıktı alanı, açılıp kapanabilen detay panelleri |
| `style.css` | Durum rozet renk durumları (beklemede, çalışıyor, tamamlandı, hata) için minimal stil |
| `app.js` | Fetch çağrısı, `ReadableStream` satır okuyucu ve DOM güncelleme mantığı |

> **İpucu:** `index.html` dosyasını doğrudan tarayıcınızda açarak düzeni önizleyin. Henüz çalışacak bir arka uç olmadığından fonksiyonel olmayacaktır ama yapıyı görebilirsiniz.

### Akış Okuyucu Nasıl Çalışır

`app.js` içindeki temel fonksiyon, gelen yanıt gövdesini parça parça okur ve satır sonlarında ayrımlar yapar:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // eksik kalan son satırı koru

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

Her ayrıştırılan mesaj `handleMessage()` fonksiyonuna iletilir ve `msg.type` temelinde ilgili DOM öğesi güncellenir.

---

## Alıştırmalar

### Alıştırma 1: Python Arka Ucunu UI ile Çalıştırma

Python (FastAPI) çeşidi zaten bir akış API uç noktası sağlıyor. Tek değişiklik `ui/` klasörünü statik dosya olarak bağlamak.

**1.1** Python API dizinine gidin ve bağımlılıkları yükleyin:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Sunucuyu başlatın:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Tarayıcınızda `http://localhost:8000` adresini açın. Üç metin alanı ve "Generate Article" düğmesi içeren Zava Creative Writer UI'yı görmelisiniz.

**1.4** Varsayılan değerlerle **Generate Article** düğmesine tıklayın. Her ajan işini tamamladıkça durum rozetlerinin "Waiting"den "Running"e ve ardından "Done"a değişimini izleyin ve makale metninin çıktı paneline token token aktığını görün.

> **Sorun Giderme:** Sayfa UI yerine JSON yanıtı gösteriyorsa, statik dosyaları bağlayan güncellenmiş `main.py`'yi çalıştırdığınızdan emin olun. `/api/article` uç noktası hala kendi orijinal yolunda çalışıyor; statik dosya mount ise diğer tüm rotalarda UI'yı servis ediyor.

**Nasıl çalışır:** Güncellenmiş `main.py` dosyasının sonunda tek satır eklenir:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Bu, `zava-creative-writer-local/ui/` içindeki her dosyayı statik varlık olarak sunar ve `index.html` varsayılan belge olur. `/api/article` POST rotası statik mount'tan önce kayıt edilir, bu yüzden önceliğe sahiptir.

---

### Alıştırma 2: JavaScript Çeşidine Web Sunucu Ekleme

JavaScript çeşidi şu anda bir CLI uygulamasıdır (`main.mjs`). Yeni bir dosya, `server.mjs`, aynı ajan modüllerini HTTP sunucusu arkasına sarar ve paylaşılan UI'yı servis eder.

**2.1** JavaScript dizinine gidin ve bağımlılıkları yükleyin:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Web sunucusunu başlatın:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Aşağıdakini görmelisiniz:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Tarayıcınızda `http://localhost:3000` adresini açın ve **Generate Article** düğmesine tıklayın. Aynı UI, JavaScript arka ucu ile sorunsuz çalışır.

**Kodu inceleyin:** `server.mjs` dosyasını açın ve ana desenlere dikkat edin:

- **Statik dosya servisi için** Node.js dahili `http`, `fs` ve `path` modülleri kullanılır, harici çerçeve gerekmez.
- **Yol geçişi koruması** istenilen yol normalize edilir ve `ui/` dizini içinde kaldığı doğrulanır.
- **NDJSON akışı** her nesneyi serileştiren, iç satır sonlarını kaldıran ve sonuna satır sonu ekleyen `sendLine()` yardımcısını kullanır.
- **Ajan düzenlemesi** mevcut `researcher.mjs`, `product.mjs`, `writer.mjs` ve `editor.mjs` modüllerini değiştirmeden yeniden kullanır.

<details>
<summary>server.mjs'den önemli kesit</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Araştırmacı
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Yazar (yayın yapıyor)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Alıştırma 3: C# Çeşidine Minimal API Ekleme

C# çeşidi şu anda konsol uygulamasıdır. Yeni bir proje, `csharp-web`, aynı boru hattını web servisi olarak sunmak için ASP.NET Core minimal API'leri kullanır.

**3.1** C# web projesine gidin ve paketleri geri yükleyin:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Web sunucusunu başlatın:

```bash
dotnet run
```

```powershell
dotnet run
```

Aşağıdakini görmelisiniz:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Tarayıcınızda `http://localhost:5000` adresini açın ve **Generate Article** düğmesine tıklayın.

**Kodu inceleyin:** `csharp-web` dizininde `Program.cs` dosyasını açın ve aşağıdakilere dikkat edin:

- Proje dosyası `Microsoft.NET.Sdk.Web` kullanıyor, `Microsoft.NET.Sdk` değil; bu ASP.NET Core desteği ekler.
- Statik dosyalar `UseDefaultFiles` ve `UseStaticFiles` ile paylaşılan `ui/` dizinine yönlendirilir.
- `/api/article` uç noktası NDJSON satırlarını doğrudan `HttpContext.Response`'a yazar ve gerçek zamanlı akış için her satırdan sonra boşaltır.
- Tüm ajan mantığı (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) konsol sürümüyle aynıdır.

<details>
<summary>csharp-web/Program.cs'den önemli kesit</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### Alıştırma 4: Ajan Durum Rozetlerini İnceleyin

Çalışan bir UI olduğuna göre, ön yüzün durum rozetlerini nasıl güncellediğine bakın.

**4.1** Editörünüzde `zava-creative-writer-local/ui/app.js` dosyasını açın.

**4.2** `handleMessage()` fonksiyonunu bulun. Mesaj tiplerini DOM güncellemelerine nasıl eşlediğini fark edin:

| Mesaj tipi | UI aksiyonu |
|-------------|-----------|
| `"researcher"` içeren `message` | Araştırmacı rozeti "Running" olur |
| `researcher` | Araştırmacı rozeti "Done" olur, Araştırma Sonuçları paneli doldurulur |
| `marketing` | Ürün Arama rozeti "Done" olur, Ürün Eşleşmeleri paneli doldurulur |
| `data.start` içeren `writer` | Yazarlık rozeti "Running" olur ve makale çıktısı temizlenir |
| `partial` | Token metni makale çıktısına eklenir |
| `data.complete` içeren `writer` | Yazarlık rozeti "Done" olur |
| `editor` | Editör rozeti "Done" olur ve Editör Geri Bildirim paneli doldurulur |

**4.3** Makalenin altındaki açılıp kapanabilen "Araştırma Sonuçları", "Ürün Eşleşmeleri" ve "Editör Geri Bildirim" panellerini açarak her ajanın ürettiği ham JSON'u inceleyin.

---

### Alıştırma 5: UI'ı Özelleştirin (Geliştirme)

Aşağıdaki geliştirmelerden birini veya birkaçını deneyin:

**5.1 Kelime sayısı ekleyin.** Yazar tamamlandıktan sonra, çıktı panelinin altında makaledeki kelime sayısını gösterin. Bunu `handleMessage` içinde `type === "writer"` ve `data.complete` true iken hesaplayabilirsiniz:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Yeniden deneme göstergesi ekleyin.** Editör revizyon istediğinde boru hattı tekrar çalışır. Durum paneline "Revision 1" veya "Revision 2" başlığı gösterin. "Revision" içeren `message` tipini dinleyip yeni bir DOM öğesi güncelleyin.

**5.3 Karanlık mod.** Bir geçiş düğmesi ve `<body>`'ye `.dark` sınıfı ekleyin. `style.css` içinde arka plan, metin ve panel renklerini `body.dark` seçicisiyle geçersiz kılın.

---

## Özet

| Ne Yaptınız | Nasıl |
|-------------|-----|
| UI'ı Python arka uçtan servis ettiniz | FastAPI'de `StaticFiles` ile `ui/` klasörünü mount ettiniz |
| JavaScript çeşidine HTTP sunucu eklediniz | Dahili Node.js `http` modülü kullanarak `server.mjs` oluşturdunuz |
| C# çeşidine web API eklediniz | ASP.NET Core minimal API'lerle `csharp-web` projesi oluşturdunuz |
| Tarayıcıda akış halinde NDJSON tükettiniz | `fetch()` ile `ReadableStream` ve satır satır JSON ayrıştırma yaptınız |
| UI'ı gerçek zamanlı güncellediniz | Mesaj tiplerini DOM güncellemelerine eşlediniz (rozetler, metin, paneller) |

---

## Önemli Noktalar

1. **Paylaşılan statik ön yüz**, aynı akış protokolünü kullanan herhangi bir arka uçla çalışabilir; bu da OpenAI uyumlu API deseninin değerini pekiştirir.
2. **Yeni satırlarla ayrılmış JSON (NDJSON)**, tarayıcıdaki `ReadableStream` API ile doğal olarak çalışan basit bir akış formatıdır.
3. **Python çeşidi** en az değişiklik gerektirdi çünkü zaten FastAPI uç noktası vardı; JavaScript ve C# çeşidi ise ince bir HTTP sarmalayıcıya ihtiyaç duydu.
4. UI'ın **vanilla HTML/CSS/JS** olarak kalması, yapı araçlarından, çatı bağımlılıklarından ve ek karmaşıklıktan kaçınarak atölye katılımcıları için kolaylık sağlar.
5. Aynı ajan modülleri (Researcher, Product, Writer, Editor) değişmeden yeniden kullanılır; sadece taşıma katmanı değişir.

---

## Daha Fazla Okuma

| Kaynak | Bağlantı |
|----------|------|
| MDN: Readable Streams Kullanımı | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Statik Dosyalar | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Statik Dosyalar | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON Spesifikasyonu | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Atölye boyunca inşa ettiklerinizin özetini görmek için [13. Bölüme: Atölye Tamamlandı](part13-workshop-complete.md) devam edin.

---
[← Bölüm 11: Araç Çağırma](part11-tool-calling.md) | [Bölüm 13: Atölye Tamamlandı →](part13-workshop-complete.md)