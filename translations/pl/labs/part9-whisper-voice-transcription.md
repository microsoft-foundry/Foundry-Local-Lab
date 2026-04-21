![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Część 9: Transkrypcja głosu przy użyciu Whisper i Foundry Local

> **Cel:** Użyj modelu OpenAI Whisper działającego lokalnie przez Foundry Local, aby transkrybować pliki audio - całkowicie na urządzeniu, bez konieczności korzystania z chmury.

## Przegląd

Foundry Local to nie tylko generowanie tekstu; obsługuje również modele **speech-to-text**. W tym laboratorium użyjesz modelu **OpenAI Whisper Medium**, aby transkrybować pliki audio całkowicie na swoim komputerze. To idealne rozwiązanie w scenariuszach takich jak transkrypcja rozmów obsługi klienta Zava, nagrań recenzji produktów lub sesji planowania warsztatów, gdzie dane audio nigdy nie mogą opuścić twojego urządzenia.


---

## Cele nauki

Na koniec tego laboratorium będziesz potrafił:

- Zrozumieć model mowy na tekst Whisper i jego możliwości
- Pobierać i uruchamiać model Whisper używając Foundry Local
- Transkrybować pliki audio korzystając z Foundry Local SDK w Python, JavaScript i C#
- Zbudować prostą usługę transkrypcji działającą całkowicie na urządzeniu
- Zrozumieć różnice między modelami czatu/tekstu a modelami audio w Foundry Local

---

## Wymagania wstępne

| Wymaganie | Szczegóły |
|-----------|-----------|
| **Foundry Local CLI** | Wersja **0.8.101 lub wyższa** (modele Whisper dostępne od v0.8.101 wzwyż) |
| **System operacyjny** | Windows 10/11 (x64 lub ARM64) |
| **Środowisko uruchomieniowe** | **Python 3.9+** i/lub **Node.js 18+** i/lub **.NET 9 SDK** ([Pobierz .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Ukończone** | [Część 1: Pierwsze kroki](part1-getting-started.md), [Część 2: Szczegóły Foundry Local SDK](part2-foundry-local-sdk.md) oraz [Część 3: SDK i API](part3-sdk-and-apis.md) |

> **Uwaga:** Modele Whisper muszą być pobierane przez **SDK** (nie CLI). CLI nie obsługuje punktu końcowego transkrypcji audio. Sprawdź swoją wersję poleceniem:
> ```bash
> foundry --version
> ```

---

## Koncepcja: Jak działa Whisper z Foundry Local

Model OpenAI Whisper to model rozpoznawania mowy ogólnego przeznaczenia, wytrenowany na dużym zbiorze różnorodnych danych audio. Działając przez Foundry Local:

- Model działa **całkowicie na twoim procesorze (CPU)** – nie wymaga GPU
- Dźwięk nigdy nie opuszcza twojego urządzenia – **pełna prywatność**
- Foundry Local SDK zajmuje się pobieraniem modelu i zarządzaniem cachem
- **JavaScript i C#** oferują wbudowany `AudioClient` w SDK, który obsługuje cały proces transkrypcji — nie wymaga ręcznej konfiguracji ONNX
- **Python** używa SDK do zarządzania modelem oraz ONNX Runtime do bezpośredniego wnioskowania na modelach encoder/decoder w formacie ONNX

### Jak działa pipeline (JavaScript i C#) — SDK AudioClient

1. **Foundry Local SDK** pobiera i buforuje model Whisper
2. `model.createAudioClient()` (JS) lub `model.GetAudioClientAsync()` (C#) tworzy `AudioClient`
3. `audioClient.transcribe(path)` (JS) lub `audioClient.TranscribeAudioAsync(path)` (C#) obsługuje cały pipeline wewnętrznie — wstępne przetwarzanie audio, encoder, decoder oraz dekodowanie tokenów
4. `AudioClient` posiada właściwość `settings.language` (ustawioną na `"en"` dla angielskiego), która umożliwia poprawną transkrypcję

### Jak działa pipeline (Python) — ONNX Runtime

1. **Foundry Local SDK** pobiera i buforuje pliki modelu Whisper w formacie ONNX
2. **Wstępne przetwarzanie audio** konwertuje plik WAV na mel spektrogram (80 pasków melowych x 3000 ramek)
3. **Encoder** przetwarza mel spektrogram i generuje stany ukryte oraz tensory klucza/wartości w cross-attention
4. **Decoder** działa autoregresywnie, generując pojedyncze tokeny aż do wygenerowania tokena końca tekstu
5. **Tokenizer** dekoduje wyjściowe ID tokenów z powrotem na czytelny tekst

### Warianty modelu Whisper

| Alias | ID modelu | Urządzenie | Rozmiar | Opis |
|-------|-----------|------------|---------|------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Przyspieszenie na GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Optymalizacja pod CPU (zalecane dla większości urządzeń) |

> **Uwaga:** W przeciwieństwie do modeli czatu które są domyślnie wylistowane, modele Whisper są sklasyfikowane pod zadaniem `automatic-speech-recognition`. Użyj `foundry model info whisper-medium`, aby zobaczyć szczegóły.

---

## Ćwiczenia w laboratorium

### Ćwiczenie 0 - Pobierz przykładowe pliki audio

To laboratorium zawiera gotowe pliki WAV oparte na scenariuszach produktów Zava DIY. Wygeneruj je za pomocą dołączonego skryptu:

```bash
# Z korzenia repozytorium - najpierw utwórz i aktywuj .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Tworzy sześć plików WAV w `samples/audio/`:

| Plik | Scenariusz |
|------|------------|
| `zava-customer-inquiry.wav` | Klient pyta o **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Klient recenzuje **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | Rozmowa wsparcia o **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | Majster planuje taras z **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Przegląd warsztatu używając **wszystkich pięciu produktów Zava** |
| `zava-full-project-walkthrough.wav` | Rozszerzona prezentacja remontu garażu używając **wszystkich produktów Zava** (~4 min, do testów długiego audio) |

> **Wskazówka:** Możesz także użyć własnych plików WAV/MP3/M4A lub nagrać się samodzielnie za pomocą Windows Voice Recorder.

---

### Ćwiczenie 1 - Pobierz model Whisper używając SDK

Ze względu na niekompatybilność CLI z modelami Whisper w nowszych wersjach Foundry Local, użyj **SDK** do pobrania i załadowania modelu. Wybierz swój język:

<details>
<summary><b>🐍 Python</b></summary>

**Zainstaluj SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Uruchom usługę
manager = FoundryLocalManager()
manager.start_service()

# Sprawdź informacje katalogowe
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Sprawdź, czy już jest w pamięci podręcznej
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Załaduj model do pamięci
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Zapisz jako `download_whisper.py` i uruchom:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Zainstaluj SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Utwórz menedżera i uruchom usługę
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Pobierz model z katalogu
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

// Załaduj model do pamięci
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Zapisz jako `download-whisper.mjs` i uruchom:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Zainstaluj SDK:**
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

> **Dlaczego SDK zamiast CLI?** Foundry Local CLI nie obsługuje pobierania ani serwowania modeli Whisper bezpośrednio. SDK zapewnia niezawodny sposób programowego pobierania i zarządzania modelami audio. SDK dla JavaScript i C# zawiera wbudowany `AudioClient`, który obsługuje cały pipeline transkrypcji. Python używa ONNX Runtime do bezpośredniego wnioskowania na pobranych plikach modelu.

---

### Ćwiczenie 2 - Zrozumienie SDK Whisper

Transkrypcja Whisper używa różnych podejść w zależności od języka. **JavaScript i C#** mają wbudowany `AudioClient` w Foundry Local SDK, który obsługuje cały pipeline (wstępne przetwarzanie audio, encoder, decoder, dekodowanie tokenów) w pojedynczym wywołaniu metody. **Python** używa Foundry Local SDK do zarządzania modelem oraz ONNX Runtime do bezpośredniego wnioskowania na modelach encoder/decoder w formacie ONNX.

| Komponent | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **Pakiety SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Zarządzanie modelem** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Ekstrakcja cech** | `WhisperFeatureExtractor` + `librosa` | Obsługiwane przez SDK `AudioClient` | Obsługiwane przez SDK `AudioClient` |
| **Wnioskowanie** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Dekodowanie tokenów** | `WhisperTokenizer` | Obsługiwane przez SDK `AudioClient` | Obsługiwane przez SDK `AudioClient` |
| **Ustawienie języka** | Poprzez `forced_ids` w tokenach dekodera | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Wejście** | Ścieżka pliku WAV | Ścieżka pliku WAV | Ścieżka pliku WAV |
| **Wyjście** | Dekodowany tekst | `result.text` | `result.Text` |

> **Ważne:** Zawsze ustawiaj język na `AudioClient` (np. `"en"` dla angielskiego). Bez wyraźnego ustawienia języka model może generować zniekształcony tekst, próbując automatycznie wykryć język.

> **Wzorce SDK:** Python używa `FoundryLocalManager(alias)` do uruchomienia, potem `get_cache_location()`, żeby odnaleźć pliki ONNX modelu. JavaScript i C# korzystają z wbudowanego `AudioClient` — uzyskanego przez `model.createAudioClient()` (JS) lub `model.GetAudioClientAsync()` (C#) — który obsługuje cały pipeline transkrypcji. Zobacz [Część 2: Szczegóły Foundry Local SDK](part2-foundry-local-sdk.md) dla pełnych informacji.

---

### Ćwiczenie 3 - Zbuduj prostą aplikację do transkrypcji

Wybierz swój język i stwórz prostą aplikację, która transkrybuje plik audio.

> **Obsługiwane formaty audio:** WAV, MP3, M4A. Dla najlepszych rezultatów stosuj pliki WAV z próbkowaniem 16 kHz.

<details>
<summary><h3>Ścieżka Python</h3></summary>

#### Konfiguracja

```bash
cd python
python -m venv venv

# Aktywuj środowisko wirtualne:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Kod transkrypcji

Utwórz plik `foundry-local-whisper.py`:

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

# Krok 1: Bootstrap - uruchamia usługę, pobiera i ładuje model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Buduj ścieżkę do pamięci podręcznej plików modelu ONNX
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Krok 2: Załaduj sesje ONNX i ekstraktor cech
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

# Krok 3: Ekstrahuj cechy spektrogramu mel
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Krok 4: Uruchom enkoder
enc_out = encoder.run(None, {"audio_features": input_features})
# Pierwszy wynik to ukryte stany; pozostałe to pary KV atencji krzyżowej
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Krok 5: Dekodowanie autoregresywne
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transkrypcja, bez znaczników czasu
input_ids = np.array([initial_tokens], dtype=np.int32)

# Pusta pamięć podręczna par KV atencji własnej
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

    if next_token == 50257:  # koniec tekstu
        break
    generated.append(next_token)

    # Aktualizuj pamięć podręczną par KV atencji własnej
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Uruchomienie

```bash
# Transkrybuj scenariusz produktu Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Lub wypróbuj inne:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Kluczowe punkty Pythona

| Metoda | Cel |
|--------|-----|
| `FoundryLocalManager(alias)` | Bootstrap: uruchomienie serwisu, pobranie i załadowanie modelu |
| `manager.get_cache_location()` | Uzyskanie ścieżki do zapisanych plików modelu ONNX |
| `WhisperFeatureExtractor.from_pretrained()` | Załadowanie ekstraktora cech mel spektrogramu |
| `ort.InferenceSession()` | Utworzenie sesji ONNX Runtime dla encoder i decoder |
| `tokenizer.decode()` | Konwersja wyjściowych ID tokenów na tekst |

</details>

<details>
<summary><h3>Ścieżka JavaScript</h3></summary>

#### Konfiguracja

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Kod transkrypcji

Utwórz plik `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Krok 1: Bootstrap - utwórz managera, uruchom usługę i załaduj model
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

// Krok 2: Utwórz klienta audio i transkrybuj
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Sprzątanie
await model.unload();
```

> **Uwaga:** Foundry Local SDK zapewnia wbudowany `AudioClient` przez `model.createAudioClient()`, który obsługuje cały pipeline wnioskowania ONNX wewnętrznie — nie trzeba importować `onnxruntime-node`. Zawsze ustawiaj `audioClient.settings.language = "en"`, by zapewnić dokładną transkrypcję angielskiego.

#### Uruchomienie

```bash
# Transkrybuj scenariusz produktu Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Lub wypróbuj inne:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Kluczowe punkty JavaScriptu

| Metoda | Cel |
|--------|-----|
| `FoundryLocalManager.create({ appName })` | Utworzenie singletonu managera |
| `await catalog.getModel(alias)` | Pobranie modelu z katalogu |
| `model.download()` / `model.load()` | Pobranie i załadowanie modelu Whisper |
| `model.createAudioClient()` | Utworzenie klienta audio do transkrypcji |
| `audioClient.settings.language = "en"` | Ustawienie języka transkrypcji (wymagane dla dokładności) |
| `audioClient.transcribe(path)` | Transkrypcja pliku audio, zwraca `{ text, duration }` |

</details>

<details>
<summary><h3>Ścieżka C#</h3></summary>

#### Konfiguracja

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Uwaga:** Ścieżka C# używa pakietu `Microsoft.AI.Foundry.Local`, który posiada wbudowany `AudioClient` przez `model.GetAudioClientAsync()`. Obsługuje cały pipeline transkrypcji w procesie — nie jest potrzebne osobne ustawianie ONNX Runtime.

#### Kod transkrypcji

Zamień zawartość `Program.cs`:

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

#### Uruchomienie

```bash
# Transkrybuj scenariusz produktu Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Albo wypróbuj inne:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Kluczowe punkty C#

| Metoda | Cel |
|--------|-----|
| `FoundryLocalManager.CreateAsync(config)` | Inicjalizacja Foundry Local z konfiguracją |
| `catalog.GetModelAsync(alias)` | Pobranie modelu z katalogu |
| `model.DownloadAsync()` | Pobranie modelu Whisper |
| `model.GetAudioClientAsync()` | Pobranie AudioClient (nie ChatClient!) |
| `audioClient.Settings.Language = "en"` | Ustawienie języka transkrypcji (wymagane do dokładnego wyniku) |
| `audioClient.TranscribeAudioAsync(path)` | Transkrypcja pliku audio |
| `result.Text` | Transkrybowany tekst |
> **C# vs Python/JS:** SDK C# zapewnia wbudowany `AudioClient` do transkrypcji w procesie za pomocą `model.GetAudioClientAsync()`, podobnie jak SDK JavaScript. Python korzysta bezpośrednio z ONNX Runtime do inferencji na lokowanych modelach enkodera/dekodera.

</details>

---

### Ćwiczenie 4 - Transkrypcja wsadowa wszystkich próbek Zava

Teraz, gdy masz działającą aplikację do transkrypcji, przetranskrybuj wszystkie pięć plików próbek Zava i porównaj wyniki.

<details>
<summary><h3>Ścieżka Python</h3></summary>

Pełny przykład `python/foundry-local-whisper.py` już obsługuje transkrypcję wsadową. Po uruchomieniu bez argumentów transkrybuje wszystkie pliki `zava-*.wav` w `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Przykład używa `FoundryLocalManager(alias)` do uruchomienia, a następnie wykonuje sesje ONNX enkodera i dekodera dla każdego pliku.

</details>

<details>
<summary><h3>Ścieżka JavaScript</h3></summary>

Pełny przykład `javascript/foundry-local-whisper.mjs` już obsługuje transkrypcję wsadową. Po uruchomieniu bez argumentów transkrybuje wszystkie pliki `zava-*.wav` w `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Przykład używa `FoundryLocalManager.create()` i `catalog.getModel(alias)` do inicjalizacji SDK, następnie korzysta z `AudioClient` (z `settings.language = "en"`) do transkrypcji każdego pliku.

</details>

<details>
<summary><h3>Ścieżka C#</h3></summary>

Pełny przykład `csharp/WhisperTranscription.cs` już obsługuje transkrypcję wsadową. Po uruchomieniu bez konkretnego argumentu pliku transkrybuje wszystkie pliki `zava-*.wav` w `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Przykład używa `FoundryLocalManager.CreateAsync()` i wbudowanego w SDK `AudioClient` (z `Settings.Language = "en"`) do transkrypcji w procesie.

</details>

**Na co zwrócić uwagę:** Porównaj wynik transkrypcji z oryginalnym tekstem w `samples/audio/generate_samples.py`. Jak dokładnie Whisper rozpoznaje nazwy produktów, jak „Zava ProGrip” i terminy techniczne takie jak „brushless motor” czy „composite decking”?

---

### Ćwiczenie 5 - Zrozumienie kluczowych wzorców kodu

Przestudiuj, jak transkrypcja Whisper różni się od uzupełnień w chatcie we wszystkich trzech językach:

<details>
<summary><b>Python - Kluczowe różnice względem chatu</b></summary>

```python
# Uzupełnianie czatu (Części 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transkrypcja audio (Ta część):
# Używa ONNX Runtime bezpośrednio zamiast klienta OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... pętla autokorelacyjnego dekodera ...
print(tokenizer.decode(generated_tokens))
```

**Kluczowy wniosek:** Modele chatowe korzystają z API kompatybilnego z OpenAI przez `manager.endpoint`. Whisper używa SDK, aby odnaleźć lokalne modele ONNX, a następnie wykonuje inferencję bezpośrednio za pomocą ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Kluczowe różnice względem chatu</b></summary>

```javascript
// Ukończenie czatu (części 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transkrypcja audio (ta część):
// Używa wbudowanego AudioClient SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Zawsze ustawiaj język dla najlepszych rezultatów
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Kluczowy wniosek:** Modele chatowe korzystają z API kompatybilnego z OpenAI przez `manager.urls[0] + "/v1"`. Transkrypcja Whisper używa `AudioClient` SDK uzyskanego z `model.createAudioClient()`. Ustaw `settings.language`, aby uniknąć zniekształconego wyniku z automatycznego wykrywania.

</details>

<details>
<summary><b>C# - Kluczowe różnice względem chatu</b></summary>

Podejście C# korzysta z wbudowanego w SDK `AudioClient` do transkrypcji w procesie:

**Inicjalizacja modelu:**

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

**Transkrypcja:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Kluczowy wniosek:** C# używa `FoundryLocalManager.CreateAsync()` i pobiera `AudioClient` bezpośrednio — bez potrzeby konfiguracji ONNX Runtime. Ustaw `Settings.Language`, aby uniknąć zniekształconego wyniku z automatycznego wykrywania.

</details>

> **Podsumowanie:** Python używa Foundry Local SDK do zarządzania modelami i ONNX Runtime do bezpośredniej inferencji na modelach enkodera/dekodera. JavaScript i C# używają wbudowanego `AudioClient` w SDK dla uproszczonej transkrypcji — utwórz klienta, ustaw język i wywołaj `transcribe()` / `TranscribeAudioAsync()`. Zawsze ustawiaj właściwość języka w AudioClient, aby uzyskać dokładne wyniki.

---

### Ćwiczenie 6 - Eksperymentuj

Wypróbuj te modyfikacje, aby pogłębić zrozumienie:

1. **Wypróbuj różne pliki audio** - nagraj siebie mówiącego za pomocą Windows Voice Recorder, zapisz jako WAV i przetranskrybuj

2. **Porównaj warianty modelu** - jeśli masz GPU NVIDIA, wypróbuj wariant CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Porównaj prędkość transkrypcji w stosunku do wariantu CPU.

3. **Dodaj formatowanie wyjścia** - odpowiedź JSON może zawierać:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Zbuduj REST API** - opakuj swój kod transkrypcji w serwer WWW:

   | Język | Framework | Przykład |
   |-------|-----------|----------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` z `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` z `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` z `IFormFile` |

5. **Wieloturnusowość z transkrypcją** - połącz Whisper z agentem chat z Części 4: najpierw przetranskrybuj audio, potem przekaż tekst agentowi do analizy lub podsumowania.

---

## SDK Audio API Reference

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — tworzy instancję `AudioClient`
> - `audioClient.settings.language` — ustawia język transkrypcji (np. `"en"`)
> - `audioClient.settings.temperature` — kontrola losowości (opcjonalnie)
> - `audioClient.transcribe(filePath)` — transkrybuje plik, zwraca `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — strumieniowa transkrypcja fragmentów przez callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — tworzy instancję `OpenAIAudioClient`
> - `audioClient.Settings.Language` — ustawia język transkrypcji (np. `"en"`)
> - `audioClient.Settings.Temperature` — kontrola losowości (opcjonalnie)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transkrybuje plik, zwraca obiekt z `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — zwraca `IAsyncEnumerable` fragmentów transkrypcji

> **Wskazówka:** Zawsze ustawiaj właściwość języka przed transkrypcją. Bez tego model Whisper próbuje wykrycia automatycznego, co może dać zniekształcony wynik (pojedynczy znak zastępczy zamiast tekstu).

---

## Porównanie: Modele Chat vs Whisper

| Aspekt | Modele Chat (Części 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|--------------------------|------------------|-------------------|
| **Typ zadania** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Wejście** | Wiadomości tekstowe (JSON) | Pliki audio (WAV/MP3/M4A) | Pliki audio (WAV/MP3/M4A) |
| **Wyjście** | Generowany tekst (strumieniowo) | Przetranskrybowany tekst (kompletny) | Przetranskrybowany tekst (kompletny) |
| **Pakiet SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Metoda API** | `client.chat.completions.create()` | Bezpośrednio ONNX Runtime | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Ustawienie języka** | N/D | Tokeny podpowiedzi dekodera | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Strumieniowanie** | Tak | Nie | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Korzyść prywatności** | Kod/dane pozostają lokalnie | Dane audio pozostają lokalnie | Dane audio pozostają lokalnie |

---

## Kluczowe wnioski

| Koncepcja | Czego się nauczyłeś |
|-----------|--------------------|
| **Whisper na urządzeniu** | Przekształcanie mowy na tekst działa całkowicie lokalnie, idealne do transkrypcji rozmów klientów Zava i recenzji produktów na urządzeniu |
| **SDK AudioClient** | SDK JavaScript i C# dostarczają wbudowany `AudioClient`, który obsługuje cały proces transkrypcji w jednym wywołaniu |
| **Ustawienie języka** | Zawsze ustawiaj język AudioClient (np. `"en"`) — bez tego automatyczne wykrywanie może dać zniekształcony wynik |
| **Python** | Używa `foundry-local-sdk` do zarządzania modelem + `onnxruntime` + `transformers` + `librosa` do bezpośredniej inferencji ONNX |
| **JavaScript** | Używa `foundry-local-sdk` z `model.createAudioClient()` — ustaw `settings.language`, potem wywołaj `transcribe()` |
| **C#** | Używa `Microsoft.AI.Foundry.Local` z `model.GetAudioClientAsync()` — ustaw `Settings.Language`, potem wywołaj `TranscribeAudioAsync()` |
| **Wsparcie strumieniowania** | SDK JS i C# oferują też `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` do wyniku fragmentowego |
| **Optymalizacja pod CPU** | Wersja CPU (3,05 GB) działa na dowolnym urządzeniu Windows bez GPU |
| **Prywatność przede wszystkim** | Idealne do utrzymania interakcji klientów Zava i danych produktowych na urządzeniu |

---

## Zasoby

| Zasób | Link |
|-------|------|
| Dokumentacja Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referencja SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Model OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Strona Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Następny krok

Kontynuuj do [Części 10: Używanie modeli niestandardowych lub Hugging Face](part10-custom-models.md), aby kompilować własne modele z Hugging Face i uruchamiać je przez Foundry Local.