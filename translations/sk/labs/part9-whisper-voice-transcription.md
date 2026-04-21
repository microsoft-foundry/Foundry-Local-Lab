![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Časť 9: Prepis hlasu pomocou Whisper a Foundry Local

> **Cieľ:** Použiť model OpenAI Whisper spustený lokálne cez Foundry Local na prepis audio súborov - úplne na zariadení, bez potreby cloudu.

## Prehľad

Foundry Local nie je len na generovanie textu; podporuje aj **modely premeny reči na text**. V tomto cvičení použijete model **OpenAI Whisper Medium** na prepis audio súborov kompletne na vašom zariadení. Je to ideálne pre situácie ako prepis zákazníckych hovorov Zava, nahrávok recenzií produktov alebo plánovania workshopov, kde audio dáta nesmú nikdy opustiť vaše zariadenie.

---

## Učebné ciele

Po skončení tohto cvičenia budete schopní:

- Pochopiť model premeny reči na text Whisper a jeho možnosti
- Stiahnuť a spustiť model Whisper pomocou Foundry Local
- Prepisovať audio súbory pomocou Foundry Local SDK v Pythone, JavaScripte a C#
- Vytvoriť jednoduchú službu prepisu, ktorá beží kompletne na zariadení
- Pochopiť rozdiely medzi chatovými/textovými modelmi a audio modelmi vo Foundry Local

---

## Požiadavky

| Požiadavka | Podrobnosti |
|------------|-------------|
| **Foundry Local CLI** | Verzia **0.8.101 alebo novšia** (Whisper modely sú dostupné od verzie v0.8.101) |
| **OS** | Windows 10/11 (x64 alebo ARM64) |
| **Jazykové runtime** | **Python 3.9+** a/alebo **Node.js 18+** a/alebo **.NET 9 SDK** ([Stiahnuť .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Dokončené** | [Časť 1: Začíname](part1-getting-started.md), [Časť 2: Hĺbkový pohľad na Foundry Local SDK](part2-foundry-local-sdk.md) a [Časť 3: SDK a API](part3-sdk-and-apis.md) |

> **Poznámka:** Modely Whisper sa musia sťahovať cez **SDK** (nie cez CLI). CLI nepodporuje endpoint pre prepis audia. Verziu si overíte príkazom:
> ```bash
> foundry --version
> ```

---

## Koncepcia: Ako Whisper funguje s Foundry Local

Model OpenAI Whisper je univerzálny model rozpoznávania reči trénovaný na veľkej dátovej sade rôznorodého audia. Pri spustení cez Foundry Local:

- Model beží **úplne na vašom CPU** - GPU nie je potrebné
- Audio nikdy neopúšťa vaše zariadenie - **plná ochrana súkromia**
- Foundry Local SDK sa stará o stiahnutie a správu cache modelu
- **JavaScript a C#** poskytujú v SDK vstavaného klienta `AudioClient`, ktorý spracuje celý prepis bez manuálneho nastavovania ONNX
- **Python** používa SDK na správu modelu a ONNX Runtime pre priamy inference na enkódovacích/dekódovacích ONNX modeloch

### Ako funguje pipeline (JavaScript a C#) — SDK AudioClient

1. **Foundry Local SDK** stiahne a uloží do cache model Whisper
2. `model.createAudioClient()` (JS) alebo `model.GetAudioClientAsync()` (C#) vytvorí `AudioClient`
3. `audioClient.transcribe(path)` (JS) alebo `audioClient.TranscribeAudioAsync(path)` (C#) zavolá celý pipeline interne — predspracovanie audia, enkódovanie, dekódovanie a dekódovanie tokenov
4. `AudioClient` vystavuje vlastnosť `settings.language` (nastavenú na `"en"` pre angličtinu) pre presný prepis

### Ako funguje pipeline (Python) — ONNX Runtime

1. **Foundry Local SDK** stiahne a uloží do cache ONNX modely Whisper
2. **Predspracovanie audia** prevedie WAV audio na mel spektrálny graf (80 mel košov x 3000 snímok)
3. **Enkóder** spracuje mel spektrum a vytvorí skryté stavy plus key/value tenzory pre cross-attention
4. **Dekóder** beží autoregresívne, generuje token po tokene až do vzniku tokenu konca textu
5. **Tokenizér** dekóduje výstupné token ID späť do čitateľného textu

### Varianty modelu Whisper

| Alias | ID modelu | Zariadenie | Veľkosť | Popis |
|-------|-----------|------------|---------|-------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU akcelerované (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Optimalizované pre CPU (odporúčané pre väčšinu zariadení) |

> **Poznámka:** Na rozdiel od chat modelov, ktoré sú predvolene v zozname, sú modely Whisper klasifikované v úlohe `automatic-speech-recognition`. Pre zobrazenie detailov použite `foundry model info whisper-medium`.

---

## Laboratórne cvičenia

### Cvičenie 0 - Stiahnite si ukážkové audio súbory

Táto laboratórna úloha obsahuje predpripravené WAV súbory založené na scenároch produktov Zava DIY. Vygenerujte ich pomocou priloženého skriptu:

```bash
# Zo základného adresára repozitára - najprv vytvorte a aktivujte .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Toto vytvorí šesť WAV súborov v adresári `samples/audio/`:

| Súbor | Scenár |
|-------|---------|
| `zava-customer-inquiry.wav` | Zákazník sa pýta na **Zava ProGrip bezkáblovú vŕtačku** |
| `zava-product-review.wav` | Zákaznícka recenzia na **Zava UltraSmooth vnútornú farbu** |
| `zava-support-call.wav` | Podporný hovor o **Zava TitanLock nástrojovej skrinke** |
| `zava-project-planning.wav` | Plánovanie terasy s **Zava EcoBoard kompozitným terasovým materiálom** |
| `zava-workshop-setup.wav` | Prehliadka dielne s použitím **všetkých piatich produktov Zava** |
| `zava-full-project-walkthrough.wav` | Rozsiahly prehľad renovácie garáže s použitím **všetkých produktov Zava** (~4 min, pre testovanie dlhého audia) |

> **Tip:** Môžete použiť aj vlastné WAV/MP3/M4A súbory alebo nahrať sa pomocou Windows Voice Recorder.

---

### Cvičenie 1 - Stiahnite model Whisper pomocou SDK

Kvôli nekompatibilite CLI s modelmi Whisper v novších verziách Foundry Local použite **SDK** na stiahnutie a načítanie modelu. Vyberte si svoj jazyk:

<details>
<summary><b>🐍 Python</b></summary>

**Inštalujte SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Spustiť službu
manager = FoundryLocalManager()
manager.start_service()

# Skontrolovať informácie o katalógu
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Skontrolovať, či je už v cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Načítať model do pamäte
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Uložte ako `download_whisper.py` a spustite:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Inštalujte SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Vytvorte manažéra a spustite službu
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Získajte model z katalógu
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

// Načítajte model do pamäte
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Uložte ako `download-whisper.mjs` a spustite:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Inštalujte SDK:**
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

> **Prečo SDK namiesto CLI?** Foundry Local CLI priamo nepodporuje sťahovanie alebo službu modelov Whisper. SDK poskytuje spoľahlivý spôsob programatickej správy a sťahovania audio modelov. JavaScript a C# SDK obsahujú vstavaného `AudioClient`, ktorý spracuje celý pipeline prepisu. Python používa ONNX Runtime pre priamy inference na uložených modelových súboroch.

---

### Cvičenie 2 - Pochopte Whisper SDK

Prepis pomocou Whisper používa rôzne prístupy podľa jazyka. **JavaScript a C#** poskytujú v Foundry Local SDK vstavaného `AudioClient`, ktorý spracuje celý pipeline (predspracovanie, enkódovanie, dekódovanie a dekódovanie tokenov) v jednom volaní. **Python** používa Foundry Local SDK na správu modelu a ONNX Runtime na priamu inferenciu enkódovacích a dekódovacích ONNX modelov.

| Komponent | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK balíčky** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Správa modelu** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Extrakcia príznakov** | `WhisperFeatureExtractor` + `librosa` | Rieši SDK `AudioClient` | Rieši SDK `AudioClient` |
| **Inference** | `ort.InferenceSession` (enkóder + dekóder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Dekódovanie tokenov** | `WhisperTokenizer` | Rieši SDK `AudioClient` | Rieši SDK `AudioClient` |
| **Nastavenie jazyka** | Nastavenie cez `forced_ids` v dekodéri tokenov | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Vstup** | Cesta k WAV súboru | Cesta k WAV súboru | Cesta k WAV súboru |
| **Výstup** | Dekódovaný textový reťazec | `result.text` | `result.Text` |

> **Dôležité:** Vždy nastavte jazyk na `AudioClient` (napr. `"en"` pre angličtinu). Bez explicitného nastavenia jazyka môže model produkovať nezrozumiteľný výstup, lebo sa snaží jazyk automaticky zistiť.

> **SDK vzory:** Python používa `FoundryLocalManager(alias)` na bootstrap, potom `get_cache_location()` pre načítanie ONNX modelových súborov. JavaScript a C# využívajú vstavaného `AudioClient` SDK — získaného cez `model.createAudioClient()` (JS) alebo `model.GetAudioClientAsync()` (C#) — ktorý spracuje celý pipeline prepisu. Kompletné detaily nájdete v [Časť 2: Hĺbkový pohľad na Foundry Local SDK](part2-foundry-local-sdk.md).

---

### Cvičenie 3 - Vytvorte jednoduchú aplikáciu pre prepis

Vyberte si svoj jazyk a vytvorte minimálnu aplikáciu, ktorá prepisuje zvukový súbor.

> **Podporované audio formáty:** WAV, MP3, M4A. Pre najlepšie výsledky používajte WAV súbory s frekvenciou 16kHz.

<details>
<summary><h3>Python track</h3></summary>

#### Nastavenie

```bash
cd python
python -m venv venv

# Aktivujte virtuálne prostredie:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Kód prepisu

Vytvorte súbor `foundry-local-whisper.py`:

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

# Krok 1: Bootstrap - spúšťa službu, sťahuje a načítava model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Vytvorenie cesty k uloženým ONNX modelovým súborom
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Krok 2: Načítanie ONNX sedení a extraktora funkcií
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

# Krok 3: Extrahovanie mel spektrogramových funkcií
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Krok 4: Spustenie enkódera
enc_out = encoder.run(None, {"audio_features": input_features})
# Prvý výstup sú skryté stavy; zvyšné sú KV páry krížovej pozornosti
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Krok 5: Autoregresívne dekódovanie
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, prepis, bez časových značiek
input_ids = np.array([initial_tokens], dtype=np.int32)

# Prázdna cache KV pre vlastnú pozornosť
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

    if next_token == 50257:  # koniec textu
        break
    generated.append(next_token)

    # Aktualizácia cache KV pre vlastnú pozornosť
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Spustite ho

```bash
# Prepísať scenár produktu Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Alebo vyskúšajte iné:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Kľúčové body pre Python

| Metóda | Účel |
|--------|-------|
| `FoundryLocalManager(alias)` | Bootstrap: spustenie služby, stiahnutie a načítanie modelu |
| `manager.get_cache_location()` | Získanie cesty k uloženým ONNX modelovým súborom |
| `WhisperFeatureExtractor.from_pretrained()` | Načítanie extraktora mel spektra |
| `ort.InferenceSession()` | Vytvorenie ONNX Runtime sessions pre enkóder a dekóder |
| `tokenizer.decode()` | Prevod výstupných token ID na text |

</details>

<details>
<summary><h3>JavaScript track</h3></summary>

#### Nastavenie

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Kód prepisu

Vytvorte súbor `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Krok 1: Bootstrap - vytvorte manažéra, spustite službu a načítajte model
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

// Krok 2: Vytvorte audio klienta a prepisujte
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Upratanie
await model.unload();
```

> **Poznámka:** Foundry Local SDK poskytuje vstavaného `AudioClient` cez `model.createAudioClient()`, ktorý spracuje celý ONNX pipeline interne — nie je potrebné importovať `onnxruntime-node`. Vždy nastavte `audioClient.settings.language = "en"` pre presný anglický prepis.

#### Spustite ho

```bash
# Prepísať scenár produktu Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Alebo vyskúšajte iné:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Kľúčové body pre JavaScript

| Metóda | Účel |
|--------|-------|
| `FoundryLocalManager.create({ appName })` | Vytvorenie singleton manažéra |
| `await catalog.getModel(alias)` | Získanie modelu z katalógu |
| `model.download()` / `model.load()` | Stiahnutie a načítanie modelu Whisper |
| `model.createAudioClient()` | Vytvorenie audio klienta pre prepis |
| `audioClient.settings.language = "en"` | Nastavenie jazyka pre prepis (nutné pre presný výstup) |
| `audioClient.transcribe(path)` | Prepis audio súboru, vráti `{ text, duration }` |

</details>

<details>
<summary><h3>C# track</h3></summary>

#### Nastavenie

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Poznámka:** C# track používa balík `Microsoft.AI.Foundry.Local`, ktorý poskytuje vstavaného `AudioClient` cez `model.GetAudioClientAsync()`. Ten spracuje celý pipeline prepisu v procese — nemusíte zvlášť nastavovať ONNX Runtime.

#### Kód prepisu

Nahraďte obsah súboru `Program.cs`:

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

#### Spustite ho

```bash
# Prepísať scenár produktu Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Alebo skúste iné:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Kľúčové body pre C#

| Metóda | Účel |
|--------|-------|
| `FoundryLocalManager.CreateAsync(config)` | Inicializácia Foundry Local s konfiguráciou |
| `catalog.GetModelAsync(alias)` | Získanie modelu z katalógu |
| `model.DownloadAsync()` | Stiahnutie modelu Whisper |
| `model.GetAudioClientAsync()` | Získanie AudioClienta (nie ChatClienta!) |
| `audioClient.Settings.Language = "en"` | Nastavenie jazyka pre prepis (nutné pre presný výstup) |
| `audioClient.TranscribeAudioAsync(path)` | Prepis audio súboru |
| `result.Text` | Prepisaný text |
> **C# vs Python/JS:** SDK C# poskytuje vstavaný `AudioClient` pre transkripciu v rámci procesu cez `model.GetAudioClientAsync()`, podobne ako JavaScript SDK. Python priamo používa ONNX Runtime na inferenciu nad uloženými encoder/decoder modelmi.

</details>

---

### Cvičenie 4 - Hromadná transkripcia všetkých Zava vzoriek

Keď už máte funkčnú transkripčnú aplikáciu, preveďte transkripciu všetkých piatich Zava vzoriek a porovnajte výsledky.

<details>
<summary><h3>Python sekcia</h3></summary>

Celý vzorový súbor `python/foundry-local-whisper.py` už podporuje hromadnú transkripciu. Keď sa spustí bez argumentov, prepisuje všetky súbory `zava-*.wav` v `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Vzorový príklad používa `FoundryLocalManager(alias)` na bootstrap, potom spúšťa ONNX sessions encodéra a decodéra pre každý súbor.

</details>

<details>
<summary><h3>JavaScript sekcia</h3></summary>

Celý vzorový súbor `javascript/foundry-local-whisper.mjs` už podporuje hromadnú transkripciu. Keď sa spustí bez argumentov, prepisuje všetky súbory `zava-*.wav` v `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Vzorový príklad používa `FoundryLocalManager.create()` a `catalog.getModel(alias)` na inicializáciu SDK, potom používa `AudioClient` (s `settings.language = "en"`) na transkripciu každého súboru.

</details>

<details>
<summary><h3>C# sekcia</h3></summary>

Celý vzorový súbor `csharp/WhisperTranscription.cs` už podporuje hromadnú transkripciu. Keď sa spustí bez konkrétneho argumentu súboru, prepisuje všetky súbory `zava-*.wav` v `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Vzorový príklad používa `FoundryLocalManager.CreateAsync()` a SDK `AudioClient` (s `Settings.Language = "en"`) pre transkripciu v rámci procesu.

</details>

**Na čo sa zamerať:** Porovnajte transkripčný výstup s pôvodným textom v `samples/audio/generate_samples.py`. Ako presne Whisper zachytáva názvy produktov ako "Zava ProGrip" a technické termíny ako "brushless motor" alebo "composite decking"?

---

### Cvičenie 5 - Pochopenie kľúčových kódových vzorov

Preskúmajte, ako sa transkripcia Whisper líši od chatových doplnení vo všetkých troch jazykoch:

<details>
<summary><b>Python – kľúčové rozdiely oproti chatu</b></summary>

```python
# Dokončenie rozhovoru (časti 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Prepis zvuku (táto časť):
# Používa ONNX Runtime priamo namiesto klienta OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregresívna slučka dekodéra ...
print(tokenizer.decode(generated_tokens))
```

**Kľúčový postreh:** Chat modely používajú API kompatibilné s OpenAI cez `manager.endpoint`. Whisper používa SDK na lokalizáciu uložených ONNX modelových súborov a potom priamo vykonáva inferenciu cez ONNX Runtime.

</details>

<details>
<summary><b>JavaScript – kľúčové rozdiely oproti chatu</b></summary>

```javascript
// Dokončenie chatu (Časti 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Prepis zvuku (Táto časť):
// Používa vstavaný AudioClient SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Pre najlepšie výsledky vždy nastavte jazyk
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Kľúčový postreh:** Chat modely používajú API kompatibilné s OpenAI cez `manager.urls[0] + "/v1"`. Transkripcia Whisper využíva SDK `AudioClient`, získaný z `model.createAudioClient()`. Nastavte `settings.language`, aby ste sa vyhli nečitateľnému výstupu z automatickej detekcie.

</details>

<details>
<summary><b>C# – kľúčové rozdiely oproti chatu</b></summary>

Prístup C# využíva vstavaný SDK `AudioClient` pre transkripciu v rámci procesu:

**Inicializácia modelu:**

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

**Transkripcia:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Kľúčový postreh:** C# používa `FoundryLocalManager.CreateAsync()` a priamo získava `AudioClient` – nie je potrebné nastavovať ONNX Runtime. Nastavte `Settings.Language`, aby ste predišli nečitateľnému výstupu z automatickej detekcie.

</details>

> **Zhrnutie:** Python používa Foundry Local SDK na správu modelov a ONNX Runtime pre priamu inferenciu nad encoder/decoder modelmi. JavaScript a C# oba používajú vstavaný SDK `AudioClient` pre zjednodušenú transkripciu – vytvorte klienta, nastavte jazyk a volajte `transcribe()` / `TranscribeAudioAsync()`. Vždy nastavte jazyk vlastnosť v AudioClient pre presné výsledky.

---

### Cvičenie 6 - Experimentujte

Vyskúšajte tieto zmeny, aby ste si prehĺbili porozumenie:

1. **Vyskúšajte rôzne audio súbory** – nahrajte si seba hovoriaceho pomocou Windows Voice Recorder, uložte ako WAV a prepíšte ho

2. **Porovnajte varianty modelov** – ak máte NVIDIA GPU, skúste variant CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
 Porovnajte rýchlosť transkripcie oproti CPU variantu.

3. **Pridajte formátovanie výstupu** – JSON odpoveď môže obsahovať:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Vytvorte REST API** – zabaľte svoj transkripčný kód do webového servera:

   | Jazyk | Framework | Príklad |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` s `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` s `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` s `IFormFile` |

5. **Viackolové použitie s transkripciou** – kombinujte Whisper s chatovým agentom z Časti 4: najprv prepisujte audio a potom tento text poskytnite agentovi na analýzu alebo zhrnutie.

---

## SDK Audio API Referencia

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — vytvorí inštanciu `AudioClient`
> - `audioClient.settings.language` — nastavuje jazyk transkripcie (napr. `"en"`)
> - `audioClient.settings.temperature` — ovláda náhodnosť (voliteľné)
> - `audioClient.transcribe(filePath)` — prepíše súbor, vracia `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — streamuje transkripčné kúsky cez callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — vytvorí inštanciu `OpenAIAudioClient`
> - `audioClient.Settings.Language` — nastavuje jazyk transkripcie (napr. `"en"`)
> - `audioClient.Settings.Temperature` — ovláda náhodnosť (voliteľné)
> - `await audioClient.TranscribeAudioAsync(filePath)` — prepíše súbor, vracia objekt s `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — vracia `IAsyncEnumerable` s transkripčnými kúskami

> **Tip:** Vždy nastavte jazyk vlastnosť pred transkripciou. Bez nej model Whisper skúša automatickú detekciu, čo môže viesť k nečitateľnému výstupu (jeden náhradný znak namiesto textu).

---

## Porovnanie: Chat modely vs. Whisper

| Aspekt | Chat modely (Časti 3-7) | Whisper – Python | Whisper – JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Typ úlohy** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Vstup** | Textové správy (JSON) | Audio súbory (WAV/MP3/M4A) | Audio súbory (WAV/MP3/M4A) |
| **Výstup** | Generovaný text (streamovaný) | Prepísaný text (kompletný) | Prepísaný text (kompletný) |
| **SDK balík** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API metóda** | `client.chat.completions.create()` | ONNX Runtime priamo | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Nastavenie jazyka** | N/A | Tokeny promptu dekodéra | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Áno | Nie | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Výhoda pre súkromie** | Kód/dáta zostávajú lokálne | Audio dáta zostávajú lokálne | Audio dáta zostávajú lokálne |

---

## Hlavné poznatky

| Koncept | Čo ste sa naučili |
|---------|-----------------|
| **Whisper na zariadení** | Prevádzka konverzie reči na text beží kompletne lokálne, ideálne pre prepis zákazníckych hovorov a produktových recenzií Zava priamo na zariadení |
| **SDK AudioClient** | JavaScript a C# SDK poskytujú vstavaný `AudioClient`, ktorý spracuje celú transkripčnú pipeline v jednom volaní |
| **Nastavenie jazyka** | Vždy nastavte jazyk v AudioClient (napr. `"en"`) – bez toho môže automatická detekcia generovať nečitateľný výstup |
| **Python** | Používa `foundry-local-sdk` pre správu modelov + `onnxruntime` + `transformers` + `librosa` pre priamu ONNX inferenciu |
| **JavaScript** | Používa `foundry-local-sdk` s `model.createAudioClient()` – nastavte `settings.language`, potom volajte `transcribe()` |
| **C#** | Používa `Microsoft.AI.Foundry.Local` s `model.GetAudioClientAsync()` – nastavte `Settings.Language`, potom volajte `TranscribeAudioAsync()` |
| **Podpora streamovania** | JS a C# SDK tiež ponúkajú `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` pre výstup po častiach |
| **Optimalizované pre CPU** | CPU varianta (3,05 GB) funguje na akomkoľvek Windows zariadení bez GPU |
| **Súkromie na prvom mieste** | Ideálne na uchovanie zákazníckych interakcií a proprietárnych produktových dát Zava priamo na zariadení |

---

## Zdroje

| Zdroj | Odkaz |
|----------|------|
| Foundry Local dokumentácia | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referencia | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper model | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local webstránka | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Ďalší krok

Pokračujte na [Časť 10: Používanie vlastných alebo Hugging Face modelov](part10-custom-models.md) a zostavte si vlastné modely z Hugging Face a spustite ich cez Foundry Local.