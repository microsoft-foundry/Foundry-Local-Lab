# Instrucciones para Agentes de Codificación

Este archivo proporciona contexto para agentes de codificación AI (GitHub Copilot, Copilot Workspace, Codex, etc.) que trabajan en este repositorio.

## Visión General del Proyecto

Este es un **taller práctico** para crear aplicaciones de IA con [Foundry Local](https://foundrylocal.ai) — un entorno ligero que descarga, gestiona y sirve modelos de lenguaje completamente en el dispositivo a través de una API compatible con OpenAI. El taller incluye guías paso a paso y ejemplos de código ejecutables en Python, JavaScript y C#.

## Estructura del Repositorio

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Detalles de Lenguaje y Framework

### Python
- **Ubicación:** `python/`, `zava-creative-writer-local/src/api/`
- **Dependencias:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Paquetes clave:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Versión mínima:** Python 3.9+
- **Ejecutar:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Ubicación:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Dependencias:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Paquetes clave:** `foundry-local-sdk`, `openai`
- **Sistema de módulos:** módulos ES (`.mjs` archivos, `"type": "module"`)
- **Versión mínima:** Node.js 18+
- **Ejecutar:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Ubicación:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Archivos de proyecto:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Paquetes clave:** `Microsoft.AI.Foundry.Local` (no Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superconjunto con QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Objetivo:** .NET 9.0 (TFM condicional: `net9.0-windows10.0.26100` en Windows, `net9.0` en otros)
- **Ejecutar:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Convenciones de Codificación

### General
- Todos los ejemplos de código son **ejemplos de un solo archivo autónomos** — sin librerías utilitarias compartidas ni abstracciones.
- Cada ejemplo se ejecuta de forma independiente instalando sus propias dependencias.
- Las claves API siempre se configuran como `"foundry-local"` — Foundry Local usa esto como un marcador de posición.
- Las URLs base usan `http://localhost:<port>/v1` — el puerto es dinámico y se descubre en tiempo de ejecución a través del SDK (`manager.urls[0]` en JS, `manager.endpoint` en Python).
- El SDK de Foundry Local gestiona el arranque del servicio y el descubrimiento de endpoints; prefiera patrones del SDK en lugar de puertos codificados.

### Python
- Use el SDK `openai` con `OpenAI(base_url=..., api_key="not-required")`.
- Use `FoundryLocalManager()` de `foundry_local` para el ciclo de vida gestionado por el SDK.
- Streaming: itere sobre el objeto `stream` con `for chunk in stream:`.
- Sin anotaciones de tipo en archivos de ejemplo (mantener ejemplos concisos para aprendices del taller).

### JavaScript
- Sintaxis de módulo ES: `import ... from "..."`.
- Use `OpenAI` de `"openai"` y `FoundryLocalManager` de `"foundry-local-sdk"`.
- Patrón de inicialización SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Se usa `await` a nivel superior a lo largo del código.

### C#
- Nullable habilitado, usings implícitos, .NET 9.
- Use `FoundryLocalManager.StartServiceAsync()` para ciclo de vida gestionado por el SDK.
- Streaming: `CompleteChatStreaming()` con `foreach (var update in completionUpdates)`.
- El archivo principal `csharp/Program.cs` es un enrutador CLI que despacha a métodos estáticos `RunAsync()`.

### Llamada a Herramientas
- Solo ciertos modelos soportan llamada a herramientas: familia **Qwen 2.5** (`qwen2.5-*`) y **Phi-4-mini** (`phi-4-mini`).
- Los esquemas de herramientas siguen el formato JSON de llamada a función de OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- La conversación usa un patrón de múltiples turnos: usuario → asistente (tool_calls) → herramienta (resultados) → asistente (respuesta final).
- El `tool_call_id` en los mensajes de resultados de herramienta debe coincidir con el `id` de la llamada a función del modelo.
- Python usa el SDK de OpenAI directamente; JavaScript usa el `ChatClient` nativo del SDK (`model.createChatClient()`); C# usa el SDK de OpenAI con `ChatTool.CreateFunctionTool()`.

### ChatClient (Cliente nativo SDK)
- JavaScript: `model.createChatClient()` devuelve un `ChatClient` con `completeChat(messages, tools?)` y `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` devuelve un `ChatClient` estándar que puede usarse sin importar el paquete NuGet de OpenAI.
- Python no tiene un ChatClient nativo — use el SDK OpenAI con `manager.endpoint` y `manager.api_key`.
- **Importante:** `completeStreamingChat` en JavaScript usa un **patrón de callback**, no iteración async.

### Modelos de Razonamiento
- `phi-4-mini-reasoning` envuelve su razonamiento en etiquetas `<think>...</think>` antes de la respuesta final.
- Analice las etiquetas para separar razonamiento de la respuesta cuando sea necesario.

## Guías de Laboratorio

Los archivos de laboratorio están en `labs/` en Markdown. Siguen una estructura consistente:
- Imagen de encabezado con logo
- Llamado con título y objetivo
- Visión general, objetivos de aprendizaje, prerequisitos
- Secciones explicativas conceptuales con diagramas
- Ejercicios numerados con bloques de código y salida esperada
- Tabla resumen, conclusiones clave, lecturas adicionales
- Enlace de navegación a la siguiente parte

Al editar el contenido del laboratorio:
- Mantenga el estilo de formato Markdown existente y la jerarquía de secciones.
- Los bloques de código deben especificar el lenguaje (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Proporcione variantes bash y PowerShell para comandos donde importe el SO.
- Use estilos de llamada a la atención `> **Note:**`, `> **Tip:**`, y `> **Troubleshooting:**`.
- Las tablas usan el formato con barras `| Header | Header |`.

## Comandos de Construcción y Prueba

| Acción | Comando |
|--------|---------|
| **Ejemplos Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Ejemplos JS** | `cd javascript && npm install && node <script>.mjs` |
| **Ejemplos C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **CLI Foundry Local** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generar diagramas** | `npx mmdc -i <input>.mmd -o <output>.svg` (requiere `npm install` con permisos root) |

## Dependencias Externas

- El **CLI de Foundry Local** debe estar instalado en la máquina del desarrollador (`winget install Microsoft.FoundryLocal` o `brew install foundrylocal`).
- El **servicio Foundry Local** corre localmente y expone una API REST compatible con OpenAI en un puerto dinámico.
- No se requieren servicios en la nube, claves API ni suscripciones Azure para ejecutar ninguno de los ejemplos.
- La Parte 10 (modelos personalizados) requiere además `onnxruntime-genai` y descarga los pesos desde Hugging Face.

## Archivos que No Deben Ser Comiteados

El archivo `.gitignore` debe excluir (y lo hace para la mayoría):
- `.venv/` — entornos virtuales Python
- `node_modules/` — dependencias npm
- `models/` — salida de modelos ONNX compilados (archivos binarios grandes, generados en Parte 10)
- `cache_dir/` — caché de descarga de modelos Hugging Face
- `.olive-cache/` — directorio de trabajo Microsoft Olive
- `samples/audio/*.wav` — muestras de audio generadas (regeneradas con `python samples/audio/generate_samples.py`)
- Artefactos estándar de compilación Python (`__pycache__/`, `*.egg-info/`, `dist/`, etc.)

## Licencia

MIT — ver `LICENSE`.