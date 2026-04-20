![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 12: Construyendo una interfaz web para el Zava Creative Writer

> **Objetivo:** Añadir una interfaz basada en navegador al Zava Creative Writer para que puedas ver la ejecución de la tubería de múltiples agentes en tiempo real, con insignias de estado de agentes en vivo y texto de artículo transmitido, todo servido desde un único servidor web local.

En [Parte 7](../../../labs/part7-zava-creative-writer.md exploraste el Zava Creative Writer como una **aplicación CLI** (JavaScript, C) y una **API sin interfaz gráfica** (Python). En este laboratorio conectarás una interfaz compartida de **HTML/CSS/JavaScript puro** a cada backend para que los usuarios puedan interactuar con la tubería a través de un navegador en lugar de una terminal.

---

## Lo que aprenderás

| Objetivo | Descripción |
|-----------|-------------|
| Servir archivos estáticos desde un backend | Montar un directorio HTML/CSS/JS junto a tu ruta API |
| Consumir NDJSON en streaming en el navegador | Usar la API Fetch con `ReadableStream` para leer JSON delimitado por saltos de línea |
| Protocolo unificado de streaming | Asegurar que los backend Python, JavaScript y C# emitan el mismo formato de mensajes |
| Actualizaciones progresivas de UI | Actualizar las insignias de estado de agentes y transmitir el texto del artículo token por token |
| Añadir una capa HTTP a una aplicación CLI | Envolver la lógica existente del orquestador en un servidor estilo Express (JS) o API mínima de ASP.NET Core (C#) |

---

## Arquitectura

La interfaz es un conjunto único de archivos estáticos (`index.html`, `style.css`, `app.js`) compartidos por los tres backends. Cada backend expone las mismas dos rutas:

![Arquitectura UI Zava — interfaz compartida con tres backends](../../../images/part12-architecture.svg)

| Ruta | Método | Propósito |
|-------|--------|---------|
| `/` | GET | Sirve la interfaz estática |
| `/api/article` | POST | Ejecuta la tubería de múltiples agentes y transmite NDJSON |

El front end envía un cuerpo JSON y lee la respuesta como un flujo de mensajes JSON delimitados por saltos de línea. Cada mensaje tiene un campo `type` que la UI usa para actualizar el panel correcto:

| Tipo de mensaje | Significado |
|-------------|---------|
| `message` | Actualización de estado (p. ej. "Iniciando tarea del agente investigador...") |
| `researcher` | Resultados de la investigación están listos |
| `marketing` | Resultados de búsqueda de productos están listos |
| `writer` | El escritor comenzó o terminó (contiene `{ start: true }` o `{ complete: true }`) |
| `partial` | Un único token transmitido desde el Escritor (contiene `{ text: "..." }`) |
| `editor` | El veredicto del editor está listo |
| `error` | Algo salió mal |

![Enrutamiento por tipo de mensaje en el navegador](../../../images/part12-message-types.svg)

![Secuencia de streaming — Comunicación navegador a backend](../../../images/part12-streaming-sequence.svg)

---

## Requisitos previos

- Completar la [Parte 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Tener instalado Foundry Local CLI y el modelo `phi-3.5-mini` descargado
- Un navegador web moderno (Chrome, Edge, Firefox o Safari)

---

## La interfaz compartida

Antes de tocar cualquier código del backend, tómate un momento para explorar el front end que usarán las tres implementaciones. Los archivos están en `zava-creative-writer-local/ui/`:

| Archivo | Propósito |
|------|---------|
| `index.html` | Maquetación de página: formulario de entrada, insignias de estado de agentes, área de salida de artículo, paneles desplegables de detalles |
| `style.css` | Estilos mínimos con estados de color para las insignias (esperando, en ejecución, listo, error) |
| `app.js` | Lógica para Fetch, lector de líneas con `ReadableStream` y actualización del DOM |

> **Consejo:** Abre `index.html` directamente en tu navegador para previsualizar la maquetación. Nada funcionará todavía porque no hay backend, pero puedes ver la estructura.

### Cómo funciona el lector de stream

La función clave en `app.js` lee el cuerpo de la respuesta por fragmentos y divide en los límites de salto de línea:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // mantener la línea final incompleta

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

Cada mensaje analizado se envía a `handleMessage()`, que actualiza el elemento DOM relevante según `msg.type`.

---

## Ejercicios

### Ejercicio 1: Ejecutar el backend Python con la UI

La variante Python (FastAPI) ya tiene un endpoint API para streaming. El único cambio es montar la carpeta `ui/` como archivos estáticos.

**1.1** Navega al directorio de la API Python e instala dependencias:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** Inicia el servidor:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** Abre el navegador en `http://localhost:8000`. Deberías ver la interfaz Zava Creative Writer con tres campos de texto y un botón "Generate Article".

**1.4** Haz clic en **Generate Article** usando los valores predeterminados. Observa cómo las insignias de estado de los agentes cambian de "Waiting" a "Running" a "Done" a medida que cada agente completa su trabajo, y cómo el texto del artículo se transmite token por token al panel de salida.

> **Solución de problemas:** Si la página muestra una respuesta JSON en lugar de la UI, asegúrate de estar ejecutando el `main.py` actualizado que monta los archivos estáticos. El endpoint `/api/article` sigue funcionando en la ruta original; el montaje de archivos estáticos sirve la UI en todas las demás rutas.

**Cómo funciona:** El `main.py` actualizado añade una línea al final:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

Esto sirve cada archivo desde `zava-creative-writer-local/ui/` como un recurso estático, con `index.html` como documento predeterminado. La ruta POST `/api/article` se registra antes del montaje estático, por lo que tiene prioridad.

---

### Ejercicio 2: Añadir un servidor web a la variante JavaScript

La variante JavaScript es actualmente una aplicación CLI (`main.mjs`). Un nuevo archivo, `server.mjs`, envuelve los mismos módulos de agentes detrás de un servidor HTTP y sirve la UI compartida.

**2.1** Navega al directorio JavaScript e instala dependencias:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** Inicia el servidor web:

```bash
node server.mjs
```

```powershell
node server.mjs
```

Deberías ver:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** Abre `http://localhost:3000` en tu navegador y haz clic en **Generate Article**. La misma UI funciona idéntica contra el backend JavaScript.

**Estudia el código:** Abre `server.mjs` y nota los patrones clave:

- El **servicio de archivos estáticos** usa módulos nativos de Node.js `http`, `fs` y `path` sin necesidad de framework externo.
- La **protección contra traversal de ruta** normaliza la ruta solicitada y verifica que se mantenga dentro del directorio `ui/`.
- El **streaming NDJSON** utiliza una función auxiliar `sendLine()` que serializa cada objeto, elimina saltos de línea internos y añade un salto de línea final.
- La **orquestación de agentes** reutiliza los módulos existentes `researcher.mjs`, `product.mjs`, `writer.mjs` y `editor.mjs` sin cambios.

<details>
<summary>Extracto clave de server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// Investigador
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// Escritor (transmisión)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### Ejercicio 3: Añadir una API mínima a la variante C#

La variante C# es actualmente una aplicación de consola. Un nuevo proyecto, `csharp-web`, usa APIs mínimas de ASP.NET Core para exponer la misma tubería como un servicio web.

**3.1** Navega al proyecto web C# y restaura los paquetes:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** Ejecuta el servidor web:

```bash
dotnet run
```

```powershell
dotnet run
```

Deberías ver:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** Abre `http://localhost:5000` en tu navegador y haz clic en **Generate Article**.

**Estudia el código:** Abre `Program.cs` en el directorio `csharp-web` y observa:

- El archivo del proyecto usa `Microsoft.NET.Sdk.Web` en lugar de `Microsoft.NET.Sdk`, lo que añade soporte para ASP.NET Core.
- Los archivos estáticos se sirven mediante `UseDefaultFiles` y `UseStaticFiles` apuntando al directorio compartido `ui/`.
- El endpoint `/api/article` escribe las líneas NDJSON directamente en `HttpContext.Response` y hace flush después de cada línea para streaming en tiempo real.
- Toda la lógica de agentes (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) es igual que la versión de consola.

<details>
<summary>Extracto clave de csharp-web/Program.cs</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### Ejercicio 4: Explora las insignias de estado de los agentes

Ahora que tienes una UI funcionando, mira cómo el front end actualiza las insignias de estado.

**4.1** Abre `zava-creative-writer-local/ui/app.js` en tu editor.

**4.2** Busca la función `handleMessage()`. Observa cómo mapea tipos de mensajes a actualizaciones del DOM:

| Tipo de mensaje | Acción en la UI |
|-------------|-----------|
| `message` que contiene "researcher" | Cambia la insignia del investigador a "Running" |
| `researcher` | Cambia la insignia del investigador a "Done" y llena el panel de Resultados de investigación |
| `marketing` | Cambia la insignia de búsqueda de productos a "Done" y llena el panel de Coincidencias de producto |
| `writer` con `data.start` | Cambia la insignia del escritor a "Running" y limpia la salida del artículo |
| `partial` | Añade el texto del token a la salida del artículo |
| `writer` con `data.complete` | Cambia la insignia del escritor a "Done" |
| `editor` | Cambia la insignia del editor a "Done" y llena el panel de Retroalimentación del editor |

**4.3** Abre los paneles desplegables "Research Results", "Product Matches" y "Editor Feedback" debajo del artículo para inspeccionar el JSON crudo que produjo cada agente.

---

### Ejercicio 5: Personaliza la UI (extra)

Prueba una o más de estas mejoras:

**5.1 Añade un contador de palabras.** Después de que el Escritor termine, muestra el conteo de palabras del artículo debajo del panel de salida. Puedes calcular esto en `handleMessage` cuando `type === "writer"` y `data.complete` es verdadero:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 Añade un indicador de reintento.** Cuando el Editor solicite una revisión, la tubería se vuelve a ejecutar. Muestra un banner "Revisión 1" o "Revisión 2" en el panel de estado. Escucha un mensaje de tipo `message` que contenga "Revision" y actualiza un nuevo elemento DOM.

**5.3 Modo oscuro.** Añade un botón de alternancia y una clase `.dark` al `<body>`. Sobrescribe los colores de fondo, texto y paneles en `style.css` usando un selector `body.dark`.

---

## Resumen

| Lo que hiciste | Cómo |
|-------------|-----|
| Servir la UI desde el backend Python | Montaste la carpeta `ui/` con `StaticFiles` en FastAPI |
| Añadir un servidor HTTP a la variante JavaScript | Creaste `server.mjs` usando el módulo `http` nativo de Node.js |
| Añadir una API web a la variante C# | Creaste un proyecto nuevo `csharp-web` con APIs mínimas de ASP.NET Core |
| Consumir NDJSON en streaming en el navegador | Usaste `fetch()` con `ReadableStream` y análisis JSON línea por línea |
| Actualizar la UI en tiempo real | Mapeaste tipo de mensajes a actualizaciones del DOM (insignias, texto, paneles desplegables) |

---

## Puntos clave

1. Un **front end estático compartido** puede funcionar con cualquier backend que hable el mismo protocolo de streaming, reforzando el valor del patrón API compatible con OpenAI.
2. El **JSON delimitado por saltos de línea (NDJSON)** es un formato de streaming sencillo que funciona de forma nativa con la API `ReadableStream` del navegador.
3. La **variante Python** necesitó el menor cambio porque ya tenía un endpoint FastAPI; las variantes JavaScript y C# necesitaron una capa HTTP delgada.
4. Mantener la UI como **HTML/CSS/JS puro** evita herramientas de construcción, dependencias de framework y complejidad adicional para los participantes del taller.
5. Los mismos módulos de agentes (Researcher, Product, Writer, Editor) se reutilizan sin modificaciones; sólo cambia la capa de transporte.

---

## Lecturas adicionales

| Recurso | Enlace |
|----------|------|
| MDN: Using Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI Static Files | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core Static Files | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| Especificación NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

Continúa a [Parte 13: Taller completado](part13-workshop-complete.md) para un resumen de todo lo que has construido en este taller.

---
[← Parte 11: Llamada a la herramienta](part11-tool-calling.md) | [Parte 13: Taller completado →](part13-workshop-complete.md)