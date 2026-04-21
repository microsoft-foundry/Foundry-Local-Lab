![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 9: Puheen tekstitys Whisperilla ja Foundry Localilla

> **Tavoite:** Käytä OpenAI Whisper -mallia, joka toimii paikallisesti Foundry Localin kautta äänen tiedostojen tekstittämiseen – täysin laitteella, pilveä ei tarvita.

## Yleiskatsaus

Foundry Local ei ole pelkästään tekstin generointiin; se tukee myös **puheesta tekstiksi** -malleja. Tässä harjoituksessa käytät **OpenAI Whisper Medium** -mallia äänitiedostojen transkriptioon täysin omalla koneellasi. Tämä on ihanteellista tilanteissa kuten Zava-asiakaspalvelupuheluiden, tuotearviointitallenteiden tai työpajasuunnittelusessioiden tekstittämiseen, joissa äänidata ei saa koskaan poistua laitteeltasi.


---

## Oppimistavoitteet

Tämän harjoituksen lopussa osaat:

- Ymmärtää Whisper-puheesta tekstiksi -mallin ja sen ominaisuudet
- Ladata ja ajaa Whisper-mallia Foundry Localin avulla
- Tekstittää äänitiedostoja Foundry Local SDK:n avulla Pythonilla, JavaScriptillä ja C#:lla
- Rakentaa yksinkertaisen tekstityspalvelun, joka toimii kokonaan laitteella
- Ymmärtää erot chat-/tekstimallien ja äänimallien välillä Foundry Localissa

---

## Esivaatimukset

| Vaatimus | Tiedot |
|-------------|---------|
| **Foundry Local CLI** | Versio **0.8.101 tai uudempi** (Whisper-mallit ovat saatavilla v0.8.101 alkaen) |
| **Käyttöjärjestelmä** | Windows 10/11 (x64 tai ARM64) |
| **Ohjelmointikielen ajonaikainen ympäristö** | **Python 3.9+** ja/tai **Node.js 18+** ja/tai **.NET 9 SDK** ([Lataa .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Suoritettu** | [Osa 1: Alkuun pääsy](part1-getting-started.md), [Osa 2: Foundry Local SDK syväsukellus](part2-foundry-local-sdk.md), ja [Osa 3: SDK:t ja API:t](part3-sdk-and-apis.md) |

> **Huom:** Whisper-mallit on ladattava **SDK:n** kautta (ei CLI:n). CLI ei tue äänen tekstityspistettä. Tarkista versiosi komennolla:
> ```bash
> foundry --version
> ```

---

## Käsite: Miten Whisper toimii Foundry Localin kanssa

OpenAI Whisper -malli on yleiskäyttöinen puheentunnistusmalli, joka on koulutettu suurella, monipuolisella äänidatalla. Foundry Localin kautta ajettaessa:

- Malli toimii **täysin CPU:lla** – GPU:ta ei tarvita
- Ääni ei koskaan poistu laitteeltasi – **täydellinen yksityisyys**
- Foundry Local SDK hoitaa mallin latauksen ja välimuistin hallinnan
- **JavaScript ja C#** sisältävät SDK:ssa valmiin `AudioClient`-luokan, joka hoitaa koko tekstitysputken — ei tarvitse manuaalista ONNX-konfiguraatiota
- **Python** käyttää SDK:ta mallinhallintaan ja ONNX Runtimea suorittaakseen suoraan inferencea encoder- ja decoder-ONNX-malleilla

### Miten putki toimii (JavaScript ja C#) — SDK AudioClient

1. **Foundry Local SDK** lataa ja tallentaa välimuistiin Whisper-mallin
2. `model.createAudioClient()` (JS) tai `model.GetAudioClientAsync()` (C#) luo `AudioClient`-olion
3. `audioClient.transcribe(path)` (JS) tai `audioClient.TranscribeAudioAsync(path)` (C#) hoitaa koko tekstitysprosessin sisäisesti — äänen esikäsittely, enkooderi, dekooderi ja tokenien dekoodaus
4. `AudioClient` tarjoaa `settings.language`-ominaisuuden (asetetaan `"en"` englannille) tarkkaa tekstitystä varten

### Miten putki toimii (Python) — ONNX Runtime

1. **Foundry Local SDK** lataa ja tallentaa välimuistiin Whisper-ONNX-mallin tiedostot
2. **Äänen esikäsittely** muuntaa WAV-äänen mel-spektrogrammiksi (80 mel-bin:iä x 3000 ruutua)
3. **Encoder** käsittelee mel-spektrogrammin ja tuottaa piilotettuja tiloja sekä cross-attentionin avain/arvotensoreita
4. **Decoder** toimii autoregressiivisesti, generoiden tokenin kerrallaan kunnes se tuottaa tekstin lopetus-tokenin
5. **Tokenisoija** muuntaa token-ID:t takaisin luettavaan tekstiin

### Whisper-mallivariaatiot

| Aliaksen nimi | Mallin ID | Laite | Koko | Kuvaus |
|--------------|------------|-------|------|--------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-kiihdytetty (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU:lle optimoitu (suositeltu useimmille laitteille) |

> **Huom:** Toisin kuin chat-mallit, jotka näkyvät oletuksena, Whisper-mallit luokitellaan `automatic-speech-recognition`-tehtävän alle. Käytä komentoa `foundry model info whisper-medium` nähdäksesi tiedot.

---

## Lab-harjoitukset

### Harjoitus 0 - Hanki näyteäänitiedostot

Tässä työpajassa on valmiiksi tehtyjä WAV-tiedostoja Zava DIY -tuotetilanteista. Luo ne mukana tulevalla skriptillä:

```bash
# Repojuuresta - luo ja aktivoi ensin .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Tämä luo kuusi WAV-tiedostoa kansioon `samples/audio/`:

| Tiedosto | Tilanne |
|----------|---------|
| `zava-customer-inquiry.wav` | Asiakas kysyy **Zava ProGrip Langattomasta Porakoneesta** |
| `zava-product-review.wav` | Asiakas arvostelee **Zava UltraSmooth Sisustusmaalin** |
| `zava-support-call.wav` | Tukipuhelu **Zava TitanLock Työkalukaapista** |
| `zava-project-planning.wav` | Tee-se-itse -tekijä suunnittelee terassia **Zava EcoBoard Komposiitti-terassilaudoilla** |
| `zava-workshop-setup.wav` | Työpajan läpikäynti käyttäen **kaikkia viittä Zava-tuotetta** |
| `zava-full-project-walkthrough.wav` | Laajempi autotallin remontin läpikäynti käyttäen **kaikkia Zava-tuotteita** (~4 minuuttia, pitkän äänen testi) |

> **Vinkki:** Voit myös käyttää omia WAV/MP3/M4A-tiedostojasi tai äänittää itsesi Windows Voice Recorderilla.

---

### Harjoitus 1 - Lataa Whisper-malli SDK:n avulla

Uudemmissa Foundry Localin versioissa CLI ei ole yhteensopiva Whisper-mallien kanssa, joten käytä **SDK:ta** mallin lataamiseen ja latauksen hallintaan. Valitse ohjelmointikielesi:

<details>
<summary><b>🐍 Python</b></summary>

**Asenna SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Käynnistä palvelu
manager = FoundryLocalManager()
manager.start_service()

# Tarkista luettelon tiedot
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Tarkista onko jo välimuistissa
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Lataa malli muistiin
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Tallenna tiedostoksi `download_whisper.py` ja suorita:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Asenna SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Luo hallinnoija ja käynnistä palvelu
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Hae malli luettelosta
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

// Lataa malli muistiin
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Tallenna tiedostoksi `download-whisper.mjs` ja suorita:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Asenna SDK:**
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

> **Miksi SDK eikä CLI?** Foundry Local CLI ei tue Whisper-mallien lataamista tai tarjoamista suoraan. SDK tarjoaa luotettavan tavan ladata ja hallita äänimalleja ohjelmallisesti. JavaScriptin ja C#:n SDK:ssa on sisäänrakennettu `AudioClient`, joka hoitaa koko tekstitysputken. Python käyttää ONNX Runtimea suoraan tallennettuihin malliin.

---

### Harjoitus 2 - Ymmärrä Whisper SDK

Whisper-tekstitys käyttää eri lähestymistapoja eri kielillä. **JavaScript ja C#** tarjoavat Foundry Local SDK:ssa valmiin `AudioClient`-luokan, joka hoitaa koko putken (äänen esikäsittely, enkooderi, dekooderi, tokenien dekoodaus) yhdellä metodikutsulla. **Python** käyttää Foundry Local SDK:ta mallinhallintaan ja ONNX Runtimea suoraan encoder-/decoder-ONNX-malleille.

| Komponentti | Python | JavaScript | C# |
|-------------|--------|------------|----|
| **SDK-paketit** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Mallinhallinta** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **Ominaisuuksien poiminta** | `WhisperFeatureExtractor` + `librosa` | SDK:n `AudioClient` hoitaa | SDK:n `AudioClient` hoitaa |
| **Päätelmä** | `ort.InferenceSession` (enkooderi + dekooderi) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Tokenien dekoodaus** | `WhisperTokenizer` | SDK:n `AudioClient` hoitaa | SDK:n `AudioClient` hoitaa |
| **Kielen asetus** | Asetetaan `forced_ids` dekooderilla | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Syöte** | WAV-tiedoston polku | WAV-tiedoston polku | WAV-tiedoston polku |
| **Tuloste** | Dekoodattu teksti | `result.text` | `result.Text` |

> **Tärkeää:** Aseta aina kieli `AudioClient`ille (esim. `"en"` englannille). Ilman selkeää kieliasetusta malli saattaa tuottaa sekavaa tulosta yrittäessään tunnistaa kieltä automaattisesti.

> **SDK-mallit:** Python käyttää `FoundryLocalManager(alias)`-instanssia alustusvaiheessa ja `get_cache_location()` löytää ONNX-mallit. JavaScript ja C# käyttävät SDK:n sisäänrakennettua `AudioClient`ia — haetaan `model.createAudioClient()` (JS) tai `model.GetAudioClientAsync()` (C#) -metodilla — joka hoitaa koko tekstitysputken. Katso [Osa 2: Foundry Local SDK syväsukellus](part2-foundry-local-sdk.md) täydellisiä tietoja varten.

---

### Harjoitus 3 - Rakenna yksinkertainen tekstityssovellus

Valitse oma kieliprojektisi ja rakenna minimaalinen sovellus, joka tekstittää äänitiedoston.

> **Tuetut ääniformaatit:** WAV, MP3, M4A. Parhaan tuloksen saat WAV-tiedostoilla, joissa näytetaajuus 16 kHz.

<details>
<summary><h3>Python-rata</h3></summary>

#### Aseta ympäristö

```bash
cd python
python -m venv venv

# Aktivoi virtuaaliympäristö:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Tekstityskoodi

Luo tiedosto `foundry-local-whisper.py`:

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

# Vaihe 1: Bootstrap - käynnistää palvelun, lataa ja lataa mallin
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Rakenna polku välimuistissa oleviin ONNX-mallin tiedostoihin
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Vaihe 2: Lataa ONNX-istunnot ja ominaisuuksien erottaja
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

# Vaihe 3: Erottele mel-spektrogrammin ominaisuudet
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Vaihe 4: Suorita kooderi
enc_out = encoder.run(None, {"audio_features": input_features})
# Ensimmäinen ulostulo on piilotetut tilat; loput ovat ristihavaitun KV-parit
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Vaihe 5: Autoregressiivinen dekoodaus
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transkripti, ei aikatunnisteita
input_ids = np.array([initial_tokens], dtype=np.int32)

# Tyhjä itsehuomion KV-välimuisti
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

    if next_token == 50257:  # tekstin loppu
        break
    generated.append(next_token)

    # Päivitä itsehuomion KV-välimuisti
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Suorita koodi

```bash
# Kirjoita Zava-tuotetilanne
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Tai kokeile muita:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Keskeiset Python-pisteet

| Metodi | Tarkoitus |
|--------|-----------|
| `FoundryLocalManager(alias)` | Alustus: käynnistää palvelun, lataa ja lataa mallin |
| `manager.get_cache_location()` | Hakee polun välimuistiin tallennettuihin ONNX-mallien tiedostoihin |
| `WhisperFeatureExtractor.from_pretrained()` | Lataa mel-spektrogrammin ominaisuuden poimijan |
| `ort.InferenceSession()` | Luo ONNX Runtime -istunnot enkooderille ja dekooderille |
| `tokenizer.decode()` | Muuntaa mallin tuottamat token-ID:t takaisin tekstiksi |

</details>

<details>
<summary><h3>JavaScript-rata</h3></summary>

#### Aseta ympäristö

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Tekstityskoodi

Luo tiedosto `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Vaihe 1: Bootstrap - luo hallinnoija, käynnistä palvelu ja lataa malli
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

// Vaihe 2: Luo ääniasiakas ja tee puheen tekstitys
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Siivous
await model.unload();
```

> **Huom:** Foundry Local SDK tarjoaa sisäänrakennetun `AudioClient`-luokan metodilla `model.createAudioClient()`, joka hoitaa koko ONNX-inferenssiputken sisäisesti — erillistä `onnxruntime-node`-importtia ei tarvita. Aseta aina `audioClient.settings.language = "en"` tarkkaan englanninkieliseen tekstitykseen.

#### Suorita koodi

```bash
# Tekstitä Zava-tuotetilanne
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Tai kokeile muita:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Keskeiset JavaScript-pisteet

| Metodi | Tarkoitus |
|--------|-----------|
| `FoundryLocalManager.create({ appName })` | Luo manager singletonin |
| `await catalog.getModel(alias)` | Hae malli katalogista |
| `model.download()` / `model.load()` | Lataa ja lataa Whisper-malli |
| `model.createAudioClient()` | Luo AudioClient tekstitykselle |
| `audioClient.settings.language = "en"` | Aseta tekstityksen kieli (vaaditaan tarkkaan tulokseen) |
| `audioClient.transcribe(path)` | Tekstitä äänitiedosto, palauttaa `{ text, duration }` |

</details>

<details>
<summary><h3>C#-rata</h3></summary>

#### Aseta ympäristö

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Huom:** C#-radalla käytetään `Microsoft.AI.Foundry.Local` -pakettia, joka tarjoaa sisäänrakennetun `AudioClient`-luokan metodilla `model.GetAudioClientAsync()`. Se hoitaa koko tekstitysputken prosessin sisällä — erillistä ONNX Runtime -asetusta ei tarvita.

#### Tekstityskoodi

Korvaa `Program.cs`-tiedoston sisältö:

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

#### Suorita koodi

```bash
# Kirjoita ylös Zava-tuotetilanne
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Tai kokeile muita:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Keskeiset C#-pisteet

| Metodi | Tarkoitus |
|--------|-----------|
| `FoundryLocalManager.CreateAsync(config)` | Alusta Foundry Local konfiguraatiolla |
| `catalog.GetModelAsync(alias)` | Hae malli katalogista |
| `model.DownloadAsync()` | Lataa Whisper-malli |
| `model.GetAudioClientAsync()` | Hae AudioClient (ei ChatClient!) |
| `audioClient.Settings.Language = "en"` | Aseta tekstityksen kieli (vaaditaan tarkkaan tulokseen) |
| `audioClient.TranscribeAudioAsync(path)` | Tekstitä äänitiedosto |
| `result.Text` | Tekstitetty tulos |


> **C# vs Python/JS:** C#-SDK tarjoaa sisäänrakennetun `AudioClient`-luokan prosessissa tapahtuviin litterointeihin `model.GetAudioClientAsync()` avulla, kuten JavaScript SDK. Python käyttää ONNX Runtimea suoraan koodin pakatussa enkooderi/dekooderi-mallien päällä inferenssiin.

</details>

---

### Harjoitus 4 - Litteroi kaikki Zava-näytteet erissä

Nyt kun sinulla on toimiva litterointisovellus, litteroi kaikki viisi Zava-näytetiedostoa ja vertaa tuloksia.

<details>
<summary><h3>Python-ketju</h3></summary>

Koko näyte `python/foundry-local-whisper.py` tukee jo erälitterointia. Kun sitä ajetaan ilman argumentteja, se litteroi kaikki `zava-*.wav` -tiedostot kansiossa `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Näyte käyttää `FoundryLocalManager(alias)` käynnistämiseen ja sitten suorittaa enkooderin ja dekooderin ONNX-istunnot jokaiselle tiedostolle.

</details>

<details>
<summary><h3>JavaScript-ketju</h3></summary>

Koko näyte `javascript/foundry-local-whisper.mjs` tukee jo erälitterointia. Kun sitä ajetaan ilman argumentteja, se litteroi kaikki `zava-*.wav` -tiedostot kansiossa `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Näyte käyttää `FoundryLocalManager.create()` ja `catalog.getModel(alias)` SDK:n alustamiseen, sitten käyttää `AudioClient`-instanssia (asetuksella `settings.language = "en"`) jokaisen tiedoston litterointiin.

</details>

<details>
<summary><h3>C#-ketju</h3></summary>

Koko näyte `csharp/WhisperTranscription.cs` tukee jo erälitterointia. Kun sitä ajetaan ilman tiedostospecificointia, se litteroi kaikki `zava-*.wav` -tiedostot kansiossa `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Näyte käyttää `FoundryLocalManager.CreateAsync()` ja SDK:n `AudioClient`-luokkaa (asetuksella `Settings.Language = "en"`) prosessissa tapahtuvalle litteroinnille.

</details>

**Mitä tarkkailla:** Vertaa litterointitulosta alkuperäiseen tekstiin `samples/audio/generate_samples.py`-tiedostosta. Kuinka tarkasti Whisper tunnistaa tuotemerkit kuten "Zava ProGrip" ja tekniset termit kuten "harjaton moottori" tai "komposiittilankku"?

---

### Harjoitus 5 - Ymmärrä keskeiset koodimallit

Tarkastele, miten Whisperin litterointi eroaa chat-completioneista kaikilla kolmella kielellä:

<details>
<summary><b>Python - Keskeiset erot chatiin</b></summary>

```python
# Chat-vastaus (Osa 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Äänitietojen tarkistus (Tämä osa):
# Käyttää ONNX Runtimea suoraan OpenAI-asiakkaan sijaan
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressiivinen dekooderi-silmukka ...
print(tokenizer.decode(generated_tokens))
```

**Keskeinen oivallus:** Chat-mallit käyttävät OpenAI-yhteensopivaa APIa `manager.endpoint`-osoitteen kautta. Whisper käyttää SDK:ta paikantaakseen välimuistissa olevat ONNX-mallitiedostot ja suorittaa inferenssin suoraan ONNX Runtimella.

</details>

<details>
<summary><b>JavaScript - Keskeiset erot chatiin</b></summary>

```javascript
// Keskustelun täydennys (Osat 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Äänen transkriptio (Tämä osa):
// Käyttää SDK:n sisäänrakennettua AudioClientia
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Aseta aina kieli parhaita tuloksia varten
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Keskeinen oivallus:** Chat-mallit käyttävät OpenAI-yhteensopivaa APIa osoitteessa `manager.urls[0] + "/v1"`. Whisper-litterointi käyttää SDK:n `AudioClient`-instanssia, joka saadaan `model.createAudioClient()`-kutsulla. Aseta `settings.language` estääksesi auto-tunnistuksen aiheuttaman epämääräisen tekstin.

</details>

<details>
<summary><b>C# - Keskeiset erot chatiin</b></summary>

C#-lähestymistavassa käytetään SDK:n sisäänrakennettua `AudioClient`-luokkaa prosessissa tapahtuviin litterointeihin:

**Mallin alustaminen:**

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

**Litterointi:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Keskeinen oivallus:** C# käyttää `FoundryLocalManager.CreateAsync()`-kutsua ja saa `AudioClient`-instanssin suoraan — ei tarvita erillistä ONNX Runtime -asetusta. Aseta `Settings.Language` välttääksesi auto-tunnistuksen aiheuttaman sotkuisen tekstin.

</details>

> **Yhteenveto:** Python käyttää Foundry Local SDK:a mallien hallintaan ja ONNX Runtimea suorittaakseen inferenssin enkooderi/dekooderi-malleja vastaan. JavaScript ja C# käyttävät molemmat SDK:n sisäänrakennettua `AudioClient`-luokkaa sujuvaan litterointiin — luo client, aseta kieli, ja kutsu `transcribe()` / `TranscribeAudioAsync()`. Aseta aina kieliasetus AudioClientille tarkkojen tulosten saamiseksi.

---

### Harjoitus 6 - Kokeile

Kokeile näitä muutoksia syventääksesi ymmärrystäsi:

1. **Kokeile eri äänitiedostoja** - tallenna omaa puhettasi Windows Voice Recorderilla, tallenna WAV-muotoon ja litteroi

2. **Vertaa mallivariaatioita** - jos sinulla on NVIDIA GPU, kokeile CUDA-versiota:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Vertaa litteroinnin nopeutta CPU-versioon.

3. **Lisää tulostuksen muotoilu** - JSON-vastauksessa voi olla:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Rakenna REST API** - kiedo litterointikoodisi web-palvelimeen:

   | Kieli | Kehys | Esimerkki |
   |-------|--------|---------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` käyttäen `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` käyttäen `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` käyttäen `IFormFile` |

5. **Monikierros litteroinnilla** - yhdistä Whisper chat-agenttiin osassa 4: litteroi ensin ääni, anna sitten teksti agentille analysoitavaksi tai tiivistettäväksi.

---

## SDK Audio API Viite

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — luo `AudioClient`-instanssin
> - `audioClient.settings.language` — asettaa litterointikielen (esim. `"en"`)
> - `audioClient.settings.temperature` — ohjaa sattumanvaraisuutta (valinnainen)
> - `audioClient.transcribe(filePath)` — litteroi tiedoston, palauttaa `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — suoratoistaa litterointipalasia callback-funktiolla
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — luo `OpenAIAudioClient`-instanssin
> - `audioClient.Settings.Language` — asettaa litterointikielen (esim. `"en"`)
> - `audioClient.Settings.Temperature` — ohjaa sattumanvaraisuutta (valinnainen)
> - `await audioClient.TranscribeAudioAsync(filePath)` — litteroi tiedoston, palauttaa objektin `.Text`-kentällä
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — palauttaa `IAsyncEnumerable` litterointipalasia

> **Vinkki:** Aseta aina kieliasetus ennen litterointia. Ilman sitä Whisper-malli yrittää automaattitunnistusta, mikä voi synnyttää sotkuista tekstiä (yksi korvausmerkki tekstin sijaan).

---

## Vertailu: Chat-mallit vs. Whisper

| Ominaisuus | Chat-mallit (osat 3–7) | Whisper - Python | Whisper - JS / C# |
|------------|------------------------|------------------|-------------------|
| **Tehtävätyyppi** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Syöte** | Tekstiviestit (JSON) | Äänitiedostot (WAV/MP3/M4A) | Äänitiedostot (WAV/MP3/M4A) |
| **Tulos** | Generoitu teksti (striimattu) | Litteroitu teksti (kokonainen) | Litteroitu teksti (kokonainen) |
| **SDK-paketti** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API-metodi** | `client.chat.completions.create()` | ONNX Runtime suoraan | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Kieliasetus** | Ei käytössä | Dekooderin kehotustokenit | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Suoratoisto** | Kyllä | Ei | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Yksityisyys** | Koodi/data pysyy paikallisena | Äänidata pysyy paikallisena | Äänidata pysyy paikallisena |

---

## Keskeiset opit

| Käsite | Mitä opit |
|--------|-----------|
| **Whisper laitteessa** | Puhetekstiksi -muunnos toimii kokonaan paikallisesti, ihanteellinen Zava-asiakaspalvelupuheluiden ja tuotearvostelujen litterointiin laitteella |
| **SDK AudioClient** | JavaScript- ja C#-SDK:t tarjoavat sisäänrakennetun `AudioClient`-luokan, joka hoitaa koko litterointiputken yhdellä kutsulla |
| **Kieliasetus** | Aseta aina AudioClientin kieli (esim. `"en"`) — ilman sitä auto-tunnistus voi aiheuttaa sotkuista tekstiä |
| **Python** | Käyttää `foundry-local-sdk` mallien hallintaan + `onnxruntime` + `transformers` + `librosa` suoralla ONNX-inferenssillä |
| **JavaScript** | Käyttää `foundry-local-sdk`:a mallin luomiseen `model.createAudioClient()` — aseta `settings.language`, sitten kutsu `transcribe()` |
| **C#** | Käyttää `Microsoft.AI.Foundry.Local` -SDK:a `model.GetAudioClientAsync()`-kutsulla — aseta `Settings.Language`, sitten kutsu `TranscribeAudioAsync()` |
| **Suoratoistotuki** | JS- ja C#-SDK:t tarjoavat myös `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` chunkki kerrallaan tulostukseen |
| **CPU-optimoitu** | CPU-versio (3,05 Gt) toimii millä tahansa Windows-laitteella ilman GPU:ta |
| **Yksityisyys ensin** | Täydellinen ratkaisu pitää Zava-asiakaskohtaamiset ja tuotetiedot paikallisina |

---

## Resurssit

| Resurssi | Linkki |
|----------|--------|
| Foundry Local -dokumentaatio | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Viite | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper -malli | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local -verkkosivusto | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Seuraava askel

Jatka osaan [Part 10: Using Custom or Hugging Face Models](part10-custom-models.md) käärittääksesi omia mallejasi Hugging Facesta ja ajaaksesi niitä Foundry Localin kautta.