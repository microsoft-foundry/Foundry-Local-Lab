<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Taller Foundry Local - Construye Aplicaciones de IA en el Dispositivo

Un taller práctico para ejecutar modelos de lenguaje en tu propia máquina y construir aplicaciones inteligentes con [Foundry Local](https://foundrylocal.ai) y el [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **¿Qué es Foundry Local?** Foundry Local es un entorno de ejecución ligero que te permite descargar, administrar y servir modelos de lenguaje completamente en tu hardware. Exponen una **API compatible con OpenAI** para que cualquier herramienta o SDK que soporte OpenAI pueda conectarse, sin necesidad de una cuenta en la nube.

### 🌐 Soporte Multilingüe

#### Compatible vía GitHub Action (Automatizado y Siempre Actualizado)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Árabe](../ar/README.md) | [Bengalí](../bn/README.md) | [Búlgaro](../bg/README.md) | [Birmano (Myanmar)](../my/README.md) | [Chino (Simplificado)](../zh-CN/README.md) | [Chino (Tradicional, Hong Kong)](../zh-HK/README.md) | [Chino (Tradicional, Macao)](../zh-MO/README.md) | [Chino (Tradicional, Taiwán)](../zh-TW/README.md) | [Croata](../hr/README.md) | [Checo](../cs/README.md) | [Danés](../da/README.md) | [Holandés](../nl/README.md) | [Estonio](../et/README.md) | [Finlandés](../fi/README.md) | [Francés](../fr/README.md) | [Alemán](../de/README.md) | [Griego](../el/README.md) | [Hebreo](../he/README.md) | [Hindi](../hi/README.md) | [Húngaro](../hu/README.md) | [Indonesio](../id/README.md) | [Italiano](../it/README.md) | [Japonés](../ja/README.md) | [Kannada](../kn/README.md) | [Jemer](../km/README.md) | [Coreano](../ko/README.md) | [Lituano](../lt/README.md) | [Malayo](../ms/README.md) | [Malayalam](../ml/README.md) | [Maratí](../mr/README.md) | [Nepalí](../ne/README.md) | [Pidgin Nigeriano](../pcm/README.md) | [Noruego](../no/README.md) | [Persa (Farsi)](../fa/README.md) | [Polaco](../pl/README.md) | [Portugués (Brasil)](../pt-BR/README.md) | [Portugués (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Rumano](../ro/README.md) | [Ruso](../ru/README.md) | [Serbio (Cirílico)](../sr/README.md) | [Eslovaco](../sk/README.md) | [Esloveno](../sl/README.md) | [Español](./README.md) | [Swahili](../sw/README.md) | [Sueco](../sv/README.md) | [Tagalo (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Tailandés](../th/README.md) | [Turco](../tr/README.md) | [Ucraniano](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamita](../vi/README.md)

> **¿Prefieres Clonar Localmente?**
>
> Este repositorio incluye más de 50 traducciones, lo que aumenta significativamente el tamaño de la descarga. Para clonar sin traducciones, utiliza sparse checkout:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Esto te proporciona todo lo necesario para completar el curso con una descarga mucho más rápida.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Objetivos de Aprendizaje

Al final de este taller podrás:

| # | Objetivo |
|---|----------|
| 1 | Instalar Foundry Local y administrar modelos con la CLI |
| 2 | Dominar la API del SDK de Foundry Local para la gestión programática de modelos |
| 3 | Conectarte al servidor local de inferencia usando los SDKs de Python, JavaScript y C# |
| 4 | Construir una canalización de Generación Aumentada por Recuperación (RAG) que fundamente respuestas en tus propios datos |
| 5 | Crear agentes de IA con instrucciones y personalidades persistentes |
| 6 | Orquestar flujos de trabajo multiagente con bucles de retroalimentación |
| 7 | Explorar una aplicación capstone en producción: el Zava Creative Writer |
| 8 | Construir marcos de evaluación con conjuntos de datos 'golden' y puntuaciones de LLM-como-juez |
| 9 | Transcribir audio con Whisper – reconocimiento de voz a texto en el dispositivo usando Foundry Local SDK |
| 10 | Compilar y ejecutar modelos personalizados o de Hugging Face con ONNX Runtime GenAI y Foundry Local |
| 11 | Permitir que modelos locales llamen a funciones externas con el patrón de llamadas a herramientas |
| 12 | Construir una interfaz de usuario basada en navegador para Zava Creative Writer con streaming en tiempo real |

---

## Requisitos Previos

| Requisito | Detalles |
|-----------|----------|
| **Hardware** | Mínimo 8 GB de RAM (16 GB recomendado); CPU compatible con AVX2 o GPU soportada |
| **SO** | Windows 10/11 (x64/ARM), Windows Server 2025, o macOS 13+ |
| **Foundry Local CLI** | Instalar vía `winget install Microsoft.FoundryLocal` (Windows) o `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Consulta la [guía para empezar](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) para más detalles. |
| **Entorno de ejecución** | **Python 3.9+** y/o **.NET 9.0+** y/o **Node.js 18+** |
| **Git** | Para clonar este repositorio |

---

## Primeros Pasos

```bash
# 1. Clona el repositorio
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifica que Foundry Local esté instalado
foundry model list              # Lista los modelos disponibles
foundry model run phi-3.5-mini  # Inicia un chat interactivo

# 3. Elige tu ruta de idioma (consulta el laboratorio Parte 2 para la configuración completa)
```

| Lenguaje | Inicio Rápido |
|----------|---------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Partes del Taller

### Parte 1: Primeros Pasos con Foundry Local

**Guía del laboratorio:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Qué es Foundry Local y cómo funciona
- Instalación de la CLI en Windows y macOS
- Exploración de modelos: listar, descargar, ejecutar
- Entender alias de modelos y puertos dinámicos

---

### Parte 2: Profundización en el SDK de Foundry Local

**Guía del laboratorio:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Por qué usar el SDK en lugar de la CLI para el desarrollo de aplicaciones
- Referencia completa de la API SDK para Python, JavaScript y C#
- Gestión del servicio, exploración de catálogo, ciclo de vida del modelo (descargar, cargar, descargar)
- Patrones de inicio rápido: bootstrap del constructor Python, `init()` de JavaScript, `CreateAsync()` de C#
- Metadatos `FoundryModelInfo`, alias, y selección de modelo óptima para el hardware

---

### Parte 3: SDKs y APIs

**Guía del laboratorio:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Conexión a Foundry Local desde Python, JavaScript y C#
- Uso del SDK de Foundry Local para administrar el servicio de forma programática
- Completaciones de chat en streaming vía la API compatible con OpenAI
- Referencia de métodos del SDK para cada lenguaje

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|---------|-------------|
| Python | `python/foundry-local.py` | Chat básico en streaming |
| C# | `csharp/BasicChat.cs` | Chat en streaming con .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat en streaming con Node.js |

---

### Parte 4: Generación Aumentada por Recuperación (RAG)

**Guía del laboratorio:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Qué es RAG y por qué es importante
- Construcción de una base de conocimiento en memoria
- Recuperación por superposición de palabras clave con puntuación
- Composición de prompts de sistema fundamentados
- Ejecución completa de un pipeline RAG en el dispositivo

**Ejemplos de código:**

| Lenguaje | Archivo |
|----------|---------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Parte 5: Construyendo Agentes de IA

**Guía del laboratorio:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Qué es un agente de IA (vs. una llamada directa a un LLM)
- Patrón `ChatAgent` y Microsoft Agent Framework
- Instrucciones del sistema, personalidades y conversaciones de múltiples turnos
- Salida estructurada (JSON) de agentes

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|---------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agente único con Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agente único (patrón ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agente único (patrón ChatAgent) |

---

### Parte 6: Flujos de Trabajo Multiagente

**Guía del laboratorio:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines multiagente: Investigador → Escritor → Editor
- Orquestación secuencial y bucles de retroalimentación
- Configuración compartida y entregas estructuradas
- Diseñar tu propio flujo de trabajo multiagente

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|---------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline de tres agentes |
| C# | `csharp/MultiAgent.cs` | Pipeline de tres agentes |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline de tres agentes |

---

### Parte 7: Zava Creative Writer - Aplicación Capstone

**Guía del laboratorio:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Aplicación multiagente estilo producción con 4 agentes especializados
- Pipeline secuencial con bucles de retroalimentación dirigidos por evaluadores
- Salida en streaming, búsqueda de catálogo de productos, entregas JSON estructuradas
- Implementación completa en Python (FastAPI), JavaScript (CLI Node.js) y C# (consola .NET)

**Ejemplos de código:**

| Lenguaje | Directorio | Descripción |
|----------|------------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Servicio web FastAPI con orquestador |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplicación CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplicación consola .NET 9 |

---

### Parte 8: Desarrollo Guiado por Evaluación

**Guía del laboratorio:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construir un marco sistemático de evaluación para agentes de IA usando conjuntos de datos `golden`
- Controles basados en reglas (longitud, cobertura de palabras clave, términos prohibidos) + puntuación con LLM-como-juez
- Comparación lado a lado de variantes de prompts con tablas de puntuación agregadas
- Extiende el patrón del agente Editor de Zava de la Parte 7 hacia un suite de pruebas offline
- Rutas para Python, JavaScript y C#

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|---------|-------------|
| Python | `python/foundry-local-eval.py` | Marco de evaluación |
| C# | `csharp/AgentEvaluation.cs` | Marco de evaluación |
| JavaScript | `javascript/foundry-local-eval.mjs` | Marco de evaluación |

---

### Parte 9: Transcripción de Voz con Whisper

**Guía del laboratorio:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Transcripción de voz a texto usando OpenAI Whisper ejecutándose localmente
- Procesamiento de audio priorizando la privacidad: el audio nunca sale de tu dispositivo
- Rutas en Python, JavaScript y C# con `client.audio.transcriptions.create()` (Python/JS) y `AudioClient.TranscribeAudioAsync()` (C#)
- Incluye archivos de audio de ejemplo con temática Zava para práctica práctica

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|---------|-------------|
| Python   | `python/foundry-local-whisper.py` | Transcripción de voz Whisper |
| C#       | `csharp/WhisperTranscription.cs` | Transcripción de voz Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcripción de voz Whisper |

> **Nota:** Este laboratorio usa el **Foundry Local SDK** para descargar y cargar programáticamente el modelo Whisper, luego envía audio al endpoint local compatible con OpenAI para transcripción. El modelo Whisper (`whisper`) está listado en el catálogo de Foundry Local y se ejecuta totalmente en el dispositivo — no se requieren claves de API en la nube ni acceso a la red.

---

### Parte 10: Uso de modelos personalizados o Hugging Face

**Guía del laboratorio:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilar modelos Hugging Face a formato ONNX optimizado usando el compilador de modelos GenAI de ONNX Runtime
- Compilación específica para hardware (CPU, GPU NVIDIA, DirectML, WebGPU) y cuantización (int4, fp16, bf16)
- Creación de archivos de configuración de plantillas de chat para Foundry Local
- Agregar modelos compilados a la caché de Foundry Local
- Ejecutar modelos personalizados mediante CLI, API REST y SDK de OpenAI
- Ejemplo de referencia: compilación de Qwen/Qwen3-0.6B de extremo a extremo

---

### Parte 11: Llamada a herramientas con modelos locales

**Guía del laboratorio:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permitir que modelos locales llamen a funciones externas (llamadas a herramientas/funciones)
- Definir esquemas de herramientas usando el formato de llamada a funciones de OpenAI
- Manejar el flujo de conversación de multi-turno para llamadas a herramientas
- Ejecutar llamadas a herramientas localmente y devolver resultados al modelo
- Elegir el modelo adecuado para escenarios de llamada a herramientas (Qwen 2.5, Phi-4-mini)
- Usar el `ChatClient` nativo del SDK para llamada a herramientas (JavaScript)

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|---------|-------------|
| Python   | `python/foundry-local-tool-calling.py` | Llamada a herramientas con aplicaciones de clima/población |
| C#       | `csharp/ToolCalling.cs` | Llamada a herramientas con .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Llamada a herramientas con ChatClient |

---

### Parte 12: Crear una interfaz web para el Zava Creative Writer

**Guía del laboratorio:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Añadir un front-end basado en navegador para el Zava Creative Writer
- Servir la UI compartida desde Python (FastAPI), JavaScript (Node.js HTTP) y C# (ASP.NET Core)
- Consumir NDJSON en streaming en el navegador con Fetch API y ReadableStream
- Insignias de estado de agentes en vivo y transmisión en tiempo real del texto del artículo

**Código (UI compartida):**

| Archivo                                | Descripción         |
|--------------------------------------|---------------------|
| `zava-creative-writer-local/ui/index.html` | Diseño de la página |
| `zava-creative-writer-local/ui/style.css` | Estilos             |
| `zava-creative-writer-local/ui/app.js` | Lógica de lectura de stream y actualización del DOM |

**Adiciones al backend:**

| Lenguaje | Archivo                                  | Descripción                |
|----------|-----------------------------------------|----------------------------|
| Python   | `zava-creative-writer-local/src/api/main.py` | Actualizado para servir UI estática |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nuevo servidor HTTP que envuelve al orquestador |
| C#       | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nuevo proyecto mínimo API ASP.NET Core |

---

### Parte 13: Taller completado

**Guía del laboratorio:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Resumen de todo lo construido en las 12 partes
- Ideas adicionales para extender tus aplicaciones
- Enlaces a recursos y documentación

---

## Estructura del proyecto

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Recursos

| Recurso | Enlace |
|---------|--------|
| Sitio web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catálogo de modelos | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Guía de inicio | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referencia SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licencia

Este material del taller se proporciona para fines educativos.

---

**¡Feliz construcción! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento ha sido traducido utilizando el servicio de traducción automática [Co-op Translator](https://github.com/Azure/co-op-translator). Aunque nos esforzamos por la precisión, tenga en cuenta que las traducciones automáticas pueden contener errores o inexactitudes. El documento original en su idioma nativo debe considerarse la fuente autorizada. Para información crítica, se recomienda una traducción profesional realizada por humanos. No nos responsabilizamos por malentendidos o interpretaciones erróneas que surjan del uso de esta traducción.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->