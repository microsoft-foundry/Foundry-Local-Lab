![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deo 9: Претварање гласа у текст помоћу Whisper и Foundry Local

> **Циљ:** Користити OpenAI Whisper модел који ради локално преко Foundry Local за транскрипцију аудио фајлова - потпуно на уређају, без потребе за облаком.

## Преглед

Foundry Local није само за генерисање текста; он такође подржава моделе за **претварање говора у текст**. У овом туторијалу ћете користити **OpenAI Whisper Medium** модел да у потпуности трансрибујете аудио фајлове на вашем рачунару. Ово је идеално за сценарије као што су транскрипција позива корисничке службе Зава, снимака рецензија производа или сесија планирања радионице где аудио подаци никада не смеју напустити ваш уређај.

---

## Циљеви учења

На крају овог туторијала моћи ћете да:

- Разумете Whisper модел за претварање говора у текст и његове могућности
- Преузмете и покренете Whisper модел користећи Foundry Local
- Транскрибујете аудио фајлове помоћу Foundry Local SDK-а у Python-у, JavaScript-у и C#
- Направите једноставну услугу транскрипције која ради потпуно на уређају
- Разумете разлике између ћаскаћих/текстуалних модела и аудио модела у Foundry Local

---

## Захтеви

| Захтев | Детаљи |
|-------------|---------|
| **Foundry Local CLI** | Верзија **0.8.101 или новија** (Whisper модели су доступни од верзије 0.8.101 нагоре) |
| **Оперативни систем** | Windows 10/11 (x64 или ARM64) |
| **Извршна околина** | **Python 3.9+** и/или **Node.js 18+** и/или **.NET 9 SDK** ([Преузми .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Завршено** | [Део 1: Почетак рада](part1-getting-started.md), [Део 2: Foundry Local SDK детаљно](part2-foundry-local-sdk.md) и [Део 3: SDK-ови и API-ји](part3-sdk-and-apis.md) |

> **Напомена:** Whisper моделе треба преузети преко **SDK-а** (не CLI). CLI не подржава аудио транскрипцијски крајњи пункт. Проверите верзију са:
> ```bash
> foundry --version
> ```

---

## Концепт: Како Whisper ради са Foundry Local

OpenAI Whisper модел је општи модел за распознавање говора обучен на великом скупу разноврсних аудио снимака. Када ради кроз Foundry Local:

- Модел се извршава **потпуно на вашем CPU-у** - без потребе за GPU-ом
- Аудио никада не напушта ваш уређај - **потпуна приватност**
- Foundry Local SDK управља преузимањем и кеширањем модела
- **JavaScript и C#** пружају уграђени `AudioClient` у SDK-у који обрађује целу транскрипцијску цевоводну линију — није потребна ручна ONNX конфигурација
- **Python** користи SDK за управљање моделом и ONNX Runtime за директно извршење на енкодер/декодер ONNX моделима

### Како ради цевовод (JavaScript и C#) — SDK AudioClient

1. **Foundry Local SDK** преузима и кешира Whisper модел
2. `model.createAudioClient()` (JS) или `model.GetAudioClientAsync()` (C#) креира `AudioClient`
3. `audioClient.transcribe(path)` (JS) или `audioClient.TranscribeAudioAsync(path)` (C#) интерно обрађује целу цевоводну линију — претпрерада аудио сигнала, енкодер, декодер и декодирање токена
4. `AudioClient` има својство `settings.language` (подешено на `"en"` за енглески) за вођење тачне транскрипције

### Како ради цевовод (Python) — ONNX Runtime

1. **Foundry Local SDK** преузима и кешира Whisper ONNX датотеке модела
2. **Претпрерада аудио сигнала** конвертује WAV аудио у мел спектограм (80 мел канала x 3000 фрејмова)
3. **Енкодер** обрађује мел спектограм и производи скривене стања и тензоре кључ/вредност за крос-атенцију
4. **Декодер** ради ауторегресивно, генеришући један токен истовремено док не произведе ознаку краја текста
5. **Токенизатор** декодира ID-ове излазних токена назад у читљив текст

### Варијанте Whisper модела

| Алијас | ИД модела | Уређај | Величина | Опис |
|-------|----------|--------|---------|------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU убрзано (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Оптимизовано за CPU (препоручено за већину уређаја) |

> **Напомена:** За разлику од ћаскачих модела који се подразумевано листвују, Whisper модели су категорисани под задатком `automatic-speech-recognition`. Користите `foundry model info whisper-medium` за детаље.

---

## Лабораторијске вежбе

### Вежба 0 - Преузми пример аудио фајлова

Овај туторијал садржи унапред припремљене WAV фајлове базиране на Зава DIY производним сценаријима. Генеришите их помоћу приложеног скрипта:

```bash
# Из корена репозиторијума - прво направите и активирајте .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Ово креира шест WAV фајлова у `samples/audio/`:

| Фајл | Сценарио |
|------|----------|
| `zava-customer-inquiry.wav` | Купац пита о **Zava ProGrip бежичној бушилици** |
| `zava-product-review.wav` | Купац прегледа производ **Zava UltraSmooth унутрашњу боју** |
| `zava-support-call.wav` | Подршка на позиву у вези са **Zava TitanLock алатним орманом** |
| `zava-project-planning.wav` | Ради сам планира палубу са **Zava EcoBoard композитном палубом** |
| `zava-workshop-setup.wav` | Преглед радионице користећи **свих пет Зава производа** |
| `zava-full-project-walkthrough.wav` | Продужени преглед реновирања гараже користећи **све Зава производе** (~4 минута, за тестирање дугог аудио записа) |

> **Савет:** Такође можете користити своје WAV/MP3/M4A фајлове или снимати себе помоћу Windows Voice Recorder-а.

---

### Вежба 1 - Преузми Whisper модел користећи SDK

Због некомпатибилности CLI-а са Whisper моделима у новијим верзијама Foundry Local, користите **SDK** за преузимање и учитавање модела. Изаберите свој програмски језик:

<details>
<summary><b>🐍 Python</b></summary>

**Инсталирајте SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Покрени сервис
manager = FoundryLocalManager()
manager.start_service()

# Провери информације каталога
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Провери да ли је већ кеширано
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Учитај модел у меморију
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Сачувајте као `download_whisper.py` и покрените:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Инсталирајте SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Креирај менаџера и покрени сервис
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Преузми модел из каталога
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

// Учитај модел у меморију
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Сачувајте као `download-whisper.mjs` и покрените:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Инсталирајте SDK:**
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

> **Зашто SDK уместо CLI?** Foundry Local CLI не подржава директно преузимање или сервирање Whisper модела. SDK пружа поуздан начин за преузимање и управљање аудио моделима програмски. JavaScript и C# SDK имају уграђени `AudioClient` који обрађује целу транскрипцијску цевоводну линију. Python користи ONNX Runtime за директно извршење преко кешираног модела.

---

### Вежба 2 - Разумевање Whisper SDK

Whisper транскрипција користи различите приступе у зависности од језика. **JavaScript и C#** пружају уграђени `AudioClient` у Foundry Local SDK-у који обрађује цео процес (претпрерада аудио сигнала, енкодер, декодер, декодирање токена) у једном позиву методе. **Python** користи Foundry Local SDK за управљање моделом и ONNX Runtime за директну инференцију на енкодер/декодер ONNX моделима.

| Компонента | Python | JavaScript | C# |
|------------|--------|------------|----|
| **SDK пакети** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Управљање моделом** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + каталог |
| **Издвајање карактеристика** | `WhisperFeatureExtractor` + `librosa` | Обрађује SDK `AudioClient` | Обрађује SDK `AudioClient` |
| **Инференција** | `ort.InferenceSession` (енкодер + декодер) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Декодирање токена** | `WhisperTokenizer` | Обрађује SDK `AudioClient` | Обрађује SDK `AudioClient` |
| **Подешавање језика** | Поставља се кроз `forced_ids` у декодер токенима | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Улаз** | Путања до WAV фајла | Путања до WAV фајла | Путања до WAV фајла |
| **Излаз** | Декодирани текст | `result.text` | `result.Text` |

> **Важно:** Увек подесите језик на `AudioClient` (нпр. `"en"` за енглески). Без експлицитног подешавања језика, модел може дати недефинисан резултат јер покушава да аутоматски открије језик.

> **Обрасци SDK-а:** Python користи `FoundryLocalManager(alias)` за покретање, затим `get_cache_location()` за проналазак ONNX датотека модела. JavaScript и C# користе уграђени `AudioClient` SDK-а — добијен преко `model.createAudioClient()` (JS) или `model.GetAudioClientAsync()` (C#) — који управља целокупном транскрипцијом. Видети [Део 2: Foundry Local SDK детаљно](part2-foundry-local-sdk.md) за више детаља.

---

### Вежба 3 - Направите једноставну апликацију за транскрипцију

Изаберите језички део и направите минималну апликацију која транскрибује аудио фајл.

> **Подржани аудио формати:** WAV, MP3, M4A. За најбоље резултате користите WAV фајлове са фреквенцијом узорковања од 16 kHz.

<details>
<summary><h3>Python</h3></summary>

#### Подешавање

```bash
cd python
python -m venv venv

# Активирајте виртуелно окружење:
# Виндоус (ПоверШел):
venv\Scripts\Activate.ps1
# макОС:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Код за транскрипцију

Креирајте фајл `foundry-local-whisper.py`:

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

# Корак 1: Bootstrap - покреће сервис, преузима и учитава модел
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Направи путању до кешираних ONNX модела
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Корак 2: Учитај ONNX сесије и екстрактор карактеристика
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

# Корак 3: Израђује мел спектрограм карактеристике
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Корак 4: Покрени енкодер
enc_out = encoder.run(None, {"audio_features": input_features})
# Први излаз је скривено стање; остали су парови кључ-вредност за крос-атенцију
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Корак 5: Ауторегресивно дешифровање
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, транскрибуј, без ознака времена
input_ids = np.array([initial_tokens], dtype=np.int32)

# Празан кеш кључ-вредност за самопажњу
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

    if next_token == 50257:  # крај текста
        break
    generated.append(next_token)

    # Ажурирај кеш кључ-вредност за самопажњу
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Покретање

```bash
# Транскрибовати сценарио производа Зава
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Или пробај друге:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Кључне тачке за Python

| Метод | Намена |
|--------|---------|
| `FoundryLocalManager(alias)` | Покретање: старт сервиса, преузимање и учитавање модела |
| `manager.get_cache_location()` | Проналазак путање до кешираних ONNX датотека модела |
| `WhisperFeatureExtractor.from_pretrained()` | Учитавање извлачиоца карактеристика мел спектрограма |
| `ort.InferenceSession()` | Креирање сесија ONNX Runtime за енкодер и декодер |
| `tokenizer.decode()` | Конверзија ID-ова излазних токена у текст |

</details>

<details>
<summary><h3>JavaScript</h3></summary>

#### Подешавање

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Код за транскрипцију

Креирајте фајл `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Корак 1: Покрени - направи менаџера, покрени сервис и учитај модел
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

// Корак 2: Направи аудио клијента и транскрибуј
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Чишћење
await model.unload();
```

> **Напомена:** Foundry Local SDK пружа уграђени `AudioClient` преко `model.createAudioClient()` који интерно обрађује целу ONNX трансформацију — није потребно увозити `onnxruntime-node`. Увек подесите `audioClient.settings.language = "en"` за тачну транскрипцију на енглески.

#### Покретање

```bash
# Транскрибујте сценарио производа Зава
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Или испробајте друге:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Кључне тачке за JavaScript

| Метод | Намена |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Креирање синглтон менаџера |
| `await catalog.getModel(alias)` | Преузимање модела из каталога |
| `model.download()` / `model.load()` | Преузимање и учитавање Whisper модела |
| `model.createAudioClient()` | Креирање аудио клијента за транскрипцију |
| `audioClient.settings.language = "en"` | Подешавање језика транскрипције (обавезно за тачан резултат) |
| `audioClient.transcribe(path)` | Транскрипција аудио фајла, враћа `{ text, duration }` |

</details>

<details>
<summary><h3>C#</h3></summary>

#### Подешавање

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Напомена:** C# користи пакет `Microsoft.AI.Foundry.Local` који обезбеђује уграђени `AudioClient` преко `model.GetAudioClientAsync()`. Ово обрађује целу транскрипцијску цевоводну линију у процесу — није потребна посебна ONNX Runtime конфигурација.

#### Код за транскрипцију

Замена садржаја у фајлу `Program.cs`:

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

#### Покретање

```bash
# Пренесите сценарио производа Зава
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Или испробајте друге:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Кључне тачке за C#

| Метод | Намена |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Иницијализација Foundry Local са конфигурацијом |
| `catalog.GetModelAsync(alias)` | Преузимање модела из каталога |
| `model.DownloadAsync()` | Преузимање Whisper модела |
| `model.GetAudioClientAsync()` | Добијање AudioClient-а (не ChatClient-а!) |
| `audioClient.Settings.Language = "en"` | Подешавање језика транскрипције (обавезно за тачан излаз) |
| `audioClient.TranscribeAudioAsync(path)` | Транскрипција аудио фајла |
| `result.Text` | Претворени текст |


> **C# vs Python/JS:** C# SDK пружа уграђени `AudioClient` за транскрипцију у процесу преко `model.GetAudioClientAsync()`, слично као JavaScript SDK. Python користи ONNX Runtime директно за предвиђање над кешираним encoder/decoder моделима.

</details>

---

### Вежба 4 - Пакетна транскрипција свих Zava узорака

Сада када имате радну апликацију за транскрипцију, транскрибујте свих пет Zava пример датотека и упоредите резултате.

<details>
<summary><h3>Python ток</h3></summary>

Потпуни пример `python/foundry-local-whisper.py` већ подржава пакетну транскрипцију. Када се покрене без аргумената, транскрибује све `zava-*.wav` датотеке у `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Пример користи `FoundryLocalManager(alias)` за покретање, затим извршава encoder и decoder ONNX сесије за сваку датотеку.

</details>

<details>
<summary><h3>JavaScript ток</h3></summary>

Потпуни пример `javascript/foundry-local-whisper.mjs` већ подржава пакетну транскрипцију. Када се покрене без аргумената, транскрибује све `zava-*.wav` датотеке у `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Пример користи `FoundryLocalManager.create()` и `catalog.getModel(alias)` за иницијализацију SDK, затим користи `AudioClient` (са `settings.language = "en"`) за транскрипцију сваке датотеке.

</details>

<details>
<summary><h3>C# ток</h3></summary>

Потпуни пример `csharp/WhisperTranscription.cs` већ подржава пакетну транскрипцију. Када се покрене без специфичног аргумента за датотеку, транскрибује све `zava-*.wav` датотеке у `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Пример користи `FoundryLocalManager.CreateAsync()` и SDK-ов `AudioClient` (са `Settings.Language = "en"`) за транскрипцију у процесу.

</details>

**На шта обратити пажњу:** Упоредите излаз транскрипције са оригиналним текстом у `samples/audio/generate_samples.py`. Колико прецизно Whisper препознаје имена производа као што су "Zava ProGrip" и стручне термине као што су "brushless motor" или "composite decking"?

---

### Вежба 5 - Разумевање кључних образаца кода

Проучите како се Whisper транскрипција разликује од ћаскања (chat completions) у све три језика:

<details>
<summary><b>Python - Кључне разлике у односу на ћаскање</b></summary>

```python
# Допуњавање ћаскања (делови 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Транскрипција аудија (овaj део):
# Користи ONNX Runtime директно уместо OpenAI клијента
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... ауторегресивна петља декодера ...
print(tokenizer.decode(generated_tokens))
```

**Кључни увид:** Chat модели користе OpenAI-компатибилни API преко `manager.endpoint`. Whisper користи SDK да пронађе кеширане ONNX модел датотеке, а затим директно покреће предвиђање са ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Кључне разлике у односу на ћаскање</b></summary>

```javascript
// Допуна ћаскања (делови 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Аудио транскрипција (овај део):
// Користи уграђени AudioClient у SDK-у
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Увек подесите језик за најбоље резултате
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Кључни увид:** Chat модели користе OpenAI-компатибилни API преко `manager.urls[0] + "/v1"`. Whisper транскрипцију користи SDK-ов `AudioClient`, добијен из `model.createAudioClient()`. Поставите `settings.language` да бисте избегли нечитљив излаз услед аутоматске детекције.

</details>

<details>
<summary><b>C# - Кључне разлике у односу на ћаскање</b></summary>

C# приступ користи SDK-ов уграђени `AudioClient` за транскрипцију у процесу:

**Иницијализација модела:**

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

**Транскрипција:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Кључни увид:** C# користи `FoundryLocalManager.CreateAsync()` и директно добија `AudioClient` — није потребна конфигурација ONNX Runtime-а. Поставите `Settings.Language` да бисте избегли нечитљив излаз услед аутоматске детекције.

</details>

> **Сажетак:** Python користи Foundry Local SDK за управљање моделима и ONNX Runtime за директно извршавање над encoder/decoder моделима. JavaScript и C# оба користе SDK-ов уграђени `AudioClient` за поједностављену транскрипцију — креирајте клијента, поставите језик и позовите `transcribe()` / `TranscribeAudioAsync()`. Увек поставите својство језика на AudioClient-у ради прецизних резултата.

---

### Вежба 6 - Експериментисање

Испробајте ове измене да боље разумете:

1. **Пробајте друге аудио датотеке** - снимите себе како говорите помоћу Windows Voice Recorder-а, сачувајте као WAV и транскрибујте

2. **Упоредите варијанте модела** - ако имате NVIDIA GPU, пробајте CUDA варијанту:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Упоредите брзину транскрипције у односу на CPU варијанту.

3. **Додајте форматирање излаза** - JSON одговор може укључивати:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Направите REST API** - упакујте код за транскрипцију у веб сервер:

   | Језик | Фрејмворк | Пример |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` са `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` са `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` са `IFormFile` |

5. **Мулти-турн са транскрипцијом** - комбинујте Whisper са chat агентом из дела 4: прво транскрибујте аудио, затим проследите текст агенту за анализу или резимирање.

---

## SDK Audio API Референца

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — креира `AudioClient` инстанцу
> - `audioClient.settings.language` — поставља језик транскрипције (нпр. `"en"`)
> - `audioClient.settings.temperature` — контролише случајност (опционо)
> - `audioClient.transcribe(filePath)` — транскрибује датотеку, враћа `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — стримује делове транскрипције преко повратне функције
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — креира `OpenAIAudioClient` инстанцу
> - `audioClient.Settings.Language` — поставља језик транскрипције (нпр. `"en"`)
> - `audioClient.Settings.Temperature` — контролише случајност (опционо)
> - `await audioClient.TranscribeAudioAsync(filePath)` — транскрибује датотеку, враћа објекат са `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — враћа `IAsyncEnumerable` делова транскрипције

> **Савет:** Увек поставите језик пре транскрипције. Без тога, Whisper покушава аутоматско откривање, што може произвести нечитљив излаз (један заменски знак уместо текста).

---

## Поређење: Chat модели vs. Whisper

| Аспект | Chat модели (делови 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Тип задатка** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Улаз** | Текстуалне поруке (JSON) | Аудио датотеке (WAV/MP3/M4A) | Аудио датотеке (WAV/MP3/M4A) |
| **Излаз** | Генерисани текст (стримовање) | Транскрибован текст (комплетан) | Транскрибован текст (комплетан) |
| **SDK пакет** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API метода** | `client.chat.completions.create()` | ONNX Runtime директно | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Подешавање језика** | Н/Д | Токени приступног упита decoder-а | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Стриминг** | Да | Не | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Предност приватности** | Код/подаци остају локално | Аудио подаци остају локално | Аудио подаци остају локално |

---

## Кључне поуке

| Концепт | Шта сте научили |
|---------|-----------------|
| **Whisper на уређају** | Претварање говора у текст се извршава потпуно локално, идеално за транскрипцију Зава корисничких позива и рецензија производа на уређају |
| **SDK AudioClient** | JavaScript и C# SDK пружају уграђени `AudioClient` који обрађује цео ток транскрипције у једном позиву |
| **Постављање језика** | Увек поставите језик AudioClient-а (нпр. `"en"`) — без тога, аутоматска детекција може производити нечитљив излаз |
| **Python** | Користи `foundry-local-sdk` за управљање моделима + `onnxruntime` + `transformers` + `librosa` за директно ONNX извршавање |
| **JavaScript** | Користи `foundry-local-sdk` са `model.createAudioClient()` — поставите `settings.language`, затим позовите `transcribe()` |
| **C#** | Користи `Microsoft.AI.Foundry.Local` са `model.GetAudioClientAsync()` — поставите `Settings.Language`, затим позовите `TranscribeAudioAsync()` |
| **Подршка стримингу** | JS и C# SDK такође нуде `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` за излаз делова транскрипције |
| **Оптимизовано за CPU** | CPU варијанта (3.05 GB) ради на било ком Windows уређају без GPU-а |
| **Приватност на првом месту** | Савршено за чување Зава корисничких интеракција и поверљивих података о производима на уређају |

---

## Ресурси

| Ресурс | Линк |
|----------|------|
| Foundry Local документација | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Референца | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper модел | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local вебсајт | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Следећи корак

Наставите са [Део 10: Коришћење прилагођених или Hugging Face модела](part10-custom-models.md) да компајлирате своје моделе са Hugging Face и покренете их преко Foundry Local.