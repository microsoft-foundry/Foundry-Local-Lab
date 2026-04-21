![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Част 9: Преписване на глас с Whisper и Foundry Local

> **Цел:** Използване на модела OpenAI Whisper, работещ локално чрез Foundry Local, за преписване на аудио файлове - изцяло на устройството, без нужда от облак.

## Преглед

Foundry Local не е само за генериране на текст; той поддържа и модели за **глас към текст**. В този лабораторен упражнения ще използвате модела **OpenAI Whisper Medium**, за да транскрибирате аудио файлове изцяло на своя компютър. Това е идеално за ситуации като транскрибирането на клиентски повиквания на Zava, записвания на продуктови ревюта или планиране на работилници, където аудио данните никога не трябва да напускат устройството ви.


---

## Учебни цели

Към края на тази лаборатория ще можете да:

- Разбирате модела за глас към текст Whisper и неговите възможности
- Изтеглите и пуснете модела Whisper чрез Foundry Local
- Транскрибирате аудио файлове с помощта на Foundry Local SDK на Python, JavaScript и C#
- Създадете проста услуга за транскрипция, която работи изцяло на устройството
- Разберете разликите между чат/текст моделите и аудио моделите във Foundry Local

---

## Изисквания

| Изискване | Подробности |
|-----------|-------------|
| **Foundry Local CLI** | Версия **0.8.101 или по-нова** (Whisper моделите са налични от v0.8.101 нататък) |
| **Операционна система** | Windows 10/11 (x64 или ARM64) |
| **Среда за програмиране** | **Python 3.9+** и/или **Node.js 18+** и/или **.NET 9 SDK** ([Изтегли .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Завършени** | [Част 1: Първи стъпки](part1-getting-started.md), [Част 2: Foundry Local SDK Задълбочен Преглед](part2-foundry-local-sdk.md), и [Част 3: SDK-та и API-та](part3-sdk-and-apis.md) |

> **Забележка:** Whisper моделите трябва да се изтеглят чрез **SDK** (не чрез CLI). CLI не поддържа крайна точка за аудио транскрипция. Проверете версията си с:
> ```bash
> foundry --version
> ```

---

## Концепция: Как Whisper работи с Foundry Local

Моделът OpenAI Whisper е универсален модел за разпознаване на глас, обучен върху голям набор от разнообразни аудио данни. Когато се използва чрез Foundry Local:

- Моделът работи **изцяло на вашия процесор** - не е необходима видеокарта
- Аудиото никога не напуска устройството ви - **пълна поверителност**
- Foundry Local SDK управлява изтеглянето и кеширането на модели
- **JavaScript и C#** предоставят вграден `AudioClient` в SDK, който се грижи за целия процес на транскрипция — не е необходима ръчна настройка на ONNX
- **Python** използва SDK за управление на модела и ONNX Runtime за директно извеждане срещу encoder/decoder ONNX моделите

### Как работи процесът (JavaScript и C#) — SDK AudioClient

1. **Foundry Local SDK** изтегля и кешира модела Whisper
2. `model.createAudioClient()` (JS) или `model.GetAudioClientAsync()` (C#) създава `AudioClient`
3. `audioClient.transcribe(path)` (JS) или `audioClient.TranscribeAudioAsync(path)` (C#) вътрешно управлява целия процес — аудио предпроцесиране, енкодер, декодер и разкодиране на токени
4. `AudioClient` предоставя свойство `settings.language` (зададено на `"en"` за английски), което насочва към точна транскрипция

### Как работи процесът (Python) — ONNX Runtime

1. **Foundry Local SDK** изтегля и кешира Whisper ONNX файловете
2. **Аудио предпроцесиране** преобразува WAV аудиото в мел-спектрограма (80 мел ленти x 3000 кадъра)
3. **Енкодер** обработва мел-спектрограмата и генерира скрити състояния плюс ключ/стойност тензори за кръстосано внимание
4. **Декодер** функционира автoрегресивно, генерирайки един токен наведнъж до достигане на край на текста
5. **Токенизатор** декодира ID-та на изходните токени обратно в четим текст

### Варианти на модела Whisper

| Псевдоним | ID на модела | Устройство | Размер | Описание |
|-----------|-------------|------------|--------|----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Ускорен с GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Оптимизиран за CPU (препоръчително за повечето устройства) |

> **Забележка:** За разлика от чат моделите, които са изброени по подразбиране, Whisper моделите са категоризирани под задачата `automatic-speech-recognition`. Използвайте `foundry model info whisper-medium`, за да видите подробности.

---

## Лабораторни упражнения

### Упражнение 0 - Вземане на примерни аудио файлове

Тази лаборатория включва предварително изградени WAV файлове, базирани на сценарии с продукти на Zava DIY. Генерирайте ги с включения скрипт:

```bash
# От корена на репото - първо създайте и активирайте .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Това създава шест WAV файла в `samples/audio/`:

| Файл | Сценарий |
|------|----------|
| `zava-customer-inquiry.wav` | Клиент пита за **Zava ProGrip безжична бормашина** |
| `zava-product-review.wav` | Клиентски отзив за **Zava UltraSmooth интериорна боя** |
| `zava-support-call.wav` | Обаждане за поддръжка за **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | Планиране на палуба с **Zava EcoBoard композитна палуба** |
| `zava-workshop-setup.wav` | Преглед на работилница с използване на **всички пет продукта на Zava** |
| `zava-full-project-walkthrough.wav` | Разширен преглед на гаражна реконструкция с използването на **всички продукти на Zava** (~4 минути, за тестване на дълги аудиозаписи) |

> **Съвет:** Можете също да използвате свои WAV/MP3/M4A файлове или да запишете с Windows Voice Recorder.

---

### Упражнение 1 - Изтеглете модела Whisper с помощта на SDK

Поради несъвместимости с CLI при Whisper моделите в по-нови версии на Foundry Local, използвайте **SDK** за изтегляне и зареждане на модела. Изберете своя език:

<details>
<summary><b>🐍 Python</b></summary>

**Инсталиране на SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Стартирайте услугата
manager = FoundryLocalManager()
manager.start_service()

# Проверете информацията на каталога
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Проверете дали вече е кеширано
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Заредете модела в паметта
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Запазете като `download_whisper.py` и изпълнете:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Инсталиране на SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Създайте мениджър и стартирайте услугата
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Вземете модел от каталога
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

// Заредете модела в паметта
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Запазете като `download-whisper.mjs` и изпълнете:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Инсталиране на SDK:**
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

> **Защо SDK вместо CLI?** Foundry Local CLI не поддържа директно изтегляне или обслужване на Whisper модели. SDK предоставя надежден начин за програмно изтегляне и управление на аудио модели. JavaScript и C# SDK включват вграден `AudioClient`, който управлява целия процес на транскрипция. Python използва ONNX Runtime за директно извеждане срещу кешираните файлове.

---

### Упражнение 2 - Разберете SDK-то на Whisper

Преписването с Whisper използва различни подходи в зависимост от езика. **JavaScript и C#** предоставят вграден `AudioClient` в Foundry Local SDK, който управлява пълния процес (аудио предпроцесиране, енкодер, декодер, разкодиране на токени) с един метод. **Python** използва Foundry Local SDK за управление на моделите и ONNX Runtime за директно извеждане срещу encoder/decoder ONNX моделите.

| Компонент | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK пакети** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Управление на модел** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + каталог |
| **Извличане на характеристики** | `WhisperFeatureExtractor` + `librosa` | Управлявано от SDK `AudioClient` | Управлявано от SDK `AudioClient` |
| **Извеждане** | `ort.InferenceSession` (енкодер + декодер) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Декодиране на токени** | `WhisperTokenizer` | Управлявано от SDK `AudioClient` | Управлявано от SDK `AudioClient` |
| **Настройка на език** | Задава се чрез `forced_ids` в токените на декодера | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Вход** | Път към WAV файл | Път към WAV файл | Път към WAV файл |
| **Изход** | Декодиран текстов низ | `result.text` | `result.Text` |

> **Важно:** Винаги настройвайте езика в `AudioClient` (напр. `"en"` за английски). Без изрична настройка на езика моделът може да произвежда неразбираем текст, докато се опитва да открие езика автоматично.

> **SDK модели:** Python използва `FoundryLocalManager(alias)` за стартиране, след което `get_cache_location()` за намиране на ONNX файловете. JavaScript и C# използват вградения `AudioClient` на SDK — създаден чрез `model.createAudioClient()` (JS) или `model.GetAudioClientAsync()` (C#) — който управлява целия процес на транскрипция. Вижте [Част 2: Foundry Local SDK Задълбочен Преглед](part2-foundry-local-sdk.md) за пълни подробности.

---

### Упражнение 3 - Създайте проста транскрипционна апликация

Изберете своя езиков път и създайте минимално приложение, което транскрибира аудио файл.

> **Поддържани аудио формати:** WAV, MP3, M4A. За най-добри резултати използвайте WAV файлове с честота на дискретизация 16kHz.

<details>
<summary><h3>Python път</h3></summary>

#### Настройка

```bash
cd python
python -m venv venv

# Активирайте виртуалната среда:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Код за транскрипция

Създайте файл `foundry-local-whisper.py`:

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

# Стъпка 1: Bootstrap - стартира услуга, изтегля и зарежда модела
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Построяване на път към кешираните ONNX файлове на модела
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Стъпка 2: Зареждане на ONNX сесии и екстрактор на характеристики
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

# Стъпка 3: Извличане на характеристики на мел спектрограма
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Стъпка 4: Стартиране на енкодера
enc_out = encoder.run(None, {"audio_features": input_features})
# Първият изход са скрити състояния; останалите са двойки ключ-стойност за кръстосано внимание
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Стъпка 5: Автрегресивно декодиране
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, транскрибиране, без отметки за време
input_ids = np.array([initial_tokens], dtype=np.int32)

# Празен кеш за ключ-стойност на самовнимание
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

    if next_token == 50257:  # край на текста
        break
    generated.append(next_token)

    # Актуализация на кеша за ключ-стойност на самовнимание
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Стартиране

```bash
# Транскрибирайте сценарий на продукт на Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Или опитайте други:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Ключови точки за Python

| Метод | Цел |
|--------|---------|
| `FoundryLocalManager(alias)` | Стартиране: стартиране на услуга, изтегляне и зареждане на модел |
| `manager.get_cache_location()` | Връща пътя към кешираните ONNX файлове |
| `WhisperFeatureExtractor.from_pretrained()` | Зарежда екстрактора за мел спектрограми |
| `ort.InferenceSession()` | Създава ONNX Runtime сесии за енкодер и декодер |
| `tokenizer.decode()` | Преобразува ID на токени обратно в текст |

</details>

<details>
<summary><h3>JavaScript път</h3></summary>

#### Настройка

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Код за транскрипция

Създайте файл `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Стъпка 1: Стартиране - създайте мениджър, стартирайте услугата и заредете модела
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

// Стъпка 2: Създайте аудио клиент и транскрибирайте
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Изчистване
await model.unload();
```

> **Забележка:** Foundry Local SDK предоставя вграден `AudioClient` чрез `model.createAudioClient()`, който управлява цялата ONNX инференция вътрешно — не е нужна импорт на `onnxruntime-node`. Винаги задавайте `audioClient.settings.language = "en"`, за да осигурите точна английска транскрипция.

#### Стартиране

```bash
# Транскрибирайте сценарио за продукт на Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Или опитайте други:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Ключови точки за JavaScript

| Метод | Цел |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Създава мениджъра (singleton) |
| `await catalog.getModel(alias)` | Взима модел от каталога |
| `model.download()` / `model.load()` | Изтегля и зарежда Whisper модела |
| `model.createAudioClient()` | Създава аудио клиент за транскрипция |
| `audioClient.settings.language = "en"` | Задава езика за транскрипция (задължително за точен резултат) |
| `audioClient.transcribe(path)` | Транскрибира аудио файл, връща `{ text, duration }` |

</details>

<details>
<summary><h3>C# път</h3></summary>

#### Настройка

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Забележка:** C# пътят използва пакета `Microsoft.AI.Foundry.Local`, който предоставя вграден `AudioClient` чрез `model.GetAudioClientAsync()`. Той управлява целия процес на транскрипция вътрешно — не е нужна допълнителна настройка на ONNX Runtime.

#### Код за транскрипция

Заменете съдържанието на `Program.cs`:

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

#### Стартиране

```bash
# Транскрибиране на сценарий за продукт на Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Или опитайте други:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Ключови точки за C#

| Метод | Цел |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Инициализира Foundry Local с конфигурация |
| `catalog.GetModelAsync(alias)` | Взима модел от каталога |
| `model.DownloadAsync()` | Изтегля Whisper модела |
| `model.GetAudioClientAsync()` | Взима AudioClient (не ChatClient!) |
| `audioClient.Settings.Language = "en"` | Задава езика на транскрипция (задължително за точен резултат) |
| `audioClient.TranscribeAudioAsync(path)` | Транскрибира аудио файл |
| `result.Text` | Текстът на транскрипцията |
> **C# срещу Python/JS:** C# SDK предоставя вграден `AudioClient` за вътрешно транскрибиране чрез `model.GetAudioClientAsync()`, подобно на JavaScript SDK. Python използва ONNX Runtime директно за извършване на разпознаване спрямо кешираните енкодер/декодер модели.

</details>

---

### Упражнение 4 - Пакетно транскрибиране на всички Zava проби

Сега, след като имате работещо приложение за транскрибиране, транскрибирайте всичките пет Zava примерни файла и сравнете резултатите.

<details>
<summary><h3>Python писта</h3></summary>

Пълният пример `python/foundry-local-whisper.py` вече поддържа пакетно транскрибиране. При стартиране без аргументи, той транскрибира всички файлове `zava-*.wav` в `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Примерът използва `FoundryLocalManager(alias)` за инициализация, след което изпълнява ONNX сесиите за енкодер и декодер за всеки файл.

</details>

<details>
<summary><h3>JavaScript писта</h3></summary>

Пълният пример `javascript/foundry-local-whisper.mjs` вече поддържа пакетно транскрибиране. При стартиране без аргументи, той транскрибира всички файлове `zava-*.wav` в `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Примерът използва `FoundryLocalManager.create()` и `catalog.getModel(alias)` за инициализиране на SDK, след което използва `AudioClient` (с `settings.language = "en"`) за транскрибиране на всеки файл.

</details>

<details>
<summary><h3>C# писта</h3></summary>

Пълният пример `csharp/WhisperTranscription.cs` вече поддържа пакетно транскрибиране. При стартиране без конкретен файлов аргумент, той транскрибира всички файлове `zava-*.wav` в `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Примерът използва `FoundryLocalManager.CreateAsync()` и SDK-то `AudioClient` (с `Settings.Language = "en"`) за вътрешно транскрибиране.

</details>

**Какво да наблюдавате:** Сравнете изхода от транскрипцията със оригиналния текст в `samples/audio/generate_samples.py`. Колко точно Whisper улови имена на продукти като "Zava ProGrip" и технически термини като "brushless motor" или "composite decking"?

---

### Упражнение 5 - Разберете ключовите кодови шаблони

Изследвайте как транскрипцията с Whisper се различава от чат завършванията във всички три езика:

<details>
<summary><b>Python - Ключови различия от чат</b></summary>

```python
# Допълване на чат (Части 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Транскрипция на аудио (Тази част):
# Използва ONNX Runtime директно вместо OpenAI клиента
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... автогенериращ цикъл на декодера ...
print(tokenizer.decode(generated_tokens))
```

**Ключово прозрение:** Чат моделите използват API, съвместимо с OpenAI, чрез `manager.endpoint`. Whisper използва SDK-то за намиране на кешираните ONNX моделни файлове и след това изпълнява разпознаване директно чрез ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Ключови различия от чат</b></summary>

```javascript
// Допълване на чат (Части 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Аудио транскрипция (Тази част):
// Използва вградения AudioClient на SDK-то
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Винаги задавайте език за най-добри резултати
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Ключово прозрение:** Чат моделите използват API, съвместимо с OpenAI, чрез `manager.urls[0] + "/v1"`. Транскрипцията с Whisper използва SDK-то `AudioClient`, получен от `model.createAudioClient()`. Задайте `settings.language`, за да избегнете непонятен изход от авто-детекция.

</details>

<details>
<summary><b>C# - Ключови различия от чат</b></summary>

Подходът в C# използва вградения `AudioClient` на SDK-то за вътрешно транскрибиране:

**Инициализация на модела:**

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

**Транскрипция:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Ключово прозрение:** C# използва `FoundryLocalManager.CreateAsync()` и взема `AudioClient` директно — не е нужна настройка на ONNX Runtime. Задайте `Settings.Language`, за да избегнете непонятен изход от авто-детекция.

</details>

> **Обобщение:** Python използва Foundry Local SDK за управление на моделите и ONNX Runtime за директна работа с енкодер/декодер моделите. JavaScript и C# двата използват вградения `AudioClient` на SDK-то за опростено транскрибиране – създайте клиента, задайте езика и извикайте `transcribe()` / `TranscribeAudioAsync()`. Винаги задавайте езика в AudioClient за точни резултати.

---

### Упражнение 6 - Експериментирай

Опитайте тези модификации, за да задълбочите разбирането си:

1. **Опитайте различни аудио файлове** - запишете себе си с Windows Voice Recorder, запазете като WAV и го транскрибирайте

2. **Сравнете варианти на моделите** - ако имате NVIDIA GPU, опитайте CUDA варианта:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Сравнете скоростта на транскрипция с CPU варианта.

3. **Добавете форматиране на изхода** - JSON отговорът може да включва:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Изградете REST API** - обвийте своя код за транскрипция в уеб сървър:

   | Език | Фреймуърк | Пример |
   |------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` с `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` с `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` с `IFormFile` |

5. **Многостъпков разговор с транскрипция** - комбинирайте Whisper с чат агент от Част 4: първо транскрибирайте аудиото, след това предайте текста на агент за анализ или обобщение.

---

## Справочник за Audio API на SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — създава инстанция на `AudioClient`
> - `audioClient.settings.language` — задава езика за транскрипция (напр. `"en"`)
> - `audioClient.settings.temperature` — контролира случайността (по избор)
> - `audioClient.transcribe(filePath)` — транскрибира файл, връща `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — стриймва транскрипционни части чрез callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — създава инстанция на `OpenAIAudioClient`
> - `audioClient.Settings.Language` — задава езика за транскрипция (напр. `"en"`)
> - `audioClient.Settings.Temperature` — контролира случайността (по избор)
> - `await audioClient.TranscribeAudioAsync(filePath)` — транскрибира файл, връща обект с `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — връща `IAsyncEnumerable` от транскрипционни части

> **Съвет:** Винаги задавайте езика преди транскрипция. Без зададен език Whisper моделът се опитва автоматично разпознаване, което може да даде неразбираем изход (един символ за замяна вместо текст).

---

## Сравнение: Чат модели срещу Whisper

| Аспект | Чат Модели (Части 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|-----------------|-------------------|
| **Тип задача** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Вход** | Текстови съобщения (JSON) | Аудио файлове (WAV/MP3/M4A) | Аудио файлове (WAV/MP3/M4A) |
| **Изход** | Генериран текст (поточно) | Транскрибиран текст (пълен) | Транскрибиран текст (пълен) |
| **SDK пакет** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API метод** | `client.chat.completions.create()` | ONNX Runtime директно | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Настройка на езика** | Няма | Токени на подканващия декодер | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Стрийминг** | Да | Не | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Предимство за поверителност** | Код/данни остават локални | Аудиоданните остават локални | Аудиоданните остават локални |

---

## Основни изводи

| Концепция | Научено от вас |
|-----------|----------------|
| **Whisper локално на устройството** | Разпознаването на реч в текст се изпълнява изцяло локално, идеално за транскрибиране на обаждания на клиенти на Zava и продуктови ревюта на устройството |
| **SDK AudioClient** | JavaScript и C# SDK предоставят вграден `AudioClient`, който обработва целия процес на транскрипция с едно извикване |
| **Настройка на езика** | Винаги задавайте езика на AudioClient (например `"en"`) — без това авто-детекцията може да даде неразбираем изход |
| **Python** | Използва `foundry-local-sdk` за управление на моделите + `onnxruntime` + `transformers` + `librosa` за директен ONNX inference |
| **JavaScript** | Използва `foundry-local-sdk` с `model.createAudioClient()` — задава `settings.language`, след което извиква `transcribe()` |
| **C#** | Използва `Microsoft.AI.Foundry.Local` с `model.GetAudioClientAsync()` — задава `Settings.Language`, след което извиква `TranscribeAudioAsync()` |
| **Поддръжка на стрийминг** | JS и C# SDK също предоставят `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` за изход на части |
| **Оптимизирано за CPU** | CPU вариантът (3.05 GB) работи на всяко Windows устройство без GPU |
| **Поверителност на първо място** | Перфектно за запазване на взаимодействията с клиенти на Zava и защитените продуктови данни локално |

---

## Ресурси

| Ресурс | Връзка |
|--------|--------|
| Документация Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Справочник Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper модел | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Уебсайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Следваща стъпка

Продължете към [Част 10: Използване на собствени или Hugging Face модели](part10-custom-models.md), за да компилирате собствени модели от Hugging Face и да ги изпълните чрез Foundry Local.