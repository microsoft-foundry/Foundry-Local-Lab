![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Частина 9: Транскрипція голосу за допомогою Whisper і Foundry Local

> **Мета:** Використовувати модель OpenAI Whisper, що працює локально через Foundry Local, для транскрипції аудіофайлів — повністю на пристрої, хмара не потрібна.

## Огляд

Foundry Local — це не просто генерація тексту; він також підтримує **моделі розпізнавання мовлення у текст**. У цій лабораторній роботі ви використаєте модель **OpenAI Whisper Medium** для повної транскрипції аудіофайлів на вашому комп’ютері. Це ідеально підходить для сценаріїв, як транскрипція дзвінків служби підтримки Zava, записів оглядів продуктів або сесій планування майстерень, де аудіодані ніколи не повинні покидати ваш пристрій.

---

## Цілі навчання

По завершенню цієї лабораторної роботи ви зможете:

- Розуміти модель Whisper для перетворення мовлення у текст і її можливості
- Завантажувати та запускати модель Whisper за допомогою Foundry Local
- Транскрибувати аудіофайли, використовуючи Foundry Local SDK у Python, JavaScript та C#
- Створити простий сервіс транскрипції, який працює повністю на пристрої
- Розуміти відмінності між чат-/текстовими моделями та аудіомоделями у Foundry Local

---

## Необхідні умови

| Вимога | Деталі |
|-------------|---------|
| **Foundry Local CLI** | Версія **0.8.101 або вище** (моделі Whisper доступні з версії v0.8.101) |
| **ОС** | Windows 10/11 (x64 або ARM64) |
| **Обчислювальне середовище** | **Python 3.9+** і/або **Node.js 18+** і/або **.NET 9 SDK** ([Завантажити .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Виконано** | [Частина 1: Початок роботи](part1-getting-started.md), [Частина 2: Поглиблене вивчення Foundry Local SDK](part2-foundry-local-sdk.md) та [Частина 3: SDK та API](part3-sdk-and-apis.md) |

> **Примітка:** Моделі Whisper потрібно завантажувати через **SDK** (не CLI). CLI не підтримує аудіо-транскрипційний кінцевий пункт. Перевірте вашу версію командою:
> ```bash
> foundry --version
> ```

---

## Концепція: Як Whisper працює з Foundry Local

Модель OpenAI Whisper — це модель розпізнавання мовлення загального призначення, навчена на великому різноманітному наборі аудіоданих. Запускаючись через Foundry Local:

- Модель працює **повністю на вашому ЦП** — без необхідності GPU
- Аудіо ніколи не покидає ваш пристрій — **повна конфіденційність**
- Foundry Local SDK керує завантаженням моделі та управлінням кешем
- **JavaScript та C#** надають вбудований `AudioClient` у SDK, який керує повним процесом транскрипції — жодної ручної настройки ONNX не потрібно
- **Python** використовує SDK для управління моделлю та ONNX Runtime для безпосереднього інференсу над ONNX-моделями енкодера/декодера

### Як працює конвеєр (JavaScript та C#) — SDK AudioClient

1. **Foundry Local SDK** завантажує та кешує модель Whisper
2. `model.createAudioClient()` (JS) або `model.GetAudioClientAsync()` (C#) створює `AudioClient`
3. `audioClient.transcribe(path)` (JS) або `audioClient.TranscribeAudioAsync(path)` (C#) внутрішньо виконує весь процес — попередня обробка аудіо, енкодер, декодер та декодування токенів
4. `AudioClient` має властивість `settings.language` (встановити в `"en"` для англійської) для точної транскрипції

### Як працює конвеєр (Python) — ONNX Runtime

1. **Foundry Local SDK** завантажує та кешує файли моделей Whisper ONNX
2. **Попередня обробка аудіо** конвертує WAV у мел-спектрограму (80 мел-бінів × 3000 кадрів)
3. **Енкодер** обробляє мел-спектрограму та видає приховані стани і тензори ключів/значень крос-уваги
4. **Декодер** працює авторегресивно, генеруючи по одному токену доки не випустить токен кінця тексту
5. **Токенізатор** декодує вихідні ID токенів назад у читабельний текст

### Варіанти моделей Whisper

| Псевдонім | ID моделі | Пристрій | Розмір | Опис |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 ГБ | З апаратним прискоренням на GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 ГБ | Оптимізовано для ЦП (рекомендується для більшості пристроїв) |

> **Примітка:** На відміну від чат-моделей, які показуються за замовчуванням, моделі Whisper категоризовані під завданням `automatic-speech-recognition`. Використовуйте `foundry model info whisper-medium` для перегляду деталей.

---

## Лабораторні вправи

### Вправа 0 - Отримати приклади аудіофайлів

У цій лабораторній роботі є готові WAV-файли, створені на основі сценаріїв продуктів Zava DIY. Згенеруйте їх за допомогою включеного скрипту:

```bash
# Від кореня репозиторію - спочатку створіть і активуйте .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Це створить шість WAV-файлів у `samples/audio/`:

| Файл | Сценарій |
|------|----------|
| `zava-customer-inquiry.wav` | Запит клієнта про **дриль Zava ProGrip бездротовий** |
| `zava-product-review.wav` | Огляд клієнтом **фарби для інтер'єру Zava UltraSmooth** |
| `zava-support-call.wav` | Дзвінок у службу підтримки щодо **ящика для інструментів Zava TitanLock** |
| `zava-project-planning.wav` | Планування настилу для DIY із **композитним настилом Zava EcoBoard** |
| `zava-workshop-setup.wav` | Огляд майстерні з використанням **всіх п’яти продуктів Zava** |
| `zava-full-project-walkthrough.wav` | Детальний огляд ремонту гаража з використанням **всіх продуктів Zava** (~4 хв, для тестування довгого аудіо) |

> **Порада:** Ви також можете використовувати свої WAV/MP3/M4A файли або записати себе за допомогою Windows Voice Recorder.

---

### Вправа 1 - Завантажити модель Whisper за допомогою SDK

Через несумісність CLI з моделями Whisper у новіших версіях Foundry Local, використовуйте **SDK** для завантаження та завантаження моделі. Оберіть вашу мову програмування:

<details>
<summary><b>🐍 Python</b></summary>

**Встановіть SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Запустити службу
manager = FoundryLocalManager()
manager.start_service()

# Перевірити інформацію каталогу
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Перевірити, чи вже кешовано
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Завантажити модель у пам'ять
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Збережіть як `download_whisper.py` і запустіть:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Встановіть SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Створити менеджера і запустити сервіс
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Отримати модель з каталогу
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

// Завантажити модель у пам'ять
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Збережіть як `download-whisper.mjs` і запустіть:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Встановіть SDK:**
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

> **Чому SDK, а не CLI?** CLI Foundry Local не підтримує пряме завантаження або роботу з моделями Whisper. SDK забезпечує надійний спосіб програмного завантаження і керування аудіомоделями. JavaScript та C# SDK містять вбудований `AudioClient` для повного процесу транскрипції. Python використовує ONNX Runtime для безпосереднього інференсу у закешованих файлах моделей.

---

### Вправа 2 - Розібратися з SDK для Whisper

Транскрипція Whisper використовує різні підходи залежно від мови. **JavaScript та C#** пропонують вбудований `AudioClient` у Foundry Local SDK, який виконує весь конвеєр (попередня обробка аудіо, енкодер, декодер, декодування токенів) одним викликом. **Python** використовує Foundry Local SDK для управління моделлю і ONNX Runtime для інференсу над моделями енкодера/декодера.

| Компонент | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK пакети** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Управління моделлю** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + каталог |
| **Виділення ознак** | `WhisperFeatureExtractor` + `librosa` | Керується SDK `AudioClient` | Керується SDK `AudioClient` |
| **Інференс** | `ort.InferenceSession` (енкодер + декодер) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Декодування токенів** | `WhisperTokenizer` | Керується SDK `AudioClient` | Керується SDK `AudioClient` |
| **Налаштування мови** | Встановлено через `forced_ids` у токенах декодера | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Вхідні дані** | Шлях до WAV файла | Шлях до WAV файла | Шлях до WAV файла |
| **Вихід** | Розкодований текстовий рядок | `result.text` | `result.Text` |

> **Важливо:** Завжди встановлюйте мову у `AudioClient` (наприклад, `"en"` для англійської). Без явного налаштування мови модель може видавати несмисловий текст, намагаючись автоматично визначати мову.

> **Шаблони SDK:** Python використовує `FoundryLocalManager(alias)` для ініціалізації, потім `get_cache_location()` для пошуку файлів моделей ONNX. JavaScript і C# використовують вбудований у SDK `AudioClient` — отриманий через `model.createAudioClient()` (JS) або `model.GetAudioClientAsync()` (C#) — який бере на себе весь конвеєр транскрипції. Деталі див. у [Частина 2: Поглиблене вивчення Foundry Local SDK](part2-foundry-local-sdk.md).

---

### Вправа 3 - Створити простий додаток для транскрипції

Виберіть вашу мову і створіть мінімальний додаток, який транскрибує аудіофайл.

> **Підтримувані аудіоформати:** WAV, MP3, M4A. Для кращих результатів використовуйте WAV-файли з частотою дискретизації 16kHz.

<details>
<summary><h3>Python трек</h3></summary>

#### Налаштування

```bash
cd python
python -m venv venv

# Активуйте віртуальне середовище:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Код транскрипції

Створіть файл `foundry-local-whisper.py`:

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

# Крок 1: Завантаження - запускає сервіс, завантажує та завантажує модель
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Побудова шляху до кешованих ONNX файлів моделей
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Крок 2: Завантаження ONNX сесій та екстрактора ознак
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

# Крок 3: Витягування ознак мел-спектрограми
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Крок 4: Запуск енкодера
enc_out = encoder.run(None, {"audio_features": input_features})
# Перший вихід - приховані стани; решта - пари ключів і значень крос-уваги
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Крок 5: Автопрогресивне декодування
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, транскрибувати, без часових позначок
input_ids = np.array([initial_tokens], dtype=np.int32)

# Порожній кеш ключів і значень власної уваги
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

    if next_token == 50257:  # кінець тексту
        break
    generated.append(next_token)

    # Оновити кеш ключів і значень власної уваги
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Запустіть його

```bash
# Транскрибувати сценарій продукту Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Або спробуйте інші:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Основні моменти Python

| Метод | Призначення |
|--------|---------|
| `FoundryLocalManager(alias)` | Ініціалізація: запуск сервісу, завантаження та завантаження моделі |
| `manager.get_cache_location()` | Отримати шлях до кешованих файлів моделей ONNX |
| `WhisperFeatureExtractor.from_pretrained()` | Завантажити екстрактор ознак мел-спектрограми |
| `ort.InferenceSession()` | Створити сесії ONNX Runtime для енкодера та декодера |
| `tokenizer.decode()` | Перетворити ID токенів у текст |

</details>

<details>
<summary><h3>JavaScript трек</h3></summary>

#### Налаштування

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Код транскрипції

Створіть файл `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Крок 1: Ініціалізація - створити менеджера, запустити сервіс і завантажити модель
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

// Крок 2: Створити аудіоклієнта та транскрибувати
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Прибирання
await model.unload();
```

> **Примітка:** Foundry Local SDK надає вбудований `AudioClient` через `model.createAudioClient()`, який внутрішньо керує всіма процесами інференсу ONNX – імпорт `onnxruntime-node` не потрібен. Завжди встановлюйте `audioClient.settings.language = "en"` для точної транскрипції англійською.

#### Запустіть його

```bash
# Транскрибувати сценарій продукту Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Або спробуйте інші:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Основні моменти JavaScript

| Метод | Призначення |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Створити синглтон менеджера |
| `await catalog.getModel(alias)` | Отримати модель з каталогу |
| `model.download()` / `model.load()` | Завантажити і завантажити модель Whisper |
| `model.createAudioClient()` | Створити клієнта аудіо для транскрипції |
| `audioClient.settings.language = "en"` | Встановити мову транскрипції (потрібно для точної роботи) |
| `audioClient.transcribe(path)` | Транскрибувати аудіофайл, повертає `{ text, duration }` |

</details>

<details>
<summary><h3>C# трек</h3></summary>

#### Налаштування

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Примітка:** У треці C# використовується пакет `Microsoft.AI.Foundry.Local`, який надає вбудований `AudioClient` через `model.GetAudioClientAsync()`. Він виконує повний конвеєр транскрипції у процесі — окреме налаштування ONNX Runtime не потрібне.

#### Код транскрипції

Замініть вміст `Program.cs`:

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

#### Запустіть його

```bash
# Транскрибувати сценарій продукту Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Або спробуйте інші:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Основні моменти C#

| Метод | Призначення |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Ініціалізувати Foundry Local з конфігурацією |
| `catalog.GetModelAsync(alias)` | Отримати модель із каталогу |
| `model.DownloadAsync()` | Завантажити модель Whisper |
| `model.GetAudioClientAsync()` | Отримати AudioClient (не ChatClient!) |
| `audioClient.Settings.Language = "en"` | Встановити мову транскрипції (потрібно для точної роботи) |
| `audioClient.TranscribeAudioAsync(path)` | Транскрибувати аудіофайл |
| `result.Text` | Транскрибований текст |
> **C# vs Python/JS:** SDK C# надає вбудований `AudioClient` для транскрипції в процесі через `model.GetAudioClientAsync()`, подібно до SDK JavaScript. Python безпосередньо використовує ONNX Runtime для інференсу на кешованих моделях енкодера/декодера.

</details>

---

### Вправа 4 - Пакетна транскрипція всіх зразків Zava

Тепер, коли у вас є робочий додаток для транскрипції, транскрибуйте всі п’ять зразків файлів Zava та порівняйте результати.

<details>
<summary><h3>Трек Python</h3></summary>

Повний приклад `python/foundry-local-whisper.py` вже підтримує пакетну транскрипцію. При запуску без аргументів він транскрибує всі файли `zava-*.wav` у `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

У прикладі використовується `FoundryLocalManager(alias)` для ініціалізації, потім для кожного файлу запускаються сесії енкодера та декодера ONNX.

</details>

<details>
<summary><h3>Трек JavaScript</h3></summary>

Повний приклад `javascript/foundry-local-whisper.mjs` вже підтримує пакетну транскрипцію. При запуску без аргументів він транскрибує всі файли `zava-*.wav` у `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

У прикладі використовується `FoundryLocalManager.create()` та `catalog.getModel(alias)` для ініціалізації SDK, потім для транскрипції кожного файлу використовується `AudioClient` (з `settings.language = "en"`).

</details>

<details>
<summary><h3>Трек C#</h3></summary>

Повний приклад `csharp/WhisperTranscription.cs` вже підтримує пакетну транскрипцію. При запуску без конкретного аргументу файлу він транскрибує всі файли `zava-*.wav` у `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

У прикладі використовується `FoundryLocalManager.CreateAsync()` та SDK `AudioClient` (з `Settings.Language = "en"`) для транскрипції у процесі.

</details>

**На що звернути увагу:** Порівняйте результат транскрипції з оригінальним текстом у `samples/audio/generate_samples.py`. Наскільки точно Whisper розпізнає назви продуктів, такі як "Zava ProGrip", і технічні терміни, як-от "brushless motor" або "composite decking"?

---

### Вправа 5 - Зрозумійте ключові патерни коду

Вивчіть, як транскрипція Whisper відрізняється від чат-завершень у всіх трьох мовах:

<details>
<summary><b>Python - ключові відмінності від чату</b></summary>

```python
# Завершення чат-сесії (Частини 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Транскрипція аудіо (Ця частина):
# Використовує ONNX Runtime безпосередньо замість клієнта OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... автовідтворюючий цикл декодера ...
print(tokenizer.decode(generated_tokens))
```

**Ключове розуміння:** Чат-моделі використовують сумісний з OpenAI API через `manager.endpoint`. Whisper використовує SDK для пошуку кешованих файлів моделей ONNX, а потім запускає інференс безпосередньо через ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - ключові відмінності від чату</b></summary>

```javascript
// Завершення чату (частини 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Транскрипція аудіо (ця частина):
// Використовує вбудований AudioClient SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Завжди встановлюйте мову для кращих результатів
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Ключове розуміння:** Чат-моделі використовують сумісний з OpenAI API через `manager.urls[0] + "/v1"`. Транскрипція Whisper використовує `AudioClient` SDK, отриманий із `model.createAudioClient()`. Встановіть `settings.language`, щоб уникнути спотворень через авто-виявлення.

</details>

<details>
<summary><b>C# - ключові відмінності від чату</b></summary>

Підхід C# використовує вбудований `AudioClient` SDK для транскрипції в процесі:

**Ініціалізація моделі:**

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

**Транскрипція:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Ключове розуміння:** C# використовує `FoundryLocalManager.CreateAsync()` і отримує `AudioClient` напряму — конфігурація ONNX Runtime не потрібна. Встановіть `Settings.Language`, щоб уникнути спотворень при авто-виявленні.

</details>

> **Резюме:** Python використовує Foundry Local SDK для керування моделями та ONNX Runtime для прямого інференсу на моделях енкодера/декодера. JavaScript та C# обидва використовують вбудований `AudioClient` SDK для спрощеної транскрипції — створіть клієнта, встановіть мову та викличте `transcribe()` / `TranscribeAudioAsync()`. Завжди задавайте властивість мови у AudioClient для точних результатів.

---

### Вправа 6 - Експериментуйте

Спробуйте ці зміни, щоб поглибити розуміння:

1. **Спробуйте інші аудіофайли** – запишіть свій голос за допомогою Windows Voice Recorder, збережіть у WAV та транскрибуйте

2. **Порівняйте варіанти моделі** – якщо у вас NVIDIA GPU, спробуйте варіант CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
 Зрівняйте швидкість транскрипції з CPU-варіантом.

3. **Додайте форматування виводу** – JSON відповіді можуть містити:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Побудуйте REST API** – обгорніть код транскрипції у веб-сервер:

   | Мова | Фреймворк | Приклад |
   |------|-----------|---------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` з `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` з `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` з `IFormFile` |

5. **Мультітур з транскрипцією** – об’єднайте Whisper з чат-агентом з Частини 4: спочатку транскрибуйте аудіо, потім передайте текст агенту для аналізу або підсумовування.

---

## SDK Audio API Reference

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — створює екземпляр `AudioClient`
> - `audioClient.settings.language` — встановлює мову транскрипції (наприклад, `"en"`)
> - `audioClient.settings.temperature` — керує рівнем випадковості (необов’язково)
> - `audioClient.transcribe(filePath)` — транскрибує файл, повертає `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — транскрибує аудіо потоковим режимом та повертає частини через callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — створює екземпляр `OpenAIAudioClient`
> - `audioClient.Settings.Language` — встановлює мову транскрипції (наприклад, `"en"`)
> - `audioClient.Settings.Temperature` — керує рівнем випадковості (необов’язково)
> - `await audioClient.TranscribeAudioAsync(filePath)` — транскрибує файл, повертає об’єкт з `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — повертає `IAsyncEnumerable` транскрипційних частин

> **Порада:** Завжди задавайте властивість мови перед транскрипцією. Без неї Whisper намагається визначити мову автоматично, що може спричинити спотворений вихід (єдиний символ заміни замість тексту).

---

## Порівняння: Чат-моделі та Whisper

| Аспект | Чат-моделі (Частини 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|--------------------------|-----------------|-------------------|
| **Тип завдання** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Вхідні дані** | Текстові повідомлення (JSON) | Аудіофайли (WAV/MP3/M4A) | Аудіофайли (WAV/MP3/M4A) |
| **Вихідні дані** | Згенерований текст (потоковий) | Транскрибований текст (повний) | Транскрибований текст (повний) |
| **Пакет SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Метод API** | `client.chat.completions.create()` | Прямий ONNX Runtime | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Налаштування мови** | Н/Д | Токени підказки декодера | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Потокове виконання** | Так | Ні | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Перевага конфіденційності** | Код/дані залишаються локально | Аудіодані залишаються локально | Аудіодані залишаються локально |

---

## Основні висновки

| Концепція | Що ви дізналися |
|-----------|------------------|
| **Whisper на пристрої** | Розпізнавання мовлення працює повністю локально, ідеально для транскрипції дзвінків клієнтів Zava та оглядів продуктів на пристрої |
| **SDK AudioClient** | SDK JavaScript і C# надають вбудований `AudioClient`, який обробляє весь процес транскрипції за один виклик |
| **Налаштування мови** | Завжди встановлюйте мову в AudioClient (наприклад, `"en"`) — без цього авто-виявлення може викликати спотворення |
| **Python** | Використовує `foundry-local-sdk` для керування моделями + `onnxruntime` + `transformers` + `librosa` для прямого ONNX інференсу |
| **JavaScript** | Використовує `foundry-local-sdk` з `model.createAudioClient()` — встановіть `settings.language`, потім викличте `transcribe()` |
| **C#** | Використовує `Microsoft.AI.Foundry.Local` з `model.GetAudioClientAsync()` — встановіть `Settings.Language`, потім викличте `TranscribeAudioAsync()` |
| **Підтримка потокової транскрипції** | SDK JS і C# також підтримують `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` для отримання частин по мірі обробки |
| **Оптимізація для CPU** | Варіант для CPU (3.05 ГБ) працює на будь-якому пристрої Windows без GPU |
| **Конфіденційність перш за все** | Ідеально для утримання взаємодій клієнтів Zava та пропрієтарних даних продукту на пристрої |

---

## Ресурси

| Ресурс | Посилання |
|---------|----------|
| Документація Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Посилання на SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper model | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Вебсайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Наступний крок

Продовжуйте до [Частини 10: Використання власних або Hugging Face моделей](part10-custom-models.md), щоб зібрати власні моделі з Hugging Face і запустити їх через Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Застереження**:  
Цей документ було перекладено за допомогою сервісу автоматичного перекладу штучного інтелекту [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, будь ласка, майте на увазі, що автоматичні переклади можуть містити помилки або неточності. Оригінальний документ мовою оригіналу слід вважати авторитетним джерелом. Для критично важливої інформації рекомендується професійний переклад людиною. Ми не несемо відповідальності за будь-які непорозуміння або неправильні тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->