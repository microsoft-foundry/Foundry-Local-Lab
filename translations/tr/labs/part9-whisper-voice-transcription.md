![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bölüm 9: Whisper ve Foundry Local ile Ses Transkripsiyonu

> **Amaç:** Foundry Local üzerinden yerelde çalışan OpenAI Whisper modelini kullanarak ses dosyalarını tamamen cihaz üzerinde, bulut gerektirmeden yazıya dökmek.

## Genel Bakış

Foundry Local yalnızca metin üretimi için değil, aynı zamanda **konuşmadan metne** modellerini de destekler. Bu laboratuvarda, **OpenAI Whisper Medium** modelini kullanarak ses dosyalarını tamamen kendi makinenizde yazıya dökeceksiniz. Bu, Zava müşteri hizmetleri çağrılarının, ürün inceleme kayıtlarının ya da ses verisinin cihaz dışına çıkmasının istenmediği atölye planlama oturumlarının transkripti gibi durumlar için idealdir.


---

## Öğrenme Hedefleri

Bu laboratuvarın sonunda şunları yapabileceksiniz:

- Whisper konuşmadan metne modelini ve yeteneklerini anlamak
- Foundry Local kullanarak Whisper modelini indirip çalıştırmak
- Foundry Local SDK ile Python, JavaScript ve C# dillerinde ses dosyalarını yazıya dökmek
- Tamamen cihaz üzerinde çalışan basit bir transkripsiyon servisi oluşturmak
- Foundry Local’daki sohbet/metin modelleri ile ses modelleri arasındaki farkları kavramak

---

## Ön Koşullar

| Gereklilik | Detaylar |
|-------------|---------|
| **Foundry Local CLI** | Sürüm **0.8.101 veya üzeri** (Whisper modelleri 0.8.101 sürümünden itibaren mevcuttur) |
| **İşletim Sistemi** | Windows 10/11 (x64 veya ARM64) |
| **Programlama Dili Çalışma Zamanı** | **Python 3.9+** ve/veya **Node.js 18+** ve/veya **.NET 9 SDK** ([.NET İndir](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Tamamlanmış Bölümler** | [Bölüm 1: Başlarken](part1-getting-started.md), [Bölüm 2: Foundry Local SDK Derin Dalış](part2-foundry-local-sdk.md), ve [Bölüm 3: SDK’lar ve API’lar](part3-sdk-and-apis.md) |

> **Not:** Whisper modelleri **SDK** aracılığıyla indirilmelidir (CLI ile değil). CLI, ses transkripsiyon uç noktasını desteklemez. Sürümünüzü şu komutla kontrol edin:
> ```bash
> foundry --version
> ```

---

## Konsept: Whisper’ın Foundry Local ile Çalışma Prensibi

OpenAI Whisper modeli, çeşitli seslerden oluşan büyük bir veri seti üzerinde eğitilmiş genel amaçlı bir konuşma tanıma modelidir. Foundry Local üzerinden çalıştırıldığında:

- Model **tamamen CPU üzerinde** çalışır – GPU gerekmez
- Ses verisi asla cihazınızdan çıkmaz – **tam gizlilik**
- Foundry Local SDK modelin indirilmesi ve önbellekleme işlemlerini yönetir
- **JavaScript ve C#** SDK içerisinde yerleşik `AudioClient` sunar, bu tam transkripsiyon sürecini otomatik yönetir – elle ONNX ayarı gerekmez
- **Python** model yönetimi için SDK’yı kullanır ve kodlayıcı/çözücü ONNX modellerine doğrudan çıkarım için ONNX Runtime kullanır

### İşlem Boru Hattı Nasıl Çalışır (JavaScript ve C#) — SDK AudioClient

1. **Foundry Local SDK** Whisper modelini indirir ve önbelleğe alır
2. `model.createAudioClient()` (JS) veya `model.GetAudioClientAsync()` (C#) ile bir `AudioClient` oluşturulur
3. `audioClient.transcribe(path)` (JS) veya `audioClient.TranscribeAudioAsync(path)` (C#) tüm süreci dahili olarak gerçekleştirir — ses ön işleme, kodlayıcı, çözücü ve token çözümü
4. `AudioClient`, doğru transkripsiyon için `"en"` (İngilizce) olarak ayarlanabilen `settings.language` özelliği sunar

### İşlem Boru Hattı Nasıl Çalışır (Python) — ONNX Runtime

1. **Foundry Local SDK**, Whisper ONNX model dosyalarını indirir ve önbelleğe alır
2. **Ses ön işleme**, WAV sesini mel spektrograma dönüştürür (80 mel kutusu x 3000 kare)
3. **Kodlayıcı** mel spektrogramı işler ve gizli durumlar ile çapraz-dikkat anahtar/değer tensörleri üretir
4. **Çözücü** otoregresif olarak çalışır, metin sonu belirtisi üretilene kadar bir token oluşturur
5. **Token çözücü** çıktı token ID’lerini okunabilir metne dönüştürür

### Whisper Model Varyantları

| Takma Ad | Model Kimliği | Cihaz | Boyut | Açıklama |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU hızlandırmalı (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU için optimize edilmiş (çoğu cihaz için önerilir) |

> **Not:** Sohbet modellerinin aksine, Whisper modelleri `automatic-speech-recognition` görevi altında kategorize edilmiştir. Detaylar için `foundry model info whisper-medium` komutunu kullanın.

---

## Laboratuvar Egzersizleri

### Egzersiz 0 - Örnek Ses Dosyalarını Edinin

Bu laboratuvarda, Zava DIY ürün senaryolarına dayalı önceden oluşturulmuş WAV dosyaları bulunmaktadır. Dahil edilen betik ile oluşturabilirsiniz:

```bash
# Depo kökünden - önce bir .venv oluşturun ve etkinleştirin
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Bu, `samples/audio/` klasörüne altı WAV dosyası oluşturur:

| Dosya | Senaryo |
|------|----------|
| `zava-customer-inquiry.wav` | Müşteri, **Zava ProGrip Kablosuz Matkap** hakkında soru soruyor |
| `zava-product-review.wav` | Müşteri, **Zava UltraSmooth İç Cephe Boyası** incelemesi yapıyor |
| `zava-support-call.wav` | **Zava TitanLock Alet Sandığı** hakkında destek çağrısı |
| `zava-project-planning.wav` | DIY yapan biri **Zava EcoBoard Kompozit Decking** ile bir güverte planlıyor |
| `zava-workshop-setup.wav` | Beş Zava ürününü kullanarak atölye kurulumuyla ilgili tur |
| `zava-full-project-walkthrough.wav` | Tüm Zava ürünleri kullanılarak yapılan genişletilmiş garaj yenileme turu (~4 dk, uzun ses testi için) |

> **İpucu:** Kendi WAV/MP3/M4A dosyalarınızı da kullanabilir veya Windows Ses Kaydedici ile kendinizi kaydedebilirsiniz.

---

### Egzersiz 1 - Whisper Modelini SDK ile İndirin

Whisper modelleri ile yeni Foundry Local sürümlerindeki CLI uyumsuzlukları nedeniyle, modeli indirmek ve yüklemek için **SDK** kullanın. Dilinizi seçin:

<details>
<summary><b>🐍 Python</b></summary>

**SDK’yı yükleyin:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Servisi başlat
manager = FoundryLocalManager()
manager.start_service()

# Katalog bilgilerini kontrol et
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Zaten önbelleğe alınıp alınmadığını kontrol et
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Modeli belleğe yükle
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` olarak kaydedin ve çalıştırın:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK’yı yükleyin:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Yönetici oluştur ve servisi başlat
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Kataloğdan modeli al
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// Modeli belleğe yükle
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` olarak kaydedin ve çalıştırın:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK’yı yükleyin:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **Neden CLI değil de SDK?** Foundry Local CLI, Whisper modellerini doğrudan indirme veya servis etme işlevini desteklemez. SDK, ses modellerini programlı şekilde indirme ve yönetme için güvenilir bir yol sunar. JavaScript ve C# SDK’ları, tam transkripsiyon sürecini yöneten yerleşik bir `AudioClient` içerir. Python ise önbelleğe alınmış model dosyalarına doğrudan çıkarım için ONNX Runtime kullanır.

---

### Egzersiz 2 - Whisper SDK'yı Anlama

Whisper transkripsiyonu dil seçimine bağlı olarak farklı yaklaşımlar kullanır. **JavaScript ve C#**, Foundry Local SDK içinde yerleşik bir `AudioClient` sunar ve bu, tüm pipeline’ı (ses ön işleme, kodlayıcı, çözücü, token çözümü) tek çağrıda çalıştırır. **Python**, model yönetimi için SDK; kodlayıcı/çözücü ONNX modellerine doğrudan çıkarım için ONNX Runtime kullanır.

| Bileşen | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK paketleri** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Model yönetimi** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Özellik çıkarımı** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` tarafından yönetilir | SDK `AudioClient` tarafından yönetilir |
| **Çıkarım** | `ort.InferenceSession` (kodlayıcı + çözücü) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token çözümü** | `WhisperTokenizer` | SDK `AudioClient` tarafından | SDK `AudioClient` tarafından |
| **Dil ayarı** | Çözücü tokenlarında `forced_ids` ile | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Girdi** | WAV dosya yolu | WAV dosya yolu | WAV dosya yolu |
| **Çıktı** | Çözümlenmiş metin dizisi | `result.text` | `result.Text` |

> **Önemli:** Her zaman `AudioClient` üzerindeki dili belirleyin (örneğin İngilizce için `"en"`). Dil açıkça ayarlanmazsa, model dilini otomatik algılamaya çalışırken hatalı çıktı verebilir.

> **SDK Kalıpları:** Python, başlatmak için `FoundryLocalManager(alias)` ve ONNX model dosyalarının konumunu bulmak için `get_cache_location()` kullanır. JavaScript ve C# ise SDK içinde yerleşik olan ve tüm transkripsiyon sürecini yöneten `AudioClient`’a (JS’de `model.createAudioClient()`, C#’ta `model.GetAudioClientAsync()`) erişir. Ayrıntılar için [Bölüm 2: Foundry Local SDK Derin Dalış](part2-foundry-local-sdk.md) dosyasına bakınız.

---

### Egzersiz 3 - Basit Bir Transkripsiyon Uygulaması Oluşturun

Dil izleyicinizi seçin ve bir ses dosyasını yazıya döken minimal bir uygulama oluşturun.

> **Desteklenen ses formatları:** WAV, MP3, M4A. En iyi sonuç için 16 kHz örnekleme hızına sahip WAV dosyaları kullanın.

<details>
<summary><h3>Python İzleyicisi</h3></summary>

#### Kurulum

```bash
cd python
python -m venv venv

# Sanal ortamı etkinleştir:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transkripsiyon Kodu

`foundry-local-whisper.py` adlı dosya oluşturun:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# Adım 1: Bootstrap - servisi başlatır, indirir ve modeli yükler
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Önbelleğe alınmış ONNX model dosyalarına giden yolu oluştur
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Adım 2: ONNX oturumlarını ve özellik çıkarıcıyı yükle
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# Adım 3: Mel spektrogram özelliklerini çıkar
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Adım 4: Kodlayıcıyı çalıştır
enc_out = encoder.run(None, {"audio_features": input_features})
# İlk çıktı gizli durumlardır; kalanlar çift yönlü dikkat KV eşleridir
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Adım 5: Otoregresif kod çözme
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transkripte et, zaman damgası yok
input_ids = np.array([initial_tokens], dtype=np.int32)

# Boş öz-dikkat KV önbelleği
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # metin sonu
        break
    generated.append(next_token)

    # Öz-dikkat KV önbelleğini güncelle
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Çalıştırma

```bash
# Bir Zava ürün senaryosunu yazıya dökün
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Veya diğerlerini deneyin:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Python İçin Temel Noktalar

| Metot | Amaç |
|--------|---------|
| `FoundryLocalManager(alias)` | Bootstrap: servisi başlat, modeli indir ve yükle |
| `manager.get_cache_location()` | Önbellekteki ONNX model dosyalarının yolunu al |
| `WhisperFeatureExtractor.from_pretrained()` | Mel spektrogram özellik çıkarıcıyı yükle |
| `ort.InferenceSession()` | Kodlayıcı ve çözücü için ONNX Runtime oturumu oluştur |
| `tokenizer.decode()` | Çıktı token ID’lerini metne dönüştür |

</details>

<details>
<summary><h3>JavaScript İzleyicisi</h3></summary>

#### Kurulum

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transkripsiyon Kodu

`foundry-local-whisper.mjs` adlı dosya oluşturun:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Adım 1: Bootstrap - yönetici oluşturun, servisi başlatın ve modeli yükleyin
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// Adım 2: Bir ses istemcisi oluşturun ve metne dökün
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Temizleme
await model.unload();
```

> **Not:** Foundry Local SDK, `model.createAudioClient()` ile tam ONNX çıkarım işlem hattını yönetebilen yerleşik bir `AudioClient` sunar — `onnxruntime-node` içe aktarmaya gerek yoktur. Doğru İngilizce transkripsiyon için her zaman `audioClient.settings.language = "en"` olarak ayarlanmalıdır.

#### Çalıştırma

```bash
# Bir Zava ürün senaryosu yazıya dökün
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Veya diğerlerini deneyin:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScript İçin Temel Noktalar

| Metot | Amaç |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Yönetici nesnesini oluştur |
| `await catalog.getModel(alias)` | Katalogdan model al |
| `model.download()` / `model.load()` | Whisper modelini indir ve yükle |
| `model.createAudioClient()` | Transkripsiyon için ses istemcisi oluştur |
| `audioClient.settings.language = "en"` | Transkripsiyon dilini ayarla (doğru çıktı için zorunlu) |
| `audioClient.transcribe(path)` | Ses dosyasını yazıya dök, `{ text, duration }` döner |

</details>

<details>
<summary><h3>C# İzleyicisi</h3></summary>

#### Kurulum

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Not:** C# izleyicisi, `Microsoft.AI.Foundry.Local` paketini kullanır ve `model.GetAudioClientAsync()` ile yerleşik bir `AudioClient` sunar. Bu, işlem içinde tam transkripsiyon boru hattını yönetir — ayrıca ONNX Runtime kurulumu gerekmez.

#### Transkripsiyon Kodu

`Program.cs` dosyasının içeriğini şu şekilde değiştirin:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### Çalıştırma

```bash
# Bir Zava ürün senaryosu yazıya dökün
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Ya da başkalarını deneyin:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C# İçin Temel Noktalar

| Metot | Amaç |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local’ı yapılandırma ile başlat |
| `catalog.GetModelAsync(alias)` | Katalogdan model al |
| `model.DownloadAsync()` | Whisper modelini indir |
| `model.GetAudioClientAsync()` | AudioClient’ı al (ChatClient değil!) |
| `audioClient.Settings.Language = "en"` | Transkripsiyon dilini ayarla (doğru çıktı için zorunlu) |
| `audioClient.TranscribeAudioAsync(path)` | Bir ses dosyasını yazıya dök |
| `result.Text` | Transkripte edilmiş metin |


> **C# vs Python/JS:** C# SDK, JavaScript SDK’te olduğu gibi `model.GetAudioClientAsync()` üzerinden yerleşik bir `AudioClient` sunar ve süreç içi transkripsiyon yapar. Python ise önbelleğe alınmış encoder/decoder modellerine karşı doğrudan çıkarım için ONNX Runtime kullanır.

</details>

---

### Egzersiz 4 - Tüm Zava Örneklerini Toplu Transkripte Etme

Artık çalışan bir transkripsiyon uygulamanız olduğuna göre, tüm beş Zava örnek dosyasını transkripte edin ve sonuçları karşılaştırın.

<details>
<summary><h3>Python Yolu</h3></summary>

Tam örnek `python/foundry-local-whisper.py` zaten toplu transkripsiyonu destekler. Argüman verilmeden çalıştırıldığında, `samples/audio/` içindeki tüm `zava-*.wav` dosyalarını transkripte eder:

```bash
cd python
python foundry-local-whisper.py
```

Örnek, başlatmak için `FoundryLocalManager(alias)` kullanır, ardından her dosya için encoder ve decoder ONNX oturumlarını çalıştırır.

</details>

<details>
<summary><h3>JavaScript Yolu</h3></summary>

Tam örnek `javascript/foundry-local-whisper.mjs` zaten toplu transkripsiyonu destekler. Argüman verilmeden çalıştırıldığında, `samples/audio/` içindeki tüm `zava-*.wav` dosyalarını transkripte eder:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Örnek, SDK’yı başlatmak için `FoundryLocalManager.create()` ve `catalog.getModel(alias)` kullanır, sonra her dosyayı transkripte etmek için `AudioClient` (ayar `settings.language = "en"`) kullanır.

</details>

<details>
<summary><h3>C# Yolu</h3></summary>

Tam örnek `csharp/WhisperTranscription.cs` zaten toplu transkripsiyonu destekler. Belirli bir dosya argümanı verilmeden çalıştırıldığında, `samples/audio/` içindeki tüm `zava-*.wav` dosyalarını transkripte eder:

```bash
cd csharp
dotnet run whisper
```

Örnek, süreç içi transkripsiyon için `FoundryLocalManager.CreateAsync()` ve SDK’nın `AudioClient` (ayar `Settings.Language = "en"`) kullanır.

</details>

**Dikkat Edilecekler:** Transkripsiyon çıktısını `samples/audio/generate_samples.py` içindeki orijinal metinle karşılaştırın. Whisper, "Zava ProGrip" gibi ürün isimlerini ve "brushless motor" veya "composite decking" gibi teknik terimleri ne kadar doğru yakalıyor?

---

### Egzersiz 5 - Temel Kod Kalıplarını Anlama

Whisper transkripsiyonunun, tüm üç dildeki sohbet tamamlamalarından nasıl farklı olduğunu inceleyin:

<details>
<summary><b>Python - Sohbetten Farklılıkların Temeli</b></summary>

```python
# Sohbet tamamlama (Bölümler 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Ses transkripsiyonu (Bu Bölüm):
# OpenAI istemcisi yerine doğrudan ONNX Runtime kullanır
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregresif kod çözücü döngüsü ...
print(tokenizer.decode(generated_tokens))
```

**Temel bilgi:** Sohbet modelleri `manager.endpoint` üzerinden OpenAI uyumlu API kullanırken, Whisper SDK’yı kullanarak önbelleğe alınmış ONNX model dosyalarını bulur ve çıkarımı doğrudan ONNX Runtime ile çalıştırır.

</details>

<details>
<summary><b>JavaScript - Sohbetten Farklılıkların Temeli</b></summary>

```javascript
// Sohbet tamamlama (Bölümler 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Ses transkripti (Bu Bölüm):
// SDK'nın dahili AudioClient'ını kullanır
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // En iyi sonuçlar için dil her zaman ayarlanmalıdır
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Temel bilgi:** Sohbet modelleri `manager.urls[0] + "/v1"` üzerinden OpenAI uyumlu API kullanırken, Whisper transkripsiyonu SDK’nın `AudioClient`'ını `model.createAudioClient()` ile elde eder. Yanlış ve bozuk çıktı oluşmaması için `settings.language` ayarlanmalıdır.

</details>

<details>
<summary><b>C# - Sohbetten Farklılıkların Temeli</b></summary>

C# yaklaşımı süreç içi transkripsiyon için SDK’nın yerleşik `AudioClient`'ını kullanır:

**Model başlatma:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**Transkripsiyon:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Temel bilgi:** C#, `FoundryLocalManager.CreateAsync()` kullanır ve doğrudan `AudioClient` elde eder — ONNX Runtime kurulumu gerekmez. Yanlış ve bozuk çıktı oluşmaması için `Settings.Language` ayarlanmalıdır.

</details>

> **Özet:** Python, model yönetimi için Foundry Local SDK ve encoder/decoder modellerine doğrudan çıkarım için ONNX Runtime kullanır. JavaScript ve C# ise SDK’nın yerleşik `AudioClient`'ını kullanarak transkripsiyonu kolaylaştırır — istemci oluşturun, dili ayarlayın ve `transcribe()` / `TranscribeAudioAsync()` çağırın. Doğru sonuçlar için AudioClient’da dil özelliğini mutlaka ayarlayın.

---

### Egzersiz 6 - Deney Yapın

Anlayışınızı derinleştirmek için şu değişiklikleri deneyin:

1. **Farklı ses dosyalarını deneyin** — Windows Ses Kaydedici kullanarak kendinizi konuşurken kaydedin, WAV olarak kaydedin ve transkripte edin

2. **Model varyantlarını karşılaştırın** — NVIDIA GPU’nuz varsa CUDA varyantını deneyin:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Transkripsiyon hızını CPU varyantıyla karşılaştırın.

3. **Çıktı biçimlendirmesi ekleyin** — JSON yanıtı şunları içerebilir:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Bir REST API oluşturun** — transkripsiyon kodunuzu bir web sunucusuyla sarmalayın:

   | Dil | Çatı | Örnek |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` ile `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` ile `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` ile `IFormFile` |

5. **Çok adımlı transkripsiyon ile çoklu tur** — Bölüm 4’ten bir sohbet ajanı ile Whisper’ı birleştirin: önce sesi transkripte edin, sonra metni analiz veya özetleme için bir ajana iletin.

---

## SDK Ses API Referansı

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — bir `AudioClient` örneği oluşturur
> - `audioClient.settings.language` — transkripsiyon dili belirleme (örn. `"en"`)
> - `audioClient.settings.temperature` — rastgelelik kontrolü (opsiyonel)
> - `audioClient.transcribe(filePath)` — bir dosyayı transkripte eder, `{ text, duration }` döner
> - `audioClient.transcribeStreaming(filePath, callback)` — geri çağrıyla parça parça transkripsiyonu akış halinde sağlar
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — bir `OpenAIAudioClient` örneği oluşturur
> - `audioClient.Settings.Language` — transkripsiyon dili belirleme (örn. `"en"`)
> - `audioClient.Settings.Temperature` — rastgelelik kontrolü (opsiyonel)
> - `await audioClient.TranscribeAudioAsync(filePath)` — bir dosyayı transkripte eder, `.Text` içeren nesne döner
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — transkripsiyon parçalarının `IAsyncEnumerable` olarak akışını sağlar

> **İpucu:** Her zaman transkripsiyondan önce dil özelliğini ayarlayın. Ayarlanmazsa Whisper modelinin otomatik algılaması bozuk çıktı (metin yerine tek bir yer tutucu karakter) üretebilir.

---

## Karşılaştırma: Sohbet Modelleri vs. Whisper

| Özellik | Sohbet Modelleri (Bölümler 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Görev türü** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Girdi** | Metin mesajları (JSON) | Ses dosyaları (WAV/MP3/M4A) | Ses dosyaları (WAV/MP3/M4A) |
| **Çıktı** | Üretilmiş metin (akışlı) | Transkripte edilmiş metin (tam) | Transkripte edilmiş metin (tam) |
| **SDK paketi** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API yöntemi** | `client.chat.completions.create()` | ONNX Runtime doğrudan | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Dil ayarı** | Yok | Decoder uyarıcı tokenları | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Akış desteği** | Evet | Hayır | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Gizlilik avantajı** | Kod/veri yerel kalır | Ses verisi yerel kalır | Ses verisi yerel kalır |

---

## Temel Çıkarımlar

| Kavram | Öğrendikleriniz |
|---------|-----------------|
| **Whisper cihazda** | Konuşmadan metne işlem tamamen yerel çalışır, Zava müşteri çağrıları ve ürün yorumlarını cihazda transkripte etmek için ideal |
| **SDK AudioClient** | JavaScript ve C# SDK’ları tek seferde tüm transkripsiyon sürecini yöneten yerleşik `AudioClient` sağlar |
| **Dil ayarı** | Her zaman AudioClient’ın dilini ayarlayın (örn. `"en"`) — aksi halde otomatik algılama bozuk çıktı verebilir |
| **Python** | Model yönetimi için `foundry-local-sdk` + doğrudan ONNX çıkarımı için `onnxruntime` + `transformers` + `librosa` kullanır |
| **JavaScript** | `foundry-local-sdk` ile `model.createAudioClient()` kullanır — `settings.language` ayarlanır, sonra `transcribe()` çağrılır |
| **C#** | `Microsoft.AI.Foundry.Local` ile `model.GetAudioClientAsync()` kullanır — `Settings.Language` ayarlanır, sonra `TranscribeAudioAsync()` çağrılır |
| **Akış desteği** | JS ve C# SDK’ları ayrıca parça parça çıktı için `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` sunar |
| **CPU-optimised** | CPU varyantı (3.05 GB) herhangi bir Windows cihazında GPU olmadan çalışır |
| **Gizlilik önceliği** | Zava müşteri etkileşimlerini ve özel ürün verilerini cihazda tutmak için mükemmel |

---

## Kaynaklar

| Kaynak | Bağlantı |
|----------|------|
| Foundry Local dokümanları | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referansı | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper modeli | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local web sitesi | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Sonraki Adım

Devam edin: [Bölüm 10: Özel veya Hugging Face Modelleri Kullanma](part10-custom-models.md) — Hugging Face’den kendi modellerinizi derleyin ve Foundry Local üzerinden çalıştırın.