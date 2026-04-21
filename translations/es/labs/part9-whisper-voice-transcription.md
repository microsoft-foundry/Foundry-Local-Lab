![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 9: Transcripción de Voz con Whisper y Foundry Local

> **Objetivo:** Usar el modelo OpenAI Whisper ejecutándose localmente a través de Foundry Local para transcribir archivos de audio - completamente en el dispositivo, sin necesidad de la nube.

## Resumen

Foundry Local no es solo para generación de texto; también soporta modelos de **conversión de voz a texto**. En este laboratorio usarás el modelo **OpenAI Whisper Medium** para transcribir archivos de audio totalmente en tu máquina. Esto es ideal para escenarios como transcribir llamadas de servicio al cliente de Zava, grabaciones de reseñas de productos o sesiones de planificación de talleres donde los datos de audio nunca deben salir de tu dispositivo.


---

## Objetivos de Aprendizaje

Al final de este laboratorio serás capaz de:

- Entender el modelo de voz a texto Whisper y sus capacidades
- Descargar y ejecutar el modelo Whisper usando Foundry Local
- Transcribir archivos de audio usando el SDK de Foundry Local en Python, JavaScript y C#
- Construir un servicio de transcripción simple que funcione totalmente en el dispositivo
- Comprender las diferencias entre modelos de chat/texto y modelos de audio en Foundry Local

---

## Requisitos Previos

| Requisito | Detalles |
|-------------|---------|
| **Foundry Local CLI** | Versión **0.8.101 o superior** (los modelos Whisper están disponibles desde la v0.8.101 en adelante) |
| **SO** | Windows 10/11 (x64 o ARM64) |
| **Entorno de ejecución** | **Python 3.9+** y/o **Node.js 18+** y/o **.NET 9 SDK** ([Descargar .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Completado** | [Parte 1: Primeros Pasos](part1-getting-started.md), [Parte 2: Profundizando en el SDK de Foundry Local](part2-foundry-local-sdk.md), y [Parte 3: SDKs y APIs](part3-sdk-and-apis.md) |

> **Nota:** Los modelos Whisper deben descargarse mediante el **SDK** (no con el CLI). El CLI no soporta el endpoint de transcripción de audio. Verifica tu versión con:
> ```bash
> foundry --version
> ```

---

## Concepto: Cómo Funciona Whisper con Foundry Local

El modelo OpenAI Whisper es un modelo general de reconocimiento de voz entrenado con un gran conjunto de datos de audio diverso. Cuando se ejecuta a través de Foundry Local:

- El modelo se ejecuta **completamente en tu CPU** - no se requiere GPU
- El audio nunca sale de tu dispositivo - **privacidad total**
- El SDK de Foundry Local gestiona la descarga del modelo y el manejo del caché
- **JavaScript y C#** proporcionan un `AudioClient` incorporado en el SDK que maneja toda la tubería de transcripción — no se requiere configuración manual de ONNX
- **Python** usa el SDK para la gestión del modelo y ONNX Runtime para inferencia directa contra los modelos ONNX del codificador/decodificador

### Cómo Funciona la Tubería (JavaScript y C#) — SDK AudioClient

1. El **SDK Foundry Local** descarga y guarda en caché el modelo Whisper
2. `model.createAudioClient()` (JS) o `model.GetAudioClientAsync()` (C#) crea un `AudioClient`
3. `audioClient.transcribe(path)` (JS) o `audioClient.TranscribeAudioAsync(path)` (C#) realiza toda la tubería internamente — preprocesamiento de audio, codificador, decodificador y decodificación de tokens
4. El `AudioClient` expone una propiedad `settings.language` (configurada en `"en"` para inglés) para guiar una transcripción precisa

### Cómo Funciona la Tubería (Python) — ONNX Runtime

1. El **SDK Foundry Local** descarga y guarda en caché los archivos del modelo ONNX de Whisper
2. El **preprocesamiento de audio** convierte audio WAV en un espectrograma mel (80 bins mel x 3000 frames)
3. El **codificador** procesa el espectrograma mel y produce estados ocultos más tensores key/value de atención cruzada
4. El **decodificador** funciona autoregresivamente, generando un token a la vez hasta producir un token de fin de texto
5. El **tokenizador** decodifica los IDs de tokens de salida a texto legible

### Variantes del Modelo Whisper

| Alias | ID del Modelo | Dispositivo | Tamaño | Descripción |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Acelerado por GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Optimizado para CPU (recomendado para la mayoría de dispositivos) |

> **Nota:** A diferencia de los modelos de chat que aparecen listados por defecto, los modelos Whisper están categorizados bajo la tarea `automatic-speech-recognition`. Usa `foundry model info whisper-medium` para ver detalles.

---

## Ejercicios del Laboratorio

### Ejercicio 0 - Obtener Archivos de Audio de Ejemplo

Este laboratorio incluye archivos WAV preconstruidos basados en escenarios de productos Zava DIY. Généralos con el script incluido:

```bash
# Desde la raíz del repositorio - crea y activa primero un .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Esto crea seis archivos WAV en `samples/audio/`:

| Archivo | Escenario |
|------|----------|
| `zava-customer-inquiry.wav` | Cliente preguntando sobre el **Taladro Inalámbrico Zava ProGrip** |
| `zava-product-review.wav` | Cliente reseñando la **Pintura Interior UltraSmooth de Zava** |
| `zava-support-call.wav` | Llamada de soporte sobre el **Baúl de Herramientas Zava TitanLock** |
| `zava-project-planning.wav` | Usuario planificando una terraza con el **Deck compuesto Zava EcoBoard** |
| `zava-workshop-setup.wav` | Recorrido de un taller usando **los cinco productos Zava** |
| `zava-full-project-walkthrough.wav` | Recorrido extendido de renovación de garaje usando **todos los productos Zava** (~4 min, para pruebas de audio largo) |

> **Consejo:** También puedes usar tus propios archivos WAV/MP3/M4A, o grabarte con el Grabador de Voz de Windows.

---

### Ejercicio 1 - Descargar el Modelo Whisper Usando el SDK

Debido a incompatibilidades del CLI con modelos Whisper en versiones recientes de Foundry Local, usa el **SDK** para descargar y cargar el modelo. Elige tu lenguaje:

<details>
<summary><b>🐍 Python</b></summary>

**Instala el SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Iniciar el servicio
manager = FoundryLocalManager()
manager.start_service()

# Verificar información del catálogo
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Verificar si ya está en caché
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Cargar el modelo en la memoria
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Guárdalo como `download_whisper.py` y ejecútalo:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Instala el SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Crear el gestor e iniciar el servicio
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Obtener el modelo del catálogo
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

// Cargar el modelo en la memoria
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Guárdalo como `download-whisper.mjs` y ejecútalo:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Instala el SDK:**
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

> **¿Por qué usar el SDK en lugar del CLI?** El CLI de Foundry Local no soporta descargar o servir modelos Whisper directamente. El SDK proporciona una manera confiable de descargar y gestionar modelos de audio programáticamente. Los SDKs de JavaScript y C# incluyen un `AudioClient` integrado que maneja toda la tubería de transcripción. Python usa ONNX Runtime para inferencia directa con los archivos de modelo en caché.

---

### Ejercicio 2 - Entender el SDK Whisper

La transcripción con Whisper usa diferentes enfoques según el lenguaje. **JavaScript y C#** proporcionan un `AudioClient` incorporado en el SDK de Foundry Local que maneja la tubería completa (preprocesamiento de audio, codificador, decodificador y decodificación de tokens) en una sola llamada. **Python** usa el SDK de Foundry Local para la gestión del modelo y ONNX Runtime para inferencia directa contra los modelos ONNX del codificador/decodificador.

| Componente | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **Paquetes SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Gestión del modelo** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catálogo |
| **Extracción de características** | `WhisperFeatureExtractor` + `librosa` | Manejada por `AudioClient` del SDK | Manejada por `AudioClient` del SDK |
| **Inferencia** | `ort.InferenceSession` (codificador + decodificador) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Decodificación de tokens** | `WhisperTokenizer` | Manejada por `AudioClient` del SDK | Manejada por `AudioClient` del SDK |
| **Configuración de idioma** | Se establece con `forced_ids` en tokens del decodificador | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Entrada** | Ruta a archivo WAV | Ruta a archivo WAV | Ruta a archivo WAV |
| **Salida** | Cadena de texto decodificado | `result.text` | `result.Text` |

> **Importante:** Siempre establece el idioma en el `AudioClient` (por ejemplo `"en"` para inglés). Sin una configuración explícita del idioma, el modelo puede producir resultados ininteligibles al intentar detectar el idioma automáticamente.

> **Patrones del SDK:** Python usa `FoundryLocalManager(alias)` para iniciar, luego `get_cache_location()` para encontrar los archivos modelo ONNX. JavaScript y C# usan el `AudioClient` incorporado del SDK — obtenido vía `model.createAudioClient()` (JS) o `model.GetAudioClientAsync()` (C#) — que maneja toda la tubería de transcripción. Consulta [Parte 2: Profundizando en el SDK de Foundry Local](part2-foundry-local-sdk.md) para detalles completos.

---

### Ejercicio 3 - Construir una Aplicación Simple de Transcripción

Elige tu lenguaje y construye una aplicación mínima que transcriba un archivo de audio.

> **Formatos de audio soportados:** WAV, MP3, M4A. Para mejores resultados, usa archivos WAV con frecuencia de muestreo 16kHz.

<details>
<summary><h3>Pista Python</h3></summary>

#### Configuración

```bash
cd python
python -m venv venv

# Activar el entorno virtual:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Código de Transcripción

Crea un archivo `foundry-local-whisper.py`:

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

# Paso 1: Bootstrap - inicia el servicio, descarga y carga el modelo
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Construir la ruta a los archivos ONNX en caché
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Paso 2: Cargar sesiones ONNX y extractor de características
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

# Paso 3: Extraer características del espectrograma mel
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Paso 4: Ejecutar el codificador
enc_out = encoder.run(None, {"audio_features": input_features})
# La primera salida son los estados ocultos; el resto son pares KV de atención cruzada
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Paso 5: Decodificación autorregresiva
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcribir, sin marcas de tiempo
input_ids = np.array([initial_tokens], dtype=np.int32)

# Caché KV de auto-atención vacío
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

    if next_token == 50257:  # fin del texto
        break
    generated.append(next_token)

    # Actualizar caché KV de auto-atención
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Ejecútalo

```bash
# Transcribir un escenario de producto Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# O probar otros:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Puntos Clave de Python

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager(alias)` | Inicializar: iniciar servicio, descargar y cargar modelo |
| `manager.get_cache_location()` | Obtener ruta a los archivos modelo ONNX en caché |
| `WhisperFeatureExtractor.from_pretrained()` | Cargar extractor de características de espectrograma mel |
| `ort.InferenceSession()` | Crear sesiones de ONNX Runtime para codificador y decodificador |
| `tokenizer.decode()` | Convertir IDs de tokens de salida a texto |

</details>

<details>
<summary><h3>Pista JavaScript</h3></summary>

#### Configuración

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Código de Transcripción

Crea un archivo `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Paso 1: Inicializar - crear el gestor, iniciar el servicio y cargar el modelo
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

// Paso 2: Crear un cliente de audio y transcribir
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Limpieza
await model.unload();
```

> **Nota:** El SDK Foundry Local provee un `AudioClient` incorporado a través de `model.createAudioClient()` que maneja toda la tubería de inferencia ONNX internamente — no se necesita importar `onnxruntime-node`. Siempre configura `audioClient.settings.language = "en"` para asegurar una transcripción precisa en inglés.

#### Ejecútalo

```bash
# Transcribir un escenario de producto Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# O probar otros:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Puntos Clave de JavaScript

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager.create({ appName })` | Crear el singleton del administrador |
| `await catalog.getModel(alias)` | Obtener un modelo del catálogo |
| `model.download()` / `model.load()` | Descargar y cargar el modelo Whisper |
| `model.createAudioClient()` | Crear un cliente de audio para transcripción |
| `audioClient.settings.language = "en"` | Ajustar idioma de transcripción (requerido para salida precisa) |
| `audioClient.transcribe(path)` | Transcribir un archivo de audio, devuelve `{ text, duration }` |

</details>

<details>
<summary><h3>Pista C#</h3></summary>

#### Configuración

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Nota:** La pista C# usa el paquete `Microsoft.AI.Foundry.Local` que proporciona un `AudioClient` incorporado vía `model.GetAudioClientAsync()`. Esto maneja toda la tubería de transcripción en proceso — no se requiere configuración separada de ONNX Runtime.

#### Código de Transcripción

Reemplaza el contenido de `Program.cs`:

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

#### Ejecutarlo

```bash
# Transcribir un escenario de producto de Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# O prueba otros:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Puntos Clave de C#

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager.CreateAsync(config)` | Inicializar Foundry Local con configuración |
| `catalog.GetModelAsync(alias)` | Obtener modelo del catálogo |
| `model.DownloadAsync()` | Descargar el modelo Whisper |
| `model.GetAudioClientAsync()` | Obtener el AudioClient (¡no ChatClient!) |
| `audioClient.Settings.Language = "en"` | Ajustar idioma de transcripción (requerido para salida precisa) |
| `audioClient.TranscribeAudioAsync(path)` | Transcribir un archivo de audio |
| `result.Text` | Texto transcrito |


> **C# vs Python/JS:** El SDK de C# proporciona un `AudioClient` incorporado para la transcripción en proceso mediante `model.GetAudioClientAsync()`, similar al SDK de JavaScript. Python utiliza ONNX Runtime directamente para la inferencia con los modelos de codificador/decodificador en caché.

</details>

---

### Ejercicio 4 - Transcribir por lotes todas las muestras de Zava

Ahora que tienes una aplicación de transcripción funcionando, transcribe los cinco archivos de muestra de Zava y compara los resultados.

<details>
<summary><h3>Pista Python</h3></summary>

La muestra completa `python/foundry-local-whisper.py` ya admite transcripción por lotes. Cuando se ejecuta sin argumentos, transcribe todos los archivos `zava-*.wav` en `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

La muestra utiliza `FoundryLocalManager(alias)` para iniciar, luego ejecuta las sesiones ONNX del codificador y decodificador para cada archivo.

</details>

<details>
<summary><h3>Pista JavaScript</h3></summary>

La muestra completa `javascript/foundry-local-whisper.mjs` ya admite transcripción por lotes. Cuando se ejecuta sin argumentos, transcribe todos los archivos `zava-*.wav` en `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

La muestra usa `FoundryLocalManager.create()` y `catalog.getModel(alias)` para inicializar el SDK, luego usa el `AudioClient` (con `settings.language = "en"`) para transcribir cada archivo.

</details>

<details>
<summary><h3>Pista C#</h3></summary>

La muestra completa `csharp/WhisperTranscription.cs` ya admite transcripción por lotes. Cuando se ejecuta sin un argumento de archivo específico, transcribe todos los archivos `zava-*.wav` en `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

La muestra usa `FoundryLocalManager.CreateAsync()` y el `AudioClient` del SDK (con `Settings.Language = "en"`) para la transcripción en proceso.

</details>

**Qué observar:** Compara la salida de la transcripción con el texto original en `samples/audio/generate_samples.py`. ¿Qué tan bien captura Whisper nombres de productos como "Zava ProGrip" y términos técnicos como "brushless motor" o "composite decking"?

---

### Ejercicio 5 - Entender los patrones clave del código

Estudia cómo difiere la transcripción de Whisper de las completaciones de chat en los tres lenguajes:

<details>
<summary><b>Python - Diferencias clave respecto al chat</b></summary>

```python
# Finalización de chat (Partes 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transcripción de audio (Esta Parte):
# Utiliza directamente ONNX Runtime en lugar del cliente OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... bucle del decodificador autorregresivo ...
print(tokenizer.decode(generated_tokens))
```

**Conclusión clave:** Los modelos de chat usan la API compatible con OpenAI mediante `manager.endpoint`. Whisper usa el SDK para localizar los archivos del modelo ONNX en caché, luego ejecuta la inferencia directamente con ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Diferencias clave respecto al chat</b></summary>

```javascript
// Finalización de chat (Partes 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transcripción de audio (Esta Parte):
// Usa el AudioClient incorporado del SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Siempre establezca el idioma para obtener mejores resultados
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Conclusión clave:** Los modelos de chat usan la API compatible con OpenAI mediante `manager.urls[0] + "/v1"`. La transcripción de Whisper usa el `AudioClient` del SDK, obtenido con `model.createAudioClient()`. Establece `settings.language` para evitar resultados distorsionados por la autodetección.

</details>

<details>
<summary><b>C# - Diferencias clave respecto al chat</b></summary>

El enfoque de C# utiliza el `AudioClient` incorporado en el SDK para transcripción en proceso:

**Inicialización del modelo:**

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

**Transcripción:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Conclusión clave:** C# usa `FoundryLocalManager.CreateAsync()` y obtiene un `AudioClient` directamente — no se necesita configurar ONNX Runtime. Ajusta `Settings.Language` para evitar resultados distorsionados por la autodetección.

</details>

> **Resumen:** Python usa Foundry Local SDK para la gestión del modelo y ONNX Runtime para inferencia directa con los modelos de codificador/decodificador. JavaScript y C# usan el `AudioClient` incorporado del SDK para una transcripción simplificada: crea el cliente, establece el idioma y llama a `transcribe()` / `TranscribeAudioAsync()`. Siempre configura la propiedad de idioma en el AudioClient para obtener resultados precisos.

---

### Ejercicio 6 - Experimenta

Prueba estas modificaciones para profundizar tu comprensión:

1. **Prueba diferentes archivos de audio** - grábate hablando con Windows Voice Recorder, guarda en WAV y transcríbelo

2. **Compara variantes del modelo** - si tienes una GPU NVIDIA, prueba la variante CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Compara la velocidad de transcripción respecto a la variante CPU.

3. **Agrega formato de salida** - la respuesta JSON puede incluir:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Crea una API REST** - envuelve tu código de transcripción en un servidor web:

   | Lenguaje | Framework | Ejemplo |
   |----------|-----------|---------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` con `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` con `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` con `IFormFile` |

5. **Multi-turn con transcripción** - combina Whisper con un agente de chat de la Parte 4: transcribe audio primero y luego pasa el texto a un agente para análisis o resumen.

---

## Referencia de la API de Audio del SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — crea una instancia de `AudioClient`
> - `audioClient.settings.language` — establece el idioma de la transcripción (ej. `"en"`)
> - `audioClient.settings.temperature` — controla la aleatoriedad (opcional)
> - `audioClient.transcribe(filePath)` — transcribe un archivo, devuelve `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — transmite fragmentos de transcripción vía callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — crea una instancia de `OpenAIAudioClient`
> - `audioClient.Settings.Language` — establece el idioma de la transcripción (ej. `"en"`)
> - `audioClient.Settings.Temperature` — controla la aleatoriedad (opcional)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transcribe un archivo, devuelve objeto con `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — devuelve `IAsyncEnumerable` de fragmentos de transcripción

> **Consejo:** Siempre establece la propiedad de idioma antes de transcribir. Sin ella, el modelo Whisper intenta autodetección, lo que puede producir una salida distorsionada (un solo carácter de reemplazo en lugar de texto).

---

## Comparación: Modelos de Chat vs. Whisper

| Aspecto | Modelos de Chat (Partes 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------------|------------------|-------------------|
| **Tipo de tarea** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Entrada** | Mensajes de texto (JSON) | Archivos de audio (WAV/MP3/M4A) | Archivos de audio (WAV/MP3/M4A) |
| **Salida** | Texto generado (transmitido) | Texto transcrito (completo) | Texto transcrito (completo) |
| **Paquete SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Método API** | `client.chat.completions.create()` | ONNX Runtime directo | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Configuración de idioma** | N/A | Tokens del prompt del decodificador | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Sí | No | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Beneficio de privacidad** | Código/datos permanecen local | Datos de audio permanecen local | Datos de audio permanecen local |

---

## Conclusiones clave

| Concepto | Lo que aprendiste |
|----------|-------------------|
| **Whisper en dispositivo** | El reconocimiento de voz a texto se ejecuta completamente localmente, ideal para transcribir llamadas de clientes y reseñas de productos de Zava en dispositivo |
| **SDK AudioClient** | Los SDKs de JavaScript y C# proporcionan un `AudioClient` incorporado que maneja toda la pipeline de transcripción en una sola llamada |
| **Configuración de idioma** | Siempre establece el idioma del AudioClient (ej. `"en"`) — sin esto, la autodetección puede producir salidas distorsionadas |
| **Python** | Usa `foundry-local-sdk` para gestión del modelo + `onnxruntime` + `transformers` + `librosa` para inferencia ONNX directa |
| **JavaScript** | Usa `foundry-local-sdk` con `model.createAudioClient()` — establece `settings.language` y luego llama a `transcribe()` |
| **C#** | Usa `Microsoft.AI.Foundry.Local` con `model.GetAudioClientAsync()` — establece `Settings.Language` y luego llama a `TranscribeAudioAsync()` |
| **Soporte streaming** | Los SDKs JS y C# también ofrecen `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` para salidas por fragmentos |
| **Optimizado para CPU** | La variante CPU (3.05 GB) funciona en cualquier dispositivo Windows sin GPU |
| **Privacidad ante todo** | Perfecto para mantener las interacciones con clientes y datos de productos propietarios de Zava en el dispositivo |

---

## Recursos

| Recurso | Enlace |
|---------|--------|
| Documentación Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referencia SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Modelo OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Sitio web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Próximo paso

Continúa en [Parte 10: Usar modelos personalizados o de Hugging Face](part10-custom-models.md) para compilar tus propios modelos de Hugging Face y ejecutarlos con Foundry Local.