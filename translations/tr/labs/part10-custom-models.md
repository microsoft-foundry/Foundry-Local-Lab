![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bölüm 10: Foundry Local ile Özel veya Hugging Face Modellerinin Kullanımı

> **Amaç:** Foundry Local'ın gerektirdiği optimize edilmiş ONNX formatına bir Hugging Face modelini derlemek, bir sohbet şablonuyla yapılandırmak, yerel önbelleğe eklemek ve CLI, REST API ve OpenAI SDK kullanarak üzerinde çıkarım yapmak.

## Genel Bakış

Foundry Local, önceden derlenmiş seçilmiş bir model kataloğu ile gelir, ancak bu liste ile sınırlı değilsiniz. [Hugging Face](https://huggingface.co/) üzerinde bulunan (veya yerel olarak PyTorch / Safetensors formatında depolanan) herhangi bir transformer tabanlı dil modeli, optimize edilmiş bir ONNX modeline derlenebilir ve Foundry Local üzerinden sunulabilir.

Derleme hattı, `onnxruntime-genai` paketine dahil olan komut satırı aracı olan **ONNX Runtime GenAI Model Builder** kullanır. Model oluşturucu, kaynak ağırlıkların indirilmesi, ONNX formatına dönüştürülmesi, kuantizasyonun uygulanması (int4, fp16, bf16) ve Foundry Local'ın beklediği yapılandırma dosyalarının (sohbet şablonu ve tokenizer dahil) oluşturulması gibi ağır işleri halleder.

Bu laboratuvarda Hugging Face’den **Qwen/Qwen3-0.6B** modelini derleyecek, Foundry Local’a kaydedecek ve tamamen cihazınızda onunla sohbet edeceksiniz.

---

## Öğrenim Hedefleri

Bu laboratuvarın sonunda şunları yapabileceksiniz:

- Özel model derlemenin neden faydalı olduğunu ve ne zaman gerekebileceğini açıklamak
- ONNX Runtime GenAI model oluşturucusunu kurmak
- Bir Hugging Face modelini tek komutla optimize ONNX formatına derlemek
- Temel derleme parametrelerini (çalıştırma sağlayıcısı, hassasiyet) anlamak
- `inference_model.json` sohbet şablonu yapılandırma dosyasını oluşturmak
- Derlenmiş bir modeli Foundry Local önbelleğine eklemek
- CLI, REST API ve OpenAI SDK kullanarak özel model üzerinde çıkarım yapmak

---

## Ön Koşullar

| Gereksinim | Detaylar |
|-------------|---------|
| **Foundry Local CLI** | Kurulu ve `PATH`’inizde ([Bölüm 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI model oluşturucu için gerekli |
| **pip** | Python paket yöneticisi |
| **Disk alanı** | Kaynak ve derlenmiş model dosyaları için en az 5 GB boş alan |
| **Hugging Face hesabı** | Bazı modeller indirilmeden önce lisans kabulü gerektirir. Qwen3-0.6B Apache 2.0 lisansı kullanır ve ücretsizdir. |

---

## Çevre Kurulumu

Model derleme, birkaç büyük Python paketi gerektirir (PyTorch, ONNX Runtime GenAI, Transformers). Bu paketlerin sistem Python’unuza veya diğer projelere müdahale etmemesi için özel bir sanal ortam oluşturun.

```bash
# Depo kökünden
python -m venv .venv
```

Ortamı etkinleştirin:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Bağımlılık çözümleme sorunlarını önlemek için pip'i güncelleyin:

```bash
python -m pip install --upgrade pip
```

> **İpucu:** Daha önceki laboratuvarlardan `.venv` ortamınız varsa, yeniden kullanabilirsiniz. Devam etmeden önce etkinleştirildiğinden emin olun.

---

## Kavram: Derleme Hattı

Foundry Local modellerin ONNX formatında ve ONNX Runtime GenAI yapılandırması ile olmasını gerektirir. Hugging Face'deki çoğu açık kaynak model PyTorch veya Safetensors ağırlıkları olarak dağıtılır, bu nedenle bir dönüştürme adımı gereklidir.

![Özel model derleme hattı](../../../images/custom-model-pipeline.svg)

### Model Oluşturucu Ne Yapar?

1. Kaynak modeli Hugging Face’den indirir (veya yerel bir yoldan okur).
2. PyTorch / Safetensors ağırlıklarını ONNX formatına dönüştürür.
3. Bellek kullanımını azaltmak ve verimliliği artırmak için modelin hassasiyetini daha küçük bir seviyeye kuantize eder (örneğin int4).
4. Foundry Local’ın modeli yükleyip sunabilmesi için ONNX Runtime GenAI yapılandırması (`genai_config.json`), sohbet şablonu (`chat_template.jinja`) ve tüm tokenizer dosyalarını oluşturur.

### ONNX Runtime GenAI Model Builder ve Microsoft Olive Karşılaştırması

Modellerin optimizasyonu için alternatif bir araç olarak **Microsoft Olive** referansları ile karşılaşabilirsiniz. Her iki araç da ONNX modelleri üretebilir ancak farklı amaçlara hizmet eder ve farklı ödünler verir:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paket** | `onnxruntime-genai` | `olive-ai` |
| **Birincil amaç** | ONNX Runtime GenAI çıkarımı için üretici yapay zeka modellerini dönüştürme ve kuantize etme | Çok sayıda backend ve donanım hedefini destekleyen uçtan uca model optimizasyon çerçevesi |
| **Kullanım kolaylığı** | Tek komut — tek adım dönüştürme ve kuantizasyon | İş akışı tabanlı — YAML/JSON ile yapılandırılabilir çok aşamalı süreçler |
| **Çıktı formatı** | Foundry Local için hazır ONNX Runtime GenAI formatı | İş akışına bağlı olarak genel ONNX, ONNX Runtime GenAI veya diğer formatlar |
| **Donanım hedefleri** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN ve daha fazlası |
| **Kuantizasyon seçenekleri** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, artı grafik optimizasyonları, katman bazlı ayar |
| **Model kapsamı** | Üretici yapay zeka modelleri (LLM’ler, SLM’ler) | ONNX’ye dönüştürülebilen herhangi bir model (görsel, NLP, ses, multimodal) |
| **En iyi kullanım alanı** | Yerel çıkarım için hızlı tek model derleme | İnce ayar gerektiren üretim hatları |
| **Bağımlılık ayak izi** | Orta (PyTorch, Transformers, ONNX Runtime) | Daha büyük (Olive çerçevesi eklenir, iş akışına bağlı ekler) |
| **Foundry Local entegrasyonu** | Doğrudan — çıktı hemen uyumlu | `--use_ort_genai` bayrağı ve ek yapılandırma gerektirir |

> **Neden bu laboratuvar Model Builder kullanıyor:** Tek bir Hugging Face modelini derlemek ve Foundry Local’a kaydetmek için Model Builder en basit ve en güvenilir yoldur. Foundry Local’ın beklediği tam çıktı formatını tek komutla üretir. Eğer daha sonra hassas kuantizasyon, grafik modifikasyonu veya çok aşamalı ayar gibi gelişmiş optimizasyon özelliklerine ihtiyaç duyarsanız, Olive güçlü bir seçenek olacaktır. Daha fazla bilgi için [Microsoft Olive dokümantasyonuna](https://microsoft.github.io/Olive/) bakınız.

---

## Laboratuvar Alıştırmaları

### Alıştırma 1: ONNX Runtime GenAI Model Oluşturucuyu Kurun

Model oluşturucu aracını içeren ONNX Runtime GenAI paketini kurun:

```bash
pip install onnxruntime-genai
```

Model oluşturucunun mevcut olduğunu doğrulayın:

```bash
python -m onnxruntime_genai.models.builder --help
```

`-m` (model adı), `-o` (çıktı yolu), `-p` (hassasiyet) ve `-e` (çalıştırma sağlayıcısı) gibi parametreleri listeleyen yardım çıktısı görmelisiniz.

> **Not:** Model oluşturucu PyTorch, Transformers ve diğer paketlere bağımlıdır. Kurulum birkaç dakika sürebilir.

---

### Alıştırma 2: Qwen3-0.6B’yi CPU için Derleyin

Aşağıdaki komutu çalıştırarak Hugging Face’den Qwen3-0.6B modelini indirin ve int4 kuantizasyonlu CPU çıkarımı için derleyin:

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell):**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### Parametrelerin Anlamları

| Parametre | Amacı | Kullanılan Değer |
|-----------|---------|------------|
| `-m` | Hugging Face model kimliği veya yerel dizin yolu | `Qwen/Qwen3-0.6B` |
| `-o` | Derlenmiş ONNX modelinin kaydedileceği dizin | `models/qwen3` |
| `-p` | Derleme sırasında uygulanan kuantizasyon hassasiyeti | `int4` |
| `-e` | ONNX Runtime çalışma sağlayıcısı (hedef donanım) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face kimlik doğrulamasını atlar (genel modeller için uygun) | `hf_token=false` |

> **Ne kadar sürer?** Derleme süresi donanımınıza ve model boyutuna bağlıdır. Modern CPU’da Qwen3-0.6B int4 kuantizasyonla yaklaşık 5 ila 15 dakika sürer. Daha büyük modeller nispeten daha uzun sürer.

Komut tamamlandığında `models/qwen3` dizini içinde derlenmiş model dosyalarını görmelisiniz. Çıktıyı doğrulayın:

```bash
ls models/qwen3
```

Aşağıdaki dosyalar bulunmalıdır:
- `model.onnx` ve `model.onnx.data` — derlenmiş model ağırlıkları
- `genai_config.json` — ONNX Runtime GenAI yapılandırması
- `chat_template.jinja` — modelin sohbet şablonu (otomatik oluşturulmuş)
- `tokenizer.json`, `tokenizer_config.json` — tokenizer dosyaları
- Diğer sözlük ve yapılandırma dosyaları

---

### Alıştırma 3: GPU için Derleyin (İsteğe Bağlı)

CUDA destekli NVIDIA GPU’nuz varsa, daha hızlı çıkarım için GPU optimize edilmiş bir varyant derleyebilirsiniz:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Not:** GPU derleme için `onnxruntime-gpu` ve çalışan bir CUDA kurulumu gerekir. Bunlar mevcut değilse model oluşturucu hata bildirir. Bu alıştırmayı atlayabilir ve CPU varyantıyla devam edebilirsiniz.

#### Donanıma Özgü Derleme Referansı

| Hedef | Çalıştırma Sağlayıcısı (`-e`) | Önerilen Hassasiyet (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` veya `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` veya `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Hassasiyet Ödünleşmeleri

| Hassasiyet | Boyut | Hız | Kalite |
|-----------|------|-------|---------|
| `fp32` | En büyük | En yavaş | En yüksek doğruluk |
| `fp16` | Büyük | Hızlı (GPU) | Çok iyi doğruluk |
| `int8` | Küçük | Hızlı | Hafif doğruluk kaybı |
| `int4` | En küçük | En hızlı | Orta düzey doğruluk kaybı |

Yerel geliştirme için CPU’da `int4` en iyi hız ve kaynak kullanımı dengesini sağlar. Üretim kalitesinde çıktı için CUDA destekli GPU’da `fp16` tavsiye edilir.

---

### Alıştırma 4: Sohbet Şablonu Yapılandırmasını Oluşturun

Model oluşturucu çıktı dizininde otomatik olarak `chat_template.jinja` ve `genai_config.json` dosyalarını üretir. Ancak Foundry Local, modelinizin istemleri nasıl biçimlendireceğini anlamak için bir `inference_model.json` dosyasına da ihtiyaç duyar. Bu dosya, model adını ve kullanıcı mesajlarını doğru özel tokenlerle saran istem şablonunu tanımlar.

#### Adım 1: Derlenmiş Çıktıyı İnceleyin

Derlenmiş model dizininin içeriğini listeleyin:

```bash
ls models/qwen3
```

Aşağıdaki dosyalar olmalıdır:
- `model.onnx` ve `model.onnx.data` — derlenmiş model ağırlıkları
- `genai_config.json` — ONNX Runtime GenAI yapılandırması (otomatik oluşturulmuş)
- `chat_template.jinja` — modelin sohbet şablonu (otomatik oluşturulmuş)
- `tokenizer.json`, `tokenizer_config.json` — tokenizer dosyaları
- Diğer çeşitli yapılandırma ve sözlük dosyaları

#### Adım 2: inference_model.json Dosyasını Oluşturun

`inference_model.json` dosyası Foundry Local’a kullanıcı istemlerini nasıl biçimlendireceğini söyler. `generate_chat_template.py` adlı bir Python betiğini **depo kökünde** (models/ dizininin bulunduğu aynı dizin) oluşturun:

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Sohbet şablonunu çıkarmak için minimal bir konuşma oluşturun
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# inference_model.json yapısını oluşturun
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

Betik kök dizinden çalıştırın:

```bash
python generate_chat_template.py
```

> **Not:** `transformers` paketi `onnxruntime-genai` bağımlısı olarak zaten kurulmuştur. `ImportError` alırsanız önce `pip install transformers` komutunu çalıştırın.

Betik, `models/qwen3` dizini içinde `inference_model.json` dosyasını üretir. Bu dosya Foundry Local’a Qwen3 için kullanıcı girdisini doğru özel tokenlerle nasıl sarmalaması gerektiğini söyler.

> **Önemli:** `inference_model.json` içindeki `"Name"` alanı (bu betikte `qwen3-0.6b` olarak ayarlanmıştır), sonraki tüm komutlar ve API çağrılarında kullanacağınız **model takma adıdır**. Bu adı değiştirirseniz Alıştırma 6–10’daki model adını da güncelleyin.

#### Adım 3: Yapılandırmayı Doğrulayın

`models/qwen3/inference_model.json` dosyasını açın ve içinde `Name` alanı ile `assistant` ve `prompt` anahtarlarına sahip `PromptTemplate` nesnesi olduğunu doğrulayın. İstem şablonu `<|im_start|>` ve `<|im_end|>` gibi özel tokenlar içermelidir (kesin tokenlar modelin sohbet şablonuna bağlıdır).

> **Manuel alternatif:** Betik çalıştırmak istemiyorsanız dosyayı kendiniz oluşturabilirsiniz. Önemli olan `prompt` alanının kullanıcının mesajı için `{Content}` yer tutucusunu içeren modelin tam sohbet şablonunu barındırmasıdır.

---

### Alıştırma 5: Model Dizin Yapısını Doğrulayın
Model oluşturucu, derlenmiş tüm dosyaları belirttiğiniz çıktı dizinine doğrudan yerleştirir. Nihai yapının doğru göründüğünden emin olun:

```bash
ls models/qwen3
```

Dizin aşağıdaki dosyaları içermelidir:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **Not:** Bazı diğer derleme araçlarının aksine, model oluşturucu iç içe alt dizinler oluşturmaz. Tüm dosyalar doğrudan çıktı klasöründe bulunur, bu da Foundry Local'ın beklediği tam yapıdır.

---

### Alıştırma 6: Modeli Foundry Local Önbelleğine Ekleme

Derlenmiş modelinizi Foundry Local'ın bulabilmesi için dizini önbelleğine ekleyin:

```bash
foundry cache cd models/qwen3
```

Modelin önbellekte göründüğünü doğrulayın:

```bash
foundry cache ls
```

Özel modelinizin, daha önce önbelleğe alınmış modellerle (örneğin `phi-3.5-mini` veya `phi-4-mini`) birlikte listelendiğini görmelisiniz.

---

### Alıştırma 7: CLI ile Özel Modeli Çalıştırma

Yeni derlediğiniz model ile etkileşimli bir sohbet oturumu başlatın (`qwen3-0.6b` takma adı, `inference_model.json` dosyasındaki `Name` alanından gelir):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` bayrağı, ek tanılama bilgileri gösterir; bu, özel bir modeli ilk kez test ederken faydalıdır. Model başarıyla yüklenirse etkileşimli bir istem görürsünüz. Birkaç mesaj deneyin:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Oturumu sonlandırmak için `exit` yazabilir veya `Ctrl+C` tuşlarına basabilirsiniz.

> **Sorun Giderme:** Model yüklenemezse aşağıdakileri kontrol edin:
> - `genai_config.json` dosyasının model oluşturucu tarafından üretildiği.
> - `inference_model.json` dosyasının mevcut ve geçerli JSON olduğu.
> - ONNX model dosyalarının doğru dizinde bulunduğu.
> - Yeterli RAM’e sahip olduğunuz (Qwen3-0.6B int4 için yaklaşık 1 GB gereklidir).
> - Qwen3, `<think>` etiketleri üreten bir akıl yürütme modelidir. Yanıtların başında `<think>...</think>` görüyorsanız, bu normal bir davranıştır. `inference_model.json` içindeki istem şablonu, düşünme çıktısını bastırmak için ayarlanabilir.

---

### Alıştırma 8: REST API Üzerinden Özel Modeli Sorgulama

Alıştırma 7’deki etkileşimli oturumu kapattıysanız, model artık yüklenmemiş olabilir. Foundry Local hizmetini başlatın ve modeli önce yükleyin:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Hizmetin hangi portta çalıştığını kontrol edin:

```bash
foundry service status
```

Ardından bir istek gönderin (eğer farklıysa `5273` numarasını gerçek portunuzla değiştirin):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows notu:** Yukarıdaki `curl` komutu bash sözdizimi kullanır. Windows’ta bunun yerine PowerShell `Invoke-RestMethod` cmdlet'ini kullanın.

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### Alıştırma 9: Özel Modeli OpenAI SDK ile Kullanma

Özel modelinize, yerleşik modeller için kullandığınız aynı OpenAI SDK kodunu kullanarak bağlanabilirsiniz (bakınız [Bölüm 3](part3-sdk-and-apis.md)). Tek fark model adıdır.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local API anahtarlarını doğrulamaz
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local API anahtarlarını doğrulamaz
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Ana nokta:** Foundry Local, OpenAI uyumlu bir API sunduğundan, yerleşik modellerle çalışan her kod, özel modellerle de çalışır. Sadece `model` parametresini değiştirmeniz yeterlidir.

---

### Alıştırma 10: Özel Modeli Foundry Local SDK ile Test Etme

Önceki laboratuvarlarda Foundry Local SDK kullanarak hizmeti başlatıp uç nokta keşfi yapıp modeli otomatik olarak yönetmiştiniz. Aynı yaklaşımı özel derlenmiş modelinizle de uygulayabilirsiniz. SDK, hizmeti başlatma ve uç nokta keşfini halleder, bu nedenle kodunuz `localhost:5273` gibi port numarasını sabit kodlamak zorunda değildir.

> **Not:** Örnekleri çalıştırmadan önce Foundry Local SDK'nın yüklü olduğundan emin olun:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` ve `OpenAI` NuGet paketlerini ekleyin
>
> Her betik dosyasını **depo kök dizinine** kaydedin (yani `models/` klasörünüzle aynı dizin).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Adım 1: Foundry Local servisini başlatın ve özel modeli yükleyin
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Adım 2: Özel model için önbelleği kontrol edin
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Adım 3: Modeli belleğe yükleyin
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Adım 4: SDK tarafından bulunan uç noktayı kullanarak bir OpenAI istemcisi oluşturun
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Adım 5: Akışlı sohbet tamamlama isteği gönderin
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

Çalıştırın:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// Adım 1: Foundry Local servisini başlat
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Adım 2: Katalogdan özel modeli al
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Adım 3: Modeli belleğe yükle
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Adım 4: SDK tarafından bulunan uç nokta kullanılarak bir OpenAI istemcisi oluştur
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Adım 5: Akışlı sohbet tamamlama isteği gönder
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

Çalıştırın:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **Ana nokta:** Foundry Local SDK, uç noktayı dinamik olarak keşfeder, böylece port numarasını asla sabit kodlamazsınız. Bu, üretim uygulamaları için önerilen yaklaşımdır. Özel derlenmiş modeliniz, SDK üzerinden yerleşik katalog modelleriyle tamamen aynı şekilde çalışır.

---

## Derlenecek Model Seçimi

Bu laboratuvarda referans örnek olarak Qwen3-0.6B kullanılmıştır çünkü küçük, hızlı derlenir ve Apache 2.0 lisansı altında ücretsizdir. Ancak birçok diğer modeli de derleyebilirsiniz. İşte bazı öneriler:

| Model | Hugging Face ID | Parametreler | Lisans | Notlar |
|-------|-----------------|--------------|--------|--------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Çok küçük, hızlı derleme, test için uygun |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Daha iyi kalite, yine hızlı derlenir |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Güçlü kalite, daha fazla RAM gerektirir |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face’de lisans kabulü gerekir |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Yüksek kalite, daha büyük indirme ve uzun derleme süresi |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Zaten Foundry Local kataloğunda (karşılaştırma için faydalı) |

> **Lisans hatırlatması:** Kullanımdan önce modelin Hugging Face üzerindeki lisansını mutlaka kontrol edin. Bazı modeller (örneğin Llama) lisans anlaşmasını kabul etmenizi ve indirmeden önce `huggingface-cli login` ile kimlik doğrulaması yapmanızı gerektirir.

---

## Kavramlar: Özel Modelleri Ne Zaman Kullanmalı

| Senaryo | Neden Kendi Modelinizi Derlemelisiniz? |
|---------|----------------------------------------|
| **İhtiyacınız olan model katalogda yoksa** | Foundry Local kataloğu seçkidir. İstediğiniz model listede değilse kendiniz derleyin. |
| **İnce ayarlı modeller** | Alanınıza özgü verilere göre ince ayar yapılmış bir modeliniz varsa, kendi ağırlıklarınızı derlemeniz gerekir. |
| **Özel kuantizasyon gereksinimleri** | Kataloğun varsayılanından farklı hassasiyet veya kuantizasyon stratejisi isteyebilirsiniz. |
| **Yeni model sürümleri** | Hugging Face’de yeni bir model yayınlandığında, Foundry Local kataloğunda henüz olmayabilir. Kendiniz derlemekle hemen erişebilirsiniz. |
| **Araştırma ve deneyler** | Üretim seçimine karar vermeden önce farklı model mimarileri, boyutları veya konfigürasyonlarını yerelde denemek için. |

---

## Özet

Bu laboratuvarda şunları öğrendiniz:

| Adım | Yaptığınız İşlem |
|------|------------------|
| 1 | ONNX Runtime GenAI model oluşturucusunu yüklediniz |
| 2 | `Qwen/Qwen3-0.6B` modelini Hugging Face’den optimize edilmiş ONNX formatına derlediniz |
| 3 | `inference_model.json` sohbet şablonu yapılandırma dosyası oluşturdunuz |
| 4 | Derlenmiş modeli Foundry Local önbelleğine eklediniz |
| 5 | Özel model ile CLI üzerinden etkileşimli sohbet başlattınız |
| 6 | OpenAI uyumlu REST API ile modeli sorguladınız |
| 7 | Python, JavaScript ve C# dillerinde OpenAI SDK ile bağlandınız |
| 8 | Foundry Local SDK ile özel modeli uçtan uca test ettiniz |

Temel çıkarım: **Herhangi bir transformer tabanlı model, ONNX formatına derlendikten sonra Foundry Local üzerinden çalışabilir.** OpenAI uyumlu API sayesinde mevcut uygulama kodunuz değişmeden çalışır; sadece model adını değiştirirsiniz.

---

## Temel Noktalar

| Kavram | Detay |
|--------|-------|
| ONNX Runtime GenAI Model Oluşturucu | Hugging Face modellerini tek komutla ONNX formatına kuantizasyon ile dönüştürür |
| ONNX formatı | Foundry Local, ONNX Runtime GenAI konfigürasyonlu ONNX modelleri ister |
| Sohbet şablonları | `inference_model.json` dosyası, belirli model için istemlerin nasıl oluşturulacağını Foundry Local’a bildirir |
| Donanım hedefleri | Donanımınıza bağlı olarak CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) veya WebGPU için derleyin |
| Kuantizasyon | Düşük hassasiyet (int4), boyutu küçültür ve hızı artırır; kesinlik kaybı olabilir; fp16 ise GPU’da yüksek kaliteyi korur |
| API uyumluluğu | Özel modeller, yerleşik modellerle aynı OpenAI uyumlu API’yi kullanır |
| Foundry Local SDK | SDK, hizmet başlatma, uç nokta keşfi ve model yüklemeyi hem katalog hem özel modeller için otomatik yapar |

---

## Daha Fazla Kaynak

| Kaynak | Bağlantı |
|--------|----------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local özel model rehberi | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 model ailesi | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive dokümantasyonu | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Sonraki Adımlar

Yerel modellerinizin dış işlevleri çağırmasını etkinleştirmeyi öğrenmek için [Bölüm 11: Yerel Modellerle Araç Çağrısı](part11-tool-calling.md)’na devam edin.

[← Bölüm 9: Whisper Ses Transkripsiyonu](part9-whisper-voice-transcription.md) | [Bölüm 11: Araç Çağrısı →](part11-tool-calling.md)