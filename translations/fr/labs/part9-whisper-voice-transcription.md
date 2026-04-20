![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partie 9 : Transcription vocale avec Whisper et Foundry Local

> **Objectif :** Utiliser le modèle OpenAI Whisper fonctionnant localement via Foundry Local pour transcrire des fichiers audio — entièrement sur l’appareil, sans cloud.

## Vue d’ensemble

Foundry Local ne se limite pas à la génération de texte ; il prend également en charge les modèles **speech-to-text**. Dans ce laboratoire, vous utiliserez le modèle **OpenAI Whisper Medium** pour transcrire des fichiers audio entièrement sur votre machine. Cela est idéal pour des scénarios comme la transcription d’appels du service client Zava, des enregistrements d’avis produits ou des sessions de planification d’atelier où les données audio ne doivent jamais quitter votre appareil.

---

## Objectifs d’apprentissage

À la fin de ce laboratoire, vous serez capable de :

- Comprendre le modèle de reconnaissance vocale Whisper et ses capacités
- Télécharger et exécuter le modèle Whisper via Foundry Local
- Transcrire des fichiers audio en utilisant le SDK Foundry Local en Python, JavaScript et C#
- Construire un service de transcription simple qui s’exécute entièrement sur l’appareil
- Comprendre les différences entre les modèles de chat/texte et les modèles audio dans Foundry Local

---

## Prérequis

| Exigence | Détails |
|-------------|---------|
| **Foundry Local CLI** | Version **0.8.101 ou supérieure** (les modèles Whisper sont disponibles à partir de la v0.8.101) |
| **OS** | Windows 10/11 (x64 ou ARM64) |
| **Environnement d'exécution** | **Python 3.9+** et/ou **Node.js 18+** et/ou **SDK .NET 9** ([Télécharger .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Validé** | [Partie 1 : Prise en main](part1-getting-started.md), [Partie 2 : Exploration du SDK Foundry Local](part2-foundry-local-sdk.md), [Partie 3 : SDKs et APIs](part3-sdk-and-apis.md) |

> **Note :** Les modèles Whisper doivent être téléchargés via le **SDK** (et non la CLI). La CLI ne supporte pas le point de terminaison de transcription audio. Vérifiez votre version avec :
> ```bash
> foundry --version
> ```

---

## Concept : Comment Whisper fonctionne avec Foundry Local

Le modèle OpenAI Whisper est un modèle général de reconnaissance vocale entraîné sur un large jeu de données audio diversifié. Lorsqu’il est exécuté via Foundry Local :

- Le modèle s’exécute **entièrement sur votre CPU** — pas besoin de GPU
- L’audio ne quitte jamais votre appareil — **confidentialité totale**
- Le SDK Foundry Local gère le téléchargement et la gestion du cache du modèle
- **JavaScript et C#** fournissent un `AudioClient` intégré dans le SDK qui gère toute la chaîne de transcription — aucune configuration ONNX manuelle requise
- **Python** utilise le SDK pour la gestion du modèle et ONNX Runtime pour l’inférence directe sur les modèles ONNX encodeur/décodeur

### Fonctionnement de la chaîne (JavaScript et C#) — SDK AudioClient

1. Le **SDK Foundry Local** télécharge et met en cache le modèle Whisper
2. `model.createAudioClient()` (JS) ou `model.GetAudioClientAsync()` (C#) crée un `AudioClient`
3. `audioClient.transcribe(path)` (JS) ou `audioClient.TranscribeAudioAsync(path)` (C#) gère en interne toute la chaîne — prétraitement audio, encodeur, décodeur, décodage des tokens
4. `AudioClient` expose une propriété `settings.language` (réglée sur `"en"` pour l’anglais) pour guider une transcription précise

### Fonctionnement de la chaîne (Python) — ONNX Runtime

1. Le **SDK Foundry Local** télécharge et met en cache les fichiers modèle Whisper ONNX
2. Le **prétraitement audio** convertit l’audio WAV en spectrogramme mel (80 bandes mel x 3000 frames)
3. L’**encodeur** traite le spectrogramme mel et produit des états cachés ainsi que des tenseurs clés/valeurs d’attention croisée
4. Le **décodeur** génère de façon autoregressive un token à la fois jusqu’à produire un token de fin de texte
5. Le **tokeniseur** décode les ID de tokens en texte lisible

### Variantes du modèle Whisper

| Alias | ID du modèle | Dispositif | Taille | Description |
|-------|--------------|------------|--------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Accélération GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Optimisé CPU (recommandé pour la majorité des appareils) |

> **Note :** Contrairement aux modèles de chat listés par défaut, les modèles Whisper sont catégorisés sous la tâche `automatic-speech-recognition`. Utilisez `foundry model info whisper-medium` pour voir les détails.

---

## Exercices du laboratoire

### Exercice 0 – Obtenir les fichiers audio exemples

Ce laboratoire inclut des fichiers WAV préconstruits basés sur des scénarios produits Zava. Générez-les avec le script inclus :

```bash
# Depuis la racine du dépôt - créez et activez d'abord un .venv
python -m venv .venv

# Windows (PowerShell) :
.venv\Scripts\Activate.ps1
# macOS :
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Cela crée six fichiers WAV dans `samples/audio/` :

| Fichier | Scénario |
|---------|----------|
| `zava-customer-inquiry.wav` | Client posant des questions sur la **Perceuse sans fil Zava ProGrip** |
| `zava-product-review.wav` | Client évaluant la **Peinture intérieure Zava UltraSmooth** |
| `zava-support-call.wav` | Appel support concernant la **Caisse à outils Zava TitanLock** |
| `zava-project-planning.wav` | Bricoleur planifiant une terrasse avec **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Présentation d’un atelier utilisant **les cinq produits Zava** |
| `zava-full-project-walkthrough.wav` | Visite détaillée de la rénovation d’un garage utilisant **tous les produits Zava** (~4 min, test long audio) |

> **Astuce :** Vous pouvez aussi utiliser vos propres fichiers WAV/MP3/M4A ou vous enregistrer avec l’enregistreur vocal Windows.

---

### Exercice 1 – Télécharger le modèle Whisper via le SDK

À cause d’incompatibilités CLI avec les modèles Whisper dans les versions récentes de Foundry Local, utilisez le **SDK** pour télécharger et charger le modèle. Choisissez votre langage :

<details>
<summary><b>🐍 Python</b></summary>

**Installer le SDK :**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Démarrer le service
manager = FoundryLocalManager()
manager.start_service()

# Vérifier les informations du catalogue
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Vérifier si déjà en cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Charger le modèle en mémoire
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Sauvegardez sous `download_whisper.py` et exécutez :
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Installer le SDK :**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Créez le gestionnaire et démarrez le service
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Obtenez le modèle du catalogue
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

// Chargez le modèle en mémoire
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Sauvegardez sous `download-whisper.mjs` et exécutez :
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Installer le SDK :**
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

> **Pourquoi le SDK plutôt que la CLI ?** La CLI Foundry Local ne supporte pas le téléchargement ni le service direct des modèles Whisper. Le SDK offre une manière fiable de télécharger et gérer les modèles audio via programmation. Les SDK JavaScript et C# incluent un `AudioClient` intégré qui gère l’ensemble du pipeline de transcription. Python utilise ONNX Runtime pour l’inférence directe sur les fichiers modèles mis en cache.

---

### Exercice 2 – Comprendre le SDK Whisper

La transcription Whisper utilise des approches différentes selon le langage. **JavaScript et C#** fournissent un `AudioClient` intégré dans le SDK Foundry Local qui gère toute la chaîne (prétraitement audio, encodeur, décodeur, décodage des tokens) en un seul appel de méthode. **Python** utilise le SDK Foundry Local pour la gestion du modèle et ONNX Runtime pour l’inférence directe sur les modèles ONNX encodeur/décodeur.

| Composant | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **Packages SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Gestion du modèle** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalogue |
| **Extraction de caractéristiques** | `WhisperFeatureExtractor` + `librosa` | Gérée par `AudioClient` du SDK | Gérée par `AudioClient` du SDK |
| **Inférence** | `ort.InferenceSession` (encodeur + décodeur) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Décodage des tokens** | `WhisperTokenizer` | Géré par `AudioClient` du SDK | Géré par `AudioClient` du SDK |
| **Réglage de la langue** | Définie via `forced_ids` dans les tokens du décodeur | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Entrée** | Chemin fichier WAV | Chemin fichier WAV | Chemin fichier WAV |
| **Sortie** | Chaîne de texte décodé | `result.text` | `result.Text` |

> **Important :** Toujours définir la langue sur l’`AudioClient` (par ex. `"en"` pour l’anglais). Sans réglage explicite de langue, le modèle peut produire des résultats erronés en tentant de détecter automatiquement la langue.

> **Modèles d’utilisation SDK :** Python utilise `FoundryLocalManager(alias)` au démarrage puis `get_cache_location()` pour localiser les fichiers modèles ONNX. JavaScript et C# utilisent `AudioClient` intégré via `model.createAudioClient()` (JS) ou `model.GetAudioClientAsync()` (C#) qui gère toute la chaîne de transcription. Consultez [Partie 2 : Exploration du SDK Foundry Local](part2-foundry-local-sdk.md) pour plus de détails.

---

### Exercice 3 – Construire une application de transcription simple

Choisissez votre langage et créez une application minimale qui transcrit un fichier audio.

> **Formats audio supportés :** WAV, MP3, M4A. Pour de meilleurs résultats, utilisez des fichiers WAV échantillonnés à 16 kHz.

<details>
<summary><h3>Parcours Python</h3></summary>

#### Installation

```bash
cd python
python -m venv venv

# Activez l'environnement virtuel :
# Windows (PowerShell) :
venv\Scripts\Activate.ps1
# macOS :
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Code de transcription

Créez un fichier `foundry-local-whisper.py` :

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

# Étape 1 : Bootstrap - démarre le service, télécharge et charge le modèle
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Construire le chemin vers les fichiers de modèle ONNX mis en cache
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Étape 2 : Charger les sessions ONNX et l'extracteur de caractéristiques
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

# Étape 3 : Extraire les caractéristiques du spectrogramme mel
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Étape 4 : Exécuter l'encodeur
enc_out = encoder.run(None, {"audio_features": input_features})
# La première sortie est les états cachés ; les autres sont des paires KV d'attention croisée
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Étape 5 : Décodage autorégressif
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcrire, sans horodatage
input_ids = np.array([initial_tokens], dtype=np.int32)

# Cache KV d'attention auto vide
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

    if next_token == 50257:  # fin du texte
        break
    generated.append(next_token)

    # Mettre à jour le cache KV d'attention auto
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Exécution

```bash
# Transcrire un scénario de produit Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Ou essayer d'autres :
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Points clés Python

| Méthode | But |
|---------|-----|
| `FoundryLocalManager(alias)` | Initialisation : démarrer le service, télécharger et charger le modèle |
| `manager.get_cache_location()` | Obtenir le chemin des fichiers modèles ONNX en cache |
| `WhisperFeatureExtractor.from_pretrained()` | Charger l’extracteur de caractéristiques du spectrogramme mel |
| `ort.InferenceSession()` | Créer des sessions ONNX Runtime pour encodeur et décodeur |
| `tokenizer.decode()` | Convertir les tokens de sortie en texte |

</details>

<details>
<summary><h3>Parcours JavaScript</h3></summary>

#### Installation

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Code de transcription

Créez un fichier `foundry-local-whisper.mjs` :

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Étape 1 : Bootstrap - créer un gestionnaire, démarrer le service et charger le modèle
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

// Étape 2 : Créer un client audio et transcrire
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Nettoyage
await model.unload();
```

> **Note :** Le SDK Foundry Local fournit un `AudioClient` intégré via `model.createAudioClient()` qui gère en interne toute la chaîne d’inférence ONNX — aucune importation `onnxruntime-node` nécessaire. Toujours définir `audioClient.settings.language = "en"` pour assurer une transcription anglaise précise.

#### Exécution

```bash
# Transcrire un scénario de produit Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Ou essayer d'autres :
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Points clés JavaScript

| Méthode | But |
|---------|-----|
| `FoundryLocalManager.create({ appName })` | Créer le singleton manager |
| `await catalog.getModel(alias)` | Obtenir un modèle dans le catalogue |
| `model.download()` / `model.load()` | Télécharger et charger le modèle Whisper |
| `model.createAudioClient()` | Créer un client audio pour la transcription |
| `audioClient.settings.language = "en"` | Définir la langue de transcription (nécessaire pour précision) |
| `audioClient.transcribe(path)` | Transcrire un fichier audio, retourne `{ text, duration }` |

</details>

<details>
<summary><h3>Parcours C#</h3></summary>

#### Installation

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Note :** Le parcours C# utilise le package `Microsoft.AI.Foundry.Local` qui fournit un `AudioClient` intégré via `model.GetAudioClientAsync()`. Cela gère tout le pipeline de transcription en processus — aucune configuration ONNX Runtime distincte nécessaire.

#### Code de transcription

Remplacez le contenu de `Program.cs` :

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

#### Exécution

```bash
# Transcrire un scénario de produit Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Ou essayez d'autres :
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Points clés C#

| Méthode | But |
|---------|-----|
| `FoundryLocalManager.CreateAsync(config)` | Initialiser Foundry Local avec la configuration |
| `catalog.GetModelAsync(alias)` | Obtenir un modèle depuis le catalogue |
| `model.DownloadAsync()` | Télécharger le modèle Whisper |
| `model.GetAudioClientAsync()` | Obtenir l’AudioClient (pas ChatClient !) |
| `audioClient.Settings.Language = "en"` | Définir la langue de transcription (nécessaire à l’exactitude) |
| `audioClient.TranscribeAudioAsync(path)` | Transcrire un fichier audio |
| `result.Text` | Texte transcrit |


> **C# vs Python/JS :** Le SDK C# fournit un `AudioClient` intégré pour la transcription en processus via `model.GetAudioClientAsync()`, similaire au SDK JavaScript. Python utilise directement ONNX Runtime pour l'inférence sur les modèles encodeur/décodeur mis en cache.

</details>

---

### Exercice 4 - Transcrire en lot tous les échantillons Zava

Maintenant que vous avez une application de transcription fonctionnelle, transcrivez les cinq fichiers d'exemple Zava et comparez les résultats.

<details>
<summary><h3>Parcours Python</h3></summary>

L'exemple complet `python/foundry-local-whisper.py` prend déjà en charge la transcription par lot. Lorsqu'il est exécuté sans arguments, il transcrit tous les fichiers `zava-*.wav` dans `samples/audio/` :

```bash
cd python
python foundry-local-whisper.py
```

L'exemple utilise `FoundryLocalManager(alias)` pour démarrer, puis lance les sessions encodeur et décodeur ONNX pour chaque fichier.

</details>

<details>
<summary><h3>Parcours JavaScript</h3></summary>

L'exemple complet `javascript/foundry-local-whisper.mjs` prend déjà en charge la transcription par lot. Lorsqu'il est exécuté sans arguments, il transcrit tous les fichiers `zava-*.wav` dans `samples/audio/` :

```bash
cd javascript
node foundry-local-whisper.mjs
```

L'exemple utilise `FoundryLocalManager.create()` et `catalog.getModel(alias)` pour initialiser le SDK, puis utilise le `AudioClient` (avec `settings.language = "en"`) pour transcrire chaque fichier.

</details>

<details>
<summary><h3>Parcours C#</h3></summary>

L'exemple complet `csharp/WhisperTranscription.cs` prend déjà en charge la transcription par lot. Lorsqu'il est exécuté sans argument de fichier spécifique, il transcrit tous les fichiers `zava-*.wav` dans `samples/audio/` :

```bash
cd csharp
dotnet run whisper
```

L'exemple utilise `FoundryLocalManager.CreateAsync()` et le `AudioClient` du SDK (avec `Settings.Language = "en"`) pour la transcription en processus.

</details>

**À observer :** Comparez la sortie de la transcription avec le texte original dans `samples/audio/generate_samples.py`. Dans quelle mesure Whisper capture-t-il fidèlement des noms de produits comme "Zava ProGrip" et des termes techniques comme "brushless motor" ou "composite decking" ?

---

### Exercice 5 - Comprendre les principaux schémas de code

Étudiez comment la transcription Whisper diffère des complétions de chat dans les trois langages :

<details>
<summary><b>Python - Différences clés avec le chat</b></summary>

```python
# Complétion de chat (Parties 2-6) :
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transcription audio (Cette partie) :
# Utilise ONNX Runtime directement au lieu du client OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... boucle du décodeur autorégressif ...
print(tokenizer.decode(generated_tokens))
```

**Idée clé :** Les modèles de chat utilisent l'API compatible OpenAI via `manager.endpoint`. Whisper utilise le SDK pour localiser les fichiers de modèles ONNX mis en cache, puis exécute l'inférence directement avec ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Différences clés avec le chat</b></summary>

```javascript
// Complétion de chat (Parties 2-6) :
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transcription audio (Cette partie) :
// Utilise l'AudioClient intégré du SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Toujours définir la langue pour de meilleurs résultats
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Idée clé :** Les modèles de chat utilisent l'API compatible OpenAI via `manager.urls[0] + "/v1"`. La transcription Whisper utilise le `AudioClient` du SDK, obtenu via `model.createAudioClient()`. Configurez `settings.language` pour éviter des sorties brouillées dues à la détection automatique.

</details>

<details>
<summary><b>C# - Différences clés avec le chat</b></summary>

L'approche C# utilise le `AudioClient` intégré du SDK pour la transcription en processus :

**Initialisation du modèle :**

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

**Transcription :**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Idée clé :** C# utilise `FoundryLocalManager.CreateAsync()` et récupère un `AudioClient` directement — aucune configuration ONNX Runtime nécessaire. Configurez `Settings.Language` pour éviter des sorties brouillées dues à la détection automatique.

</details>

> **Résumé :** Python utilise le Foundry Local SDK pour la gestion des modèles et ONNX Runtime pour l'inférence directe sur les modèles encodeur/décodeur. JavaScript et C# utilisent tous deux le `AudioClient` intégré du SDK pour une transcription simplifiée — créez le client, définissez la langue, puis appelez `transcribe()` / `TranscribeAudioAsync()`. Configurez toujours la propriété langue sur l’AudioClient pour des résultats précis.

---

### Exercice 6 - Expérimentez

Essayez ces modifications pour approfondir votre compréhension :

1. **Essayez différents fichiers audio** - enregistrez-vous en parlant avec l'Enregistreur Vocal Windows, sauvegardez au format WAV, puis transcrivez-le

2. **Comparez les variantes du modèle** - si vous avez un GPU NVIDIA, essayez la variante CUDA :
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Comparez la vitesse de transcription avec la variante CPU.

3. **Ajoutez un formatage de sortie** - la réponse JSON peut inclure :
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Construisez une API REST** - encapsulez votre code de transcription dans un serveur web :

   | Langage | Framework | Exemple |
   |----------|-----------|---------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` avec `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` avec `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` avec `IFormFile` |

5. **Multi-tours avec transcription** - combinez Whisper avec un agent de chat de la Partie 4 : transcrivez l'audio d'abord, puis transmettez le texte à un agent pour analyse ou résumé.

---

## Référence de l’API Audio du SDK

> **JavaScript AudioClient :**
> - `model.createAudioClient()` — crée une instance `AudioClient`
> - `audioClient.settings.language` — définit la langue de transcription (par exemple `"en"`)
> - `audioClient.settings.temperature` — contrôle la randomisation (optionnel)
> - `audioClient.transcribe(filePath)` — transcrit un fichier, retourne `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — transcription en flux via callback
>
> **C# AudioClient :**
> - `await model.GetAudioClientAsync()` — crée une instance `OpenAIAudioClient`
> - `audioClient.Settings.Language` — définit la langue de transcription (par exemple `"en"`)
> - `audioClient.Settings.Temperature` — contrôle la randomisation (optionnel)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transcrit un fichier, retourne un objet avec `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — retourne `IAsyncEnumerable` de segments de transcription

> **Astuce :** Configurez toujours la propriété language avant la transcription. Sinon, le modèle Whisper tente une détection automatique qui peut produire une sortie brouillée (un caractère de remplacement unique au lieu de texte).

---

## Comparaison : Modèles de chat vs Whisper

| Aspect | Modèles de chat (Parties 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|-------------------------------|------------------|-------------------|
| **Type de tâche** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Entrée** | Messages texte (JSON) | Fichiers audio (WAV/MP3/M4A) | Fichiers audio (WAV/MP3/M4A) |
| **Sortie** | Texte généré (streamé) | Texte transcrit (complet) | Texte transcrit (complet) |
| **Package SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Méthode API** | `client.chat.completions.create()` | ONNX Runtime direct | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Réglage de langue** | N/A | Jetons d’invite du décodeur | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Oui | Non | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Avantage confidentialité** | Code/données restent locales | Données audio restent locales | Données audio restent locales |

---

## Points clés à retenir

| Concept | Ce que vous avez appris |
|---------|------------------------|
| **Whisper sur appareil** | La reconnaissance vocale s'exécute entièrement localement, idéal pour transcrire les appels client Zava et avis produits sur appareil |
| **SDK AudioClient** | Les SDK JavaScript et C# fournissent un `AudioClient` intégré qui gère toute la pipeline de transcription en un seul appel |
| **Réglage de langue** | Configurez toujours la langue sur l’AudioClient (ex. `"en"`) — sans cela la détection automatique peut produire une sortie brouillée |
| **Python** | Utilise `foundry-local-sdk` pour la gestion modèles + `onnxruntime` + `transformers` + `librosa` pour inférence ONNX directe |
| **JavaScript** | Utilise `foundry-local-sdk` avec `model.createAudioClient()` — configurez `settings.language`, puis appelez `transcribe()` |
| **C#** | Utilise `Microsoft.AI.Foundry.Local` avec `model.GetAudioClientAsync()` — configurez `Settings.Language`, puis appelez `TranscribeAudioAsync()` |
| **Support streaming** | Les SDK JS et C# offrent aussi `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` pour sortie par segments |
| **Optimisé CPU** | La variante CPU (3,05 Go) fonctionne sur n'importe quel appareil Windows sans GPU |
| **Privacy-first** | Parfait pour garder les interactions client Zava et les données produit propriétaires sur appareil |

---

## Ressources

| Ressource | Lien |
|----------|------|
| Docs Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Référence SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Modèle OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Étape suivante

Continuez vers [Partie 10 : Utiliser des modèles personnalisés ou Hugging Face](part10-custom-models.md) pour compiler vos propres modèles depuis Hugging Face et les exécuter via Foundry Local.