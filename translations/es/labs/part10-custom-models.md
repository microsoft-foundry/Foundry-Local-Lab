![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 10: Uso de modelos personalizados o de Hugging Face con Foundry Local

> **Objetivo:** Compilar un modelo de Hugging Face en el formato ONNX optimizado que requiere Foundry Local, configurarlo con una plantilla de chat, agregarlo a la caché local y ejecutar inferencia contra él usando la CLI, API REST y el SDK de OpenAI.

## Visión general

Foundry Local se entrega con un catálogo seleccionado de modelos precompilados, pero no estás limitado a esa lista. Cualquier modelo de lenguaje basado en transformadores disponible en [Hugging Face](https://huggingface.co/) (o almacenado localmente en formato PyTorch / Safetensors) puede compilarse en un modelo ONNX optimizado y servirse a través de Foundry Local.

La tubería de compilación utiliza el **ONNX Runtime GenAI Model Builder**, una herramienta de línea de comandos incluida en el paquete `onnxruntime-genai`. El constructor del modelo se encarga del trabajo pesado: descargar los pesos fuente, convertirlos a formato ONNX, aplicar cuantización (int4, fp16, bf16) y generar los archivos de configuración (incluida la plantilla de chat y el tokenizador) que Foundry Local espera.

En este laboratorio compilarás **Qwen/Qwen3-0.6B** de Hugging Face, lo registrarás con Foundry Local y chatearás con él completamente en tu dispositivo.

---

## Objetivos de aprendizaje

Al final de este laboratorio podrás:

- Explicar por qué la compilación de modelos personalizados es útil y cuándo podrías necesitarla
- Instalar el constructor de modelos ONNX Runtime GenAI
- Compilar un modelo de Hugging Face a formato ONNX optimizado con un solo comando
- Entender los parámetros clave de compilación (proveedor de ejecución, precisión)
- Crear el archivo de configuración de plantilla de chat `inference_model.json`
- Agregar un modelo compilado a la caché de Foundry Local
- Ejecutar inferencia contra el modelo personalizado usando la CLI, API REST y SDK de OpenAI

---

## Prerrequisitos

| Requisito | Detalles |
|-------------|---------|
| **Foundry Local CLI** | Instalado y en tu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Python 3.10+** | Necesario para el constructor de modelos ONNX Runtime GenAI |
| **pip** | Gestor de paquetes de Python |
| **Espacio en disco** | Al menos 5 GB libres para los archivos de modelo fuente y compilado |
| **Cuenta de Hugging Face** | Algunos modelos requieren aceptar una licencia antes de descargarlos. Qwen3-0.6B utiliza la licencia Apache 2.0 y está disponible libremente. |

---

## Configuración del entorno

La compilación del modelo requiere varios paquetes de Python grandes (PyTorch, ONNX Runtime GenAI, Transformers). Crea un entorno virtual dedicado para que estos no interfieran con tu Python del sistema u otros proyectos.

```bash
# Desde la raíz del repositorio
python -m venv .venv
```

Activa el entorno:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Actualiza pip para evitar problemas de resolución de dependencias:

```bash
python -m pip install --upgrade pip
```

> **Consejo:** Si ya tienes un `.venv` de laboratorios anteriores, puedes reutilizarlo. Solo asegúrate de que esté activado antes de continuar.

---

## Concepto: La tubería de compilación

Foundry Local requiere modelos en formato ONNX con configuración ONNX Runtime GenAI. La mayoría de modelos de código abierto en Hugging Face se distribuyen como pesos PyTorch o Safetensors, por lo que se necesita un paso de conversión.

![Tubería de compilación de modelo personalizado](../../../images/custom-model-pipeline.svg)

### ¿Qué hace el constructor del modelo?

1. **Descarga** el modelo fuente desde Hugging Face (o lo lee desde una ruta local).
2. **Convierte** los pesos PyTorch / Safetensors a formato ONNX.
3. **Cuantiza** el modelo a una precisión menor (por ejemplo, int4) para reducir uso de memoria y mejorar rendimiento.
4. **Genera** la configuración ONNX Runtime GenAI (`genai_config.json`), la plantilla de chat (`chat_template.jinja`) y todos los archivos del tokenizador para que Foundry Local pueda cargar y servir el modelo.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Podrías encontrar referencias a **Microsoft Olive** como herramienta alternativa para optimización de modelos. Ambas herramientas pueden producir modelos ONNX, pero tienen propósitos y compensaciones diferentes:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paquete** | `onnxruntime-genai` | `olive-ai` |
| **Propósito principal** | Convertir y cuantizar modelos de IA generativa para inferencia con ONNX Runtime GenAI | Marco de optimización de modelos de extremo a extremo soportando muchos backends y hardware |
| **Facilidad de uso** | Comando único — conversión + cuantización en un paso | Basado en flujo de trabajo — tuberías configurables multi-paso con YAML/JSON |
| **Formato de salida** | Formato ONNX Runtime GenAI (listo para Foundry Local) | ONNX genérico, ONNX Runtime GenAI u otros formatos según flujo de trabajo |
| **Hardware objetivo** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN y más |
| **Opciones de cuantización** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, además optimizaciones de grafo y afinación capa a capa |
| **Alcance del modelo** | Modelos de IA generativa (LLMs, SLMs) | Cualquier modelo convertible a ONNX (visión, PLN, audio, multimodal) |
| **Mejor para** | Compilación rápida de un solo modelo para inferencia local | Tuberías de producción que necesitan control fino de optimización |
| **Dependencias** | Moderado (PyTorch, Transformers, ONNX Runtime) | Mayor (agrega framework Olive, extras opcionales según flujo de trabajo) |
| **Integración con Foundry Local** | Directa — la salida es inmediatamente compatible | Requiere la bandera `--use_ort_genai` y configuración adicional |

> **Por qué este laboratorio usa el Model Builder:** Para la tarea de compilar un solo modelo de Hugging Face y registrarlo con Foundry Local, el Model Builder es el camino más sencillo y fiable. Produce el formato exacto que Foundry Local espera en un solo comando. Si necesitas luego funciones avanzadas de optimización —como cuantización consciente de precisión, cirugía de grafos, o afinación multi-paso— Olive es una opción poderosa para explorar. Consulta la [documentación de Microsoft Olive](https://microsoft.github.io/Olive/) para más detalles.

---

## Ejercicios del laboratorio

### Ejercicio 1: Instalar el ONNX Runtime GenAI Model Builder

Instala el paquete ONNX Runtime GenAI, que incluye la herramienta de construcción de modelos:

```bash
pip install onnxruntime-genai
```

Verifica la instalación comprobando que el constructor esté disponible:

```bash
python -m onnxruntime_genai.models.builder --help
```

Deberías ver la ayuda del comando listando parámetros como `-m` (nombre del modelo), `-o` (ruta de salida), `-p` (precisión) y `-e` (proveedor de ejecución).

> **Nota:** El constructor depende de PyTorch, Transformers y otros paquetes. La instalación puede tardar unos minutos.

---

### Ejercicio 2: Compilar Qwen3-0.6B para CPU

Ejecuta el siguiente comando para descargar el modelo Qwen3-0.6B de Hugging Face y compilarlo para inferencia en CPU con cuantización int4:

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell):**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### Qué hace cada parámetro

| Parámetro | Propósito | Valor usado |
|-----------|-----------|-------------|
| `-m` | ID del modelo en Hugging Face o ruta local | `Qwen/Qwen3-0.6B` |
| `-o` | Directorio donde se guardará el modelo ONNX compilado | `models/qwen3` |
| `-p` | Precisión de cuantización aplicada durante la compilación | `int4` |
| `-e` | Proveedor de ejecución ONNX Runtime (hardware objetivo) | `cpu` |
| `--extra_options hf_token=false` | Omite autenticación Hugging Face (adecuado para modelos públicos) | `hf_token=false` |

> **¿Cuánto tarda esto?** El tiempo de compilación depende de tu hardware y tamaño del modelo. Para Qwen3-0.6B con cuantización int4 en CPU moderno, espera aproximadamente entre 5 y 15 minutos. Modelos más grandes tardan proporcionalmente más.

Una vez que el comando termine deberías ver un directorio `models/qwen3` con los archivos compilados. Verifica la salida:

```bash
ls models/qwen3
```

Deberías ver archivos tales como:
- `model.onnx` y `model.onnx.data` — los pesos compilados del modelo
- `genai_config.json` — configuración ONNX Runtime GenAI
- `chat_template.jinja` — plantilla de chat del modelo (generada automáticamente)
- `tokenizer.json`, `tokenizer_config.json` — archivos del tokenizador
- Otros archivos de vocabulario y configuración

---

### Ejercicio 3: Compilar para GPU (Opcional)

Si tienes una GPU NVIDIA con soporte CUDA, puedes compilar una variante optimizada para GPU para inferencia más rápida:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Nota:** La compilación para GPU requiere `onnxruntime-gpu` y una instalación CUDA funcional. Si no dispones de estos, el constructor mostrará un error. Puedes saltarte este ejercicio y continuar con la variante CPU.

#### Referencia de compilación específica por hardware

| Objetivo | Proveedor de ejecución (`-e`) | Precisión recomendada (`-p`) |
|----------|-------------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| GPU NVIDIA | `cuda` | `fp16` o `int4` |
| DirectML (GPU en Windows) | `dml` | `fp16` o `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Compensaciones de precisión

| Precisión | Tamaño | Velocidad | Calidad |
|-----------|--------|-----------|---------|
| `fp32` | Más grande | Más lenta | Mayor precisión |
| `fp16` | Grande | Rápida (GPU) | Muy buena precisión |
| `int8` | Pequeña | Rápida | Pérdida leve de precisión |
| `int4` | Más pequeña | Más rápida | Pérdida moderada de precisión |

Para desarrollo local la mejor relación velocidad/recursos suele ser `int4` en CPU. Para salida con calidad de producción se recomienda `fp16` en GPU CUDA.

---

### Ejercicio 4: Crear la configuración de la plantilla de chat

El constructor del modelo genera automáticamente un archivo `chat_template.jinja` y un archivo `genai_config.json` en el directorio de salida. Sin embargo, Foundry Local también necesita un archivo `inference_model.json` para entender cómo formatear los prompts para tu modelo. Este archivo define el nombre del modelo y la plantilla de prompt que envuelve los mensajes del usuario con los tokens especiales correctos.

#### Paso 1: Inspeccionar la salida compilada

Lista el contenido del directorio del modelo compilado:

```bash
ls models/qwen3
```

Deberías ver archivos como:
- `model.onnx` y `model.onnx.data` — pesos compilados del modelo
- `genai_config.json` — configuración ONNX Runtime GenAI (generado automáticamente)
- `chat_template.jinja` — plantilla de chat del modelo (generada automáticamente)
- `tokenizer.json`, `tokenizer_config.json` — archivos del tokenizador
- Otros archivos varios de configuración y vocabulario

#### Paso 2: Generar el archivo inference_model.json

El archivo `inference_model.json` le indica a Foundry Local cómo formatear los prompts. Crea un script Python llamado `generate_chat_template.py` **en la raíz del repositorio** (el mismo directorio que contiene la carpeta `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Construya una conversación mínima para extraer la plantilla de chat
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# Construya la estructura inference_model.json
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

Ejecuta el script desde la raíz del repositorio:

```bash
python generate_chat_template.py
```

> **Nota:** El paquete `transformers` ya fue instalado como dependencia de `onnxruntime-genai`. Si ves un `ImportError`, ejecuta primero `pip install transformers`.

El script produce un archivo `inference_model.json` dentro del directorio `models/qwen3`. Este archivo indica a Foundry Local cómo envolver la entrada del usuario con los tokens especiales correctos para Qwen3.

> **Importante:** El campo `"Name"` en `inference_model.json` (establecido a `qwen3-0.6b` en este script) es el **alias del modelo** que usarás en todos los comandos y llamadas API siguientes. Si cambias este nombre, actualiza el nombre del modelo en los Ejercicios 6–10 en consecuencia.

#### Paso 3: Verificar la configuración

Abre `models/qwen3/inference_model.json` y confirma que contiene un campo `Name` y un objeto `PromptTemplate` con las claves `assistant` y `prompt`. La plantilla de prompt debe incluir tokens especiales como `<|im_start|>` y `<|im_end|>` (los tokens exactos dependen de la plantilla de chat del modelo).

> **Alternativa manual:** Si prefieres no ejecutar el script, puedes crear el archivo manualmente. El requisito clave es que el campo `prompt` contenga la plantilla completa de chat del modelo con `{Content}` como marcador para el mensaje del usuario.

---

### Ejercicio 5: Verificar la estructura del directorio del modelo
El compilador de modelos coloca todos los archivos compilados directamente en el directorio de salida que especificaste. Verifica que la estructura final sea correcta:

```bash
ls models/qwen3
```

El directorio debe contener los siguientes archivos:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **Nota:** A diferencia de algunas otras herramientas de compilación, el compilador de modelos no crea subdirectorios anidados. Todos los archivos están directamente en la carpeta de salida, que es exactamente lo que Foundry Local espera.

---

### Ejercicio 6: Añadir el Modelo a la Caché de Foundry Local

Indica a Foundry Local dónde encontrar tu modelo compilado añadiendo el directorio a su caché:

```bash
foundry cache cd models/qwen3
```

Verifica que el modelo aparezca en la caché:

```bash
foundry cache ls
```

Deberías ver tu modelo personalizado listado junto con modelos previamente almacenados en caché (como `phi-3.5-mini` o `phi-4-mini`).

---

### Ejercicio 7: Ejecutar el Modelo Personalizado con la CLI

Inicia una sesión de chat interactiva con tu modelo recién compilado (el alias `qwen3-0.6b` proviene del campo `Name` que configuraste en `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

La bandera `--verbose` muestra información diagnóstica adicional, útil al probar un modelo personalizado por primera vez. Si el modelo se carga correctamente, verás un prompt interactivo. Prueba algunos mensajes:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Escribe `exit` o presiona `Ctrl+C` para terminar la sesión.

> **Solución de problemas:** Si el modelo no se carga, comprueba lo siguiente:
> - El archivo `genai_config.json` fue generado por el compilador de modelos.
> - El archivo `inference_model.json` existe y es un JSON válido.
> - Los archivos del modelo ONNX están en el directorio correcto.
> - Tienes suficiente RAM disponible (Qwen3-0.6B int4 necesita aproximadamente 1 GB).
> - Qwen3 es un modelo de razonamiento que produce etiquetas `<think>`. Si ves `<think>...</think>` al inicio de las respuestas, este es un comportamiento normal. La plantilla del prompt en `inference_model.json` puede ajustarse para suprimir la salida del pensamiento.

---

### Ejercicio 8: Consultar el Modelo Personalizado vía la API REST

Si saliste de la sesión interactiva en el Ejercicio 7, es posible que el modelo ya no esté cargado. Inicia el servicio Foundry Local y carga el modelo primero:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Verifica en qué puerto está corriendo el servicio:

```bash
foundry service status
```

Luego envía una petición (reemplaza `5273` con el puerto real si es diferente):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Nota para Windows:** El comando `curl` arriba usa sintaxis bash. En Windows, usa el cmdlet `Invoke-RestMethod` de PowerShell a continuación.

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### Ejercicio 9: Usar el Modelo Personalizado con el SDK de OpenAI

Puedes conectarte a tu modelo personalizado usando exactamente el mismo código del SDK de OpenAI que usaste para los modelos integrados (ver [Parte 3](part3-sdk-and-apis.md)). La única diferencia es el nombre del modelo.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local no valida claves API
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local no valida las claves API
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Punto clave:** Dado que Foundry Local expone una API compatible con OpenAI, cualquier código que funcione con los modelos integrados también funciona con tus modelos personalizados. Solo necesitas cambiar el parámetro `model`.

---

### Ejercicio 10: Probar el Modelo Personalizado con el SDK de Foundry Local

En laboratorios anteriores usaste el SDK de Foundry Local para iniciar el servicio, descubrir el endpoint y gestionar modelos automáticamente. Puedes seguir exactamente el mismo patrón con tu modelo compilado personalizado. El SDK maneja el inicio del servicio y el descubrimiento del endpoint, por lo que tu código no necesita codificar el puerto `localhost:5273`.

> **Nota:** Asegúrate de que el SDK de Foundry Local esté instalado antes de ejecutar estos ejemplos:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Agrega los paquetes NuGet `Microsoft.AI.Foundry.Local` y `OpenAI`
>
> Guarda cada archivo de script **en la raíz del repositorio** (en el mismo directorio que tu carpeta `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Paso 1: Inicie el servicio Foundry Local y cargue el modelo personalizado
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Paso 2: Verifique la caché para el modelo personalizado
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Paso 3: Cargue el modelo en la memoria
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Paso 4: Cree un cliente OpenAI usando el endpoint descubierto por el SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Paso 5: Envíe una solicitud de finalización de chat en streaming
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

Ejecuta el script:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// Paso 1: Iniciar el servicio Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Paso 2: Obtener el modelo personalizado del catálogo
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Paso 3: Cargar el modelo en memoria
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Paso 4: Crear un cliente OpenAI usando el endpoint descubierto por el SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Paso 5: Enviar una solicitud de finalización de chat en streaming
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

Ejecuta el script:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **Punto clave:** El SDK de Foundry Local descubre el endpoint dinámicamente, por lo que nunca codificas un número de puerto fijo. Este es el enfoque recomendado para aplicaciones en producción. Tu modelo compilado personalizado funciona de manera idéntica a los modelos del catálogo integrados a través del SDK.

---

## Elección de un Modelo para Compilar

Qwen3-0.6B se usa como ejemplo de referencia en este laboratorio porque es pequeño, rápido de compilar y está disponible libremente bajo la licencia Apache 2.0. Sin embargo, puedes compilar muchos otros modelos. Aquí algunas sugerencias:

| Modelo | ID de Hugging Face | Parámetros | Licencia | Notas |
|--------|--------------------|------------|----------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Muy pequeño, compilación rápida, ideal para pruebas |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Mejor calidad, aún rápido para compilar |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Alta calidad, requiere más RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Requiere aceptación de licencia en Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Alta calidad, descarga mayor y compilación más larga |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Ya está en el catálogo de Foundry Local (útil para comparaciones) |

> **Recordatorio de licencia:** Siempre verifica la licencia del modelo en Hugging Face antes de usarlo. Algunos modelos (como Llama) requieren que aceptes un acuerdo de licencia y te autentiques con `huggingface-cli login` antes de descargarlos.

---

## Conceptos: Cuándo Usar Modelos Personalizados

| Escenario | Por qué Compilar el Tuyo Propio |
|-----------|---------------------------------|
| **Un modelo que necesitas no está en el catálogo** | El catálogo de Foundry Local está curado. Si el modelo que quieres no aparece, compílalo tú mismo. |
| **Modelos fine-tuned** | Si has ajustado un modelo con datos específicos del dominio, necesitas compilar tus propios pesos. |
| **Requisitos específicos de cuantización** | Puede que quieras una precisión o estrategia de cuantización diferente al valor predeterminado del catálogo. |
| **Nuevas versiones de modelos** | Cuando se lanza un nuevo modelo en Hugging Face, puede que aún no esté en el catálogo de Foundry Local. Compilarlo tú mismo te da acceso inmediato. |
| **Investigación y experimentación** | Probar diferentes arquitecturas, tamaños o configuraciones localmente antes de decidir una opción para producción. |

---

## Resumen

En este laboratorio aprendiste a:

| Paso | Qué Hiciste |
|-------|------------|
| 1 | Instalaste el compilador de modelos ONNX Runtime GenAI |
| 2 | Compilaste `Qwen/Qwen3-0.6B` desde Hugging Face a un modelo ONNX optimizado |
| 3 | Creaste un archivo de configuración de plantilla de chat `inference_model.json` |
| 4 | Añadiste el modelo compilado a la caché de Foundry Local |
| 5 | Ejecutaste un chat interactivo con el modelo personalizado vía CLI |
| 6 | Consultaste el modelo a través de la API REST compatible con OpenAI |
| 7 | Te conectaste desde Python, JavaScript y C# usando el SDK de OpenAI |
| 8 | Probaste el modelo personalizado de extremo a extremo con el SDK de Foundry Local |

La conclusión clave es que **cualquier modelo basado en transformadores puede ejecutarse a través de Foundry Local** una vez que ha sido compilado a formato ONNX. La API compatible con OpenAI significa que todo tu código de aplicación existente funciona sin cambios; solo necesitas cambiar el nombre del modelo.

---

## Puntos Clave

| Concepto | Detalle |
|----------|---------|
| Compilador de modelos ONNX Runtime GenAI | Convierte modelos de Hugging Face a formato ONNX con cuantización en un solo comando |
| Formato ONNX | Foundry Local requiere modelos ONNX con configuración de ONNX Runtime GenAI |
| Plantillas de chat | El archivo `inference_model.json` indica a Foundry Local cómo formatear prompts para un modelo dado |
| Objetivos de hardware | Compilar para CPU, GPU NVIDIA (CUDA), DirectML (GPU en Windows) o WebGPU, según tu hardware |
| Cuantización | Precisión más baja (int4) reduce tamaño y mejora velocidad a costa de algo de precisión; fp16 mantiene alta calidad en GPU |
| Compatibilidad de API | Los modelos personalizados usan la misma API compatible con OpenAI que los modelos integrados |
| SDK de Foundry Local | El SDK maneja inicio de servicio, descubrimiento de endpoint y carga de modelos automáticamente para catálogo y modelos personalizados |

---

## Lectura Adicional

| Recurso | Enlace |
|---------|--------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Guía de modelos personalizados para Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Familia de modelos Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Documentación Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Próximos Pasos

Continúa con [Parte 11: Llamadas a Herramientas con Modelos Locales](part11-tool-calling.md) para aprender cómo habilitar que tus modelos locales llamen a funciones externas.

[← Parte 9: Transcripción de Voz con Whisper](part9-whisper-voice-transcription.md) | [Parte 11: Llamadas a Herramientas →](part11-tool-calling.md)