![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Část 9: Přepis hlasu pomocí Whisper a Foundry Local

> **Cíl:** Použít model OpenAI Whisper běžící lokálně přes Foundry Local k přepisu zvukových souborů – zcela na zařízení, bez potřeby cloudu.

## Přehled

Foundry Local není určen pouze pro generování textu; podporuje také modely **řeč–text**. V tomto cvičení použijete model **OpenAI Whisper Medium** k přepisu zvukových souborů zcela na vašem zařízení. To je ideální pro scénáře jako přepis zákaznických hovorů Zava, nahrávek recenzí produktů nebo plánování workshopů, kde nesmí zvuková data nikdy opustit vaše zařízení.


---

## Výukové cíle

Na konci tohoto cvičení budete schopni:

- Porozumět modelu Whisper pro řeč na text a jeho schopnostem
- Stáhnout a spustit model Whisper pomocí Foundry Local
- Přepisovat zvukové soubory pomocí Foundry Local SDK v Pythonu, JavaScriptu a C#
- Vytvořit jednoduchou službu pro přepis, která běží zcela na zařízení
- Porozumět rozdílům mezi chat/textovými modely a audio modely ve Foundry Local

---

## Požadavky

| Požadavek | Podrobnosti |
|-----------|-------------|
| **Foundry Local CLI** | Verze **0.8.101 a novější** (Whisper modely jsou dostupné od verze v0.8.101 výš) |
| **OS** | Windows 10/11 (x64 nebo ARM64) |
| **Jazykové prostředí** | **Python 3.9+** a/nebo **Node.js 18+** a/nebo **.NET 9 SDK** ([Stáhnout .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Dokončeno** | [Část 1: Začínáme](part1-getting-started.md), [Část 2: Hluboký ponor do Foundry Local SDK](part2-foundry-local-sdk.md), a [Část 3: SDK a API](part3-sdk-and-apis.md) |

> **Poznámka:** Whisper modely je nutné stahovat přes **SDK** (ne přes CLI). CLI nepodporuje endpoint pro přepis audia. Zkontrolujte verzi příkazem:
> ```bash
> foundry --version
> ```

---

## Koncept: Jak Whisper funguje s Foundry Local

Model OpenAI Whisper je obecný model pro rozpoznávání řeči trénovaný na velkém a rozmanitém datovém souboru zvuků. Při běhu přes Foundry Local:

- Model běží **zcela na vašem CPU** – není nutná GPU
- Zvuk nikdy neopouští vaše zařízení – **plné soukromí**
- Foundry Local SDK se stará o stažení modelu a správu cache
- **JavaScript a C#** mají v SDK zabudovaný `AudioClient`, který řeší celý přepisový proces — není potřeba manuální nastavení ONNX
- **Python** používá SDK pro správu modelu a ONNX Runtime pro přímý běh inferencí na encoder/decoder ONNX modelech

### Jak funguje pipeline (JavaScript a C#) — SDK AudioClient

1. **Foundry Local SDK** stáhne a uloží model Whisper do cache
2. `model.createAudioClient()` (JS) nebo `model.GetAudioClientAsync()` (C#) vytvoří `AudioClient`
3. `audioClient.transcribe(path)` (JS) nebo `audioClient.TranscribeAudioAsync(path)` (C#) interně vyřídí celý proces — předzpracování audia, encoder, decoder a dekódování tokenů
4. `AudioClient` nabízí vlastnost `settings.language` (nastavit na `"en"` pro angličtinu) pro přesnější přepis

### Jak funguje pipeline (Python) — ONNX Runtime

1. **Foundry Local SDK** stáhne a uloží modelové soubory Whisper ONNX do cache
2. **Předzpracování audia** převádí WAV audio do mel spektrogramu (80 mel pásem x 3000 snímků)
3. **Encoder** zpracuje mel spektrogram a vytvoří skryté stavy a tenzory klíče/hodnoty pro cross-attention
4. **Decoder** běží autoregresivně, generuje token po tokenu, dokud nevytvoří token konce textu
5. **Tokenizér** dekóduje ID tokenů zpět do čitelného textu

### Varianty modelu Whisper

| Alias | ID modelu | Zařízení | Velikost | Popis |
|-------|-----------|----------|----------|-------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-akcelerovaný (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Optimalizováno pro CPU (doporučeno pro většinu zařízení) |

> **Poznámka:** Na rozdíl od chat modelů, které jsou ve výchozím seznamu, jsou Whisper modely zařazeny pod úlohu `automatic-speech-recognition`. Pro zobrazení podrobností použijte `foundry model info whisper-medium`.

---

## Cvičení v laboratoři

### Cvičení 0 - Získejte ukázkové zvukové soubory

Tento lab obsahuje předpřipravené WAV soubory založené na scénářích Zava DIY produktů. Vygenerujte je pomocí přiloženého skriptu:

```bash
# Ze kořenového adresáře repo - nejprve vytvořte a aktivujte .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Tím vytvoříte šest WAV souborů v adresáři `samples/audio/`:

| Soubor | Scénář |
|--------|---------|
| `zava-customer-inquiry.wav` | Zákazník se ptá na **Zava ProGrip bezdrátovou vrtačku** |
| `zava-product-review.wav` | Zákaznická recenze na **Zava UltraSmooth interiérovou barvu** |
| `zava-support-call.wav` | Podpora ohledně **Zava TitanLock nářaďové skříně** |
| `zava-project-planning.wav` | Domácí kutil plánující terasu s **Zava EcoBoard kompozitním terasovým materiálem** |
| `zava-workshop-setup.wav` | Prohlídka workshopu používajícího **všechny pět produktů Zava** |
| `zava-full-project-walkthrough.wav` | Rozšířená prohlídka renovace garáže se **všemi produkty Zava** (~4 min, pro testování dlouhého audia) |

> **Tip:** Můžete použít také vlastní WAV/MP3/M4A soubory nebo se nahrát Windows Voice Recorderem.

---

### Cvičení 1 - Stáhněte model Whisper přes SDK

Kvůli nekompatibilitě CLI s Whisper modely v novějších verzích Foundry Local použijte **SDK** ke stažení a načtení modelu. Vyberte jazykové prostředí:

<details>
<summary><b>🐍 Python</b></summary>

**Nainstalujte SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Spusťte službu
manager = FoundryLocalManager()
manager.start_service()

# Zkontrolujte informace o katalogu
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Zkontrolujte, zda už je v mezipaměti
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Načtěte model do paměti
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Uložte jako `download_whisper.py` a spusťte:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Nainstalujte SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Vytvořit správce a spustit službu
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Získat model z katalogu
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

// Načíst model do paměti
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Uložte jako `download-whisper.mjs` a spusťte:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Nainstalujte SDK:**
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

> **Proč SDK místo CLI?** Foundry Local CLI nepodporuje přímé stahování ani spouštění Whisper modelů. SDK nabízí spolehlivý způsob, jak spravovat audio modely programově. JavaScript a C# SDK obsahují zabudovaný `AudioClient`, který zvládá celý přepisový proces. Python používá ONNX Runtime pro přímé volání inference nad uloženými modelovými soubory.

---

### Cvičení 2 - Porozumění Whisper SDK

Whisper přepis používá různé přístupy podle jazyka. **JavaScript a C#** mají v Foundry Local SDK zabudovaný `AudioClient`, který řeší celý proces (předzpracování audia, encoder, decoder, dekódování tokenů) v jediném volání. **Python** používá Foundry Local SDK pro správu modelu a ONNX Runtime pro přímou inferenci na encoder/decoder ONNX modelech.

| Komponenta | Python | JavaScript | C# |
|------------|--------|------------|----|
| **SDK balíčky** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Správa modelu** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Extrahování funkcí** | `WhisperFeatureExtractor` + `librosa` | Řeší SDK `AudioClient` | Řeší SDK `AudioClient` |
| **Inference** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Dekódování tokenů** | `WhisperTokenizer` | Řeší SDK `AudioClient` | Řeší SDK `AudioClient` |
| **Nastavení jazyka** | Nastaví se přes `forced_ids` v dekódovaných tokenech | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Vstup** | Cesta k WAV souboru | Cesta k WAV souboru | Cesta k WAV souboru |
| **Výstup** | Dekódovaný textový řetězec | `result.text` | `result.Text` |

> **Důležité:** Vždy nastavte jazyk na `AudioClient` (např. `"en"` pro angličtinu). Bez explicitního nastavení jazyka může model vytvářet nesrozumitelný výstup, protože se snaží jazyk automaticky detekovat.

> **Vzory SDK:** Python používá `FoundryLocalManager(alias)` pro inicializaci, pak `get_cache_location()` pro nalezení ONNX souborů. JavaScript a C# využívají zabudovaný `AudioClient` SDK – získaný přes `model.createAudioClient()` (JS) nebo `model.GetAudioClientAsync()` (C#) –, který vyřizuje celý přepis. Viz [Část 2: Hluboký ponor do Foundry Local SDK](part2-foundry-local-sdk.md) pro detailní informace.

---

### Cvičení 3 - Vytvořte jednoduchou aplikaci pro přepis

Vyberte svůj jazykový směr a vytvořte minimální aplikaci, která přepíše zvukový soubor.

> **Podporované audio formáty:** WAV, MP3, M4A. Pro nejlepší výsledky používejte WAV soubory s vzorkovací frekvencí 16 kHz.

<details>
<summary><h3>Python</h3></summary>

#### Nastavení

```bash
cd python
python -m venv venv

# Aktivujte virtuální prostředí:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Přepisovací kód

Vytvořte soubor `foundry-local-whisper.py`:

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

# Krok 1: Bootstrap - spustí službu, stáhne a načte model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Vytvořit cestu k uloženým ONNX modelovým souborům
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Krok 2: Načíst ONNX relace a extraktor funkcí
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

# Krok 3: Extrahovat mel spektrogramové rysy
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Krok 4: Spustit enkodér
enc_out = encoder.run(None, {"audio_features": input_features})
# První výstup jsou skryté stavy; zbývající jsou klíčové hodnoty křížové pozornosti
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Krok 5: Autoregresivní dekódování
initial_tokens = [50258, 50259, 50359, 50363]  # sot, ang, přepis, bez časových značek
input_ids = np.array([initial_tokens], dtype=np.int32)

# Prázdná mezipaměť klíčových hodnot vlastní pozornosti
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

    if next_token == 50257:  # konec textu
        break
    generated.append(next_token)

    # Aktualizovat mezipaměť klíčových hodnot vlastní pozornosti
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Spuštění

```bash
# Přepsat scénář produktu Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Nebo vyzkoušejte jiné:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Klíčové body v Pythonu

| Metoda | Účel |
|--------|-------|
| `FoundryLocalManager(alias)` | Inicializace: spuštění služby, stažení a načtení modelu |
| `manager.get_cache_location()` | Získat cestu ke cached modelovým ONNX souborům |
| `WhisperFeatureExtractor.from_pretrained()` | Načíst extraktor mel spektrogramů |
| `ort.InferenceSession()` | Vytvořit ONNX Runtime sessions pro encoder a decoder |
| `tokenizer.decode()` | Převést výstupní tokeny zpět na text |

</details>

<details>
<summary><h3>JavaScript</h3></summary>

#### Nastavení

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Přepisovací kód

Vytvořte soubor `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Krok 1: Bootstrap - vytvořit správce, spustit službu a načíst model
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

// Krok 2: Vytvořit audio klienta a přepsat
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Úklid
await model.unload();
```

> **Poznámka:** Foundry Local SDK poskytuje zabudovaný `AudioClient` přes `model.createAudioClient()`, který interně zpracovává celý ONNX inference pipeline — není třeba importovat `onnxruntime-node`. Vždy nastavte `audioClient.settings.language = "en"` pro přesný anglický přepis.

#### Spuštění

```bash
# Přepis scénáře produktu Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Nebo vyzkoušejte jiné:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Klíčové body v JavaScriptu

| Metoda | Účel |
|--------|-------|
| `FoundryLocalManager.create({ appName })` | Vytvoří singleton správce |
| `await catalog.getModel(alias)` | Získá model z katalogu |
| `model.download()` / `model.load()` | Stáhne a načte Whisper model |
| `model.createAudioClient()` | Vytvoří audio klienta pro přepis |
| `audioClient.settings.language = "en"` | Nastaví jazyk přepisu (nutné pro přesný výstup) |
| `audioClient.transcribe(path)` | Přepíše audio soubor, vrací `{ text, duration }` |

</details>

<details>
<summary><h3>C#</h3></summary>

#### Nastavení

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Poznámka:** C# využívá balíček `Microsoft.AI.Foundry.Local`, který nabízí zabudovaný `AudioClient` přes `model.GetAudioClientAsync()`. Ten řeší celý přepisový pipeline interně – není potřeba samostatné nastavení ONNX Runtime.

#### Přepisovací kód

Nahraďte obsah `Program.cs`:

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

#### Spuštění

```bash
# Přepsat scénář produktu Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Nebo vyzkoušet jiné:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Klíčové body v C#

| Metoda | Účel |
|--------|-------|
| `FoundryLocalManager.CreateAsync(config)` | Inicializuje Foundry Local s konfigurací |
| `catalog.GetModelAsync(alias)` | Získá model z katalogu |
| `model.DownloadAsync()` | Stáhne Whisper model |
| `model.GetAudioClientAsync()` | Získá AudioClient (ne ChatClient!) |
| `audioClient.Settings.Language = "en"` | Nastaví jazyk přepisu (nutné pro přesný výstup) |
| `audioClient.TranscribeAudioAsync(path)` | Přepíše audio soubor |
| `result.Text` | Přepsaný text |


> **C# vs Python/JS:** SDK C# poskytuje vestavěný `AudioClient` pro přepis v procesu přes `model.GetAudioClientAsync()`, podobně jako SDK JavaScript. Python používá přímo ONNX Runtime pro inferenci na uložených modelech enkodéru/dekodéru.

</details>

---

### Cvičení 4 - Hromadný přepis všech ukázek Zava

Nyní, když máte funkční aplikaci pro přepis, přepište všech pět ukázkových souborů Zava a porovnejte výsledky.

<details>
<summary><h3>Pythonová větev</h3></summary>

Celý příklad `python/foundry-local-whisper.py` již podporuje hromadný přepis. Při spuštění bez argumentů přepisuje všechny soubory `zava-*.wav` ve složce `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Ukázka používá `FoundryLocalManager(alias)` pro bootstrap, pak spouští ONNX sezení enkodéru a dekodéru pro každý soubor.

</details>

<details>
<summary><h3>JavaScriptová větev</h3></summary>

Celý příklad `javascript/foundry-local-whisper.mjs` již podporuje hromadný přepis. Při spuštění bez argumentů přepisuje všechny soubory `zava-*.wav` ve složce `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Ukázka používá `FoundryLocalManager.create()` a `catalog.getModel(alias)` k inicializaci SDK, pak využívá `AudioClient` (s `settings.language = "en"`) k přepisu každého souboru.

</details>

<details>
<summary><h3>C# větev</h3></summary>

Celý příklad `csharp/WhisperTranscription.cs` již podporuje hromadný přepis. Při spuštění bez specifického argumentu souboru přepisuje všechny soubory `zava-*.wav` ve složce `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Ukázka používá `FoundryLocalManager.CreateAsync()` a SDK `AudioClient` (s `Settings.Language = "en"`) pro přepis v procesu.

</details>

**Na co si dát pozor:** Porovnejte výstup přepisu s originálním textem ve skriptu `samples/audio/generate_samples.py`. Jak přesně Whisper zachytí názvy produktů jako "Zava ProGrip" a technické výrazy jako "brushless motor" nebo "composite decking"?

---

### Cvičení 5 - Pochopte klíčové vzory kódu

Studujte, jak se přepis Whisper liší od chatovacích doplňků v celých třech jazycích:

<details>
<summary><b>Python – klíčové rozdíly oproti chatu</b></summary>

```python
# Dokončení chatu (Části 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Přepis zvuku (tato část):
# Používá přímo ONNX Runtime místo klienta OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregresivní dekódovací smyčka ...
print(tokenizer.decode(generated_tokens))
```

**Klíčový poznatek:** Chatovací modely používají API kompatibilní s OpenAI přes `manager.endpoint`. Whisper využívá SDK pro lokalizaci uložených souborů ONNX modelů a pak přímo inferuje pomocí ONNX Runtime.

</details>

<details>
<summary><b>JavaScript – klíčové rozdíly oproti chatu</b></summary>

```javascript
// Dokončení chatu (Části 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Přepis zvuku (tato část):
// Používá vestavěného AudioClient SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Pro nejlepší výsledky vždy nastavte jazyk
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Klíčový poznatek:** Chatovací modely používají API kompatibilní s OpenAI přes `manager.urls[0] + "/v1"`. Přepis Whisper využívá SDK `AudioClient`, získaný pomocí `model.createAudioClient()`. Nastavte `settings.language`, aby nedošlo ke zkreslení z automatické detekce.

</details>

<details>
<summary><b>C# – klíčové rozdíly oproti chatu</b></summary>

Přístup v C# používá vestavěný SDK `AudioClient` pro přepis v procesu:

**Inicializace modelu:**

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

**Přepis:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Klíčový poznatek:** C# používá `FoundryLocalManager.CreateAsync()` a získá přímo `AudioClient` — není potřeba nastavovat ONNX Runtime. Nastavte `Settings.Language`, aby nedošlo ke zkreslení z automatické detekce.

</details>

> **Shrnutí:** Python využívá Foundry Local SDK pro správu modelů a ONNX Runtime pro přímou inferenci na modelech enkodéru/dekodéru. JavaScript a C# používají vestavěný SDK `AudioClient` pro zjednodušený přepis – vytvořte klienta, nastavte jazyk a zavolejte `transcribe()` / `TranscribeAudioAsync()`. Vždy nastavte jazykovou vlastnost na AudioClient pro přesné výsledky.

---

### Cvičení 6 - Experimentujte

Vyzkoušejte tyto modifikace pro lepší porozumění:

1. **Vyzkoušejte různé audio soubory** – nahrajte si vlastní hlas pomocí Windows Voice Recorder, uložte jako WAV a přepište

2. **Porovnejte varianty modelů** – pokud máte NVIDIA GPU, vyzkoušejte variantu CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Porovnejte rychlost přepisu oproti CPU variantě.

3. **Přidejte formátování výstupu** – JSON odpověď může obsahovat:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Vytvořte REST API** – zabalte svůj přepis do webového serveru:

   | Jazyk | Framework | Příklad |
   |-------|-----------|---------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` s `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` s `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` s `IFormFile` |

5. **Vícekolové sezení s přepisem** – zkombinujte Whisper s chat agentem z části 4: nejprve přepište audio, pak předávejte text agentovi pro analýzu nebo shrnutí.

---

## Reference Audio API SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — vytvoří instanci `AudioClient`
> - `audioClient.settings.language` — nastavte jazyk přepisu (např. `"en"`)
> - `audioClient.settings.temperature` — ovládání náhodnosti (volitelné)
> - `audioClient.transcribe(filePath)` — přepis souboru, vrací `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — streamování částí přepisu přes callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — vytvoří instanci `OpenAIAudioClient`
> - `audioClient.Settings.Language` — nastavte jazyk přepisu (např. `"en"`)
> - `audioClient.Settings.Temperature` — ovládání náhodnosti (volitelné)
> - `await audioClient.TranscribeAudioAsync(filePath)` — přepis souboru, vrací objekt s `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — vrací `IAsyncEnumerable` částí přepisu

> **Tip:** Vždy nastavte jazykovou vlastnost před přepisem. Bez ní se model Whisper pokusí o automatickou detekci, která může produkovat zkreslený výstup (jediný náhradní znak místo textu).

---

## Porovnání: Chat modely vs. Whisper

| Aspekt | Chat modely (časti 3-7) | Whisper – Python | Whisper – JS / C# |
|--------|-------------------------|-----------------|-------------------|
| **Typ úlohy** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Vstup** | Textové zprávy (JSON) | Audio soubory (WAV/MP3/M4A) | Audio soubory (WAV/MP3/M4A) |
| **Výstup** | Generovaný text (streamovaný) | Přepsaný text (kompletní) | Přepsaný text (kompletní) |
| **SDK balíček** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API metoda** | `client.chat.completions.create()` | ONNX Runtime přímo | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Nastavení jazyka** | N/A | Tokeny promptu dekodéru | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streamování** | Ano | Ne | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Výhoda soukromí** | Kód/data zůstávají lokální | Audiodata zůstávají lokální | Audiodata zůstávají lokální |

---

## Hlavní poznatky

| Koncept | Co jste se naučili |
|---------|--------------------|
| **Whisper na zařízení** | Přepis řeči na text běží plně lokálně, ideální pro přepis zákaznických hovorů a recenzí produktů Zava přímo na zařízení |
| **SDK AudioClient** | JavaScript a C# SDK poskytují vestavěný `AudioClient`, který řeší celý přepis jedním voláním |
| **Nastavení jazyka** | Vždy nastavte jazyk AudioClientu (např. `"en"`) — bez toho může auto-detekce produkovat zkreslený výstup |
| **Python** | Používá `foundry-local-sdk` pro správu modelů + `onnxruntime` + `transformers` + `librosa` pro přímou inferenci ONNX |
| **JavaScript** | Používá `foundry-local-sdk` s `model.createAudioClient()` — nastav `settings.language`, pak zavolej `transcribe()` |
| **C#** | Používá `Microsoft.AI.Foundry.Local` s `model.GetAudioClientAsync()` — nastav `Settings.Language`, pak zavolej `TranscribeAudioAsync()` |
| **Podpora streamování** | JS a C# SDK také nabízejí `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` pro výstup po částech |
| **Optimalizace pro CPU** | CPU varianta (3,05 GB) funguje na libovolném Windows zařízení bez GPU |
| **Soukromí na prvním místě** | Ideální pro zachování zákaznických interakcí Zava a proprietárních dat na zařízení |

---

## Zdroje

| Zdroj | Odkaz |
|-------|-------|
| Dokumentace Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Reference Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper model | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Další krok

Pokračujte do [části 10: Použití vlastních nebo Hugging Face modelů](part10-custom-models.md) pro sestavení vlastních modelů z Hugging Face a jejich spuštění přes Foundry Local.