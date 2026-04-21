![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Часть 9: Распознавание голоса с Whisper и Foundry Local

> **Цель:** Использовать модель OpenAI Whisper, работающую локально через Foundry Local, для расшифровки аудиофайлов — полностью на устройстве, без подключения к облаку.

## Обзор

Foundry Local предназначен не только для генерации текста; он также поддерживает модели **речь-в-текст**. В этой лабораторной работе вы будете использовать модель **OpenAI Whisper Medium** для полной расшифровки аудиофайлов на вашем устройстве. Это идеально подходит для таких сценариев, как расшифровка звонков в службу поддержки Zava, записей обзоров продуктов или сессий планирования семинаров, где аудиоданные ни в коем случае не должны покидать ваше устройство.

---

## Цели обучения

К концу этой лабораторной работы вы сможете:

- Понимать модель распознавания речи Whisper и её возможности
- Скачать и запустить модель Whisper через Foundry Local
- Расшифровывать аудиофайлы с использованием Foundry Local SDK на Python, JavaScript и C#
- Создать простое приложение для расшифровки, работающее полностью на устройстве
- Понимать различия между чат/текст моделями и аудиомоделями в Foundry Local

---

## Требования

| Требование | Подробности |
|------------|-------------|
| **Foundry Local CLI** | Версия **0.8.101 или выше** (модели Whisper доступны с версии v0.8.101 и новее) |
| **Операционная система** | Windows 10/11 (x64 или ARM64) |
| **Среда выполнения** | **Python 3.9+** и/или **Node.js 18+** и/или **.NET 9 SDK** ([Скачать .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Пройдено** | [Часть 1: Начало работы](part1-getting-started.md), [Часть 2: Глубокое изучение Foundry Local SDK](part2-foundry-local-sdk.md), и [Часть 3: SDK и API](part3-sdk-and-apis.md) |

> **Примечание:** Модели Whisper необходимо скачивать через **SDK** (не через CLI). CLI не поддерживает конечную точку для аудиотранскрипции. Проверьте свою версию командой:
> ```bash
> foundry --version
> ```

---

## Концепция: Как Whisper работает с Foundry Local

Модель OpenAI Whisper — универсальная модель распознавания речи, обученная на большом наборе разнообразных аудиозаписей. При работе через Foundry Local:

- Модель работает **полностью на вашем процессоре (CPU)** — никакого GPU не требуется
- Аудио никогда не покидает ваше устройство — **полная конфиденциальность**
- Foundry Local SDK управляет загрузкой модели и кешированием
- **JavaScript и C#** предоставляют встроенный `AudioClient` в SDK, который обрабатывает всю конвейерную цепочку транскрипции — настройка ONNX вручную не нужна
- **Python** использует SDK для управления моделью и ONNX Runtime для прямого вывода по ONNX моделям энкодера/декодера

### Как работает конвейер (JavaScript и C#) — SDK AudioClient

1. **Foundry Local SDK** скачивает и кэширует модель Whisper
2. `model.createAudioClient()` (JS) или `model.GetAudioClientAsync()` (C#) создаёт `AudioClient`
3. `audioClient.transcribe(path)` (JS) или `audioClient.TranscribeAudioAsync(path)` (C#) обрабатывает весь конвейер — препроцессинг аудио, энкодер, декодер и декодирование токенов
4. `AudioClient` предоставляет свойство `settings.language` (установите в `"en"` для английского), чтобы обеспечить точную транскрипцию

### Как работает конвейер (Python) — ONNX Runtime

1. **Foundry Local SDK** скачивает и кэширует ONNX файлы модели Whisper
2. **Препроцессинг аудио** преобразует WAV в мел-спектрограмму (80 мел-банок × 3000 кадров)
3. **Энкодер** обрабатывает мел-спектрограмму и выдаёт скрытые состояния и ключевые/значения для кросс-внимания
4. **Декодер** автогресивно генерирует токены по одному, пока не появится токен конца текста
5. **Токенизатор** декодирует выходные ID токенов обратно в читаемый текст

### Варианты моделей Whisper

| Псевдоним | ID модели | Устройство | Размер | Описание |
|-----------|-----------|------------|--------|----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 ГБ | Ускорение на GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 ГБ | Оптимизировано для CPU (рекомендуется для большинства устройств) |

> **Примечание:** В отличие от чат-моделей, которые показываются по умолчанию, модели Whisper классифицированы под задачей `automatic-speech-recognition`. Используйте `foundry model info whisper-medium` для просмотра деталей.

---

## Упражнения лабораторной работы

### Упражнение 0 — Получить пример аудиофайлов

В этой лабораторной работе включены предварительно созданные WAV-файлы, основанные на сценариях DIY от Zava. Сгенерируйте их с помощью включённого скрипта:

```bash
# Из корня репозитория - сначала создайте и активируйте .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Это создаст шесть WAV-файлов в `samples/audio/`:

| Файл | Сценарий |
|------|----------|
| `zava-customer-inquiry.wav` | Клиент спрашивает про **беспроводную дрель Zava ProGrip** |
| `zava-product-review.wav` | Отзыв клиента о **краске для интерьера Zava UltraSmooth** |
| `zava-support-call.wav` | Звонок в поддержку по **ящику инструментов Zava TitanLock** |
| `zava-project-planning.wav` | Планирование террасы с **композитной террасной доской Zava EcoBoard** |
| `zava-workshop-setup.wav` | Обход мастерской с использованием **всех пяти продуктов Zava** |
| `zava-full-project-walkthrough.wav` | Подробный обзор ремонта гаража с использованием **всех продуктов Zava** (~4 минуты, для тестирования длинного аудио) |

> **Совет:** Вы также можете использовать собственные WAV/MP3/M4A файлы или записать себя с помощью Windows Voice Recorder.

---

### Упражнение 1 — Скачать модель Whisper через SDK

Из-за несовместимостей CLI с моделями Whisper в новых версиях Foundry Local используйте **SDK** для скачивания и загрузки модели. Выберите свой язык:

<details>
<summary><b>🐍 Python</b></summary>

**Установите SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Запустить службу
manager = FoundryLocalManager()
manager.start_service()

# Проверить информацию каталога
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Проверить, если уже в кэше
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Загрузить модель в память
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Сохраните как `download_whisper.py` и запустите:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Установите SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Создайте менеджера и запустите службу
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Получите модель из каталога
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

// Загрузите модель в память
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Сохраните как `download-whisper.mjs` и запустите:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Установите SDK:**
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

> **Почему SDK, а не CLI?** CLI Foundry Local не поддерживает загрузку и обслуживание моделей Whisper напрямую. SDK предоставляет надёжный способ программно скачивать и управлять аудиомоделями. JavaScript и C# SDK включают встроенный `AudioClient`, который обрабатывает весь конвейер транскрипции. Python использует ONNX Runtime для прямого вывода по закешированным файлам модели.

---

### Упражнение 2 — Понимание Whisper SDK

Транскрипция Whisper использует разные подходы в зависимости от языка. **JavaScript и C#** предоставляют встроенный `AudioClient` в Foundry Local SDK, который обрабатывает весь конвейер (препроцессинг аудио, энкодер, декодер, декодирование токенов) в одном методе. **Python** использует Foundry Local SDK для управления моделью и ONNX Runtime для прямого вывода по ONNX моделям энкодера/декодера.

| Компонент | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **Пакеты SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Управление моделью** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + каталог |
| **Извлечение признаков** | `WhisperFeatureExtractor` + `librosa` | Обрабатывается SDK `AudioClient` | Обрабатывается SDK `AudioClient` |
| **Инференс** | `ort.InferenceSession` (энкодер + декодер) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Декодирование токенов** | `WhisperTokenizer` | Обрабатывается SDK `AudioClient` | Обрабатывается SDK `AudioClient` |
| **Настройка языка** | Устанавливается через `forced_ids` в токенах декодера | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Входные данные** | Путь к WAV-файлу | Путь к WAV-файлу | Путь к WAV-файлу |
| **Выходные данные** | Раскодированная строка текста | `result.text` | `result.Text` |

> **Важно:** Всегда указывайте язык в `AudioClient` (например, `"en"` для английского). Без явной установки языка модель может выдать нечитаемый результат, пытаясь автоматически определить язык.

> **Паттерны SDK:** Python использует `FoundryLocalManager(alias)` для инициализации, затем `get_cache_location()` для поиска ONNX файлов модели. JavaScript и C# используют встроенный `AudioClient` SDK — получаемый через `model.createAudioClient()` (JS) или `model.GetAudioClientAsync()` (C#) — который обрабатывает весь конвейер транскрипции. Подробности в [Часть 2: Глубокое изучение Foundry Local SDK](part2-foundry-local-sdk.md).

---

### Упражнение 3 — Создайте простое приложение для транскрипции

Выберите свой язык и создайте минимальное приложение для расшифровки аудиофайла.

> **Поддерживаемые аудиоформаты:** WAV, MP3, M4A. Для наилучшего результата используйте WAV с частотой дискретизации 16 кГц.

<details>
<summary><h3>Python</h3></summary>

#### Настройка

```bash
cd python
python -m venv venv

# Активировать виртуальное окружение:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Код транскрипции

Создайте файл `foundry-local-whisper.py`:

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

# Шаг 1: Bootstrap - запуск сервиса, загрузка и загрузка модели
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Построить путь к кэшированным файлам модели ONNX
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Шаг 2: Загрузить сессии ONNX и извлекатель признаков
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

# Шаг 3: Извлечение признаков мел-спектрограммы
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Шаг 4: Запуск кодировщика
enc_out = encoder.run(None, {"audio_features": input_features})
# Первый вывод - скрытые состояния; остальные - пары KV кросс-внимания
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Шаг 5: Автогрессирующее декодирование
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, транскрипция, без отметок времени
input_ids = np.array([initial_tokens], dtype=np.int32)

# Пустой кэш KV для самовнимания
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

    if next_token == 50257:  # конец текста
        break
    generated.append(next_token)

    # Обновить кэш KV для самовнимания
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Запуск

```bash
# Расшифровать сценарий продукта Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Или попробуйте другие:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Ключевые моменты Python

| Метод | Назначение |
|--------|-------------|
| `FoundryLocalManager(alias)` | Инициализация: запуск сервиса, загрузка и загрузка модели |
| `manager.get_cache_location()` | Получение пути к кешу ONNX модели |
| `WhisperFeatureExtractor.from_pretrained()` | Загрузка извлекатель признаков мел-спектрограммы |
| `ort.InferenceSession()` | Создание сессий ONNX Runtime для энкодера и декодера |
| `tokenizer.decode()` | Преобразование ID токенов обратно в текст |

</details>

<details>
<summary><h3>JavaScript</h3></summary>

#### Настройка

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Код транскрипции

Создайте файл `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Шаг 1: Загрузка - создать менеджера, запустить сервис и загрузить модель
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

// Шаг 2: Создать аудиоклиента и выполнять транскрипцию
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Очистка
await model.unload();
```

> **Примечание:** Foundry Local SDK предоставляет встроенный `AudioClient` через `model.createAudioClient()`, который обрабатывает весь конвейер инференса ONNX внутри — импорт `onnxruntime-node` не требуется. Всегда устанавливайте `audioClient.settings.language = "en"` для точной транскрипции на английском.

#### Запуск

```bash
# Транскрибировать сценарий продукта Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Или попробуйте другие:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Ключевые моменты JavaScript

| Метод | Назначение |
|--------|-------------|
| `FoundryLocalManager.create({ appName })` | Создать синглтон менеджера |
| `await catalog.getModel(alias)` | Получить модель из каталога |
| `model.download()` / `model.load()` | Скачать и загрузить модель Whisper |
| `model.createAudioClient()` | Создать аудиоклиент для транскрипции |
| `audioClient.settings.language = "en"` | Установить язык транскрипции (необходимо для точного результата) |
| `audioClient.transcribe(path)` | Транскрибировать аудиофайл, возвращает `{ text, duration }` |

</details>

<details>
<summary><h3>C#</h3></summary>

#### Настройка

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Примечание:** В треке C# используется пакет `Microsoft.AI.Foundry.Local`, который предоставляет встроенный `AudioClient` через `model.GetAudioClientAsync()`. Это обрабатывает весь конвейер транскрипции непосредственно — отдельная установка ONNX Runtime не требуется.

#### Код транскрипции

Замените содержимое `Program.cs`:

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

#### Запуск

```bash
# Транскрибировать сценарий продукта Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Или попробуйте другие:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Ключевые моменты C#

| Метод | Назначение |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config)` | Инициализация Foundry Local с конфигурацией |
| `catalog.GetModelAsync(alias)` | Получение модели из каталога |
| `model.DownloadAsync()` | Загрузка модели Whisper |
| `model.GetAudioClientAsync()` | Получение `AudioClient` (не `ChatClient`!) |
| `audioClient.Settings.Language = "en"` | Установка языка транскрипции (необходимо для точного результата) |
| `audioClient.TranscribeAudioAsync(path)` | Транскрибирование аудиофайла |
| `result.Text` | Транскрибированный текст |


> **C# vs Python/JS:** SDK C# предоставляет встроенный `AudioClient` для транскрипции в процессе через `model.GetAudioClientAsync()`, аналогично SDK на JavaScript. Python использует ONNX Runtime напрямую для вывода на кэшированных моделях энкодера/декодера.

</details>

---

### Упражнение 4 - Пакетная транскрипция всех образцов Zava

Теперь, когда у вас есть работащее приложение для транскрипции, распознайте все пять файлов образцов Zava и сравните результаты.

<details>
<summary><h3>Python трек</h3></summary>

Полный пример `python/foundry-local-whisper.py` уже поддерживает пакетную транскрипцию. При запуске без аргументов он транскрибирует все файлы `zava-*.wav` в `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Пример использует `FoundryLocalManager(alias)` для инициализации, затем запускает ONNX-сессии энкодера и декодера для каждого файла.

</details>

<details>
<summary><h3>JavaScript трек</h3></summary>

Полный пример `javascript/foundry-local-whisper.mjs` уже поддерживает пакетную транскрипцию. При запуске без аргументов он транскрибирует все файлы `zava-*.wav` в `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Пример использует `FoundryLocalManager.create()` и `catalog.getModel(alias)` для инициализации SDK, затем применяет `AudioClient` (с `settings.language = "en"`) для транскрипции каждого файла.

</details>

<details>
<summary><h3>C# трек</h3></summary>

Полный пример `csharp/WhisperTranscription.cs` уже поддерживает пакетную транскрипцию. При запуске без указанного файла он транскрибирует все файлы `zava-*.wav` в `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Пример использует `FoundryLocalManager.CreateAsync()` и SDK `AudioClient` (с `Settings.Language = "en"`) для транскрипции в процессе.

</details>

**На что обращать внимание:** Сравните вывод транскрипции с оригинальным текстом в `samples/audio/generate_samples.py`. Насколько точно Whisper распознаёт названия продуктов вроде "Zava ProGrip" и технические термины, например, "brushless motor" или "composite decking"?

---

### Упражнение 5 - Понимание ключевых шаблонов кода

Изучите, как транскрипция Whisper отличается от chat-комплишенов на всех трёх языках:

<details>
<summary><b>Python - ключевые отличия от чата</b></summary>

```python
# Завершение чата (Части 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Транскрипция аудио (Эта часть):
# Использует ONNX Runtime напрямую вместо клиента OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... цикл авторегрессивного декодера ...
print(tokenizer.decode(generated_tokens))
```

**Ключевое понимание:** Чат-модели используют OpenAI-совместимый API через `manager.endpoint`. Whisper использует SDK для поиска кэшированных файлов моделей ONNX и запускает вывод напрямую через ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - ключевые отличия от чата</b></summary>

```javascript
// Завершение чата (Части 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Транскрипция аудио (Эта часть):
// Использует встроенный AudioClient SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Всегда устанавливайте язык для лучших результатов
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Ключевое понимание:** Чат-модели используют OpenAI-совместимый API через `manager.urls[0] + "/v1"`. Транскрипция Whisper использует `AudioClient` SDK, полученный через `model.createAudioClient()`. Чтобы избежать искажённого вывода от автодетекции, установите `settings.language`.

</details>

<details>
<summary><b>C# - ключевые отличия от чата</b></summary>

Подход C# использует встроенный SDK `AudioClient` для транскрипции в процессе:

**Инициализация модели:**

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

**Ключевое понимание:** В C# используется `FoundryLocalManager.CreateAsync()` и непосредственное получение `AudioClient` — настройка ONNX Runtime не требуется. Установите `Settings.Language`, чтобы избежать искажения при автодетекции.

</details>

> **Итог:** Python применяет Foundry Local SDK для управления моделями и ONNX Runtime для прямого вывода на моделях энкодера/декодера. JavaScript и C# используют встроенный SDK `AudioClient` для упрощённой транскрипции — создайте клиент, установите язык и вызовите `transcribe()` / `TranscribeAudioAsync()`. Всегда задавайте свойство языка у AudioClient для точных результатов.

---

### Упражнение 6 - Эксперименты

Попробуйте следующие изменения, чтобы углубить понимание:

1. **Испытайте разные аудиофайлы** — запишите себя с помощью Windows Voice Recorder, сохраните в WAV и транскрибируйте

2. **Сравните варианты моделей** — если у вас есть NVIDIA GPU, попробуйте CUDA-вариант:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Сравните скорость транскрипции с CPU-версией.

3. **Добавьте форматирование вывода** — JSON-ответ может включать:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Постройте REST API** — оберните код транскрипции в веб-сервер:

   | Язык | Фреймворк | Пример |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` с `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` с `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` с `IFormFile` |

5. **Многошаговый диалог с транскрипцией** — комбинируйте Whisper с чат-агентом из Части 4: сначала транскрибируйте аудио, затем передавайте текст агенту для анализа или суммирования.

---

## Справочник SDK Audio API

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — создаёт экземпляр `AudioClient`
> - `audioClient.settings.language` — задаёт язык транскрипции (например, `"en"`)
> - `audioClient.settings.temperature` — управление случайностью (опционально)
> - `audioClient.transcribe(filePath)` — транскрибирует файл, возвращает `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — потоковая транскрипция с передачей чанков через callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — создаёт экземпляр `OpenAIAudioClient`
> - `audioClient.Settings.Language` — задаёт язык транскрипции (например, `"en"`)
> - `audioClient.Settings.Temperature` — управление случайностью (опционально)
> - `await audioClient.TranscribeAudioAsync(filePath)` — транскрибирует файл, возвращает объект с `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — возвращает `IAsyncEnumerable` с чанками транскрипции

> **Совет:** Всегда задавайте свойство языка перед транскрипцией. Без него модель Whisper пытается автоопределять язык, что может привести к искажённому выводу (один символ замены вместо текста).

---

## Сравнение: Чат-модели vs Whisper

| Аспект | Чат-модели (Части 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|------------------|-------------------|
| **Тип задачи** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Входные данные** | Текстовые сообщения (JSON) | Аудиофайлы (WAV/MP3/M4A) | Аудиофайлы (WAV/MP3/M4A) |
| **Выходные данные** | Сгенерированный текст (потоково) | Транскрибированный текст (полный) | Транскрибированный текст (полный) |
| **Пакет SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Метод API** | `client.chat.completions.create()` | Прямой ONNX Runtime | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Настройка языка** | Нет | Токены подсказки декодера | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Потоковая обработка** | Да | Нет | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Преимущество конфиденциальности** | Код/данные остаются локально | Аудиоданные остаются локально | Аудиоданные остаются локально |

---

## Основные выводы

| Концепция | Что вы узнали |
|---------|-------------------|
| **Whisper на устройстве** | Распознавание речи происходит полностью локально, идеально для транскрибирования звонков и обзоров продуктов Zava на устройстве |
| **SDK AudioClient** | SDK для JavaScript и C# предоставляют встроенный `AudioClient`, автоматизирующий полный процесс транскрипции в одном вызове |
| **Настройка языка** | Всегда задавайте язык в AudioClient (например, `"en"`) — без этого автодетекция может выдать искажённый вывод |
| **Python** | Использует `foundry-local-sdk` для управления моделями + `onnxruntime` + `transformers` + `librosa` для прямого вывода ONNX |
| **JavaScript** | Использует `foundry-local-sdk` с `model.createAudioClient()` — задаёт `settings.language`, затем вызывает `transcribe()` |
| **C#** | Использует `Microsoft.AI.Foundry.Local` с `model.GetAudioClientAsync()` — задаёт `Settings.Language`, затем вызывает `TranscribeAudioAsync()` |
| **Поддержка потоковой передачи** | SDK JS и C# поддерживают `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` для покадрового вывода |
| **Оптимизация под CPU** | Версия для CPU (3.05 ГБ) работает на любом Windows-устройстве без GPU |
| **Конфиденциальность прежде всего** | Идеально подходит для сохранения взаимодействия с клиентами Zava и данных о продуктах на устройстве |

---

## Ресурсы

| Ресурс | Ссылка |
|----------|--------|
| Документация Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Справочник SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Модель OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Сайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Следующий шаг

Продолжайте к [Часть 10: Использование пользовательских моделей или Hugging Face](part10-custom-models.md), чтобы собирать собственные модели с Hugging Face и запускать их через Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с использованием сервиса машинного перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на стремление к точности, пожалуйста, имейте в виду, что автоматический перевод может содержать ошибки или неточности. Оригинальный документ на языке оригинала следует считать авторитетным источником. Для получения критически важной информации рекомендуется обратиться к профессиональному человеческому переводу. Мы не несем ответственности за любые недоразумения или неправильные толкования, возникшие в результате использования этого перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->