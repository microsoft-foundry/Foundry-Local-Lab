![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bölüm 11: Yerel Modellerle Araç Çağırma

> **Amaç:** Yerel modelinizin dış fonksiyonları (araçları) çağırmasını sağlayarak gerçek zamanlı veri almasını, hesaplamalar yapmasını veya API'lerle etkileşimde bulunmasını mümkün kılmak — tümü cihazınızda özel olarak çalışır.

## Araç Çağırma Nedir?

Araç çağırma (diğer adıyla **fonksiyon çağırma**) dil modelinin sizin tanımladığınız fonksiyonların yürütülmesini istemesini sağlar. Model cevabı tahmin etmek yerine, bir aracın yardımcı olacağını fark eder ve kodunuzun çalıştırması için yapılandırılmış bir istek döner. Uygulamanız fonksiyonu çalıştırır, sonucu geri gönderir ve model bu bilgiyi nihai yanıtına dahil eder.

![Araç çağırma akışı](../../../images/tool-calling-flow.svg)

Bu desen, aşağıdaki yeteneklere sahip ajanlar inşa etmek için çok önemlidir:

- **Canlı veriyi sorgulamak** (hava durumu, borsa fiyatları, veritabanı sorguları)
- **Hassas hesaplamalar yapmak** (matematik, birim dönüşümleri)
- **Eylemler gerçekleştirmek** (e-posta göndermek, bilet oluşturmak, kayıtları güncellemek)
- **Özel sistemlere erişmek** (dahili API'ler, dosya sistemleri)

---

## Araç Çağırma Nasıl Çalışır

Araç çağırma akışı dört aşamadan oluşur:

| Aşama | Ne Olur |
|-------|---------|
| **1. Araçları tanımla** | Mevcut fonksiyonları JSON Şeması kullanarak tanımlarsınız — isim, açıklama ve parametreler |
| **2. Model karar verir** | Model mesajınızı ve araç tanımlarını alır. Bir araç yardımcı olacaksa, metin cevabı yerine `tool_calls` yanıtı döner |
| **3. Yerel çalıştır** | Kodunuz araç çağrısını ayrıştırır, fonksiyonu çalıştırır ve sonucu toplar |
| **4. Nihai cevap** | Araç sonucunu modele geri gönderirsiniz, model nihai yanıtını üretir |

> **Önemli nokta:** Model hiç kod çalıştırmaz. Sadece aracın çağrılmasını *ister*. Uygulamanız bu isteği yerine getirip getirmemeye karar verir — böylece kontrol tamamen sizde kalır.

---

## Hangi Modeller Araç Çağırmayı Destekler?

Her model araç çağırmayı desteklemez. Mevcut Foundry Local kataloğunda aşağıdaki modeller araç çağırma yeteneğine sahiptir:

| Model | Boyut | Araç Çağırma |
|-------|-------|:------------:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **İpucu:** Bu laboratuvarda **qwen2.5-0.5b** kullanıyoruz — küçük (822 MB indirilebilir), hızlı ve güvenilir araç çağırma desteğine sahip.

---

## Öğrenme Hedefleri

Bu laboratuvarın sonunda şunları yapabileceksiniz:

- Araç çağırma desenini ve neden AI ajanları için önemli olduğunu açıklamak
- OpenAI fonksiyon çağırma formatını kullanarak araç şemaları tanımlamak
- Çok turlu araç çağırma konuşma akışını yönetmek
- Araç çağrılarını yerelde çalıştırmak ve sonuçları modele geri göndermek
- Araç çağırma senaryoları için doğru modeli seçmek

---

## Ön Koşullar

| Gereksinim | Detaylar |
|------------|----------|
| **Foundry Local CLI** | Kurulu ve `PATH` üzerinde ([Bölüm 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript veya C# SDK kurulumu ([Bölüm 2](part2-foundry-local-sdk.md)) |
| **Araç çağırma modeli** | qwen2.5-0.5b (otomatik indirilecek) |

---

## Alıştırmalar

### Alıştırma 1 — Araç Çağırma Akışını Anlamak

Kod yazmadan önce, bu sıralama diyagramını inceleyin:

![Araç çağırma sıralama diyagramı](../../../images/tool-calling-sequence.svg)

**Önemli gözlemler:**

1. Araçları önceden JSON Şeması nesneleri olarak tanımlıyorsunuz
2. Modelin yanıtı normal içerik yerine `tool_calls` içeriyor
3. Her araç çağrısının sonucu dönerken referans göstermeniz gereken benzersiz bir `id`si vardır
4. Model nihai cevabı oluştururken tüm önceki mesajları *ve* araç sonuçlarını görür
5. Tek bir yanıtta birden fazla araç çağrısı olabilir

> **Tartışma:** Model neden doğrudan fonksiyonları çalıştırmak yerine araç çağrısı döner? Bu hangi güvenlik avantajlarını sağlar?

---

### Alıştırma 2 — Araç Şemalarını Tanımlama

Araçlar standart OpenAI fonksiyon çağırma formatı ile tanımlanır. Her aracın olması gerekenler:

- **`type`**: Her zaman `"function"`
- **`function.name`**: Açıklayıcı bir fonksiyon adı (örneğin `get_weather`)
- **`function.description`**: Modelin aracı ne zaman çağıracağına karar vermesi için net bir açıklama
- **`function.parameters`**: Beklenen argümanları tanımlayan bir JSON Şeması nesnesi

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **Araç açıklamaları için en iyi uygulamalar:**
> - Spesifik olun: "Belirli bir şehir için güncel hava durumunu al" "Hava durumu al"dan daha iyidir
> - Parametreleri net açıklayın: model doğru değerleri doldurmak için bu açıklamaları okur
> - Gerekli ve isteğe bağlı parametreleri belirtiler — bu modelin ne sorması gerektiğine karar vermesine yardımcı olur

---

### Alıştırma 3 — Araç Çağırma Örneklerini Çalıştırma

Her dil örneği iki aracı (`get_weather` ve `get_population`) tanımlar, araç kullanımını tetikleyen bir soru gönderir, aracı yerel olarak çalıştırır ve sonucu geri göndererek nihai yanıtı alır.

<details>
<summary><strong>🐍 Python</strong></summary>

**Ön Koşullar:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Çalıştır:**
```bash
python foundry-local-tool-calling.py
```

**Beklenen çıktı:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kod incelemesi** (`python/foundry-local-tool-calling.py`):

```python
# Araçları bir işlev şemaları listesi olarak tanımlayın
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# Araçlarla gönderin — model içerik yerine tool_calls döndürebilir
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Modelin bir aracı çağırmak isteyip istemediğini kontrol edin
if response.choices[0].message.tool_calls:
    # Aracı çalıştırın ve sonucu geri gönderin
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Ön Koşullar:**
```bash
cd javascript
npm install
```

**Çalıştır:**
```bash
node foundry-local-tool-calling.mjs
```

**Beklenen çıktı:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kod incelemesi** (`javascript/foundry-local-tool-calling.mjs`):

Bu örnek OpenAI SDK yerine Foundry Local SDK'nın yerel `ChatClient` sınıfını kullanır, `createChatClient()` yönteminin kolaylığını gösterir:

```javascript
// Model nesnesinden doğrudan bir ChatClient alın
const chatClient = model.createChatClient();

// Araçlarla gönder — ChatClient OpenAI uyumlu formatı yönetir
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Araç çağrılarını kontrol et
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Araçları çalıştır ve sonuçları geri gönder
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Ön Koşullar:**
```bash
cd csharp
dotnet restore
```

**Çalıştır:**
```bash
dotnet run toolcall
```

**Beklenen çıktı:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Kod incelemesi** (`csharp/ToolCalling.cs`):

C# araç tanımlamak için `ChatTool.CreateFunctionTool` yardımcı metodunu kullanır:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### Alıştırma 4 — Araç Çağırma Konuşma Akışı

Mesaj yapısını anlamak çok önemlidir. İşte her aşamada `messages` dizisinin tam hali:

**Aşama 1 — İlk istek:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Aşama 2 — Model araç çağrıları (içerik değil) ile yanıt verir:**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**Aşama 3 — Siz asistan mesajını ve araç sonucunu eklersiniz:**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**Aşama 4 — Model araç sonucunu kullanarak nihai cevabı üretir.**

> **Önemli:** Araç mesajındaki `tool_call_id`, araç çağrısından dönen `id` ile eşleşmelidir. Bu, modelin sonuçları isteklerle ilişkilendirme yoludur.

---

### Alıştırma 5 — Birden Fazla Araç Çağrısı

Model tek bir yanıtta birden çok araç çağrısı isteyebilir. Kullanıcı mesajını değiştirmeyi deneyin:

```python
# Python'da — kullanıcı mesajını değiştir:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScript'te — kullanıcı mesajını değiştir:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Model iki `tool_calls` döndürmeli — biri `get_weather` diğeri `get_population` için. Kodunuz tüm araç çağrılarını döngüyle işlediği için bunu halleder.

> **Deneyin:** Kullanıcı mesajını değiştirip örneği tekrar çalıştırın. Model her iki aracı da çağırıyor mu?

---

### Alıştırma 6 — Kendi Aracınızı Ekleyin

Örneklerden birine yeni bir araç ekleyin. Örneğin, `get_time` adlı bir araç:

1. Araç şemasını tanımlayın:
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. Çalıştırma mantığını ekleyin:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Gerçek bir uygulamada, bir zaman dilimi kütüphanesi kullanın
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... mevcut araçlar ...
```

3. `tools` dizisine aracı ekleyin ve "Tokyo'da saat kaç?" sorusuyla test edin.

> **Meydan okuma:** Hesaplama yapan bir araç ekleyin, örneğin Celsius ile Fahrenheit arası dönüşüm yapan `convert_temperature`. "100°F'yi Celsius'a çevir" ile test edin.

---

### Alıştırma 7 — SDK'nın ChatClient ile Araç Çağırma (JavaScript)

JavaScript örneği zaten OpenAI SDK yerine SDK'nın yerel `ChatClient` sınıfını kullanıyor. Bu, OpenAI istemcisi oluşturma ihtiyacını ortadan kaldıran bir kolaylık sağlar:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient doğrudan model nesnesinden oluşturulur
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat ikinci parametre olarak araçları kabul eder
const response = await chatClient.completeChat(messages, tools);
```

Bunu Python örneği ile karşılaştırın; Python OpenAI SDK'yı açıkça kullanır:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Her iki desen de geçerlidir. `ChatClient` daha kullanışlıdır; OpenAI SDK ise tüm OpenAI parametrelerine erişim sağlar.

> **Deneyin:** JavaScript örneğini `ChatClient` yerine OpenAI SDK kullanacak şekilde değiştirin. `import OpenAI from "openai"` yapıp `manager.urls[0]` ile istemci oluşturmanız gerekir.

---

### Alıştırma 8 — tool_choice Parametresini Anlamak

`tool_choice` parametresi modelin *zorunlu* olarak mı araç kullanacağını yoksa serbestçe mi seçeceğini belirtir:

| Değer | Davranış |
|-------|----------|
| `"auto"` | Model araç çağırıp çağırmayacağına karar verir (varsayılan) |
| `"none"` | Model hiçbir aracı çağırmaz, araç sunulsa bile |
| `"required"` | Model en az bir araç çağırmak zorundadır |
| `{"type": "function", "function": {"name": "get_weather"}}` | Model belirtilen aracı çağırmak zorundadır |

Python örneğinde her seçeneği deneyin:

```python
# Modelin get_weather çağırmasını zorla
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Not:** Tüm `tool_choice` seçenekleri her model tarafından desteklenmeyebilir. Bir model `"required"` desteği yoksa ayarı yok sayıp `"auto"` gibi hareket edebilir.

---

## Yaygın Hatalar

| Problem | Çözüm |
|---------|-------|
| Model araç çağırmıyor | Araç çağırma destekli model kullandığınızdan emin olun (örneğin qwen2.5-0.5b). Yukarıdaki tabloya bakın. |
| `tool_call_id` uyuşmazlığı | Araç çağrısından dönen `id`yi mutlaka kullandığınızdan emin olun, sabit değer kullanmayın |
| Model `arguments` içinde hatalı JSON döndürüyor | Daha küçük modeller bazen geçersiz JSON üretir. `JSON.parse()`'i try/catch ile sarmalayın |
| Model var olmayan bir aracı çağırıyor | `execute_tool` fonksiyonuna varsayılan bir handler ekleyin |
| Sonsuz araç çağırma döngüsü | Aşırı istekleri önlemek için maksimum tur sayısı belirleyin (örneğin 5) |

---

## Önemli Noktalar

1. **Araç çağırma**, modellerin cevabı tahmin etmek yerine fonksiyon yürütme istemesini sağlar
2. Model **hiçbir zaman kod çalıştırmaz**; hangi kodun çalıştırılacağı uygulamanızın kararıdır
3. Araçlar **OpenAI fonksiyon çağırma formatına uygun JSON Şeması** olarak tanımlanır
4. Konuşma **çok turlu bir desen** izler: kullanıcı → asistan (tool_calls) → araç (sonuçlar) → asistan (nihai cevap)
5. Her zaman araç çağırmayı destekleyen **model seçin** (Qwen 2.5, Phi-4-mini)
6. SDK'nın `createChatClient()` metodu OpenAI istemcisi oluşturmadan kolayca araç çağırmayı sağlar

---

Devam etmek için [Bölüm 12: Zava Creative Writer için Web UI Oluşturma](part12-zava-ui.md) sayfasına gidin; çoklu ajan hattına gerçek zamanlı akışla tarayıcı tabanlı ön yüz ekleyin.

---

[← Bölüm 10: Özel Modeller](part10-custom-models.md) | [Bölüm 12: Zava Writer UI →](part12-zava-ui.md)