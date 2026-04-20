![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Teil 9: Sprachtranskription mit Whisper und Foundry Local

> **Ziel:** Verwenden Sie das OpenAI Whisper-Modell, das lokal über Foundry Local ausgeführt wird, um Audiodateien zu transkribieren – vollständig auf dem Gerät, keine Cloud erforderlich.

## Übersicht

Foundry Local ist nicht nur für die Textgenerierung geeignet; es unterstützt auch **Speech-to-Text**-Modelle. In diesem Labor verwenden Sie das **OpenAI Whisper Medium**-Modell, um Audiodateien vollständig auf Ihrem Gerät zu transkribieren. Dies ist ideal für Szenarien wie die Transkription von Kundendienstanrufen von Zava, Produktbewertungsaufnahmen oder Workshop-Planungssitzungen, bei denen Audiodaten das Gerät niemals verlassen dürfen.

---

## Lernziele

Am Ende dieses Labors werden Sie in der Lage sein:

- Das Whisper-Spracherkennungsmodell und seine Fähigkeiten zu verstehen
- Das Whisper-Modell mit Foundry Local herunterzuladen und auszuführen
- Audiodateien mithilfe des Foundry Local SDK in Python, JavaScript und C# zu transkribieren
- Einen einfachen Transkriptionsdienst zu erstellen, der vollständig auf dem Gerät läuft
- Die Unterschiede zwischen Chat-/Textmodellen und Audiomodellen in Foundry Local zu verstehen

---

## Voraussetzungen

| Anforderung | Details |
|-------------|---------|
| **Foundry Local CLI** | Version **0.8.101 oder höher** (Whisper-Modelle sind ab v0.8.101 verfügbar) |
| **Betriebssystem** | Windows 10/11 (x64 oder ARM64) |
| **Programmiersprachen-Runtime** | **Python 3.9+** und/oder **Node.js 18+** und/oder **.NET 9 SDK** ([.NET herunterladen](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Abgeschlossen** | [Teil 1: Erste Schritte](part1-getting-started.md), [Teil 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), und [Teil 3: SDKs und APIs](part3-sdk-and-apis.md) |

> **Hinweis:** Whisper-Modelle müssen über das **SDK** heruntergeladen werden (nicht über die CLI). Die CLI unterstützt den Audiotranskriptions-Endpunkt nicht. Prüfen Sie Ihre Version mit:
> ```bash
> foundry --version
> ```

---

## Konzept: Wie Whisper mit Foundry Local funktioniert

Das OpenAI Whisper-Modell ist ein universelles Spracherkennungsmodell, das auf einem großen Datensatz vielfältiger Audiodaten trainiert wurde. Beim Ausführen mit Foundry Local:

- Läuft das Modell **vollständig auf Ihrer CPU** – keine GPU erforderlich
- Audio verlässt das Gerät niemals – **vollständiger Datenschutz**
- Das Foundry Local SDK verwaltet den Modell-Download und das Cache-Management
- **JavaScript und C#** bieten einen integrierten `AudioClient` im SDK, der die gesamte Transkriptions-Pipeline übernimmt – keine manuelle ONNX-Konfiguration erforderlich
- **Python** nutzt das SDK für das Modellmanagement und ONNX Runtime für direkte Inferenz auf die Encoder-/Decoder-ONNX-Modelle

### Wie die Pipeline funktioniert (JavaScript und C#) – SDK AudioClient

1. **Foundry Local SDK** lädt das Whisper-Modell herunter und cached es
2. `model.createAudioClient()` (JS) oder `model.GetAudioClientAsync()` (C#) erstellt einen `AudioClient`
3. `audioClient.transcribe(path)` (JS) oder `audioClient.TranscribeAudioAsync(path)` (C#) kümmert sich intern um die komplette Pipeline — Audio-Vorverarbeitung, Encoder, Decoder und Token-Dekodierung
4. Der `AudioClient` bietet eine Eigenschaft `settings.language` (auf `"en"` für Englisch gesetzt), um die genaue Transkription zu steuern

### Wie die Pipeline funktioniert (Python) – ONNX Runtime

1. **Foundry Local SDK** lädt die Whisper-ONNX-Modell-Dateien herunter und cached sie
2. **Audio-Vorverarbeitung** wandelt WAV-Audio in ein Mel-Spektrogramm um (80 Mel-Bins x 3000 Frames)
3. **Encoder** verarbeitet das Mel-Spektrogramm und erzeugt versteckte Zustände sowie Cross-Attention-Key-/Value-Tensoren
4. **Decoder** läuft autoregressiv und generiert ein Token nach dem anderen, bis ein End-of-Text-Token erzeugt wird
5. **Tokenizer** dekodiert die ausgegebenen Token-IDs zurück in lesbaren Text

### Whisper-Modellvarianten

| Alias | Modell-ID | Gerät | Größe | Beschreibung |
|-------|-----------|-------|-------|--------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1,53 GB | GPU-beschleunigt (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3,05 GB | CPU-optimiert (für die meisten Geräte empfohlen) |

> **Hinweis:** Anders als Chatmodelle, die standardmäßig angezeigt werden, sind Whisper-Modelle unter der Kategorie `automatic-speech-recognition` eingeordnet. Verwenden Sie `foundry model info whisper-medium`, um Details zu sehen.

---

## Laborübungen

### Übung 0 – Beispiel-Audiodateien erhalten

Dieses Labor enthält vorgefertigte WAV-Dateien, die auf Zava DIY-Produktszenarien basieren. Erzeugen Sie diese mit dem beigefügten Skript:

```bash
# Vom Repo-Stammverzeichnis aus - zuerst eine .venv erstellen und aktivieren
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Dies erstellt sechs WAV-Dateien in `samples/audio/`:

| Datei | Szenario |
|-------|----------|
| `zava-customer-inquiry.wav` | Kunde fragt nach der **Zava ProGrip Akku-Bohrmaschine** |
| `zava-product-review.wav` | Kunde bewertet die **Zava UltraSmooth Innenfarbe** |
| `zava-support-call.wav` | Supportanruf zur **Zava TitanLock Werkzeugkiste** |
| `zava-project-planning.wav` | Heimwerker plant eine Terrasse mit **Zava EcoBoard Verbundterrassendielen** |
| `zava-workshop-setup.wav` | Rundgang durch eine Werkstatt mit **allen fünf Zava-Produkten** |
| `zava-full-project-walkthrough.wav` | Ausführlicher Garagenrenovierungsrundgang mit **allen Zava-Produkten** (~4 Min, für Langzeit-Audio-Tests) |

> **Tipp:** Sie können auch eigene WAV-/MP3-/M4A-Dateien verwenden oder sich mit dem Windows-Sprachrekorder aufnehmen.

---

### Übung 1 – Whisper-Modell mit dem SDK herunterladen

Aufgrund von CLI-Inkompatibilitäten mit Whisper-Modellen in neueren Foundry Local-Versionen verwenden Sie das **SDK**, um das Modell herunterzuladen und zu laden. Wählen Sie Ihre Sprache:

<details>
<summary><b>🐍 Python</b></summary>

**SDK installieren:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Starte den Dienst
manager = FoundryLocalManager()
manager.start_service()

# Kataloginformationen überprüfen
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Prüfen, ob bereits zwischengespeichert
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Lade das Modell in den Speicher
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Speichern Sie als `download_whisper.py` und führen Sie aus:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK installieren:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Manager erstellen und Dienst starten
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Modell aus dem Katalog abrufen
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

// Das Modell in den Speicher laden
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Speichern Sie als `download-whisper.mjs` und führen Sie aus:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK installieren:**
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

> **Warum SDK statt CLI?** Die Foundry Local CLI unterstützt nicht das direkte Herunterladen oder Bereitstellen von Whisper-Modellen. Das SDK bietet eine verlässliche Möglichkeit, Audiomodelle programmatisch herunterzuladen und zu verwalten. Die JavaScript- und C#-SDKs enthalten einen integrierten `AudioClient`, der die gesamte Transkriptions-Pipeline übernimmt. Python nutzt ONNX Runtime für direkte Inferenz gegen die zwischengespeicherten Modelldateien.

---

### Übung 2 – Whisper SDK verstehen

Die Whisper-Transkription verwendet je nach Sprache unterschiedliche Ansätze. **JavaScript und C#** bieten im Foundry Local SDK einen eingebauten `AudioClient`, der die gesamte Pipeline (Audiovorverarbeitung, Encoder, Decoder, Token-Dekodierung) in einem einzelnen Methodenaufruf abwickelt. **Python** nutzt das Foundry Local SDK für das Modellmanagement und ONNX Runtime für direkte Inferenz gegen die Encoder-/Decoder-ONNX-Modelle.

| Komponente | Python | JavaScript | C# |
|------------|--------|------------|----|
| **SDK-Pakete** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Modellverwaltung** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + Katalog |
| **Feature-Extraktion** | `WhisperFeatureExtractor` + `librosa` | Vom SDK `AudioClient` verwaltet | Vom SDK `AudioClient` verwaltet |
| **Inference** | `ort.InferenceSession` (Encoder + Decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token-Dekodierung** | `WhisperTokenizer` | Vom SDK `AudioClient` verwaltet | Vom SDK `AudioClient` verwaltet |
| **Sprach-Einstellung** | Über `forced_ids` in Decoder-Tokens | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Eingabe** | WAV-Dateipfad | WAV-Dateipfad | WAV-Dateipfad |
| **Ausgabe** | Dekodierter Text-String | `result.text` | `result.Text` |

> **Wichtig:** Stellen Sie immer die Sprache im `AudioClient` ein (z. B. `"en"` für Englisch). Ohne eine explizite Spracheinstellung kann das Modell unsinnige Ausgaben erzeugen, da es versucht, die Sprache automatisch zu erkennen.

> **SDK-Muster:** Python nutzt `FoundryLocalManager(alias)` zum Bootstrap, dann `get_cache_location()`, um die ONNX-Modell-Dateien zu finden. JavaScript und C# verwenden den integrierten `AudioClient` des SDKs – er wird über `model.createAudioClient()` (JS) oder `model.GetAudioClientAsync()` (C#) bezogen – der die gesamte Transkriptions-Pipeline übernimmt. Details siehe [Teil 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md).

---

### Übung 3 – Eine einfache Transkriptions-App bauen

Wählen Sie Ihren Sprachfahrplan und erstellen Sie eine minimale Anwendung, die eine Audiodatei transkribiert.

> **Unterstützte Audioformate:** WAV, MP3, M4A. Für beste Ergebnisse verwenden Sie WAV-Dateien mit 16kHz Abtastrate.

<details>
<summary><h3>Python-Spur</h3></summary>

#### Einrichtung

```bash
cd python
python -m venv venv

# Aktiviere die virtuelle Umgebung:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transkriptions-Code

Erstellen Sie eine Datei namens `foundry-local-whisper.py`:

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

# Schritt 1: Bootstrap - startet den Dienst, lädt und lädt das Modell
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Pfad zu den zwischengespeicherten ONNX-Modell-Dateien erstellen
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Schritt 2: ONNX-Sitzungen und Merkmalsextraktor laden
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

# Schritt 3: Mel-Spektrogramm-Merkmale extrahieren
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Schritt 4: Encoder ausführen
enc_out = encoder.run(None, {"audio_features": input_features})
# Erste Ausgabe sind versteckte Zustände; die restlichen sind Cross-Attention-KV-Paare
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Schritt 5: Autoregressives Dekodieren
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transkribieren, keine Zeitstempel
input_ids = np.array([initial_tokens], dtype=np.int32)

# Leerer Self-Attention-KV-Cache
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

    if next_token == 50257:  # Ende des Textes
        break
    generated.append(next_token)

    # Self-Attention-KV-Cache aktualisieren
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Ausführen

```bash
# Transkribiere ein Zava-Produktszenario
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Oder probiere andere:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Wichtige Python-Punkte

| Methode | Zweck |
|---------|-------|
| `FoundryLocalManager(alias)` | Bootstrap: Dienst starten, Modell herunterladen und laden |
| `manager.get_cache_location()` | Pfad zu den zwischengespeicherten ONNX-Modell-Dateien erhalten |
| `WhisperFeatureExtractor.from_pretrained()` | Mel-Spektrogramm-Feature-Extractor laden |
| `ort.InferenceSession()` | ONNX Runtime-Sitzungen für Encoder und Decoder erstellen |
| `tokenizer.decode()` | Ausgabe-Token-IDs zurück in Text konvertieren |

</details>

<details>
<summary><h3>JavaScript-Spur</h3></summary>

#### Einrichtung

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transkriptions-Code

Erstellen Sie eine Datei namens `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Schritt 1: Bootstrap - Manager erstellen, Dienst starten und Modell laden
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

// Schritt 2: Einen Audio-Client erstellen und transkribieren
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Aufräumen
await model.unload();
```

> **Hinweis:** Das Foundry Local SDK stellt über `model.createAudioClient()` einen integrierten `AudioClient` bereit, der die gesamte ONNX-Inferenz-Pipeline intern übernimmt – kein Import von `onnxruntime-node` nötig. Stellen Sie immer `audioClient.settings.language = "en"` ein, um eine genaue englische Transkription zu gewährleisten.

#### Ausführen

```bash
# Ein Zava-Produktszenario transkribieren
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Oder andere ausprobieren:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Wichtige JavaScript-Punkte

| Methode | Zweck |
|---------|-------|
| `FoundryLocalManager.create({ appName })` | Erzeugt die Singleton-Manager-Instanz |
| `await catalog.getModel(alias)` | Ruft ein Modell aus dem Katalog ab |
| `model.download()` / `model.load()` | Laden und Initialisieren des Whisper-Modells |
| `model.createAudioClient()` | Erstellt einen Audio-Client für die Transkription |
| `audioClient.settings.language = "en"` | Setzt die Transkriptionssprache (für genaue Ausgabe erforderlich) |
| `audioClient.transcribe(path)` | Transkribiert eine Audiodatei, gibt `{ text, duration }` zurück |

</details>

<details>
<summary><h3>C#-Spur</h3></summary>

#### Einrichtung

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Hinweis:** Die C#-Spur nutzt das Paket `Microsoft.AI.Foundry.Local`, das über `model.GetAudioClientAsync()` einen integrierten `AudioClient` bereitstellt. Dieser übernimmt die gesamte Transkriptions-Pipeline im Prozess – kein separates ONNX-Runtime-Setup erforderlich.

#### Transkriptions-Code

Ersetzen Sie den Inhalt von `Program.cs`:

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

#### Ausführen

```bash
# Übertragen Sie ein Zava-Produktszenario
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Oder probieren Sie andere aus:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Wichtige C#-Punkte

| Methode | Zweck |
|---------|-------|
| `FoundryLocalManager.CreateAsync(config)` | Initialisiert Foundry Local mit der Konfiguration |
| `catalog.GetModelAsync(alias)` | Holt das Modell aus dem Katalog |
| `model.DownloadAsync()` | Lädt das Whisper-Modell herunter |
| `model.GetAudioClientAsync()` | Holt den AudioClient (nicht ChatClient!) |
| `audioClient.Settings.Language = "en"` | Setzt die Transkriptionssprache (erforderlich für genaue Ergebnisse) |
| `audioClient.TranscribeAudioAsync(path)` | Transkribiert eine Audiodatei |
| `result.Text` | Der transkribierte Text |
> **C# vs Python/JS:** Das C# SDK stellt einen integrierten `AudioClient` für In-Process-Transkriptionen über `model.GetAudioClientAsync()` bereit, ähnlich wie das JavaScript SDK. Python verwendet direkt ONNX Runtime zur Inferenz der zwischengespeicherten Encoder-/Decoder-Modelle.

</details>

---

### Übung 4 - Alle Zava-Beispieldateien batchweise transkribieren

Jetzt, wo Sie eine funktionierende Transkriptions-App haben, transkribieren Sie alle fünf Zava-Beispieldateien und vergleichen die Ergebnisse.

<details>
<summary><h3>Python-Track</h3></summary>

Das vollständige Beispiel `python/foundry-local-whisper.py` unterstützt bereits die Batch-Transkription. Wird es ohne Argumente ausgeführt, transkribiert es alle `zava-*.wav` Dateien in `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Das Beispiel verwendet `FoundryLocalManager(alias)` zum Bootstrapping und führt dann die ONNX-Sessions für Encoder und Decoder für jede Datei aus.

</details>

<details>
<summary><h3>JavaScript-Track</h3></summary>

Das vollständige Beispiel `javascript/foundry-local-whisper.mjs` unterstützt bereits die Batch-Transkription. Wird es ohne Argumente ausgeführt, transkribiert es alle `zava-*.wav` Dateien in `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Das Beispiel verwendet `FoundryLocalManager.create()` und `catalog.getModel(alias)`, um das SDK zu initialisieren, und nutzt anschließend den `AudioClient` (mit `settings.language = "en"`), um jede Datei zu transkribieren.

</details>

<details>
<summary><h3>C#-Track</h3></summary>

Das vollständige Beispiel `csharp/WhisperTranscription.cs` unterstützt bereits die Batch-Transkription. Wird es ohne spezifisches Dateiarugment ausgeführt, transkribiert es alle `zava-*.wav` Dateien in `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Das Beispiel nutzt `FoundryLocalManager.CreateAsync()` und den SDK-`AudioClient` (mit `Settings.Language = "en"`) für eine In-Process-Transkription.

</details>

**Worauf Sie achten sollten:** Vergleichen Sie die Transkriptionsergebnisse mit dem Originaltext in `samples/audio/generate_samples.py`. Wie genau erfasst Whisper Produktnamen wie „Zava ProGrip“ und technische Begriffe wie „brushless motor“ oder „composite decking“?

---

### Übung 5 - Verstehen der wichtigsten Code-Muster

Analysieren Sie, wie sich die Whisper-Transkription von Chat-Completions in allen drei Sprachen unterscheidet:

<details>
<summary><b>Python – Wichtige Unterschiede zu Chat</b></summary>

```python
# Chat-Vervollständigung (Teil 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Audio-Transkription (Dieser Teil):
# Verwendet ONNX Runtime direkt statt des OpenAI-Clients
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressiver Decoder-Schleife ...
print(tokenizer.decode(generated_tokens))
```

**Wichtigster Punkt:** Chat-Modelle nutzen die OpenAI-kompatible API über `manager.endpoint`. Whisper verwendet das SDK, um die zwischengespeicherten ONNX-Modell-Dateien zu finden, und führt die Inferenz direkt mit ONNX Runtime durch.

</details>

<details>
<summary><b>JavaScript – Wichtige Unterschiede zu Chat</b></summary>

```javascript
// Chat-Vervollständigung (Teile 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Audio-Transkription (Dieser Teil):
// Verwendet den integrierten AudioClient des SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Sprache immer für beste Ergebnisse festlegen
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Wichtigster Punkt:** Chat-Modelle verwenden die OpenAI-kompatible API über `manager.urls[0] + "/v1"`. Die Whisper-Transkription nutzt den SDK-`AudioClient`, der aus `model.createAudioClient()` erhalten wird. Stellen Sie `settings.language` ein, um eine verzerrte Ausgabe durch automatische Erkennung zu vermeiden.

</details>

<details>
<summary><b>C# – Wichtige Unterschiede zu Chat</b></summary>

Der C#-Ansatz verwendet den integrierten SDK-`AudioClient` für In-Process-Transkriptionen:

**Modellinitialisierung:**

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

**Transkription:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Wichtigster Punkt:** C# verwendet `FoundryLocalManager.CreateAsync()` und erhält einen `AudioClient` direkt — keine ONNX Runtime Einrichtung erforderlich. Stellen Sie `Settings.Language` ein, um eine verzerrte Ausgabe durch automatische Erkennung zu vermeiden.

</details>

> **Zusammenfassung:** Python verwendet das Foundry Local SDK für das Modellmanagement und ONNX Runtime für direkte Inferenz gegen die Encoder/Decoder-Modelle. JavaScript und C# nutzen beide den integrierten SDK-`AudioClient` für eine optimierte Transkription — Client erstellen, Sprache setzen, dann `transcribe()` / `TranscribeAudioAsync()` aufrufen. Setzen Sie stets die Spracheigenschaft im AudioClient für genaue Ergebnisse.

---

### Übung 6 – Experimentieren

Probieren Sie diese Änderungen aus, um Ihr Verständnis zu vertiefen:

1. **Probieren Sie verschiedene Audiodateien aus** – Nehmen Sie sich selbst mit dem Windows-Sprachrekorder auf, speichern Sie als WAV und transkribieren Sie die Aufnahme

2. **Vergleichen Sie Modellvarianten** – Wenn Sie eine NVIDIA-GPU haben, probieren Sie die CUDA-Variante aus:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Vergleichen Sie die Transkriptionsgeschwindigkeit mit der CPU-Variante.

3. **Fügen Sie Ausgabeformatierung hinzu** – Die JSON-Antwort kann enthalten:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Bauen Sie eine REST-API** – Kapseln Sie Ihren Transkriptionscode in einem Webserver ein:

   | Sprache | Framework | Beispiel |
   |---------|-----------|----------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` mit `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` mit `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` mit `IFormFile` |

5. **Multi-Turn mit Transkription** – Kombinieren Sie Whisper mit einem Chat-Agenten aus Teil 4: Transkribieren Sie zuerst Audio, dann übergeben Sie den Text an den Agenten für Analyse oder Zusammenfassung.

---

## SDK Audio API Referenz

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — erstellt eine `AudioClient`-Instanz
> - `audioClient.settings.language` — setzt die Transkriptionssprache (z. B. `"en"`)
> - `audioClient.settings.temperature` — steuert die Zufälligkeit (optional)
> - `audioClient.transcribe(filePath)` — transkribiert eine Datei, gibt `{ text, duration }` zurück
> - `audioClient.transcribeStreaming(filePath, callback)` — streamt Transkriptionsabschnitte via Callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — erstellt eine `OpenAIAudioClient`-Instanz
> - `audioClient.Settings.Language` — setzt die Transkriptionssprache (z. B. `"en"`)
> - `audioClient.Settings.Temperature` — steuert die Zufälligkeit (optional)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transkribiert eine Datei, gibt ein Objekt mit `.Text` zurück
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — gibt `IAsyncEnumerable` von Transkriptionsabschnitten zurück

> **Tipp:** Setzen Sie immer die Spracheigenschaft vor der Transkription. Ohne diese versucht das Whisper-Modell eine automatische Erkennung, was zu verzerrter Ausgabe führen kann (ein einzelnes Ersetzungszeichen anstelle von Text).

---

## Vergleich: Chat-Modelle vs. Whisper

| Aspekt | Chat-Modelle (Teile 3-7) | Whisper – Python | Whisper – JS / C# |
|--------|--------------------------|-----------------|-------------------|
| **Aufgabentyp** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Eingabe** | Textnachrichten (JSON) | Audiodateien (WAV/MP3/M4A) | Audiodateien (WAV/MP3/M4A) |
| **Ausgabe** | Generierter Text (gestreamt) | Transkribierter Text (vollständig) | Transkribierter Text (vollständig) |
| **SDK-Paket** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API-Methode** | `client.chat.completions.create()` | ONNX Runtime direkt | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Spracheinstellung** | n.v. | Decoder Prompt Tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Ja | Nein | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Datenschutzvorteil** | Code / Daten bleiben lokal | Audiodaten bleiben lokal | Audiodaten bleiben lokal |

---

## Wichtige Erkenntnisse

| Konzept | Was Sie gelernt haben |
|---------|----------------------|
| **Whisper on-device** | Sprache-zu-Text läuft komplett lokal, ideal, um Zava-Kundengespräche und Produktbewertungen on-device zu transkribieren |
| **SDK AudioClient** | JavaScript- und C#-SDKs bieten einen integrierten `AudioClient`, der die komplette Transkriptionspipeline in einem Aufruf abdeckt |
| **Spracheinstellung** | Setzen Sie immer die Sprache im AudioClient (z. B. `"en"`) – ohne diese Einstellung erzeugt die automatische Erkennung möglicherweise fehlerhafte Ausgaben |
| **Python** | Verwendet `foundry-local-sdk` für Modellmanagement + `onnxruntime` + `transformers` + `librosa` für direkte ONNX-Inferenz |
| **JavaScript** | Verwendet `foundry-local-sdk` mit `model.createAudioClient()` — `settings.language` setzen, danach `transcribe()` aufrufen |
| **C#** | Verwendet `Microsoft.AI.Foundry.Local` mit `model.GetAudioClientAsync()` — `Settings.Language` setzen, danach `TranscribeAudioAsync()` aufrufen |
| **Streaming-Unterstützung** | JS- und C#-SDK bieten auch `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` für Ausgabe Stück für Stück |
| **CPU-optimiert** | Die CPU-Variante (3,05 GB) funktioniert auf jedem Windows-Gerät ohne GPU |
| **Privacy-first** | Perfekt, um Zava-Kundeninteraktionen und proprietäre Produktdaten lokal auf dem Gerät zu halten |

---

## Ressourcen

| Ressource | Link |
|----------|------|
| Foundry Local Dokumentation | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referenz | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper Modell | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local Webseite | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Nächster Schritt

Fahren Sie fort mit [Teil 10: Verwenden benutzerdefinierter oder Hugging Face Modelle](part10-custom-models.md), um eigene Modelle von Hugging Face zu kompilieren und mit Foundry Local auszuführen.