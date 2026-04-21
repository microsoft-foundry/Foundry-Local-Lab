# Registro de cambios — Taller local de Foundry

Todos los cambios notables en este taller están documentados a continuación.

---

## 2026-03-11 — Parte 12 y 13, Interfaz web, Reescritura de Whisper, Corrección WinML/QNN y Validación

### Añadido
- **Parte 12: Construcción de una interfaz web para el escritor creativo Zava** — nueva guía de laboratorio (`labs/part12-zava-ui.md`) con ejercicios que cubren transmisión NDJSON, `ReadableStream` del navegador, insignias de estado en vivo del agente y transmisión en tiempo real del texto del artículo
- **Parte 13: Taller completado** — nuevo laboratorio resumen (`labs/part13-workshop-complete.md`) con un repaso de las 12 partes, ideas adicionales y enlaces a recursos
- **Interfaz frontal Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — interfaz de navegador compartida en HTML/CSS/JS estándar consumida por los tres backends
- **Servidor HTTP en JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — nuevo servidor HTTP estilo Express que envuelve al orquestador para acceso desde navegador
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` y `ZavaCreativeWriterWeb.csproj` — nuevo proyecto de API mínima que sirve la UI y streaming NDJSON
- **Generador de muestras de audio:** `samples/audio/generate_samples.py` — script TTS offline usando `pyttsx3` para generar archivos WAV temáticos de Zava para la Parte 9
- **Muestra de audio:** `samples/audio/zava-full-project-walkthrough.wav` — nueva muestra de audio más larga para pruebas de transcripción
- **Script de validación:** `validate-npu-workaround.ps1` — script PowerShell automatizado para validar la solución alternativa NPU/QNN en todas las muestras de C#
- **Diagramas Mermaid en SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Soporte multiplataforma WinML:** Los 3 archivos `.csproj` de C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ahora usan TFM condicional y referencias mutualmente exclusivas para soporte multiplataforma. En Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superset que incluye plugin QNN EP). En no-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK base). El RID fijo `win-arm64` en proyectos Zava fue reemplazado por detección automática. Una solución alternativa transitoria excluye activos nativos de `Microsoft.ML.OnnxRuntime.Gpu.Linux` que tiene referencia rota en win-arm64. La solución try/catch previa para NPU ha sido eliminada de los 7 archivos C#.

### Cambiado
- **Parte 9 (Whisper):** Gran reescritura — JavaScript ahora usa `AudioClient` incorporado del SDK (`model.createAudioClient()`) en lugar de inferencia manual ONNX Runtime; actualizado descripciones de arquitectura, tablas comparativas y diagramas de pipeline para reflejar enfoque JS/C# `AudioClient` frente a Python ONNX Runtime
- **Parte 11:** Actualizados enlaces de navegación (ahora apunta a Parte 12); añadidos diagramas SVG renderizados para flujo y secuencia de llamadas a herramientas
- **Parte 10:** Navegación actualizada para pasar por Parte 12 en lugar de terminar el taller
- **Whisper en Python (`foundry-local-whisper.py`):** Ampliado con muestras de audio adicionales y mejor manejo de errores
- **Whisper en JavaScript (`foundry-local-whisper.mjs`):** Reescrito para usar `model.createAudioClient()` con `audioClient.transcribe()` en lugar de sesiones ONNX Runtime manuales
- **FastAPI en Python (`zava-creative-writer-local/src/api/main.py`):** Actualizado para servir archivos estáticos de UI junto con la API
- **Consola C# Zava (`zava-creative-writer-local/src/csharp/Program.cs`):** Eliminada solución alternativa NPU (ahora manejada por paquete WinML)
- **README.md:** Añadida sección Parte 12 con tablas de muestras de código y adiciones de backend; añadida sección Parte 13; actualizados objetivos de aprendizaje y estructura del proyecto
- **KNOWN-ISSUES.md:** Eliminado Issue #7 resuelto (Variante de modelo NPU SDK C# — ahora manejado por paquete WinML). Renumerados problemas restantes a #1–#6. Actualizados detalles de entorno con .NET SDK 10.0.104
- **AGENTS.md:** Actualizado árbol de estructura del proyecto con nuevas entradas `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); actualizados paquetes clave de C# y detalles de TFM condicional
- **labs/part2-foundry-local-sdk.md:** Actualizado ejemplo `.csproj` para mostrar patrón multiplataforma completo con TFM condicional, referencias mutualmente exclusivas y nota explicativa

### Validado
- Los 3 proyectos C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) se compilan exitosamente en Windows ARM64
- Muestra de chat (`dotnet run chat`): modelo carga como `phi-3.5-mini-instruct-qnn-npu:1` vía WinML/QNN — la variante NPU carga directamente sin fallback a CPU
- Muestra de agente (`dotnet run agent`): corre end-to-end con conversación de múltiples turnos, código de salida 0
- Foundry Local CLI v0.8.117 y SDK v0.9.0 sobre .NET SDK 9.0.312

---

## 2026-03-11 — Correcciones de código, limpieza de modelos, diagramas Mermaid y validación

### Corregido
- **Las 21 muestras de código (7 Python, 7 JavaScript, 7 C#):** Añadido `model.unload()` / `unload_model()` / `model.UnloadAsync()` para limpieza al salir y resolver advertencias de fugas de memoria OGA (Problema conocido #4)
- **csharp/WhisperTranscription.cs:** Reemplazada ruta relativa frágil `AppContext.BaseDirectory` con `FindSamplesDirectory()` que recorre directorios hacia arriba para localizar `samples/audio` de forma fiable (Problema conocido #7)
- **csharp/csharp.csproj:** Reemplazado `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` fijo con detección automática usando `$(NETCoreSdkRuntimeIdentifier)` para que `dotnet run` funcione en cualquier plataforma sin flag `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Cambiado
- **Parte 8:** Convertido bucle de iteración evaluativo de diagrama de caja ASCII a imagen SVG renderizada
- **Parte 10:** Convertido diagrama de pipeline de compilación de flechas ASCII a imagen SVG renderizada
- **Parte 11:** Convertidos diagramas de flujo y secuencia de llamada a herramientas a imágenes SVG renderizadas
- **Parte 10:** Movida sección "¡Taller completado!" a Parte 11 (el laboratorio final); reemplazada con enlace "Próximos pasos"
- **KNOWN-ISSUES.md:** Revalidación completa de todos los problemas contra CLI v0.8.117. Eliminados problemas resueltos: fuga de memoria OGA (añadida limpieza), ruta Whisper (FindSamplesDirectory), inferencia HTTP 500 sostenida (no reproducible, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), limitaciones tool_choice (ahora funciona con `"required"` y targeting de función específica en qwen2.5-0.5b). Actualizado problema JS Whisper — ahora todos los archivos retornan salida vacía/binaria (regresión de v0.9.x, severidad elevada a Mayor). Actualizado RID #4 C# con workaround de detección automática y enlace [#497](https://github.com/microsoft/Foundry-Local/issues/497). Quedan 7 problemas abiertos.
- **javascript/foundry-local-whisper.mjs:** Corregido nombre variable de limpieza (`whisperModel` → `model`)

### Validado
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — ejecutan correctamente con limpieza
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — ejecutan correctamente con limpieza
- C#: `dotnet build` pasa con 0 advertencias, 0 errores (objetivo net9.0)
- Los 7 archivos Python pasan chequeo de sintaxis con `py_compile`
- Los 7 archivos JavaScript pasan validación sintáctica con `node --check`

---

## 2026-03-10 — Parte 11: Llamada a herramientas, expansión API SDK y cobertura de modelos

### Añadido
- **Parte 11: Llamada a herramientas con modelos locales** — nueva guía de laboratorio (`labs/part11-tool-calling.md`) con 8 ejercicios que cubren esquemas de herramientas, flujo multi-turno, múltiples llamadas a herramientas, herramientas personalizadas, llamadas a herramientas con ChatClient y `tool_choice`
- **Muestra en Python:** `python/foundry-local-tool-calling.py` — llamadas a herramientas con `get_weather`/`get_population` usando OpenAI SDK
- **Muestra en JavaScript:** `javascript/foundry-local-tool-calling.mjs` — llamadas a herramientas usando `ChatClient` nativo del SDK (`model.createChatClient()`)
- **Muestra en C#:** `csharp/ToolCalling.cs` — llamada a herramientas usando `ChatTool.CreateFunctionTool()` con SDK OpenAI C#
- **Parte 2, Ejercicio 7:** `ChatClient` nativo — `model.createChatClient()` (JS) y `model.GetChatClientAsync()` (C#) como alternativas al SDK OpenAI
- **Parte 2, Ejercicio 8:** Variantes de modelo y selección de hardware — `selectVariant()`, `variants`, tabla de variantes NPU (7 modelos)
- **Parte 2, Ejercicio 9:** Actualizaciones de modelos y refresco de catálogo — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Parte 2, Ejercicio 10:** Modelos de razonamiento — `phi-4-mini-reasoning` con ejemplos de análisis de etiqueta `<think>`
- **Parte 3, Ejercicio 4:** `createChatClient` como alternativa al SDK OpenAI, con documentación de patrón de callbacks de streaming
- **AGENTS.md:** Añadidas convenciones de codificación para llamada a herramientas, ChatClient y modelos de razonamiento

### Cambiado
- **Parte 1:** Catálogo de modelos ampliado — añadidos phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Parte 2:** Tablas de referencia API ampliadas — añadidos `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Parte 2:** Renumerados ejercicios 7-9 → 10-13 para acomodar nuevos ejercicios
- **Parte 3:** Actualizada tabla de conclusiones clave para incluir ChatClient nativo
- **README.md:** Añadida sección Parte 11 con tabla de muestras de código; añadido objetivo de aprendizaje #11; actualizado árbol de estructura del proyecto
- **csharp/Program.cs:** Añadido caso `toolcall` al router CLI y actualizado texto de ayuda

---

## 2026-03-09 — Actualización SDK v0.9.0, inglés británico y pasada de validación

### Cambiado
- **Todas las muestras de código (Python, JavaScript, C#):** Actualizadas a API Foundry Local SDK v0.9.0 — corregido `await catalog.getModel()` (falta `await`), actualizados patrones de inicialización `FoundryLocalManager`, corregida detección de endpoints
- **Todas las guías de laboratorio (Partes 1-10):** Convertidas a inglés británico (colour, catalogue, optimised, etc.)
- **Todas las guías de laboratorio:** Código de ejemplo SDK actualizado para coincidir con la superficie API v0.9.0
- **Todas las guías de laboratorio:** Tablas de referencia API y bloques de código de ejercicios actualizados
- **Corrección crítica JavaScript:** Añadido `await` faltante en `catalog.getModel()` — retornaba una `Promise` no un objeto `Model`, causando fallos silenciosos posteriores

### Validado
- Todas las muestras Python ejecutan correctamente contra servicio Foundry Local
- Todas las muestras JavaScript ejecutan correctamente (Node.js 18+)
- Proyecto C# compila y ejecuta en .NET 9.0 (compatibilidad directa desde ensamblado SDK net8.0)
- 29 archivos modificados y validados en todo el taller

---

## Índice de archivos

| Archivo | Última actualización | Descripción |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Catálogo de modelos ampliado |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nuevos ejercicios 7-10, tablas API ampliadas |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nuevo ejercicio 4 (ChatClient), conclusiones actualizadas |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + inglés británico |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + inglés británico |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Inglés británico |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Inglés británico |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Diagrama Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Inglés británico |
| `labs/part10-custom-models.md` | 2026-03-11 | Diagrama Mermaid, se movió Taller Completo a la Parte 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nuevo laboratorio, diagramas Mermaid, sección Taller Completo |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nuevo: ejemplo de llamadas a herramientas |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nuevo: ejemplo de llamadas a herramientas |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nuevo: ejemplo de llamadas a herramientas |
| `csharp/Program.cs` | 2026-03-10 | Comando CLI `toolcall` añadido |
| `README.md` | 2026-03-10 | Parte 11, estructura del proyecto |
| `AGENTS.md` | 2026-03-10 | Llamadas a herramientas + convenciones ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Se eliminó el problema resuelto #7, quedan 6 problemas abiertos |
| `csharp/csharp.csproj` | 2026-03-11 | TFM multiplataforma, referencias condicionales WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM multiplataforma, detección automática RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM multiplataforma, detección automática RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Eliminada solución temporal try/catch NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Eliminada solución temporal try/catch NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Eliminada solución temporal try/catch NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Eliminada solución temporal try/catch NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Eliminada solución temporal try/catch NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Ejemplo multiplataforma .csproj |
| `AGENTS.md` | 2026-03-11 | Actualizados paquetes C# y detalles de TFM |
| `CHANGELOG.md` | 2026-03-11 | Este archivo |