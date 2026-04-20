# Problemas Conocidos — Taller Local de Foundry

Problemas encontrados durante la construcción y prueba de este taller en un dispositivo **Snapdragon X Elite (ARM64)** con Windows, usando Foundry Local SDK v0.9.0, CLI v0.8.117 y .NET SDK 10.0.

> **Última validación:** 2026-03-11

---

## 1. CPU Snapdragon X Elite No Reconocida por ONNX Runtime

**Estado:** Abierto  
**Severidad:** Advertencia (no bloqueante)  
**Componente:** ONNX Runtime / cpuinfo  
**Reproducción:** Cada inicio del servicio Foundry Local en hardware Snapdragon X Elite  

Cada vez que se inicia el servicio Foundry Local, se emiten dos advertencias:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Impacto:** Las advertencias son meramente cosméticas — la inferencia funciona correctamente. Sin embargo, aparecen en cada ejecución y pueden confundir a los participantes del taller. La biblioteca cpuinfo de ONNX Runtime necesita actualizarse para reconocer los núcleos Qualcomm Oryon CPU.

**Esperado:** Snapdragon X Elite debería reconocerse como un CPU ARM64 compatible sin emitir mensajes de error.

---

## 2. NullReferenceException en SingleAgent en la Primera Ejecución

**Estado:** Abierto (intermitente)  
**Severidad:** Crítico (caída)  
**Componente:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reproducción:** Ejecutar `dotnet run agent` — se bloquea inmediatamente después de cargar el modelo  

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Contexto:** La línea 37 llama a `model.IsCachedAsync(default)`. El bloqueo ocurrió en la primera ejecución del agente tras un `foundry service stop` limpio. Las ejecuciones posteriores con el mismo código tuvieron éxito.

**Impacto:** Intermitente — sugiere una condición de carrera en la inicialización del servicio del SDK o en la consulta del catálogo. La llamada `GetModelAsync()` puede devolver antes de que el servicio esté completamente listo.

**Esperado:** `GetModelAsync()` debería bloquearse hasta que el servicio esté listo o devolver un mensaje de error claro si el servicio aún no ha terminado de inicializarse.

---

## 3. El SDK de C# Requiere RuntimeIdentifier Explícito

**Estado:** Abierto — rastreado en [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Severidad:** Falta en documentación  
**Componente:** Paquete NuGet `Microsoft.AI.Foundry.Local`  
**Reproducción:** Crear un proyecto .NET 8+ sin `<RuntimeIdentifier>` en el `.csproj`  

La compilación falla con:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Causa raíz:** El requisito del RID es esperado — el SDK incluye binarios nativos (P/Invoke en `Microsoft.AI.Foundry.Local.Core` y ONNX Runtime), por lo que .NET necesita saber qué biblioteca específica de plataforma resolver.

Esto está documentado en MS Learn ([Cómo usar completaciones de chat nativas](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), donde las instrucciones de ejecución muestran:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
Sin embargo, los usuarios deben recordar la bandera `-r` cada vez, lo cual es fácil de olvidar.

**Solución temporal:** Añadir una detección automática a tu `.csproj` para que `dotnet run` funcione sin banderas:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` es una propiedad incorporada de MSBuild que resuelve automáticamente el RID de la máquina host. Los propios proyectos de prueba del SDK ya usan este patrón. Las banderas `-r` explícitas siguen respetándose cuando se proporcionan.

> **Nota:** El `.csproj` del taller incluye esta detección automática para que `dotnet run` funcione de inmediato en cualquier plataforma.

**Esperado:** La plantilla `.csproj` en la documentación de MS Learn debería incluir este patrón de detección automática para que los usuarios no tengan que recordar la bandera `-r`.

---

## 4. JavaScript Whisper — Transcripción de Audio Devuelve Salida Vacía/Binaria

**Estado:** Abierto (regresión — empeorado desde el reporte inicial)  
**Severidad:** Mayor  
**Componente:** Implementación JavaScript Whisper (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reproducción:** Ejecutar `node foundry-local-whisper.mjs` — todos los archivos de audio devuelven salida vacía o binaria en lugar de transcripción de texto  

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Originalmente sólo el quinto archivo de audio devolvía vacío; desde la v0.9.x, los 5 archivos devuelven un solo byte (`\ufffd`) en lugar del texto transcrito. La implementación Whisper en Python usando el SDK OpenAI transcribe correctamente los mismos archivos.

**Esperado:** `createAudioClient()` debería devolver la transcripción de texto que coincide con las implementaciones Python/C#.

---

## 5. El SDK de C# Sólo Incluye net8.0 — Sin Objetivo Oficial para .NET 9 o .NET 10

**Estado:** Abierto  
**Severidad:** Falta en documentación  
**Componente:** Paquete NuGet `Microsoft.AI.Foundry.Local` v0.9.0  
**Comando de instalación:** `dotnet add package Microsoft.AI.Foundry.Local`  

El paquete NuGet sólo incluye un marco de destino:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
No se incluye ningún TFM para `net9.0` o `net10.0`. En contraste, el paquete complementario `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) incluye `net8.0`, `net9.0`, `net10.0`, `net472` y `netstandard2.0`.

### Pruebas de Compatibilidad

| Marco de Destino | Compilación | Ejecución | Notas |
|-----------------|-------------|-----------|-------|
| net8.0          | ✅          | ✅        | Soportado oficialmente |
| net9.0          | ✅          | ✅        | Compila vía compatibilidad hacia adelante — usado en muestras del taller |
| net10.0         | ✅          | ✅        | Compila y corre vía compatibilidad hacia adelante con runtime .NET 10.0.3 |

El ensamblado net8.0 se carga en runtimes más nuevos a través del mecanismo de compatibilidad hacia adelante de .NET, por lo que la compilación tiene éxito. Sin embargo, esto no está documentado ni probado por el equipo del SDK.

### Por qué las Muestras Apuntan a net9.0

1. **.NET 9 es la última versión estable** — la mayoría de los participantes del taller la tendrán instalada  
2. **La compatibilidad hacia adelante funciona** — el ensamblado net8.0 en el paquete NuGet se ejecuta en el runtime .NET 9 sin problemas  
3. **.NET 10 (preview/RC)** es demasiado nuevo para apuntar en un taller que debe funcionar para todos  

**Esperado:** Las futuras versiones del SDK deberían considerar añadir los TFM `net9.0` y `net10.0` junto con `net8.0` para igualar el patrón usado en `Microsoft.Agents.AI.OpenAI` y proveer soporte validado para runtimes más nuevos.

---

## 6. Streaming de JavaScript ChatClient Usa Callbacks, No Iteradores Async

**Estado:** Abierto  
**Severidad:** Falta en documentación  
**Componente:** JavaScript `foundry-local-sdk` v0.9.x — `ChatClient.completeStreamingChat()`  

El `ChatClient` devuelto por `model.createChatClient()` provee un método `completeStreamingChat()`, pero usa un **patrón de callback** en lugar de devolver un iterable async:

```javascript
// ❌ Esto NO funciona — lanza "stream no es iterable asíncrono"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Patrón correcto — pasar una función de devolución de llamada
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Impacto:** Los desarrolladores familiarizados con el patrón de iteración async del SDK OpenAI (`for await`) encontrarán errores confusos. El callback debe ser una función válida o el SDK lanza "Callback must be a valid function."

**Esperado:** Documentar el patrón de callback en la referencia del SDK. Alternativamente, soportar el patrón iterable async para mantener consistencia con el SDK OpenAI.

---

## Detalles del Entorno

| Componente                 | Versión         |
|---------------------------|-----------------|
| SO                        | Windows 11 ARM64|
| Hardware                  | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI         | 0.8.117         |
| Foundry Local SDK (C#)    | 0.9.0           |
| Microsoft.Agents.AI.OpenAI| 1.0.0-rc3       |
| OpenAI C# SDK             | 2.9.0           |
| .NET SDK                  | 9.0.312, 10.0.104|
| foundry-local-sdk (Python)| 0.5.x           |
| foundry-local-sdk (JS)    | 0.9.x           |
| Node.js                   | 18+             |
| ONNX Runtime              | 1.18+           |