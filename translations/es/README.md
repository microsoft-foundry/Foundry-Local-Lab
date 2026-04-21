<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Taller Foundry Local - Construye aplicaciones de IA en el dispositivo

Un taller práctico para ejecutar modelos de lenguaje en tu propia máquina y construir aplicaciones inteligentes con [Foundry Local](https://foundrylocal.ai) y el [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **¿Qué es Foundry Local?** Foundry Local es un entorno de ejecución ligero que te permite descargar, gestionar y servir modelos de lenguaje completamente en tu hardware. Expone una **API compatible con OpenAI** para que cualquier herramienta o SDK que use OpenAI pueda conectarse, sin necesidad de cuenta en la nube.

---

## Objetivos de aprendizaje

Al final de este taller podrás:

| # | Objetivo |
|---|-----------|
| 1 | Instalar Foundry Local y gestionar modelos con la CLI |
| 2 | Dominar la API del SDK de Foundry Local para la gestión programática de modelos |
| 3 | Conectarte al servidor de inferencia local usando los SDKs de Python, JavaScript y C# |
| 4 | Construir una canalización de Generación Aumentada por Recuperación (RAG) que base las respuestas en tus propios datos |
| 5 | Crear agentes de IA con instrucciones y personalidades persistentes |
| 6 | Orquestar flujos de trabajo multiagente con bucles de retroalimentación |
| 7 | Explorar una aplicación de proyecto final para producción: el Escritor Creativo Zava |
| 8 | Construir marcos de evaluación con conjuntos de datos de referencia y puntuación con LLM-como-juez |
| 9 | Transcribir audio con Whisper - reconocimiento de voz a texto en el dispositivo usando Foundry Local SDK |
| 10 | Compilar y ejecutar modelos personalizados o de Hugging Face con ONNX Runtime GenAI y Foundry Local |
| 11 | Permitir que los modelos locales llamen a funciones externas usando el patrón de llamada de herramientas |
| 12 | Construir una UI basada en navegador para el Escritor Creativo Zava con streaming en tiempo real |

---

## Requisitos previos

| Requisito | Detalles |
|-------------|---------|
| **Hardware** | Mínimo 8 GB RAM (16 GB recomendado); CPU con soporte AVX2 o GPU compatible |
| **Sistema operativo** | Windows 10/11 (x64/ARM), Windows Server 2025, o macOS 13+ |
| **Foundry Local CLI** | Instalar con `winget install Microsoft.FoundryLocal` (Windows) o `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Consulta la [guía para comenzar](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) para más detalles. |
| **Entorno de ejecución** | **Python 3.9+** y/o **.NET 9.0+** y/o **Node.js 18+** |
| **Git** | Para clonar este repositorio |

---

## Comenzando

```bash
# 1. Clona el repositorio
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifica que Foundry Local esté instalado
foundry model list              # Lista los modelos disponibles
foundry model run phi-3.5-mini  # Inicia un chat interactivo

# 3. Elige tu pista de idioma (consulta el laboratorio de la Parte 2 para la configuración completa)
```

| Lenguaje | Inicio rápido |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Partes del taller

### Parte 1: Comenzando con Foundry Local

**Guía del laboratorio:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Qué es Foundry Local y cómo funciona
- Instalación de la CLI en Windows y macOS
- Exploración de modelos - listado, descarga, ejecución
- Comprendiendo alias de modelos y puertos dinámicos

---

### Parte 2: Profundización en Foundry Local SDK

**Guía del laboratorio:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Por qué usar el SDK en lugar de la CLI para desarrollo de aplicaciones
- Referencia completa de la API del SDK para Python, JavaScript y C#
- Gestión del servicio, exploración del catálogo, ciclo de vida del modelo (descargar, cargar, descargar de memoria)
- Patrones de inicio rápido: constructor de Python, `init()` en JavaScript, `CreateAsync()` en C#
- Metadatos `FoundryModelInfo`, alias y selección de modelo óptima para hardware

---

### Parte 3: SDKs y APIs

**Guía del laboratorio:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Conectarse a Foundry Local desde Python, JavaScript y C#
- Uso del SDK para gestionar el servicio programáticamente
- Completados de chat en streaming a través de la API compatible con OpenAI
- Referencia de métodos del SDK para cada lenguaje

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat básico con streaming |
| C# | `csharp/BasicChat.cs` | Chat en streaming con .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat en streaming con Node.js |

---

### Parte 4: Generación Aumentada por Recuperación (RAG)

**Guía del laboratorio:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Qué es RAG y por qué es importante
- Construir una base de conocimiento en memoria
- Recuperación por solapamiento de palabras clave con puntuación
- Composición de indicaciones sistemáticas fundamentadas
- Ejecutar una canalización RAG completa en el dispositivo

**Ejemplos de código:**

| Lenguaje | Archivo |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Parte 5: Construcción de Agentes de IA

**Guía del laboratorio:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Qué es un agente de IA (vs. una llamada directa a LLM)
- Patrón `ChatAgent` y Microsoft Agent Framework
- Instrucciones de sistema, personalidades y conversaciones multi-turno
- Salida estructurada (JSON) desde agentes

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agente único con Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agente único (patrón ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agente único (patrón ChatAgent) |

---

### Parte 6: Flujos de Trabajo Multi-Agente

**Guía del laboratorio:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Canalizaciones multiagente: Investigador → Escritor → Editor
- Orquestación secuencial y bucles de retroalimentación
- Configuración compartida y entregas estructuradas
- Diseña tu propio flujo de trabajo multiagente

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Canalización de tres agentes |
| C# | `csharp/MultiAgent.cs` | Canalización de tres agentes |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Canalización de tres agentes |

---

### Parte 7: Escritor Creativo Zava - Aplicación Final

**Guía del laboratorio:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- App multiagente estilo producción con 4 agentes especializados
- Pipeline secuencial con bucles de retroalimentación guiados por evaluadores
- Salida en streaming, búsqueda en catálogo de productos, entregas JSON estructuradas
- Implementación completa en Python (FastAPI), JavaScript (CLI Node.js) y C# (consola .NET)

**Ejemplos de código:**

| Lenguaje | Directorio | Descripción |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Servicio web FastAPI con orquestador |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplicación CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplicación consola .NET 9 |

---

### Parte 8: Desarrollo Orientado a la Evaluación

**Guía del laboratorio:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construir un marco sistemático de evaluación para agentes de IA usando datasets dorados
- Verificaciones basadas en reglas (longitud, cobertura de palabras clave, términos prohibidos) + puntuación con LLM-como-juez
- Comparación lado a lado de variantes de indicaciones con tarjetas de puntajes agregados
- Extiende el patrón Zava Editor visto en la Parte 7 a una suite de pruebas offline
- Rutas para Python, JavaScript y C#

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Marco de evaluación |
| C# | `csharp/AgentEvaluation.cs` | Marco de evaluación |
| JavaScript | `javascript/foundry-local-eval.mjs` | Marco de evaluación |

---

### Parte 9: Transcripción de Voz con Whisper

**Guía del laboratorio:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Transcripción de voz a texto usando OpenAI Whisper ejecutándose localmente
- Procesamiento de audio con privacidad - el audio nunca sale de tu dispositivo
- Rutas para Python, JavaScript y C# con `client.audio.transcriptions.create()` (Python/JS) y `AudioClient.TranscribeAudioAsync()` (C#)
- Incluye archivos de audio temáticos de Zava para práctica

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transcripción de voz con Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcripción de voz con Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcripción de voz con Whisper |

> **Nota:** Este laboratorio usa el **Foundry Local SDK** para descargar y cargar programáticamente el modelo Whisper, luego envía audio al endpoint local compatible con OpenAI para transcripción. El modelo Whisper (`whisper`) está listado en el catálogo de Foundry Local y corre enteramente en dispositivo, sin necesidad de claves API en la nube o acceso a la red.

---

### Parte 10: Uso de Modelos Personalizados o de Hugging Face

**Guía del laboratorio:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilar modelos Hugging Face a formato ONNX optimizado usando el compilador ONNX Runtime GenAI
- Compilación específica para hardware (CPU, GPU NVIDIA, DirectML, WebGPU) y cuantización (int4, fp16, bf16)
- Crear archivos de configuración de plantilla para chat en Foundry Local
- Añadir modelos compilados a la caché de Foundry Local
- Ejecutar modelos personalizados vía CLI, API REST y SDK OpenAI
- Ejemplo de referencia: compilación completa Qwen/Qwen3-0.6B

---

### Parte 11: Llamada a Herramientas con Modelos Locales

**Guía del laboratorio:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permitir que modelos locales llamen a funciones externas (llamadas a herramientas/funciones)
- Definir esquemas de herramientas usando el formato de llamada de funciones OpenAI
- Gestionar el flujo de conversación de múltiples turnos para llamadas de herramientas
- Ejecutar llamadas a herramientas localmente y devolver resultados al modelo
- Elegir el modelo correcto para escenarios de llamadas a herramientas (Qwen 2.5, Phi-4-mini)
- Usar el `ChatClient` nativo del SDK para llamadas a herramientas (JavaScript)

**Ejemplos de código:**

| Lenguaje | Archivo | Descripción |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Llamadas a herramientas con herramientas de clima/población |
| C# | `csharp/ToolCalling.cs` | Llamadas a herramientas con .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Llamadas a herramientas con ChatClient |

---

### Parte 12: Construcción de una UI Web para el Escritor Creativo Zava

**Guía del laboratorio:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Añadir una interfaz de usuario en navegador para el Escritor Creativo Zava
- Servir la UI compartida desde Python (FastAPI), JavaScript (HTTP Node.js) y C# (ASP.NET Core)
- Consumir NDJSON en streaming en el navegador con Fetch API y ReadableStream
- Indicadores de estado en vivo del agente y streaming en tiempo real del texto del artículo

**Código (UI compartida):**

| Archivo | Descripción |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Diseño de página |
| `zava-creative-writer-local/ui/style.css` | Estilos |
| `zava-creative-writer-local/ui/app.js` | Lector de stream y lógica de actualización del DOM |

**Adiciones backend:**

| Lenguaje | Archivo | Descripción |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Actualizado para servir UI estática |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nuevo servidor HTTP que envuelve al orquestador |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nuevo proyecto API minimalista ASP.NET Core |

---

### Parte 13: Taller completado
**Guía del laboratorio:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Resumen de todo lo que ha construido a lo largo de las 12 partes
- Más ideas para ampliar sus aplicaciones
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
|----------|------|
| Sitio web de Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catálogo de modelos | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| GitHub de Foundry Local | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Guía para comenzar | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referencia del SDK de Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licencia

Este material del taller se proporciona con fines educativos.

---

**¡Feliz construcción! 🚀**